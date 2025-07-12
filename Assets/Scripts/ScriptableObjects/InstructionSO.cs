using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewInstruction", menuName = "ScriptableObjects/InstructionSO", order = 1)]
    public class InstructionSO : ScriptableObject
    {
        public string levelId;
        [TextArea] public string instructionVN;
        [TextArea] public string instructionEN;
        public string GetInstruction(bool isVN)
        {
            return isVN ? instructionVN : instructionEN;
        }
    }
}