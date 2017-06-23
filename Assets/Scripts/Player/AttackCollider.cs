using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollider : MonoBehaviour {

    //当たった判定
    bool Flag;

	// Use this for initialization
	void Start () {
        Flag = false;
	}
	
	// Update is called once per frame
	void Update () {
        Flag = false;
	}

    void OnTriggerEnter(Collider coll)
    {
        //敵と当たったら
        if (coll.gameObject.tag == "Enemy" || coll.gameObject.tag == "Break")
        {
            Flag = true;
            Destroy(coll.gameObject);
        }
    }

    public bool GetFlag()
    {
        return Flag;
    }
}
