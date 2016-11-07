using LeagueSharp;
using LeagueSharp.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarkMage
{
    class SyndraMenu : Menu
    {
        LeagueSharp.Common.Menu _comboMenu,_drawingMenu, _harassMenu, _keyMenu,_targetsRMe;
        public SyndraMenu(string menuName, SyndraCore core) : base(menuName, core)
        {
        }
        public override void LoadComboMenu()
        {
            _comboMenu = new LeagueSharp.Common.Menu("Combo", "Combo Menu");
            {
                _comboMenu.AddItem(new MenuItem("CQ", "Use Q").SetValue(true));
                _comboMenu.AddItem(new MenuItem("CW", "Use W").SetValue(true));
                _comboMenu.AddItem(new MenuItem("CE", "Use E").SetValue(true));
                _comboMenu.AddItem(new MenuItem("CR", "Use R").SetValue(true));
            }
        }
        public override void LoadHarashMenu()
        {
            _harassMenu = new LeagueSharp.Common.Menu("Harass", "Harass Menu");
            {
                _harassMenu.AddItem(new MenuItem("HQ", "Use Q").SetValue(true));
                _harassMenu.AddItem(new MenuItem("HW", "Use W").SetValue(false));
                _harassMenu.AddItem(new MenuItem("HE", "Use E").SetValue(false));
            }
        }
        public override void LoadDrawings()
        {
            _drawingMenu = new LeagueSharp.Common.Menu("Drawings", "Draw Menu");
            {
                _drawingMenu.AddItem(new MenuItem("DQ", "Draw Q Range").SetValue(true));
                _drawingMenu.AddItem(new MenuItem("DW", "Draw W Range").SetValue(true));
                _drawingMenu.AddItem(new MenuItem("DE", "Draw E Range").SetValue(true));
                _drawingMenu.AddItem(new MenuItem("DR", "Draw R Range").SetValue(true));
                _drawingMenu.AddItem(new MenuItem("DO", "Draw Orbs").SetValue(true));
                _drawingMenu.AddItem(new MenuItem("DST", "Draw Sphere Text").SetValue(true));
            }
            _targetsRMe = new LeagueSharp.Common.Menu("Targets R" , "Targets R");
            {
                foreach(Obj_AI_Hero hero in HeroManager.Enemies)
                {
                    _targetsRMe.AddItem(new MenuItem(hero.ChampionName, hero.ChampionName).SetValue(true));
                }
            }
        }
        public override void LoadkeyMenu()
        {
            _keyMenu = new LeagueSharp.Common.Menu("Keys", "Keys Menu");
            {
                _keyMenu.AddItem(new MenuItem("QEkey", "Q+E To Mouse Key").SetValue(new KeyBind('T', KeyBindType.Press)));
            }
        }
        public override void CloseMenu()
        {
            GetMenu.AddSubMenu(_comboMenu);
            GetMenu.AddSubMenu(_harassMenu);
            GetMenu.AddSubMenu(_targetsRMe);
            GetMenu.AddSubMenu(_keyMenu);
            GetMenu.AddSubMenu(_drawingMenu);
            base.CloseMenu();
        }
    }
}
