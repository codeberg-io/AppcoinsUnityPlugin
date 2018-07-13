using UnityEditor;
using System;
using System.Collections;
using System.IO;

[Serializable]
class SetupAndroidProject
{
    private static bool didSetAndroidProjectOccured = false;
    private static string manifestLocation = Application.dataPath + "/Plugins/Android/AndroidManifest.xml";
    private static string tagToSerach = "</application>";
    private static string lineToAdd = "<activity android:name=\"com.aptoide.appcoinsunity.PurchaseActivity\" " +
                                      "android:label=\"@string/app_name\" " + 
                                      "android:theme=\"@android:style/Theme.Translucent.NoTitleBar\" />";

    public static void SetupAndroidManifest()
    {
        // Check if [Path-to-Unity-Project]/Assets/Plugins/Android/AndroidManifest exists
        if(File.Exists(SetupAndroidProject.manifestLocation))
        {
            // Open file and check for the 'activity' tag
            StreamReader fileReader = new StreamReader(manifestLocation);
            ArrayList fileLines = new ArrayList();
            int numberOfTabs = 0;
            string line;

            while((line = fileReader.ReadLine()) != null)
            {
                if(line.Contains(tagToSerach))
                {
                    while(line[numberOfTabs].IsWhitepace())
                    {
                        numberOfTabs++;
                    }

                    // Check for the start of the 'tagToSearch' string
                    int startIndex = 

                    fileLines.Add(SetupAndroidManifest.lineToAdd);

                }

                fileLines.Add(line);
            }
        }
    }
}

public class KMP
{
    public static int[] Prefix(string prefix)
    {
        int[] backArray = new int[prefix.Length];
        backArray[0] = 0;

        int i = -1;

        for(int q = 1; q < backArray.Length; q++)
        {
            while(i >= 0 && prefix[i + 1] != prefix[q])
            {
                i = backArray[i];
            }

            if(prefix[i + 1] == prefix[q])
            {
                i++;
            }

            backArray[q] = i;
        }

        return backArray;
    }

    public static int Matcher(string text, string prefix)
    {
        int backArray = KMP.Prefix(prefix);

        int i = -1;

        for(int q = 0; q < text.Length; q++)
        {
            while(i >= 0 && prefix[i + 1] != text[q])
            {
                i = backArray[i];
            }

            if(prefix[i + 1] == text[q])
            {
                i++;
            }

            if(i == (prefix.Length - 1))
            {
                return (q - prefix.Length - 1);
            }
        }
    }
}