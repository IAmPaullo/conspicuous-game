using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardController : MonoBehaviour
{
    private readonly List<Card> selectedCards = new();
    private List<Card> allCards = new();
    [SerializeField, ReadOnly] private int maxSelectedCards = 2;
    [SerializeField] private Transform cardHolder;

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

        if (!GameStateManager.Instance.CanSelectCard()) return;

        if (selectedCards.Count >= maxSelectedCards)
            return;


        if (selectedCards.Contains(card))
        {
            selectedCards.Remove(card);
            card.DisableCard();
            return;
        }


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
        card1.OnMatch();
        card2.OnMatch();
        GameAnimationHandler.MatchSuccessAnimation(card1.transform, card2.transform);
        MatchFoundEvent?.Invoke(card1, card2);
        ClearSelectedCards();

        //End
        if (AllCardsMatched())
            GameStateManager.Instance.EndGame();
    }

    public bool AllCardsMatched()
    {
        int counter = 0;
        for (int i = 0; i < allCards.Count; i++)
        {
            if (allCards[i].IsMatched)
                counter++;
        }
        return counter >= allCards.Count;

    }

    private void OnMismatch(Card card1, Card card2)
    {
        GameAnimationHandler.MatchFailAnimation(card1.transform, card2.transform);
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