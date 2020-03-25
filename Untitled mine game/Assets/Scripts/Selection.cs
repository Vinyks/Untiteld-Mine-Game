using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Selection : MonoBehaviour
{
    public Sprite pickUpCursor;
    public Sprite mineableCursor;
    public Sprite emptySprite;

    void Update()
    {
        if(FindObjectOfType<PointAndClickEvent>().checkRay())
        {
            RaycastHit hit = FindObjectOfType<PointAndClickEvent>().castRay();
            

            if(hit.distance < InventoryManager.playerReach)
            {
                if (hit.collider.tag == "PickUp")
                {
                    this.GetComponent<Image>().sprite = pickUpCursor;
                }
                else if (hit.collider.tag == "Mineable")
                {
                    this.GetComponent<Image>().sprite = mineableCursor;
                }
                else
                {
                    this.GetComponent<Image>().sprite = emptySprite;
                }
            }
            else
            {
                this.GetComponent<Image>().sprite = emptySprite;
            }
        }
    }
}
