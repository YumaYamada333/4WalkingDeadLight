using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//アクションの種類用定数
//struct ACT_TYPE
//{
//    public const int ACT_BREAK = 0;
//    public const int ACT_MOVE = 1;
//    public const int ACT_ROT = 2;
//    public const int ACT_SCALE = 3;
//}

//各アクションの基底クラス
//public class LinkBlockAction
//{
//    //準備関数(引数は参照渡し)
//    public virtual void Preparation(ref GameObject obj) { }

//    //実行関数(引数は参照渡し)
//    public virtual void Execute(ref GameObject obj) { }
//}

//動くアクションのクラス
public class LinkBlockMove : BlockAction
{
    //スタートの時間
    private float m_start_time = 0.0f;
    //スタート位置
    private Vector3 m_start_pos = Vector3.zero;

    //パーティクルフラグ取得
    private bool PartTim;

    //準備関数、実行関数のオーバーライド(引数は参照渡し)
    override public void Preparation(ref GameObject obj) { Init(ref obj); }
    override public void Execute(ref GameObject obj) { Move(ref obj); }

    //パーティカルフラグを返す関数
    public bool GetFlag()
    {
        return PartTim;
    }

    public void Init(ref GameObject obj)
    {
        //現在地と時間を取得
        m_start_time = Time.time;
        m_start_pos = obj.GetComponent<LinkActionCountDown>().GetActObject().transform.position;
    }

    public void Move(ref GameObject obj)
    {
        //フラグがたっていたら移動(補間)
        if (obj.GetComponent<LinkActionCountDown>().GetActionFlag())
        {
            //経過時間を移動時間で割る
            float timeStep = (Time.time - m_start_time) / obj.GetComponent<LinkActionCountDown>().GetActTime();

            obj.GetComponent<LinkActionCountDown>().GetActObject().transform.position = 
                (1 - timeStep) * m_start_pos + timeStep * (m_start_pos + obj.GetComponent<LinkActionCountDown>().GetActDistance());

            //移動時間になったらフラグを止める
            if (timeStep > 1.0f)
            {
                //微調整
                obj.GetComponent<LinkActionCountDown>().GetActObject().transform.position = 
                    m_start_pos + obj.GetComponent<LinkActionCountDown>().GetActDistance();

                PartTim = true;

                //次のアクションへ移行
                obj.GetComponent<LinkActionCountDown>().ChangeNextAct();
            }
            else
            {
                PartTim = false;
            }
        }
    }
}

//回転アクションのクラス
public class LinkBlockRot : BlockAction
{
    //スタートの時間
    private float m_start_time = 0.0f;
    //スタート位置
    //private Vector3 m_start_deg = Vector3.zero;
    //1回だけ通る用変数
    private bool m_rot_flag;
    //今の角度
    private float m_degree_x = 0.0f;
    private float m_degree_y = 0.0f;
    private float m_degree_z = 0.0f;
    //パーティクルフラグ取得
    //private bool PartTim;

    //準備関数、実行関数のオーバーライド(引数は参照渡し)
    override public void Preparation(ref GameObject obj) { Init(ref obj); }
    override public void Execute(ref GameObject obj) { Rot(ref obj); }

    //パーティカルフラグを返す関数
    //public bool GetFlag()
    //{
    //    return PartTim;
    //}

    public void Init(ref GameObject obj)
    {
        //時間を取得
        m_start_time = Time.time;
        m_rot_flag = false;
    }

