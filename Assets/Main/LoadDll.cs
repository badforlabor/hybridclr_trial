using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HotDllConfig
{
    // 配置一次，不允许修改
    public static List<string> hotDlls = new List<string>()
    {
        "CrazyCollects.dll",
        "Google.Protobuf.dll",
        "Test.dll",
        // "Assembly-CSharp-firstpass.dll",
        // "Assembly-CSharp.dll",
    };    
}

public class LoadDll : MonoBehaviour
{
    private string logFilePath = "";
    void Start()
    {
        // HOOK log
#if UNITY_EDITOR
        logFilePath = Path.Combine(Application.dataPath, ".." , "mylog.txt");
#else
        logFilePath = Path.Combine(Application.persistentDataPath, "mylog.txt");
#endif
        File.WriteAllText(logFilePath, $"{DateTime.Now.ToString()} start...\r\n");
        
        // Application.logMessageReceived += LogMessageReceived;
        Application.logMessageReceivedThreaded += LogMessageReceived;
        
        
        Debug.Log("Load Dll...");
        
        
        
        BetterStreamingAssets.Initialize();
        LoadGameDll();
        RunMain();
    }

    void LogMessageReceived(string condition, string stackTrace, LogType type)
    {
        if (string.IsNullOrEmpty(logFilePath))
        {
            return;
        }

        File.AppendAllText(logFilePath, $"{DateTime.Now.ToString()} [{type.ToString()}] {condition} {stackTrace}");
    }

    private System.Reflection.Assembly gameAss;

    public static AssetBundle AssemblyAssetBundle { get; private set; }

    private void LoadGameDll()
    {
        AssetBundle dllAB = AssemblyAssetBundle = BetterStreamingAssets.LoadAssetBundle("common");
#if !UNITY_EDITOR
        var hotDlls = HotDllConfig.hotDlls;
        foreach (var hot in hotDlls)
        {
            if (string.IsNullOrEmpty(hot))
            {
                continue;
            }

            TextAsset dllContent = dllAB.LoadAsset<TextAsset>($"{hot}.bytes");
            try { 
                System.Reflection.Assembly.Load(dllContent.bytes);
                Debug.Log($"Load succ. dll={hot}");
            }
            catch (Exception e)
            {
                Debug.LogError($"Load failed. dll={hot}, err={e.Message}");

                // 终止
                // throw new Exception($"load dll failed={hot}, err={e.Message}");
                return;
            }    
        }

        TextAsset dllBytes1 = dllAB.LoadAsset<TextAsset>("HotFix.dll.bytes");
        System.Reflection.Assembly.Load(dllBytes1.bytes);
        TextAsset dllBytes2 = dllAB.LoadAsset<TextAsset>("HotFix2.dll.bytes");
        gameAss = System.Reflection.Assembly.Load(dllBytes2.bytes);
#else
        gameAss = AppDomain.CurrentDomain.GetAssemblies().First(assembly => assembly.GetName().Name == "HotFix2");
#endif

        GameObject testPrefab = GameObject.Instantiate(dllAB.LoadAsset<UnityEngine.GameObject>("HotUpdatePrefab.prefab"));
    }

    public void RunMain()
    {
        if (gameAss == null)
        {
            UnityEngine.Debug.LogError("dll未加载");
            return;
        }
        var appType = gameAss.GetType("App");
        var mainMethod = appType.GetMethod("Main");
        mainMethod.Invoke(null, null);

        // 如果是Update之类的函数，推荐先转成Delegate再调用，如
        //var updateMethod = appType.GetMethod("Update");
        //var updateDel = System.Delegate.CreateDelegate(typeof(Action<float>), null, updateMethod);
        //updateDel(deltaTime);
    }

    public void LoadScene2()
    {
        SceneManager.LoadScene("scene2");
    }
}
