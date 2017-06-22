//ステージ9の落ちるフルーツのスクリプト

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallLava : MonoBehaviour
{

    //アクション量
    public Vector3 m_fall_distance = Vector3.zero;
    public Vector3 m_fall_scale = Vector3.zero;

    //移動時間
    public float m_fall_act_time = 1.0f;
    public float m_scale_act_time = 1.0f;
    public float m_return_act_time = 1.0f;

    //待機時間
    public float m_wait_time = 0.0f;
    private float m_init_wait = 0.0f;

    //ゲームマネージャー
    public GameObject m_game_manager;
    //カウントを行うオブジェクト
    public GameObject m_count_obj;
    //パーティクル
    public GameObject m_particle;

    //スタート時の時間,位置
    private float m_fall_start_time;
    private float m_scale_start_time;
    private float m_return_start_time;

    private Vector3 m_init_pos;
    private Vector3 m_start_scale;

    //フラグ
    public bool m_awake_flag = false;

    private bool m_old_flag = false;
    private bool m_scale_old_flag = false;
    private bool m_return_old_flag = false;

    private bool m_action_flag = false;
    private bool m_scale_action_flag = false;
    private bool m_return_action_flag = false;

    //自分を取得するための変数
    GameObject m_obj;

    /*ギミック音*/
    public AudioClip DropSound;
    public AudioClip MagmaSound;

    //パーティクルフラグ取得
    public bool PartTim;

    // Use this for initialization
    void Start()
    {
        //自身を取得
        m_obj = this.gameObject;
        //初期位置を取得
        m_init_pos = m_obj.transform.position;
        //初期待機時間を取得
        m_init_wait = m_wait_time;
    }

    // Update is called once per frame
    void Update()
    {
        //カウントでフラグを上げる
        GetCountZero();

        //フラグが立ったらアクションの準備
        if (m_action_flag == true &&
            m_action_flag != m_old_flag)
        {
            FallInit();
            m_old_flag = m_action_flag;
        }
        FallAct();
        if (m_scale_action_flag == true &&
            m_scale_action_flag != m_scale_old_flag)
        {
            ScaleInit();
            m_scale_old_flag = m_scale_action_flag;
        }
        ScaleAct();
        if (m_return_action_flag == true &&
            m_return_action_flag != m_return_old_flag)
        {
            ReturnInit();
            m_return_old_flag = m_return_action_flag;
        }
        ReturnAct();
    }

    void FallInit()
    {
        m_fall_start_time = Time.time;
    }

    void ScaleInit()
    {
        m_scale_start_time = Time.time;
        m_start_scale = m_obj.transform.localScale;
    }

    void ReturnInit()
    {
        //現在地を設定
        m_obj.transform.position = m_init_pos;

        //現在のスケールと時間を取得
        m_return_start_time = Time.time;
        m_start_scale = m_obj.transform.localScale;
        // m_move_start_pos = m_move_stone.transform.position;
    }

    void FallAct()
    {
        //  フラグがたっていたら移動(補間)
        if (m_action_flag && m_action_flag == m_old_flag)
        {
            //経過時間を移動時間で割る
            float timeStep = (Time.time - m_fall_start_time) / m_fall_act_time;

            m_obj.transform.position = (1 - timeStep) * m_init_pos + timeStep * (m_init_pos + m_fall_distance);

            //移動時間になったらフラグを止める
            if (timeStep > 1.0f)
            {
                /*ギミック音を鳴らす*/
                AudioSource audioSource = GameObject.Find("GimmickAudio").GetComponent<AudioSource>();
                audioSource.PlayOneShot(MagmaSound);
                audioSource.PlayOneShot(MagmaSound);
                audioSource.PlayOneShot(MagmaSound);
                audioSource.PlayOneShot(MagmaSound);

                //パーティクルを再生
                m_particle.GetComponent<ParticleSystem>().Play();

                m_action_flag = false;

                m_scale_action_flag = true;
                //m_game_manager.GetComponent<GameManager>().SetGimmickFlag(false);


                //GameObject manager = GameObject.Find("GameManager");
                /*manager*/
                //obj.GetComponent<ActionCountDown>().m_game_manager.GetComponent<GameManager>().SetGimmickFlag(false);
            }
        }
    }

    void ScaleAct()
    {
        //フラグがたっていたらスケーリング(補間)
        if (m_scale_action_flag && m_scale_action_flag == m_scale_old_flag)
        {
            //経過時間を移動時間で割る
            float timeStep = (Time.time - m_scale_start_time) / m_scale_act_time;

            //スケーリング
            m_obj.transform.localScale = (1 - timeStep) * m_start_scale + timeStep * (m_start_scale + m_fall_scale);

            //時間になったらフラグを止める
            if (timeStep > 1.0f)
            {
                m_scale_action_flag = false;

                m_return_action_flag = true;
            }
        }
    }

    void ReturnAct()
    {
        //フラグがたっていたらスケーリング(補間)
        if (m_return_action_flag && m_return_action_flag == m_return_old_flag)
        {
            //経過時間を行動時間で割る
            float timeStep = (Time.time - m_return_start_time) / m_return_act_time;

            //スケーリング
            m_obj.transform.localScale = (1 - timeStep) * m_start_scale + timeStep * (m_start_scale - m_fall_scale);

            //時間になったらフラグを止める
            if (timeStep > 1.0f)
            {
                m_return_action_flag = false;

                m_game_manager.GetComponent<GameManager>().SetGimmickFlag(false);

                //カウント初期化
                m_count_obj.GetComponent<CountDown>().SetCount();

                m_awake_flag = false;
                m_old_flag = false;
                m_scale_old_flag = false;
                m_return_old_flag = false;
                m_wait_time = m_init_wait;
            }
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
            //待機時間があるなら待機する
            if (m_wait_time > 0.0f)
            {
                //60fps
                m_wait_time -= 1 / 60.0f;
            }
            else
            {
                //ゲームマネージャーのギミックが移動している判定用のフラグをあげる
                m_game_manager.GetComponent<GameManager>().SetGimmickFlag(true);
                //起動フラグを上げる
                m_awake_flag = true;


                //フラグを上げる
                m_action_flag = true;
            }
        }
    }
}