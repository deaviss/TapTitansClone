using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CloneTitans
{
    class Upgrade
    {
        public Rectangle RECT { get; set; }
        public float COST { get; set; } = 0;
        public float DAMAGE { get; set; } = 0;
        public int COUNT { get; set; }
        public float COST_MULT { get; set; } = 1.14f;
        public float DAMAGE_OVER_TIME { get; set; } = 0;

        public Upgrade(Rectangle rect,
                       float cost = 0,
                       float damage=0,
                       float damage_over_time=0,
                       int count = 0)
        {
            this.RECT = rect;
            this.COST = cost;
            this.DAMAGE = damage;
            this.COUNT = count;
            this.DAMAGE_OVER_TIME = damage_over_time;
        }
    }
}
