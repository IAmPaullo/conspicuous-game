using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BoardController : MonoBehaviour
{
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private Transform boardTransform;
    [SerializeField] private GridLayoutGroup gridLayout;

    [SerializeField] private List<CardSO> cardsScriptableObjects = new();

    private List<GameObject> cardsOnBoard;
    private Dictionary<int, Card> cardDictionary;

    public int maxMatches { get; private set; }

    public void CreateBoard(int rows, int columns)
    {
        ClearBoard();


        gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayout.constraintCount = columns;
        FillCards(rows, columns);
    }

    private void FillCards(int rows, int columns)
    {
        List<int> chosenCard = new();
        int cardIndex;
        for (int i = 0; i < rows * columns; i++)
        {
            //cardsOnBoard.Add(card);

            do
            {
                cardIndex = Random.Range(0, cardsScriptableObjects.Count + 1);
            }
            while (chosenCard.Contains(cardIndex));
            for (int c = 0; c < maxMatches; c++)
            {
                GameObject card = Instantiate(cardPrefab, boardTransform);
                var spawnedCard = card.GetComponent<Card>();
                spawnedCard.Init(this, cardsScriptableObjects[cardIndex]);
                RegisterCard(spawnedCard);
                cardsOnBoard.Add(card);
            }
        }
    }

    private CardSO SelectCardFrontside()
    {
        return null;
    }

    private void RegisterCard(Card card)
    {
        if (cardDictionary.ContainsKey(card.ID)) return;

        cardDictionary.Add(card.ID, card);
    }
    private void ClearBoard()
    {
        if (boardTransform.childCount <= 0) return;

        foreach (Transform child in boardTransform)
        {
            Destroy(child.gameObject);
        }
    }

}
