using LeagueSharp;
using LeagueSharp.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarkMage
{
    class DrawDamage
    {
        SyndraCore core;
        public DrawDamage(SyndraCore core)
        {
            this.core = core;
            LeagueSharp.Drawing.OnDraw += Ondraw;
        }

        private void Ondraw(EventArgs args)
        {
            float QDamage = 0, WDamage = 0, EDamage = 0, RDamage = 0;
            foreach (var tar in HeroManager.Enemies.Where(x => !x.IsDead))
            {
                if (core.GetSpells.getQ.IsReady()) QDamage = core.GetSpells.getQ.GetDamage(tar);
                if (core.GetSpells.getW.IsReady()) WDamage = core.GetSpells.getW.GetDamage(tar);
                if (core.GetSpells.getE.IsReady()) EDamage = core.GetSpells.getE.GetDamage(tar);
                if (core.GetSpells.getR.IsReady()) RDamage = core.GetSpells.RDamage(tar);
                float TotalSpellDamage = QDamage + WDamage + EDamage + RDamage;
                if (tar.IsHPBarRendered && tar.Position.IsOnScreen())
                {
                    var percentHealthAfterDamage = Math.Max(0, tar.Health - TotalSpellDamage) / tar.MaxHealth;
                    var HpPos = tar.HPBarPosition;
                    float currentXPos= HpPos.X;
                    if(RDamage!=0)
                    {
                        Drawing.DrawLine(currentXPos , HpPos.Y, currentXPos + RDamage, HpPos.Y, 5, System.Drawing.Color.Red);
                        currentXPos += RDamage;
                    }
                    if (WDamage != 0)
                    {
                        Drawing.DrawLine(currentXPos, HpPos.Y, currentXPos+ WDamage, HpPos.Y, 5, System.Drawing.Color.BlueViolet);
                        currentXPos += WDamage;
                    }
                     if(EDamage!=0)
                    {
                        Drawing.DrawLine(currentXPos, HpPos.Y, currentXPos +EDamage, HpPos.Y, 5, System.Drawing.Color.Yellow);
                        currentXPos += EDamage;
                    }
                     if(QDamage!=0)
                    {
                        Drawing.DrawLine(currentXPos, HpPos.Y,currentXPos + QDamage, HpPos.Y, 5, System.Drawing.Color.AliceBlue);
                        currentXPos += QDamage;
                    }
                }
            }
        }
    }
}
