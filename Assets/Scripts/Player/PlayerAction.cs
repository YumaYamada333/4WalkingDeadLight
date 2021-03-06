﻿//__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/
//! @file   PlayerAction
//!
//! @brief  プレイヤーの移動
//!
//! @date   2017/04/27
//!
//! @author N.Sakuma
//__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//定数の定義
static class Constants
{
    public const int Attack      = 1;  //attak
    public const int StageHeight = 2;  //ステージの高さ
    public const int RunPow      = 2;  //走る距離
    public const int SuperAttack = 3;  //superattack
    public const int MaxEnemy    = 4;  //敵の数
    public const int MaxJumpPow  = 5;  //最大のジャンプ力
    public const int MaxAnimation= 7;  //最大のアニメーションの数
    public const int MaxTime     = 10; //最大時間
    public const int MoveCount   = 60; //移動エフェクトのループ再生する間隔

    public const float FallVelocity = 0.06f;//落下速度
    public const float Adjustment   = 0.5f; //調整
    public const float MassDistance = 2.2f; //マスの距離
}
//アニメーション
enum ANIMATION { MOVE, JUMP, ATTACK, COUNT, OVER, };
//パーティクル
enum PARTICLE { NONE, MOVE,ATTACK,DAMAGE,LANDING,WATER,POISON,HIT,LAVA};
public class PlayerAction : MonoBehaviour
{
    private int effect_count = 0;   //エフェクト再生用のカウント
    private int animationNum = 0;   //アニメーションの番号

    private float time      = 0.5f; //時間
    private float jumpPower = 2.14f;//ジャンプ
    private float diff;             //経過時間
    private float startTime;        //走り始めた時間

    private bool[] animationFlag = new bool[Constants.MaxAnimation];   //アニメーションしているかどうかのフラグ
    private bool idleFlag;      //待機かどうかのフラグ
    private bool cardSetFlag;   //カードがセットされたかどうかのフラグ
    private bool isGround;      //地面についているかのフラグ
    private Vector3 middlePosition; 　                    //中間地点
    private Vector3 endPosition = new Vector3(2, 0, 0);   //走り終わる場所
    private Vector3 nextPosition = new Vector3(2, 0, 0);  //次の場所
    private Vector3 startPosition;                        //走り始める場所

    private System.String animationName;  //アニメーションの名前
    private GameObject[] enemy;           //敵
    private AudioSource audioSource;      //音
    private Animator animator;            //アニメーター
    private CharacterController controller;  //charactercontroller

    //Resultを動かすためのフラグ
    public bool ClearFlag = false;
    public bool OverFlag = false;

    // クリアコンポーネント
    public GameObject T_GameClear;
    // オーバーコンポーネント
    public GameObject GameOver;

    //ゲーム終了時に表示するボタン
    public GameObject RetryButton;
    public GameObject SelectButton;
    //ボードを消す
    public GameObject CanvasBord;
    public GameObject ImageBord;
    public GameObject ImageBord2;
    public GameObject CanvasResetButton;
    public GameObject CanvasSetButton;
    public GameObject CanvasPlayButton;

    //カメラのポジション
    Vector3 CameraPos;

    //下降する座標
    private float FallPos = 8.0f;

    //下降する値
    private float FallNum = 0.05f;

    //ループする回数
    private int RoopCnt = 10;

    //下降する座標(y座標)
    private float FallPosY;

    //下降する座標(z座標)
    private float FallPosZ = 1;

    //音
    public AudioClip Attack;
    public AudioClip Jump;
    public AudioClip Hit;
    public AudioClip Move;
    public AudioClip Water;
    public AudioClip Magma;
    public AudioClip Wall;
    public AudioClip Countsound;


    //パーティカルの種類判別用
    public int particleType;

    //当たり判定用子オブジェクト
    GameObject child;
    //カウントマネージャー
    public GameObject CountManager;

    //滑る床関連
    RaycastHit slideHit;         //Ray
    bool isSliding;              //下のオブジェクトが斜めかどうか
    bool isSlidisgOld;           //下のオブジェクトが斜めだったかどうか
    public float gravity = 5.8f; //滑っている最中の重力
    Vector3 dir;                 //滑る時に格納するvec3

