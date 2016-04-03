using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacketAnalyzer
{
    public class OStatisticsResult
    {
        private Dictionary<string, int> _statisticsResult = new Dictionary<string, int>();
        
        public void IncrementResult(string name)
        {
            // Check if key is exist or not
            if (true == _statisticsResult.ContainsKey(name))
            {
                _statisticsResult[name] += 1;
                return;
            }
            // At this time, we don't have such key, and thus add it
            _statisticsResult.Add(name, 1);
        }
        public int GetItemCount()
        {
            return _statisticsResult.Count;
        }
        public Dictionary<string, int>.Enumerator GetEnumerator()
        {
            return _statisticsResult.GetEnumerator();
        }
    }
}
