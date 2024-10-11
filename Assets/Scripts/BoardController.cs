using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BoardController : MonoBehaviour
{
    [SerializeField] private CardController cardController;
    [SerializeField] private Countdown countdown;
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private Transform cardHolder;
    public Transform CardHolder => cardHolder;
    [SerializeField] private Transform boardTransform;
    [SerializeField] private GridLayoutGroup gridLayout;

    [SerializeField] private List<CardSO> cardsScriptableObjects = new();
    [SerializeField] private List<Card> cards = new();
    [SerializeField] List<Vector3> positions = new();


    public List<int> availableIndices;

    private List<GameObject> cardsOnBoard = new();
    private Dictionary<int, List<Card>> cardDictionary = new();

    [SerializeField] private int maxMatches;
    [SerializeField] private float waitTime;

    private void Start()
    {
        CreateBoard(2, 2);
    }
    public void CreateBoard(int rows, int columns)
    {
        List<CardSO> cardPool = GenerateCardPool(rows * columns);
        FillCards(cardPool);
        cardController.Initialize(cards);
        AnimateCardsToGrid();

    }
    private void FillCards(List<CardSO> shuffledCards)
    {
        foreach (CardSO cardSO in shuffledCards)
        {
            GameObject cardObj = Instantiate(cardPrefab, cardHolder);
            Card card = cardObj.GetComponent<Card>();
            card.Init(this, cardController, cardSO);
            cardsOnBoard.Add(cardObj);
            cards.Add(card);
        }
    }

    private void AnimateCardsToGrid()
    {
        List<Vector3> finalPositions = new();
        List<Transform> cardTransforms = new();

        for (int i = 0; i < cardsOnBoard.Count; i++)
        {
            cardTransforms.Add(cardsOnBoard[i].transform);
            finalPositions.Add(cardsOnBoard[i].transform.position);
        }

        MoveCardsToGrid();
        StartCoroutine(FlipToFrontsideThenBack());
    }
    private void MoveCardsToGrid()
    {
        for (int i = 0; i < cardsOnBoard.Count; i++)
        {
            cardsOnBoard[i].transform.SetParent(gridLayout.transform, false);
            //cardsOnBoard[i].transform.localScale = Vector3.one;  
        }
    }
    private IEnumerator FlipToFrontsideThenBack()
    {
        yield return new WaitForSeconds(0.1f);

        foreach (var card in cards)
        {
            card.Flip(true);
        }


        yield return new WaitForSeconds(waitTime);


        foreach (var card in cards)
        {
            card.Flip(false);
        }
        gridLayout.enabled = false;
        yield return countdown.CountdownCoroutine();
        GameStateManager.Instance.SetGameState(GameStateManager.GameState.Playing);

    }


    


    private void ClearBoard()
    {
        if (boardTransform.childCount <= 0) return;

        foreach (Transform child in boardTransform)
        {
            Destroy(child.gameObject);
        }
    }


    private List<CardSO> GenerateCardPool(int totalCards)
    {
        List<CardSO> cardPool = new List<CardSO>();

        while (cardPool.Count < totalCards)
        {
            foreach (CardSO cardSO in cardsScriptableObjects)
            {
                for (int j = 0; j < maxMatches; j++)
                {
                    if (cardPool.Count >= totalCards)
                        break;
                    cardPool.Add(cardSO);
                }
            }
        }

        cardPool.Shuffle();
        return cardPool;
    }
    #region button utilities
    public async void ShuffleBoard()
    {
        List<Transform> targets = new();


        for (int i = 0; i < cardsOnBoard.Count; i++)
        {
            positions.Add(cardsOnBoard[i].transform.position);
            targets.Add(cardsOnBoard[i].transform);
        }
        positions.Shuffle();
        await GameAnimationHandler.MoveAllToSinglePointAsync(targets, Vector3.zero, 0.04f);
        for (int i = 0; i < cardsOnBoard.Count; i++)
        {
            await GameAnimationHandler.MoveToPositionAsync(cardsOnBoard[i].transform, positions[i], 0.1f);
        }
    }
    #endregion
}


