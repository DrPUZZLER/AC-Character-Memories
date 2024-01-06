[System.Serializable]
public class Memory {
//    public NPC npcToAffect;
    public int memoryIndex;
    public string memoryName;
    public bool memoryPreset;


    public Memory() {
//        npcToAffect = null;
        memoryIndex = 0;
        memoryName = "Memory";
        memoryPreset = false;

    }
}