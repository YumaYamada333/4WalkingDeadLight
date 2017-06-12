using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakBox : MonoBehaviour {

    //アクション量
    public Vector3 m_act_distance = Vector3.zero;
    //待機時間
    public float m_wait_time = 0.0f;
    //スイッチオブジェクト
    public GameObject m_switch_obj;
    //動くオブジェクト
    public GameObject m_move_obj;
    //ゲームマネージャー
    public GameObject m_manager;
    /*ギミック音*/
    public AudioClip GimmickSound;

    //スタート時の時間,位置
    private float m_start_time;
    private Vector3 m_start_pos;

    //フラグ
    bool m_act_flag = false;
    bool m_old_flag = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //スイッチが消えたらフラグを上げる
        //GetCountZero();
        if (m_switch_obj == null &&
            m_act_flag == false &&
            m_old_flag == false)
        {
            //ゲームマネージャーのギミックが移動している判定用のフラグをあげる
            //GameObject manager = GameObject.Find("GameManager");
            /*manager*/
            m_manager.GetComponent<GameManager>().SetGimmickFlag(true);

            //待機時間があるなら待機する
            if (m_wait_time > 0.0f)
            {
                //60fps
                m_wait_time -= 1 / 60.0f;
            }
            else
            {
                /*ギミック音を鳴らす*/
                AudioSource audioSource = GameObject.Find("GimmickAudio").GetComponent<AudioSource>();
                audioSource.PlayOneShot(GimmickSound);

                //フラグを上げる
                m_act_flag = true;
            }
        }


        //フラグが立ったらアクションの準備
        if (m_act_flag == true &&
            m_act_flag != m_old_flag)
        {
            m_start_time = Time.time;
            m_start_pos = m_move_obj.transform.position;
            m_old_flag = m_act_flag;
        }
        //フラグがたっていたら移動(補間)
        if (m_act_flag && m_act_flag == m_old_flag)
        {
            //経過時間を移動時間で割る
            float timeStep = (Time.time - m_start_time);

            m_move_obj.transform.position = (1 - timeStep) * m_start_pos + timeStep * (m_start_pos + m_act_distance);

            //移動時間になったらフラグを止める
            if (timeStep > 1.0f)
            {
                m_act_flag = false;

                //GameObject manager = GameObject.Find("GameManager");
                /*manager*/
                m_manager.GetComponent<GameManager>().SetGimmickFlag(false);
            }
        }
    }
}
