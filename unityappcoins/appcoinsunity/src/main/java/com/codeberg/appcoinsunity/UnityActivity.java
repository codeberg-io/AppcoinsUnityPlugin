package com.codeberg.appcoinsunity;
import com.unity3d.player.UnityPlayerActivity;

import android.content.Intent;
import android.os.Bundle;
import android.util.Log;

public class UnityActivity extends UnityPlayerActivity {
    protected void onCreate(Bundle savedInstanceState) {
        // call UnityPlayerActivity.onCreate()
        super.onCreate(savedInstanceState);
        // print debug message to logcat
        Log.d("UnityActivity", "Activity began.");
    }

    @Override
    protected void onActivityResult(int requestCode, int resultCode, Intent data) {
        super.onActivityResult(requestCode, resultCode, data);
        UnityAppcoins.instance.onActivityResult(requestCode, resultCode, data);
        Log.d("onActivityResultOd", "Function called.");
    }
}