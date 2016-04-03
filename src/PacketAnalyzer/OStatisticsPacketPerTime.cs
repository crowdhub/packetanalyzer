using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacketAnalyzer
{
    public class OStatisticsPacketPerTime : OStatistics
    {
        private ulong _basetime = 0;
        protected override void _Initialize(OCurrentPacketList list)
        {
            // Find fastest time from the packet
            int nCount = list.GetCount();
            for (int i = 0; i < nCount; i++)
            {
                OPacket packet = list.GetAt(i);
                if (0 == _basetime || _basetime > packet.GetCollectedTime())
                {
                    _basetime = packet.GetCollectedTime();
                }
            }
        }
        protected override string _GetNameForPacket(OPacket packet)
        {
            string res;
            res = (packet.GetCollectedTime() - _basetime).ToString();
            return res;
        }
    }
}
