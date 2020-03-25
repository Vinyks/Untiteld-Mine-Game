using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabList : MonoBehaviour
{
    public List[] list;
}
[System.Serializable]
public struct List
{
    public int ID;
    public Object prefab;
}
