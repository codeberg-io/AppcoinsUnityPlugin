package com.aptoide.appcoinsunity;

import android.app.Fragment;
import android.content.Intent;
import android.os.Bundle;
import android.util.Log;

import com.unity3d.player.UnityPlayer;

/**
 * Created by codeberg on 3/21/2018.
 * Modified by Aptoide
 */

public class UnityAppcoins  {

    private static int REQUEST_CODE = 1337;
    private static  String developerAddress = "0xa43646ed0ece7595267ed7a2ff6f499f9f10f3c7";
    public static UnityAppcoins instance;

    public static void start()
    {
        Application.setupSDK(developerAddress);

        instance = new UnityAppcoins();
    }

    public static void setAddress(String walletAddress){
        developerAddress=walletAddress;
    }

    public static void makePurchase(String skuid)
    {
        if(Application.getIABFlag()) {
            Log.d("UnityAppCoins", "[PrepareBuy] Calling makePurchase with skuid" + skuid + ",");

            Intent shareIntent = new Intent();
            shareIntent.putExtra(PurchaseActivity.SKUID_TAG,skuid);
            shareIntent.setClass(UnityPlayer.currentActivity,PurchaseActivity.class);
            UnityPlayer.currentActivity.startActivityForResult(shareIntent, REQUEST_CODE);
        }
    }

    public  static void addNewSku(String name, String skuid, double price){
        if(Application.getIABFlag()) {
            Application.addSku(name, skuid, price);
        }
    }

    public  static void enableIAB(boolean iab){
        Application.setIAB(iab);
    }

}
