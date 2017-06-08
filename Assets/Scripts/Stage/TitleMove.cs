using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleMove : MonoBehaviour {

    GameObject TitleImage;
    GameObject StartImage;
    GameObject ExitImage;
    //タイトルを動かすための始点と終点と時間
    Vector3 titleStartPos = new Vector3(-1, 700, 0);
    Vector3 titleEndPos = new Vector3(-1, 46, 0);

    //Startを動かすための始点と終点と時間
    Vector3 ButtonStartPos = new Vector3(0, 520, 0);
    Vector3 ButtonEndPos = new Vector3(0, -180, 0);

    //Exitを動かすための始点と終点と時間
    Vector3 exitStartPos = new Vector3(304, -300, 0);
    Vector3 exitEndPos = new Vector3(304, -166, 0);

    private float titleTime;
    float timeStep;

    //移動させるためのフラグ
    bool MoveFlag = true;
    // Use this for initialization
    void Start () {
        TitleImage = GameObject.Find("TitleImage");
        StartImage = GameObject.Find("StateImage");
        ExitImage = GameObject.Find("EndButton");
        timeStep = 0;
        titleTime = Time.time;
        
    }

    // Update is called once per frame
    void Update () {
        if (MoveFlag)
        {
            timeStep = (Time.time - titleTime) / 1.5f;
            //タイトルの移動
            TitleImage.transform.localPosition = MathClass.Lerp(titleStartPos, titleEndPos, timeStep);
            //スタートの移動
            StartImage.transform.localPosition = MathClass.Lerp(ButtonStartPos, ButtonEndPos, timeStep);
            //exitの移動
            ExitImage.transform.localPosition = MathClass.Lerp(exitStartPos, exitEndPos, timeStep);
            if (Input.GetMouseButtonUp(0))
            {
                timeStep = 0;
                titleTime = Time.time;
                MoveFlag = false;
            }
        }
        else if (MoveFlag == false)
        {
            timeStep = (Time.time - titleTime) / 1.5f;
            //タイトルの移動
            TitleImage.transform.localPosition = MathClass.Lerp(titleEndPos, titleStartPos, timeStep);
            //スタートの移動
            StartImage.transform.localPosition = MathClass.Lerp( ButtonEndPos, ButtonStartPos, timeStep);
            //exitの移動
            ExitImage.transform.localPosition = MathClass.Lerp( exitEndPos, exitStartPos, timeStep);
        }
    }
}
