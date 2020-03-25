using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemList : MonoBehaviour
{
    public itemlist[] itemList;
}
[System.Serializable]
public struct itemlist
{
    public int ID;
    public string name;
    public string Description;
    public int ToolLevel;
    public Sprite icon;
}
