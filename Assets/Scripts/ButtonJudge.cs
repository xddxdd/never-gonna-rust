using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonJudge : MonoBehaviour
{
    public bool isRunning = false;
    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isRunning == true)
        {
            if(!audioSource.isPlaying)
            {
                this.gameObject.GetComponent<Button>().GetComponent<Image>().color = Color.white;
                isRunning = false;
                GameObject.Find("EventSystem").GetComponent<PlayMusic>().state = this.gameObject.name;
                GameObject.Find("EventSystem").GetComponent<PlayMusic>().isEnd = true;
                
            }
        }
    }
}
