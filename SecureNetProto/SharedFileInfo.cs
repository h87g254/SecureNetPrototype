using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureNetProto
{
    public class SharedFileInfo
    {
        public string Owner { get; set; }
        public string FileName { get; set; }
        public long FileSize { get; set; }
        public int TotalChunks { get; set; }
    }

}
