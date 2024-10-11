using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;


public class Countdown : Validatable
{
    [Header("Configuration")]
    [SerializeField] private float tickDuration = default;
    [Header("References")]
    [SerializeField] private Image background = default;
    [SerializeField] private GameObject numberOne = default;
    [SerializeField] private GameObject numberTwo = default;
    [SerializeField] private GameObject numberThree = default;
    [SerializeField] private GameObject playText = default;
    [SerializeField] private AudioSource playAudio = default;
    [SerializeField] private AudioSource tickAudio = default;



    private void HideAllObjects()
    {
        background.gameObject.SetActive(false);
        numberOne.SetActive(false);
        numberTwo.SetActive(false);
        numberThree.SetActive(false);
        playText.SetActive(false);
    }

    public IEnumerator CountdownCoroutine()
    {
        yield return new WaitForSeconds(tickDuration);


        CountdownAnimator.Appear(numberThree, tickDuration);
        tickAudio.Play();
        yield return new WaitForSeconds(tickDuration);


        CountdownAnimator.Appear(numberTwo, tickDuration);
        tickAudio.Play();
        yield return new WaitForSeconds(tickDuration);


        CountdownAnimator.Appear(numberOne, tickDuration);
        tickAudio.Play();
        yield return new WaitForSeconds(tickDuration);

        CountdownAnimator.Appear(playText, tickDuration);
        playAudio.Play();

        background.DOFade(0f, tickDuration).OnComplete(() => background.gameObject.SetActive(false));
    }

    public void Restart()
    {
        StartCoroutine(CountdownCoroutine());
    }

    private void CheckDependencies()
    {
        ValidateObject(numberOne, nameof(numberOne));
        ValidateObject(numberTwo, nameof(numberTwo));
        ValidateObject(numberThree, nameof(numberThree));
        ValidateObject(playAudio, nameof(playAudio));
        ValidateObject(tickAudio, nameof(tickAudio));

        if (tickDuration <= 0)
            throw new System.Exception($"Tick duration is invalid!");
    }

}

