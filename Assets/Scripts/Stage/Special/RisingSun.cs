using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RisingSun : MonoBehaviour {

    //移動時間
    public float m_act_time = 1.0f;

    //ゴースト君
    public GameObject[] m_ghost;
    //カウントを行うオブジェクト
    public GameObject m_count_obj;
    //ゲームマネージャー
    public GameObject m_game_manager;

    //フラグ
    private bool m_old_flag = false;
    private bool m_action_flag = false;
    private bool m_awake_flag = false;

    //オブジェクトを格納するための変数
    public GameObject m_obj;

    //スタートの時間
    private float m_start_time = 0.0f;
    //スタート位置
    private float m_start_pow = 0.0f;
    //スタート時の透明度
    private SkinnedMeshRenderer[] m_mesh = new SkinnedMeshRenderer[4];
    private Color[] m_color = new Color[4];

    // Use this for initialization
    void Start () {
        for(int i =0;i<4;i++)
        {
            m_mesh[i] = m_ghost[i].GetComponent<SkinnedMeshRenderer>();
            m_color[i] = m_mesh[i].material.color;
        }
	}

    // Update is called once per frame
    void Update()
    {
        //カウントダウン
        GetCountZero();
        
        //フラグが立ったらアクションの準備
        if (m_action_flag == true &&
            m_action_flag != m_old_flag)
        {
            //現在地と時間を取得
            m_start_time = Time.time;
            m_start_pow = m_obj.GetComponent<Light>().intensity;

            m_old_flag = m_action_flag;
        }
        //アクションの実行
        //フラグがたっていたら移動(補間)
        if (m_action_flag)
        {
            //経過時間を移動時間で割る
            float timeStep = (Time.time - m_start_time) / m_act_time;

            m_obj.GetComponent<Light>().intensity = (1 - timeStep) * m_start_pow + timeStep * (1);
            for (int i = 0; i < m_ghost.Length; i++)
            {
                m_mesh[i].material.color = new Color(m_color[i].r, m_color[i].g, m_color[i].b, (1 - timeStep) * 1 + 0.4f);
            }

            //移動時間になったらフラグを止める
            if (timeStep > 1.0f)
            {
                m_action_flag = false;

               m_game_manager.GetComponent<GameManager>().SetGimmickFlag(false);
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
            //ゲームマネージャーのギミックが移動している判定用のフラグをあげる
            m_game_manager.GetComponent<GameManager>().SetGimmickFlag(true);
            //起動フラグを上げる
            m_awake_flag = true;
            
            //フラグを上げる
            m_action_flag = true;
        }

    }
}
