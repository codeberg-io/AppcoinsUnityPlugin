cd "C:/Users/aptoide/Desktop/Android Build/Appcoins Unity"
set var=error
for /f "tokens=*" %%a in ('"C:\Users\aptoide\AppData\Local\Android\Sdk Unity 5.5/platform-tools/adb" get-state') do set var=%%a
if "%var%" == "device" ("C:\Users\aptoide\AppData\Local\Android\Sdk Unity 5.5/platform-tools/adb" shell am start -n "com.aptoide.sample/.UnityPlayerActivity" 2>"C:/Users/aptoide/Documents/GitHub/AppcoinsUnityPlugin/Appcoins Unity/Assets\AppcoinsUnity\Tools\ProcessError.out")
if "%var%" == "error" ( echo error: no usb device found >"C:/Users/aptoide/Documents/GitHub/AppcoinsUnityPlugin/Appcoins Unity/Assets\AppcoinsUnity\Tools\ProcessError.out")
echo done >"C:/Users/aptoide/Documents/GitHub/AppcoinsUnityPlugin/Appcoins Unity/Assets\AppcoinsUnity\Tools\ProcessCompleted.out"