    public void Rot(ref GameObject obj)
    {
        //フラグがたっていたら移動(補間)
        if (obj.GetComponent<LinkActionCountDown>().GetActionFlag())
        {
            //経過時間を移動時間で割る
            float timeStep = (Time.time - m_start_time) / obj.GetComponent<LinkActionCountDown>().GetActTime();

            //回転
            if (!m_rot_flag)
            {
                m_degree_x = obj.GetComponent<LinkActionCountDown>().GetActObject().transform.rotation.eulerAngles.x;
                m_degree_y = obj.GetComponent<LinkActionCountDown>().GetActObject().transform.rotation.eulerAngles.y;
                m_degree_z = obj.GetComponent<LinkActionCountDown>().GetActObject().transform.rotation.eulerAngles.z;

                iTween.RotateTo(obj.GetComponent<LinkActionCountDown>().GetActObject(), 
                    iTween.Hash("y", m_degree_y + obj.GetComponent<LinkActionCountDown>().GetActDistance().y,
                    "x" , m_degree_x + obj.GetComponent<LinkActionCountDown>().GetActDistance().x,
                    "z" , m_degree_z + obj.GetComponent<LinkActionCountDown>().GetActDistance().z,
                    "time", obj.GetComponent<LinkActionCountDown>().GetActTime()));
                m_rot_flag = true;
            }

            //移動時間になったらフラグを止める
            if (timeStep > 1.0f)
            {
                //PartTim = true;

                //繰り返しパターン用初期化
                obj.GetComponent<LinkActionCountDown>().ChangeNextAct();

            }
            //else
            //{
            //    PartTim = false;
            //}
        }
    }
}

//スケールアクションのクラス
public class LinkBlockScale : BlockAction
{
    //スタートの時間
    private float m_start_time = 0.0f;
    //スタート時の大きさ
    private Vector3 m_start_scale = Vector3.zero;

    //パーティクルフラグ取得
    private bool PartTim;

    //準備関数、実行関数のオーバーライド(引数は参照渡し)
    override public void Preparation(ref GameObject obj) { Init(ref obj); }
    override public void Execute(ref GameObject obj) { Scale(ref obj); }

    //パーティカルフラグを返す関数
    public bool GetFlag()
    {
        return PartTim;
    }

    public void Init(ref GameObject obj)
    {
        //現在地と時間を取得
        m_start_time = Time.time;
        m_start_scale = obj.GetComponent<LinkActionCountDown>().GetActObject().transform.localScale;
    }

    public void Scale(ref GameObject obj)
    {
        //フラグがたっていたら移動(補間)
        if (obj.GetComponent<LinkActionCountDown>().GetActionFlag())
        {
            //経過時間を移動時間で割る
            float timeStep = (Time.time - m_start_time) / obj.GetComponent<LinkActionCountDown>().GetActTime();

            obj.GetComponent<LinkActionCountDown>().GetActObject().transform.localScale 
                = (1 - timeStep) * m_start_scale + timeStep * (m_start_scale + obj.GetComponent<LinkActionCountDown>().GetActDistance());

            //移動時間になったらフラグを止める
            if (timeStep > 1.0f)
            {
                PartTim = true;

                //次のアクションへ
                obj.GetComponent<LinkActionCountDown>().ChangeNextAct();
            }
            else
            {
                PartTim = false;
            }
        }
    }
}

//壊れるアクションのクラス
public class LinkBlockBreak : BlockAction
{

    override public void Execute(ref GameObject obj) { Break(ref obj); }

    ////パーティクルフラグ取得
    //private bool PartTim;

    //public bool GetFlag()
    //{
    //    return PartTim;
    //}

    public void Break(ref GameObject obj)
    {
        //フラグがたったら壊れる
        if (obj.GetComponent<LinkActionCountDown>().GetActionFlag())
        {
            //オブジェクトを破棄
            obj.GetComponent<LinkActionCountDown>().SetActionFlag(false);
            obj.GetComponent<LinkActionCountDown>().DestroyObject();
            //PartTim = true;
        }
        //else
        //{
        //    PartTim = false;
        //}
    }
}

//実行クラス
public class LinkActionCountDown : MonoBehaviour
{
    [System.Serializable]
    struct ActionValue
    {
        //アクション量
        public Vector3 distance;
        //移動時間
        public float time;
        //待機時間
        //public float wait_time;
        //アクションの種類
        [SerializeField, Range(0, 3)]
        public int action_type;
        //直前のアクションに連結するかフラグ
        public bool link_action;
        //動作するオブジェクト
        public GameObject obj;
    }

