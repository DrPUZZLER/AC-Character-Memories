using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;


[RequireComponent(typeof(AC.NPC))]
public class CharacterMemory : MonoBehaviour {

    [SerializeField] string newMemoryPath = "Assets/The-Source/Memories";
    [SerializeField] string newAssetName = "/MemoryData";
    public Memories memories;
    
    [HideInInspector]
    public List<Vector2Int> hasRemembered; // (index, value)
#if UNITY_EDITOR
    public void CreateNewMemoryAsset() {
        if (memories == null) {
            Memories newMemory = ScriptableObject.CreateInstance<Memories>();
            AssetDatabase.CreateAsset(newMemory,GetUniqueAssetPath(newMemoryPath, newAssetName));
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            memories = newMemory;
            EditorUtility.SetDirty(memories);
        } else {
            if (EditorUtility.DisplayDialog("Error", "This object already has a MemoryData asset. Do you want to create another one?", "Yes", "Cancel")) {
                Memories newMemory = ScriptableObject.CreateInstance<Memories>();
                AssetDatabase.CreateAsset(newMemory,GetUniqueAssetPath(newMemoryPath, newAssetName));
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                memories = newMemory;
                EditorUtility.SetDirty(memories);
            }
        }  
    }

    private string GetUniqueAssetPath(string assetPath, string baseName) {
        int counter = 0;
        string newName = baseName;

        // Check for existing assets and increment counter if necessary
        while (AssetDatabase.LoadAssetAtPath($"{assetPath}{newName}.asset", typeof(UnityEngine.Object)) != null)
        {
            counter++;
            newName = $"{baseName}_{counter}";
        }

        return $"{assetPath}{newName}.asset";
    }
    #endif
    void Start() {
        foreach(Memory e in memories.memory) {
            hasRemembered.Add(new Vector2Int(e.memoryIndex, Convert.ToInt32(e.memoryPreset)));
        }
    }
}

