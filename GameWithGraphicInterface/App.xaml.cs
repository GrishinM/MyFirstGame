using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using MyGameEngine;

namespace GameWithGraphicInterface
{
    
    public partial class App : Application
    {
        public static readonly SortedDictionary<string, Army> armys = new SortedDictionary<string, Army>();
        
        public static readonly SortedDictionary<string, BattleUnitsStack> stacks = new SortedDictionary<string, BattleUnitsStack>();

        public static readonly Unit[] units =
        {
            new Abaddon(), new Axe(), new Doom(), new Drow_Ranger(), new Huskar(), new Juggernaut(), new Keeper_of_the_light(), new Lich(), new LineMeleeeCreep(),
            new Omniknight(), new Sniper(), new Sven(), new Tower()
        };

        public static int round;
        public static int delta;
        public static int players;
        public static bool GAME;
        public static bool game;
        public static bool menu;
        public static bool success;
        public static int step;
        public static int player;
    }
}
