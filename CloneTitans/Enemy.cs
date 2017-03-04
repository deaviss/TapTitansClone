using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CloneTitans
{
    class Enemy
    {

        public string NAME { get; set; } = "Blob";
        public float HP { get; set; } = 0;
        public float MAXHP { get; set; } = 0;
        public float GOLD_MIN { get; set; } = 0;
        public float GOLD_MAX { get; set; } = 0;
        public int LVL { get; set; } = 0;
        public float DEFENCE { get; set; } = 0;
        public float HP_PERCENT { get; set; } = 0;
        public float HPPERCENT { get { return HP / MAXHP; } }


        Random random = new Random();




        public Enemy(string name = "bob",
                    float hp = 0,
                    float maxhp = 0,
                    float gold_min = 0,
                    float gold_max = 0,
                    int lvl = 0,
                    float defence = 0)
        {
            this.NAME = name;
            this.HP = hp;
            this.GOLD_MIN = gold_min;
            this.GOLD_MAX = gold_max;
            this.LVL = lvl;
            this.MAXHP = maxhp;
            this.DEFENCE = defence;
        }


    }
}
