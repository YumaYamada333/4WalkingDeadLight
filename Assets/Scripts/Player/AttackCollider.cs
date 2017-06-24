using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollider : MonoBehaviour {

    //当たった判定
    bool Flag;

    /*音*/
    public AudioClip block;
    AudioSource audioSource;
    AudioSource PlayerAudio;

    // Use this for initialization
    void Start ()
    {
        Flag = false;

        audioSource = GameObject.Find("GimmickAudio").GetComponent<AudioSource>();
        PlayerAudio = GameObject.Find("unitychan").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update ()
    {
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
        if(coll.gameObject.tag == "Block")
        {
            PlayerAudio.Stop();
            audioSource.PlayOneShot(block);
        }
        if(coll.gameObject.tag == "Break")
        {
            audioSource.Stop();
        }
    }

    public bool GetFlag()
    {
        return Flag;
    }
}
