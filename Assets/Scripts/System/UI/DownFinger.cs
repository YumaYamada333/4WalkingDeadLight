using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//時間関連の定数
static class TimeCons
{
    public const float TouchTimeStep = 0.5f;    //タッチ用のtimeStep
    public const float SwipeTimeStep = 1.0f;    //スワイプ用のtimeStep
    public const float WaitTime      = 1.5f;    //待つ時間
    public const float WaitStartTime = 3.0f;    //startTimeの初期化
}
//補完関連の定数
static class LerpPos
{
    public const float VecX            = 130;   //Vector3.x
    public const float StartVecY       = -32;   //スタート地点のVector3.y
    public const float EndVecY         = -138;  //終了地点のVector3.y
    public const float MarginStartVecY = -35;   //余裕を持ったスタート地点のVector3.y    　
    public const float MarginEndVecY   = -136;  //余裕を持った終了地点のVector3.y
    public const float VecZ            = -15;   //Vector3.y
}

//色関連の関数
static class Tinge
{
    public const float DefaultColor = 1.0f;     //基底の色
    public const float MinusAlpha   = 0.01f;    //減算させるアルファブレンド値
}

public class DownFinger : MonoBehaviour {

    // Use this for initialization
    private RectTransform downFinger;                   //指
    private GameObject SSDObj;                          //StageSelectDirectorのobj
    private float waitTime = 0;                         //待たせる時間
    private Vector3 startPos;                           //補完を始める場所
    private Vector3 endPos;                             //補完を終える場所
    private float timeStep = TimeCons.TouchTimeStep;    //何秒で補完するか
    private float startTime = 0;                        //補完が始まった時間
    private float alpha = Tinge.DefaultColor;           //アルファブレンド
    void Start()
    {
        //指の情報を取得
        downFinger = GameObject.Find("DownImage").GetComponent<RectTransform>();
        //StageSelectDirectorを取得
        SSDObj = GameObject.Find("StageSelectDirector");
        //初期化
        startTime = Time.timeSinceLevelLoad + TimeCons.WaitStartTime;          //補完が始まった時間
        startPos = new Vector3(LerpPos.VecX, LerpPos.StartVecY, LerpPos.VecZ); //補完を始める場所
        endPos = new Vector3(LerpPos.VecX, LerpPos.StartVecY, 0);               //補完を終える場所

    }
    void Update()
    {
        //待たせる時間の更新
        waitTime += Time.deltaTime;
        //コルーチン
        StartCoroutine("SwipeDown");
    }
    private IEnumerator SwipeDown()
    {
        //1.5秒待つ
        yield return new WaitForSeconds(TimeCons.WaitTime);
        //指を動かす
        MoveFinger();
    }

    //----------------------------------------------------------------------
    //! @brief 指を動かす
    //!
    //! @param[in] なし
    //!
    //! @return なし
    //----------------------------------------------------------------------
    private void MoveFinger()
    {
        StageSelectDirector SSD = SSDObj.GetComponent<StageSelectDirector>();
        if (SSD.GetSwipCnt() == 0)
        {
            //経過時間
            var diff = Time.timeSinceLevelLoad - startTime;
            //進行率
            var rate = diff / timeStep;
            //補完
            downFinger.localPosition = Vector3.Lerp(startPos, endPos, rate);
            //補完を終えたら
            if (diff > timeStep)
            {
                //次の補完のために情報を更新する
                if (downFinger.localPosition.y >= LerpPos.MarginStartVecY)
                {
                    //スワイプ用
                    SetLerp(LerpPos.StartVecY, 0, LerpPos.EndVecY, 0, TimeCons.SwipeTimeStep);
                }
                else if (downFinger.localPosition.y <= LerpPos.MarginEndVecY)
                {
                    //1.5秒待つ
                    if (waitTime > TimeCons.WaitTime)
                    {
                        //タッチ用
                        SetLerp(LerpPos.StartVecY, LerpPos.VecZ, LerpPos.StartVecY, 0, TimeCons.TouchTimeStep);
                    }
                }
            }
        }
        //スワイプされたら
        else if (SSD.GetSwipCnt() > 0)
        {
            //アルファブレンド値の減算処理
            alpha -= Tinge.MinusAlpha;
            GetComponent<Image>().color = new Color(Tinge.DefaultColor, Tinge.DefaultColor, Tinge.DefaultColor, alpha);
        }
    }

    //----------------------------------------------------------------------
    //! @brief 次の補完を設定する
    //!
    //! @param[in] startPosのVecor3 y,z,endPosのVecor3 y,z,何秒で補完するか
    //!
    //! @return なし
    //----------------------------------------------------------------------
    void SetLerp(float Svy, float Svz, float Evy, float Evz, float time)
    {
        startPos = new Vector3(LerpPos.VecX, Svy, Svz);
        endPos = new Vector3(LerpPos.VecX, Evy, Evz);
        timeStep = time;
        waitTime = 0;
        startTime = Time.timeSinceLevelLoad;
        downFinger.localPosition = startPos;
    }
}
