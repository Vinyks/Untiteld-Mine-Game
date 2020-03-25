using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointAndClickEvent : MonoBehaviour
{
    public Camera kamera;
    RaycastHit hit;

    public RaycastHit castRay() // casts a Ray, only hits Layer:MineAble
    {
        Ray ray = kamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {  
            return hit;            
        }
        return hit;
    }
    public bool checkRay() //Returns true if a layer is hit, false if it didin't hit anyting 
    {
        Ray ray = kamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
