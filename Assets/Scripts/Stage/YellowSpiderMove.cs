using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowSpiderMove : MonoBehaviour {

    public Vector3 m_end_pos1;
    public Vector3 m_middle_pos;
    public Vector3 m_end_pos2;

    public GameObject m_rot_obj;

    //アクション量
    [SerializeField]
    private Vector3 m_act_distance = Vector3.zero;
    //移動時間
    public float m_act_time = 1.0f;

    private int m_go_pos = 0;

    private float m_degree;
    private float m_start_degree;

    //カウントを行うオブジェクト
    public GameObject m_count_obj;
    //ゲームマネージャー
    public GameObject m_game_manager;

    private bool m_old_flag = false;
    private bool m_rot_old_flag = false;
    private bool m_action_flag = false;
    private bool m_rot_act_flag = false;
    private bool m_awake_flag = false;

    //アクションの実行カウント
    private int m_action_count = 1;

    //GameObjectでColliderと触れているの要素数
    private List<GameObject> ride = new List<GameObject>();
    private Vector3 m_correction_value_ride;

    //スタートの時間
    private float m_start_time = 0.0f;
    //スタート位置
    private Vector3 m_start_pos = Vector3.zero;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //カウントダウン
        GetCountZero();
        
        //フラグが立ったらアクションの準備
        if (m_action_flag == true &&
            m_action_flag != m_old_flag)
        {
            MoveInit();
        }
        //アクションの実行
        MoveUpdate();
        //if(m_rot_act_flag == true &&
        //    m_rot_old_flag != m_old_flag)
        //{
        //    RotInit();
        //}
        //RotUpdate();
        //GameObjectの移動
        foreach (GameObject otherObj in ride)
        {
            if (m_game_manager.GetComponent<GameManager>().GetGimmickFlag())
            {
                //動く床の位置にObjectの座標を合わせる
                Vector3 v = otherObj.transform.position;
                otherObj.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + m_correction_value_ride.y/*v.y*/, v.z);
            }
        }
    }

    //移動量を設定
    void SetActDistance()
    {
        if(m_action_count == 0)
        {
            m_act_distance = m_middle_pos - this.transform.position;
            if(m_go_pos == 0)
            {
                m_degree = 115.0f;
            }
            else
            {
                m_degree = -125.0f;
            }
        }
        else
        {
            if (m_go_pos == 0)
            {
                m_act_distance = m_end_pos1 - this.transform.position;
                m_degree = -180.0f;
            }
            else
            {
                m_act_distance = m_end_pos2 - this.transform.position;
                m_degree = 180.0f;
            }
        }
        
    }

    void MoveInit()
    {
        //現在地と時間を取得
        m_start_time = Time.time;
        m_start_pos = this.transform.position;

        //移動量を設定
        SetActDistance();

        m_old_flag = m_action_flag;
    }

    void MoveUpdate()
    {
        //フラグがたっていたら移動(補間)
        if (m_action_flag)
        {
            //経過時間を移動時間で割る
            float timeStep = (Time.time - m_start_time) / m_act_time;

            this.transform.position = (1 - timeStep) * m_start_pos + timeStep * (m_start_pos + m_act_distance);

            //移動時間になったらフラグを止める
            if (timeStep > 1.0f)
            {
                m_action_flag = false;
                m_awake_flag = false;
                //微調整
                this.transform.position = m_start_pos + m_act_distance;

                //m_game_manager.GetComponent<GameManager>().SetGimmickFlag(false);

                //m_rot_act_flag = true;

                //繰り返しパターン用初期化
                //アクション実行回数を増やす
                m_action_count++;

                //カウント初期化
                m_count_obj.GetComponent<CountDown>().SetCount();
                //繰り返しフラグをセット
                m_old_flag = false;

                //反転フラグがたっている,かつタイミングになっていたら
                if (m_action_count >= 2)
                {
                    //アクションカウントを初期化
                    m_action_count = 0;

                    //移動先を変更
                    m_go_pos = 1;
                }
            }
        }
    }

    void RotInit()
    {
        m_start_time = Time.time;
        m_start_degree = m_rot_obj.transform.rotation.x;

        m_rot_old_flag = m_rot_act_flag;
    }

    void RotUpdate()
    {
        //フラグがたっていたら移動(補間)
        if (m_rot_act_flag && m_rot_act_flag == m_rot_old_flag)
        {
            //経過時間を移動時間で割る
            //float timeStep = (Time.time - m_start_time) / m_act_time;

            //回転
           // float obj_deg = timeStep * (m_degree);
            m_rot_obj.transform.Rotate(new Vector3(0,0,1), m_degree);

            //移動時間になったらフラグを止める
            //if (m_rot_obj.transform.rotation.x == obj_deg + m_degree)
            //{
                m_rot_act_flag = false;

                //GameObject manager = GameObject.Find("GameManager");
                /*manager*/
                m_game_manager.GetComponent<GameManager>().SetGimmickFlag(false);

                //PartTim = true;

                //繰り返しパターン用初期化
                //カウント初期化
                m_count_obj.GetComponent<CountDown>().SetCount();
                //繰り返しフラグをセット
                m_old_flag = false;
                m_rot_old_flag = false;

            //}
            //else
            //{
            //    PartTim = false;
            //}
        }
    }

    //カウントダウンによってフラグをあげる
    void GetCountZero()
    {
        //カウントが0になり、フラグがたっておらず、いままで一度もフラグが立っていなかったら
        if (/*m_obj.transform.FindChild("CountUI")*/m_count_obj.GetComponent<CountDown>().GetCount() == 0 &&
            m_action_flag == false &&
            m_old_flag == false)
        {
            //ゲームマネージャーのギミックが移動している判定用のフラグをあげる
            //GameObject manager = GameObject.Find("GameManager");
            /*manager*/
            m_game_manager.GetComponent<GameManager>().SetGimmickFlag(true);
            //起動フラグを上げる
            m_awake_flag = true;

            //フラグを上げる
            m_action_flag = true;
        }

    }

    //他のObjectが接触している時
    void OnTriggerEnter(Collider otherObj)
    {
        ride.Add(otherObj.gameObject);
        m_correction_value_ride = otherObj.transform.position - this.transform.position;
    }
    //他のObjectが離れている時
    void OnTriggerExit(Collider otherObj)
    {
        //床から離れたので削除
        ride.Remove(otherObj.gameObject);
    }
}
