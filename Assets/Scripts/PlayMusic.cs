using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayMusic : MonoBehaviour
{
    public string state = "STOP"; // 控制现在播放
    public bool isEnd = false;// 判断音乐是否播放完毕，由按钮传入
    public bool isStop = false;// 判断是否按停止键
    public bool isLoop = false;// 判断是否在循环过程中，避免重复
    private int playOrder = 0;// 判断随机播放（0）还是顺序播放（1）
    private int sentenceNow = 0;// 判断顺序播放时当前句子
    private GameObject selectObject;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isEnd == true)
        {
            isEnd = false;
            
            if(isStop == true)
            {
                state = "STOP";
                isLoop = false;
            }else if(state == "RANDOM" && isLoop == false)
            {
                isLoop = true;
                playOrder = 0;
                selectObject = GameObject.Find("NEVER");
            }else if(state == "PLAY" && isLoop == false)
            {
                isLoop = true;
                playOrder = 1;
                sentenceNow = 0;
                selectObject = GameObject.Find("NEVER");
            }else if(state == "NEVER")
            {
                selectObject = GameObject.Find("GONNA");
            }else if(state == "GONNA")
            {
                if(playOrder == 1)
                {
                    if(sentenceNow == 0)
                    {
                        selectObject = GameObject.Find("GIVE");
                    }else if(sentenceNow == 1)
                    {
                        selectObject = GameObject.Find("LET");
                    }else if(sentenceNow == 2)
                    {
                        selectObject = GameObject.Find("RUN");
                    }else if(sentenceNow == 3)
                    {
                        selectObject = GameObject.Find("MAKE");
                    }else if(sentenceNow == 4)
                    {
                        selectObject = GameObject.Find("SAY");
                    }else if(sentenceNow == 5)
                    {
                        selectObject = GameObject.Find("TELL");
                    }   
                }else if(playOrder == 0)
                {
                    int randomNum = Random.Range(0, 6);
                    if(randomNum == 0)
                    {
                        selectObject = GameObject.Find("SAY");
                    }else if(randomNum == 1)
                    {
                        selectObject = GameObject.Find("RUN");
                    }else if(randomNum == 2)
                    {
                        selectObject = GameObject.Find("TELL");
                    }else if(randomNum == 3)
                    {
                        selectObject = GameObject.Find("MAKE");
                    }else if(randomNum == 4)
                    {
                        selectObject = GameObject.Find("GIVE");
                    }else if(randomNum == 5)
                    {
                        selectObject = GameObject.Find("LET");
                    }
                }
                
            }else if(state == "SAY")
            {
                selectObject = GameObject.Find("GOODBYE");
                sentenceNow = 5; //切换
            }else if(state == "GOODBYE" || state == "DOWN" || state == "CRY" || state == "UP" || state == "YOU1")
            {
                selectObject = GameObject.Find("NEVER");
            }else if(state == "RUN")
            {
                selectObject = GameObject.Find("AROUND");
            }else if(state == "AROUND")
            {
                selectObject = GameObject.Find("AND");
            }else if(state == "AND")
            {
                if(playOrder == 0)
                {
                    int randomNum = Random.Range(0, 2);
                    if(randomNum == 0)
                    {
                        selectObject = GameObject.Find("DESERT");
                    }else if(randomNum == 1)
                    {
                        selectObject = GameObject.Find("HURT");
                    }
                }else if(playOrder == 1)
                {
                    if(sentenceNow == 5)
                    {
                        selectObject = GameObject.Find("HURT");
                        sentenceNow = 0; //切换
                    }else if(sentenceNow == 2)
                    {
                        selectObject = GameObject.Find("DESERT");
                        sentenceNow = 3; //切换
                    }
                }
            }else if(state == "DESERT" || state == "HURT")
            {
                selectObject = GameObject.Find("YOU1");
            }else if(state == "TELL")
            {
                selectObject = GameObject.Find("ALIE");
            }else if(state == "ALIE")
            {
                selectObject = GameObject.Find("AND");
            }else if(state == "LET")
            {
                selectObject = GameObject.Find("YOU2");
            }else if(state == "YOU2")
            {
                selectObject = GameObject.Find("DOWN");
                sentenceNow = 2; //切换
            }else if(state == "GIVE" || state == "MAKE")
            {
                selectObject = GameObject.Find("YOU");
            }else if(state == "YOU")
            {
                if(playOrder == 0)
                {
                    int randomNum = Random.Range(0, 2);
                    if(randomNum == 0)  
                    {
                        selectObject = GameObject.Find("CRY");
                    }else if(randomNum == 1)
                    {
                        selectObject = GameObject.Find("UP");
                    }
                }else if(playOrder == 1)
                {
                    if(sentenceNow == 0)
                    {
                        selectObject = GameObject.Find("UP");
                        sentenceNow = 1; //切换
                    }else if(sentenceNow == 3)
                    {
                        selectObject = GameObject.Find("CRY");
                        sentenceNow = 4; //切换
                    }
                }
            }
            if(isStop == false)
            {
                selectObject.GetComponent<Button>().onClick.Invoke();
                selectObject.GetComponent<Button>().GetComponent<Image>().color = Color.red;
                selectObject.GetComponent<ButtonJudge>().isRunning = true;
            }
            
        }
        
    }
}
