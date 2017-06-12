using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class SleepTextController: MonoBehaviour {

    // テキストコンポーネント
    private Text m_text;
    //トランスフォーム
    private RectTransform m_trans;
    //アクションするオブジェクト
    public GameObject m_act_obj;
    //テキストの色
    private Color m_color = Color.clear;

    private int m_count = 0;

    // Use this for initialization
    void Start () {
        //テキストを取得
		m_text = GetComponent<Text>();
        m_trans = GetComponent<RectTransform>();
        m_text.color = m_color;
        m_color = Color.blue;
    }

    
	
	// Update is called once per frame
	void Update () {

        //アクションカウントが０
        if (m_act_obj.GetComponent<LinkActionCountDown>().GetActionCount() == 0)
        {
            //アクションフラグがたっていない
            if(!m_act_obj.GetComponent<LinkActionCountDown>().GetActionFlag())
            {
                m_count+= 2;
                if (m_count >= 360) m_count = 0;

                m_trans.transform.localPosition = new Vector3(0.0f, Mathf.Sin(m_count * Mathf.Deg2Rad) * 0.1f + 0.6f ,0.0f);

            }
            else
            {
                m_trans.transform.localPosition = new Vector3(0.2f, 0.7f, 0.0f);
                //テキストの変更
                if (m_text != null)
                    m_text.text = "!";
                m_color = Color.red;
            }
        }
        else
        {
            //テキストの変更
            if (m_text != null)
                m_text.text = "";
        }

        //色の変更
        m_text.color = m_color;
    }
}
