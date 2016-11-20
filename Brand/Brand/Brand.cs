using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;

namespace Brand
{
    class BrandCore : Core
    {
        public BrandCore(string championName, string menuTittle) : base(championName, menuTittle)
        {
            Console.WriteLine("Brand Loaded");
        }
   
    }
}
