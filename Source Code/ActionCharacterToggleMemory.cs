/*

Adventure Creator
by Chris Burton, 2013-2023

Character Toggle Memory
by Dr. Puzzler
Part of the Character Memory addon for AC
*/

using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;


#if UNITY_EDITOR
using UnityEditor;
#endif

namespace AC
{

    [System.Serializable]
    public class ActionCharacterToggleMemory : Action
    {

        // Declare properties here
        public override ActionCategory Category { get { return ActionCategory.Character; } }
        public override string Title { get { return "Memory Set"; } }
        public override string Description { get { return "This will toggle a character memory"; } }


        // Declare variables here
        public NPC npcToEffect;
        public bool memoryState;
        public int memoryIndex;
        public override float Run()
        {
            CharacterMemory characterMemory = npcToEffect.gameObject.GetComponent<CharacterMemory>();

            for (int i = 0; i < characterMemory.hasRemembered.Count; i++)
            {
                if (characterMemory.hasRemembered[i].x == memoryIndex)
                {
                    Vector2Int temp = characterMemory.hasRemembered[i];
                    temp.y = Convert.ToInt32(memoryState);
                    characterMemory.hasRemembered[i] = temp;
                }
            }
            return 0f;
        }


        public override void Skip()
        {
            Run();
        }


#if UNITY_EDITOR

        [SerializeField] private string memoryName;
        public override void ShowGUI()
        {
            npcToEffect = (NPC)EditorGUILayout.ObjectField("NPC to affect:", npcToEffect, typeof(NPC), true);
            if (npcToEffect != null)
            {
                if (npcToEffect.GetComponent<CharacterMemory>() == null)
                {
                    EditorGUILayout.LabelField("This NPC has no Character Memory component.");
                    if (GUILayout.Button("Create a new character memory component."))
                    {
                        CharacterMemory cm = npcToEffect.gameObject.AddComponent(typeof(CharacterMemory)) as CharacterMemory;
                    }
                }
                else
                {
                    CharacterMemory characterMemory = npcToEffect.gameObject.GetComponent<CharacterMemory>();
                    if (characterMemory.memories != null)
                    {
                        Memories memories = characterMemory.memories;
                        List<string> options = new List<string>();
                        foreach (Memory e in memories.memory)
                        {
                            options.Add(e.memoryName);
                        }

                        // Remove null or empty memories from the list
                        options.RemoveAll(string.IsNullOrEmpty);

                        if (memories.memory.Count < 1)
                        {
                            EditorGUILayout.LabelField("This NPC has no memories defined.");
                            EditorGUILayout.LabelField("Please define these on the object");
                        }
                        else
                        {
                            int selectedMemoryIndex = EditorGUILayout.Popup("Reference value:", options.IndexOf(memoryName), options.ToArray());
                            if (selectedMemoryIndex >= 0 && selectedMemoryIndex < memories.memory.Count)
                            {
                                memoryIndex = memories.memory[selectedMemoryIndex].memoryIndex;
                                memoryName = memories.memory[selectedMemoryIndex].memoryName;
                                memoryState = EditorGUILayout.Toggle("Memory State", memoryState);
                            }
                        }

                    }
                    else
                    {
                        EditorGUILayout.LabelField("This NPC has no memory asset.");
                        if (GUILayout.Button("Create a new memory asset."))
                        {
                            characterMemory.CreateNewMemoryAsset();
                        }
                    }
                }
            }
        }


        public override string SetLabel()
        {
            return string.Empty;
        }

#endif

    }

}
