using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReciepeList : MonoBehaviour
{
    public Recipies[] recipies;    
}
[System.Serializable]
public struct Recipies
{
    public string name;
    public int ID;
    public int tier;
    public material[] material;
}

[System.Serializable]

public struct material
{
    public int ID;
    public int number;
}