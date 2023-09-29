using UnityEngine;

public class BasicDropItem : MonoBehaviour
{
    [SerializeField] DropItemType _itemType;
    public DropItemType ItemType => _itemType;


    void Start()
    {
        // Ensure tag is correct
        this.tag = TagConstraintsConfig.COLLECTIBLE_ITEM_TAG;
    }
}