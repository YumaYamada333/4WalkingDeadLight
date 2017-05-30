using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetActDistance : MonoBehaviour {

    //アクション量を変えるオブジェクト
    public GameObject m_act_obj;

    //中心点をそろえるために必要な補正値
    public Vector3 m_correction_value;

    //移動量
    private Vector3 m_distance;

    //対象オブジェクトの初期位置
    private Vector3 m_init_pos;

	// Use this for initialization
	void Start () {
        m_init_pos = m_act_obj.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        if(!m_act_obj.GetComponent<ActionCountDown>().GetAwakeFlag())
        {
            if(m_act_obj.transform.position == m_init_pos)
            {
                m_distance = (this.transform.position + m_correction_value) - m_act_obj.transform.position;
            }
            else
            {
                m_distance = m_init_pos - m_act_obj.transform.position;
            }

            m_act_obj.GetComponent<ActionCountDown>().SetActDistance(m_distance);
        }
	}

    public void Set()
    {
       
    }
}