    //滑る床のLerp用
    Vector3 slideStartPos;  //開始座標
    Vector3 slideEndPos;    //終了座標
    float slideStartTime;   //時間
    bool isSlideLerp;       //補完するべきか

    //クリア時にだすボタンの座標 (y,z)
    float endButtonPosY = -210;
    float endButtonPosZ = -1;
    float endButtonPosX = 90;

    //液体ブロック種類判別用
    public int LiquidType;
    int LandCnt;
    bool LandFlag;
    private Vector3 LandPos;

    /*水の音フラグ*/
    private bool water_flag;

    // カウント演出UI
    private GameObject watchController;

    void OnEnable() //objが生きている場合
    {
        if (time <= 0)
        {
            return;
        }
        //シーンが呼ばれた時点からの経過時間を取得
        startPosition = transform.position;
    }

    private void Awake()
    {
        watchController = GameObject.Find("WatchPrefab");
    }

    // Use this for initialization
    void Start()
    {
        dir = Vector3.zero;
        //Physics.gravity = new Vector3(0, 20.81f, 0);
        //参照の取得
        animator = GetComponent<Animator>();
        audioSource = gameObject.GetComponent<AudioSource>();
        controller = GetComponent<CharacterController>();
        child = transform.Find("AttackColl").gameObject;
        ImageBord = GameObject.Find("Imagebord");
        ImageBord2 = GameObject.Find("Imagebord2");
        particleType = (int)PARTICLE.NONE;
        LiquidType = 0;
        water_flag = true;
        LandFlag = false;
        LandCnt = 0;
        LandPos = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {

        //カメラのポジション
        CameraPos = GameObject.FindGameObjectWithTag("MainCamera").transform.position;
        //OverPosに代入
        FallPosY = CameraPos.y / 2 + 10;
        //真下にRayを飛ばして、当たっているかどうか
        if (Physics.Raycast(transform.position, Vector3.down, out slideHit, 1.0f))
         {
            //下のオブジェクトが斜めかどうか
            if (Vector3.Angle(slideHit.normal, Vector3.up) > controller.slopeLimit)
            {
                isSliding = true;
                isSlidisgOld = true;
            }
            else
            {
                isSliding = false;
            }
        }
        //敵の数を取得
        enemy = GameObject.FindGameObjectsWithTag("Enemy");

        //エフェクトの再生
        PlayEffect(animationNum);

        //プレイヤーの重力による座標の処理
        GravityForPlayer();

        //待機中の場合
        if (IsIdle())
        {
            //中間地点yを取得
            middlePosition.y = transform.position.y;

            //中間地点xを取得
            middlePosition.x = endPosition.x - nextPosition.x / 2;

        }
        //アクションを決める
        SetAction(animationNum);
        //プレイヤーの移動
        PlayerMove(animationNum, animationName);
        //補完するべきの場合
        if(isSlideLerp)
        {   
            //歩く
            animator.SetBool("Move", true);
            //補完
            var diff = Time.timeSinceLevelLoad - slideStartTime;
            var rate = diff / time;
            transform.position = Vector3.Lerp(slideStartPos, slideEndPos, rate);
            //終わったら
            if(diff > time)
            {
                //止まる
                animator.SetBool("Move", false);
                isSlideLerp = false;
            }
        }

        //if (Physics.Raycast(transform.position, Vector3.forward, out slideHit))
        //{
        //    //敵との当たり判定
        //    for (int i = 0; i < enemy.Length; i++)
        //    {
        //        //attack
        //        if (animationFlag[(int)ANIMATION.ATTACK] == true)
        //            Destroy(enemy[i]);
        //    }
        //}
        //characterとgroundの判定

        if (controller.isGrounded)
        {
            isGround = true;
            //下のオブジェクトが斜めだった場合
            if (isSliding)
            {
                Vector3 hitNormal = slideHit.normal; //法線ベクトルを取得
                dir.x = hitNormal.x * 10;            //そのままの法線ベクトルだと小さすぎるので大きくする
                dir.y = gravity * Time.deltaTime;    //重力を強めにかける
                dir.z = hitNormal.z;                 //そのまま
                transform.position += dir * Time.deltaTime * 1.1f;　//滑らせる
            }
        }
        else
        {
            isGround = false;
            particleType = (int)PARTICLE.NONE;               //パーティクルの種類決定
        }

        if(LandFlag)
        {
            LandCnt++;
            if(LandCnt<=30)
            {
                particleType = (int)PARTICLE.LANDING;               //パーティクルの種類決定
            }
            else
            {
                LandCnt = 0;
                particleType = (int)PARTICLE.NONE;               //パーティクルの種類決定
                LandFlag = false;
            }
        }

        //液体パーティクル判別
        GetLiquid();

        //ClearFlagがtrueだったら
        ClearControl();

        //OverFlagがtrueだったら
        OverControl();
        //Debug.Log(isGround);
    }

    //----------------------------------------------------------------------
    //! @brief プレイヤーの重力による座標の処理
    //!
    //! @param[in] なし
    //!
    //! @return なし
    //----------------------------------------------------------------------
    void GravityForPlayer()
    {
        //今現在のy地点をに記憶させる
            endPosition.y = transform.position.y;
    }
    //----------------------------------------------------------------------
    //! @brief プレイヤーの攻撃
    //!
    //! @param[in] なし
    //!
    //! @return なし
    //----------------------------------------------------------------------
    void PlayerAttack(int i, int num)
    {
        //プレイヤーの攻撃範囲に敵がいる場合
        if (enemy[i].transform.position.x - transform.position.x <= Constants.MassDistance * num
            && transform.position.y > enemy[i].transform.position.y
            && enemy[i].transform.position.y - transform.position.y >= -Constants.Adjustment)
        {
            //敵を消す
            Destroy(enemy[i]);
            //音を出す
            audioSource.PlayOneShot(Hit);
        }
    }
    //----------------------------------------------------------------------
    //! @brief 今待機中かどうか
    //!
    //! @param[in] なし
    //!
    //! @return idelFlag
    //----------------------------------------------------------------------
    public bool IsIdle()
    {
        //待機中の場合
        if (animationFlag[(int)ANIMATION.MOVE] == false && animationFlag[(int)ANIMATION.JUMP] == false && animationFlag[(int)ANIMATION.ATTACK] == false
            && animationFlag[(int)ANIMATION.COUNT] == false)
        {
            idleFlag = true;
        }
        //そうでない場合
        else
        {
            idleFlag = false;
        }
        return idleFlag;
    }

    //----------------------------------------------------------------------
    //! @brief アクションを決める
    //!
    //! @param[in] アニメーションの番号
    //!
    //! @return なし
    //----------------------------------------------------------------------
    void SetAction(int animationFlagNum)
    {
        //カードをセットした
        if (cardSetFlag == true)
        {
            //場所を記憶させる
            startPosition = transform.position;
            //アニメーション
            animationFlag[animationFlagNum] = true;
            //時間の計測
            startTime = Time.timeSinceLevelLoad;
            //アニメーションの番号を取得
            switch (animationFlagNum)
            {
                //move
                case (int)ANIMATION.MOVE:
                    middlePosition.x = transform.position.x + Constants.RunPow / 2;
                    endPosition.x = transform.position.x + Constants.RunPow;
                    break;
                //jump
                case (int)ANIMATION.JUMP:
                    //middlePosにjumpPowを足す
                    middlePosition = new Vector3(transform.position.x + Constants.RunPow, middlePosition.y += jumpPower, 0);
                    //終点を決める
                    endPosition = new Vector3(transform.position.x + Constants.RunPow * 2, endPosition.y, 0);
                    break;
                //attack
                case (int)ANIMATION.ATTACK:
                case (int)ANIMATION.COUNT:
                    //移動しない
                    middlePosition = new Vector3(transform.position.x, middlePosition.y, 0);
                    //移動しない
                    endPosition = new Vector3(transform.position.x, endPosition.y, 0);
                    if (animationFlagNum == (int)ANIMATION.COUNT)
                    {
                        particleType = (int)PARTICLE.NONE;
                    }
                    break;
            }

        }

    }
    //----------------------------------------------------------------------
    //! @brief プレイヤーの移動
    //!
    //! @param[in] アニメーションの番号、アニメーションの名前
    //!
    //! @return なし
    //----------------------------------------------------------------------
    void PlayerMove(int animationFlagNum, System.String animation)
    {
        //アニメーションが実行された
        if (animationFlag[animationFlagNum] == true)
        {
            //カードセットの処理を止める
            cardSetFlag = false;
            //アニメーション
            //animator.Play(animation, 0, 0.0f);
            animator.SetBool(animation, true);
            //経過時間
            diff = Time.timeSinceLevelLoad - startTime;
            //進行率
            var rate = diff / time;

            //等速で移動させる
            transform.position = Vector3.Lerp(startPosition, middlePosition, rate);
            //中間地点を超えたら
            if (diff > time)
            {
                //middlePosの情報をstartPosに代入
                startPosition.y = middlePosition.y;
                //等速で移動させる
                transform.position = Vector3.Lerp(startPosition, endPosition, rate / 2);
                //attack
                if (animationFlag[(int)ANIMATION.ATTACK] == true)
                {

                    Vector3 trans = new Vector3(1.0f, 0.8f, 0.68f);
                    child.SetActive(true);
                    child.transform.position = transform.position + trans;
                }
                //endPositionに到着
                if (diff > time * 2)
                {
                    if (animationFlag[(int)ANIMATION.ATTACK] == true)
                        child.SetActive(false);
                    if (isGround)
                    {
                        CardBord board = GameObject.Find("ActionBord").GetComponent<CardBord>();
                        if (board.GetCardType(board.usingCard - 1) == CardManagement.CardType.Move)
                        {
                            CountManager.GetComponent<CountDownManager>().ManagerCountDown(CountDown.CountType.ActionMove);
                        }
                        if (board.GetCardType(board.usingCard - 1) == CardManagement.CardType.Count)
                        {
                            CountManager.GetComponent<CountDownManager>().ManagerCountDown(CountDown.CountType.ActionCountDown);
                        }
                        //animationを止めるフラグ
                        animationFlag[animationFlagNum] = false;
                        //アニメーションを止める
                        animator.SetBool(animation, false);
                        //カウントダウンフラグを立てる
                        //CardBord board = GameObject.Find("ActionBord").GetComponent<CardBord>();
                        //CountDown.SetCountDown(board.GetCardType(board.usingCard - 1));
                        //次の場所との差
                        endPosition += nextPosition;
                        particleType = (int)PARTICLE.NONE;        //パーティカルの種類決定
                    }
                    else
                    {
                        transform.position -= new Vector3(0, Constants.FallVelocity, 0);
                        particleType = (int)PARTICLE.NONE;        //パーティカルの種類決定

                    }
                }
            }

        }
    }

    //----------------------------------------------------------------------
    //! @brief カードの情報を取得
    //!
    //! @param[in] カードの種類
    //!
    //! @return なし
    //----------------------------------------------------------------------
    public void ActionPlay(CardManagement.CardType type)
    {
        //地面についている
        if (isGround == true)
        {
            switch (type)
            {
                //move
                case CardManagement.CardType.Move:
                    audioSource.PlayOneShot(Move);      //音
                    cardSetFlag = true;                 //カードセットフラグ
                    animationNum = (int)ANIMATION.MOVE;  //アニメーションの番号
                    animationName = "Move";              //アニメーションの名前
                    if (!OverFlag || isGround)
                    {
                        particleType = (int)PARTICLE.MOVE;               //パーティクルの種類決定
                    }
                    if(OverFlag)
                    {
                        audioSource.Stop();
                    }
                    break;
                //jump
                case CardManagement.CardType.Jump:
                    audioSource.PlayOneShot(Jump);      //音
                    cardSetFlag = true;                 //カードセットフラグ
                    animationNum = (int)ANIMATION.JUMP; //アニメーションの番号
                    animationName = "Jump";             //アニメーションの名前
                    particleType = (int)PARTICLE.NONE;        //パーティカルの種類決定
                    if (OverFlag)
                    {
                        audioSource.Stop();
                    }
                    
                    break;
                //attack
                case CardManagement.CardType.Attack:
                    audioSource.PlayOneShot(Attack);        //音
                    cardSetFlag = true;                     //カードセットフラグ
                    animationNum = (int)ANIMATION.ATTACK;   //アニメーションの番号
                    animationName = "Attack";               //アニメーションの名前
                    if (!OverFlag)
                    {
                        particleType = (int)PARTICLE.ATTACK;        //パーティカルの種類決定
                    }
                    else
                    {
                        audioSource.Stop();
                    }
                    break;
                case CardManagement.CardType.Count:
                    for (int i = 0; i < 20; i++)
                    {
                        audioSource.PlayOneShot(Countsound,1.0f);        //音
                    }
                    cardSetFlag = true;                     //カードセットフラグ
                    animationNum = (int)ANIMATION.COUNT;   //アニメーションの番号
                    animationName = "Count";               //アニメーションの名前
                    //EffekseerHandle attack = EffekseerSystem.PlayEffect("attake", transform.position);
                    //particleType = (int)PARTICLE.ATTACK;        //パーティカルの種類決定
                    //CountDown.SetCountDown(type);
                    watchController.GetComponent<WatchController>().CountWatchEffect();
                    if (OverFlag)
                    {
                        audioSource.Stop();
                    }
                    break;
                //finish
                case CardManagement.CardType.Finish:
                    cardSetFlag = true;                     //カードセットフラグ
                    animationNum = (int)ANIMATION.ATTACK;   //アニメーションの番号
                    animationName = "Over";                 //アニメーションの名前
                    //particleType = (int)PARTICLE.DAMAGE;
                    //プレイヤーのアクションを止める
                    AnimationStop();
                    //Overの文字を移動するためのフラグをonに
                    OverFlag = true;
            

                    break;
                default:
                    particleType = (int)PARTICLE.NONE;
                    break;

            }
        }
        else
        {
            particleType = (int)PARTICLE.NONE;
        }
    }

    //----------------------------------------------------------------------
    //! @brief 敵との当たり判定
    //!
    //! @param[in] Collider
    //!
    //! @return なし
    //----------------------------------------------------------------------
    void OnTriggerEnter(Collider coll)
    {
        //プレイヤーと敵が当たったら
        if (coll.gameObject.tag == "Enemy" && !animationFlag[(int)ANIMATION.ATTACK])
        {
            particleType = (int)PARTICLE.DAMAGE;        //パーティカルの種類決定

            //カードボードなどの操作系を消す
            Invoke("SetCanvasActive", 0);
            //プレイヤーのアクションを止める
            AnimationStop();
            //Overの文字を移動するためのフラグをonに
            OverFlag = true;

            audioSource.Stop();
            


        }
        else
        {
            particleType = (int)PARTICLE.NONE;        //パーティカルの種類決定
        }

        //プレイヤーがゴールについたら
        if (coll.gameObject.tag == "Goal" && !animationFlag[(int)ANIMATION.ATTACK])
        {
            //カードボードなどの操作系を消す
            Invoke("SetCanvasActive", 0);
            //プレイヤーのアクションを止める
            AnimationStop();
            //CLEARの文字を移動するためのフラグをonに
            ClearFlag = true;
        }

        //トゲ
        if (coll.gameObject.tag == "Thorn" && !animationFlag[(int)ANIMATION.ATTACK])
        {
            //カードボードなどの操作系を消す
            Invoke("SetCanvasActive", 0);
            //プレイヤーのアクションを止める
            AnimationStop();
            //Overの文字を移動するためのフラグをonに
            OverFlag = true;
            //専用のパーティカルを設定
            particleType = (int)PARTICLE.HIT;

            audioSource.Stop();
        }
        // ブロック     回転すると死んじゃうよ～～～～
        if (coll.gameObject.tag == "Block" && !animationFlag[(int)ANIMATION.ATTACK])
        {
           
            //カードボードなどの操作系を消す
            Invoke("SetCanvasActive", 0);
            //プレイヤーのアクションを止める
            AnimationStop();
            //Overの文字を移動するためのフラグをonに
            OverFlag = true;
            //専用のパーティカルを設定
            particleType = (int)PARTICLE.HIT;

            audioSource.Stop();
            audioSource.PlayOneShot(Wall);
            audioSource.PlayOneShot(Wall);


        }

        //落下限界
        if (coll.gameObject.tag == "GameOverZone")
        {
            //カードボードなどの操作系を消す
            Invoke("SetCanvasActive", 0);
            //プレイヤーのアクションを止める
            AnimationStop();
            //Overの文字を移動するためのフラグをonに
            OverFlag = true;
        }

        //液体ブロック判定
        if (coll.gameObject.tag == "Water")
        {
            //水
            LiquidType = (int)PARTICLE.WATER;
            
        }
        else if (coll.gameObject.tag == "Poison")
        {
            //毒
            LiquidType = (int)PARTICLE.POISON;
            if (water_flag)
            {
                audioSource.PlayOneShot(Water);
                water_flag = false;
            }
        }
        else if(coll.gameObject.tag=="magma")
        {

            audioSource.PlayOneShot(Magma);
            audioSource.PlayOneShot(Magma);
            //溶岩
            LiquidType = (int)PARTICLE.LAVA;
        }
        else
        {
            //なし
            LiquidType = (int)PARTICLE.NONE;
        }

    }

    //----------------------------------------------------------------------
    //! @brief ステージオブジェクトとの当たり判定
    //!
    //! @param[in] ControllerColliderHit
    //!
    //! @return なし
    //----------------------------------------------------------------------
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        //ゴール
        if (hit.gameObject.tag == "Goal")
        {
            // 五秒後にクリア
            GameObject.Find("GameManager").GetComponent<ToResultScene>().ToClear(3);
        }
        //地面
        if (!isGround)
        {
            //tagがUntagged
            if (hit.gameObject.tag == "Untagged")
            {
                //middlePosを超えたら
                if (diff > time)
                {
                    if (!ClearFlag && !OverFlag)
                    {
                        LandFlag = true;
                        LandPos = transform.position;
                    }
                }
            }
        }
        //tagがUntaggedで待機中の場合
        if (hit.gameObject.tag == "Untagged" && IsIdle())
        {
            slideStartTime = Time.timeSinceLevelLoad;
            slideStartPos = transform.position;
            float pos = transform.position.x;
            particleType = (int)PARTICLE.NONE;        //パーティカルの種類決定
            //場所が違う場合
            if (pos % 2 != 0 && isSlidisgOld == true)
            {
                //補完する
                slideEndPos = new Vector3(Mathf.FloorToInt(pos) + 1, transform.position.y, transform.position.z);
                //まだ違う場合
                if(slideEndPos.x % 2 != 0)
                {
                    //補完する
                    slideEndPos = new Vector3(Mathf.FloorToInt(pos) + 2, transform.position.y, transform.position.z);
                }
                isSlidisgOld = false;
                isSlideLerp = true;
            }
        }
    }

