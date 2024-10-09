using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    [SerializeField] private Image imageRender;
    [SerializeField] private Sprite cardBackside;
    //[SerializeField] private int ID;

    private int cardID;
    private Sprite cardFrontside;
    private BoardController controller;
    public int ID => cardID;


    private bool isBacksideShowing = true;



    public void Init(BoardController controller, CardSO cardSO)
    {
        this.controller = controller;
        cardID = cardSO.ID;
        cardFrontside = cardSO.sprite;
        imageRender.sprite = cardBackside;
        imageRender.enabled = false;
        isBacksideShowing = true;
    }
    private void SelectCard()
    {


        imageRender.sprite = isBacksideShowing ? cardFrontside : cardBackside;

    }

    public void FlipCard()
    {
        int dir = isBacksideShowing ? 1 : -1;
        var cardSide = isBacksideShowing ? cardFrontside : cardBackside;
        if (!imageRender.enabled)
            imageRender.enabled = true;

        AnimationHandler.FlipCardWithImageChange(transform, imageRender, cardSide, 0.15f, dir);
    }
}
