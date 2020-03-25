using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySelf : MonoBehaviour
{
    public int numberOfSecondsBeforeDestruction = 3;
    void Start()
    {
        Destroy(this.gameObject, numberOfSecondsBeforeDestruction);
    }

}