    //----------------------------------------------------------------------
    //! @brief エフェクトを再生する関数
    //!
    //! @param[in] inputAnimeNum(今の動作を取得)
    //!
    //! @return なし
    //----------------------------------------------------------------------
    void PlayEffect(int anime_num)
    {
        //待機中でないなら
        if (!idleFlag)
        {
            switch (anime_num)
            {
                //move
                case (int)ANIMATION.MOVE:
                    //エフェクトを設定した間隔で再生
                    effect_count++;
                    if (effect_count >= Constants.MoveCount)
                    {
                        EffekseerHandle attack = EffekseerSystem.PlayEffect("smoke", transform.position);
                        effect_count = 0;
                    }
                    break;
            }
        }
    }
    //----------------------------------------------------------------------
    //! @brief 地面についているか
    //!
    //! @param[in] なし
    //!
    //! @return 地面についているか
    //----------------------------------------------------------------------
    public bool IsGround()
    {
        return isGround;
    }

    public bool IsSlideLerp()
    {
        return isSlideLerp;
    }
    //----------------------------------------------------------------------
    //! @brief アニメーションを止める
    //!
    //! @param[in] なし
    //!
    //! @return なし
    //----------------------------------------------------------------------
    public void AnimationStop()
    {
        animator.SetBool(animationName, false);
    }
    //----------------------------------------------------------------------
    //! @brief ボタンの表示
    //!
    //! @param[in] なし
    //!
    //! @return なし
    //----------------------------------------------------------------------
    public void SetButtonOn()
    {
        RetryButton.transform.localPosition = new Vector3(endButtonPosX, endButtonPosY, endButtonPosZ);
        SelectButton.transform.localPosition = new Vector3(-endButtonPosX, endButtonPosY, endButtonPosZ);

        RetryButton.SetActive(true);
        SelectButton.SetActive(true);
    }

