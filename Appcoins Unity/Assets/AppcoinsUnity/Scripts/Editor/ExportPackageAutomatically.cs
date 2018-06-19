using UnityEditor;
using UnityEngine;

public class ExportPackageAutomatically : ScriptableObject
{
    [MenuItem("Export Package/Unity 2018.b")]
    public static void ExportFromUnity2018()
    {
        string[] assetsPath = new string[] {
            "Assets/AppcoinsUnity/Example",
            "Assets/AppcoinsUnity/Prefabs",
            "Assets/AppcoinsUnity/Scripts/AppcoinsPurchaser.cs",
            "Assets/AppcoinsUnity/Scripts/AppcoinsSku.cs",
            "Assets/AppcoinsUnity/Scripts/AppcoinsUnity.cs",
            "Assets/AppcoinsUnity/Scripts/Editor/AppcoinsStartup.cs",
            "Assets/AppcoinsUnity/Scripts/Editor/AppCoinsProductEditor.cs",
            "Assets/AppcoinsUnity/Scripts/Editor/ProductMaker.cs",
            "Assets/AppcoinsUnity/Scripts/Editor/CustomBuild.cs",
            "Assets/Plugins"
        };

        string packagePath = Application.dataPath +
                                        "/../../" + 
                                        "AppCoins_Unity_Package.unitypackage";
        
        ExportPackageOptions options = ExportPackageOptions.Recurse;
        AssetDatabase.ExportPackage(assetsPath, packagePath, options);

        UnityEngine.Debug.Log("Export done successfully");
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