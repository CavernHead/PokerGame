using System;
using System.Collections.Generic;
using System.Text;

namespace PokerGame
{
    class Power
    {
        public enum eCombinationType
        {
            HighCard,
            Pair,
            TwoPair,
            ThreeOfAkind,
            Straight,
            Flush,
            FullHouse,
            FourOfAkind,
            StraightFlush,
            RoyalFlush,
        }
        public List<Card> main;
        public List<Card> kickers;
        public eCombinationType comb;
        public Power(List<Card> main, List<Card> kickers, eCombinationType comb)
        {
            this.main = main;
            this.kickers = kickers;
            this.comb = comb;
        }
        public Card getLastMain()
        {
            return main[main.Count - 1];
        }
        public Card getBeforeLastMain()
        {
            return main[main.Count - 2];
        }
        public Card GetKicker(int i)
        {
            return kickers[kickers.Count - i - 1];
        }
        public Card GetMain(int i)
        {
            return main[main.Count - 1-i];
        }
        public int CompareTo(Power pow)
        {
            int combStrThis = (int)comb;
            int combStrThat = (int)pow.comb;
            if (combStrThis == combStrThat)
            {
                switch(comb)
                {
                    case eCombinationType.HighCard:
                        return CompareKickers(pow, GameMain.Q_TABLE_CARDS);

                    case eCombinationType.Pair:
                        return CompareStandard(pow, 3);

                    case eCombinationType.TwoPair:
                        return CompareStandard(pow, 1);

                    case eCombinationType.ThreeOfAkind:
                        return CompareStandard(pow, 2);

                    case eCombinationType.Straight:
                        return CompareStandard(pow, 0);
                      
                    case eCombinationType.Flush:
                        return CompareStandard(pow, 0);
                        
                    case eCombinationType.FullHouse:
                        return CompareStandard(pow, 0);
                       
                    case eCombinationType.FourOfAkind:
                        return CompareStandard(pow, 1);
                       
                    case eCombinationType.StraightFlush:
                        return CompareStandard(pow, 0);
     
                    case eCombinationType.RoyalFlush:
                        return 0;  
                }
            }
            else
            if (combStrThis < combStrThat)
            {
                return -1;
            }
            else
            {
                return 1;
            }
            return 0;
        }
        int CompareKickers(Power pow,int ammount)
        {
            for (int i = 0; i < ammount; i++)
            {
                if (GetKicker(i).value == pow.GetKicker(i).value)
                {
                    continue;
                }
                else
                if (GetKicker(i).value < pow.GetKicker(i).value)
                {
                    return -1;
                }
                else
                {
                    return 1;
                }
            }
            return 0;
        }
        public int CompareStandard(Power pow, int amountOfKickersToCompare)
        {
            for(int i = 0; i< main.Count;i++)
            {
                if (GetMain(i).value == pow.GetMain(i).value)
                {
                    continue;
                }
                else
                if (GetMain(i).value < pow.GetMain(i).value)
                {
                    return -1;
                }
                else
                {
                    return 1;
                }
            }
            if(kickers==null)
            {
                return 0;
            }
            return CompareKickers(pow,amountOfKickersToCompare);
        }
    }
}
