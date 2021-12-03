using System;
using System.Collections.Generic;
using System.Text;

namespace PokerGame
{
    class Utillity
    {
        private static Random _randomObj;
        public static void Init()
        {
            _randomObj = new Random();
        }
        public static int RandomNumber(int min, int max)
        {
            return _randomObj.Next(min, max);
        }
    }
   
}
