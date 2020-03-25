using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InitRecipies : MonoBehaviour
{
    public GameObject recipieGameObject;
    Recipies[] recipies;
    void Start()
    {
        recipies = FindObjectOfType<ReciepeList>().recipies;

        for(int i = 0; i < recipies.Length; i++)
        {
            int[,] craftingCost = new int[6,2];
            Instantiate(recipieGameObject, transform);
            transform.GetChild(i).GetComponent<Craft>().ItemID = recipies[i].ID;
            //PictureHolder
            //Text
            transform.GetChild(i).GetChild(0).GetChild(0).GetComponent<Text>().text = recipies[i].name;
            //Picture

            //Add code for picture implementation here !!!!!!!!!!!!!

            int numberOfMaterials = recipies[i].material.Length;
            for (int b = 0; b < 2; b++)
            {
                for(int c = 0; c < 3; c++)
                {
                    if(c + (b * 3) < numberOfMaterials)
                    {
                        transform.GetChild(i).GetChild(b+1).GetChild(c).GetChild(0).GetComponent<Text>().text = FindObjectOfType<ItemList>().itemList[recipies[i].material[c + (b * 3)].ID].name;
                        transform.GetChild(i).GetChild(b+1).GetChild(c).GetChild(1).GetComponent<Text>().text = recipies[i].material[c + (b * 3)].number.ToString();
                        craftingCost[c + (b * 3), 0] = recipies[i].material[c + (b * 3)].ID;
                        craftingCost[c + (b * 3), 1] = recipies[i].material[c + (b * 3)].number;
                    }
                    else
                    {
                        Destroy(transform.GetChild(i).GetChild(b + 1).GetChild(c).gameObject);
                    }
                    
                }
                transform.GetChild(i).GetComponent<Craft>().craftingCost = craftingCost;
                
            }

        }
    }
}
