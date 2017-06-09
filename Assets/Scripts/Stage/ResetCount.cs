using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetCount : MonoBehaviour {

    private GameObject m_obj;
    public GameObject m_manager;
    private int m_wait_frame = 0;

	// Use this for initialization
	void Start () {
        m_obj = this.gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		if(m_obj.GetComponent<CountDown>().GetCount() == 0)
        {
            if(m_wait_frame < 2)
            {
                m_wait_frame++;
            }
            else
            {
                if (!m_manager.GetComponent<GameManager>().GetGimmickFlag())
                {
                    m_obj.GetComponent<CountDown>().SetCount();
                    m_wait_frame = 0; 
                }
            }

        }
	}
}
