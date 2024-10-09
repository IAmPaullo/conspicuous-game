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


    private bool isFlipped;

    //public Card(BoardController controller, CardSO cardSO)
    //{

    //    this.controller = controller;
    //    //this.ID = ID;
    //    //this.cardFrontside = cardFrontside;
    //    Init();
    //}

    public void Init(BoardController controller, CardSO cardSO)
    {
        this.controller = controller;
        cardID = cardSO.ID;
        cardFrontside = cardSO.sprite;
        imageRender.sprite = cardFrontside;
        imageRender.enabled = false;
    }
    private void SelectCard()
    {


        imageRender.sprite = isFlipped ? cardFrontside : cardBackside;

    }

    public void FlipCard()
    {

        int dir = isFlipped ? 1 : -1;
        if (!imageRender.enabled)
            imageRender.enabled = true;
        AnimationHandler.FlipHorizontal(transform, dir, 0.5f);
    }
}
