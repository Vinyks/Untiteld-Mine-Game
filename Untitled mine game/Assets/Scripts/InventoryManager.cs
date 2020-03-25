using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class InventoryManager : MonoBehaviour
{
    
    public bool locked = true;
    public int allSlots;
    public GameObject inventory;
    public GameObject slotHolder;
    public GameObject equipHolder;
    public GameObject equipImage;
    public GameObject craftingWin;
    public GameObject itemTexture;
    public GameObject[] slot;
    public ItemHandler[] itemhandler;
    public GameObject toolSlot;

    public int hotBarSlots = 10;
    bool ableToDrop = true;
    public float dropDistance = 10f;
    public static float renderDistance = 30f;
    public static float playerReach = 5f;
    public GameObject dropPointObject;
    public Camera playerCamera;
    public HotBar hotbar;
    public GameObject mineEffect;
    Object prefab;

    GameObject objectBeingMined;
    GameObject objectLastMined;
    int pickaxeHits;
    int toolLevel;


    void Start()
    {
        slot = new GameObject[allSlots+ hotBarSlots];
        
        for (int i = 0; i < hotBarSlots; i++)
        {
            slot[i] = hotbar.transform.GetChild(i).gameObject;
        }

        for (int i = 0; i < allSlots; i++)
        {
            slot[i+ hotBarSlots] = slotHolder.transform.GetChild(i).gameObject;
        }
        itemhandler = new ItemHandler[allSlots+ hotBarSlots];


        toolSlot = equipHolder.transform.GetChild(0).gameObject;

        equipImage = inventory.transform.GetChild(1).gameObject;
        craftingWin = inventory.transform.GetChild(4).gameObject;
        
    }
    void Update()
    {

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        bool tabWasPressedinFrame = false;
        //Inventory Open and Close Input
        if (locked == false){
            if (Input.GetKeyDown("escape") || Input.GetKeyDown("tab") || Input.GetKeyDown("c"))
            {
                Cursor.lockState = CursorLockMode.Locked;
                EnableMovement();
                locked = true;
                slotHolder.SetActive(false);
                equipImage.SetActive(false);
                equipHolder.SetActive(false);
                craftingWin.SetActive(false);
                tabWasPressedinFrame = true;
            }
        }
        if (tabWasPressedinFrame == false){
            if (Input.GetKeyDown("tab") || Input.GetKeyDown("c"))
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                DisableMovement();
                locked = false;
                if(Input.GetKeyDown("tab"))
                {
                    slotHolder.SetActive(true);
                    equipImage.SetActive(true);
                    equipHolder.SetActive(true);
                }
                if(Input.GetKeyDown("c"))
                {
                    slotHolder.SetActive(true);
                    equipImage.SetActive(true);
                    equipHolder.SetActive(true);
                    craftingWin.SetActive(true);
                }
                
            }
        }

        //Updates Slot Content
        updateInventory();

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        if (Input.GetMouseButtonDown(0))
        {
            if(FindObjectOfType<PointAndClickEvent>().checkRay())
            {
                mine(FindObjectOfType<PointAndClickEvent>().castRay());
            }
        }

        if(Input.GetKeyDown("e"))
        {
            if (FindObjectOfType<PointAndClickEvent>().checkRay())
            {
                pickUp(FindObjectOfType<PointAndClickEvent>().castRay());
            }
        }

        if (Input.GetMouseButton(1) && ableToDrop)
        {
            int slotNum = hotbar.selectedSlot-1;
            
            
            if (FindObjectOfType<HotBar>().slot[slotNum].transform.childCount > 0)
            {
                GameObject child = hotbar.transform.GetChild(slotNum).GetChild(0).gameObject;

                prefab = FindObjectOfType<PrefabList>().list[child.GetComponent<ItemHandler>().item.ID].prefab;
                ableToDrop = false;
                drop();
                child.GetComponent<ItemHandler>().item.amount--;
                if(child.GetComponent<ItemHandler>().item.amount <= 0)
                {
                    Destroy(child.gameObject);
                }
            }
            
        }
        
        if (Input.GetMouseButtonUp(1) == true)
        {
            ableToDrop = true;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


    }


    void DisableMovement()
    {
        FindObjectOfType<Mouselook>().enabled = false;
    }

    void EnableMovement()
    {
        FindObjectOfType<Mouselook>().enabled = true;
    }


    /*
     * Method that scans the Inventory to add an Item
     * 1. To an existing Stack of Items
     * 2. Creates a new Item Stack if the inventory didn't contain a variable of that item
     */
    public void addItemToInventoryByRaycast(RaycastHit hitInfo)
    {
        bool completed = false; // Variable for checking if the item already exists


        //1.
        //Loop that checks if item is already avaliable in inventory

        for (int i = 0; i < slot.Length; i++)
        {
            if (slot[i].transform.childCount > 0)
            {
                if (slot[i].transform.GetChild(0).GetComponent<ItemHandler>().item.ID == hitInfo.collider.gameObject.GetComponent<AddTag>().ID)
                {
                    slot[i].transform.GetChild(0).GetComponent<ItemHandler>().item.amount++;

                    completed = true;
                    break;
                }
            }
        }


        //2.
        //Loop that creates a new item on an empty slot in inventory
        if (completed == false)
        {
            for (int i = 0; i < slot.Length; i++)
            {
                if (slot[i].GetComponent<Slot>().isEmpty == true)
                {

                    Instantiate(itemTexture, slot[i].transform);

                    initItem(i, hitInfo.collider.GetComponent<AddTag>().ID); //initializes  ItemData into an EmptySlot

                    slot[i].GetComponent<Slot>().isEmpty = false;
                    
                    break;
                }
            }
        }
    }



    public void addItemToInventoryById(int ID)
    {
        bool completed = false; // Variable for checking if the item already exists

        //Loop that checks if item is already avaliable in Inventory
        for (int i = 0; i < slot.Length; i++)
        {
            if (slot[i].transform.childCount > 0)
            {
                if (slot[i].transform.GetChild(0).GetComponent<ItemHandler>().item.ID == ID)
                {
                    slot[i].transform.GetChild(0).GetComponent<ItemHandler>().item.amount++;

                    completed = true;
                    break;
                }

            }
        }
        //Loop that creates a new item on an empty slot
        if (completed == false)
        {
            for (int i = 0; i < slot.Length; i++)
            {
                if (slot[i].GetComponent<Slot>().isEmpty == true)
                {

                    Instantiate(itemTexture, slot[i].transform);

                    initItem(i, ID); //initializes  ItemData into an EmptySlot

                    slot[i].GetComponent<Slot>().isEmpty = false;

                    break;
                }
            }
        }
    }

    /*
     *Initilizes an Item with the Info from Pickup(Raycast):
     *assigns the values of the item type that got picked up by the Player
     *to the new item GameObject
     */
    public void initItem(int i, int ID){
        
        
        Item item = new Item();

        ItemList itemlist = FindObjectOfType<ItemList>();

        item.amount = 1;
        item.title = itemlist.itemList[ID].name;
        item.ID = itemlist.itemList[ID].ID;
        item.description = itemlist.itemList[ID].Description;
        item.icon = itemlist.itemList[ID].icon;
        item.toolLevel = itemlist.itemList[ID].ToolLevel;

        slot[i].GetComponent<Slot>().transform.GetChild(0).GetComponent<ItemHandler>().item = item;
        updateInventory();
    }

    public void mine(RaycastHit hit)
    {
        if (locked)
        {
            int hardness = 10;

            objectBeingMined = hit.collider.gameObject;

            if (objectBeingMined != objectLastMined && objectBeingMined.CompareTag("Mineable"))
            {
                pickaxeHits = 0;
                objectLastMined = objectBeingMined;
            }
            if (objectBeingMined.CompareTag("Mineable"))
            {
                hardness = objectBeingMined.GetComponent<Regenerate>().oreLevel - toolLevel;

                if(hardness <= 5)
                {
                    Instantiate(mineEffect, hit.point, transform.rotation);
                    pickaxeHits++;
                    objectLastMined = objectBeingMined;
                }

            }
            
            

            if (pickaxeHits >= hardness)
            {
                if (FindObjectOfType<PointAndClickEvent>().checkRay())
                {
                    if (hit.collider.gameObject.tag == "Mineable" && hit.distance < playerReach)
                    {
                        Vector3 dropPoint = hit.collider.transform.position;
                        Quaternion dropAngle = hit.collider.transform.rotation;
                        GameObject objectWhenMined = hit.collider.gameObject.GetComponent<AddTag>().objectWhenMined;

                        hit.collider.gameObject.GetComponent<MeshRenderer>().enabled = false;
                        hit.collider.gameObject.GetComponent<MeshCollider>().enabled = false;
                        hit.collider.gameObject.GetComponent<Regenerate>().mined = true;
                        Instantiate(objectWhenMined, dropPoint, dropAngle);
                        pickaxeHits = 0;
                    }
                }
            }
            
        }
    }

    public void pickUp(RaycastHit hit)
    {
        if (locked)
        {
            if (FindObjectOfType<PointAndClickEvent>().checkRay())
            {
                if (hit.collider.gameObject.tag == "PickUp" && hit.distance < playerReach)
                {
                    Destroy(hit.collider.gameObject);
                    addItemToInventoryByRaycast(hit);
                }
            }
        }
    }
    public void drop()
    {
        Vector3 dropPoint = dropPointObject.transform.position;

        Instantiate(prefab, dropPoint, Quaternion.identity.normalized);
    }

    //Loops trough Every Item needed trough all Item Slots and returns true if this is the case
    public bool scanInventory(int[,] craftingCost)
    {
        bool[] containsEnough = new bool[6];
        int materialsPlayerHasEnoughOf = 0;
        int amountOfMaterials = 0;
        
        //Reads the Recipe and saves how many materials are needed
        for(int i = 0; i < craftingCost.Length/2; i++)
        {
            if (craftingCost[i, 1] > 0)
            {
                amountOfMaterials++;
            }
        }

        //Loops once for every material
        for (int c = 0; c < amountOfMaterials; c++)
        {
            //loops once for every slot
            int amount = 0;
            for (int i = 0; i < slot.Length; i++)
            {
                //Checks if the Slot contains an Item   
                if(slot[i].transform.childCount > 0)
                {
                    //Checks the Slot for the Item ID and if it is identical with the ID of the recipe material
                    //If this returns true it is added to the amount variable at the end it checks if the player has enough material
                    if (slot[i].transform.GetChild(0).GetComponent<ItemHandler>().item.ID == craftingCost[c, 0])
                    {
                        amount += slot[i].transform.GetChild(0).GetComponent<ItemHandler>().item.amount;
                    }
                }
            }
            //Checks if the player has enough of every material --> sets the containsEnough variable to true if that is the case
            if(amount >= craftingCost[c, 1] && craftingCost[c, 1] != 0)
            {
                containsEnough[c] = true;
            }
        }
        for (int i = 0; i < containsEnough.Length; i++)
        {
            if(containsEnough[i])
            {
                materialsPlayerHasEnoughOf++;
            }
        }
        //If the player has all the materials that are needed this function returns true
        if(materialsPlayerHasEnoughOf == amountOfMaterials)
        {
            return true;
        }
        
        return false;
    }

    //"uses" every item needed by the receipe
    //should only be used if scanIventory returns true
    public void useItemForCrafting(int[,] craftingCost)
    {
        int amountOfMaterials = 0;
        //Reads the Recipe and saves how many materials are needed
        for (int i = 0; i < craftingCost.Length / 2; i++)
        { 
            if (craftingCost[i, 1] > 0)
            {
                amountOfMaterials++;
            }
        }

        for (int c = 0; c < amountOfMaterials ; c++)
        {
            int usedItemAmount = 0;
            for (int i = 0; i < slot.Length; i++)
            {
                if (slot[i].transform.childCount > 0)
                {
                    Item item = slot[i].transform.GetChild(0).GetComponent<ItemHandler>().item;
                    if (item.ID == craftingCost[c, 0])
                    {
                        //Deletes item, lowers the cost and searches for more Items
                        if (item.amount < craftingCost[c, 1] - usedItemAmount)
                        {
                            usedItemAmount += item.amount;
                            Destroy(slot[i].transform.GetChild(0).gameObject);
                        }
                        //Deletes the item and breaks the loop 
                        else if(item.amount == craftingCost[c, 1]- usedItemAmount)
                        {
                            Destroy(slot[i].transform.GetChild(0).gameObject);
                            break;
                        }
                        //Subtracts the amount from the item and breaks the loop
                        else if(item.amount > craftingCost[c, 1]- usedItemAmount)
                        {
                            slot[i].transform.GetChild(0).GetComponent<ItemHandler>().item.amount -= craftingCost[c, 1]- usedItemAmount;
                            break;
                        }

                        //If enough smaller item stacks got used this breaks the loop for the first if clause
                        if(craftingCost[c, 1] - usedItemAmount == 0)
                        {
                            break;
                        }

                    }
                }
            }
        }
    }

    public void updateEquip()
    {
        if(toolSlot.transform.childCount > 0)
        {
            toolLevel = toolSlot.transform.GetChild(0).GetComponent<ItemHandler>().item.toolLevel;
        }
        else
        {
            toolLevel = 0;
        }
    }

    public void updateInventory()
    {
        for (int i = 0; i < allSlots + hotBarSlots; i++)
        {
            Transform transform = slot[i].GetComponent<Slot>().transform;

            if (transform.childCount > 0)
            {
                transform.GetChild(0).GetComponent<Image>().sprite = transform.GetChild(0).GetComponent<ItemHandler>().item.icon;
                transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = transform.GetChild(0).GetComponent<ItemHandler>().item.amount.ToString();
            }

        }
    }
}
