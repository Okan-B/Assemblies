using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LeagueSharp;
using LeagueSharp.Common;
using Color = System.Drawing.Color;

namespace ayyGangplank
{
    class Program
    {
        public static Obj_AI_Base Player { get { return ObjectManager.Player; } }
        public static string ChampName = "Gangplank";
        public static Orbwalking.Orbwalker Orbwalker;
        public static Spell Q, W, E, R;
        public static Items.Item DFG;
        public static Menu menu;

        static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += Game_OnGameLoad;

        }

        private static void Game_OnGameLoad(EventArgs args)
        {
            if (Player.BaseSkinName != ChampName)
                return;

            Q = new Spell(SpellSlot.Q, 625);
            W = new Spell(SpellSlot.W);
            E = new Spell(SpellSlot.E, 1300);


            menu = new Menu("ayy" + ChampName, "ChampName", true);


            menu.AddSubMenu(new Menu("Orbwalker", "Orbwalker"));
            Orbwalker = new Orbwalking.Orbwalker(menu.SubMenu("Orbwalker"));

            var ts = menu.AddSubMenu(new Menu("Target Selector", "Target Selector")); ;
            SimpleTs.AddToMenu(ts);

            menu.AddSubMenu(new Menu("Combo", "Combo"));
            menu.SubMenu("Combo").AddItem(new MenuItem("useQ", "Use Q?").SetValue(true));
            menu.SubMenu("Combo").AddItem(new MenuItem("useW", "Use W?").SetValue(true));
            menu.SubMenu("Combo").AddItem(new MenuItem("useE", "Use E?").SetValue(true));
            menu.SubMenu("Combo").AddItem(new MenuItem("ComboActive", "Combo").SetValue(new KeyBind(32, KeyBindType.Press)));

            menu.AddItem(new MenuItem("NFE", "No-Face Exploit").SetValue(true));
            menu.AddToMainMenu();

            Drawing.OnDraw += Drawing_OnDraw;
            Game.OnGameUpdate += Game_OnGameUpdate;

            Game.PrintChat(ChampName + " loaded! By ayySilver");
            Game.PrintChat("Welcome to my first ever assembly, ayyGangplank");
        }

        private static void Game_OnGameUpdate(EventArgs args)
        {
            if (menu.Item("ComboActive").GetValue<KeyBind>().Active)
            {
                Combo();
            }
            if (W.IsReady() && menu.Item("useW").GetValue<bool>() &&
                (Player.HasBuffOfType(BuffType.Stun) || Player.HasBuffOfType(BuffType.Fear) ||
                 Player.HasBuffOfType(BuffType.Slow) || Player.HasBuffOfType(BuffType.Snare) ||
                 Player.HasBuffOfType(BuffType.Taunt)))
            {
                W.CastOnUnit(Player);
            }
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            if (!Player.IsDead)
            {
                if (Q.IsReady())
                {
                    Utility.DrawCircle(Player.Position, Q.Range, Color.Aqua);
                }
                else
                {
                    Utility.DrawCircle(Player.Position, Q.Range, Color.DarkRed);
                }
            }
        }

        private static void Combo()
        {
            var target = SimpleTs.GetTarget(E.Range, SimpleTs.DamageType.Physical);

            if (target.IsValidTarget(Q.Range) && Q.IsReady() && menu.Item("useQ").GetValue<bool>())
            {

                Q.CastOnUnit(target, menu.Item("NFE").GetValue<bool>());
            }

            if (W.IsReady() && menu.Item("useW").GetValue<bool>() &&
                (Player.HasBuffOfType(BuffType.Stun) || Player.HasBuffOfType(BuffType.Fear) ||
                 Player.HasBuffOfType(BuffType.Slow) || Player.HasBuffOfType(BuffType.Snare) ||
                 Player.HasBuffOfType(BuffType.Taunt)))
            {
                W.CastOnUnit(Player);
            }
            if (W.IsReady())
            {
                float healthPercent = ObjectManager.Player.Health / ObjectManager.Player.MaxHealth * 100;

                if (healthPercent < 10)
                {

                }
            }
            if (target.IsValidTarget(E.Range) && E.IsReady() && menu.Item("useE").GetValue<bool>())
            {
                E.CastOnUnit(Player);
            }
            {

            }

        }
    }
}