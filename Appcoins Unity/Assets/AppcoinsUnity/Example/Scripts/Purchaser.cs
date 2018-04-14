using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Codeberg.AppcoinsUnity;

public class Purchaser : AppcoinsPurchaser {

	public Text message;


	void Start(){
		message.text = "Welcome to cody's snacks shop!";
	}

	public override void purchaseSuccess (string skuid)
	{
		base.purchaseSuccess (skuid);
		//purchase is successful release the product

		if(skuid=="dodo"){
		message.text="Thanks! You bought dodo";
		}
		else if(skuid=="monster"){
		message.text="Thanks! You bought monster drink";
		}
		else if(skuid=="chocolate"){
			message.text="Thanks! You bought chocolate";
		}
	}

	public override void purchaseFailure (string skuid)
	{
		base.purchaseFailure (skuid);
		//purchase failed perhaps show some error message

		if(skuid=="dodo"){
			message.text="Sorry! Purchase failed for dodo";
		}
		else if(skuid=="monster"){
			message.text="Sorry! Purchase failed for drink";
		}
		else if(skuid=="chocolate"){
			message.text="Sorry! Purchase failed for chocolate";
		}
	}


	//method called when you click their respective buttons to purchase snacks
	public void buyDodo(){
		makePurchase ("dodo");
	}

	public void buyMonster(){
		makePurchase ("monster");
	}

	public void buyChocolate(){
		makePurchase ("chocolate");
	}
}
