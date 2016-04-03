using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacketAnalyzer
{
    public abstract class OStatistics
    {
        public enum StatisticsType { PacketPerSecond, TrafficPerProtocol, Uknown };

        // Initialize code if needed
        protected virtual void _Initialize(OCurrentPacketList list) { }
        protected abstract string _GetNameForPacket(OPacket packet);
        
        public OStatisticsResult CalculateStatistics(OCurrentPacketList list, OStatistics.StatisticsType type)
        {
            OStatisticsResult res = new OStatisticsResult();

            _Initialize(list);
            // for all packets
            int nPacketCount = list.GetCount();
            for (int i=0; i<nPacketCount; i++)
            {
                OPacket packet = list.GetAt(i);
                string n = _GetNameForPacket(packet);
                res.IncrementResult(n);
            }

            return res;
        }
    }
}
