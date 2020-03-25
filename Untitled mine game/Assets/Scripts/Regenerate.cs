using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Regenerate : MonoBehaviour
{
    public int oreLevel;
    float counter;
    float counterPar;
    public bool mined = false;
    MeshRenderer mesh;
    MeshCollider coll;

    void Start()
    {
        mesh = this.GetComponent<MeshRenderer>();
        coll = this.GetComponent<MeshCollider>();
    }

    void Update()
    {
        if(mined)
        {
            counter += 1 * Time.deltaTime;
            if(counter >= 10)
            {
                mesh.enabled = true;
                coll.enabled = true;
                counter = 0;
            }
        }

    }

}
