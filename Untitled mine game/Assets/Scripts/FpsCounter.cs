using UnityEngine;
using UnityEngine.UI;

public class FpsCounter : MonoBehaviour
{
    Text text;
    double time = 0;
    int frames = 0;
    void Start()
    {
        text = GetComponent<Text>();
    }
    void Update()
    {
        frames++;
        time += Time.deltaTime;

        if(time >= 1)
        {
            text.text = ("Fps: "+frames.ToString());
            frames = 0;
            time = 0;
        }
    }
}
