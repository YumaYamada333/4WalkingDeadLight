using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EParticaleScript : MonoBehaviour {

    //パーティカル
    ParticleSystem particle;

    //発生させるパーティクルのオブジェクト
    public GameObject breakObject;
    Vector3 tmp;

    bool atkFlag;
    //攻撃判定
    private bool hit;

    //ダメージスクリプト取得用
    GameObject attack;
    AttackCollider atk;

    //プレイヤー取得用
    GameObject player;

    // Use this for initialization
    void Start()
    {
        //コンポーネントの取得
        particle = GetComponent<ParticleSystem>();
        hit = false;
        tmp = Vector3.zero;

        //プレイヤーの取得
        player = GameObject.Find("unitychan");
        //スクリプトの取得
        attack = player.transform.Find("AttackColl").gameObject;

        particle.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        //オブジェクトより少し上に配置
        if (breakObject != null)
        {
            tmp = breakObject.transform.position;
            particle.transform.position = new Vector3(tmp.x, tmp.y + 0.5f, tmp.z);
        }

        //攻撃判定が存在するならば 
        if(attack.activeSelf)
        {
            atk = attack.GetComponent<AttackCollider>();

            atkFlag = atk.GetFlag();
        }

        //敵のパーティカルを発生
        EnemyParticle();
    }


    //破壊可能オブジェクトのパーティカル発生判定関数
    void EnemyParticle()
    {
        if (breakObject == null)
        {
            hit = true;
        }
        else
        {
            hit = false;
        }

        if (!attack.activeSelf && !hit)
        {
            //パーティクルの発生
            particle.Play();
        }
        else if(hit && !attack.activeSelf)
        {
            //パーティクルの停止
            particle.Stop();
        }    
    }
}
