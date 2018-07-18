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

    public static string RemoveAllWhiteSpaces(string s)
    {
        string newString = "";

        foreach(char c in s)
        {
            newString = newString.Insert(newString.Length, Char.IsWhiteSpace(c) ? "" : c.ToString());
        }

        return newString;
    }

    // Merge current mainTemplate.gradle with appcoins mainTemplate.gradle
    public static void SetupMainTemplate()
    {
        string[] delimiters = {"\n"};

        // Get both files to string arrays (each index is a line of the file)
        string currentMainTemplate = ReadFile(currentMainTemplate);
        string appcoinsMainTemplate = RadFile(appcoinsMainTemplate);

        Search(currentMainTemplate, appcoinsMainTemplate, 0);

        StreamWriter fileWriter = new StreamWriter(currentMainTemplate, false);

        foreach(string line in formattedCurrentMainTemplate)
        {
            fileWriter.WriteLine(line);
        }

        fileWriter.Close();
    }

    // Convert a file to an ArrayList. Each index corrensponds to a line of the file.
    private static string ReadFile(string pathToFile)
    {
        StreamReader fileReader = new StreamReader(pathToFile);
        string s = fileReader.ReadToEnd();
        fileReader.Close();
        return s;
    }

    // Recursive function to check if some lines of 'mergeFileLines' does not exist in 'outFileLines' array.
    // 'containerIndexes' is a FIFO that tells us the actual container of the 'indexMergeLine' line of 'mergeFileLines' array.
    private static void Search(string outFileLines, string mergeFileLines, int indexMergeFile)
    {
        // There are not more lines to check
        if(indexMergeFile == mergeFileLines.Length)
        {
            return;
        }

        bool openContainer = CheckOpenContainer((string) mergeFileLines[indexMergeFile]);
        bool closeContainer = CheckCloseContainer((string) mergeFileLines[indexMergeFile]);
        
        string line = (string) mergeFileLines[indexMergeFile];

        if(openContainer)
        {
            bool containerExists = findContainer(outFileLines, line);

            if(!containerExists)
            {
                copyContainer();
            }
        }

        // Is not a container and we just have to copy the line to the current container.
        else
        {

        }

        if(!word.Equals(""))
        {
            // Check if 'word' exists in 'outFileLines' array
            for(int i = 0; i < outFileLines.Count; i++)
            {
                if(((string) outFileLines[i]).Contains(word))
                {
                    wordIndexInOutFile = i;
                    existsInOutFile = true;
                    break;
                }
            }
        }

        // word is not in 'outFileLines' array
        if(word.Equals("") || existsInOutFile)
        {
            if(openContainer)
            {
                containerIndexes.Push(wordIndexInOutFile); 
            }

            else if(closeContainer)
            {
                containerIndexes.Pop();
            }

            Search(outFileLines, containerIndexes, mergeFileLines, ++indexMergeFile);
        }

        else
        {
            CopyLines(outFileLines, containerIndexes, mergeFileLines, indexMergeFile, openContainer);
        }
    }

    private static bool findContainer(string outFileLines, string container)
    {
        int[2] containerBounds = {-1, -1};
        int i = 0;
        int matchIndex;

        if((matchIndex = KMP.Matcher(outFileLines, container)) != -1)
        {
            containerBounds[0] = i++;
            break;
        }

        // Read outFileLines until close bracket of the current container is reached.
        int innerContainer = 0;
        bool closeContainerFound = false;

        while(containerExit > 0 && !closeContainerFound)
        {
            int innerContainer = KMP.Matcher()
            
            if(currentOutLine.Contains("{"))
            {
                innerContainer++;
            }

            if(currentOutLine.Contains("}"))
            {
                innerContainer--;
            }
        }
    }

    private static void CopyLines(ArrayList outFileLines, Stack containerIndexes ,ArrayList mergeFileLines, int indexMergeFile, bool openContainer)
    {
        int innerContainer = 0;
        int insertIndex = ((int) containerIndexes.Peek()) + 1;
        string line;

        do
        {
            line = (string) mergeFileLines[indexMergeFile];

            if(line.Contains("{"))
            {
                innerContainer++;
            }

            if(line.Contains("}"))
            {
                innerContainer--;
            }

            formattedCurrentMainTemplate.Insert(insertIndex, formattedAppcoinsMainTemplate[indexMergeFile]);
            insertIndex++;
            indexMergeFile++;
        } while(innerContainer > 0);

        containerIndexes.Pop();
        containerIndexes.Push(insertIndex - 1);
        Search(outFileLines, containerIndexes, mergeFileLines, indexMergeFile);
    }

    private static string GetWord(string line)
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

    private static bool CheckOpenContainer(string line)
    {
        return line.Contains("{");
    }

    private static bool CheckCloseContainer(string line)
    {
        return line.Contains("}");
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
                return (q - prefix.Length + 1);
                break;
            }
        }

        return -1;
    }
}