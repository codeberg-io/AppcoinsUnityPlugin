#!/bin/sh
cd '/Users/aptoide/Desktop/Appcoins Unity'
'/Users/aptoide/Library/Android/sdk/platform-tools/adb' install -r './build/outputs/apk/release/Appcoins Unity-release.apk'
echo 'done' > '/Users/aptoide/Documents/GitHub/AppcoinsUnityPlugin/Appcoins Unity/Assets/AppcoinsUnity/Tools/ProcessCompleted.out'
exit
