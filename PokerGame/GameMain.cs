using System;
using System.Collections.Generic;

namespace PokerGame
{
    class GameMain
    {
        public const int Q_TABLE_CARDS = 5;
        public const int ACE_VALUE = 14;
        public const int AMOUNT_OF_TESTS = 100;
        static void Main(string[] args)
        {
            Utillity.Init();
            for (int i = 0;i < AMOUNT_OF_TESTS; i++)
            {
                Console.WriteLine("----------------------------test: "+ i + " --------------------------------------------");
                Deck gameDeck = new Deck();
                gameDeck.shuffle();
                List<Card> handCards1 = gameDeck.drawNCards(2);
                List<Card> handCards2 = gameDeck.drawNCards(2);
                List<Card> tableCards = gameDeck.drawNCards(5);

                Console.WriteLine("************Cards hand 1");
                LogCardInfo(handCards1);
                Console.WriteLine("************Cards hand 2");
                LogCardInfo(handCards2);
                Console.WriteLine("***********table");
                LogCardInfo(tableCards);
               
                Power hand1power = GetStrongestPower(handCards1, tableCards);
                Power hand2power = GetStrongestPower(handCards2, tableCards);
                int comparaison = hand1power.CompareTo(hand2power);
                Console.WriteLine("***********results");

                if (comparaison == 0)
                {
                    Console.WriteLine("DRAW, with " + hand1power.comb.ToString());
                }
                if (0 < comparaison)
                {
                    Console.WriteLine("HAND1 WINS, with " + hand1power.comb.ToString());
                }
                else
                {
                    Console.WriteLine("HAND2 WINS, with " + hand2power.comb.ToString());
                }
                Console.WriteLine("------------------------------------------------------------------------");
            }

        }
    
