using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MingmingData))]
public class MingmingDataEditor : Editor
{
    private const string MingmingDataPath = "Assets/Data/MingmingData";

    public override void OnInspectorGUI()
    {
        // Draw the default Inspector UI for the script
        DrawDefaultInspector();

        if (GUILayout.Button("Reset all Ids"))
        {
            if (target is MingmingData)
            {
                var mingmings = LoadAllMingmings().OrderBy(m => m.name).ToList();
                for (int i = 0; i < mingmings.Count; i++)
                {
                    mingmings[i].SetId(i);
                }
            }
        }
    }

    private static IEnumerable<MingmingData> LoadAllMingmings()
    {
        string[] guids = AssetDatabase.FindAssets("t:MingmingData", new[] { MingmingDataPath });
        return guids.Select(g => AssetDatabase.GUIDToAssetPath(g)).Select(p => AssetDatabase.LoadAssetAtPath<MingmingData>(p)).ToList();
    }

}
