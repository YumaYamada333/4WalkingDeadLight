using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockColliderScript : MonoBehaviour
{

    //パーティクルシステム
    public ParticleSystem WaterParticle;
    //パーティクルフラグ
    bool flagParticle;
    //パーティクルカウント
    float cout;
    //パーティクル継続時間
    const float Duration = 3.0f;

    void OnTriggerEnter(Collider hit)
    {
        //水に触れたら
        if (hit.gameObject.tag == "Water")
        {
            //エフェクトを発生させる
            WaterParticle.Play();
            flagParticle = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        //パーティクルが発生して一定時間経ったら
        if (other.gameObject.tag == "GameOverZone" && cout >= Duration)
        {
            WaterParticle.Stop();
        }
    }
    // Use this for initialization
    void Start()
    {
        //エフェクトを止めておく
        WaterParticle.Stop();
        flagParticle = false;
        cout = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //パーティクル継続時間計測
        if (flagParticle)
        {
            cout += 0.1f;
        }
    }
}
