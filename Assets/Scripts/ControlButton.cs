using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ClickButton()
    {
        GameObject.Find("EventSystem").GetComponent<PlayMusic>().state = this.gameObject.name;
        GameObject.Find("EventSystem").GetComponent<PlayMusic>().isEnd = true;
        if(this.gameObject.name == "EXIT")
        {
            Application.Quit();
        }else if(this.gameObject.name == "STOP")
        {
            GameObject.Find("EventSystem").GetComponent<PlayMusic>().isStop = true;
        }else
        {
            GameObject.Find("EventSystem").GetComponent<PlayMusic>().isStop = false;
        }
    }
}
