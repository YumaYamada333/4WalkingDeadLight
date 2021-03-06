﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetButton : MonoBehaviour {
    // カメラ
    public GameObject mainCamera;
    // ボード  
    private GameObject ActionBord;
    private GameObject HandsBord;

    private GameObject Bord;

    //private GameObject RetuneButton;
    //private GameObject ResetButton;
    // ボタン
    private GameObject SpeedButton;
    private GameObject ExecutionButton;
    //リセットボタン
    private GameObject ResetButton;

    [SerializeField]
    private GameObject ScrollLeftButton;
    [SerializeField]
    private GameObject ScrollRifhtButton;

    [SerializeField]
    GameManager gameManager;

    private bool flag = true;
    private bool canScroll = true;
    //private float BordPosition_x;
    //private float BordPosition_y;
    //private float BordPosition_z;
    //private float SetPosition_x;
    //private float SetPosition_y;
    //private float SetPosition_z;
    //private bool position_Flag;

    // handsboradの初期位置
    private Vector3 firstPos = Vector3.zero;

    // セット時　アクション時のカードboard位置
    [SerializeField]
    private Vector3 setPosActionBord = Vector3.zero;
    [SerializeField]
    private Vector3 setPosHandsBord = Vector3.zero;
    [SerializeField]
    private Vector3 actPosActionBord = Vector3.zero;


    MathClass lerp;

    private GameObject imagebord;

    private GameObject imagebord2;

    public  int Move_cnt;

    private float m_autoMoveTime = 0.0f;

    public float timeStep;

    public float startTimeStep;

    float LerpMovement_x;
    float LerpMovement_y1;
    float LerpMovement_y2;
    float LerpMovement_y3;

    private bool Start_flag;

    //ボタン押下判定
    private bool buttonFlag;

    void Start()
    {
        buttonFlag = false;
    }

    // Use this for initialization
    void Awake ()
    {
        ActionBord = GameObject.Find("ActionBord");
        HandsBord = GameObject.Find("HandsBord");
        Bord = GameObject.Find("Boards");
        SpeedButton = GameObject.Find("SpeedButton");
        ExecutionButton = GameObject.Find("PlayButton");
        //リセットボタン
        ResetButton = GameObject.Find("Reset");

        //上イメージボード
        imagebord = GameObject.Find("Imagebord");
        //下イメージボード
        imagebord2 = GameObject.Find("Imagebord2");


        //移動フラグ
        Move_cnt = 0;


        m_autoMoveTime = Time.time ;
        
        timeStep = 0;
        
        // actionboardの配置
        ActionBord.transform.localPosition = actPosActionBord;

        // 初期位置取得
        firstPos = mainCamera.transform.position;

        //HandsBord.SetActive(false);

        /*初期化*/
        LerpMovement_x = 350.0f;
        LerpMovement_y1 = 170.0f;
        LerpMovement_y2 = 189.0f;
        LerpMovement_y3 = 300.0f;

        Start_flag = false;

    }

    // Update is called once per frame
    void Update ()
    {
        //if (imagebord.transform.localPosition.y >= -187.0f)
        //{
        //    Start_flag = true;
        //}

        if (!Start_flag)
        {
            StartMoveLerp();
        }

        if(ActionBord.transform.localPosition.y <=171)
        {
            Start_flag = true;
        }

        //アクションボードが上にある時だけフラグを立てる
        if (imagebord.transform.localPosition.y > 160.0f)
        {
            buttonFlag = false;
        }
        else
        {
            buttonFlag = true;
        }

        // actionboardの位置更新
        if (flag)
        {
            //ActionBord.transform.localPosition = actPosActionBord/* + mainCamera.transform.position - firstPos*/;
        }
        CardBord board = ActionBord.GetComponent<CardBord>();
        if (canScroll)
        {
            if (board.CheckLeftEnd()) ScrollRifhtButton.SetActive(false);
            else ScrollRifhtButton.SetActive(true);

            if (board.CheckRightEnd()) ScrollLeftButton.SetActive(false);
            else ScrollLeftButton.SetActive(true);
        }
        timeStep = (Time.time - m_autoMoveTime) /*/ 10.0f*/;
        if (timeStep > 1.0f)
        {
            Move_cnt = 0;
            timeStep = 1.0f;
        }

        startTimeStep = (Time.time - m_autoMoveTime) /*/ 10.0f*/;
        if (startTimeStep > 1.0f)
        {
            Move_cnt = 0;
            startTimeStep = 1.0f;
        }

        //MoveInLerp();
        
    }

    public void OnClick()
    {
        buttonFlag = true;
        /*音を鳴らす*/
        //GetComponent<AudioSource>().Play();

        if (Move_cnt != 0)
        {
            return;
        }

        //timeStep = (Time.time - m_autoMoveTime);
        m_autoMoveTime = Time.time;
        
        //if(imagebord.transform.localPosition.y <= -170.0f)
        //{
        //    Move_cnt = 1;
        //}
        //if(imagebord.transform.localPosition.y >= 160.0f)
        //{
        //    Move_cnt = 2;
        //}

        if (flag)
        {
            // カメラの切り替え　ボタンの配置
            flag = false;
            //position_Flag = true;
            //SpeedButton.SetActive(false);
            //ExecutionButton.SetActive(false);
            //リセットボタン
            //ResetButton.SetActive(true);

            //RetuneButton.SetActive(true);
            

            // boardの配置
            //HandsBord.transform.localPosition = setPosHandsBord;
            //ActionBord.transform.localPosition = setPosActionBord;
            // HandsBordを表示
            HandsBord.SetActive(true);

            //Move_flag = true;
        }
        else
        {
            flag = true;
            // カメラの切り替え　ボタンの配置
            //position_Flag = false;
            //SpeedButton.SetActive(true);
            //ExecutionButton.SetActive(true);
            //リセットボタン
            //ResetButton.SetActive(false);

            //RetuneButton.SetActive(false);
            //ResetButton.SetActive(false);

            //actionbordの配置
            ActionBord.transform.localPosition = actPosActionBord;

            // HandsBordを非表示
            //HandsBord.SetActive(false);

            //Move_flag = false;
        }
    }


    //----------------------------------------------------------------------
    //! @brief ボタンとボードのLerp処理
    //!
    //! @param[in] なし
    //!
    //! @return なし
    //----------------------------------------------------------------------
    public void MoveInLerp()
    {
        switch (Move_cnt)
        {
         case 0:

            break;

         case 1:

                //HandsBord.transform.localPosition =
                //    MathClass.Lerp(new Vector3(0.0f, -110, 0),
                //    new Vector3(0, 0, 0), timeStep);



                //ResetButton.transform.localPosition =
                //    MathClass.Lerp(new Vector3(LerpMovement_x, -LerpMovement_y3, 0),
                //    new Vector3(350.0f, -LerpMovement_y2, 0), timeStep);


                //ActionBord.transform.localPosition =
                //    MathClass.Lerp(new Vector3(0, 0, 0),
                //    new Vector3(0.0f, LerpMovement_y3 + 57, 0), timeStep);


                //imagebord.transform.localPosition =
                //    MathClass.Lerp(new Vector3(0.0f, -LerpMovement_y2, 0),
                //    new Vector3(0.0f, LerpMovement_y1, 0), timeStep);


                //imagebord2.transform.localPosition =
                //    MathClass.Lerp(new Vector3(0.0f, -LerpMovement_y3, 0),
                //    new Vector3(0.0f, -LerpMovement_y2, 0), timeStep);


                //Move_flag = true;


                break;

            case 2:
                //HandsBord.transform.localPosition =
                //    MathClass.Lerp(new Vector3(0, 0, 0),
                //    new Vector3(0.0f, -110, 0), timeStep);


                //ResetButton.transform.localPosition =
                //    MathClass.Lerp(new Vector3(LerpMovement_x, -LerpMovement_y2, 0),
                //    new Vector3(LerpMovement_x, -LerpMovement_y3, 0), timeStep);


                ActionBord.transform.localPosition =
                    MathClass.Lerp(new Vector3(0.0f, LerpMovement_y1 + 57, 0),
                    new Vector3(0, -LerpMovement_y2, 0), timeStep);


                imagebord.transform.localPosition =
                    MathClass.Lerp(new Vector3(0.0f, LerpMovement_y1, 0),
                    new Vector3(0.0f, -LerpMovement_y2, 0), timeStep);


                //imagebord2.transform.localPosition =
                //    MathClass.Lerp(new Vector3(0.0f, -LerpMovement_y2, 0),
                //    new Vector3(0.0f, -LerpMovement_y3, 0), timeStep);

                break;
        }
    }

    public void SetButtonToFasle()
    {
        ScrollRifhtButton.SetActive(false);
        ScrollLeftButton.SetActive(false);
        canScroll = false;
    }

    void StartMoveLerp()
    {
        imagebord.transform.localPosition =
            MathClass.Lerp(new Vector3(0.0f, LerpMovement_y3, 0),
            new Vector3(0.0f, LerpMovement_y1, 0f), startTimeStep);

        imagebord2.transform.localPosition =
            MathClass.Lerp(new Vector3(0.0f, -LerpMovement_y3, 0),
            new Vector3(0.0f, -LerpMovement_y2, 0f), startTimeStep);

        ActionBord.transform.localPosition =
            MathClass.Lerp(new Vector3(0.0f, LerpMovement_y3, 0),
            new Vector3(0.0f, LerpMovement_y1, 0f), startTimeStep);

        HandsBord.transform.localPosition =
            MathClass.Lerp(new Vector3(0.0f, -LerpMovement_y3, 0),
            new Vector3(0.0f, -LerpMovement_y2, 0f), startTimeStep);

    }
    public bool GetFlag()
    {
        return buttonFlag;
    }

}
