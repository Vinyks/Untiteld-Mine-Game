using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Item item = new Item();
    public static GameObject itemBeingDragged;
    Vector3 startPosition;
    Transform startParent;

    

    public void OnBeginDrag(PointerEventData eventData)
    {
        itemBeingDragged = gameObject;
        startPosition = transform.position;
        startParent = transform.parent;
        GetComponent<CanvasGroup>().blocksRaycasts = false;

        transform.SetParent(transform.root);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition; //Attaches the dragged Image to the cursor
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        itemBeingDragged = null;

        
        if(transform.parent == startParent || transform.parent == transform.root)
        { 
            transform.position = startPosition; //Returns the Item to the old position if the Parent didn't change
            transform.SetParent(startParent);

        }
        if(transform.parent != startParent)
        { 
            this.transform.parent.gameObject.GetComponent<Slot>().isEmpty = false; //Sets IsEmpty to false on the onto dragged Slot

        }
        
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        FindObjectOfType<InventoryManager>().updateEquip();
    }

    





}
