using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Timers;
using System.Text.Json;

namespace SecureNetProto
{
    public partial class MainForm : Form
    {
        const string USERNAME_FILE = "Data\\username.txt";
        const int BROADCAST_PORT = 37000;
        const int FILE_TRANSFER_PORT = 38000;
        const int BROADCAST_INTERVAL = 1000; // milliseconds

        UdpClient udpBroadcaster;
        UdpClient udpListener;
        System.Timers.Timer broadcastTimer;
        HashSet<string> onlinePeers = new HashSet<string>();
        string currentIdentifier = "";
        List<SharedFileInfo> mySharedFiles = new List<SharedFileInfo>();
        Dictionary<string, List<SharedFileInfo>> remoteFiles = new Dictionary<string, List<SharedFileInfo>>();
        private Dictionary<int, (string Owner, string FileName)> contentIndex = new();

        public MainForm()
        {
            InitializeComponent();
            Directory.CreateDirectory("Data");
            Directory.CreateDirectory("Data\\Chunks");
            LoadUsername();

            // Focus username box on load
            this.Shown += (s, e) => {
                if (string.IsNullOrWhiteSpace(txtUsername.Text))
                    txtUsername.Focus();
                else
                    btnShare.Focus();
            };

            // Optional: Alternate row color for ListBox
            lstContent.DrawMode = DrawMode.OwnerDrawFixed;
            lstContent.DrawItem += (s, e) => {
                if (e.Index < 0) return;
                var bg = (e.Index % 2 == 0) ? Color.FromArgb(60, 66, 90) : Color.FromArgb(50, 56, 74);
                e.Graphics.FillRectangle(new SolidBrush(bg), e.Bounds);
                e.Graphics.DrawString(lstContent.Items[e.Index].ToString(),
                    e.Font, Brushes.WhiteSmoke, e.Bounds.Left, e.Bounds.Top + 1);
                e.DrawFocusRectangle();
            };

            // Optional: Tooltip on files
            var tip = new ToolTip();
            lstContent.MouseMove += (s, e) => {
                int idx = lstContent.IndexFromPoint(e.Location);
                if (idx >= 0 && idx < lstContent.Items.Count)
                    tip.SetToolTip(lstContent, lstContent.Items[idx].ToString());
            };
        }


        // --------- Username Storage ---------
        private void LoadUsername()
        {
            if (File.Exists(USERNAME_FILE))
            {
                txtUsername.Text = File.ReadAllText(USERNAME_FILE);
            }
        }

        private void SaveUsername()
        {
            Directory.CreateDirectory("Data");
            File.WriteAllText(USERNAME_FILE, txtUsername.Text.Trim());
        }

        private string GetLocalIdentifier()
        {
            // "username_MachineName"
            return $"{txtUsername.Text.Trim()}_{Environment.MachineName}";
        }

