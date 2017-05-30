using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollider : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider coll)
    {
        //敵と当たったら
        if (coll.gameObject.tag == "Enemy" || coll.gameObject.tag == "Break")
        {
            Destroy(coll.gameObject);
        }
    }
}
