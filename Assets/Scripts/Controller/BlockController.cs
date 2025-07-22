using UnityEngine;

public class BlockController : MonoBehaviour
{
    public BlockType blockType;

    [SerializeField] private SpriteRenderer spRenderer;

    public void SetTypeBlock(BlockType type)
    {
        blockType = type;
    }
}