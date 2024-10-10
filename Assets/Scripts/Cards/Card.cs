using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image imageRender;
    [SerializeField] private Sprite cardBackside;

    private int cardID;
    private Sprite cardFrontside;
    private BoardController boardController;
    private CardController cardController;

    private bool isShowingFront;
    private bool isEnabled = true;

    public int ID => cardID;

    public void Init(BoardController boardController, CardController cardController, CardSO cardSO)
    {
        this.boardController = boardController;
        this.cardController = cardController;
        cardID = cardSO.ID;
        cardFrontside = cardSO.sprite;
        imageRender.sprite = cardBackside;
        //imageRender.enabled = false;
        isEnabled = true;
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        if (isEnabled)
        {
            cardController.SelectCard(this);
        }
    }


    public void Flip(bool showFront)
    {
        if (showFront != isShowingFront)
        {
            Sprite targetSprite = showFront ? cardFrontside : cardBackside;
            FlipCard(targetSprite);
            isShowingFront = showFront;
        }
    }


    private void FlipCard(Sprite newSprite)
    {
        transform.DOScaleX(0, 0.2f).OnComplete(() =>
        {
            imageRender.sprite = newSprite;
            transform.DOScaleX(1, 0.2f);
        });
    }

    public bool IsEqual(Card otherCard)
    {
        return otherCard != null && cardID == otherCard.cardID;
    }

    public void DisableCard()
    {
        isEnabled = false;
    }

    public void EnableCard()
    {
        isEnabled = true;
        isShowingFront = true;
        imageRender.sprite = cardBackside;
        //imageRender.enabled = true;
    }

    public bool IsEnabled()
    {
        return isEnabled;
    }
}
