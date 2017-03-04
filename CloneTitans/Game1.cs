using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace CloneTitans
{

    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch sb;
        Random random = new Random();
        //Enemy enemy;
        MapHandler poziom = new MapHandler();
        enum State
        {
            GAME,
            SHOP
        }
        State state = State.GAME;

        public float points;
        public float damage = 1;
        public float points_on_sec;
        public float damage_over_time = 0;



        List<Upgrade> upgradeList = new List<Upgrade>();
        List<Enemy> enemyList = new List<Enemy>();

        int do_zabicia = 10;
        int hp_ = 13;
        int _hp = 15;





        KeyboardState kb;
        KeyboardState pkb;

        MouseState ms;
        MouseState pms;

        Rectangle cursor;
        Rectangle cursorX;
        Rectangle enemyRect;
        Rectangle bg;

        Texture2D bgT;
        Texture2D cursorT;
        Texture2D enemyT;
        Texture2D healthT;

        SpriteFont font, fontS;





        public bool LPM()
        {
            return (ms.LeftButton == ButtonState.Pressed && pms.LeftButton == ButtonState.Released) ? true : false;
        }
        public bool Klik(Keys k)
        {
            return (kb.IsKeyDown(k) && pkb.IsKeyUp(k)) ? true : false;
        }



        private void EnemyUpdate()
        {
            foreach(Enemy x in enemyList.ToList())
            {
                if (x.HP <= 0.999999999)
                {
                    OnKill();

                }
                if (enemyRect.Intersects(cursor) && LPM())
                {
                    OnHit();
                }
                x.HP -= damage_over_time / 30 / 2;

            };

        }
        private void AddEnemy()
        {

            string[] nazwy =
            {
                "Blob",
                "Inny syf",
                "Cwel"
            };
            string[] nazwyBossow =
            {
                "Glutang",
                "Ape",
                "Kozak"
            };

            if (poziom.CURRENT_WORLD_LVL % 5 == 0)
            {
                int los = random.Next(0, nazwyBossow.Length);
                _hp += 31;
                hp_ += 28;
                float hape = random.Next(hp_ + 40, _hp + 40);

                int lewel = poziom.CURRENT_WORLD_LVL;

                Enemy e = new Enemy(nazwyBossow[los], hape, hape, 40, 150, lewel, 0);
                e.HP *= e.LVL + 0.2f;
                e.MAXHP *= e.LVL + 0.2f;
                e.DEFENCE *= e.LVL + 0.5f;
                e.GOLD_MIN *= e.LVL + 0.2f;
                e.GOLD_MAX *= e.LVL + 0.2f;
                enemyList.Add(e);

                poziom.TO_KILL = 0;
            }
            else
            {


                int los = random.Next(0, nazwy.Length);

                float hape = random.Next(11, 14);

                int lewel = poziom.CURRENT_WORLD_LVL;

                if (lewel > 5)
                    hape = random.Next(hp_, _hp);

                var k = random.Next(1, 11);
                if (k == 3)
                {
                    hp_++;
                    _hp++;
                }

                Enemy e = new Enemy(nazwy[los], hape, hape, 2, 7, lewel, 0);
                e.HP *= e.LVL + 0.7f;
                e.MAXHP *= e.LVL + 0.7f;
                e.DEFENCE *= e.LVL + 0.5f;
                e.GOLD_MIN *= e.LVL;
                e.GOLD_MAX *= e.LVL;
                enemyList.Add(e);

                poziom.TO_KILL = do_zabicia;
            }

        }
        private void OnKill()
        {
            foreach(Enemy x in enemyList.ToList())
            {
                var gmin = x.GOLD_MIN * x.LVL / 2;
                var gmax = x.GOLD_MAX * x.LVL / 2;
                
                // Math.floor(Math.random() * (max - min + 1)) + min; //
                var srednia = Math.Floor(random.NextDouble() * (gmax - gmin + 1)) + gmin;
                points += (float)srednia;
                enemyList.Remove(x);
            };


            poziom.KILLED++;

            CheckLvl();
            AddEnemy();
        }
        private void CheckLvl()
        {
            if (poziom.KILLED > poziom.TO_KILL)
            {
                poziom.CURRENT_WORLD_LVL++;
                poziom.KILLED = 0;
            }
        }
        private void OnHit()
        {
            foreach(Enemy x in enemyList.ToList())
            {
                if (damage <= x.HP)
                    x.HP -= damage;
                else
                    OnKill();

            };
        }


        private void AddUpgrades()
        {
            Upgrade[] up = {
                new Upgrade(new Rectangle(bg.X + 130,bg.Y + 100,160,32),
                            10,     //CENA
                            0.9f,   //DAMAGE
                            0.2f),  //DAMAGE_OVER_TIME
                new Upgrade(new Rectangle(bg.X + 130,bg.Y + 150,160,32),
                            500,    //CENA
                            4.4f,      //DAMAGE
                            1),     //DAMAGE_OVER_TIME
                new Upgrade(new Rectangle(bg.X + 130,bg.Y + 200,160,32),
                            3140,   //CENA
                            8,     //DAMAGE
                            9),     //DAMAGE_OVER_TIME
            };

            upgradeList.AddRange(up);
        }
        private void UpgradeCheck()
        {
            foreach (var x in upgradeList)
                if (cursor.Intersects(x.RECT) && LPM())
                {
                    if (points >= x.COST)
                        BuyUpgrade();
                }
        }
        private void BuyUpgrade()
        {
            foreach (var x in upgradeList)
            {
                if (cursor.Intersects(x.RECT) && LPM())
                {
                    points -= x.COST;
                    x.COST *= x.COST_MULT;
                    damage += x.DAMAGE;
                    x.COUNT++;
                    damage_over_time += x.DAMAGE_OVER_TIME;
                    if (x.COUNT == 25)
                        x.DAMAGE *= 5;
                }

            }
        }


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {


            base.Initialize();
        }


        protected override void LoadContent()
        {
            //enemyList = enemy.EnemyList;
            AddEnemy();
            AddUpgrades();

            sb = new SpriteBatch(GraphicsDevice);

            cursorX = new Rectangle(1, 1, 16, 16);
            cursor = new Rectangle(1, 1, 8, 8);
            enemyRect = new Rectangle(300, 200, 128, 128);


            cursorT = Content.Load<Texture2D>("bullet");
            enemyT = Content.Load<Texture2D>("player");

            bg = new Rectangle(0, 50, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight - 50);
            bgT = Content.Load<Texture2D>("bg");

            healthT = Content.Load<Texture2D>("health");

            font = Content.Load<SpriteFont>("font");
            fontS = Content.Load<SpriteFont>("fontS");
        }


        protected override void UnloadContent()
        {

        }




        protected override void Update(GameTime gameTime)
        {

            ms = Mouse.GetState();
            kb = Keyboard.GetState();
            cursorX.X = ms.X - 8;
            cursorX.Y = ms.Y - 8;
            cursor.X = cursorX.X + 4;
            cursor.Y = cursorX.Y + 4;




            if (Klik(Keys.Space))
                state = State.SHOP;
            if (Klik(Keys.A))
                points += 200;
            if (Klik(Keys.D))
                OnKill();

            switch (state)
            {
                case State.GAME:
                    EnemyUpdate();

                    //points += points_on_sec / 30 / 2;
                    break;


                case State.SHOP:
                    if (Klik(Keys.Escape))
                        state = State.GAME;

                    upgradeList.ForEach(x =>
                    {
                        if (cursor.Intersects(x.RECT) && LPM())
                        {
                            UpgradeCheck();
                        }
                    });
                    break;
            }




            pms = ms;
            pkb = kb;

            base.Update(gameTime);
        }

        private static string FormatNumber(float num)
        {
            // Ensure number has max 3 significant digits (no rounding up can happen)
            float i = (float)Math.Pow(10, (int)Math.Max(0, Math.Log10(num) - 2));
            num = num / i * i;

            if (num >= 1000000000000000000)
                return (num / 1000000000000000000D).ToString("0.##") + " E";
            if (num >= 1000000000000000)
                return (num / 1000000000000000D).ToString("0.##") + " D";
            if (num >= 1000000000000)
                return (num / 1000000000000D).ToString("0.##") + " C";
            if (num >= 1000000000)
                return (num / 1000000000D).ToString("0.##") + " B";
            if (num >= 1000000)
                return (num / 1000000D).ToString("0.##") + " M";
            if (num >= 1000)
                return (num / 1000D).ToString("0.##") + " K";

            return num.ToString("#,0.#");
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);


            sb.Begin();

            // sb.Draw(cursorT, cursor, Color.White);




            switch (state)
            {
                case State.GAME:
                    sb.Draw(enemyT, enemyRect, Color.White);


                    foreach (var x in enemyList)
                    {
                        float szerokosc = healthT.Width * x.HPPERCENT;
                        sb.Draw(healthT, new Rectangle(enemyRect.X, enemyRect.Y - 33, 128, 20), Color.Gray);
                        Rectangle healthBarR = new Rectangle(enemyRect.X, enemyRect.Y - 33, (int)szerokosc, 20);
                        sb.Draw(healthT, healthBarR, Color.IndianRed);


                        sb.DrawString(font, "       " + x.NAME + "\nHP " + FormatNumber((long)x.HP) + "/" + FormatNumber((long)x.MAXHP), new Vector2(300, 150), Color.Black);
                    }
                    break;

                case State.SHOP:
                    sb.Draw(bgT, bg, Color.White);
                    foreach (var x in upgradeList)
                    {
                        sb.Draw(enemyT, x.RECT, Color.White);
                        sb.DrawString(fontS, string.Format("  Cost   {0} \n  Count {1}", FormatNumber(x.COST), FormatNumber(x.COUNT)), new Vector2(x.RECT.X, x.RECT.Y + 4), Color.Black);
                        if (cursor.Intersects(x.RECT))
                        {
                            if (x.DAMAGE_OVER_TIME <= 0)
                                sb.DrawString(fontS, string.Format("Gives {0} damage", FormatNumber(x.DAMAGE)), new Vector2(cursor.X + 12, cursor.Y - 8), Color.Black);
                            else
                                sb.DrawString(fontS, string.Format("Gives {0} damage\nGives {1} damage over time", FormatNumber(x.DAMAGE), FormatNumber(x.DAMAGE_OVER_TIME)), new Vector2(cursor.X + 12, cursor.Y - 8), Color.Black);
                        }
                    }
                    break; 
            }





            sb.DrawString(font, string.Format("Points {0}\nDamage {1}\nDamage over time {2}\n{3}/{4}", FormatNumber(points), FormatNumber(damage), FormatNumber(damage_over_time), hp_, _hp), Vector2.Zero, Color.Black);
            sb.DrawString(font, string.Format("Current world {0}\nKilled {1}/{2}", poziom.CURRENT_WORLD_LVL, poziom.KILLED, poziom.TO_KILL), new Vector2(300, 0), Color.Black);
            sb.Draw(cursorT, cursorX, Color.White);
            sb.End();





            base.Draw(gameTime);
        }
    }
}
