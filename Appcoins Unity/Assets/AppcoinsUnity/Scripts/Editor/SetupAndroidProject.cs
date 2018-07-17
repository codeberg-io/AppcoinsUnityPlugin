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
    private static ArrayList formattedAppcoinsMainTemplate;

    // Merge current mainTemplate.gradle with appcoins mainTemplate.gradle
    public static void SetupMainTemplate()
    {
        string[] delimiters = {"\n"};

        // Get both files to string arrays (each index is a line of the file)
        ArrayList formattedCurrentMainTemplate = readFileToStringArray(currentMainTemplate, delimiters, StringSplitOptions.None);
        formattedAppcoinsMainTemplate = readFileToStringArray(appcoinsMainTemplate, delimiters, StringSplitOptions.RemoveEmptyEntries);
        ArrayList allCurrentMainTemplateLines = new ArrayList();
        ArrayList allAppcoinsMainTemplateLines = new ArrayList();

        int length = formattedAppcoinsMainTemplate.Count > formattedCurrentMainTemplate.Count ? formattedAppcoinsMainTemplate.Count : formattedCurrentMainTemplate.Count;

        // Get new ArrayLists to represent each files without any whitespaces
        for(int i = 0; i < length; i++)
        {
            if(i < formattedAppcoinsMainTemplate.Count)
            {
                allAppcoinsMainTemplateLines.Add(((string) formattedAppcoinsMainTemplate[i]).Trim());
            }

            if(i < formattedCurrentMainTemplate.Count)
            {
                allCurrentMainTemplateLines.Add(((string) formattedCurrentMainTemplate[i]).Trim());
            }
        }

        Search(allCurrentMainTemplateLines, new Queue(), allAppcoinsMainTemplateLines, 0);

        StreamWriter fileWriter = new StreamWriter(currentMainTemplate, false);

        foreach(string line in allCurrentMainTemplateLines)
        {
            fileWriter.WriteLine(line);
        }

        fileWriter.Close();
    }

    // Convert a file to an ArrayList. Each index corrensponds to a line of the file.
    private static ArrayList readFileToStringArray(string pathToFile, string[] delimiters, StringSplitOptions options)
    {
        StreamReader fileReader = new StreamReader(pathToFile);
        ArrayList array = new ArrayList(fileReader.ReadToEnd().Split(delimiters, options));
        fileReader.Close();
        return array;
    }

    // Recursive function to check if some lines of 'mergeFileLines' does not exist in 'outFileLines' array.
    // 'containerIndexes' is a FIFO that tells us the actual container of the 'indexMergeLine' line of 'mergeFileLines' array.
    private static void Search(ArrayList outFileLines, Queue containerIndexes, ArrayList mergeFileLines, int indexMergeFile)
    {
        // There are not more lines to check
        if(indexMergeFile == mergeFileLines.Count)
        {
            return;
        }

        string word = GetWord((string) mergeFileLines[indexMergeFile]);
        bool openContainer = CheckOpenContainer((string) mergeFileLines[indexMergeFile]);
        bool closeContainer = CheckCloseContainer((string) mergeFileLines[indexMergeFile]);
        int outFileIndex = -1;

        if(!word.Equals(""))
        {
            // Check if 'word' exists in 'outFileLines' array
            for(int i = 0; i < outFileLines.Count; i++)
            {
                if(((string) outFileLines[i]).Contains(word))
                {
                    outFileIndex = 1;
                    break;
                }
            }
        }

        // word is not in 'outFileLines' array
        if(word.Equals("") || outFileIndex != -1)
        {
            if(openContainer)
            {
                containerIndexes.Enqueue(outFileIndex); 
                Search(outFileLines, containerIndexes, mergeFileLines, ++indexMergeFile);
            }

            else if(closeContainer)
            {
                containerIndexes.Dequeue();
                Search(outFileLines, containerIndexes, mergeFileLines, ++indexMergeFile);
            }

            else
            {
                Search(outFileLines, containerIndexes, mergeFileLines, ++indexMergeFile);
            }
        }

        else
        {
            if(openContainer)
            {
                containerIndexes.Enqueue(outFileIndex);
                CopyLines(outFileLines, containerIndexes, mergeFileLines, indexMergeFile, openContainer);
            }

            else
            {
                CopyLines(outFileLines, containerIndexes, mergeFileLines, indexMergeFile, openContainer);
            }
        }
    }

    private static void CopyLines(ArrayList outFileLines, Queue containerIndexes ,ArrayList mergeFileLines, int indexMergeFile, bool openContainer)
    {
        int innerContainer = 0;
        int insertIndex = indexMergeFile;

        if(openContainer)
        {
            insertIndex = ((int) containerIndexes.Peek()) + 1;
        }
        string line = (string) mergeFileLines[indexMergeFile];

        outFileLines.Insert(insertIndex, formattedAppcoinsMainTemplate[indexMergeFile]);
        indexMergeFile++;
        insertIndex++;
        innerContainer++;

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

            outFileLines.Insert(insertIndex, formattedAppcoinsMainTemplate[indexMergeFile]);
            insertIndex++;
            indexMergeFile++;
        } while(innerContainer > 0);

        containerIndexes.Dequeue();
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
                break;
            }
        }

        return (q - prefix.Length + 1);
    }
}