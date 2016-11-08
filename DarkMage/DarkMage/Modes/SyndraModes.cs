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
    class SyndraModes : Modes
    {
        public override void Combo(SyndraCore core)
        {
            var useQ = core.GetMenu.GetMenu.Item("CQ").GetValue<bool>();
            var useW = core.GetMenu.GetMenu.Item("CW").GetValue<bool>();
            var useE = core.GetMenu.GetMenu.Item("CE").GetValue<bool>();
            var useR = core.GetMenu.GetMenu.Item("CR").GetValue<bool>();
            if (useQ)
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
                if (!QE)
                {
                    var gameCursor = Game.CursorPos;
                    core.GetSpells.getQ.Cast(core.Hero.Position.Extend(Game.CursorPos, core.GetSpells.getQ.Range));
                    Utility.DelayAction.Add(500 + Game.Ping, () => core.GetSpells.getE.Cast(gameCursor));
                    QE = true;
                }
            }
            base.Keys(core);
        }
        public override void LastHit(SyndraCore core)
        {


            base.LastHit(core);
        }
        public override void Laneclear(SyndraCore core)
        {
            var useQ = core.GetMenu.GetMenu.Item("LQ").GetValue<bool>();
            var useW = core.GetMenu.GetMenu.Item("LW").GetValue<bool>();
            var miniumMana = core.GetMenu.GetMenu.Item("LM").GetValue<Slider>().Value;
            if (core.Hero.ManaPercent < miniumMana) return;
            if (useQ)
            {
                var minionQ =
    MinionManager.GetMinions(
                    core.Hero.Position,
                     core.GetSpells.getQ.Range,
                     MinionTypes.All,
                     MinionTeam.Enemy,
                     MinionOrderTypes.MaxHealth);
                if (minionQ != null)
                {
                    var QfarmPos = core.GetSpells.getQ.GetCircularFarmLocation(minionQ);
                    if (QfarmPos.Position.IsValid())
                        if (QfarmPos.MinionsHit >= 2)
                        {
                            core.GetSpells.getQ.Cast(QfarmPos.Position);
                        }
                }
            }
            if (useW)
            {
                var minionW =
MinionManager.GetMinions(
    core.Hero.Position,
     core.GetSpells.getW.Range,
     MinionTypes.All,
     MinionTeam.Enemy,
     MinionOrderTypes.MaxHealth);
                if (minionW != null)
                {
                    var WfarmPos = core.GetSpells.getQ.GetCircularFarmLocation(minionW);
                    if (WfarmPos.Position.IsValid())
                    {
                        if (WfarmPos.MinionsHit >= 3)
                        {
                            core.GetSpells.castWToPos(WfarmPos.Position);
                        }
                    }
                }
            }
            base.Laneclear(core);
        }
        public override void Jungleclear(SyndraCore core)
        {
            var useQ = core.GetMenu.GetMenu.Item("JQ").GetValue<bool>();
            var useW = core.GetMenu.GetMenu.Item("JW").GetValue<bool>();
            var useE = core.GetMenu.GetMenu.Item("JE").GetValue<bool>();
            var miniumMana = core.GetMenu.GetMenu.Item("JM").GetValue<Slider>().Value;
            if (core.Hero.ManaPercent < miniumMana) return;
            if (useQ)
            {
                var minionQ =
    MinionManager.GetMinions(
                    core.Hero.Position,
                     core.GetSpells.getQ.Range,
                     MinionTypes.All,
                     MinionTeam.Neutral,
                     MinionOrderTypes.MaxHealth);
                if (minionQ != null)
                {
                    var QfarmPos = core.GetSpells.getQ.GetCircularFarmLocation(minionQ);
                    if (QfarmPos.Position.IsValid())
                        if (QfarmPos.MinionsHit >= 2)
                        {
                            core.GetSpells.getQ.Cast(QfarmPos.Position);
                        }
                }
            }
            if (useW)
            {
                var minionW =
MinionManager.GetMinions(
    core.Hero.Position,
     core.GetSpells.getW.Range,
     MinionTypes.All,
     MinionTeam.Neutral,
     MinionOrderTypes.MaxHealth);
                if (minionW != null)
                {
                    var WfarmPos = core.GetSpells.getQ.GetCircularFarmLocation(minionW);
                    if (WfarmPos.Position.IsValid())
                    {
                        if (WfarmPos.MinionsHit >= 3)
                        {
                            core.GetSpells.castWToPos(WfarmPos.Position);
                        }
                    }
                }
            }
            if (useE)
            {
                var minionE =
MinionManager.GetMinions(
    core.Hero.Position,
     core.GetSpells.getQ.Range,
     MinionTypes.All,
     MinionTeam.Neutral,
     MinionOrderTypes.MaxHealth);
                foreach (Vector3 pos in core.GetSpells.getOrbs.GetOrbs())
                {
                    var result = minionE.Where(x => x.Position.Distance(pos) < 50);
                    if (result != null)
                    {
                        core.GetSpells.getE.Cast(pos);
                    }
                }
                base.Jungleclear(core);
            }
        }
    }
    }

