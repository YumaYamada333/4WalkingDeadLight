using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollPanelRide : MonoBehaviour {

    //GameObjectでColliderと触れているの要素数
    private List<GameObject> ride = new List<GameObject>();
    private Vector3 m_correction_value_ride;

    public GameObject m_game_manager;
    private Collider m_coll;

    // Use this for initialization
    void Start () {
        m_coll = this.GetComponent<Collider>();
	}
	
	// Update is called once per frame
	void Update () {
        //GameObjectの移動
        foreach (GameObject otherObj in ride)
        {
            if (m_game_manager.GetComponent<GameManager>().GetGimmickFlag())
            {
                //動く床の位置にObjectの座標を合わせる
                Vector3 v = otherObj.transform.position;
                otherObj.transform.position = new Vector3(m_coll.transform.position.x, m_coll.transform.position.y + m_correction_value_ride.y/*v.y*/, m_coll.transform.position.z);
            }
        }
    }

    //他のObjectが接触している時
    void OnTriggerEnter(Collider otherObj)
    {
        ride.Add(otherObj.gameObject);
        m_correction_value_ride = otherObj.transform.position - m_coll.transform.position;
    }
    //他のObjectが離れている時
    void OnTriggerExit(Collider otherObj)
    {
        //床から離れたので削除
        ride.Remove(otherObj.gameObject);
    }
}
