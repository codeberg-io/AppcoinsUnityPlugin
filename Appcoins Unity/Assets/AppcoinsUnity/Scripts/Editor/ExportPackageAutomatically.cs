using UnityEditor;
using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class ExportPackageAutomatically : ScriptableObject
{
    [MenuItem("Export Package/Unity")]
    public static void ExportFromUnity()
    {
        // Just the file name with extension
        List<string> filesToRemove = new List<string> {
            "ExportPackageAutomatically.cs"
        };

        int pathToRemove = (Path.GetDirectoryName(Application.dataPath) + "/").Length;

        string[] allFiles = Directory.GetFiles(Application.dataPath + "/", "*.*", SearchOption.AllDirectories);
        List<string> filesToExport = new List<string>(allFiles);

        for(int i = filesToExport.Count - 1; i >= 0; i--)
        {
            filesToExport[i] = filesToExport[i].Substring(pathToRemove);

            if(Path.GetFileName(filesToExport[i]).Substring(0, 1) == ".")
            {
                filesToExport.RemoveAt(i);
            }

            else if(Path.GetExtension(filesToExport[i]) == ".meta")
            {
                filesToExport.RemoveAt(i);
            }

            else if(filesToRemove.Contains(Path.GetFileName(filesToExport[i])))
            {
                filesToExport.RemoveAt(i);
            }
        }

        string packagePath = Application.dataPath +
                                        "/../../" + 
                                        "AppCoins_Unity_Package.unitypackage";
        
        ExportPackageOptions options = ExportPackageOptions.Recurse;
        AssetDatabase.ExportPackage(filesToExport.ToArray(), packagePath, options);

        UnityEngine.Debug.Log("Export done successfully");
    }
}