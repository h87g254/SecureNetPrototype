using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace SecureNetProto
{
    public partial class MainForm : Form
    {
        // ───────────── Fields & Constants ─────────────

        private string startupUsername = "";
        private CancellationTokenSource connectingCts;

        private const string USERNAME_FILE = "Data\\username.txt";
        private const int BROADCAST_PORT = 37000;
        private const int FILE_TRANSFER_PORT = 38000;
        private const int BROADCAST_INTERVAL = 1000; // milliseconds

        private UdpClient udpBroadcaster;
        private UdpClient udpListener;
        private System.Timers.Timer broadcastTimer;

        private HashSet<string> onlinePeers = new HashSet<string>();
        private string currentIdentifier = "";

        private List<SharedFileInfo> mySharedFiles = new List<SharedFileInfo>();
        private Dictionary<string, List<SharedFileInfo>> remoteFiles = new Dictionary<string, List<SharedFileInfo>>();
        private Dictionary<int, (string Owner, string FileName)> contentIndex = new();

        // ─── “Main Peer” Flag ───
        private bool isMainPeer = false;

        // ───────────── Constructor ─────────────

        public MainForm()
        {
            InitializeComponent();
            Directory.CreateDirectory("Data");
            Directory.CreateDirectory("Data\\Chunks");
            LoadUsername();

            // Focus logic on first Show
            this.Shown += (s, e) =>
            {
                if (panelStartup.Visible)
                {
                    if (string.IsNullOrWhiteSpace(txtStartupUsername.Text))
                        txtStartupUsername.Focus();
                    else
                        btnStartupConnect.Focus();
                }
                else if (panelMain.Visible)
                {
                    btnShare.Focus();
                }
            };

            // Alternate‐row coloring for ListBox
            lstContent.DrawMode = DrawMode.OwnerDrawFixed;
            lstContent.DrawItem += (s, e) =>
            {
                if (e.Index < 0) return;
                var bg = (e.Index % 2 == 0)
                    ? Color.FromArgb(60, 66, 90)
                    : Color.FromArgb(50, 56, 74);
                e.Graphics.FillRectangle(new SolidBrush(bg), e.Bounds);
                e.Graphics.DrawString(
                    lstContent.Items[e.Index].ToString(),
                    e.Font,
                    Brushes.WhiteSmoke,
                    e.Bounds.Left,
                    e.Bounds.Top + 1
                );
                e.DrawFocusRectangle();
            };

            // Tooltip on files
            var tip = new ToolTip();
            lstContent.MouseMove += (s, e) =>
            {
                int idx = lstContent.IndexFromPoint(e.Location);
                if (idx >= 0 && idx < lstContent.Items.Count)
                    tip.SetToolTip(lstContent, lstContent.Items[idx].ToString());
            };

            // Panels at start
            panelStartup.Visible = true;
            panelConnecting.Visible = false;
            panelMain.Visible = false;

            // AcceptButton initially = btnStartupConnect
            this.AcceptButton = btnStartupConnect;

            // Startup events
            txtStartupUsername.TextChanged += txtStartupUsername_TextChanged;
            btnStartupConnect.Click += btnStartupConnect_Click;
        }

        // ───────────── Username / Startup Logic ─────────────

        private void txtStartupUsername_TextChanged(object sender, EventArgs e)
        {
            var valid = !string.IsNullOrWhiteSpace(txtStartupUsername.Text)
                        && txtStartupUsername.Text.Trim().Length >= 3;
            btnStartupConnect.Enabled = valid;
        }

        private async void btnStartupConnect_Click(object sender, EventArgs e)
        {
            // 1) Read & store the username
            startupUsername = txtStartupUsername.Text.Trim();

            // 2) Flip to the “Connecting…” UI
            panelStartup.Visible = false;
            panelConnecting.Visible = true;
            panelMain.Visible = false;
            txtConnectingLog.Clear();
            lblConnecting.Text = "Connecting…";

            // 3) Initialize the CTS here
            connectingCts = new CancellationTokenSource();
            isMainPeer = false;  // reset flag on each connect attempt

            try
            {
                bool success = await ConnectFlow(connectingCts.Token);

                if (success)
                {
                    // Only if ConnectFlow returned true do we move to the Main panel
                    panelConnecting.Visible = false;
                    panelMain.Visible = true;
                    InitializeMainPanelAfterConnect();
                }
                else
                {
                    // ConnectFlow only returns false on unrecoverable error
                    txtConnectingLog.AppendText(Environment.NewLine +
                        "⛔ Connection failed. Returning to startup." + Environment.NewLine);
                    await Task.Delay(2000);
                    panelConnecting.Visible = false;
                    panelStartup.Visible = true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[CONNECT ERROR] {ex}");
                txtConnectingLog.AppendText(Environment.NewLine
                    + $"⛔ Exception: {ex.GetType().Name}: {ex.Message}{Environment.NewLine}"
                    + ex.StackTrace);
                txtConnectingLog.ForeColor = Color.LightPink;
                lblConnecting.Text = "Connection Failed";

                MessageBox.Show(ex.ToString(), "ConnectFlow Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);

                await Task.Delay(2500);
                panelConnecting.Visible = false;
                panelStartup.Visible = true;
            }
        }

        /// <summary>
        /// Runs the LAN‐only discovery. 
        /// If at least one peer is found, returns true. 
        /// If none are found, sets isMainPeer = true and still returns true.
        /// Only returns false on unrecoverable error.
        /// </summary>
        private async Task<bool> ConnectFlow(CancellationToken ct)
        {
            Action<string> log = msg => Invoke(new Action(() =>
            {
                txtConnectingLog.AppendText(msg + Environment.NewLine);
            }));

            // 1) Validate username
            try
            {
                log("Validating username...");
                await Task.Delay(400, ct);
                if (startupUsername.Length < 3)
                    throw new Exception("Username too short.");
            }
            catch (Exception ex)
            {
                log($"[ERROR] {ex.Message}");
                return false;
            }

            // 2) Save username to disk
            try
            {
                log("Saving username...");
                await Task.Delay(350, ct);
                Directory.CreateDirectory("Data");
                File.WriteAllText(USERNAME_FILE, startupUsername);
            }
            catch (Exception ex)
            {
                log($"[ERROR] Failed to write username file: {ex.Message}");
                return false;
            }

            // 3) Initialize network (placeholder)
            try
            {
                log("Initializing network...");
                await Task.Delay(500, ct);
            }
            catch (Exception ex)
            {
                log($"[ERROR] Network init failed: {ex.Message}");
                return false;
            }

            // ── Attempt LAN discovery only ──
            try
            {
                log("Broadcasting presence (LAN)...");
                StartBroadcasting();
                StartListening();
                await Task.Delay(2000, ct);  // wait 2 seconds to see if any LAN peers respond
            }
            catch (Exception ex)
            {
                log($"[ERROR] LAN broadcast error: {ex.Message}");
            }

            // If any peer showed up on LAN, proceed as normal
            if (onlinePeers.Count > 0)
            {
                log($"Found {onlinePeers.Count} peer(s) on the same LAN.");
                log("Ready!");
                return true;
            }

            // No LAN peers → become “main peer”
            isMainPeer = true;
            log("No LAN peers found. You are now the MAIN PEER.");
            log("Ready!");
            return true;
        }

        // ───────────── Initialize Main Panel ─────────────

        private void InitializeMainPanelAfterConnect()
        {
            lblAppTitle.Text = "SecureNet 🕸️";
            lblSubtitle.Text = isMainPeer
                ? "Role: MAIN PEER — waiting for others to join."
                : "Connect. Share. Stay private. Prototype Edition.";
            lblUsername.Text = $"Username: {startupUsername}";
            lblUsersOnline.Text = $"Users online: {onlinePeers.Count + 1}";

            panelMain.Visible = true;
            this.AcceptButton = btnShare;

            // Keep broadcasting/listening in case new peers join
            StartBroadcasting();
            StartListening();
            StartFileServer();
            BroadcastMyFiles();
        }

        // ───────────── Username Storage ─────────────

        private void LoadUsername()
        {
            if (File.Exists(USERNAME_FILE))
            {
                startupUsername = File.ReadAllText(USERNAME_FILE).Trim();
                txtStartupUsername.Text = startupUsername;
                if (startupUsername.Length >= 3)
                    btnStartupConnect.Enabled = true;
            }
        }

        private string GetLocalIdentifier()
        {
            return $"{startupUsername}_{Environment.MachineName}";
        }

        // ───────────── Broadcasting Presence & Files ─────────────

        private void StartBroadcasting()
        {
            if (udpBroadcaster != null)
                return;

            udpBroadcaster = new UdpClient();
            broadcastTimer = new System.Timers.Timer(BROADCAST_INTERVAL);
            broadcastTimer.Elapsed += BroadcastTimer_Elapsed;
            broadcastTimer.Start();
        }

        private void BroadcastTimer_Elapsed(object? sender, ElapsedEventArgs e)
        {
            if (udpBroadcaster == null)
                return;

            try
            {
                string msg = GetLocalIdentifier();
                byte[] data = Encoding.UTF8.GetBytes(msg);
                udpBroadcaster.EnableBroadcast = true;
                udpBroadcaster.Send(data, data.Length, new IPEndPoint(IPAddress.Broadcast, BROADCAST_PORT));

                if (DateTime.Now.Second % 5 == 0)
                    BroadcastMyFiles();
            }
            catch
            {
                // ignore
            }
        }

        private void BroadcastMyFiles()
        {
            if (udpBroadcaster == null)
                return;

            try
            {
                var payload = JsonSerializer.Serialize(mySharedFiles);
                string msg = $"files:{GetLocalIdentifier()}:{payload}";
                byte[] data = Encoding.UTF8.GetBytes(msg);
                udpBroadcaster.Send(data, data.Length, new IPEndPoint(IPAddress.Broadcast, BROADCAST_PORT));
            }
            catch { }
        }

        // ───────────── Listening for Peers & Files ─────────────

        private void StartListening()
        {
            if (udpListener != null)
                return;

            udpListener = new UdpClient(BROADCAST_PORT);
            udpListener.EnableBroadcast = true;

            Task.Run(() =>
            {
                while (true)
                {
                    try
                    {
                        IPEndPoint ep = new IPEndPoint(IPAddress.Any, BROADCAST_PORT);
                        var data = udpListener.Receive(ref ep);
                        string message = Encoding.UTF8.GetString(data);

                        if (message.StartsWith("files:"))
                        {
                            // ── “files:” broadcast ──
                            var parts = message.Split(':', 3);
                            if (parts.Length == 3)
                            {
                                string sender = parts[1];
                                if (sender == GetLocalIdentifier())
                                    continue;

                                // 1) Add sender to onlinePeers (if not already present)
                                lock (onlinePeers)
                                {
                                    if (onlinePeers.Add(sender))
                                        UpdateOnlineCount();
                                }

                                // 2) Update remoteFiles dictionary and UI
                                var files = JsonSerializer.Deserialize<List<SharedFileInfo>>(parts[2]);
                                UpdateRemoteFiles(sender, files);
                            }
                        }
                        else
                        {
                            // ── plain identifier broadcast ──
                            string id = message;
                            if (id == GetLocalIdentifier())
                                continue;

                            lock (onlinePeers)
                            {
                                if (onlinePeers.Add(id))
                                    UpdateOnlineCount();
                            }
                        }
                    }
                    catch
                    {
                        // ignore any socket errors
                    }
                }
            });
        }

        private void UpdateRemoteFiles(string sender, List<SharedFileInfo> files)
        {
            lock (remoteFiles)
            {
                remoteFiles[sender] = files ?? new List<SharedFileInfo>();
                UpdateContentList();
            }
        }

        private void UpdateContentList()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(UpdateContentList));
                return;
            }

            lstContent.Items.Clear();
            contentIndex.Clear();
            int idx = 0;

            foreach (var f in mySharedFiles)
            {
                lstContent.Items.Add(
                    $"{f.FileName} ({f.FileSize / 1024} KB) | {f.TotalChunks} chunks | By: {f.Owner} (You)"
                );
                contentIndex[idx++] = (GetLocalIdentifier(), f.FileName);
            }

            foreach (var remote in remoteFiles)
            {
                foreach (var f in remote.Value)
                {
                    lstContent.Items.Add(
                        $"{f.FileName} ({f.FileSize / 1024} KB) | {f.TotalChunks} chunks | By: {f.Owner}"
                    );
                    contentIndex[idx++] = (remote.Key, f.FileName);
                }
            }
        }

        private void UpdateOnlineCount()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(UpdateOnlineCount));
                return;
            }

            lblUsersOnline.Text = $"Users online: {onlinePeers.Count + 1}";
        }

        // ───────────── File Sharing ─────────────

        private void btnShare_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    string filePath = ofd.FileName;
                    string fileName = Path.GetFileName(filePath);
                    long fileSize = new FileInfo(filePath).Length;

                    byte[] allBytes = File.ReadAllBytes(filePath);
                    int chunkSize = 256 * 1024;
                    int totalChunks = (int)Math.Ceiling((double)allBytes.Length / chunkSize);

                    for (int i = 0; i < totalChunks; i++)
                    {
                        string chunkFile = $"Data\\Chunks\\{fileName}_chunk{i}";
                        int actualSize = Math.Min(chunkSize, allBytes.Length - i * chunkSize);
                        File.WriteAllBytes(
                            chunkFile,
                            allBytes.Skip(i * chunkSize).Take(actualSize).ToArray()
                        );
                    }

                    mySharedFiles.Add(new SharedFileInfo
                    {
                        Owner = startupUsername,
                        FileName = fileName,
                        FileSize = fileSize,
                        TotalChunks = totalChunks
                    });

                    MessageBox.Show(
                        $"File chunked into {totalChunks} pieces.",
                        "Success",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );

                    UpdateContentList();
                    BroadcastMyFiles();
                }
            }
        }

        // ───────────── Clean up on Form Closing ─────────────

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            try
            {
                broadcastTimer?.Stop();
                udpBroadcaster?.Dispose();
                udpListener?.Dispose();
            }
            catch { }
            base.OnFormClosing(e);
        }

        private bool fileServerStarted = false;

        private void StartFileServer()
        {
            if (fileServerStarted)
                return;

            fileServerStarted = true;
            Task.Run(() =>
            {
                TcpListener listener = new TcpListener(IPAddress.Any, FILE_TRANSFER_PORT);
                listener.Start();
                while (true)
                {
                    try
                    {
                        var client = listener.AcceptTcpClient();
                        Task.Run(() => HandleFileRequest(client));
                    }
                    catch
                    {
                        break;
                    }
                }
                listener.Stop();
            });
        }

        private void HandleFileRequest(TcpClient client)
        {
            try
            {
                using var ns = client.GetStream();

                byte[] lenBuf = new byte[4];
                ns.Read(lenBuf, 0, 4);
                int nameLen = BitConverter.ToInt32(lenBuf, 0);
                byte[] nameBuf = new byte[nameLen];
                ns.Read(nameBuf, 0, nameLen);
                string reqFile = Encoding.UTF8.GetString(nameBuf);

                int chunkNum = 0;
                while (true)
                {
                    string chunkFile = $"Data\\Chunks\\{reqFile}_chunk{chunkNum}";
                    if (!File.Exists(chunkFile))
                        break;

                    byte[] chunk = File.ReadAllBytes(chunkFile);
                    ns.Write(BitConverter.GetBytes(chunk.Length), 0, 4);
                    ns.Write(chunk, 0, chunk.Length);
                    chunkNum++;
                }

                ns.Write(BitConverter.GetBytes(0), 0, 4);
            }
            catch { }
            finally
            {
                client.Close();
            }
        }

        private void lstContent_DoubleClick(object sender, EventArgs e)
        {
            if (lstContent.SelectedIndex == -1)
                return;

            var (owner, fileName) = contentIndex[lstContent.SelectedIndex];

            if (owner == GetLocalIdentifier())
                return;

            string? ownerIP = null;
            lock (onlinePeers)
            {
                foreach (var peer in onlinePeers)
                {
                    if (peer == owner || peer.StartsWith(owner))
                    {
                        var host = peer.Split('_').Last();
                        try
                        {
                            var entry = Dns.GetHostEntry(host);
                            ownerIP = entry.AddressList
                                .FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork)
                                ?.ToString();
                            if (ownerIP != null)
                                break;
                        }
                        catch { }
                    }
                }
            }

            if (ownerIP == null)
            {
                MessageBox.Show("Peer not found or offline.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Task.Run(() =>
            {
                try
                {
                    using var client = new TcpClient();
                    client.Connect(ownerIP, FILE_TRANSFER_PORT);
                    using var ns = client.GetStream();

                    byte[] nameBytes = Encoding.UTF8.GetBytes(fileName);
                    ns.Write(BitConverter.GetBytes(nameBytes.Length), 0, 4);
                    ns.Write(nameBytes, 0, nameBytes.Length);

                    Directory.CreateDirectory("Downloads");
                    string outputPath = Path.Combine("Downloads", fileName);

                    using var fs = new FileStream(outputPath, FileMode.Create, FileAccess.Write);
                    while (true)
                    {
                        byte[] lenBuf = new byte[4];
                        int read = ns.Read(lenBuf, 0, 4);
                        if (read < 4)
                            break;

                        int chunkLen = BitConverter.ToInt32(lenBuf, 0);
                        if (chunkLen == 0)
                            break;

                        byte[] chunk = new byte[chunkLen];
                        int received = 0;
                        while (received < chunkLen)
                        {
                            int r = ns.Read(chunk, received, chunkLen - received);
                            if (r <= 0)
                                break;
                            received += r;
                        }
                        fs.Write(chunk, 0, chunkLen);
                    }
                }
                catch
                {
                    Invoke(new Action(() =>
                    {
                        MessageBox.Show("Download failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }));
                    return;
                }

                Invoke(new Action(() =>
                {
                    MessageBox.Show($"Downloaded to Downloads\\{fileName}!", "Download Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }));
            });
        }
    }
}
