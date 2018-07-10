#!/bin/sh
cd '/Users/aptoide/Desktop/Android Build/Appcoins Unity'
'/Applications/Android Studio.app/Contents/gradle/gradle-4.4/bin/gradle' build 2>&1 2>'/Users/aptoide/Documents/GitHub/AppcoinsUnityPlugin/Appcoins Unity/Assets/AppcoinsUnity/Tools/ProcessLog.out'
echo 'done' > '/Users/aptoide/Documents/GitHub/AppcoinsUnityPlugin/Appcoins Unity/Assets/AppcoinsUnity/Tools/ProcessCompleted.out'
exit
