using UnityEditor;
using UnityEngine;
using System.IO;

public class PrefabPreviewSaver
{
    // [MenuItem("Tools/Save Prefab Preview as PNG")]
    static void SavePrefabPreview()
    {
        // // Prompt user to select a prefab
        // var prefab = Selection.activeObject as GameObject;
        // if (prefab == null)
        // {
        //     Debug.LogWarning("Please select a prefab in the Project window.");
        //     return;
        // }
        //
        // // Get preview texture from Unity's internal preview system
        // Texture2D preview = AssetPreview.GetAssetPreview(prefab);
        //
        // if (preview == null)
        // {
        //     Debug.LogWarning("Preview is not ready. Try again in a moment.");
        //     return;
        // }
        //
        // // Encode to PNG
        // byte[] pngData = preview.EncodeToPNG();
        //
        // // Save location
        // string path = EditorUtility.SaveFilePanel("Save Prefab Preview", Application.dataPath, prefab.name + "_Preview", "png");
        //
        // if (!string.IsNullOrEmpty(path))
        // {
        //     File.WriteAllBytes(path, pngData);
        //     Debug.Log("Saved prefab preview to: " + path);
        //     AssetDatabase.Refresh();
        // }
    }
}