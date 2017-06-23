using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using System;

public class StageSelectDirector : MonoBehaviour
{

    // シーン名登録
    public enum SceneName
    {
        newStage1,
        Stage1,
        Stage2,
        Stage3,
        Stage4,
        Stage5,
        Stage6,
        Stage7,
        Stage8,
        Stage9,
        Stage10,
    }

    [System.Serializable]
    struct Pamphlet
    {
        public GameObject pamphletPrefab;       // パンフレットのプレハブ
        public SceneName nextScene;             // 次のシーン
    }

    // 読み込むシーン名
    static string sceneName;

    // パンフレットの配置空間
    struct PamphletSpace
    {
        public Vector3 pos;
        public int pamphlietIndex;
        public void Set(Vector3 pos, int index)
        {
            this.pos = pos;
            pamphlietIndex = index;
        }
    }

    [SerializeField]
    private Vector3 m_zeroPos = new Vector3(-2.0f, -4.5f, 0.0f);// 一番前のパンフレット座標
    [SerializeField]
    private Vector3 m_pos = new Vector3(1.5f, 1.5f, 0.1f);      // 重なりのずれ
    [SerializeField]
    private Pamphlet[] m_pamphletData;        // パンフレットのデータ
    private MathClass.Looper m_selectPamphlet;                  // 選択中のパンフレット

    private GameObject[] m_pamphlet = new GameObject[5];        // パンフレット
    static PamphletSpace[] m_space;                                    // パンフレットの配置空間

    [SerializeField]
    private GameObject m_ribbon;

    [SerializeField]
    private GameObject m_clearPrefab;

    private Vector2 dragVecOld;

    [SerializeField]
    private float stepSpd = 0.01f;
    private float changeStep = 0.0f;                            // 切り替えのステップ
    private float curtainUpStep = 0.0f;

    static bool isEnd;                                         // scene切り替えフラグ
    Vector3 posBasePamphlet;
    Vector3 posRibbon;

    [SerializeField]
    private float activityAreaFlick = 15.0f;

    private enum StepDirction
    {
        Up,
        Down,
    };
    StepDirction stepDir = StepDirction.Down;

    public AudioClip PlaySound;
    public AudioClip ScrollSound;
    static private AudioClip PS;
    static private AudioClip SS;

    private bool Sound_flag;

    // パンフレットをスワイプした回数
    private int swipCount = 0;

    [SerializeField]
    Button butto;


    // Use this for initialization
    void Start()
    {
        // 配列生成
        m_pamphlet = new GameObject[m_pamphletData.Length];

        // パンフレット配置位置の配列の生成
        if (m_space == null)
        {
            m_space = new PamphletSpace[m_pamphlet.Length];
            for (int i = 0; i < m_pamphlet.Length; i++)
                m_space[i].Set(m_zeroPos + (m_pos * i), i);
        }

        // ループ変数初期化
        m_selectPamphlet = new MathClass.Looper(m_pamphletData.Length - 1, 0, 0, m_space[0].pamphlietIndex);
        
        // ベース位置の初期化
        posBasePamphlet = m_zeroPos;
        posRibbon = m_ribbon.transform.position;

        // マウスのドラック量を初期化
        dragVecOld = GetComponent<MouseSystem>().GetDragVec();
        changeStep = 1.0f;
        isEnd = false;
        // 初期パンフレットの配置 配置空間の設定
        for (int i = 0; i < m_pamphlet.Length; i++)
        {
            m_pamphlet[i] = Instantiate(m_pamphletData[i].pamphletPrefab);
            if (i > 5 - 1) m_space[i].pos += new Vector3(0, -100, 0);
            m_pamphlet[i].transform.position = new Vector3(0, -80, 0) + (m_pos * i) + m_space[m_space[i].pamphlietIndex].pos;
            //m_pamphlet[i].GetComponent<Button>().
            // ボタンの非表示
            GameObject childObject = m_pamphlet[i].transform.Find("PamphletCanvas").transform.Find("PlayButton").gameObject;
            childObject.SetActive(false);
            if (i == m_space[0].pamphlietIndex) childObject.SetActive(true);
            // ボタンのサイズ調整
            float size = childObject.GetComponent<RectTransform>().sizeDelta.y;
            childObject.GetComponent<RectTransform>().sizeDelta = new Vector2(size, size);
        }

       // FlashingUGui.SetPlayButton(m_pamphlet[0].transform.Find("PamphletCanvas").transform.Find("PlayButton").gameObject);
        /*音を入れる*/
        PS = PlaySound;
        SS = ScrollSound;

        Sound_flag = true;

        // クリア情報を削除したい場合のみコメント外してください =======================================
        //PlayerPrefs.DeleteAll();
        // ============================================================================================

        // クリア情報の読み込み
        LoadClearData();
    }

