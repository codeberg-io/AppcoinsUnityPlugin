using UnityEditor;
using UnityEngine;
using System;
using System.Collections;
using System.IO;

[Serializable]
class SetupAndroidProject
{
    private static bool didSetAndroidProjectOccured = false;
    private static string appcoinsMainTemplate = Application.dataPath + "/AppcoinsUnity/Plugins/Android/MainTemplate.gradle";
    private static string mainTemplateLocatino = Application.dataPath + "/Plugins/Android/MainTemplate.gradle";
    private static string appcoinsManifestLocation = Application.dataPath + "/AppcoinsUnity/Plugins/Android/AndroidManifest.xml";
    private static string manifestLocation = Application.dataPath + "/Plugins/Android/AndroidManifest.xml";
    private static string[] tagToSearch = {"<application>", "</application>"};
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
            int[] tagLines = {-1, -1};
            int linesNumberIndex = 0;
            int tagLinesIndex = 0;  // tagToSearch index
            string line;

            while((line = fileReader.ReadLine()) != null)
            {
                if(line.Contains(tagToSearch[tagLinesIndex]))
                {
                    tagLines[tagLinesIndex] = linesNumberIndex;

                    if(tagLinesIndex == 1)
                    {
                        // Check for the start of the 'tagToSearch' string (Assume startIndex is the number of tabs)
                        int startIndex = KMP.Matcher(line, tagToSearch[1]);
                        string newLine = line.Substring(0, startIndex);
                        newLine = string.Concat(" ", newLine);
                        newLine = string.Concat(newLine, lineToAdd);
                        fileLines.Add(newLine);

                        string tabs = "";
                        for(int i = 0; i < startIndex; i++)
                        {
                            tabs = string.Concat(" ", tabs);
                        }

                        fileLines.Add(string.Concat(tabs, line.Substring(startIndex)));
                    }

                    tagLinesIndex++;
                }

                linesNumberIndex++;
                fileLines.Add(line);
            }

            if(tagLines[0] > -1 && tagLines[1] > -1)
            {
                
            }
        }

        else
        {
            File.Copy(appcoinsManifestLocation, manifestLocation);
        }
    }

    public static void SetupMainTemplate()
    {
        string[] delimiters = {"\n"};
        string[] allLines = readFileToStringArray(appcoinsMainTemplate, delimiters);

        for(int i = 0; i < allLines.Length; i++)
        {
            allLines[i] = allLines[i].Trim();
        }

        // Search(allLines);
    }

    private static string[] readFileToStringArray(string pathToFile, string[] delimiters)
    {
        StreamReader fileReader = new StreamReader(pathToFile);
        return fileReader.ReadToEnd().Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
    }

    private string[] OverrideStrings(string[] allLines)
    {
        string line;

        for(int i = 0; i < allLines.Length; i++)
        {
            // string word = getWord(line);
            // bool isContainer = CheckContainer(line);
        }

        return allLines;
    }
}

public class KMP
{
    public static int[] Prefix(string prefix)
    {
        int[] backArray = new int[prefix.Length];
        backArray[0] = -1;

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
        int[] backArray = KMP.Prefix(prefix);

        int q = 0;
        int i = -1;

        for(; q < text.Length; q++)
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
                break;
            }
        }

        return (q - prefix.Length + 1);
    }
}