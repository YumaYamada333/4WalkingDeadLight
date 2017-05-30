using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIFead : MonoBehaviour {
    float alfa;                 //透明度
    float Fspeed = 0.02f;       //フェード速度
    float apper = 1.0f;         //表示
    float disapper = 0.0f;      //非表示
    float red, green, blue;     //原色
    bool fadeState = false;     //フェードフラグ

    //クリックシーン
    private ClickSceneLoad ClickScene;

	// Use this for initialization
	void Start () {
        red = GetComponent<RawImage>().color.r;     //R
        green = GetComponent<RawImage>().color.g;   //G
        blue = GetComponent<RawImage>().color.b;    //B

        ////コンポーネント取得
        //ClickScene = GameObject.Find("ClickSystem").GetComponent<ClickSceneLoad>();
	}
	
	// Update is called once per frame
	void Update () {
        GetComponent<RawImage>().color = new Color(red, green, blue, alfa);

        FadeInAndOut();
    }

    //フェードインとフェードアウトする関数
    void FadeInAndOut()
    {
        if (!fadeState)
        {
            //フェードイン
            alfa += Fspeed;
            if (alfa >= 1.0f)
            {
                fadeState = true;
            }
        }
        else
        {
            //フェードアウト
            alfa -= Fspeed;
            if (alfa <= 0.0f)
            {
                fadeState = false;
            }
        }
    }

    ////点滅する関数
    //void Flashing()
    //{
    //    fadeState = false;
    //    if(!fadeState)
    //    {

    //    }

    //}
}
