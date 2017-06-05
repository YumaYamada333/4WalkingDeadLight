using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    // カード操作のインターフェース
    public CardManagement cardManage;

    // カード一枚あたりの時間(s)
    public float CPS;

    //// カード間のインターバルタイム
    //public float spaceTime;

    // カード時間
    float cardTime;

    //フレーム待ってみるためのカウント
    int wait_frame_flag = 0;

    //player
    private GameObject playerAction;

    [SerializeField]
    bool m_gimmick_move_flag = false;

    private bool m_oldGimmickMoveFlag = true;
    // カメラのコントローラー
    CameraControl m_camera;

    //プレイボタンの制御用
    public GameObject m_set_button;

    // ゲームの状態
    public enum GameState
    {
        SetCard,
        Acttion
    }
    GameState gameState;

    //プレイボタン音
    public AudioClip OK;

    SetButton flag;

    GameObject setButton;

    GameObject playButton;

    GameObject Imagebord;

    // Use this for initialization
    void Start()
    {
        gameState = GameState.SetCard;
        playerAction = GameObject.Find("unitychan");
        m_camera = GameObject.Find("MainCamera").GetComponent<CameraControl>();
        flag = GameObject.Find("ToCardSetButton").GetComponent<SetButton>();
        setButton = GameObject.Find("ToCardSetButton");
        playButton = GameObject.Find("PlayButton");
        //上イメージボード
        Imagebord = GameObject.Find("Imagebord");
    }

    // Update is called once per frame
    void Update()
    {
        // 仮　アクションとカードセットを切り替える
        //if (Input.GetKeyDown(KeyCode.LeftShift))
        //{
        //    gameState++;
        //    if (gameState == GameState.Acttion + 1) gameState = GameState.SetCard;
        //    AudioSource audioSource = gameObject.GetComponent<AudioSource>();
        //    audioSource.PlayOneShot(OK);

        //}

        //flag.Move_cnt = 2;

        
        flag.MoveInLerp();

        //Debug.Log(flag.Move_cnt);

        switch (gameState)
        {

            // カードセット時の処理
            case GameState.SetCard:

                // カードセットの操作を受けつけるようにする
                cardManage.isControlCard = true;
                //cardManage.ActtionCard(true);
                break;

            // アクション時の処理
            case GameState.Acttion:
                // カードセットの操作を受け付けないようにする
                //cardManage.isControlCard = false;
                cardTime += Time.deltaTime;
                //PlayrActionの情報を取得
                PlayerAction player = playerAction.GetComponent<PlayerAction>();
                //待機中で、ギミックが動ていなくて、カウントダウンの値がなくて、滑る床による補間をしていない場合
                if (player.IsIdle() && !GetGimmickFlag()&& CountDown.GetCountDown()== CountDown.CountType.Nothing && !player.IsSlideLerp())
                {
                    //1fまってみる
                    if (wait_frame_flag < 1)
                    {
                        wait_frame_flag++;
                    }
                    else
                    { 
                        //プレイヤーがいることを確認
                        if (player != null)
                        {
                            //プレイヤーが地面にいるなら
                            if (player.IsGround())
                            {
                                GameObject.Find("unitychan").GetComponent<PlayerAction>().ActionPlay(cardManage.ActtionCard(false));
                                cardManage.ApllyUsingCard();
                            }
                        }
                        //cardManage.ActtionCard(false);
                        cardTime = 0.0f;
                        wait_frame_flag = 0;
                    }   
                }

                // カメラの制御
                if (m_oldGimmickMoveFlag != GetGimmickFlag() && !m_camera.GetCameraMove())
                {
                    if (GetGimmickFlag())
                        m_camera.ResetCamera(0.5f);
                    else
                        m_camera.SetFocusObject(playerAction, new Vector3(0, 1, -10), true, 0.5f);

                    m_oldGimmickMoveFlag = m_gimmick_move_flag;
                }
                break;

        }

    }

    public void Play()
    {
        //ボードが移動中は通らない
        if(m_set_button.GetComponent<SetButton>().Move_cnt != 0)
        {
            return;
        }

        if (Imagebord.transform.localPosition.y >= 160.0f)
        {
            flag.Move_cnt = 2;
        }

        // スクロールボタンを非表示
        setButton.GetComponent<SetButton>().SetButtonToFasle();

        Destroy(setButton);
        
        Destroy(playButton);
        
        gameState++;
        if (gameState == GameState.Acttion + 1) gameState = GameState.SetCard;
        AudioSource audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.PlayOneShot(OK);

        
    }
    public GameState GetGameState()
    {
        return gameState;
    }

    public bool GetGimmickFlag()
    {
        return m_gimmick_move_flag;
    }

    public void SetGimmickFlag(bool flag)
    {
        m_gimmick_move_flag = flag;
    }
}
