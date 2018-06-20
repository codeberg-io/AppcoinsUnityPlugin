using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using System;
using System.IO;
using System.Diagnostics;
using System.Collections;

public class CustomBuild : EditorWindow {
    internal static UnityEvent continueProcessEvent = new UnityEvent();

    [MenuItem("Custom Build/Unix Custom Android Build")]
    public static void CallUnixCustomBuild()
    {
        CustomUnixBuild unixBuild = new CustomUnixBuild();
        unixBuild.UnixCustomAndroidBuild();
    }
}

public class CustomUnixBuild : CustomBuild
{
    public static string gradlePath = "/Applications/Android\\ Studio.app/Contents/gradle/gradle-4.4/bin/";
    private string ANDROID_STRING = "android";

    public static bool androidPartDone = false;

    public void UnixCustomAndroidBuild()
    {
        ExportScenes expScenes = new ExportScenes();
        string[] scenesPath = expScenes.GetScenesToString(expScenes.AllScenesToExport());
        CustomBuild.continueProcessEvent.AddListener(delegate{this.ExportAndBuildCustomBuildTarget("android", scenesPath);});
    }

    private void ExportAndBuildCustomBuildTarget(string target, string[] scenesPath)
    {
        string path = null;

        if(target.ToLower() == ANDROID_STRING)
        {
            androidPartDone = false;
            path = this.AndroidCustomBuild(scenesPath);
        }

        androidPartDone = true;

        if(path != null)
        {
            this.UnixBuild(path);
        }
    }

    private string AndroidCustomBuild(string[] scenesPath)
    {
        return GenericBuild(scenesPath, null, BuildTarget.Android, BuildOptions.AcceptExternalModificationsToPlayer);
    }

    private string GenericBuild (string[] scenesPath, string target_dir, BuildTarget build_target, BuildOptions build_options)
    {
        string path = this.SelectPath();
        this.VerifyIfFolderAlreadyExists(path);

        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);
        UnityEditor.Build.Reporting.BuildReport error = BuildPipeline.BuildPlayer(scenesPath, path, build_target, build_options);
        UnityEngine.Debug.Log(error.steps.ToString());
        return path;
    }

    private string SelectPath() {
        return EditorUtility.SaveFolderPanel("Save Android Project to folder", "", "");
    }

    // If folder already exists in the chosen directory delete it.
    private void VerifyIfFolderAlreadyExists(string path)
    {
        string[] folders = Directory.GetDirectories(path);

        for(int i = 0; i < folders.Length; i++)
        {
            if((new DirectoryInfo(folders[i]).Name) == PlayerSettings.productName)
            {
                System.IO.DirectoryInfo di = new DirectoryInfo(folders[i]);

                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete(); 
                }
                foreach (DirectoryInfo dir in di.GetDirectories())
                {
                    dir.Delete(true); 
                }
            }
        }
    }

    private void UnixBuild(string path)
    {
        ProcessStartInfo ExportBuildAndRunProcess = new ProcessStartInfo();
        ExportBuildAndRunProcess.FileName = "/bin/bash";
        ExportBuildAndRunProcess.WorkingDirectory = "/";
        ExportBuildAndRunProcess.Arguments = "-c \"cd '" + path + "/" + PlayerSettings.productName + "' && " + gradlePath + "gradle build\"";

        UnityEngine.Debug.Log("process args: " + ExportBuildAndRunProcess.Arguments);

        UnityEngine.Debug.Log("/Users/aptoide/Desktop/" + PlayerSettings.productName);
        ExportBuildAndRunProcess.UseShellExecute = false;
        ExportBuildAndRunProcess.RedirectStandardOutput = true;
        ExportBuildAndRunProcess.RedirectStandardError = true;

        Process newProcess = Process.Start(ExportBuildAndRunProcess);
        // UnityEngine.Debug.Log(newProcess.Id);
        // UnityEngine.Debug.Log(newProcess.ProcessName);
        string strOutput = newProcess.StandardOutput.ReadToEnd();
        string strError = newProcess.StandardError.ReadToEnd();
        newProcess.WaitForExit();
        UnityEngine.Debug.Log(strOutput);
        UnityEngine.Debug.Log("Process exited with code " + newProcess.ExitCode + "\n and errors: " + strError);
    }
}

// Custom class to save the loaded scenes and a bool for each scene that tells us if the user wants to export such scene or not.
public class SceneToExport
{
    private bool _exportScene;
    public bool exportScene
    {
        get { return _exportScene; }
        set { _exportScene = value; }
    }

