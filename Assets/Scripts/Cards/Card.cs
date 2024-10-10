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


    [SerializeField] private AudioClip[] onClickAudio;
    [SerializeField] private AudioSource audioSource;
    private AudioClip[] disabledCardSound;

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
        audioSource.clip = soundsToPlay[Random.Range(0, soundsToPlay.Length)];
        audioSource.pitch = Random.value;
        audioSource.Play();
    }
    private void PlaySound(AudioClip soundToPlay)
    {
        audioSource.clip = soundToPlay;
        audioSource.pitch = Random.value;
        audioSource.Play();
    }

}
