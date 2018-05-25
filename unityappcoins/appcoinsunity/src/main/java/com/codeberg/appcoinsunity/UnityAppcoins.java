package com.codeberg.appcoinsunity;

import android.app.Fragment;
import android.content.Intent;
import android.os.Bundle;

import com.asf.appcoins.sdk.iab.payment.PaymentStatus;
import com.unity3d.player.UnityPlayer;

import static com.codeberg.appcoinsunity.Application.appCoinsSdk;

/**
 * Created by codeberg on 3/21/2018.
 */

public class UnityAppcoins extends Fragment {

    private static  String developerAddress = "0xa43646ed0ece7595267ed7a2ff6f499f9f10f3c7";
    public static UnityAppcoins instance;

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);

        // keep fragment instance across activity recreation
        setRetainInstance(true);
    }

    public static void start()
    {
        Application.setupSDK(developerAddress);
        // Instantiate Fragment.
        instance = new UnityAppcoins();
        // Add to the current 'Activity' (a static reference is stored in 'UnityPlayer').
        UnityPlayer.currentActivity.getFragmentManager().beginTransaction().add(instance, "UnityAppcoins").commit();

    }

    public static void setAddress(String walletAddress){
        developerAddress=walletAddress;
    }

    public static void makePurchase(String skuid)
    {
        if(Application.getIABFlag()) {
            appCoinsSdk.buy(skuid, instance.getActivity()).subscribe(() -> {
                // In this case the buy process was triggered as expected.
            }, throwable -> {
                // There was an error triggering the buy process.
                throwable.printStackTrace();
            });

        }
    }

    public  static void addNewSku(String name, String skuid, double price){
        if(Application.getIABFlag()) {
            Application.addSku(name, skuid, price);
        }
    }

    public  static void enableDebug(boolean debug){
        Application.setDebug(debug);
    }

    public  static void enableIAB(boolean iab){
        Application.setIAB(iab);
    }

    public  static void enablePOA(boolean poa){
        Application.setPOA(poa);
    }

    @Override
    public void onActivityResult(int requestCode, int resultCode, Intent data) {
        super.onActivityResult(requestCode, resultCode, data);

        if (Application.appCoinsSdk.onActivityResult(requestCode, requestCode, data)) {
            Application.appCoinsSdk.getCurrentPayment()
                    .subscribe(paymentDetails -> instance.getActivity().runOnUiThread(() -> {
                        if (paymentDetails.getPaymentStatus() == PaymentStatus.SUCCESS) {
                            String skuId = paymentDetails.getSkuId();
                            // Now we tell the sdk to consume the skuId.
                            Application.appCoinsSdk.consume(skuId);
                            // Purchase successfully done. Release the prize.
                            UnityPlayer.UnitySendMessage("AppcoinsUnity","purchaseSuccess",skuId);
                        }
                        else{
                            String skuId = paymentDetails.getSkuId();
                            UnityPlayer.UnitySendMessage("AppcoinsUnity","purchaseFailure",skuId);
                        }
                    }));
        }
    }

}
