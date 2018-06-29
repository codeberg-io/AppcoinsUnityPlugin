#!/bin/sh
osascript -e 'activate application "/Applications/Utilities/Terminal.app"'
cd '/Users/nunomonteiro/Documents/GitHub/AppcoinsUnityPlugin/Appcoins Unity/Android/Appcoins Unity'
'/Users/nunomonteiro/Library/Android/sdk/platform-tools/adb' install -r './build/outputs/apk/release/Appcoins Unity-release.apk'
echo 'done' > '/Users/nunomonteiro/Documents/GitHub/AppcoinsUnityPlugin/Appcoins Unity/Assets/AppcoinsUnity/Tools/ProcessCompleted.out'
exit
