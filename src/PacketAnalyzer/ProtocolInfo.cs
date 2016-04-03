using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacketAnalyzer
{
    public class ProtocolInfo
    {
        private static ProtocolInfo _instance = null;
        private Dictionary<int, string> _portlist = new Dictionary<int, string>();
        private Dictionary<int, string>.KeyCollection _keys = null;
        private void _Init()
        {
            _portlist.Add(0, "All Protocols");
            _portlist.Add(80, "HTTP");
            _portlist.Add(443, "HTTPS");
            _portlist.Add(20, "FTP-Data");
            _portlist.Add(21, "FTP-Control");
            _portlist.Add(22, "SSH");
            _portlist.Add(23, "TELNET");
            _portlist.Add(25, "SMTP");
            _portlist.Add(110, "POP3");

            _keys = _portlist.Keys;
        }

        public static ProtocolInfo instanceOf()
        {
            if (null == _instance)
            {
                _instance = new ProtocolInfo();
                _instance._Init();
            }
            return _instance;
        }

        public int GetPortCount()
        {
            return _keys.Count;
        }
        public int GetPortAt(int index)
        {
            if (index < 0 || index >= _keys.Count)
            {
                return 0;
            }
            return _keys.ElementAt(index);
        }

        public string ConvertToString(int nPort)
        {
            string res = "";
            try
            {
                res = _portlist[nPort];
            }
            catch
            {
                res = nPort.ToString();
            }
            return res;
        }
        public int ConvertToPort(string protocol)
        {
            int nCount = _keys.Count;
            for (int i=0; i<nCount; i++)
            {
                if (protocol == _portlist[_keys.ElementAt(i)])
                {
                    return _keys.ElementAt(i);
                }
            }
            return 0;
        }
    }
}
