using System;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;

public static class MyDebug
{
    public static void Assert(bool b,
        [CallerLineNumber] int line = 0,
        [CallerFilePath] string file = "",
        [CallerMemberName] string member = "")
    {
        if (!b)
        {
            // string currentFile = new System.Diagnostics.StackTrace(true).GetFrame(1).GetFileName(); 
            // int currentLine = new System.Diagnostics.StackTrace(true).GetFrame(1).GetFileLineNumber();
            string currentFile = Path.GetFileName(file);
            int currentLine = line; 
            Debug.LogError($"assert failed. file={currentFile}, line={currentLine}");
#if UNITY_EDITOR
            throw new Exception("assert failed.");
#else
            Application.Quit(1);
#endif            
        }
    }
    public static void Assert(bool b, string msg,
        [CallerFilePath] string file = "",
        [CallerMemberName] string member = "",
        [CallerLineNumber] int line = 0)
    {
        if (!b)
        {
            // string currentFile = new System.Diagnostics.StackTrace(true).GetFrame(1).GetFileName(); 
            // int currentLine = new System.Diagnostics.StackTrace(true).GetFrame(1).GetFileLineNumber();
            string currentFile = Path.GetFileName(file);
            int currentLine = line;
            Debug.LogError($"assert failed. file={currentFile}, line={currentLine}. msg={msg}");
#if UNITY_EDITOR
            throw new Exception($"assert failed. msg={msg}");
#else
            Application.Quit(1);
#endif
        }
    }
    public static void AssertDetail(bool b,
        [CallerFilePath] string file = "",
        [CallerMemberName] string member = "",
        [CallerLineNumber] int line = 0)
    {
        if (!b)
        {
            // string currentFile = new System.Diagnostics.StackTrace(true).GetFrame(1).GetFileName(); 
            // int currentLine = new System.Diagnostics.StackTrace(true).GetFrame(1).GetFileLineNumber();
            string currentFile = Path.GetFileName(file);
            int currentLine = line;
            Debug.LogError($"assert failed. file={currentFile}, line={currentLine}");
#if UNITY_EDITOR
            throw new Exception($"assert failed. ");
#else
            Application.Quit(1);
#endif
        }
        else
        {
            string currentFile = Path.GetFileName(file);
            int currentLine = line;
            Debug.Log($"assert succ. file={currentFile}, line={currentLine}");
        }
    }
    public static void Log(string text,
        [CallerFilePath] string file = "",
        [CallerMemberName] string member = "",
        [CallerLineNumber] int line = 0)
    {
        Debug.LogFormat("{0}_{1}({2}): {3}", Path.GetFileName(file), member, line, text);
    }
}