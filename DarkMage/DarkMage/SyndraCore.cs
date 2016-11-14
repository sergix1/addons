using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarkMage
{
    public class SyndraCore
    {

        private string _tittle,_version;
        public Obj_AI_Hero Hero => HeroManager.Player;
        public Menu GetMenu { get; private set; }
        public GameEvents Events { get; }
        public Spells GetSpells { get; private set; }
        private Modes _modes;
        public List<Vector3> GetOrbs { get; private set; }

        private DrawDamage drawDamage;
        public List<Champion> championsWithDodgeSpells;
        public SyndraCore()
        {
            _tittle = "[Syndra]Dark Mage";
            _version = "1.0.0.0";
            AntiGapcloser.OnEnemyGapcloser += OnGapcloser;
            Interrupter2.OnInterruptableTarget += OnInterrupt;
            CustomEvents.Game.OnGameLoad += OnLoad;
        }

        private void OnInterrupt(Obj_AI_Hero sender, Interrupter2.InterruptableTargetEventArgs args)
        {
            bool onI = GetMenu.GetMenu.Item("IE").GetValue<bool>();
            if (onI)
            {
                if (sender.IsValidTarget(300)&&GetSpells.GetE.IsInRange(sender))
                {
                    GetSpells.GetE.Cast(sender.Position);
                }
            }
        }

        private void OnGapcloser(ActiveGapcloser gapcloser)
        {
            bool onGap=GetMenu.GetMenu.Item("AE").GetValue<bool>();
            if (onGap)
            {
                if (gapcloser.Sender.IsValidTarget(300))
                {
                    GetSpells.GetE.Cast(gapcloser.Sender);
                }
            }
        }

        private void OnLoad(EventArgs args)
        {
            if (Hero.ChampionName != "Syndra") return;
            Game.PrintChat("<b><font color =\"#FF33D6\">Dark Mage Loaded!</font></b>");
            var events = new GameEvents(this);
            GetMenu = new SyndraMenu("Dark.Mage", this);
            GetSpells = new Spells();
            drawDamage = new DrawDamage(this);
            _modes = new SyndraModes();
            Game.OnUpdate += OnUpdate;
            LeagueSharp.Drawing.OnDraw += Ondraw;

        }

        private void Ondraw(EventArgs args)
        {
            var drawQ = GetMenu.GetMenu.Item("DQ").GetValue<bool>();
            var drawW = GetMenu.GetMenu.Item("DW").GetValue<bool>();
            var drawE = GetMenu.GetMenu.Item("DE").GetValue<bool>();
            var drawQE = GetMenu.GetMenu.Item("DQE").GetValue<bool>();
            var drawR = GetMenu.GetMenu.Item("DR").GetValue<bool>();
            var drawOrb = GetMenu.GetMenu.Item("DO").GetValue<bool>();
            var drawOrbText = GetMenu.GetMenu.Item("DST").GetValue<bool>();
            if (ObjectManager.Player.IsDead)
            {
                return;
            }
            if(GetSpells.GetQ.IsReady()&&drawQ)
            Render.Circle.DrawCircle(ObjectManager.Player.Position, GetSpells.GetQ.Range, System.Drawing.Color.DarkCyan, 2);
            if (GetSpells.GetW.IsReady() && drawW)
                Render.Circle.DrawCircle(ObjectManager.Player.Position, GetSpells.GetW.Range, System.Drawing.Color.DarkCyan, 2);
            if (GetSpells.GetE.IsReady() && drawE)
                Render.Circle.DrawCircle(ObjectManager.Player.Position, GetSpells.GetE.Range, System.Drawing.Color.DarkCyan, 2);
            if (GetSpells.GetR.IsReady() && drawR)
                Render.Circle.DrawCircle(ObjectManager.Player.Position, GetSpells.GetR.Range, System.Drawing.Color.DarkCyan, 2);
            if(drawQE&&GetSpells.GetE.IsReady()&&GetSpells.GetQ.IsReady())
            Render.Circle.DrawCircle(ObjectManager.Player.Position, GetSpells.GetQ.Range+500, System.Drawing.Color.Red, 2);
            var orbs = GetOrbs;
            if (orbs != null)
            {
                if (drawOrb)
                    foreach (var b in orbs)
                    {
                        Render.Circle.DrawCircle(b, 50, System.Drawing.Color.DarkRed, 2);
                        var wts = Drawing.WorldToScreen(Hero.Position);
                        var wtssxt = Drawing.WorldToScreen(b);
                        Drawing.DrawLine(wts, wtssxt, 2, System.Drawing.Color.DarkRed);
                    }
                if (drawOrbText)
                {

                    var orbsTotal = "Active Orbs R : " + (orbs.Count);
                    Drawing.DrawText(0, 200, System.Drawing.Color.Yellow, orbsTotal);
                }
            }

        }

        private void OnUpdate(EventArgs args)
        {
            GetOrbs = GetSpells.GetOrbs.GetOrbs();
            _modes.Update(this);
        }
    }
}
