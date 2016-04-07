using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacketAnalyzer
{
    public class OPacket
    {
        private string _packetPath; // file path for stored packet
        private ulong _collectedTime;
        private string _srcIP;
        private string _destIP;
        private int _protocol;
        private int _length;
        private string _info;

        public string GetPacketStoredPath()
        {
            return _packetPath;
        }
        public void SetPacketStoredPath(string path)
        {
            _packetPath = path;
        }
        public ulong GetTimeCollected()
        {
            return _collectedTime;
        }
        public void SetTimeCollected(ulong t)
        {
            _collectedTime = t;
        }
        public string GetSourceIP()
        {
            return _srcIP;
        }
        public void SetSourceIP(string ip)
        {
            _srcIP = ip;
        }
        public string GetDestinationIP()
        {
            return _destIP;
        }
        public void SetDestinationIP(string ip)
        {
            _destIP = ip;
        }
        public int GetProtocol()
        {
            return _protocol;
        }
        public void SetProtocol(int protocol)
        {
            _protocol = protocol;
        }
        public int GetLength()
        {
            return _length;
        }
        public void SetLength(int length)
        {
            _length = length;
        }
        public string GetInfo()
        {
            return _info;
        }
        public void SetInfo(string info)
        {
            _info = info;
        }
    }
}