        // --------- Connection Logic ---------
        private void btnConnect_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            if (string.IsNullOrWhiteSpace(username))
            {
                MessageBox.Show("Please enter a username.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            SaveUsername();
            currentIdentifier = GetLocalIdentifier();
            btnConnect.Enabled = false;
            txtUsername.Enabled = false;

            StartBroadcasting();
            StartListening();
            StartFileServer();
            BroadcastMyFiles();

            BroadcastMyFiles(); // Broadcast your shared files at connect
        }

        // --------- Broadcasting Presence & Files ---------
        private void StartBroadcasting()
        {
            udpBroadcaster = new UdpClient();
            broadcastTimer = new System.Timers.Timer(BROADCAST_INTERVAL);
            broadcastTimer.Elapsed += (s, e) =>
            {
                try
                {
                    // Presence
                    string msg = currentIdentifier;
                    byte[] data = Encoding.UTF8.GetBytes(msg);
                    udpBroadcaster.EnableBroadcast = true;
                    udpBroadcaster.Send(data, data.Length, new IPEndPoint(IPAddress.Broadcast, BROADCAST_PORT));

                    // Files (broadcast every 5th tick)
                    if (DateTime.Now.Second % 5 == 0)
                        BroadcastMyFiles();
                }
                catch { /* Ignore errors */ }
            };
            broadcastTimer.Start();
        }

        private void BroadcastMyFiles()
        {
            try
            {
                var payload = JsonSerializer.Serialize(mySharedFiles);
                string msg = $"files:{GetLocalIdentifier()}:{payload}";
                byte[] data = Encoding.UTF8.GetBytes(msg);
                udpBroadcaster.Send(data, data.Length, new IPEndPoint(IPAddress.Broadcast, BROADCAST_PORT));
            }
            catch { /* Ignore errors */ }
        }

        // --------- Listening for Peers & Files ---------
        private void StartListening()
        {
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

                        // Is it a file broadcast?
                        if (message.StartsWith("files:"))
                        {
                            // Parse: files:sender_id:json_payload
                            var parts = message.Split(':', 3);
                            if (parts.Length == 3)
                            {
                                string sender = parts[1];
                                if (sender == currentIdentifier) continue; // Don't process own files
                                var files = JsonSerializer.Deserialize<List<SharedFileInfo>>(parts[2]);
                                UpdateRemoteFiles(sender, files);
                            }
                        }
                        else
                        {
                            // Peer presence
                            string id = message;
                            if (id == currentIdentifier)
                                continue;

                            lock (onlinePeers)
                            {
                                if (onlinePeers.Add(id))
                                {
                                    UpdateOnlineCount();
                                }
                            }
                        }
                    }
                    catch { /* Ignore errors */ }
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
            // Show your files
            foreach (var f in mySharedFiles)
            {
                lstContent.Items.Add($"{f.FileName} ({f.FileSize / 1024} KB) | {f.TotalChunks} chunks | By: {f.Owner} (You)");
                contentIndex[idx++] = (GetLocalIdentifier(), f.FileName);
            }
            // Show remote files
            foreach (var remote in remoteFiles)
            {
                foreach (var f in remote.Value)
                {
                    lstContent.Items.Add($"{f.FileName} ({f.FileSize / 1024} KB) | {f.TotalChunks} chunks | By: {f.Owner}");
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

        // --------- File Sharing ---------
        private void btnShare_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    string filePath = ofd.FileName;
                    string fileName = Path.GetFileName(filePath);
                    long fileSize = new FileInfo(filePath).Length;

                    // Chunk file into 256KB pieces
                    byte[] allBytes = File.ReadAllBytes(filePath);
                    int chunkSize = 256 * 1024;
                    int totalChunks = (int)Math.Ceiling((double)allBytes.Length / chunkSize);

                    for (int i = 0; i < totalChunks; i++)
                    {
                        string chunkFile = $"Data\\Chunks\\{fileName}_chunk{i}";
                        int actualSize = Math.Min(chunkSize, allBytes.Length - i * chunkSize);
                        File.WriteAllBytes(chunkFile, allBytes.Skip(i * chunkSize).Take(actualSize).ToArray());
                    }

                    mySharedFiles.Add(new SharedFileInfo
                    {
                        Owner = txtUsername.Text,
                        FileName = fileName,
                        FileSize = fileSize,
                        TotalChunks = totalChunks
                    });

                    MessageBox.Show($"File chunked into {totalChunks} pieces.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    UpdateContentList();
                    BroadcastMyFiles();
                }
            }
        }

        // --------- Clean up UDP sockets on close ---------
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

        private void StartFileServer()
        {
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
                    catch { break; }
                }
                listener.Stop();
            });
        }

        private void HandleFileRequest(TcpClient client)
        {
            try
            {
                using (var ns = client.GetStream())
                {
                    // Receive the file name
                    byte[] lenBuf = new byte[4];
                    ns.Read(lenBuf, 0, 4);
                    int nameLen = BitConverter.ToInt32(lenBuf, 0);
                    byte[] nameBuf = new byte[nameLen];
                    ns.Read(nameBuf, 0, nameLen);
                    string reqFile = Encoding.UTF8.GetString(nameBuf);

                    // Send all chunks (in chunk order)
                    int chunkNum = 0;
                    while (true)
                    {
                        string chunkFile = $"Data\\Chunks\\{reqFile}_chunk{chunkNum}";
                        if (!File.Exists(chunkFile))
                            break;

                        byte[] chunk = File.ReadAllBytes(chunkFile);
                        ns.Write(BitConverter.GetBytes(chunk.Length), 0, 4); // send chunk size
                        ns.Write(chunk, 0, chunk.Length);
                        chunkNum++;
                    }
                    // Send zero to signal end
                    ns.Write(BitConverter.GetBytes(0), 0, 4);
                }
            }
            catch { }
            finally { client.Close(); }
        }

        private void lstContent_DoubleClick(object sender, EventArgs e)
        {
            if (lstContent.SelectedIndex == -1)
                return;

            var (owner, fileName) = contentIndex[lstContent.SelectedIndex];

            // If it's your own file, do nothing
            if (owner == GetLocalIdentifier())
                return;

            // Find the IP of the owner (hack: extract from UDP peer list)
            string? ownerIP = null;
            lock (onlinePeers)
            {
                foreach (var peer in onlinePeers)
                {
                    if (peer == owner || peer.StartsWith(owner))
                    {
                        // peer is like "username_MachineName"
                        // Try to resolve host to IP
                        var host = peer.Split('_').Last();
                        try
                        {
                            var entry = Dns.GetHostEntry(host);
                            ownerIP = entry.AddressList.FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork)?.ToString();
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

            // Download file!
            Task.Run(() =>
            {
                try
                {
                    using (var client = new TcpClient())
                    {
                        client.Connect(ownerIP, FILE_TRANSFER_PORT);
                        using (var ns = client.GetStream())
                        {
                            byte[] nameBytes = Encoding.UTF8.GetBytes(fileName);
                            ns.Write(BitConverter.GetBytes(nameBytes.Length), 0, 4);
                            ns.Write(nameBytes, 0, nameBytes.Length);

                            // Receive chunks and assemble
                            Directory.CreateDirectory("Downloads");
                            using (var fs = new FileStream($"Downloads\\{fileName}", FileMode.Create, FileAccess.Write))
                            {
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
                                        if (r <= 0) break;
                                        received += r;
                                    }
                                    fs.Write(chunk, 0, chunkLen);
                                }
                            }
                        }
                    }
                    MessageBox.Show($"Downloaded to Downloads\\{fileName}!", "Download Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch
                {
                    MessageBox.Show("Download failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            });
        }

    }
}