    //全てのアクションの値
    [SerializeField]
    private ActionValue[] m_action_value;
    //アクション量
    private Vector3 m_act_distance = Vector3.zero;
    //移動時間
    private float m_act_time = 1.0f;
    //待機時間
    //private float m_wait_time = 0.0f;
    //アクションの種類
    private int m_action_type;
    //動作するオブジェクト
    private GameObject m_action_obj;

    //自分自身のオブジェクト
    private GameObject m_obj;
    //カウントを行うオブジェクト
    public GameObject m_count_obj;
    //ゲームマネージャー
    public GameObject m_game_manager;
    //複数オブジェクトが同タイミングで動く場合に最後に動き終わるオブジェクト
    public GameObject m_final_move_obj;
    //リピートするかどうかのフラグ
    public bool m_repeat_flag = true;

    //初期待機時間
    //private float m_init_wait_time;

    //フラグ
    private bool m_old_flag = false;
    private bool m_action_flag = false;
    private bool m_awake_flag = false;
    private bool m_link_flag = false;

    //アクションの実行カウント
    private int m_action_count = 0;
    //アクションの総数
    private int m_all_action_num;

        

    //アクション
    private BlockAction action = null;
    //GameObjectでColliderと触れているの要素数
    private List<GameObject> ride = new List<GameObject>();
    private Vector3 m_correction_value_ride;

    //パーティクルフラグ取得
    public bool PartTim;

    // Use this for initialization
    void Start()
    {
        //アクションの総数を取得
        m_all_action_num = m_action_value.Length;
        //実行するアクションの各値を取得
        SetActionValue(m_action_count);

        //指定したアクションタイプによってアクションを変更
        SetActionType(m_action_type);

        //自身を取得
        m_obj = this.gameObject;
        //初期待機時間を取得
        //m_init_wait_time = m_wait_time;
    }

    // Update is called once per frame
    void Update()
    {
        //カウントダウン
        GetCountZero();

        //フラグが立ったらアクションの準備
        if (m_action_flag == true &&
            m_action_flag != m_old_flag)
        {
            action.Preparation(ref m_obj);
            m_old_flag = m_action_flag;
        }
        //アクションの実行
        action.Execute(ref m_obj);
        //GameObjectの移動
        foreach (GameObject otherObj in ride)
        {
            if (m_game_manager.GetComponent<GameManager>().GetGimmickFlag())
            {
                //動く床の位置にObjectの座標を合わせる
                m_correction_value_ride = otherObj.transform.position - m_obj.GetComponent<Collider>().transform.position;
                Vector3 v = otherObj.transform.position;
                otherObj.transform.position = new Vector3(m_obj.GetComponent<Collider>().transform.position.x, m_obj.GetComponent<Collider>().transform.position.y + m_correction_value_ride.y/*v.y*/, v.z);
            }
        }
    }


    //カウントダウンによってフラグをあげる
    void GetCountZero()
    {
        //カウントが0になり、フラグがたっておらず、いままで一度もフラグが立っていなかったら
        if (m_count_obj.GetComponent<CountDown>().GetCount() == 0 &&
            m_action_flag == false &&
            m_old_flag == false)
        {
            //ゲームマネージャーのギミックが移動している判定用のフラグをあげる
            m_game_manager.GetComponent<GameManager>().SetGimmickFlag(true);
            //起動フラグを上げる
            m_awake_flag = true;
            //フラグを上げる
            m_action_flag = true;
        }
    }

    //アクションの値を設定
    void SetActionValue(int action_num)
    {
        //アクション量
        m_act_distance = m_action_value[action_num].distance;
        //移動時間
        m_act_time = m_action_value[action_num].time;
        //待機時間
        //m_wait_time = m_action_value[action_num].wait_time;
        //アクションの種類
        m_action_type = m_action_value[action_num].action_type;
        //直前のアクションに連結するかフラグ
        m_link_flag = m_action_value[action_num].link_action;
        //動作するオブジェクト
        if (m_action_value[action_num].obj != null)
        { m_action_obj = m_action_value[action_num].obj; }
        else
        { m_action_obj = this.gameObject; } 

        //アクションを設定
        SetActionType(m_action_type);
    }

