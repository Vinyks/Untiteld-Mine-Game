using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayTimeSim : MonoBehaviour
{
    GameObject Light;
    float rotation;
    public float speed = 1;
    void Start()
    {
        Light = this.gameObject;
    }
    void Update()
    {
        Light.transform.localRotation = Quaternion.Euler(rotation, 0f,0f);
        rotation += speed;
    }
}
