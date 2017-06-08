using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultMove : MonoBehaviour
{

    GameObject GameOver;
    GameObject GameClear;
    //タイトルを動かすための始点と終点と時間
    Vector3 resultStartPos = new Vector3(0, 50, 0);
    Vector3 resultEndPos = new Vector3(0, 350, 0);

    private float titleTime;
    float timeStep;

    //移動させるためのフラグ
    bool ResultMoveFlag;

    //ToResultSceneのフラグを返す関数を何とかした
    ToResultScene ClearFunc;
    ToResultScene OverFunc;

    bool ClearFlag;
    bool OverFlag;
    // Use this for initialization
    void Start()
    {
        GameOver = GameObject.Find("OVER");
        GameClear = GameObject.Find("CLEAR");
        timeStep = 0;
        titleTime = Time.time;

    }

    // Update is called once per frame
    void Update()
    {
        //    ResultMoveFlag = SceneLoadScript.MoveFlag;
        //    ClearFlag = ClearFunc.Clearflag();
        //    OverFlag = OverFunc.Overflag();

        //    //クリアしたら通す
        //    if (ClearFlag)
        //    {
        //        //OnClickで関数を呼んで切り替え
        //        if (ResultMoveFlag == true)
        //        {
        //            timeStep = (Time.time - titleTime) / 1.5f;
        //            //ゲームクリアの移動
        //            GameClear.transform.localPosition = MathClass.Lerp(resultStartPos, resultEndPos, timeStep);
        //            if (Input.GetMouseButtonUp(0))
        //            {
        //                timeStep = 0;
        //                titleTime = Time.time;
        //                ClearFlag = false;
        //            }
        //        }
        //    }
        //    //ゲームオーバーしたら
        //    if (OverFlag)
        //    {
        //         //OnClickで関数を呼んで切り替え
        //         if (ResultMoveFlag == true)
        //        {
        //            timeStep = (Time.time - titleTime) / 1.5f;
        //            //ゲームオーバーの移動
        //            GameOver.transform.localPosition = MathClass.Lerp(resultStartPos, resultEndPos, timeStep);
        //            if (Input.GetMouseButtonUp(0))
        //            {
        //                timeStep = 0;
        //                titleTime = Time.time;
        //                OverFlag = false;
        //            }
        //        }
        //    }
    }
}
