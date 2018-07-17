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
    private static string currentMainTemplate = Application.dataPath + "/Plugins/Android/mainTemplate.gradle";
    private static string mainTemplateLocation = Application.dataPath + "/Plugins/Android/MainTemplate.gradle";
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

    // Merge current mainTemplate.gradle with appcoins mainTemplate.gradle
    public static void SetupMainTemplate()
    {
        string[] delimiters = {"\n"};

        // Get both files to string arrays (each index is a line of the file)
        ArrayList formattedCurrentMainTemplate; = readFileToStringArray(currentMainTemplate, delimiters, StringSplitOptions.None);
        ArrayList formattedAppcoinsMainTemplateLines = readFileToStringArray(appcoinsMainTemplate, delimiters, StringSplitOptions.RemoveEmptyEntries);
        ArrayList allCurrentMainTemplateLines = new ArrayList();
        ArrayList allAppcoinsMainTemplateLines = new ArrayList();

        int length = allAppcoinsMainTemplateLines.Length > allCurrentMainTemplateLines.Length ? allAppcoinsMainTemplateLines : allCurrentMainTemplateLines;

        // Get new ArrayLists to represent each files without any whitespaces
        for(int i = 0; i < length; i++)
        {
            if(i < allAppcoinsMainTemplateLines.Length)
            {
                allAppcoinsMainTemplateLines.Add(formattedAppcoinsMainTemplateLines[i].Trim());
            }

            if(i < formattedCurrentMainTemplate.Length)
            {
                allCurrentMainTemplateLines.Add(formattedCurrentMainTemplate[i].Trim());
            }
        }

        // Search(allAppcoinsMainTemplateLines);
    }

    // Convert a file to an ArrayList. Each index corrensponds to a line of the file.
    private static ArrayList readFileToStringArray(string pathToFile, string[] delimiters, StringSplitOptions options)
    {
        ArrayList stringArray;
        StreamReader fileReader = new StreamReader(pathToFile);
        return new stringArray(fileReader.ReadToEnd().Split(delimiters, options));
    }

    private static string[] OverrideFile(string[] mergeFileLines, string[] outFileLines)
    {
        string line;

        for(int i = 0; i < mergeFileLines.Length; i++)
        {
            line = mergeFileLines[i];
            bool isContainer = CheckContainer(line);
            string word = getWord();

        }
    }

    private static void Search(ArrayList outFileLines, int lastContainerIndex, ArrayList mergeFileLines, int indexMergeFile)
    {
        if(indexMergeFile == mergeFileLines.Length)
        {
            return;
        }

        string word = GetWord(mergeFileLines[indexMergeFile]);
        bool isContainer = CheckContainer(mergeFileLines[indexMergeFile]);
        int outFileIndex;

        if((outFileIndex = Array.FindIndex(outFileLines, lastContainerIndex, element => element.Contains(word))) != -1)
        {
            if(isContainer)
            {
                lastContainerIndex = outFileIndex; 
                Search(outFileLines, lastContainerIndex, mergeFileLines, ++indexMergeFile);
            }

            else
            {
                Search(outFileLines, lastContainerIndex, mergeFileLines, ++indexMergeFile);
            }
        }

        else
        {
            if(isContainer)
            {
                CopyLines(outFileLines, lastContainerIndex, mergeFileLines, indexMergeFile, isContainer);
            }

            else
            {
                CopyLines(outFileLines, lastContainerIndex, mergeFileLines, indexMergeFile, isContainer);
            }
        }
    }

    private static void CopyLines(ArrayList outFileLines, int lastContainerIndex ,ArrayList mergeFileLines, int indexMergeFile, bool isContainer)
    {
        int innerContainer = 0;

        do
        {
            string line = mergeFileLines[indexMergeFile];

            if(line.Contains("{"))
            {
                innerContainer++;
            }

            outFileLines.Insert(lastContainerIndex + 1, mergeFileLines[indexMergeFile]);
        }
    }

    private static GetWord(string line)
    {
        int wordLength = 0;

        foreach(char letter in line)
        {
            if(Char.IsLetter(letter))
            {
                wordLength++;
            }

            else
            {
                break;
            }
        }

        return line.Substring(0, wordLength);
    }

    private static void CheckContainer(string line)
    {
        return line.Contains("{") ? true : false;
    }
}

// KMP string matcher
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