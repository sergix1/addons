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
    class SyndraCore
    {

        private string tittle,version;
        public Obj_AI_Hero Hero => HeroManager.Player;
        Menu _menu;
        Spells _spells;
        public Menu GetMenu => _menu;
        public Spells GetSpells => _spells;
        Modes _modes;
        GameEvents events;
        public GameEvents Events=>events;
        DrawDamage drawDamage;
        public SyndraCore()
        {
            tittle = "[Syndra]Dark Mage";
            version = "1.0.0.0";
            CustomEvents.Game.OnGameLoad += OnLoad;
        }
        private void OnLoad(EventArgs args)
        {
            if (Hero.ChampionName != "Syndra") return;
            Game.PrintChat("<b><font color =\"#FF33D6\">Dark Mage Loaded!</font></b>");
            _menu = new SyndraMenu("Dark.Mage", this);
            _spells = new Spells();
            drawDamage = new DrawDamage(this);
            _modes = new SyndraModes();
 
            LeagueSharp.Drawing.OnDraw += Ondraw;

        }

        private void Ondraw(EventArgs args)
        {
            var drawQ = _menu.GetMenu.Item("DQ").GetValue<bool>();
            var drawW = _menu.GetMenu.Item("DW").GetValue<bool>();
            var drawE = _menu.GetMenu.Item("DE").GetValue<bool>();
            var drawR = _menu.GetMenu.Item("DR").GetValue<bool>();
            var drawOrb = _menu.GetMenu.Item("DO").GetValue<bool>();
            var drawOrbText = _menu.GetMenu.Item("DST").GetValue<bool>();
            if (ObjectManager.Player.IsDead)
            {
                return;
            }
            if(_spells.getQ.IsReady()&&drawQ)
            Render.Circle.DrawCircle(ObjectManager.Player.Position, _spells.getQ.Range, System.Drawing.Color.DarkCyan, 2);
            if (_spells.getW.IsReady() && drawW)
                Render.Circle.DrawCircle(ObjectManager.Player.Position, _spells.getW.Range, System.Drawing.Color.DarkCyan, 2);
            if (_spells.getE.IsReady() && drawE)
                Render.Circle.DrawCircle(ObjectManager.Player.Position, _spells.getE.Range, System.Drawing.Color.DarkCyan, 2);
            if (_spells.getR.IsReady() && drawR)
                Render.Circle.DrawCircle(ObjectManager.Player.Position, _spells.getR.Range, System.Drawing.Color.DarkCyan, 2);
            if(drawOrb)
            foreach (Vector3 b in _spells.getOrbs.GetOrbs())
            {
                Render.Circle.DrawCircle(b, 50, System.Drawing.Color.DarkRed, 2);
                var wts = Drawing.WorldToScreen(Hero.Position);
                var wtssxt = Drawing.WorldToScreen(b);
                Drawing.DrawLine(wts,wtssxt,2,System.Drawing.Color.DarkRed);
            }
            if (drawOrbText)
            {
                string orbsTotal = "Active Orbs R : " + (_spells.getOrbs.GetOrbs().Count + 4);
                Drawing.DrawText(0, 200, System.Drawing.Color.Yellow, orbsTotal);
            }
           
        }

        private void OnUpdate(EventArgs args)
        {
            _modes.Update(this);
        }
    }
}
