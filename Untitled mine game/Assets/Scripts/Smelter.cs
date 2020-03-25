using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smelter : MonoBehaviour
{
    public GameObject smelterObject;
    public GameObject moltenGold;
    public float SmeltTimeInSeconds = 5;
    int StoredGoldOre = 0;
    double timer = 0;
    
    void OnCollisionEnter(Collision collisionInfo)
    {
        {
            if (FindObjectOfType<SmeltableTag>().Smeltable_Tag)
            {
                Destroy(collisionInfo.gameObject);
                StoredGoldOre++;
            }
        }
    }

    void Update()
    {
        if (StoredGoldOre > 0)
        { 
            timer += Time.deltaTime;
            if (timer >= SmeltTimeInSeconds)
            {
                StoredGoldOre--;
                timer = 0;
                Instantiate(moltenGold, smelterObject.transform.position, Quaternion.identity);
            }
        }
    }
}
