using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class CardEffect : MonoBehaviour {
    float alfa = 1.0f;          //透明度
    float Fspeed = 0.02f;       //フェード速度
    float red, green, blue;     //原色
    bool fadeState = false;     //フェードフラグ
    bool counfFlag = false;     // カウントフラグ
    int fadeCount = 0;          //フェード回数カウント用

    Vector2 tmp;                //対象の初期位置取得用
    Vector2 pos;                //対象の現在位置取得用

    //カードマネジメント
    CardManagement cardmane;

    // Use this for initialization
    void Start()
    {
        red = GetComponent<RawImage>().color.r;     //R
        green = GetComponent<RawImage>().color.g;   //G
        blue = GetComponent<RawImage>().color.b;    //B

        cardmane = GameObject.Find("CardManager").GetComponent<CardManagement>();
        tmp = gameObject.transform.position;
        
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<RawImage>().color = new Color(red, green, blue, alfa);
        pos = gameObject.transform.position;


        if (pos == tmp)
        {
            SecondFadeInAndOut();
        }
    }

    void SecondFadeInAndOut()
    {
        if (cardmane.GetReleaseFlag())
        {
            if (fadeCount <= 2)
            {
                counfFlag = true;
            }
        }

            if (counfFlag)
            {
                if (!fadeState)
                {
                    //フェードイン
                    alfa += Fspeed;
                    if (alfa >= 1.0f)
                    {
                        fadeState = true;
                        fadeCount++;
                        counfFlag = false;
                    }
                }
                else
                {
                    //フェードアウト
                    alfa -= Fspeed;
                    if (alfa <= 0.5f)
                    {
                        fadeState = false;
                    }
                }
            }
        
    }

}
