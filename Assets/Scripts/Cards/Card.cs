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

    private bool isBacksideShowing = true;
    private bool isEnabled = true;

    public int ID => cardID;

    public void Init(BoardController boardController, CardController cardController, CardSO cardSO)
    {
        this.boardController = boardController;
        this.cardController = cardController;
        cardID = cardSO.ID;
        cardFrontside = cardSO.sprite;
        imageRender.sprite = cardBackside;
        imageRender.enabled = false;
        isBacksideShowing = true;
        isEnabled = true;
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        if (isEnabled)
        {
            cardController.SelectCard(this);
        }
    }

    public void FlipCard()
    {
        if (!isEnabled) return;

        int dir = isBacksideShowing ? 1 : -1;
        var cardSide = isBacksideShowing ? cardFrontside : cardBackside;

        if (!imageRender.enabled)
            imageRender.enabled = true;

        AnimationHandler.FlipCardWithImageChange(transform, imageRender, cardSide, 0.15f, dir);
        isBacksideShowing = !isBacksideShowing;
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
        isBacksideShowing = true;
        imageRender.sprite = cardBackside;
        imageRender.enabled = false;
    }

    public bool IsEnabled()
    {
        return isEnabled;
    }
}
