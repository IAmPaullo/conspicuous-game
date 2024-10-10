using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card : Validatable, IPointerClickHandler
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


    [SerializeField] private AudioClip[] onClickAudio;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip disabledCardSound;
    private CardSO cardSO;

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



    public void OnPointerClick(PointerEventData eventData)
    {
        if (isEnabled)
        {
            cardController.SelectCard(this);
        }
        else
        {
            PlaySound(disabledCardSound);
        }
    }


    public void Flip(bool showFront)
    {
        if (showFront != isShowingFront)
        {
            PlaySound(onClickAudio);
            Sprite targetSprite = showFront ? cardFrontside : cardBackside;
            AnimationHandler.FlipCardWithImageChange(transform, imageRender, targetSprite, 0.35f);
            isShowingFront = showFront;
        }
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
