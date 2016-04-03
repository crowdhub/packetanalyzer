using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacketAnalyzer
{
    public class OStatisticsPacketPerProtocol : OStatistics
    {
        protected override string _GetNameForPacket(OPacket packet)
        {
            string res = ProtocolInfo.instanceOf().ConvertToString(packet.GetProtocol());
            return res;
        }
    }
}
