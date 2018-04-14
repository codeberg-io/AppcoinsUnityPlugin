//created by Lukmon Agboola(Codeberg)
//Note: do not change anything here as it may break the workings of the plugin else you're very sure of
//what you're doing.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Codeberg.AppcoinsUnity{
	
public class AppcoinsUnity : MonoBehaviour {

	[Header("Your wallet address for receiving Appcoins")]
	public string receivingAddress;
	[Header("Enable debug to use testnets e.g Ropsten")]
	public bool enableDebug = false;
	[Header("Add all your products here")]
	public AppcoinsSku[] products;
	[Header("Add your purchaser object here")]
	public AppcoinsPurchaser purchaserObject;

	AndroidJavaClass _class;
	AndroidJavaObject instance { get { return _class.GetStatic<AndroidJavaObject>("instance"); } }

	// Use this for initialization
	void Start () {

		//get refference to java class
		_class = new AndroidJavaClass("com.codeberg.appcoinsunity.UnityAppcoins");

		//setup wallet address
		_class.CallStatic("setAddress",receivingAddress);

		//set debug mode
		//NOTE: this allows you to make purchases with testnets e.g Ropsten
		_class.CallStatic("enableDebug",enableDebug);

		//add all your skus here
		addAllSKUs();

		//start sdk
		_class.CallStatic("start");

	}


	//called to add all skus specified in the inpector window.
	private void addAllSKUs(){
		for(int i=0; i<products.Length; i++){
			_class.CallStatic("addNewSku",products[i].Name,products[i].SKUID,products[i].Price);
		}
	}

	//method used in making purchase
	public void makePurchase(string skuid){
		_class.CallStatic("makePurchase",skuid);
	}

	//callback on successful purchases
	public void purchaseSuccess(string skuid){
			purchaserObject.purchaseSuccess (skuid);
	}

	//callback on failed purchases
	public void purchaseFailure(string skuid){
			purchaserObject.purchaseFailure (skuid);
	}
 }

}
