using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card : Validatable, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image imageRender;
    [SerializeField] private Sprite cardBackside;

    private int cardID;
    private Sprite cardFrontside;
    private BoardController boardController;
    private CardController cardController;

    private bool isShowingFront;
    private bool isEnabled;
    private bool isSelected;

    public int ID => cardID;


    [SerializeField] private AudioClip[] onClickAudio;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip disabledCardSound;
    private CardSO cardSO;

    [SerializeField] private Color selectedColor = Color.yellow;
    [SerializeField] private float selectionAnimationScale = 1.2f;

    public void Init(BoardController boardController, CardController cardController, CardSO cardSO)
    {
        this.boardController = boardController;
        this.cardController = cardController;
        this.cardSO = cardSO;
        cardID = cardSO.ID;
        cardFrontside = cardSO.sprite;
        imageRender.sprite = cardBackside;
        isEnabled = true;
        CheckDependencies();
    }


    #region pointer


    public void OnPointerClick(PointerEventData eventData)
    {
        if (!GameStateManager.Instance.CanSelectCard()) return;

        if (!isSelected)
        {
            SelectCardVisualEffect(true);
        }
        else
        {
            SelectCardVisualEffect(false);
            PlaySound(disabledCardSound);
        }
        cardController.SelectCard(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!GameStateManager.Instance.CanSelectCard() || isSelected) return;

        GameAnimationHandler.HoverCard(transform, true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!GameStateManager.Instance.CanSelectCard() /*|| isSelected*/) return;

        GameAnimationHandler.HoverCard(transform, false);
    }

    #endregion

    public void Flip(bool showFront)
    {
        //if (isSelected)
        //    SelectCardVisualEffect(false);

        PlaySound(onClickAudio);
        Sprite targetSprite = showFront ? cardFrontside : cardBackside;
        GameAnimationHandler.FlipCardWithImageChange(transform, imageRender, targetSprite, 0.35f);
        isShowingFront = showFront;
        isSelected = showFront;
    }


    public bool IsEqual(Card otherCard)
    {
        return otherCard != null && cardID == otherCard.cardID;
    }

    public void OnMatch()
    {
        isEnabled = false;
        isSelected = false;
        SelectCardVisualEffect(false);
    }

    public void EnableCard()
    {
        imageRender.sprite = cardBackside;
    }
    public void DisableCard()
    {
        Flip(false);
        isSelected = false;
    }

    public bool IsEnabled()
    {
        return isEnabled;
    }
    private void PlaySound(AudioClip[] soundsToPlay)
    {
        if (soundsToPlay.Length == 0) return;
        var clip = soundsToPlay[Random.Range(0, soundsToPlay.Length)];
        audioSource.pitch = Random.value;
        audioSource.PlayOneShot(clip);
    }
    private void PlaySound(AudioClip soundToPlay)
    {
        if (soundToPlay == null) return;

        audioSource.pitch = Random.value;
        audioSource.PlayOneShot(soundToPlay);
    }

    public void SelectCardVisualEffect(bool selected)
    {
        isSelected = selected;

        if (isSelected)
        {
            //AnimationHandler.StartHoverAnimation(transform, selectionAnimationScale);
            //AnimationHandler.ColorObject(imageRender, selectedColor, 0.3f);
        }
        else
        {
            //AnimationHandler.StopHoverAnimation(transform);
            //AnimationHandler.ColorObject(imageRender, Color.white, 0.3f);
        }
    }


    #region validate
    private void CheckDependencies()
    {
        ValidateObject(imageRender, nameof(imageRender));
        ValidateObject(cardBackside, nameof(cardBackside));
        ValidateObject(cardFrontside, nameof(cardFrontside));
        ValidateObject(boardController, nameof(boardController));
        ValidateObject(cardController, nameof(cardController));

        if (cardID < 0)
            throw new System.Exception($"Card ID is incorrectly set on  ScriptableObj: {cardSO.name}");

        if (audioSource == null)
            Debug.LogWarning($"AudioSource is null on card {ID}, ScriptableObj: {cardSO.name}");
        if (onClickAudio == null || onClickAudio.Length == 0)
            Debug.LogWarning("No onClickAudio was defined.");
        if (disabledCardSound == null)
            Debug.LogWarning("No disabledCardSound was defined");
    }

    #endregion

}
