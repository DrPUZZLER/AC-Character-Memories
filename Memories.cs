using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "MemoryData", menuName = "AC Character Memory/Memory Data")]
[Serializable]
public class Memories : ScriptableObject
{
    public List<Memory> memory = new List<Memory>();
    
    public void CreateNewMemory()
    {
        Memory newMemory = new Memory();

        // Check for duplicate memoryIndex
        int newMemoryIndex = memory.Count;
        while (MemoryIndexExists(newMemoryIndex))
        {
            newMemoryIndex++;
        }

        newMemory.memoryIndex = newMemoryIndex;
        newMemory.memoryName = "New Memory " + (newMemoryIndex + 1).ToString();

        memory.Add(newMemory);
        EditorUtility.SetDirty(this);
    }

/*
    public Memory GetMemoryByName(string name) {
    foreach (Memory e in memory) {
        if (e.memoryName == name) {
            return e;
        }
    }

    Debug.LogError("Memory not found. Is the inputed name correct?");
    return null;
}
*/

    private bool MemoryIndexExists(int index) {
        foreach (Memory existingMemory in memory) {
            if (existingMemory.memoryIndex == index) {
                return true;
            }
        }
        return false;
    }
}
