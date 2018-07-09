using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using UnityEngine;

public abstract class Terminal
{
    protected bool ProcessFailed() {
        StreamReader reader = new StreamReader(Application.dataPath + "/AppcoinsUnity/Tools/ProcessLog.out");
        string processLog = reader.ReadToEnd();
        reader.Close();

        if(processLog.Length != 0)
        {
            return true;
        }

        return false;
    }
    
    public ProcessStartInfo InitializeProcessInfo(string terminalPath)
    {
        ProcessStartInfo processInfo = new ProcessStartInfo();
        processInfo.FileName = terminalPath;
        processInfo.WorkingDirectory = "/";
        processInfo.UseShellExecute = false;
        processInfo.CreateNoWindow = true;
        return processInfo;
    }

    public virtual void RunCommand(int buildPhase, string cmd, string cmdArgs, string path, System.Action<int> onDoneCallback) {
    }

    public void RunTerminalCommand(int buildPhase, string terminalPath, string cmd, string cmdArgs, System.Action<int> onDoneCallback)
    {
        RunCommand(buildPhase, cmd, cmdArgs, ".", onDoneCallback);
    }
}

public class Bash : Terminal
{
    protected static string TERMINAL_PATH = "/bin/bash";

    protected virtual void RunBashCommand(int buildPhase, string cmd, string cmdArgs, string path, System.Action<int> onDoneCallback) {
    }

    public override void RunCommand(int buildPhase, string cmd, string cmdArgs, string path, System.Action<int> onDoneCallback)
    {
        Bash t;
        if(Directory.Exists("/Applications/Utilities/Terminal.app") || Directory.Exists("/Applications/Terminal.app"))
        {
            t = new BashGUI();
            t.RunBashCommand(buildPhase, cmd, cmdArgs, path, onDoneCallback);
        }

        else
        {
            t = new BashCommandLine();
            t.RunBashCommand(buildPhase, cmd, cmdArgs, path,onDoneCallback);
        }
    }
}

public class BashCommandLine : Bash
{
    protected override void RunBashCommand(int buildPhase, string cmd, string cmdArgs, string path, System.Action<int> onDoneCallback)
    {
        UnityEngine.Debug.Log("Cmd is " + cmd);
        UnityEngine.Debug.Log("Path is " + path);

        ProcessStartInfo processInfo = InitializeProcessInfo(TERMINAL_PATH);
        processInfo.RedirectStandardOutput = true;
        processInfo.RedirectStandardError = true;

        processInfo.Arguments = "-c \"cd " + path + " && " + cmd + " " + cmdArgs + "\"";

        UnityEngine.Debug.Log("process args: " + processInfo.Arguments);

        Process newProcess = Process.Start(processInfo);

        string strOutput = newProcess.StandardOutput.ReadToEnd();
        string strError = newProcess.StandardError.ReadToEnd();

        newProcess.WaitForExit();
        UnityEngine.Debug.Log(strOutput);
        UnityEngine.Debug.Log("Process exited with code " + newProcess.ExitCode + "\n and errors: " + strError);

        onDoneCallback.Invoke(newProcess.ExitCode);
    }
}

public class BashGUI : Bash
{
    protected override void RunBashCommand(int buildPhase, string cmd, string cmdArgs, string path, System.Action<int> onDoneCallback)
    {
        CreateSHFileToExecuteCommand(buildPhase, cmd, cmdArgs, path);

        ProcessStartInfo processInfo = InitializeProcessInfo(TERMINAL_PATH);
        processInfo.FileName = "/Applications/Utilities/Terminal.app/Contents/MacOS/Terminal";
        processInfo.CreateNoWindow = false;

	    processInfo.Arguments = "'" + Application.dataPath + "/AppcoinsUnity/Tools/BashCommand.sh'";

	    Process newProcess = new Process();   
	    newProcess.StartInfo = processInfo;
	    newProcess.Start();

        //TODO FIXME this file might not be created ever!
        //For the process to complete we check with, 5s interval, for the existence of ProcessCompleted.out
        bool fileExists = File.Exists(Application.dataPath + "/AppcoinsUnity/Tools/ProcessCompleted.out");
        bool condition;
        do { 
            fileExists = File.Exists(Application.dataPath + "/AppcoinsUnity/Tools/ProcessCompleted.out");
            condition = !fileExists;
            Thread.Sleep(2000);
        }
        while (condition);

        //Now we can safely kill the process
        if(!newProcess.HasExited)
        {
            newProcess.Kill();
        }

        int retCode = ((ProcessFailed() == true) ? -1 : 0);
        onDoneCallback.Invoke(retCode);
    }

