using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EParticaleScript : MonoBehaviour {

    //パーティカル
    ParticleSystem particle;
    //破壊したオブジェクト名
    string breakName;

    //発生させるパーティクルのオブジェクト
    public GameObject breakObject;
    //プレイヤー
    GameObject player;
    private PlayerAction pAction;
    //攻撃判定用のオブジェクト
    GameObject attack;
    private AttackCollider ARange;

    //対象オブジェクトの座標
    private Vector3 tmp;
    //アタック判定
    const int AttackNum = 2;
    //攻撃判定
    private bool hit;
    private bool play;

    // Use this for initialization
    void Start()
    {
        //コンポーネントの取得
        particle = GetComponent<ParticleSystem>();
        player = GameObject.Find("unitychan");
        pAction = player.GetComponent<PlayerAction>();
        attack = player.transform.Find("AttackColl").gameObject;

        //初期化
        tmp = Vector3.zero;
        hit = false;
        play = false;

        //エフェクトを止めておく
        particle.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        //オブジェクトより少し上に配置
        if (breakObject != null)
        {
            //対象オブジェクトの座標を取得
            tmp = breakObject.transform.position;
            //エフェクトを対象オブジェクトの少し上に配置
            particle.transform.position = new Vector3(tmp.x, tmp.y + 0.5f, tmp.z);
        }

        //攻撃判定が存在するならば 
        if (attack.activeSelf)
        {
            ARange = attack.GetComponent<AttackCollider>();
            hit = ARange.GetFlag();
            breakName = ARange.GetName();
        }

        //敵のパーティカルを発生
        EnemyParticle();
    }


    //破壊可能オブジェクトのパーティカル発生判定関数
    void EnemyParticle()
    {
        if(pAction.particleType==AttackNum)
        {
            if(breakObject==null)
            {
                play = true;
            }
        }

        if (attack.activeSelf)
        {
            if (hit)
            {
                if (breakObject == null)
                {
                    //パーティクルの発生
                    particle.Play();
                }
            }
        }
        if (!attack.activeSelf)
        {
            //パーティクルの停止
            particle.Stop();
        }
        if(play)
        {
            particle.Stop();
        }
    }
}
