using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VignetScale : MonoBehaviour {

    //アクション量
    public Vector3 m_act_distance = Vector3.zero;
    //移動時間
    public float m_act_time = 0.5f;
    //ゲームマネージャー
    public GameObject m_game_manager;

    //スタートの時間
    private float m_start_time = 0.0f;
    //スタート時の大きさ
    private Vector3 m_start_scale = Vector3.zero;

    //フラグ
    private bool m_old_flag = false;
    private bool m_action_flag = false;
    private bool m_old_gimmick = false;

    //自分を取得するための変数
    public GameObject m_obj;

    // Use this for initialization
    void Start()
    {
        //自身を取得
        m_obj = this.gameObject;

        m_old_gimmick = m_game_manager.GetComponent<GameManager>().GetGimmickFlag();
    }

    // Update is called once per frame
    void Update()
    {
        //カウントが0になり、フラグがたっておらず、いままで一度もフラグが立っていなかったら
        if (m_game_manager.GetComponent<GameManager>().GetGimmickFlag() != m_old_gimmick &&
            m_action_flag == false &&
            m_old_flag == false)
        {
            //フラグを上げる
            m_action_flag = true;
            m_old_gimmick = m_game_manager.GetComponent<GameManager>().GetGimmickFlag();
        }
        
        //フラグが立ったらアクションの準備
        if (m_action_flag == true &&
            m_action_flag != m_old_flag)
        {
            //現在地と時間を取得
            m_start_time = Time.time;
            m_start_scale = m_obj.transform.localScale;
            m_old_flag = m_action_flag;
        }

        //フラグがたっていたらスケール(補間)
        if (m_action_flag)
        {
            //経過時間を移動時間で割る
            float timeStep = (Time.time - m_start_time) / m_act_time;

            m_obj.transform.localScale = (1 - timeStep) * m_start_scale + timeStep * (m_start_scale + m_act_distance);

            if(m_old_gimmick)
            {
                if (m_obj.transform.localScale.x <= (m_start_scale + m_act_distance).x
                    && m_obj.transform.localScale.y <= (m_start_scale + m_act_distance).y)
                {
                    m_obj.transform.localScale = (m_start_scale + m_act_distance);
                } 
            }

            //移動時間になったらフラグを止める
            if (timeStep > 1.0f)
            {
                m_action_flag = false;
                m_old_flag = false;
                //移動量を反転
                m_act_distance *= -1;
            }
        }

    }
}
