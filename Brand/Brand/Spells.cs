using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using LeagueSharp;
using LeagueSharp.Common;


namespace Brand
{
   public class BrandSpells  :Spells
    {
        public override void LoadSpells()
        {
            GetQ = new Spell(SpellSlot.Q, 1000);
            GetW = new Spell(SpellSlot.W, 940);
            GetE = new Spell(SpellSlot.E, 625);
            GetR = new Spell(SpellSlot.R, 750);

            GetQ.SetSkillshot(0.25f, 60f, 1600f, true, SkillshotType.SkillshotLine);
            GetW.SetSkillshot(1.15f, 230f, float.MaxValue, false, SkillshotType.SkillshotCircle);
            GetR.SetTargetted(0.25f, 2000f);
            base.LoadSpells();
        }
        public bool isBlazed(Obj_AI_Base target)
        {
            return target.HasBuff("brandablaze");
        }

      /*  public double getRDamage(Obj_AI_Hero tar, int count)
        {

        }*/
        public double GetWDamage(Obj_AI_Hero tar)
        {
            if (!isBlazed(tar))
            {
                double increasedDamage = GetW.GetDamage(tar)%25;
                return GetW.GetDamage(tar)+increasedDamage;
            }
            else return GetW.GetDamage(tar);
        }

        public bool isKilleable(Obj_AI_Hero target,double damage)
        {
          double totalDamage=0;
            totalDamage += damage;
            totalDamage += CalcPassiveDamage(target);
            if (target.Health <= totalDamage) return true;
            return false;
        }

        public double getRDamage(Obj_AI_Hero target,int count)
        {
            double result=0;
            if (count >= 5)
            {
                result += 5*GetR.GetDamage(target);
            }
            else if (count >= 4)
            {
                result += 4 * GetR.GetDamage(target);
            }
             else   if (count >= 3)
                result += 3 * GetR.GetDamage(target);
           else if (count >= 2)
            {
                result += 2 * GetR.GetDamage(target);
            }
            else
                result += 1 * GetR.GetDamage(target);
            return result;
        }
        public double CalcPassiveExplosion(Obj_AI_Hero tar,int charges)
        {
            int level = tar.Level;
            double leveldam;
            if (level <= 9)
            {
                double basedam = 11.5f;
                leveldam = basedam + level*0.5f;
            }
            else leveldam = 16;
            double extra_dam = (HeroManager.Player.TotalMagicalDamage/100)*1.5;
            double totalDamage =(double) tar.MaxHealth%(leveldam + extra_dam);
            // 1.5% por cada 100 de ap.
            if (!isBlazed(tar))
            {
                if (getBlazed(tar).Count >= charges)
                {
                    Game.PrintChat("Total damage : " +totalDamage);
                    return totalDamage;
                }
            }
            return 0f;
        }
        public double CalcPassiveDamage(Obj_AI_Hero tar)
        {
            double magicDamage = 0.02 * tar.MaxHealth;
            if (!isBlazed(tar))
            {

                double result =HeroManager.Player.CalcDamage(tar, LeagueSharp.Common.Damage.DamageType.Magical, magicDamage);
                return result;
            }

            var buff = getBlazed(tar);
            if (buff.Count >= 3) magicDamage += CalcPassiveExplosion(tar, 3);
            var time_rest=Game.Time*1000 - buff.StartTime*1000;
            var totalTime = 4000;
            double result2 = (magicDamage*time_rest)/totalTime;
            return result2;
        }
        public BuffInstance getBlazed(Obj_AI_Hero target)
        {
            return target.GetBuff("brandablaze");
        }

        public override bool castQ(Core core)
        {
            if (GetQ.IsReady())
            {
                var qTarget = TargetSelector.GetTarget(GetQ.Range, TargetSelector.DamageType.Magical);
                var onlyBlazedQ = core.GetMenu.getMenuBoolOption("CQS");
                if (qTarget == null || !GetQ.IsInRange(qTarget)) return false;
                if (onlyBlazedQ)
                {
                    if(isBlazed(qTarget))
                    core.cast(qTarget, GetQ, 0);
                }
                else
                {
                    core.cast(qTarget,GetQ,0);
                }
            }
            return false;
        }
        public override bool castW(Core core)
        {
            if (GetW.IsReady())
            {
                var wTarget = TargetSelector.GetTarget(GetW.Range, TargetSelector.DamageType.Magical);
                if (wTarget == null || !GetQ.IsInRange(wTarget)) return false;
                core.cast(wTarget, GetW, 0);
            }
            return base.castW(core);
        }
        public override bool castE(Core core)
        {
            if (GetE.IsReady())
            {
                var eTarget = TargetSelector.GetTarget(GetE.Range, TargetSelector.DamageType.Magical);
                if (eTarget == null || !GetE.IsInRange(eTarget)) return false;
                GetE.Cast(eTarget);
            }
            return base.castE(core);
        }

        public int GetEnemyesBlazed(Obj_AI_Hero target,List<Obj_AI_Hero> checkR )
        {
            int n = 0;
            if (isBlazed(target))
            {
                n++;
            }
            var targetsInRrange = target.GetAlliesInRange(300).Where(isBlazed);

            if (targetsInRrange.Any()) 
            foreach (Obj_AI_Hero t in targetsInRrange)
            {
                if (checkR.Contains(t)) continue;
                checkR.Add(t);
               n+= GetEnemyesBlazed(target,  checkR);
            }
            return checkR.Count;
        }



        public int getEnemiesBlazedInRange(Obj_AI_Hero target)
        {
            var checkedR=
            new List<Obj_AI_Hero>();
            return GetEnemyesBlazed(target, checkedR);

        }

        public IEnumerable<Obj_AI_Base> getBlazedTargets(Obj_AI_Hero tar,Core core)
        {
           return  ObjectManager.Get<Obj_AI_Base>().Where(x => x.Distance(tar) <= 300 && core.GetSpells.isBlazed(x));
        }
        public override bool castR(Core core)
        {
            if (GetR.IsReady())
            {
                var rTarget = TargetSelector.GetTarget(GetR.Range, TargetSelector.DamageType.Magical);
                if (rTarget == null || !GetE.IsInRange(rTarget)) return false;
                var killeable = core.GetMenu.getMenuBoolOption("CR");
                if (getBlazedTargets(rTarget,core).Count()>=2)
                {
                    GetR.Cast(rTarget);
                }

                if (killeable && isKilleable(rTarget,getRDamage(rTarget,getEnemiesBlazedInRange(rTarget)))) // contar pasiva contar daño rebotando con enemigos y minions. esto es solo para 1 rebote.
                {
                    if(notKilleableWithOthersSpells(rTarget))
                    if(core.GetMenu.getMenuBoolOption("R"+rTarget.ChampionName))                    
                    GetR.Cast(rTarget);
                }
                var min = core.GetMenu.GetMenu.Item("CRM").GetValue<Slider>().Value;
                if (getEnemiesBlazedInRange(rTarget) >= min)
                {
                    GetR.Cast(rTarget);
                }
            }
            return base.castR(core);
        }

        private bool notKilleableWithOthersSpells(Obj_AI_Hero tar)
        {
            double totalDamage = 0;
           double qDamage=0d, wDamage=0d, eDamage=0d;
            if (GetQ.IsReady()) qDamage = GetQ.GetDamage(tar);
            if (GetW.IsReady()) wDamage = GetW.GetDamage(tar);
            if (GetE.IsReady()) eDamage = GetE.GetDamage(tar);
            totalDamage = qDamage + wDamage + eDamage;
           
            if (isKilleable(tar, totalDamage)) return false;

            return true;
        }
    }
}
