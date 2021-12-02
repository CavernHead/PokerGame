using System;
using System.Collections.Generic;
using System.Text;

namespace PokerGame
{
    class Deck
    {
        List<Card> deckCards;
        
        public Deck()
        {
            deckCards = new List<Card>();
            for (int i = 0;i < Enum.GetNames(typeof(Card.eFamilly)).Length;i++)
            {
                for (int j = 2; j < 15; j++)
                {
                    deckCards.Add(new Card(j,((Card.eFamilly)i)));
                }
            }
        }
        public void shuffle()
        {
            for(int i = 0; i < deckCards.Count-1;i++)
            {
                int switchCard = Utillity.RandomNumber(i + 1, deckCards.Count);
                switchToCardsPosition(i, switchCard);
            }
        }
        public List<Card> drawNCards(int n)
        {
            List<Card> cardsRet = deckCards.GetRange(deckCards.Count - n, n);
            deckCards.RemoveRange(deckCards.Count - n, n);
            return cardsRet;
        }
        void switchToCardsPosition(int c1,int c2)
        {
            Card stored = deckCards[c1];
            deckCards[c1] = deckCards[c2];
            deckCards[c2] = stored;
        }
    }
}
