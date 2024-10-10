using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardController : MonoBehaviour
{
    private List<Card> selectedCards = new List<Card>();
    [SerializeField] private int maxSelectedCards = 2;
    private List<Card> allCards = new List<Card>();

    public delegate void OnMatchFound(Card card1, Card card2);
    public event OnMatchFound MatchFoundEvent;

    public delegate void OnMatchFail(Card card1, Card card2);
    public event OnMatchFail MatchFailEvent;

    public void Initialize(List<Card> cards)
    {
        allCards = cards;
    }

    public void SelectCard(Card card)
    {
        if (selectedCards.Contains(card) || selectedCards.Count >= maxSelectedCards)
            return;

        selectedCards.Add(card);
        card.Flip(true);

        if (selectedCards.Count == maxSelectedCards)
        {
            CheckSelectedCards();
        }
    }

    private void CheckSelectedCards()
    {
        if (selectedCards.Count != maxSelectedCards)
            return;

        Card card1 = selectedCards[0];
        Card card2 = selectedCards[1];

        if (card1.IsEqual(card2))
        {
            OnMatch(card1, card2);
        }
        else
        {
            OnMismatch(card1, card2);
        }
    }

    private void OnMatch(Card card1, Card card2)
    {
        card1.DisableCard();
        card2.DisableCard();
        MatchFoundEvent?.Invoke(card1, card2);
        ClearSelectedCards();
    }

    private void OnMismatch(Card card1, Card card2)
    {
        MatchFailEvent?.Invoke(card1, card2);
        StartCoroutine(FlipBackCards(card1, card2));
    }

    private IEnumerator FlipBackCards(Card card1, Card card2)
    {
        yield return new WaitForSeconds(1.0f);
        card1.Flip(false);
        card2.Flip(false);
        ClearSelectedCards();
    }

    private void ClearSelectedCards()
    {
        selectedCards.Clear();
    }

    public List<Card> GetSelectedCards()
    {
        return new List<Card>(selectedCards);
    }

    public bool IsCardSelected(Card card)
    {
        return selectedCards.Contains(card);
    }

    public bool AreAllCardsMatched()
    {
        foreach (Card card in allCards)
        {
            if (card.IsEnabled())
                return false;
        }
        return true;
    }

    public void ResetGame()
    {
        foreach (Card card in allCards)
        {
            card.Flip(false);
            card.EnableCard();
        }
        ClearSelectedCards();
    }
}
