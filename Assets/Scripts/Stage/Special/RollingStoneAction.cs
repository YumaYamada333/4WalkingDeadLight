//ステージ9のフルーツのスクリプト

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollingStoneAction : MonoBehaviour
{

    //アクション量
    public Vector3 m_bridge_act_distance = Vector3.zero;
    public Vector3 m_roll_stone_act_distance = Vector3.zero;
    public Vector3 m_move_stone_act_distance = Vector3.zero;
    public Vector3 m_fall_stone_act_distance = Vector3.zero;
    //移動時間
    public float m_bridge_act_time = 1.0f;
    public float m_roll_stone_act_time = 1.0f;
    public float m_move_stone_act_time = 1.0f;
    public float m_fall_stone_act_time = 1.0f;
    //待機時間
    public float m_wait_time = 0.0f;
    ////繰り返しフラグ
    //[SerializeField]
    //private bool m_repeat_flag;
    ////反転フラグ
    //[SerializeField]
    //private bool m_back_flag;
    ////反転タイミング
    //[SerializeField, Range(1, 10)]
    //private int m_back_timing = 1;
    //アクションの種類
    //[SerializeField, Range(0, 3)]
    //private int m_action_type;
    ////カウントを行うオブジェクト
    //public GameObject m_count_obj;
    //ゲームマネージャー
    public GameObject m_game_manager;
    //スイッチ
    public GameObject m_switch_obj;

    //動かすオブジェクト
    public GameObject m_bridge;
    public GameObject m_roll_stone;
    public GameObject m_move_stone;
    public GameObject m_destroy_obj;

    //スタート時の時間,位置
    private float m_bridge_start_time;
    private float m_roll_start_time;
    private float m_move_start_time;
    private float m_fall_start_time;

    private Vector3 m_move_start_pos;
    private Vector3 m_fall_start_pos;

    //フラグ
    private bool m_awake_flag = false;

    private bool m_old_flag = false;
    private bool m_roll_old_flag = false;
    private bool m_move_old_flag = false;
    private bool m_fall_old_flag = false;
   
    private bool m_action_flag = false;
    private bool m_roll_action_flag = false;
    private bool m_move_action_flag = false;
    private bool m_fall_action_flag = false;

    //自分を取得するための変数
    GameObject m_obj;

    //アクション
    //private BlockAction action = null;
    //GameObjectでColliderと触れているの要素数
    //private List<GameObject> ride = new List<GameObject>();

    //パーティクルフラグ取得
    public bool PartTim;

    // Use this for initialization
    void Start()
    {
        //自身を取得
        m_obj = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        //スイッチが消えたらフラグを上げる
        //GetCountZero();
        if (m_switch_obj == null &&
            m_action_flag == false &&
            m_old_flag == false)
        {
            //ゲームマネージャーのギミックが移動している判定用のフラグをあげる
            //GameObject manager = GameObject.Find("GameManager");
            /*manager*/
            m_game_manager.GetComponent<GameManager>().SetGimmickFlag(true);
            //起動フラグを上げる
            m_awake_flag = true;

            //待機時間があるなら待機する
            if (m_wait_time > 0.0f)
            {
                //60fps
                m_wait_time -= 1 / 60.0f;
            }
            else
            {
                //フラグを上げる
                m_action_flag = true;
            }
        }

        
        //フラグが立ったらアクションの準備
        if (m_action_flag == true &&
            m_action_flag != m_old_flag)
        {
            BridgeInit();
            m_old_flag = m_action_flag;
        }
        BridgeAct();
        if (m_roll_action_flag == true &&
            m_roll_action_flag != m_roll_old_flag)
        {
            RollInit();
            m_roll_old_flag = m_roll_action_flag;
        }
        RollStoneAct();
        if (m_move_action_flag == true &&
            m_move_action_flag != m_move_old_flag)
        {
            MoveInit();
            m_move_old_flag = m_move_action_flag;
        }
        MoveStoneAct();
        if (m_fall_action_flag == true &&
            m_fall_action_flag != m_fall_old_flag)
        {
            FallInit();
            m_fall_old_flag = m_fall_action_flag;
        }
        FallStoneAct();
    }

    void BridgeInit()
    {
        m_bridge_start_time = Time.time;
    }

    void RollInit()
    {
        m_roll_start_time = Time.time;
    }

    void MoveInit()
    {
        //現在地と時間を取得
        m_move_start_time = Time.time;
        m_move_start_pos = m_move_stone.transform.position;
    }
    void FallInit()
    {
        //現在地と時間を取得
        m_fall_start_time = Time.time;
        m_fall_start_pos = m_move_stone.transform.position;
    }

        void BridgeAct()
    {
        //  フラグがたっていたら移動(補間)
        if (m_action_flag && m_action_flag == m_old_flag)
        {
            //経過時間を移動時間で割る
            float timeStep = (Time.time - m_bridge_start_time) / m_bridge_act_time;

            //回転
            Vector3 obj_deg = timeStep * (m_bridge_act_distance);
            m_bridge.transform.rotation = Quaternion.Euler(obj_deg);

            //移動時間になったらフラグを止める
            if (timeStep > 1.0f)
            {
                m_action_flag = false;

                //GameObject manager = GameObject.Find("GameManager");
                /*manager*/
                //obj.GetComponent<ActionCountDown>().m_game_manager.GetComponent<GameManager>().SetGimmickFlag(false);

                //PartTim = true;
                m_roll_action_flag = true;
                m_move_action_flag = true;

            }
        }
    }

    void RollStoneAct()
    {
        //  フラグがたっていたら移動(補間)
        if (m_roll_action_flag && m_roll_action_flag == m_roll_old_flag)
        {
            //経過時間を移動時間で割る
            float timeStep = (Time.time - m_roll_start_time) / m_roll_stone_act_time;

            //回転
            Vector3 obj_deg = timeStep * (m_roll_stone_act_distance);
            m_roll_stone.transform.rotation = Quaternion.Euler(obj_deg);

            //移動時間になったらフラグを止める
            if (timeStep > 1.0f)
            {
                m_roll_old_flag = false;
            }
        }
    }

    void MoveStoneAct()
    {
        //フラグがたっていたら移動(補間)
        if (m_move_action_flag && m_move_action_flag == m_move_old_flag)
        {
            //経過時間を移動時間で割る
            float timeStep = (Time.time - m_move_start_time) / m_move_stone_act_time;

            m_move_stone.transform.position = (1 - timeStep) * m_move_start_pos + timeStep * (m_move_start_pos + m_move_stone_act_distance);

            //移動時間になったらフラグを止める
            if (timeStep > 1.0f)
            {
                m_move_action_flag = false;
                m_roll_action_flag = false;

                m_fall_action_flag = true;
                //GameObject manager = GameObject.Find("GameManager");
                /*manager*/
                //obj.GetComponent<ActionCountDown>().m_game_manager.GetComponent<GameManager>().SetGimmickFlag(false);
            }
        }
    }

    void FallStoneAct()
    {
        //フラグがたっていたら移動(補間)
        if (m_fall_action_flag && m_fall_action_flag == m_fall_old_flag)
        {
            //経過時間を移動時間で割る
            float timeStep = (Time.time - m_fall_start_time) / m_fall_stone_act_time;

            m_move_stone.transform.position = (1 - timeStep) * m_fall_start_pos + timeStep * (m_fall_start_pos + m_fall_stone_act_distance);

            //移動時間になったらフラグを止める
            if (timeStep > 1.0f)
            {
                m_fall_action_flag = false;

                Destroy(m_destroy_obj);
                m_game_manager.GetComponent<GameManager>().SetGimmickFlag(false);


                //GameObject manager = GameObject.Find("GameManager");
                /*manager*/
                //obj.GetComponent<ActionCountDown>().m_game_manager.GetComponent<GameManager>().SetGimmickFlag(false);
            }
        }
    }

    //自身を破棄
    public void DestroyObject()
    {
        Destroy(this.gameObject);
    }
    
}
