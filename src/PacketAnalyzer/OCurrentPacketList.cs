using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacketAnalyzer
{
    public class OCurrentPacketList
    {
        private List<OPacket> _list = new List<OPacket>();
        public void Clear()
        {
            _list.Clear();
        }
        public void AddPacket(OPacket packet)
        {
            _list.Add(packet);
        }

        public OPacket GetAt(int index)
        {
            if (index < 0 || index >= _list.Count)
            {
                return null;
            }
            return _list[index];
        }

        public int GetCount()
        {
            return _list.Count;
        }

        public void RemoveAt(int index)
        {
            if (index < 0 || index >= GetCount())
            {
                return;
            }
            _list.RemoveAt(index);
        }
    }
}
