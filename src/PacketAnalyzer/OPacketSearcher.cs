using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace PacketAnalyzer
{
    class OPacketSearcher
    {
        private OCurrentPacketList _list = null;

        public delegate void OnPacketFoundHandler(int nIndex);
        public event OnPacketFoundHandler OnPacketFound = null;

        private bool _ContainsSearchBytes(byte[] searchBytes, string path)
        {
            // Not fast method.
            byte[] fileBytes = File.ReadAllBytes(path);
            if (fileBytes.Length <= 0)
            {
                return false;
            }

            for (int i = 0; i < fileBytes.Length - searchBytes.Length; i++)
            {
                bool match = true;
                for (int k = 0; k < searchBytes.Length; k++)
                {
                    if (fileBytes[i + k] != searchBytes[k])
                    {
                        match = false;
                        break;
                    }
                }
                if (match)
                {
                    int pos = i + searchBytes.Length;
                    return match;
                }
            }
            return false;
        }
        
        public void SetCurrentPacketList(OCurrentPacketList list)
        {
            _list = list;
        }
        public void RegisterEvent(OnPacketFoundHandler handler)
        {
            OnPacketFound += handler;
        }
        public void UnregisterEvent(OnPacketFoundHandler handler)
        {
            OnPacketFound -= handler;
        }
        public void SearchPacket(byte[] searchBytes)
        {
            if (null == _list)
            {
                return;
            }

            for (int i = 0; i < _list.GetCount(); i++)
            {
                OPacket packet = _list.GetAt(i);
                if (true == _ContainsSearchBytes(searchBytes, packet.GetPacketStoredPath()))
                {
                    // Invoke registered event handler
                    if (null != OnPacketFound)
                    {
                        OnPacketFound(i);
                    }
                    break;
                }
            }
        }
    }
}