    private UnityEngine.SceneManagement.Scene _scene;
    public UnityEngine.SceneManagement.Scene scene 
    {
        get { return _scene; }
        set { _scene = value; }
    }
}

// Get all the loaded scenes and aks to the user what scenes he wants to export by 'ExportScenesWindow' class.
public class ExportScenes
{
    private SceneToExport[] scenes;

    public string[] GetScenesToString(SceneToExport[] scenes) 
    {
        ArrayList pathScenes = new ArrayList();

        for(int i = 0; i < scenes.Length; i++)
        {
            if(scenes[i].exportScene)
            {
                pathScenes.Add(scenes[i].scene.path);
            }
        }

        return (pathScenes.ToArray(typeof(string)) as string[]);
    }

    public SceneToExport[] AllScenesToExport()
    {
        this.getAllOpenScenes();
        return this.SelectScenesToExport();
    } 

    public void getAllOpenScenes()
    {
        int sceneCount = UnityEngine.SceneManagement.SceneManager.sceneCount;    
        scenes = new SceneToExport[sceneCount];

        for(int i = 0; i < sceneCount; i++)
        {
            UnityEngine.SceneManagement.Scene scene = UnityEngine.SceneManagement.SceneManager.GetSceneAt(i);

            if(scenes[i] == null) 
            {
                scenes[i] = new SceneToExport();
            }

            scenes[i].scene = scene;
            scenes[i].exportScene = scene.buildIndex != -1 ? true : false;
        }
    }

    // Open ExportScenesWindow window.
    public SceneToExport[] SelectScenesToExport() 
    {
        ExportScenesWindow.CreateExportScenesWindow(scenes);
        return scenes;
    }

    // Draw the window for the user select what scenes he wants to export and configure player settings.
    private class ExportScenesWindow : EditorWindow
    {
        private SceneToExport[] scenes;
        public Vector2 scrollViewVector = Vector2.zero;

        //Create the custom Editor Window
        public static void CreateExportScenesWindow(SceneToExport[] openScenes)
        {
            ExportScenesWindow scenesWindow = (ExportScenesWindow) EditorWindow.GetWindowWithRect(
                typeof(ExportScenesWindow), 
                new Rect(0, 0, 600, 400), 
                true, 
                "Custom Build Settings"
            );

            scenesWindow.scenes = openScenes;
            scenesWindow.minSize = new Vector2(600, 400);
            scenesWindow.Show();
        }

        // Display all the scenes, a button to open 'Player Settings, one to cancel and other to confirm(continue).
        void OnGUI()
        {
            if (!BuildPipeline.isBuildingPlayer) {
                GUI.Label(new Rect(5, 5, 590, 40), "Select the gradle path");
                CustomUnixBuild.gradlePath = GUI.TextField(new Rect(5, 25, 590, 20), CustomUnixBuild.gradlePath);

                GUI.Label(new Rect(5, 45, 590, 40), "Select what scenes you want to export:\n(Only scenes that are in build settings are true by default)");
                float scrollViewLength = scenes.Length * 25f;
                scrollViewVector = GUI.BeginScrollView(new Rect(5, 70, 590, 330), scrollViewVector, new Rect(0, 0, 590, scrollViewLength));
                for (int i = 0; i < scenes.Length; i++)
                {
                    scenes[i].exportScene = GUI.Toggle(new Rect(10, 10 + i * 20, 100, 20), scenes[i].exportScene, scenes[i].scene.name);
                }
                GUI.EndScrollView();

                if (GUI.Button(new Rect(5, 370, 100, 20), "Player Settings"))
                {
                    EditorApplication.ExecuteMenuItem("Edit/Project Settings/Player");
                }
                //if (GUI.Button(new Rect(105, 370, 100, 20), "Pick Gradle Path"))
                //{
                //    EditorUtility.OpenFilePanel("Gradle Path", "", "");
                //}
                if (GUI.Button(new Rect(460, 370, 60, 20), "Cancel"))
                {
                    this.Close();
                }

                if (CustomUnixBuild.gradlePath != "" && GUI.Button(new Rect(530, 370, 60, 20), "Confirm"))
                {
                    CustomBuild.continueProcessEvent.Invoke();
                    this.Close();
                }
            } else {
                if (!CustomUnixBuild.androidPartDone)
                    GUI.Label(new Rect(5, 30, 590, 40), "building gradle project...");
                else
                    GUI.Label(new Rect(5, 30, 590, 40), "Running gradle to generate APK...");
            }
                
        }
    }
}
