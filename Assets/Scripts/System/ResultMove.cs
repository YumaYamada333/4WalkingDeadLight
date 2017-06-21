using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultMove : MonoBehaviour
{
    GameObject GameOver;
    GameObject GameClear;
    GameObject SelectButton;
    GameObject RetryButton;
    //タイトルを動かすための始点と終点と時間
    Vector3 resultStartPos = new Vector3(0, 50, 0);
    Vector3 resultEndPos = new Vector3(0, 350, 0);

    //タイトルを動かすための始点と終点と時間
    Vector3 selectStartPos = new Vector3(-90, -103, 0);
    Vector3 selectEndPos = new Vector3(-90, -400, 0);

    Vector3 C_selectStartPos = new Vector3(0, -103, 0);
    Vector3 C_selectEndPos = new Vector3(0, -400, 0);

    Vector3 retryStartPos = new Vector3(90, -103, 0);
    Vector3 retryEndPos = new Vector3(90, -400, 0);

    private float titleTime;
    float timeStep;

    //移動させるためのフラグ
    bool ResultMoveFlag;

    //ToResultSceneのフラグを返す関数を何とかした
    ToResultScene ClearFunc;
    ToResultScene OverFunc;

    bool ClearFlag;
    bool OverFlag;
    bool isStartLerp;
    bool SoundFlag;

    public AudioClip CrearSound;
    public AudioClip OverSound;

    AudioSource audioSource;
    // Use this for initialization
    void Start()
    {
        GameOver = GameObject.Find("OVER");
        GameClear = GameObject.Find("CLEAR");
        ClearFunc = GameObject.Find("GameManager").GetComponent<ToResultScene>();
        OverFunc = ClearFunc;
        timeStep = 0;
        ResultMoveFlag = false;
        titleTime = Time.time;
        isStartLerp = false;
        SoundFlag = true;
        // 初期化がSceneLoadScriptで正常に出来ないため
        SceneLoadScript.MoveFlag = false;

        audioSource =GameObject.Find("GimmickAudio") .GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

        ResultMoveFlag = SceneLoadScript.MoveFlag;
        ClearFlag = ClearFunc.Clearflag();
        OverFlag = ClearFunc.Overflag();
        if (!isStartLerp)
        {
            titleTime = Time.time;
        }
        //クリアしたら通す
        if (ClearFlag)
        {
            SelectButton = GameObject.Find("SelectButton");

            if (SoundFlag)
            {
                GetComponent<AudioSource>().Stop();
                audioSource.PlayOneShot(CrearSound);
                SoundFlag = false;
            }
            //OnClickで関数を呼んで切り替え
            if (ResultMoveFlag == true)
            {
                timeStep = (Time.time - titleTime) / 0.5f;
                //ゲームクリアの移動
                GameClear.transform.localPosition = MathClass.Lerp(resultStartPos, resultEndPos, timeStep);
                SelectButton.transform.localPosition = MathClass.Lerp(C_selectStartPos, C_selectEndPos, timeStep);
                if (Input.GetMouseButtonUp(0))
                {
                    isStartLerp = true;
                    timeStep = 0;
                    titleTime = Time.time;
                    ClearFunc.isUpdate = false;
                    ClearFlag = false;

                }
            }
        }
        //ゲームオーバーしたら
        if (OverFlag)
        {
            SelectButton = GameObject.Find("SelectButton");
            RetryButton = GameObject.Find("TitleButton");

            if (SoundFlag)
            {
                GetComponent<AudioSource>().Stop();
                audioSource.PlayOneShot(OverSound);
                SoundFlag = false;
            }

            //OnClickで関数を呼んで切り替え
            if (ResultMoveFlag == true)
            {
                timeStep = (Time.time - titleTime) / 0.5f;
                if (timeStep > 1.0f) timeStep = 1.0f;
                //ゲームオーバーの移動
                GameOver.transform.localPosition = MathClass.Lerp(resultStartPos, resultEndPos, timeStep);
                SelectButton.transform.localPosition = MathClass.Lerp(selectStartPos, selectEndPos, timeStep);
                RetryButton.transform.localPosition = MathClass.Lerp(retryStartPos, retryEndPos, timeStep);
                if (Input.GetMouseButtonUp(0))
                {
                    isStartLerp = true;
                    timeStep = 0;
                    titleTime = Time.time;
                    OverFunc.isUpdate = false;
                    OverFlag = false;
                }
            }
        }
    }
}
