using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FroatText : MonoBehaviour {

    //スイッチオブジェクト
    public GameObject m_switch_obj;

    // テキストコンポーネント
    private Text m_text;
    //トランスフォーム
    private RectTransform m_trans;
    //初期位置
    private Vector3 m_init_pos;

    private int m_count = 0;

    // Use this for initialization
    void Start () {
        //テキストを取得
        m_text = GetComponent<Text>();
        m_trans = GetComponent<RectTransform>();
        m_init_pos = m_trans.transform.localPosition;
    }
	
	// Update is called once per frame
	void Update () {
        if(m_switch_obj != null)
        {
            //テキストの変更
            if (m_text != null)
                m_text.text = "";
        }
        else
        {
            //テキストの変更
            if (m_text != null)
                m_text.text = "BYE";
        }
        m_count += 2;
        if (m_count >= 360) m_count = 0;

        Vector3 dis = new Vector3(0.0f, Mathf.Sin(m_count * Mathf.Deg2Rad) * 0.1f + 0.6f, 0.0f);
        m_trans.transform.localPosition = m_init_pos + dis;
    }
}
