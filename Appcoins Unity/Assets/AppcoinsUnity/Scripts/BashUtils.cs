using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using UnityEngine;

// public class BashUtils {
//     private static string readWindowsArgs = "/c ";
//     private static string readUnixArgs = "-c ";
// }

public abstract class Terminal
{
    public ProcessStartInfo InitializeProcessInfo(string terminalPath)
    {
        ProcessStartInfo processInfo = new ProcessStartInfo();
        processInfo.FileName = terminalPath;
        processInfo.WorkingDirectory = "/";
        processInfo.UseShellExecute = false;
        processInfo.CreateNoWindow = true;
        return processInfo;
    }

    public virtual void RunCommand(string cmd, string path) {}

    public void RunTerminalCommand(string terminalPath, string cmd)
    {
        RunCommand(cmd, "");
    }
}

public class Bash : Terminal
{
    protected static string TERMINAL_PATH = "/bin/bash";

    protected virtual void RunBashCommand(string cmd, string path) {}

    public override void RunCommand(string cmd, string path)
    {
        Bash t;

        if(Directory.Exists("/Applications/Utilities/Terminal.app") || Directory.Exists("/Applications/Terminal.app"))
        {
            t = new BashGUI();
            t.RunBashCommand(cmd, path);
        }

        else
        {
            t = new BashCommandLine();
            t.RunBashCommand(cmd, path);
        }
    }
}

public class BashCommandLine : Bash
{
    protected override void RunBashCommand(string cmd, string path)
    {
        UnityEngine.Debug.Log("Cmd is " + cmd);
        UnityEngine.Debug.Log("Path is " + path);

        ProcessStartInfo processInfo = InitializeProcessInfo(TERMINAL_PATH);
        processInfo.RedirectStandardOutput = true;
        processInfo.RedirectStandardError = true;

        if (path != "")
        {
            processInfo.Arguments = "-c \"cd " + path + " && " + cmd + "\"";
        }
        else
        {
            processInfo.Arguments = "-c \"" + cmd + "\"";
        }

        UnityEngine.Debug.Log("process args: " + processInfo.Arguments);

        Process newProcess = Process.Start(processInfo);

        string strOutput = newProcess.StandardOutput.ReadToEnd();
        string strError = newProcess.StandardError.ReadToEnd();

        newProcess.WaitForExit();
        UnityEngine.Debug.Log(strOutput);
        UnityEngine.Debug.Log("Process exited with code " + newProcess.ExitCode + "\n and errors: " + strError);
    }
}

public class BashGUI : Bash
{
    // protected override void RunBashCommand(string cmd, string path)
    // {
    //     CreateSHFileToExecuteCommand(cmd, path);

    //     ProcessStartInfo processInfo = InitializeProcessInfo(TERMINAL_PATH);
    //     processInfo.CreateNoWindow = false;

	//     processInfo.Arguments = "-c \"chmod +x '" + Application.dataPath + "/AppcoinsUnity/Tools/BashCommand.sh' && " +
    //                             "open -n -W /Applications/Utilities/Terminal.app --args '" + Application.dataPath + "/AppcoinsUnity/Tools/BashCommand.sh' && exit\"";

	//     Process newProcess = new Process();   
	//     newProcess.StartInfo = processInfo;
	//     newProcess.Start();
    //     newProcess.WaitForExit();
    // }

    protected override void RunBashCommand(string cmd, string path)
    {
        CreateSHFileToExecuteCommand(cmd, path);

        ProcessStartInfo processInfo = InitializeProcessInfo(TERMINAL_PATH);
        processInfo.CreateNoWindow = false;

	    processInfo.Arguments = "-c \"chmod +x '" + Application.dataPath + "/AppcoinsUnity/Tools/BashCommand.sh' && " +
                                "open -n -W /Applications/Utilities/Terminal.app --args '" + Application.dataPath + "/AppcoinsUnity/Tools/BashCommand.sh' && exit\"";

	    Process newProcess = new Process();   
	    newProcess.StartInfo = processInfo;
	    newProcess.Start();
        newProcess.WaitForExit();
    }

    private void CreateSHFileToExecuteCommand(string cmd, string path)
    {
        StreamWriter writer = new StreamWriter(Application.dataPath + "/AppcoinsUnity/Tools/BashCommand.sh");

        writer.WriteLine("#!/bin/sh");
        writer.WriteLine("echo =======================");
        writer.WriteLine("echo $$");
        writer.WriteLine("echo =======================");
        writer.WriteLine("echo $PPID");
        writer.WriteLine("echo =======================");
        writer.WriteLine("ps -o ppid=$PPID");
        writer.WriteLine("echo =======================");
        writer.WriteLine("ps -f");
        writer.WriteLine("cd " + path);
        writer.WriteLine(cmd);
        writer.WriteLine("kill $PPID");
        writer.WriteLine("kill $$");
        writer.Close();
    }
}

public abstract class CMD : Terminal
{
    protected static string TERMINAL_PATH = "cmd.exe";
}

public class CMDCommandLine : CMD
{
    public override void RunCommand(string cmd, string path)
    {
        UnityEngine.Debug.Log("Cmd is " + cmd);
        UnityEngine.Debug.Log("Path is " + path);

        ProcessStartInfo processInfo = InitializeProcessInfo(TERMINAL_PATH);
        processInfo.RedirectStandardOutput = true;
        processInfo.RedirectStandardError = true;

        if (path != "")
        {
            processInfo.Arguments = "/c \"cd " + path + " && " + cmd + "\"";
        }
        else
        {
            processInfo.Arguments = "/c \"" + cmd + "\"";
        }

        // Replace string from bash fromat to cmd format
        processInfo.Arguments = processInfo.Arguments.Replace("\"", "");
        processInfo.Arguments = processInfo.Arguments.Replace("'", "\"");

        UnityEngine.Debug.Log("process args: " + processInfo.Arguments);

        Process newProcess = Process.Start(processInfo);

        string strOutput = newProcess.StandardOutput.ReadToEnd();
        string strError = newProcess.StandardError.ReadToEnd();

        newProcess.WaitForExit();
        UnityEngine.Debug.Log(strOutput);
        UnityEngine.Debug.Log("Process exited with code " + newProcess.ExitCode + "\n and errors: " + strError);
    }
}

public class CMDGUI : CMD
{
    public override void RunCommand(string cmd, string path)
    {
        string arguments = null;
        ProcessStartInfo processInfo = InitializeProcessInfo(TERMINAL_PATH);
        processInfo.CreateNoWindow = false;
        processInfo.RedirectStandardInput = true;

        if (path != "")
        {
            arguments = "/c \"cd " + path + " && " + cmd + "\"";
        }
        else
        {
            arguments = "/c \"" + cmd + "\"";
        }

        // Replace string from bash fromat to cmd format
        arguments = arguments.Replace("\"", "");
        arguments = arguments.Replace("'", "\"");

        Process newProcess = Process.Start(processInfo);
        newProcess.StandardInput.WriteLine(arguments);
        newProcess.WaitForExit();        
    }
}
