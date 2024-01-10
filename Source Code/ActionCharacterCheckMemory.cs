/*

Adventure Creator
by Chris Burton, 2013-2023

Character Check Memory
by Dr. Puzzler
Part of the Character Memory addon for AC
*/

using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif


namespace AC
{

    [System.Serializable]
    public class ActionCharacterCheckMemory : ActionCheck
    {

        // Declare properties here
        public override ActionCategory Category { get { return ActionCategory.Character; } }
        public override string Title { get { return "Memory Check"; } }
        public override string Description { get { return "Check the remembered state of a memory"; } }
        public override int NumSockets { get { return numSockets; } }


        // Declare variables here
        int numSockets = 2;
        public bool memoryState;
        public NPC npcToEffect;
        public int memoryIndex;
        public override bool CheckCondition()
        {

            CharacterMemory characterMemory = npcToEffect.gameObject.GetComponent<CharacterMemory>();

            for (int i = 0; i < characterMemory.hasRemembered.Count; i++)
            {
                if (characterMemory.hasRemembered[i].x == memoryIndex)
                {
                    memoryState = Convert.ToBoolean(characterMemory.hasRemembered[i].y);
                }
            }
            // Then, we return this value
            return memoryState;
        }


#if UNITY_EDITOR
        [SerializeField] private string memoryName;
        public override void ShowGUI()
        {
            // Action-specific Inspector GUI code here.
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
            // (Optional) Return a string used to describe the specific action's job.

            return string.Empty;
        }

#endif

    }

}
