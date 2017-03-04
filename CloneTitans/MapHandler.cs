using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CloneTitans
{
    class MapHandler
    {
        public int KILLED { get; set; } = 0;
        public int CURRENT_WORLD_LVL { get; set; } = 1;
        public int TO_KILL { get; set; } = 10;

        public MapHandler()
        {

        }
    }
}
