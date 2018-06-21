using UnityEditor;
using UnityEngine;

public class ExportPackageAutomatically : ScriptableObject
{
    [MenuItem("Export Package/Unity")]
    public static void ExportFromUnity()
    {
        string[] assetsPath = new string[] {
            "Assets/AppcoinsUnity/Example",
            "Assets/AppcoinsUnity/Prefabs",
            "Assets/AppcoinsUnity/Scripts/AppcoinsPurchaser.cs",
            "Assets/AppcoinsUnity/Scripts/AppcoinsSku.cs",
            "Assets/AppcoinsUnity/Scripts/AppcoinsUnity.cs",
            "Assets/AppcoinsUnity/Scripts/BashUtils.cs",
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

}