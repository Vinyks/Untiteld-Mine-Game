using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HotBar : MonoBehaviour
{
    int x;
    public int selectedSlot = 1;

    Color fullGamma = new Color(0,0,0,0.8f);

    Color normalGamma = new Color(0, 0, 0, 0.2f);

    public GameObject[] slot = new GameObject[10];

    void Start()
    {
        //Creates an array entry for every slot
        for (int i = 0; i < 10; i++)
        {
            slot[i] = this.transform.GetChild(i).gameObject;
        }

        //Makes every slot normal colored
        for (int i = 0; i < slot.Length; i++)
        {
            slot[i].GetComponent<Image>().color = normalGamma;
        }
        //Makes the first slot dark on start
        slot[selectedSlot - 1].GetComponent<Image>().color = fullGamma;
    }
    void Update()
    {
        
        //Determines selected slot with keyboard input
        if (int.TryParse(Input.inputString, out x))
        {
            int inputString = int.Parse(Input.inputString);
            
            if (inputString <= 9 && inputString >= 0)
            {
                selectedSlot = int.Parse(Input.inputString);

                //Normally the number equals the index+1 but for 0 its set to 10 so 10-1 equals an Index of 9 wich is the tenth slot
                if (selectedSlot == 0)
                {
                    selectedSlot = 10;
                }
                //Makes the selected slot dark
                slot[selectedSlot - 1].GetComponent<Image>().color = fullGamma;
                
                //Makes evry slot except the selected slot normal
                for (int i = 0; i < slot.Length; i++)
                {
                    if(i != selectedSlot-1)
                    {
                        slot[i].GetComponent<Image>().color = normalGamma;
                    }  
                }
            }
        }

        //Gets ScrollInput
        int scrollDelta = (int)Input.mouseScrollDelta.y;

        //Reverses Scroll Direction
        scrollDelta *= -1;

        //Keeps scrollDelta between -1 and 1
        if (scrollDelta >= 2)
        {
            scrollDelta = 1;
        }
        else if(scrollDelta <= -2)
        {
            scrollDelta = -1;
        }

        
        if (scrollDelta != 0)
        {
            //Keeps the selected slot over 0 and under 11 
            if (selectedSlot != 1 && scrollDelta < 0)
            {
                selectedSlot += scrollDelta;
            }
            else if (selectedSlot != 10 && scrollDelta > 0)
            {
                selectedSlot += scrollDelta;
            }

            //Makes the selected slot dark
            slot[selectedSlot - 1].GetComponent<Image>().color = fullGamma;

            //Makes every slot except the selected slot normal;
            for (int i = 0; i < slot.Length; i++)
                {
                    if (i != selectedSlot - 1)
                    {
                        slot[i].GetComponent<Image>().color = normalGamma;
                    }
                }
            

        }
        


        //Updates textures of evry hotbar slot
        for (int i = 0; i < 10; i++)
        {
            if (slot[i].GetComponent<Slot>().transform.childCount > 0)
            {
                slot[i].GetComponent<Slot>().transform.GetChild(0).GetComponent<Image>().sprite = slot[i].GetComponent<Slot>().transform.GetChild(0).GetComponent<ItemHandler>().item.icon;
                slot[i].GetComponent<Slot>().transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = slot[i].GetComponent<Slot>().transform.GetChild(0).GetComponent<ItemHandler>().item.amount.ToString();
            }

        }
    }
}
