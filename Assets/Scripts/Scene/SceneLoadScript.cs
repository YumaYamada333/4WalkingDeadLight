using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

using UnityEngine.UI;



public class SceneLoadScript : MonoBehaviour
{
    CurtainControl CurtainSystem;
    PlayerAction ButtonActive;
    GameObject GameOver;
    GameObject GameClear;
    private GameObject RetryButton;
    private GameObject SelectButton;

    public static bool MoveFlag = false;
    void Start()
    {
       
        CurtainSystem = GameObject.Find("Canvas").GetComponent<CurtainControl>();
        GameOver = GameObject.Find("OVER");
        GameClear = GameObject.Find("CLEAR");
        RetryButton = GameObject.Find("TitleButton");
        SelectButton = GameObject.Find("SelectButton");
        MoveFlag = false;
    }

    //----------------------------------------------------------------------
    //! @brief シーン遷移
    //!
    //! @param[in] なし
    //!
    //! @return なし
    //----------------------------------------------------------------------

    //タイトル画面へ
    public void TitleSceneLoad()
    {
        if (SceneManager.GetActiveScene().name != "StageSelect")
        {
            CurtainControl CurtainSystem = GameObject.Find("Canvas").GetComponent<CurtainControl>();
            //カーテンを閉める
            CurtainSystem.curtainOut();
        }
        //遷移先のシーンをロード
        Invoke("TitleScene", 2);
    }
    //ステージセレクト画面へ
    public void StageSelectSceneLoad()
    {

        if (SceneManager.GetActiveScene().name != "Title")
        {
            CurtainControl CurtainSystem = GameObject.Find("Canvas").GetComponent<CurtainControl>();
            //カーテンを閉める
            CurtainSystem.curtainOut();
        }
        //遷移先のシーンをロード
        Invoke("StageSelectScene", 2);
        //カーテンを閉める
        CurtainSystem.curtainOut();
    }
    //ステージ1へ
    public void Stage1SceneLoad()
    {
        CurtainSystem = GameObject.Find("Canvas").GetComponent<CurtainControl>();
        //カーテンを閉める
        CurtainSystem.curtainOut();
        //遷移先のシーンをロード
        Invoke("Stage1Scene", 2);
    }

    //----------------------------------------------------------------------
    //! @brief シーンのロード
    //!
    //! @param[in] なし
    //!
    //! @return なし
    //----------------------------------------------------------------------
    void StageSelectScene()
    {
        SceneManager.LoadScene("StageSelect");
    }
    void Stage1Scene()
    {
        //現在読み込んでいるシーンの名前を取得
        string currentScene = SceneManager.GetActiveScene().name;
        //取得したシーン名で再読み込み
        SceneManager.LoadScene(currentScene);
    }
    void TitleScene()
    {
        SceneManager.LoadScene("Title");
    }
    //----------------------------------------------------------------------
    //! @brief ステージ上の遷移用ボタンの非表示
    //!
    //! @param[in] なし
    //!
    //! @return なし
    //----------------------------------------------------------------------
    public void StageButtonActive()
    {
        // ボタン二回目クリック
        if (MoveFlag)
            return;

        GetComponent<AudioSource>().Play();

        ButtonActive = GameObject.Find("unitychan").GetComponent<PlayerAction>();
        MoveFlag = true;
        ////ボタンとリザルトを消す
        //Invoke("Active", 0.7f);
    }
    public void ResultActive()
    {
        GameOver.SetActive(false);
        GameClear.SetActive(false);
    }

    //void Active()
    //{
    //    RetryButton.SetActive(false);
    //    SelectButton.SetActive(false);
    //    GameOver.SetActive(false);
    //    GameClear.SetActive(false);
    //}
}