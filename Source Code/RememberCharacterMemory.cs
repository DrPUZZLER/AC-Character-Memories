using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AC {


    [RequireComponent(typeof(CharacterMemory))]
    public class RememberCharacterMemory : Remember {
        public override string SaveData() {
            MemoryData memoryData = new MemoryData();
            if (memoryData.characterMemoryIndex != null) {
                memoryData.characterMemoryIndex.Clear();
            } else {
                memoryData.characterMemoryIndex = new List<int>();
            }
            if (memoryData.characterMemoryValue != null) {
                memoryData.characterMemoryValue.Clear();
            } else {
                memoryData.characterMemoryValue = new List<int>();
            }
            CharacterMemory objectCharacterMemory = GetComponent<CharacterMemory>();
            
            foreach (Vector2Int e in objectCharacterMemory.hasRemembered) {
                memoryData.characterMemoryIndex.Add(e.x);
                memoryData.characterMemoryValue.Add(e.y);
            }
            memoryData.objectID = constantID;
            return Serializer.SaveScriptData<MemoryData>(memoryData);
        }

        public override void LoadData(string stringData) {
            MemoryData data = Serializer.LoadScriptData<MemoryData>(stringData);
            if (data == null) return;
            CharacterMemory objectCharacterMemory = GetComponent<CharacterMemory>();
            for (int i = 0; i < objectCharacterMemory.hasRemembered.Count; i++) {
                Vector2Int tempVal = new Vector2Int();
                tempVal.x = data.characterMemoryIndex[i];
                tempVal.y = data.characterMemoryValue[i];
                objectCharacterMemory.hasRemembered[i] = tempVal;
            }
        }
    }

    [System.Serializable]
    public class MemoryData : RememberData {
        public List<int> characterMemoryIndex;
        public List<int> characterMemoryValue;
        public MemoryData() {}
    }
}

