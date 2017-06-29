using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatRock : MonoBehaviour {

    //石の見た目用のオブジェクト
    public GameObject m_rock_obj;
    //熱の当たり判定のオブジェクト
    public GameObject m_heat_obj;
    //カウント用オブジェクト
    public GameObject m_count_obj;
    //ゲームマネージャー
    public GameObject m_game_manager;
    //変更用マテリアル
    public Material m_material;
    //複数オブジェクトが同タイミングで動く場合に最後に動き終わるオブジェクト
    public GameObject m_final_move_obj;
    /*ギミック音*/
    public AudioClip GimmickSound;

    //石の状態(初期状態代入)
    [SerializeField]
    private bool m_isHeat;

    //スタートタイム
    private float m_start_time = 0.0f;
    //スタート時のカラー
    [SerializeField]
    private Color m_start_color;
    //目標のカラー
    [SerializeField]
    private Color m_dis_color;
    //待機時間
    private float m_wait_time = 1.0f;
    //フラグ
    private bool m_old_flag = false;
    private bool m_action_flag = false;


    // Use this for initialization
    void Start () {
        m_material = m_rock_obj.GetComponent<MeshRenderer>().material;
        if(m_isHeat)
        {
            m_material.color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
            m_dis_color = new Color(0.6f, 0.6f, 0.6f, 1.0f);
        }
        else
        {
            m_material.color = new Color(0.6f, 0.6f, 0.6f, 1.0f);
            m_dis_color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
        }
        m_start_color = m_material.color;
	}
	
	// Update is called once per frame
	void Update () {
        //カウントによって状態を変更
        GetCountZero();
        if (m_action_flag == true &&
            m_action_flag != m_old_flag)
        {
            m_start_time = Time.time;
            m_old_flag = m_action_flag;
        }
        //フラグがたっていたら変更
        if (m_action_flag)
        {

            //経過時間を移動時間で割る
            float timeStep = (Time.time - m_start_time) / 1;
            if (timeStep <= 1.0f)
            {
                //色を徐々に変える
                m_material.color = new Color((1 - timeStep) * m_start_color.r + timeStep * m_dis_color.r,
                                                (1 - timeStep) * m_start_color.g + timeStep * m_dis_color.g,
                                                (1 - timeStep) * m_start_color.b + timeStep * m_dis_color.b,
                                                1.0f);
            }
               
            if(timeStep > 0.6f)
            {
                if (m_isHeat )
                {
                    //熱当たり判定を変更
                    m_heat_obj.SetActive(false);
                }
                else
                {
                    //熱当たり判定を変更
                    m_heat_obj.SetActive(true);
                }
            }
           

            //移動時間になったらフラグを止める
            if (timeStep > 1.0f)
            {
               
                //if (m_isHeat){
                //    m_dis_color = new Color(150.0f, 150.0f, 150.0f, 255.0f);
                //}
                //else{
                //    m_dis_color = new Color(255.0f, 0.0f, 0.0f, 255.0f);
                //}

                //各値初期化
                //同タイミングで動くオブジェクトが動き終わっていたら
                if (m_final_move_obj != null)
                {
                    if (!m_final_move_obj.GetComponent<FallLava>().m_awake_flag)
                    {
                        m_action_flag = false;
                        m_old_flag = false;
                        m_game_manager.GetComponent<GameManager>().SetGimmickFlag(false);
                        m_count_obj.GetComponent<CountDown>().SetCount();
                        m_wait_time = 1.0f;
                        m_isHeat = m_isHeat ? false : true;
                        m_dis_color = m_isHeat ? new Color(0.6f, 0.6f, 0.6f, 1.0f)
                                                : new Color(1.0f, 0.0f, 0.0f, 1.0f);
                        m_start_color = m_material.color;
                    }
                }
                else
                {
                    m_action_flag = false;
                    m_old_flag = false;
                    m_game_manager.GetComponent<GameManager>().SetGimmickFlag(false);
                    m_count_obj.GetComponent<CountDown>().SetCount();
                    m_wait_time = 1.0f;
                    m_isHeat = m_isHeat ? false : true;
                    m_dis_color = m_isHeat ? new Color(0.6f, 0.6f, 0.6f, 1.0f)
                                            : new Color(1.0f, 0.0f, 0.0f, 1.0f);
                    m_start_color = m_material.color;
                }
            }
        }

        //石の状態で各要素を変更
        if(m_isHeat)
        {
            //マテリアルを変更
            //m_rock_obj.GetComponent<MeshRenderer>().material = m_material[0];
            //熱当たり判定を変更
            //m_heat_obj.SetActive(true);
            //タグを変更
            this.tag = "Break";
        }
        else
        {
            //マテリアルを変更
            //m_rock_obj.GetComponent<MeshRenderer>().material = m_material[1];
            //熱当たり判定を変更
            //m_heat_obj.SetActive(false);
            //タグを変更
            this.tag = "Block";
        }

    }

    //カウントダウンによってフラグをあげる
    void GetCountZero()
    {
        //カウントが0になり、フラグがたっておらず、いままで一度もフラグが立っていなかったら
        if (m_count_obj.GetComponent<CountDown>().GetCount() == 0 &&
            m_action_flag == false &&
            m_old_flag == false)
        {
            //ゲームマネージャーのギミックが移動している判定用のフラグをあげる
            m_game_manager.GetComponent<GameManager>().SetGimmickFlag(true);

            //待機時間があるなら待機する
            if (m_wait_time > 0.0f)
            {
                //60fps
                m_wait_time -= 1 / 60.0f;
            }
            else
            {

                AudioSource audioSource = GameObject.Find("GimmickAudio").GetComponent<AudioSource>();
                audioSource.PlayOneShot(GimmickSound);

                //フラグを上げる
                m_action_flag = true;
            }
        }
    }
}
