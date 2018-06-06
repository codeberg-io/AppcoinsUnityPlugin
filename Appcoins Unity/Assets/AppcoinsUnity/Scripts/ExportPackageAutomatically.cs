using UnityEditor;
using UnityEngine;

public class ExportPackageAutomatically : ScriptableObject
{
    [MenuItem("Export Package/Unity 2018.b")]
    public static void ExportFromUnity2018()
    {
        string[] assetsPath = new string[] {"Assets/AppcoinsUnity",
                               "Assets/Plugins"};

        UnityEngine.Debug.Log(assetsPath[0] + " - " + assetsPath[1]);

        string packagePath = Application.dataPath +
                                        "/../../" + 
                                        "AppCoins_Unity_Package.unitypackge";
        
        ExportPackageOptions options = ExportPackageOptions.Recurse;
        AssetDatabase.ExportPackage(assetsPath, packagePath, options);
    }

    /*public static void PackageDemo()
    {
        string[] assetPaths = new string[]
        {
      "Assets/SysFont/Demo"
        };

        string packagePath = "unity-sysfont-demo.unitypackage";
        ExportPackageOptions options = ExportPackageOptions.Recurse;
        AssetDatabase.ExportPackage(assetPaths, packagePath, options);
    }

    public static void PackageCompatNGUI()
    {
        string[] assetPaths = new string[]
        {
      "Assets/SysFont/Compatibility"
        };

        string packagePath = "unity-sysfont-ngui.unitypackage";
        ExportPackageOptions options = ExportPackageOptions.Recurse;
        AssetDatabase.ExportPackage(assetPaths, packagePath, options);
    }*/
}