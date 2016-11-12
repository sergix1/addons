using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;
using Microsoft.Win32;
using Color = SharpDX.Color;

namespace Tristana
{
   static class Program
   {


       public static Spell Q, W,E, R;
        public  static Obj_AI_Hero Hero => HeroManager.Player;
       public static Menu menu,combo,misc,drawing,orbwalkerMenu, targetSelectorMenu;
       public static Orbwalking.Orbwalker Orb;
       public const string Name = "Tristana";
        static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += OnLoad;
        }

        private static void OnLoad(EventArgs args)
        {
            if (Hero.ChampionName != Name) return;
            
            LoadSpells();
            LoadMenu();
            Orbwalking.AfterAttack += AfterAttack;
            Interrupter2.OnInterruptableTarget += OnInterrupter;
            AntiGapcloser.OnEnemyGapcloser += OnAntiGapcloser;
            Game.OnUpdate += OnUpdate;
            LeagueSharp.Drawing.OnDraw += Ondraw;
        }



       private static void OnAntiGapcloser(ActiveGapcloser gapcloser)
        {
            var useAntiGapcloser = menu.Item("AG").GetValue<bool>();
            if (R.IsReady() && useAntiGapcloser && gapcloser.Sender.Target == Hero)
            {
                R.Cast(gapcloser.Sender);
            }
        }

        private static void OnInterrupter(Obj_AI_Hero sender, Interrupter2.InterruptableTargetEventArgs args)
        {
            var useAntiInterrupter = menu.Item("AI").GetValue<bool>();
            if (R.IsReady() && useAntiInterrupter)
                R.Cast(sender);

        }

        public static void LoadSpells()
       {
            Q = new Spell(SpellSlot.Q);
            W=new Spell(SpellSlot.W,900);
            E = new Spell(SpellSlot.E, 620);
            R = new Spell(SpellSlot.R, 620);

        }
        public static void LoadMenu()
       {
            menu = new LeagueSharp.Common.Menu(Name, Name, true).SetFontStyle(FontStyle.Regular, Color.Aquamarine);
            orbwalkerMenu = new LeagueSharp.Common.Menu("Orbwalker", "Orbwalker");
            Orb = new Orbwalking.Orbwalker(orbwalkerMenu);
            targetSelectorMenu = new LeagueSharp.Common.Menu("TargetSelector", "TargetSelector");
            combo = new LeagueSharp.Common.Menu("Combo", "Combo Menu");
            {
                combo.AddItem(new MenuItem("CQ", "Use Q").SetValue(true));
                combo.AddItem(new MenuItem("CE", "Use E").SetValue(true));
                combo.AddItem(new MenuItem("CR", "Use R to finish Target").SetValue(true));
            }
            misc = new LeagueSharp.Common.Menu("Misc", "Misc Menu");
            {
                misc.AddItem(new MenuItem("AG", "Anti Gapcloser").SetValue(true));
                misc.AddItem(new MenuItem("AI", "Auto Interrupter").SetValue(true));
            }
            drawing = new LeagueSharp.Common.Menu("Drawing", "Drawing Menu");
           {
               drawing.AddItem(new MenuItem("DW", "Draw W range").SetValue(true));
                drawing.AddItem(new MenuItem("DE", "Draw E range").SetValue(true));
                drawing.AddItem(new MenuItem("DDI", "Draw Damage Indicator").SetValue(true));
            }
            TargetSelector.AddToMenu(targetSelectorMenu);
           menu.AddSubMenu(orbwalkerMenu);        //ORBWALKER
           menu.AddToMainMenu();
           menu.AddSubMenu(combo);
           menu.AddSubMenu(misc);
           menu.AddSubMenu(drawing);
       }

       private static void Ondraw(EventArgs args)
       {
           var DrawW = menu.Item("DW").GetValue<bool>();
           var DrawE = menu.Item("DE").GetValue<bool>();
            var DrawTDamage = menu.Item("DDI").GetValue<bool>();
            if (DrawW&&W.IsReady() )
               Render.Circle.DrawCircle(ObjectManager.Player.Position, 900, System.Drawing.Color.Yellow, 2);
           if (DrawE&&E.IsReady())
               Render.Circle.DrawCircle(ObjectManager.Player.Position, E.Range, System.Drawing.Color.Yellow, 2);

           //Draw Damage
           if (DrawTDamage)
           {
               foreach (var target in HeroManager.Enemies)
               {
                    if (!target.IsHPBarRendered || !target.Position.IsOnScreen()) continue;
                    float TotalRDamage = 0, TotalEDamage = 0;
                   int n1 = 0;
                   if (R.IsReady())
                   {
                       n1 = 1;
                       TotalRDamage = R.GetDamage(target);
                   }

                   TotalEDamage = GetEDmg(target, (GetTristanaEBuff(target)) + n1);
                   float TotalDamage = TotalRDamage + TotalEDamage;
                   drawTotalDamage(target, TotalDamage,TotalRDamage);
               }
           }
       }

