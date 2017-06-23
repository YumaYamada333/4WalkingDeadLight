using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ParticaleScript : MonoBehaviour
{
    //パーティカル
    ParticleSystem particle;

    //パーティカルの種類
    const int NONE = 0;
    const int MOVE = 1;
    const int ATTACK = 2;
    const int DAMAGE = 3;
    const int LANDING = 4;
    const int WATER = 5;
    const int POISON = 6;
    const int HIT = 7;
    const int LAVA = 8;

    //const int GIMMICK = 2;
    //const int BREAK = 1;

    //パーティカル種類の判別用
    private int particleCnt;
    //private bool particleEnemy;
    //private int BlockPartical;
    //private bool flag = false;

    //プレイヤー取得用
    GameObject player;
    PlayerAction act;

    ////ダメージスクリプト取得用
    //GameObject attack;
    //AttackCollider atk;

    //アクションブロック取得用
    //GameObject block;
    //ActionCountDown actDown;
    //BlockAction blockAction;
    //BlockMove blockAct;

    // Use this for initialization
    void Start()
    {
        //コンポーネントの取得
        particle = GetComponent<ParticleSystem>();

        //プレイヤーの取得
        player = GameObject.Find("unitychan");
        act = player.GetComponent<PlayerAction>();

        //block = GameObject.Find("Block");
        //actDown = block.GetComponent<ActionCountDown>();
        //blockAction = block.GetComponent<BlockAction>() as BlockAction;
        //blockAct = blockAction.GetComponent<BlockMove>() as BlockMove;

        //パーティカルの停止
        particle.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        ////スクリプトの取得
        //attack = GameObject.Find("AttackColl");
        ////攻撃判定がアクティブ状態のときに判定
        //if (attack.activeSelf)
        //{
        //    atk = attack.GetComponent<AttackCollider>();
        //    //パーティカル発生判定
        //    particleEnemy = atk.GetFlag();
        //}

        //プレイヤーのアクションを判別
        particleCnt = act.particleType;
        //BlockPartical = actDown.GetActionType();
        //flag = blockAct.GetFlag();

        //プレイヤーのパーティカルを発生
        PlayerParticle(particleCnt);

        ////敵のパーティカルを発生
        //EnemyParticle(particleEnemy);

        ////ギミックのパーティカルを発生
        //GimmickParticle(int ParticleType)
    }

    //アクションの種類によってパーティカルを発生させる関数
    void PlayerParticle(int ParticleType)
    {
        //プレイヤーのアクションの種類によって発生させるパーティクルを決定
        switch (particleCnt)
        {
            case NONE:
                particle.Stop();
                break;
            case MOVE:
                if (particle.name == "MoveParticle")
                {
                    particle.Play();
                }
                else
                {
                    particle.Stop();
                }
                break;
            case ATTACK:
                if (particle.name == "AttackParticle")
                {
                    particle.Play();
                }
                else
                {
                    particle.Stop();
                }
                break;
            case DAMAGE:
                if (particle.name == "DamageParticle")
                {
                    particle.Play();
                }
                else
                {
                    particle.Stop();
                }
                break;
            case LANDING:
                if (particle.name == "LandingParticle")
                {
                    particle.Play();
                }
                else
                {
                    particle.Stop();
                }
                break;
            case WATER:
                if (particle.name == "WaterParticle")
                {
                    particle.Play();
                }
                else
                {
                    particle.Stop();
                }
                break;
            case POISON:
                if (particle.name == "PoisonParticle")
                {
                    particle.Play();
                }
                else
                {
                    particle.Stop();
                }
                break;
            case HIT:
                if (particle.name == "ColliderParticle" || particle.name == "ColliderParticle 1")
                {
                    particle.Play();
                }
                else
                {
                    particle.Stop();
                }
                break;
            case LAVA:
                if (particle.name == "LavaParticle")
                {
                    particle.Play();
                }
                else
                {
                    particle.Stop();
                }
                break;

        }
    }

    ////破壊可能オブジェクトのパーティカル発生判定関数
    //void EnemyParticle(bool particleEnemy)
    //{
    //    if (particleEnemy)
    //    {
    //        if (particle.name == "HitParticle")
    //        {
    //            particle.Play();
    //        }
    //        else
    //        {
    //            particle.Stop();
    //        }
    //    }
    //    else
    //    {
    //        particle.Stop();
    //    }
    //}

    ////ギミックの種類によってパーティクルを発生させる関数
    //void GimmickParticle(int ParticleType)
    //{
    //    //ギミックの種類によって発生させるパーティクルを決定
    //    switch (BlockPartical)
    //    {
    //        case GIMMICK:
    //            if ((particle.name == "GimmickParticle" || particle.name == "GimmickParticle (1)") && flag == true)
    //            {
    //                particle.Play();
    //            }
    //            break;
    //            //case BREAK:
    //            //    if (particle.name == "BreakParticle" && flag == true)
    //            //    {
    //            //        particle.Play();
    //            //    }
    //            //    break;
    //    }

    //}
}