    //指定したアクションタイプによってアクションを変更
    void SetActionType(int act_type)
    {
        switch (act_type)
        {

            case ACT_TYPE.ACT_BREAK:
                action = new LinkBlockBreak();
                break;
            case ACT_TYPE.ACT_MOVE:
                action = new LinkBlockMove();
                break;
            case ACT_TYPE.ACT_ROT:
                action = new LinkBlockRot();
                break;
            case ACT_TYPE.ACT_SCALE:
                action = new LinkBlockScale();
                break;
            default:
                break;
        }
    }

    //アクションタイプを返す
    public int GetActionType()
    {
        return m_action_type;
    }

    //アクションフラグを返す
    public bool GetActionFlag()
    {
        return m_action_flag;
    }

    //アクションフラグを設定
    public void SetActionFlag(bool flag)
    {
        m_action_flag = flag;
    }

    //繰り返し用アクションフラグを設定
    public void SetOldActionFlag(bool flag)
    {
        m_old_flag = flag;
    }

    //次のアクションへ
    public void ChangeNextAct()
    {
        //アクション実行回数を増やす
        m_action_count++;
        //アクションがすべて終わっていたら,カウントを初期化
        if(m_action_count == m_all_action_num)
        {
            m_action_count = 0;
        }

        //アクションの値を代入
        SetActionValue(m_action_count);

        //繰り返しフラグをセット
        m_old_flag = false;
        //繰り返さないなら
        if (m_action_count == 0 && !m_repeat_flag)
        {
            m_old_flag = true;
        }

        //続けて行動しないなら一旦アクションを止める
        if (!m_link_flag)
        {
            m_action_flag = false;
            m_awake_flag = false;

            //カウント初期化
            m_count_obj.GetComponent<CountDown>().SetCount();
            //全アクション実行後繰り返さないなら
            if (m_action_count == 0 && !m_repeat_flag)
            {
                m_count_obj.GetComponent<CountDown>().SetCount(0);
            }

            //同タイミングで動くオブジェクトが動き終わっていたら
            if (m_final_move_obj != null)
            {
                if (!m_awake_flag)
                {
                    m_game_manager.GetComponent<GameManager>().SetGimmickFlag(false);
                }
            }
            else
            {
                m_game_manager.GetComponent<GameManager>().SetGimmickFlag(false);
            }
        }
    }

    //起動フラグを返す
    public bool GetAwakeFlag()
    {
        return m_awake_flag;
    }

    //起動フラグを設定
    public void SetAwakeFlag(bool flag)
    {
        m_awake_flag = flag;
    }

    //自身を破棄
    public void DestroyObject()
    {
        Destroy(m_action_obj);
    }
    //他のObjectが接触している時
    void OnTriggerEnter(Collider otherObj)
    {
        ride.Add(otherObj.gameObject);
        //m_correction_value_ride = otherObj.transform.position - m_obj.GetComponent<Collider>().transform.position;
    }
    //他のObjectが離れている時
    void OnTriggerExit(Collider otherObj)
    {
        //床から離れたので削除
        ride.Remove(otherObj.gameObject);
    }

    //移動量を設定する
    public void SetActDistance(Vector3 distance)
    {
        m_act_distance = distance;
    }

    //移動量を取得
    public Vector3 GetActDistance()
    {
        return m_act_distance;
    } 

    //移動時間を取得
    public float GetActTime()
    {
        return m_act_time;
    }

    //アクションするオブジェクトを取得
    public GameObject GetActObject()
    {
        return m_action_obj;
    }

    //アクションのカウントを取得
    public int GetActionCount()
    {
        return m_action_count;
    }
}

