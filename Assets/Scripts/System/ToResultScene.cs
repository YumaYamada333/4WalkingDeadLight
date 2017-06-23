using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ToResultScene : MonoBehaviour {

    GameObject player;
    GameObject GameOver;
    GameObject GameClear;

    //"GAME OVER"と"CLEAR"を動かすための始点と終点と時間
    Vector3 resultStartPos = new Vector3(0, 350, 0);
    Vector3 resultEndPos = new Vector3(0, 15, 0);
    private float resultTime;
    float timeStep;
    bool OverFlag = false;
    bool ClearFlag = false;
    public bool isUpdate = true;

    //パーティクル継続時間計測用
    private float WaiteTime;
    //パーティクル継続時間
    const float ParticleWaite = 2.0f;

    public ParticleSystem Confetti;

    public enum OverType
    {
        NONE,
        FALL,
    }

	// Use this for initialization
	void Start () {
        player = GameObject.Find("unitychan");
        GameOver = GameObject.Find("OVER");
        GameClear = GameObject.Find("CLEAR");
        timeStep = 0;
        resultTime = Time.time + 50;
        WaiteTime = 0.0f;

        GameOver.transform.localPosition = new Vector3(0, 350, 0);
        GameClear.transform.localPosition = new Vector3(0, 350, 0);

        GameOver.transform.localScale = new Vector3(5, 2.5f, 1);
        GameClear.transform.localScale = new Vector3(5, 2.5f, 1);
        isUpdate = true;
        Confetti.Stop();


    }

    // Update is called once per frame
    void Update () {
        if (!isUpdate) return;
        if (OverFlag)
        {
            //パーティクル継続時間計測
            WaiteTime += 0.1f;
            if(WaiteTime>=ParticleWaite)
            {
                player.GetComponent<PlayerAction>().particleType = (int)PARTICLE.NONE;        //パーティカルの種類決定
            }
            timeStep = (Time.time - resultTime) / 0.3f;
            if (timeStep > 1.0f) timeStep = 1.0f;
            GameOver.transform.localPosition = MathClass.Lerp(resultStartPos, resultEndPos, timeStep);
        }
        if(ClearFlag)
        {
            timeStep = 0;

            timeStep = (Time.time - resultTime) / 0.3f;
            if (timeStep > 1.0f) timeStep = 1.0f;
            GameClear.transform.localPosition = MathClass.Lerp(resultStartPos, resultEndPos, timeStep);
        }
    }

    public void ToClear(int waitTime = 0)
    {
        resultTime = Time.time;

        if (!player.GetComponent<Animator>().GetBool("Clear"))
            player.GetComponent<PlayerAction>().AnimationStop();
        player.GetComponent<Animator>().SetBool("Clear", true);
        player.GetComponent<PlayerAction>().enabled = false;
        Confetti.Play();

        ClearFlag = true;

        // クリア情報の書き込み
        ClearSave();
    }

    public void ToOver(int waitTime = 0, OverType type = OverType.FALL)
    {
        // 演出処理
        switch (type)
        {
            case OverType.FALL:
                resultTime = Time.time;
                player.GetComponent<PlayerAction>().AnimationStop();
                player.GetComponent<Animator>().SetBool("Over", true);
                player.GetComponent<PlayerAction>().enabled = false;
                OverFlag = true;
                break;

            default:

                break;
        }
        resultTime = Time.time;
        //Invoke("ToOverScene", waitTime);
    }

    void ClearSave()
    {
        PlayerPrefs.SetInt(Application.loadedLevelName, 1);
    }
    public bool Clearflag()
    {
        return ClearFlag;
    } 

    public bool Overflag()
    {
        return OverFlag;
    }
}
