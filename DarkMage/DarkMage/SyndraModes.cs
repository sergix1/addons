using LeagueSharp;
using LeagueSharp.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarkMage
{
    class SyndraModes :Modes
    {
        public override void Combo(SyndraCore core)
        {
            var useQ = core.GetMenu.GetMenu.Item("CQ").GetValue<bool>();
            var useW = core.GetMenu.GetMenu.Item("CW").GetValue<bool>();
            var useE = core.GetMenu.GetMenu.Item("CE").GetValue<bool>();
            var useR = core.GetMenu.GetMenu.Item("CR").GetValue<bool>();
            if(useQ)
            core.GetSpells.CastQ();
            if (useW)
                core.GetSpells.castW();
            if (useE)
                core.GetSpells.castE();
            if (useR)
                core.GetSpells.castR(core);
            base.Combo(core);
        }
        public override void Harash(SyndraCore core)
        {
            var useQ = core.GetMenu.GetMenu.Item("HQ").GetValue<bool>();
            var useW = core.GetMenu.GetMenu.Item("HW").GetValue<bool>();
            var useE = core.GetMenu.GetMenu.Item("HE").GetValue<bool>();
            if (useQ)
                core.GetSpells.CastQ();
            if (useW)
                core.GetSpells.castW();
            if (useE)
                core.GetSpells.castE();
            base.Harash(core);
        }
        bool QE;
        public override void Keys(SyndraCore core)
        {
            if (core.GetSpells.getQ.IsReady() && core.GetSpells.getE.IsReady())
            {
                QE = false;
            }
            if (core.GetMenu.GetMenu.Item("QEkey").GetValue<KeyBind>().Active)
            {
                if(!QE)
                {
                    var gameCursor = Game.CursorPos;
                    core.GetSpells.getQ.Cast(core.Hero.Position.Extend(Game.CursorPos,core.GetSpells.getQ.Range));
                    Utility.DelayAction.Add(500+Game.Ping, ()=>core.GetSpells.getE.Cast(gameCursor));
                    QE = true;
                }
            }
                base.Keys(core);
        }
    }
}
