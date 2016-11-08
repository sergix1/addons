﻿using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarkMage
{
    public class Spells
    {
        private Spell Q, W, E, R;
        public Spell getQ { get { return Q; } }
        public Spell getW { get { return W; } }
        public Spell getE { get { return E; } }
        public Spell getR { get { return R; } }
        OrbManager orbManager;
        public OrbManager getOrbs { get { return orbManager; } }
        public Spells()
        {
            orbManager = new OrbManager();
            Q = new Spell(LeagueSharp.SpellSlot.Q,800);
            W = new Spell(LeagueSharp.SpellSlot.W, 925);
            E = new Spell(SpellSlot.E, 700);
            R = new Spell(SpellSlot.R, 675);
            Q.SetSkillshot(0.6f, 125f, float.MaxValue, false, SkillshotType.SkillshotCircle);
            W.SetSkillshot(0.25f, 140f, 1600f, false, SkillshotType.SkillshotCircle);
            E.SetSkillshot(0.25f, (float)(45 * 0.5), 2500f, false, SkillshotType.SkillshotCone);
        }
        public bool CastQ()
        {
            if (Q.IsReady())
                {
                var qTarget = TargetSelector.GetTarget(Q.Range, TargetSelector.DamageType.Magical);
                return Q.CastIfWillHit(qTarget, 0,false);
            }
            return false;
        }
        public bool castW()
        {
            if (W.IsReady())
            {
                var wTarget = TargetSelector.GetTarget(Q.Range, TargetSelector.DamageType.Magical);
                if (wTarget != null)
                    if (HeroManager.Player.Spellbook.GetSpell(SpellSlot.W).ToggleState == 1 && W.IsReady())
                    {
                        var orb = orbManager.GetOrbToGrab((int)W.Range);
                        if (orb != null)
                            W.Cast(orb);
                    }
                    else if (HeroManager.Player.Spellbook.GetSpell(SpellSlot.W).ToggleState != 1 && W.IsReady())
                    {
                        if (getOrbs.WObject(false) != null)
                        {
                            W.From = getOrbs.WObject(false).ServerPosition;
                            W.CastIfWillHit(wTarget, 0, false);
                            return true;
                        }
                    }
            }
                return false;
            
        }
               public bool castWToPos(Vector2 pos)
        {
            if (W.IsReady())
            {
                    if (HeroManager.Player.Spellbook.GetSpell(SpellSlot.W).ToggleState == 1 && W.IsReady())
                    {
                        var orb = orbManager.GetOrbToGrab((int)W.Range);
                        if (orb != null)
                            W.Cast(orb);
                    }
                    else if (HeroManager.Player.Spellbook.GetSpell(SpellSlot.W).ToggleState != 1 && W.IsReady())
                    {
                        if (getOrbs.WObject(false) != null)
                        {
                            W.From = getOrbs.WObject(false).ServerPosition;
                            W.Cast(pos);
                            return true;
                        }
                    }
            }
            return false;
        }
      
        public bool castE()
        {
            if (E.IsReady())
            {
                foreach (Vector3 orb in getOrbs.GetOrbs())
                {
                    foreach (Obj_AI_Base tar in HeroManager.Enemies)
                    {
                        if (orb.Distance(tar.Position) <= 50)
                        {
                            if (tar.Distance(HeroManager.Player.Position) <= E.Range)
                            {
                                E.Cast(orb);
                            }
                        }
                    }
                }
            }
            return false;
           
        }
        public bool castR(SyndraCore core)
        {
            if (R.IsReady())
            {
                var rTarget = TargetSelector.GetTarget(R.Range, TargetSelector.DamageType.Magical);
                if (rTarget != null)
                {
                    
                    if(castRCheck(rTarget,core))
                    if (NotKilleableWithOtherSpells(rTarget))
                    {
                            float totalDamageR = RDamage(rTarget);
                        if (rTarget.Health <= totalDamageR)
                        {
                            R.Cast(rTarget);
                        }
                    }
                }
            }
            return false;
        }
        public float RDamage(Obj_AI_Hero target)
        {
            float damagePerBall = (R.GetDamage(target) / 3);
            float totalDamageR = R.GetDamage(target) + damagePerBall * getOrbs.GetOrbs().Count;
            return totalDamageR;
        }
        public bool castRCheck(Obj_AI_Hero target,SyndraCore core)
        {
            var checkZhoniaMenu =  core.GetMenu.GetMenu.Item("DONTRZHONYA").GetValue<bool>();
            if(checkZhoniaMenu)
            {
                //Zhonias lol
                var zhonyaName = "ZhonyasHourglass";
                SpellSlot slot;
                for (int i =1;i<=6;i++)
                {
                    slot = core.Events.intToSpellSlot(i);
                    if (target.GetSpell(slot).Name == zhonyaName)
                    {
                        Console.WriteLine("hEY PEPE");
                        if (target.GetSpell(slot).IsReady()) return false;
                    }
                }
            }
            if (target.IsInvulnerable)
            {
                return false;
            }
            // if(core.Events.listChampions!=null)
            Console.WriteLine("Hey buddy");
            foreach (Champion tar in core.events.listChampions)
            {
              var checkFirst = core.GetMenu.GetMenu.Item(tar.Name+"-"+core.Events.SpellSlotToString(tar.SpellSlot)).GetValue<bool>();
                if (checkFirst)
                if (target.ChampionName == tar.Name)
                {
                    if(core.GetMenu.GetMenu.Item(target.ChampionName).GetValue<bool>())
                    return tar.CastRToDat();
                }
            }
            return  core.GetMenu.GetMenu.Item(target.ChampionName).GetValue<bool>();
     
        }
        private bool NotKilleableWithOtherSpells(Obj_AI_Hero target)
        {
            if (Q.IsReady() && Q.IsKillable(target))
            {
                CastQ();
                return false;
            }
            if (W.IsReady() && W.IsKillable(target))
            {
                castW();
                return false;
            }
            if (E.IsReady() && E.IsKillable(target))
            {
                castE();
                return false;
            }
            return true;
        }
    }
}
