using System;
using System.Collections.Generic;
using System.Text;

namespace PokerGame
{
    class Card
    {

        public enum eFamilly
        {
            Clubs,
            Diamonds,
            Hearts,
            Spades,
        }
        public int value;
        public eFamilly familly;
        public Card(int v, eFamilly f)
        {
            value = v;
            familly = f;
        }
        public void LogInfo()
        {
            Console.WriteLine(value + "," + familly.ToString());
        }
    }
}
