using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacketAnalyzer
{
    class OArchiveFactory
    {
        public enum ArchiveType { ArchiveSQLite, ArchiveNone }
        public static OArchive CreateArchive(OArchiveFactory.ArchiveType type)
        {
            if (OArchiveFactory.ArchiveType.ArchiveSQLite == type)
            {
                return new OArchive();
            }
            return null;
        }
    }
}
