using LeagueSharp;
using LeagueSharp.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarkMage
{
   public class Champion
    {
        private SpellSlot spellslot;
        private String name;
        public String Name => name;
        public SpellSlot SpellSlot => spellslot;
        public Obj_AI_Hero hero;
        bool InvunerableSpellReady;
        public bool CastRToDat()
        {
            return !InvunerableSpellReady;
        }
        public Champion(SpellSlot spell,String name)
        {
            this.spellslot = spell;
            this.name = name;
            Game.OnUpdate += OnUpdate;
            loadHero();
        }
        public void loadHero()
        {
            foreach(Obj_AI_Hero tar in HeroManager.Enemies)
            {
                if(tar.ChampionName.ToLower()==name.ToLower())
                {
                    hero = tar;
                    break;
                }
            }
        }
        private void OnUpdate(EventArgs args)
        {
          //  Console.WriteLine(spellslot);
          // if(hero.GetSpell(spellslot).IsReady())
           // {
             //   InvunerableSpellReady = true;
           // }
           //else
           // {
                InvunerableSpellReady = false;
           // }
        }
    }
}
