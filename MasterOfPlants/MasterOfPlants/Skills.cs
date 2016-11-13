using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;

namespace MasterOfThorns
{
    class Skills
    {
        private Spell Q, W, E, R, passive;
        private SpellSlot ignite;

        public Skills()
        {
            Q = new Spell(SpellSlot.Q, 800); // circle
            W = new Spell(SpellSlot.W, 850);  // circle
            E = new Spell(SpellSlot.E, 1100); // line
            R = new Spell(SpellSlot.R, 700); // circle
            passive = new Spell(SpellSlot.Q, 1470);
            Q.SetSkillshot(Q.Instance.SData.SpellCastTime, Q.Instance.SData.LineWidth, Q.Instance.SData.MissileSpeed, false, SkillshotType.SkillshotCircle);
            W.SetSkillshot(W.Instance.SData.SpellCastTime, W.Instance.SData.LineWidth, W.Instance.SData.MissileSpeed,false, SkillshotType.SkillshotCircle);
            E.SetSkillshot(E.Instance.SData.SpellCastTime, E.Instance.SData.LineWidth, E.Instance.SData.MissileSpeed, false, SkillshotType.SkillshotLine);
            E.SetSkillshot(R.Instance.SData.SpellCastTime, R.Instance.SData.LineWidth, R.Instance.SData.MissileSpeed, false, SkillshotType.SkillshotCircle);
            passive.SetSkillshot(passive.Instance.SData.SpellCastTime, passive.Instance.SData.LineWidth, passive.Instance.SData.MissileSpeed, false, SkillshotType.SkillshotLine);
            ignite = ObjectManager.Player.GetSpellSlot("SummonerDot");
        }

       public Spell getQ()
        {
            return Q;
        }
       public Spell getpassive()
       {
           return passive;
       }
       public Spell getW()
       {
           return W;
       }
       public Spell getE()
       {
           return E;
       }
       public Spell getR()
       {
           return R;
       }
        public SebbyLib.Prediction.HitChance hitchanceCheckOKTW(int i)
        {
            switch (i)
            {
                case 1:
                    return SebbyLib.Prediction.HitChance.Low;
                case 2:
                    return SebbyLib.Prediction.HitChance.Medium;
                case 3:
                    return SebbyLib.Prediction.HitChance.High;
                case 4:
                    return SebbyLib.Prediction.HitChance.VeryHigh;
            }
            return SebbyLib.Prediction.HitChance.Low;
        }
        public HitChance hitchanceCheck(int i)
       {
           switch (i)
           {
               case 1:
                   return HitChance.Low;
               case 2:
                   return HitChance.Medium;
               case 3:
                   return HitChance.High;
               case 4:
                   return HitChance.VeryHigh;
           }
           return HitChance.Low;
       }

       public bool qCast(Obj_AI_Base target, int hitChance,Program program)
       {
           if (target == null) return false;
           if (Q.IsReady() && Q.IsInRange(target))
           {
               program.cast(target,Q,hitChance);
               return true;
           }
           return false;

       }

       public bool wCast(Obj_AI_Base target, int hitChance,Program program )
       {
           
           if (target == null) return false;
           HitChance hit = hitchanceCheck(hitChance);
           if (W.IsReady() && W.IsInRange(target))
           {
                program.cast(target ,W,hitChance);
               return true;
               /*
               if (ObjectManager.Player.Spellbook.GetSpell(SpellSlot.W).Ammo.ToString() == "2")
               {
                   Game.PrintChat("Dentro de 2 wCast");
                   W.CastIfHitchanceEquals(target, HitChance.High);
                   Utility.DelayAction.Add(400, () => W.CastIfHitchanceEquals(target, HitChance.High));
                   return true;
               }
               else if (ObjectManager.Player.Spellbook.GetSpell(SpellSlot.W).Ammo.ToString() == "1")
               {
                   Game.PrintChat("Dentro de 1 wCast");
                   W.CastIfHitchanceEquals(target, HitChance.High);
                   return true;
               }
             * */
           }
           return false;
       }

       public bool eCast(Obj_AI_Base target, int hitChance,Program program)
       {
             if (target == null) return false;
             if (E.IsReady() && E.IsInRange(target))
           {  
                program.cast(target,E,hitChance);          
               return true;
           }
           return false;
        }

       public bool rCast(Obj_AI_Base target, int hitChance,Program program)
       {
           if (target == null) return false;
           if (R.IsReady() && R.IsInRange(target))
           {
           program.cast(target,R,hitChance);
               return true;
           }
           return false;
       }

       public bool rCastHit(Obj_AI_Base target , int min)
       {
           if (target == null) return false;
           if (R.IsReady() && R.IsInRange(target))
           {
               R.CastIfWillHit(target, min);
           }
           return false;
       }

       public bool igniteCast(Obj_AI_Base target)
       {
           if (target == null) return false;
            if (ignite.IsReady() && target.Health - ObjectManager.Player.GetSummonerSpellDamage(target, Damage.SummonerSpell.Ignite) <= 0)
            {
                ObjectManager.Player.Spellbook.CastSpell(ignite, target);
                return true;
            }
            return false;
        }
    }
}
