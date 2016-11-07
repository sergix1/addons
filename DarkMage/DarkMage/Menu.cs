using LeagueSharp.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarkMage
{
    class Menu
    {
        public LeagueSharp.Common.Menu GetMenu { get; private set; }
        private string _menuName;
        public Orbwalking.Orbwalker Orb { get; private set; }
        LeagueSharp.Common.Menu _orbWalkerMenu, _targetSelectorMenu;
        public Menu(string menuName, SyndraCore core)
        {
            Console.WriteLine("HI");
            this._menuName = menuName;
            LoadMenu(core);
            CloseMenu();
        }
        public virtual void LoadMenu(SyndraCore azir)
        {
            GetMenu = new LeagueSharp.Common.Menu(_menuName, _menuName, true);
            _orbWalkerMenu = new LeagueSharp.Common.Menu("Orbwalker", "Orbwalker");
            Orb = new Orbwalking.Orbwalker(_orbWalkerMenu);
            _targetSelectorMenu = new LeagueSharp.Common.Menu("TargetSelector", "TargetSelector");
        // LoadLaneClearMenu();
          LoadHarashMenu();
            LoadComboMenu();
         //   LoadJungleClearMenu();
            LoadDrawings();
            LoadkeyMenu();
          //  LoadMiscInterrupt(azir);
          //  LoadMiscMenu(azir);

        }

        private void LoadMiscMenu(SyndraCore azir)
        {
            throw new NotImplementedException();
        }

        private void LoadMiscInterrupt(SyndraCore azir)
        {
            throw new NotImplementedException();
        }

        public virtual void LoadkeyMenu()
        {
 
        }

       public virtual void LoadComboMenu()
        {
         
        }

      public virtual void LoadDrawings()
        {
        }

        private void LoadJungleClearMenu()
        {
            throw new NotImplementedException();
        }

        public virtual void LoadHarashMenu()
        {
  
        }

        private void LoadLaneClearMenu()
        {
            throw new NotImplementedException();
        }

        public virtual void CloseMenu()
        {
            TargetSelector.AddToMenu(_targetSelectorMenu);
            GetMenu.AddSubMenu(_orbWalkerMenu);        //ORBWALKER
            GetMenu.AddSubMenu(_targetSelectorMenu);
            GetMenu.AddToMainMenu();

        }
    }
}
