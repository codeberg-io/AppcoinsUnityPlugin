package com.codeberg.appcoinsunity;

/**
 * Created by codeberg on 3/29/2018.
 */

import android.util.Log;

import com.asf.appcoins.sdk.ads.AppCoinsAds;
import com.asf.appcoins.sdk.ads.AppCoinsAdsBuilder;
import com.asf.appcoins.sdk.iab.AppCoinsIab;
import com.asf.appcoins.sdk.iab.AppCoinsIabBuilder;
import com.asf.appcoins.sdk.iab.entity.SKU;

import java.math.BigDecimal;
import java.util.LinkedList;
import java.util.List;

public class Application extends android.app.Application {

    public static AppCoinsIab appCoinsSdk;
    private static AppCoinsAds adsSdk;
    static List<SKU> skus = new LinkedList<>();
    static boolean debugFlag=false;
    static boolean POAFlag=false;
    static boolean IABFlag=false;
    static android.app.Application application;

    @Override
    public void onCreate() {
        super.onCreate();
        application=this;
        setupAdsSDK();
        Log.d("UnityActivity", "Aplication began.");
    }

    public void setupAdsSDK() {
        final String poaValue = System.getProperty("ENABLE_POA", "false");
        Log.d("UnityActivity", "POA should be " + poaValue);
        if(poaValue == "true") {

            Log.d("UnityActivity", "POA sdk initialized");
            adsSdk = new AppCoinsAdsBuilder().withDebug(debugFlag)
                    .createAdvertisementSdk(application);
            adsSdk.init(application);
        }
    }

    public static void setupSDK(String developerAddress){
        Log.d("UnityActivity", "Set up SDK began.");

        if(IABFlag) {
            Log.d("UnityActivity", "IAB sdk initialized");
            appCoinsSdk = new AppCoinsIabBuilder(developerAddress).withSkus(buildSkus())
                    .withDebug(debugFlag)
                    .createAppCoinsIab();
        }
    }

    private static List<SKU> buildSkus() {
        return skus;
    }

    public static void addSku(String name, String id, double price){
        skus.add(new SKU(name,id, BigDecimal.valueOf(price)));
    }

    public static void setDebug(boolean debug){
        debugFlag=debug;
    }

    public static void setIAB(boolean iabflag){
        IABFlag=iabflag;
    }

    public static boolean getIABFlag(){
       return IABFlag;
    }

    public static void setPOA(boolean poaflag){
        POAFlag=poaflag;
    }
}