    // Update is called once per frame
    void Update()
    {
        if (isEnd != true) curtainUpStep += 0.01f;
        else curtainUpStep -= 0.01f;
        if (curtainUpStep > 1.0f) curtainUpStep = 1.0f;
        if (curtainUpStep < 0.0f) curtainUpStep = 0.0f;

        //// マウスのドラック量を取得
        //Vector2 dragVec = GetComponent<MouseSystem>().GetDragVec() - dragVecOld;

        // スワイプの距離を取得
        if (!(curtainUpStep < 1.0f))
        {
            // 切り替えが行われていない
            if (changeStep >= 1.0f)
            {
                if (MouseSystem.GetFlickDistance().y > activityAreaFlick)
                {
                    // 配置空間のパンフインデックスの更新
                    for (int i = 0; i < m_space.Length; i++)
                    {
                        m_selectPamphlet.Plus(1);
                        m_space[i].pamphlietIndex = m_selectPamphlet.Get();
                    }
                    stepDir = StepDirction.Down;
                    m_selectPamphlet.Plus(1);
                    changeStep = 0.0f;
                    swipCount++;
                }
                else if (MouseSystem.GetFlickDistance().y < -activityAreaFlick)
                {
                    m_selectPamphlet.Plus(-2);
                    // 配置空間のパンフインデックスの更新
                    for (int i = 0; i < m_space.Length; i++)
                    {
                        m_selectPamphlet.Plus(1);
                        m_space[i].pamphlietIndex = m_selectPamphlet.Get();
                    }
                    stepDir = StepDirction.Up;
                    m_selectPamphlet.Plus(1);
                    changeStep = 0.0f;
                    swipCount++;
                }
            }
            else
            {
                if (Sound_flag)
                {
                    /*プレイ音を鳴らす*/
                    AudioSource audioSource = GameObject.Find("StageSelectDirector").GetComponent<AudioSource>();
                    audioSource.PlayOneShot(SS);
                    Sound_flag = false;
                }
                changeStep += 0.1f;
                if (changeStep > 0.95f)
                {
                    changeStep = 1.0f;
                    Sound_flag = true;
                }
            }

            // パンフレットのラープ処理
            for (int i = 0; i < m_pamphlet.Length; i++)
            {
                if (stepDir == StepDirction.Up)
                {
                    if (i == 0 && changeStep < 0.3f)
                    {
                        m_pamphlet[m_space[0].pamphlietIndex].transform.position =
                            MathClass.Lerp(m_pamphlet[m_space[0].pamphlietIndex].transform.position,
                        new Vector3(m_space[0].pos.x, m_space[0].pos.y - 20, m_space[0].pos.z),
                        changeStep);
                    }
                    else
                    {
                        m_pamphlet[m_space[i].pamphlietIndex].transform.position =
                            MathClass.Lerp(m_pamphlet[m_space[i].pamphlietIndex].transform.position, m_space[i].pos, changeStep);
                    }
                }
                else
                {
                    if (i > 5 - 1 && changeStep < 0.3f)
                    {
                        m_pamphlet[m_space[i].pamphlietIndex].transform.position =
                            MathClass.Lerp(m_pamphlet[m_space[i].pamphlietIndex].transform.position,
                        new Vector3(m_pamphlet[m_space[i].pamphlietIndex].transform.position.x, -100, m_pamphlet[m_space[i].pamphlietIndex].transform.position.z), changeStep);
                    }
                    else
                    {
                        m_pamphlet[m_space[i].pamphlietIndex].transform.position =
                            MathClass.Lerp(m_pamphlet[m_space[i].pamphlietIndex].transform.position, m_space[i].pos, changeStep);
                    }
                }
                // ボタンを表示　非表示
                GameObject childObject = m_pamphlet[i].transform.Find("PamphletCanvas").transform.Find("PlayButton").gameObject;
                if (changeStep > 0.95f && i != m_space[0].pamphlietIndex) childObject.SetActive(false);
                if (i == m_space[0].pamphlietIndex) childObject.SetActive(true);

            }


        }
        else
        {
            for (int i = 0; i < m_pamphlet.Length; i++)
            {
                // 始まりの演出
                m_pamphlet[m_space[i].pamphlietIndex].transform.position = MathClass.Lerp(m_space[i].pos + new Vector3(0, -80, 0) + (m_pos * i), /*posBasePamphlet +*/ m_space[i].pos, curtainUpStep);
                m_ribbon.transform.position = MathClass.Lerp(new Vector3(0, 80, 0), posRibbon, curtainUpStep);
            }

        }

        // 前フレーム更新
        dragVecOld = GetComponent<MouseSystem>().GetDragVec();

        // 読み込むシーン名の更新
        sceneName = m_pamphletData[m_space[0].pamphlietIndex].nextScene.ToString();
    }

    // ボタンを押したときの処理
    public void PlayButton()
    {
        /*プレイ音を鳴らす*/
        AudioSource audioSource = GameObject.Find("StageSelectDirector").GetComponent<AudioSource>();
        audioSource.PlayOneShot(PS);

        if (SceneManager.GetActiveScene().name != "StageSelect")
        {
            CurtainControl CurtainSystem = GameObject.Find("Canvas").GetComponent<CurtainControl>();
            //カーテンを閉める
            CurtainSystem.curtainOut();
        }
        //遷移先のシーンをロード
        Invoke("StageScene", 2);

        // パンフが下に、リボンが上にはける
        isEnd = true;
    }

    // シーンの遷移
    private void StageScene()
    {
        SceneManager.LoadScene(sceneName);
    }

    //private string[] m_clearData;

    public void LoadClearData()
    {
        for (int i = 0; i < m_pamphletData.Length; i++)
        {
            int clear = PlayerPrefs.GetInt(m_pamphletData[i].nextScene.ToString());

            if (clear >= 1)
            {
                GameObject clearObj = Instantiate(m_clearPrefab);
                clearObj.gameObject.transform.parent =
                    m_pamphlet[i].gameObject.transform.Find("PamphletCanvas").transform;
                clearObj.transform.localPosition = new Vector3(6.8f, 6.9f, 0.0f);
            }
        }
    }


    static public void StaticInitilize()
    {
        if (m_space == null) return;
        {
            for (int i = 0; i < m_space.Length; i++)
            {
                m_space[i].pamphlietIndex = i;
            }
        }
    }

    public int GetSwipCnt()
    {
        return swipCount;
    }

    static public void SetisEnd(bool isend)
    {
        isEnd = isend;
    }

}
