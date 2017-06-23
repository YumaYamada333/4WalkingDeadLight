using System.Collections;
using UnityEngine;

public class ScrollScript : MonoBehaviour
{
    //カメラ初期位置
    Vector3 CameraPos;
    //カメラの現在位置
    Vector3 CameraTmp;
    public Vector2 scrollstart;         //スクロール開始座標
    bool click_flag = false;            //クリックフラグ
    Vector2 start_mouse_pos;            //初期マウス座標

    [SerializeField]
    private float rangePosX = 0.0f;   //スクロール範囲設定用変数
    [SerializeField]
    private float rangePosY = 0.0f;
    [SerializeField]
    private float underOffset = 0.0f;
    [SerializeField]
    private float safityArea = 1.0f;    // スクロールの遊び

    //public GameObject StartBlock;       //スタートブロック
    //public GameObject GoalBlock;        //ゴールブロック

    //private float StartBlockX;          //スタートブロックのｘ座標
    //private float GoalBlockX;           //ゴールブロックのｘ座標

    public bool isUpdate;

    CardManagement cardmanegement;      //カードマネジメント

    GameManager gamemanager;

    // Use this for initialization
    void Start ()
    {
        //カメラの初期位置取得
        CameraPos = GameObject.Find("MainCamera").transform.position;
        ////スタート、ゴールブロックのｘ座標取得
        //StartBlockX = StartBlock.transform.position.x;
        //GoalBlockX = GoalBlock.transform.position.x;

        //コンポーネントの取得
        cardmanegement = GameObject.Find("CardManager").GetComponent<CardManagement>();

        gamemanager = GameObject.Find("GameManager").GetComponent<GameManager>();

        //scrollstart = CameraPos;
        //scrollstart = new Vector2();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(CameraPos);
        // ギミックの動作中はスクロールさせない
        isUpdate = isUpdate ? !gamemanager.GetGimmickFlag() : isUpdate;
        if (isUpdate)
        {
            //左クリック
            if (Input.GetMouseButton(0))
            {
                //クリック直後
                if (!click_flag)
                {
                    //マウスの初期座標を取得
                    start_mouse_pos = Input.mousePosition;
                    click_flag = true;
                }

                //マウス座標を取得
                Vector2 mouse_pos = Input.mousePosition;

                //マウスクリック時カードをつかんでいないなら
                if (cardmanegement.GetGripFlag() == false)
                {
                    //カメラがターゲットポジションのより左にあるのならば
                    if (CameraTmp.x < rangePosX)
                    {
                        //右にスクロール
                        if (start_mouse_pos.x /*+ scrollstart.x*/> mouse_pos.x + safityArea)
                        {
                            //カメラを右スクロールさせる
                            transform.Translate(0.15f, 0, 0);
                        }
                    }
                    //カメラが初期座標よりのx座標より右にあるのならば
                    if (CameraTmp.x > CameraPos.x)
                    {
                        //左にスクロール
                        if (start_mouse_pos.x /*- scrollstart.x */< mouse_pos.x - safityArea)
                        {
                            //カメラを左スクロールさせる
                            transform.Translate(-0.15f, 0, 0);
                        }
                    }
                    //カメラがターゲットポジションのより下にあるのならば
                    if (CameraTmp.y < rangePosY + underOffset)
                    {
                        //上にスクロール
                        if (start_mouse_pos.y /*+ scrollstart.y*/> mouse_pos.y + safityArea)
                        {
                            //カメラを上スクロールさせる
                            transform.Translate(0, 0.15f, 0);
                        }
                    }
                    //カメラが初期座標よりのy座標より上にあるのならば
                    if (CameraTmp.y > CameraPos.y + underOffset)
                    {
                        //下にスクロール
                        if (start_mouse_pos.y /*- scrollstart.y*/< mouse_pos.y - safityArea)
                        {
                            //カメラを下スクロールさせる
                            transform.Translate(0, -0.15f, 0);
                        }
                    }

                }
            }
            else
            {
                click_flag = false;
            }
        }
            //カメラの現在位置取得
            CameraTmp = GameObject.Find("MainCamera").transform.position;

            //実行ボタンが押されたら
            if (Input.GetButtonDown("Fire3"))
            {
                //カメラの初期位置に移動
                GameObject.Find("Main Camera").transform.position = new Vector3(CameraPos.x, CameraPos.y, CameraPos.z);
            }
        }

    // タッチ開始時点の取得
    //! スクリーン座標で取得する
    // 左下基準 (スクリーンサイズによって右上の値は変わる)
    public Vector2 GetTouchPos()
    {
        return start_mouse_pos;
    }

    //  タッチ中かを取得
    public bool CheckTouch()
    {
        return click_flag;
    }
}
