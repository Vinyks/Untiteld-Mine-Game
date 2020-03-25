using UnityEngine;
using UnityEngine.UI;

public class Craft : MonoBehaviour
{
    public int ItemID;
    public int[,] craftingCost;
    void Start()
    {
        transform.GetChild(3).GetComponent<Button>().onClick.AddListener(craftItem);
    }
    void craftItem()
    {
        
        if(FindObjectOfType<InventoryManager>().scanInventory(craftingCost))
        {
            FindObjectOfType<InventoryManager>().useItemForCrafting(craftingCost);
            FindObjectOfType<InventoryManager>().addItemToInventoryById(ItemID);
        }
        
    }
}
