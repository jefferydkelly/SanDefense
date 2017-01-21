using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurchaseInfo : MonoBehaviour {

    //The price of the tower
    //The tower prefab
    public int price;
    public GameObject tower;

    //The amount of money the player has
    public int money;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        //Update the amount of money that player has
        money = GetComponentInParent<GameInfo>().currentMoney;
    }

    public void buy()
    {
        //Test if the player has enough money to buy the tower
        //Take the price of the tower away from the money the player has
        if (money >= price)
        {
            GetComponentInParent<GameInfo>().currentMoney -= price;
        }
    }

    public void refund()
    {
        //Test if the player has enough money to buy the tower
        //Take the price of the tower away from the money the player has
        if (money >= price)
        {
            GetComponentInParent<GameInfo>().currentMoney += price;
        }
    }
}
