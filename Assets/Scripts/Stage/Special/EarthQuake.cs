using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthQuake : MonoBehaviour {

    //カウントオブジェ
    public GameObject m_count_obj;
    public GameObject m_count_obj2;
    public GameObject m_count_obj3;
    //今のカウントオブジェ
    private GameObject m_now_count;
    //アクションカウント
    private int m_act_count = 0;

    //初期ポジション
    private Vector3 m_init_pos;
    //スタートタイム
    private float m_start_time = 0.0f;
    //timeカウント
    private int m_time_count = 0;

    //フラグ
    private bool m_old_flag = false;
    private bool m_action_flag = false;
    public bool m_repeat_flag = false;

    // Use this for initialization
    void Start()
    {
        m_now_count = m_count_obj;
    }

    // Update is called once per frame
    void Update()
    {
        //カウントダウン
        GetCountZero();

        //アクションがnullでないならアクションを行う

        //フラグが立ったらアクションの準備
        if (m_action_flag == true &&
            m_action_flag != m_old_flag)
        {
            m_init_pos = this.transform.position;
            m_start_time = Time.time;
            m_old_flag = m_action_flag;
        }
        //アクションの実行
        //フラグがたっていたら移動(補間)
        if (m_action_flag)
        {
            //経過時間を移動時間で割る
            float timeStep = (Time.time - m_start_time) / 1.5f;
            //タイムカウントを処理
            m_time_count += 40;
            if (m_time_count >= 360) m_time_count = 0;

            //移動量
            Vector3 move_dis = new Vector3(Mathf.Sin(m_time_count * Mathf.Deg2Rad) * 0.1f, 0.0f, 0.0f);
            this.transform.position = m_init_pos + move_dis;

            //移動時間になったらフラグを止める
            if (timeStep > 1.0f)
            {
                //初期位置に戻す
                this.transform.position = m_init_pos;

                m_action_flag = false;
                if (m_repeat_flag)
                {
                    m_old_flag = false;
                    m_act_count++;
                    if(m_act_count == 1)
                    {
                        m_now_count = m_count_obj2;
                    }
                    else if(m_act_count == 2)
                    {
                        m_now_count = m_count_obj3;
                    }
                    else
                    {
                        m_old_flag = true;
                    }
                }
            }
        }

    }

    //カウントダウンによってフラグをあげる
    void GetCountZero()
    {
        //カウントが0になり、フラグがたっておらず、いままで一度もフラグが立っていなかったら
        if (m_now_count.GetComponent<CountDown>().GetCount() == 0 &&
            m_action_flag == false &&
            m_old_flag == false)
        {
            //フラグを上げる
            m_action_flag = true;
        }
    }
}