        private static void drawTotalDamage(Obj_AI_Hero tar, float totalSpellDamage,float TotalRDamage)
        {
            const int width = 103;
            const int height = 8;
            const int xOffset = 10;
            const int yOffset = 20;
            var percentHealthAfterDamage = Math.Max(0, tar.Health - totalSpellDamage) / tar.MaxHealth;
            var barPos = tar.HPBarPosition;
            var yPos = barPos.Y + yOffset;
            var xPosDamage = barPos.X + xOffset + width * percentHealthAfterDamage;
            var xPosCurrentHp = barPos.X + xOffset + width * tar.Health / tar.MaxHealth;
            var pos1 = barPos.X + xOffset + (107 * percentHealthAfterDamage);
            var differenceInHp = xPosCurrentHp - xPosDamage;
            var hpPos = tar.HPBarPosition;
            for (var i = 0; i < differenceInHp; i++)
            {
                    Drawing.DrawLine(pos1 + i, yPos, pos1 + i, yPos + height, 1, System.Drawing.Color.OrangeRed);


            
            }
            if (tar.Health <= TotalRDamage)
            {
                Drawing.DrawText(hpPos.X, hpPos.Y - 20, System.Drawing.Color.CornflowerBlue, "Kill With R");
            }
            Drawing.DrawText(hpPos.X, hpPos.Y - 40, System.Drawing.Color.CornflowerBlue, "Total AA to Kill " + getTotalAAToKill(tar) );

        }

       public static int getTotalAAToKill(Obj_AI_Base target)
       {
            var critPercent = Hero.Crit;
           int result = (int)(target.Health/Hero.GetAutoAttackDamage(target,true));
           return result <= 0 ? 1 : result;
       }
        public static void SetRanges()
       {
            var up = 7 * (Hero.Level - 1);
            E.Range = 620 + up;
            R.Range = 620 + up;
        }
       private static void OnUpdate(EventArgs args)
       {
           SetRanges();
           switch (Orb.ActiveMode)
           {
               case Orbwalking.OrbwalkingMode.LastHit:
                   break;
               case Orbwalking.OrbwalkingMode.Mixed:
                   break;
               case Orbwalking.OrbwalkingMode.LaneClear:
                   break;
               case Orbwalking.OrbwalkingMode.Combo:
                   Combo();
                   break;
               case Orbwalking.OrbwalkingMode.Freeze:
                   break;
               case Orbwalking.OrbwalkingMode.CustomMode:
                   break;
               case Orbwalking.OrbwalkingMode.None:
                   break;
               default:
                   throw new ArgumentOutOfRangeException();
           }

        }
       private static void Combo()
       {
          var useQ = menu.Item("CQ").GetValue<bool>();
          var useE = menu.Item("CE").GetValue<bool>();
           var ETarget = ComboTarget();
           if (ETarget != null)
           {
                
               if (E.IsReady() && useE)
               {
                   E.Cast(ETarget);

               }
               if (Q.IsReady() && useQ && !E.IsReady())
               {
                   Q.Cast();
               }
               CastR();
           }
           else
           {
               
           }
       }

       public static void CastR()
       {
            //checks
            // E charges
            var useR = menu.Item("CR").GetValue<bool>();
            if(useR)
            foreach (var tar in HeroManager.Enemies)
           {
               if (tar.IsInvulnerable) continue;
                    var rdamage = R.GetDamage(tar);
               var totalDamage = GetEDmg(tar, (GetTristanaEBuff(tar)) + 1)+rdamage;
               if (totalDamage > tar.Health)
               {
                   R.Cast(tar);
               }
           }
       }

       public static bool TristanaHasBuffE(Obj_AI_Base target)
       {
           return target.HasBuff("tristanaechargesound");
       }
        private static float GetEDmg(Obj_AI_Base target,int charges)
        {
            if (!TristanaHasBuffE(target)) return 0;
            var dmg = E.GetDamage(target);
            var buffCount = charges;
            dmg += (dmg * 0.3f * (buffCount - 1));
            return dmg - (target.HPRegenRate * 4);
        }
        private static float GetEDmg(Obj_AI_Base target)
        {
            if (!TristanaHasBuffE(target)) return 0;
            var dmg = E.GetDamage(target);
            var buffCount = GetTristanaEBuff(target);
            dmg += (dmg * 0.3f * (buffCount - 1));
            return dmg - (target.HPRegenRate * 4);
        }
        public static int GetTristanaEBuff(Obj_AI_Base target)
        {
            if(target!=null)
            foreach (BuffInstance buff in target.Buffs)
            {
                if (buff.Name == "tristanaecharge") return buff.Count;
            }

            return 0;
        }
        public static Obj_AI_Hero ComboTarget()
       {
            return TargetSelector.GetTarget(E.Range, TargetSelector.DamageType.Physical);
       }
        private static void AfterAttack(AttackableUnit unit, AttackableUnit target)
        {
            if (unit.IsMe)
            {
                var target2 = ComboTarget();
                if (target2 == null) return;
                if (target.Name == target2.Name)
                {
                   
                }
            }
        }

    }
}
