using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacketAnalyzer
{
    // Singleton class which contains configuration
    public class OConfiguration
    {
        // selected NIC device which will be used on packet capturing
        private int _selectedNIC = -1;

        private static OConfiguration _instance = null;
        private OConfiguration() { }
        public static OConfiguration instanceOf()
        {
            if (null == _instance)
            {
                _instance = new OConfiguration();
            }
            return _instance;
        }

        public int GetSelectedNIC()
        {
            return _selectedNIC;
        }

        public void SetSelectedNIC(int selected)
        {
            _selectedNIC = selected;
        }

    }
}