    //This creates a bash file that gets executed in the specified path
    private void CreateSHFileToExecuteCommand(int buildPhase, string cmd, string cmdArgs, string path)
    {
        StreamWriter writer = new StreamWriter(Application.dataPath + "/AppcoinsUnity/Tools/BashCommand.sh", false);

        writer.WriteLine("#!/bin/sh");

        //Put terminal as first foreground application
        writer.WriteLine("osascript -e 'activate application \"/Applications/Utilities/Terminal.app\"'");
        writer.WriteLine("cd " + path);
        //writer.WriteLine(cmd);
        if(buildPhase == 2)
        {
            writer.WriteLine("if [ \"$(" + cmd + " get-state)\" == \"device\" ]\nthen");
        }

        // writer.WriteLine(cmd + " " + cmdArgs + " 2> '" + Application.dataPath + "/AppcoinsUnity/Tools/ProcessLog.out' | tee '" + Application.dataPath + "/AppcoinsUnity/Tools/ProcessLog.out'");
        writer.WriteLine(cmd + " " + cmdArgs + " 2>&1 2>'" + Application.dataPath + "/AppcoinsUnity/Tools/ProcessLog.out'");

        if(buildPhase == 2)
        {
            writer.WriteLine("else\necho error: no usb device found > '" + Application.dataPath + "/AppcoinsUnity/Tools/ProcessLog.out'");
            writer.WriteLine("fi");
        }

        writer.WriteLine("echo 'done' > '" + Application.dataPath + "/AppcoinsUnity/Tools/ProcessCompleted.out'");
        writer.WriteLine("exit");
        // writer.WriteLine("osascript -e 'tell application \"Terminal\" to close first window'");
        writer.Close();

        File.Delete(Application.dataPath + "/AppcoinsUnity/Tools/ProcessCompleted.out");
        File.Delete(Application.dataPath + "/AppcoinsUnity/Tools/ProcessCompleted.out");
    }
}

public class CMD : Terminal
{
    protected static string TERMINAL_PATH = "cmd.exe";
    private static bool NO_GUI = false;

    public override void RunCommand(int buildPhase, string cmd, string cmdArgs, string path, System.Action<int> onDoneCallback)
    {
        cmd = cmd.Replace("/", "\\");
        cmdArgs = cmdArgs.Replace("/", "\\");
        path = path.Replace("/", "\\");

        ProcessStartInfo processInfo = InitializeProcessInfo(TERMINAL_PATH);
        processInfo.CreateNoWindow = NO_GUI;
        processInfo.UseShellExecute = true;

        if (path != "")
        {
            processInfo.Arguments = "/c \"cd " + path + " && " + cmd + " " + cmdArgs + "\"";
        }
        else
        {
            processInfo.Arguments = "/c \"" + cmd + " " + cmdArgs + "\"";
        }

        // Replace string from bash fromat to cmd format
        processInfo.Arguments = processInfo.Arguments.Replace("\"", "");
        processInfo.Arguments = processInfo.Arguments.Replace("'", "\"");

        Process newProcess = Process.Start(processInfo);

        bool fileExists = File.Exists(Application.dataPath + "/AppcoinsUnity/Tools/ProcessCompleted.out");
        bool didProcessFail = ProcessFailed();
        bool condition;
        do { 
            fileExists = File.Exists(Application.dataPath + "/AppcoinsUnity/Tools/ProcessCompleted.out");
            didProcessFail = ProcessFailed();
            condition = !fileExists && !didProcessFail && !newProcess.HasExited;
            Thread.Sleep(5000);
        }
        while (condition);

        onDoneCallback.Invoke(newProcess.ExitCode);
    }
}
