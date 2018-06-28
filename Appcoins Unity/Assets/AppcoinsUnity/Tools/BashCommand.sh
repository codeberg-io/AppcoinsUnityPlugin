#!/bin/sh
echo =======================
echo $$
echo =======================
echo $PPID
echo =======================
ps -o ppid=$PPID
echo =======================
ps -f
cd '/Users/aptoide/Desktop/Appcoins Unity'
'/Users/aptoide/Library/Android/sdk/platform-tools/adb' install -r './build/outputs/apk/release/Appcoins Unity-release.apk'
kill $PPID
kill $$