        public static Power GetStrongestPower(List<Card> handCards,List<Card> tableCards)
        {
            List<Card> comboCards = new List<Card>();
            comboCards.AddRange(handCards);
            comboCards.AddRange(tableCards);
           
            List<Card>[] cardsSortedAndStackedByValue = CreatEmptyArrayOfLists(15);
            List<Card>[] cardsSortedAndStackedByFamilly = CreatEmptyArrayOfLists(4);
            
            comboCards.Sort(delegate (Card card1, Card card2){
            int compare = card1.value.CompareTo(card2.value);
            if (compare == 0)
            {
                return card1.value.CompareTo(card2.value);
            }
            return compare;
            });
            StackCardsByFamillyAndValue(comboCards, cardsSortedAndStackedByValue, cardsSortedAndStackedByFamilly);
           

            Power flushPower = GetFlushPower(cardsSortedAndStackedByFamilly);
            Power straightPower = GetStraightPower(cardsSortedAndStackedByValue);
            Power sameCardPower = GetSameCardPower(cardsSortedAndStackedByValue);

            //filter strongest power
            if ((straightPower!=null)&&(flushPower!=null))
            {
                Power finalPower = null;
                if (straightPower.getLastMain().value== ACE_VALUE)
                {
                    finalPower = straightPower;
                    finalPower.comb = Power.eCombinationType.StraightFlush;
                    return finalPower;
                }
                else
                {
                    finalPower = straightPower;
                    finalPower.comb = Power.eCombinationType.RoyalFlush;
                    return finalPower;
                }
            }
            else
            if(sameCardPower!=null&&sameCardPower.comb==Power.eCombinationType.FourOfAkind)
            {
                return sameCardPower;
            }
            else
            if (sameCardPower != null && sameCardPower.comb == Power.eCombinationType.FullHouse)
            {
                return sameCardPower;
            }
            else
            if(flushPower != null)
            {
                return flushPower;
            }
            else
            if(straightPower!=null)
            {
                return straightPower;
            }
            return sameCardPower;
        }
        static Power GetFlushPower(List<Card>[] structuredAndOrderedCards)
        {
            for(int i = 0; i < structuredAndOrderedCards.Length;i++)
            {
                int arraySize = structuredAndOrderedCards[i].Count;
                if (Q_TABLE_CARDS <= arraySize)
                {
                    return new Power(structuredAndOrderedCards[i].GetRange(arraySize- Q_TABLE_CARDS, Q_TABLE_CARDS), null,Power.eCombinationType.Flush);
                }
            }
            return null;
        }
        static Power GetStraightPower(List<Card>[] structuredAndOrderedCards)
        {
            List<Card> straight = new List<Card>();
            for (int i = (structuredAndOrderedCards.Length-1); 0<i; i--)
            {
                if(0<(structuredAndOrderedCards[i].Count))
                {
                    straight.Add(structuredAndOrderedCards[i][0]);
                    if(Q_TABLE_CARDS <= structuredAndOrderedCards[i].Count)
                    {
                        return new Power(straight, null, Power.eCombinationType.Straight);
                    }
                }
                else
                {
                    straight = new List<Card>();
                }
            }
            return null;
        }
        static Power GetSameCardPower(List<Card>[] structuredAndOrderedCards)
        {
            List<Card> quadruples = new List<Card>();
            List<Card> triples = new List<Card>();
            List<Card> pairs= new List<Card>();
            List<Card> kickerCards = new List<Card>();
           
            for (int i = 2; i< structuredAndOrderedCards.Length; i++)
            {
                if(4==structuredAndOrderedCards[i].Count)
                {
                    quadruples.Add(structuredAndOrderedCards[i][0]);
                }
                else
                if (3 == structuredAndOrderedCards[i].Count)
                {
                    triples.Add(structuredAndOrderedCards[i][0]);
                }
                else
                if (2 == structuredAndOrderedCards[i].Count)
                {
                    pairs.Add(structuredAndOrderedCards[i][0]);
                }
                else
                if (1 == structuredAndOrderedCards[i].Count)
                {
                    kickerCards.Add(structuredAndOrderedCards[i][0]);
                }
            }
          //  Console.WriteLine("**********************paire comp lists**************************");
          //
          //  Console.WriteLine("**********************quadruples**************************");
          //  LogCardInfo(quadruples);
          //  Console.WriteLine("**********************triples**************************");
          //  LogCardInfo(triples);
          //  Console.WriteLine("**********************pairs**************************");
          //  LogCardInfo(pairs);
          //  Console.WriteLine("**********************kickers**************************");
          //  LogCardInfo(kickerCards);
          //
          //  Console.WriteLine("************************************************");
            List<Card> mainCards = new List<Card>();
            if (0<quadruples.Count)
            {
                mainCards.Add(quadruples[0]);
                return new Power(mainCards, kickerCards, Power.eCombinationType.FourOfAkind);
            }
            else
            if (0<triples.Count)
            {
                Card  biggestTriple = triples[triples.Count-1];
                mainCards.Add(biggestTriple);
                if (0 < pairs.Count)
                {
                    Card biggestPair = pairs[pairs.Count - 1];
                    mainCards.Insert(0,biggestPair);
                    return new Power(mainCards, null, Power.eCombinationType.FullHouse);
                }
                return new Power(mainCards, kickerCards, Power.eCombinationType.ThreeOfAkind);
            }
            else
            if(0<pairs.Count)
            {
                Card biggestPair = pairs[pairs.Count - 1];
                mainCards.Add(biggestPair);
                if (1< pairs.Count)
                {
                    Card secondBiggestPair = pairs[pairs.Count - 2];
                    mainCards.Insert(0, secondBiggestPair);
                    return new Power(mainCards, kickerCards, Power.eCombinationType.TwoPair);
                }
                return new Power(mainCards, kickerCards, Power.eCombinationType.Pair);
            }
            mainCards.Add(kickerCards[kickerCards.Count - 1]);
            return new Power(mainCards, kickerCards, Power.eCombinationType.HighCard);
        }  
        public static void LogCardInfo(List<Card> cards)
        {
            foreach(Card c in cards)
            {
                c.LogInfo();
            }
        }
        static List<Card>[] CreatEmptyArrayOfLists(int size)
        {
            List<Card>[] cardsByValue = new List<Card>[size];
            for (int i = 0; i < cardsByValue.Length; i++)
            {
                cardsByValue[i] = new List<Card>();
            }
            return cardsByValue;
        }
        static void StackCardsByFamillyAndValue(List<Card> cards, List<Card>[] cardsSortedAndStackedByValue, List<Card>[] cardsSortedAndStackedByFamilly)
        {
            foreach (Card c in cards)
            {
                if (c.value == ACE_VALUE)//the ace has two value's.in termes of ordering
                {
                    cardsSortedAndStackedByValue[1].Add(c);
                }
                cardsSortedAndStackedByValue[c.value].Add(c);
                cardsSortedAndStackedByFamilly[(int)c.familly].Add(c);
            }
        }
    }
}
