using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WatchController : MonoBehaviour {

    private GameObject m_minuteHand;                // 分針
    private GameObject m_player;                    // プレイヤー座標
    private bool m_effectFlag = false;              // 演出発生フラグ
    private float m_time = 0.0f;                    // 初期時間
    private Vector3 m_pos;                          // プレイヤー位置保存用             

    // Use this for initialization
    void Start () {
        m_minuteHand = transform.FindChild("WatchCanvas").FindChild("watch2").gameObject;
        m_player = GameObject.Find("unitychan");
        gameObject.SetActive(false);

        // 時間を取得
        m_time = Time.time;
    }
	
	// Update is called once per frame
	void Update () {
        transform.position = m_player.transform.position + new Vector3(0, 3, 0);

        if (m_effectFlag)
        {
            float timeStep = (Time.time - m_time) / 1;
            float timeStep2 = (Time.time - m_time) / 0.5f;

            if (timeStep <= 1)
            {
                m_minuteHand.transform.rotation = Quaternion.Euler(Vector3.Lerp(new Vector3(0, 0, 0), new Vector3(0, 0, 363), timeStep) * -1);
                transform.localScale = Vector3.Lerp(new Vector3(0.5f, 0.5f, 0.5f), new Vector3(1, 1, 1), timeStep2);
                transform.localPosition = Vector3.Lerp(m_pos + new Vector3(0, 1, 0), m_pos + new Vector3(0, 3, 0), timeStep2);
            }
            else
            {
                m_effectFlag = false;
                gameObject.SetActive(false);
            }
        }
    }

    public void CountWatchEffect()
    {
        m_effectFlag = true;
        gameObject.SetActive(true);
        m_time = Time.time;
        m_pos = m_player.transform.position;
        m_minuteHand.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        transform.localPosition = m_pos + new Vector3(0,1,0);
    }
}
