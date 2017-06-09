using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClickSceneLoad : MonoBehaviour {

    //音源取得用
    private AudioSource audioSource;
    //クリック判定取得用
    private bool ClickFlag;
    //カウント
    private float Count;
    private const float limitTime = 1.5f;

    // Use this for initialization
    void Start () {
        //コンポーネント取得
        audioSource = GameObject.Find("AudioDirecter").GetComponent<AudioSource>();

        //変数の初期化
        ClickFlag = false;
    }

    // Update is called once per frame
    void Update () {

        Debug.Log(Count);
        //タイトルシーン内の画面内でクリックしたらシーン遷移
        if (Input.GetMouseButtonUp(0))
        {
            //フラグを立てる
            ClickFlag = true;
            //音を再生
            audioSource.Play();
        }
        if (ClickFlag)
        {
            Count += Time.deltaTime;
        }
        if (Count >= limitTime)
        {
            ClickLoad();
            ClickFlag = false;
        }


    }
    public void ClickLoad()
    {
        if (SceneManager.GetActiveScene().name == "Title")
        {
            // ステージセレクトの静的変数初期化
            StageSelectDirector.StaticInitilize();
            SceneManager.LoadScene("StageSelect");
        }
    }

    //クリックフラグ取得関数
    public bool GetClickFlag()
    {
        return ClickFlag;
    }

    //カウント取得関数
    public float GetCount()
    {
        return Count;
    }
}
