using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BoardController : MonoBehaviour
{
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private Transform boardTransform;
    [SerializeField] private GridLayoutGroup gridLayout;

    [SerializeField] private List<CardSO> cardsScriptableObjects = new();
    [SerializeField] private List<Card> cards = new();

    public List<int> availableIndices;

    private List<GameObject> cardsOnBoard = new();
    private Dictionary<int, List<Card>> cardDictionary = new();

    [SerializeField] private int maxMatches;

    private void Start()
    {
        CreateBoard(5, 4);
    }
    public void CreateBoard(int rows, int columns)
    {
        for (int i = 0; i < cardsScriptableObjects.Count; i++)
        {
            availableIndices.Add(i);
        }
        ClearBoard();
        gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayout.constraintCount = columns;
        FillCards(rows, columns);
    }
    private void FillCards(int rows, int columns)
    {
        int cardIndex;
        for (int i = 0; i < rows * columns; i++)
        {
            cardIndex = SelectNewCardIndex();
            if (cardIndex < 0)
                return;
            for (int c = 0; c < maxMatches; c++)
            {
                GameObject card = Instantiate(cardPrefab, boardTransform);
                var spawnedCard = card.GetComponent<Card>();
                spawnedCard.Init(this, cardsScriptableObjects[cardIndex]);
                cardsOnBoard.Add(card);
                cards.Add(spawnedCard);
            }
            RegisterCards(cards);
        }
    }
    private int SelectNewCardIndex()
    {
        if (availableIndices.Count == 0)
            return -1;
        int cardIndex;
        int rand = Random.Range(0, availableIndices.Count);
        cardIndex = availableIndices[rand];
        availableIndices.Remove(cardIndex);

        return cardIndex;
    }

    private void RegisterCards(List<Card> cards)
    {
        if (cardDictionary.ContainsValue(cards)) return;

        cardDictionary.Add(cards[0].ID, cards);
    }
    private void ClearBoard()
    {
        if (boardTransform.childCount <= 0) return;

        foreach (Transform child in boardTransform)
        {
            Destroy(child.gameObject);
        }
    }


    #region button utilities
    public void ShuffleBoard()
    {
        List<Vector3> positions = new();
        List<Transform> targets = new();
        for (int i = 0; i < cardsOnBoard.Count; i++)
        {
            positions.Add(cardsOnBoard[i].transform.position);
            targets.Add(cardsOnBoard[i].transform);
        }
        AnimationHandler.MoveAllToSinglePoint(targets, Vector3.zero, 0.04f);
        positions.Shuffle();
        for (int i = 0; i < cardsOnBoard.Count; i++)
        {
            cardsOnBoard[i].transform.position = positions[i];
            cards[i].FlipCard();
        }


    }
    #endregion
}


