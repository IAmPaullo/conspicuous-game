using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BoardController : MonoBehaviour
{
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private Transform cardHolder;
    [SerializeField] private Transform boardTransform;
    [SerializeField] private GridLayoutGroup gridLayout;

    [SerializeField] private List<CardSO> cardsScriptableObjects = new();
    [SerializeField] private List<Card> cards = new();

    public List<int> availableIndices;

    private List<GameObject> cardsOnBoard = new();
    private Dictionary<int, List<Card>> cardDictionary = new();

    [SerializeField] private int maxMatches;
    AnimationHandler animator;

    private void Start()
    {
        CreateBoard(5, 4);
    }
    public void CreateBoard(int rows, int columns)
    {
        //gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        //gridLayout.constraintCount = columns;
        for (int i = 0; i < cardsScriptableObjects.Count; i++)
        {
            availableIndices.Add(i);
        }
        //ClearBoard();
        FillCards(rows, columns);
        AnimateBoard();
    }

    private void AnimateBoard()
    {
        List<Transform> transforms = new();
        List<Vector3> positions = new();
        for (int i = 0; i < cardsOnBoard.Count; i++)
        {
            transforms.Add(cardsOnBoard[i].transform);
            positions.Add(cardsOnBoard[i].transform.position);
        }
        positions.Shuffle();

        MoveCardsToParent(transforms.ToArray(), cardHolder);

        AnimationHandler.MoveAllToSpecifiedPoints(transforms.ToArray(), positions.ToArray(), gridLayout.transform, 0.15f);
    }

    //TODO fix board showing card
    private void FillCards(int rows, int columns)
    {
        int cardIndex;
        for (int i = 0; i < rows * columns; i++)
        {
            cardIndex = SelectNewCardIndex();
            if (cardIndex < 0)
                return;
            for (int j = 0; j < maxMatches; j++)
            {
                GameObject card = Instantiate(cardPrefab, gridLayout.transform);
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


    private void MoveCardsToParent(Transform[] cards, Transform parent, Vector3 position = default)
    {
        for (int i = 0; i < cards.Length; i++)
        {
            cards[i].SetParent(parent);
            cards[i].position = position;
        }
    }


    #region button utilities
    public async void ShuffleBoard()
    {
        List<Vector3> positions = new();
        List<Transform> targets = new();
        for (int i = 0; i < cardsOnBoard.Count; i++)
        {
            positions.Add(cardsOnBoard[i].transform.position);
            targets.Add(cardsOnBoard[i].transform);
        }
        await AnimationHandler.MoveAllToSinglePointAsync(targets, Vector3.zero, 0.04f);
        positions.Shuffle();
        for (int i = 0; i < cardsOnBoard.Count; i++)
        {
            //cardsOnBoard[i].transform.position = positions[i];
            await AnimationHandler.MoveToPositionAsync(cardsOnBoard[i].transform, positions[i]);
            cards[i].FlipCard();
        }
    }
    #endregion
}


