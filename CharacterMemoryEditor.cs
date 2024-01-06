using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(CharacterMemory))]
public class CharacterMemoryEditor : Editor {
    private List<bool> showMemoryDetails = new List<bool>();

    public override void OnInspectorGUI() {
        CharacterMemory CM = (CharacterMemory)target;

        serializedObject.Update();

        base.OnInspectorGUI();

        if (GUILayout.Button("Create New Memory Data Asset")) {
            CM.CreateNewMemoryAsset();
        }

        GUILayout.Space(20);

        if (CM.memories != null) {
            SerializedProperty memories = serializedObject.FindProperty("memories");

            if (showMemoryDetails.Count != CM.memories.memory.Count) {
                // Ensure that the list of boolean flags matches the number of Memory objects
                showMemoryDetails.Clear();
                for (int i = 0; i < CM.memories.memory.Count; i++) {
                    showMemoryDetails.Add(false);
                }
            }

            for (int i = 0; i < CM.memories.memory.Count; i++) {
                Memory memory = CM.memories.memory[i];

                EditorGUILayout.BeginHorizontal();

                if (GUILayout.Button(memory.memoryName))
                {
                    // Toggle the clicked button
                    showMemoryDetails[i] = !showMemoryDetails[i];

                    // Close all other buttons
                    for (int j = 0; j < showMemoryDetails.Count; j++) {
                        if (j != i) {
                            showMemoryDetails[j] = false;
                        }
                    }
                }

                // Add a delete button next to each memory
                if (GUILayout.Button("Delete", GUILayout.Width(60))) {
                    CM.memories.memory.RemoveAt(i);
                    showMemoryDetails.RemoveAt(i);
                    serializedObject.ApplyModifiedProperties();
                    EditorUtility.SetDirty(CM.memories);

                    return; // Exit early to prevent accessing invalid indices
                }

                EditorGUILayout.EndHorizontal();

                if (showMemoryDetails[i]) {
                    EditorGUILayout.LabelField("Memory Index = " + memory.memoryIndex);
                    memory.memoryName = EditorGUILayout.TextField("Memory Name", memory.memoryName);
                    memory.memoryPreset = EditorGUILayout.Toggle("Memory Preset", memory.memoryPreset);
                    if (Application.isPlaying) {
                        EditorGUILayout.LabelField("Character Has Remembered: " + Convert.ToBoolean(CM.hasRemembered[i].y));
                    }
                    GUILayout.Space(5);
                }
            }

            GUILayout.Space(10);

            if (GUILayout.Button("Create New Memory")) {
                CM.memories.CreateNewMemory();
                EditorUtility.SetDirty(CM.memories);
            }
        }
        serializedObject.ApplyModifiedProperties();
        EditorUtility.SetDirty(CM.memories);
    }
}
