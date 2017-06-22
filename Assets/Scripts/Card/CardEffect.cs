using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class CardEffect : MonoBehaviour {
    float alfa = 0.0f;          //透明度
    float redl, greenl, bluel;  //左絵の原色
    float redr, greenr, bluer;  //右絵の原色
    float Fspeed = 0.025f;       //フェード速度
    bool fadeState = false;     //フェードフラグ
    bool counfFlag = false;     // カウントフラグ
    int fadeCount = 0;          //フェード回数カウント用

    [SerializeField]
    private GameObject StarL;

    [SerializeField]
    private GameObject StarR;

    Vector2 tmp;                //対象の初期位置取得用
    Vector2 pos;                //対象の現在位置取得用

    //カードマネジメント
    CardManagement cardmane;
    //ゲームマネジメント
    GameManager gamemane;
    //カードボード
    CardBord cardBord;
    //チェンジボード
    SetButton button;

    // Use this for initialization
    void Start()
    {
        redl = StarL.GetComponent<Image>().color.r;     //R
        greenl = StarL.GetComponent<Image>().color.g;   //G
        bluel = StarL.GetComponent<Image>().color.b;    //B
        redr = StarR.GetComponent<Image>().color.r;     //R
        greenr = StarR.GetComponent<Image>().color.g;   //G
        bluer = StarR.GetComponent<Image>().color.b;    //B

        cardmane = GameObject.Find("CardManager").GetComponent<CardManagement>();
        gamemane = GameObject.Find("GameManager").GetComponent<GameManager>();
        cardBord = GameObject.Find("ActionBord").GetComponent<CardBord>();
        //button = GameObject.Find("ToCardSetButton").GetComponent<SetButton>();
        tmp = gameObject.transform.position;
        
    }

    // Update is called once per frame
    void Update()
    {
        //画像の色を設定
        StarL.GetComponent<Image>().color = new Color(redl, greenl, bluel, alfa);
        StarR.GetComponent<Image>().color = new Color(redr, greenr, bluer, alfa);

        pos = gameObject.transform.position;

        //各種ボタンを押していなかったら
        if (!gamemane.GetFlag() || !cardBord.GetFlag())
        {
            if (pos == tmp)
            {
                //カードのエフェクトを発生させる
                StarEffect();
            }
        }
        if (gamemane.GetFlag() || cardBord.GetFlag())
        {
            alfa = -1.0f;
        }
    }

    void StarEffect()
    {
        if (cardmane.GetReleaseFlag())
        {
            counfFlag = true;
        }
        else if(!cardmane.GetReleaseFlag())
        {
            alfa = -1.0f;
            counfFlag = false;
        }

        if (counfFlag)
        {
            //エフェクト表示
            if (alfa == 0.0f) 
            {
                alfa = 1.0f;
            }

            //フェードアウト
            alfa -= Fspeed;
            if (alfa <= 0.0f)
            {
                Fspeed = 0.0f;
            }
        }          
    }
}