    //----------------------------------------------------------------------
    //! @brief ボタンの表示(クリア時)
    //!
    //! @param[in] なし
    //!
    //! @return なし
    //----------------------------------------------------------------------
    public void SetButtonClear()
    {
        SelectButton.transform.localPosition = new Vector3(0, endButtonPosY, endButtonPosZ);
        SelectButton.SetActive(true);
    }

    //----------------------------------------------------------------------
    //! @brief ボタンの非表示
    //!
    //! @param[in] なし
    //!
    //! @return なし
    //----------------------------------------------------------------------
    public void SetButtonOff()
    {
        RetryButton.SetActive(false);
        SelectButton.SetActive(false);
    }
    //----------------------------------------------------------------------
    //! @brief キャンバスの非表示
    //!
    //! @param[in] なし
    //!
    //! @return なし
    //----------------------------------------------------------------------
    public void SetCanvasActive()
    {
        CanvasBord.SetActive(false);
        CanvasResetButton.SetActive(false);
        //CanvasSetButton.SetActive(false);
        //CanvasPlayButton.SetActive(false);
        ImageBord.SetActive(false);
        ImageBord2.SetActive(false);
    }

    //----------------------------------------------------------------------
    //! @brief ゲーム終了時のフラグ管理()
    //!
    //! @param[in] なし
    //!
    //! @return なし
    //----------------------------------------------------------------------
    void ClearControl()
    {
        //ClearFlagがtrueだったら
        if (ClearFlag)
        {
            GameObject.Find("GameManager").GetComponent<ToResultScene>().ToClear(0);
            Invoke("SetButtonClear", 0.6f);
        }
    }
    //----------------------------------------------------------------------
    //! @brief ゲーム終了時のフラグ管理()
    //!
    //! @param[in] なし
    //!
    //! @return なし
    //----------------------------------------------------------------------
    void OverControl()
    {
        //OverFlagがtrueだったら
        if (OverFlag)
        {
            // カメラの追尾をやめる
            GameObject.Find("MainCamera").GetComponent<CameraControl>().SetFollow(false);
            GameObject.Find("GameManager").GetComponent<ToResultScene>().ToOver(0);
            Invoke("SetButtonOn", 0.6f);
        }
    }

    //----------------------------------------------------------------------
    //! @brief 液体ブッロクの判定取得
    //!
    //! @param[in] なし
    //!
    //! @return なし
    //----------------------------------------------------------------------
    private int GetLiquid()
    {
        switch (LiquidType)
        {
            case (int)PARTICLE.WATER:
                //水のパーティクルを発生させる
                particleType = (int)PARTICLE.WATER;

                if (water_flag)
                {
                    audioSource.PlayOneShot(Water);
                    water_flag = false;
                }

                break;
            case (int)PARTICLE.POISON:
                //毒のパーティクルを発生させる
                particleType = (int)PARTICLE.POISON;

                break;
            case (int)PARTICLE.LAVA:
                //溶岩のパーティクルを発生させる
                particleType = (int)PARTICLE.LAVA;
                break;
        }
        return LiquidType;
    }

    public Vector3 GetPos()
    {
        return LandPos;
    }

}

