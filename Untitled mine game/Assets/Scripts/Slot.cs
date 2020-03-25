using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IDropHandler
{
    
    public GameObject itemDrag
    {
        get
        {
            if (transform.childCount > 0)
            {
                return transform.GetChild(0).gameObject;
            }

            return null;
        }
    }

    public bool isEmpty = true;

    #region IdropHandler implementation
    public void OnDrop(PointerEventData eventData)
    {
        if (!itemDrag)
        {
            ItemHandler.itemBeingDragged.transform.SetParent(transform);//Changes the parent of the Image to the slot it got dropped on
        }
        else if(itemDrag)
        {
            if(this.GetComponent<Slot>().transform.GetChild(0).GetComponent<ItemHandler>().item.ID == this.GetComponent<Slot>().transform.parent.parent.parent.GetChild(this.GetComponent<Slot>().transform.parent.parent.parent.childCount - 1).GetComponent<ItemHandler>().item.ID)
            {
                //Merges Both ItemValues and applies it to the one Dragged onto
                this.GetComponent<Slot>().transform.GetChild(0).GetComponent<ItemHandler>().item.amount += this.GetComponent<Slot>().transform.parent.parent.parent.GetChild(this.GetComponent<Slot>().transform.parent.parent.parent.childCount - 1).GetComponent<ItemHandler>().item.amount;
                Destroy(this.GetComponent<Slot>().transform.parent.parent.parent.GetChild(this.GetComponent<Slot>().transform.parent.parent.parent.childCount - 1).gameObject);
            } 
        }
        this.gameObject.GetComponentInParent<Slot>().isEmpty = false;
    }
    #endregion

    void Update()
    {
        //Updates the is Empty Variable based on the Children of the Slot
        if(gameObject.transform.childCount == 0)
        {
            isEmpty = true; 
            
        }
        if(gameObject.transform.childCount > 0)
        {
            isEmpty = false;
        }
    }

}