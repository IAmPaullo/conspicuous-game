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
    }
    private void SelectCard()
    {


        imageRender.sprite = isFlipped ? cardFrontside : cardBackside;

    }


}
