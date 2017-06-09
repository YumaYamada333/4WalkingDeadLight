using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountDownManager : MonoBehaviour {

    //カウントダウンするゲームオブジェクト
    public GameObject[] m_countObj = new GameObject[1];
    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //ゲームマネージャーにて全てのカウントを処理
    public void ManagerCountDown(CountDown.CountType flag)
    {
        for (int i = 0; i < m_countObj.Length; i++)
        {
            for (int j = 0; j < m_countObj[i].GetComponent<CountDown>().m_countType.Length; j++)
            {
                if (m_countObj[i].GetComponent<CountDown>().m_countType[j] == flag &&
                      m_countObj[i].GetComponent<CountDown>().m_countType[j] != CountDown.CountType.Nothing)
                {
                    m_countObj[i].GetComponent<CountDown>().CountMin();
                }
            }
        }
    }
}
