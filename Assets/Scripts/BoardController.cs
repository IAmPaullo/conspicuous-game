using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BoardController : MonoBehaviour
{
    [SerializeField] private CardController cardController;
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private Transform cardHolder;
    [SerializeField] private Transform boardTransform;
    [SerializeField] private GridLayoutGroup gridLayout;

    [SerializeField] private List<CardSO> cardsScriptableObjects = new();
    [SerializeField] private List<Card> cards = new();
    [SerializeField] List<Vector3> positions = new();


    public List<int> availableIndices;

    private List<GameObject> cardsOnBoard = new();
    private Dictionary<int, List<Card>> cardDictionary = new();

    [SerializeField] private int maxMatches;
    AnimationHandler animator;
    [SerializeField] private float waitTime;

    private void Start()
    {
        CreateBoard(5, 4);
    }
    public void CreateBoard(int rows, int columns)
    {
        List<CardSO> cardPool = GenerateCardPool(rows * columns);
        FillCards(cardPool);
        AnimateCardsToGrid();

    }
    private void FillCards(List<CardSO> shuffledCards)
    {
        foreach (CardSO cardSO in shuffledCards)
        {
            GameObject cardObj = Instantiate(cardPrefab, cardHolder);
            Card card = cardObj.GetComponent<Card>();
            card.Init(this, cardController, cardSO);  // Inicializar a carta com o CardSO
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
        yield return new WaitForSeconds(1f);

        foreach (var card in cards)
        {
            card.Flip(true);
        }

        yield return new WaitForSeconds(waitTime);

        foreach (var card in cards)
        {
            card.Flip(false);
        }
    }

    private void FlipAllCardsToFront()
    {
        foreach (var card in cards)
        {
            card.Flip(true);
        }
        StartCoroutine(WaitAndFlipBack());
    }
    private IEnumerator WaitAndFlipBack()
    {
        yield return new WaitForSeconds(waitTime);
        foreach (var card in cards)
        {
            card.Flip(false);
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

        // Gera o pool de cartas baseado no maxMatches e no número de cartas necessárias
        while (cardPool.Count < totalCards)
        {
            foreach (CardSO cardSO in cardsScriptableObjects)
            {
                for (int j = 0; j < maxMatches; j++)
                {
                    if (cardPool.Count >= totalCards)  // Se o pool de cartas já tiver o número necessário, interrompe
                        break;
                    cardPool.Add(cardSO);
                }
            }
        }

        cardPool.Shuffle();  // Embaralha a lista de cartas
        return cardPool;
    }
    #region button utilities
    public async void ShuffleBoard()
    {
        //List<Vector3> positions = new();
        List<Transform> targets = new();


        for (int i = 0; i < cardsOnBoard.Count; i++)
        {
            positions.Add(cardsOnBoard[i].transform.position);
            targets.Add(cardsOnBoard[i].transform);
        }
        positions.Shuffle();
        await AnimationHandler.MoveAllToSinglePointAsync(targets, Vector3.zero, 0.04f);
        for (int i = 0; i < cardsOnBoard.Count; i++)
        {
            await AnimationHandler.MoveToPositionAsync(cardsOnBoard[i].transform, positions[i], 0.1f);
            //cards[i].FlipCard();
        }
    }
    #endregion
}


