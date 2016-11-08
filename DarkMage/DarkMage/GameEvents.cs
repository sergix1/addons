using LeagueSharp;
using LeagueSharp.Common;
using System;
using System.Collections.Generic;

namespace DarkMage
{
    public static class Lists
    {
        public static  string[] DontRSpellList = new[]
        {
      "Fizz-E","Vladimir-W","Ekkko-R","Zed-R","Yi-Q","Zilean-R","Shaco-R","Kalista-R","Lissandra-R","Kindred-R","Kayle-R","Taric-R"
        };
    }
    public class GameEvents
    {
    //Fizz E.
    //vladimir W.
    //Ekko R.
    //Zed R.
    //Yi Q.
    //Zilean-R dont cast R with zilean ulti or if zilean has it at his R range .
    //Same as  kayle ,kalista and kindred.
    //Dont cast R with that shit (Taric R).
    public GameEvents(SyndraCore core)
        {
            listChampions = new List<Champion>();
            loadNotRSpells(core);
        }
        public List<Champion> listChampions;
        public readonly string[] DontRSpellList = new[]
        {
      "Fizz-E","Vladimir-W","Ekkko-R","Zed-R","Yi-Q","Zilean-R","Shaco-R","Kalista-R","Lissandra-R","Kindred-R","Kayle-R","Taric-R"
        };
        public void loadNotRSpells(SyndraCore core)
        {
            foreach (String s in DontRSpellList)
            {
                var result=s.Split('-');
                var championName = result[0];
                var championSpell = result[1];
                listChampions.Add(new Champion(stringToSpell(championSpell),championName));
            }
            core.championsWithDodgeSpells = listChampions;   
        }
        public SpellSlot intToSpellSlot(int s)
        {
            switch (s)
            {
                case 1:
                    return SpellSlot.Item1;
                case 2:
                    return SpellSlot.Item2;
                case 3:
                    return SpellSlot.Item3;
                case 4:
                    return SpellSlot.Item4;
                case 5:
                    return SpellSlot.Item5;
                case 6:
                    return SpellSlot.Item6;
            }
            return 0;
        }
        public SpellSlot stringToSpell(String s)
        {
            switch(s)
            {
                case "Q":
                    return SpellSlot.Q;
                case "W":
                    return SpellSlot.W;
                case "E":
                    return SpellSlot.E;
                case "R":
                    return SpellSlot.R;
            }
            return SpellSlot.Unknown;
        }
        public String SpellSlotToString(SpellSlot s)
        {
            switch (s)
            {
                case SpellSlot.Q:
                    return "Q";
                case SpellSlot.W:
                    return "W";
                case SpellSlot.E:
                    return "E";
                case SpellSlot.R:
                    return "R";
            }
            return "None";
        }
    }
}