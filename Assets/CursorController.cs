using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CursorController : MonoBehaviour {

    [SerializeField]
    private Texture m_defaultCursor;              // デフォルトカーソル
    [SerializeField]
    private Texture m_cardSelectCursor;           // カードの上時カーソル
    [SerializeField]
    private Texture m_cardGrabCursor;             // カードつかみ中カーソル

    private GameObject handsBoard;
    private bool m_grabFlag = false;
    private bool m_oldGrabFlag = false;

    // Use this for initialization
    void Start () {
        // カーソル削除
        Cursor.visible = false;
        handsBoard = GameObject.Find("HandsBord");
    }

    // Update is called once per frame
    void Update () {
        // カーソルの削除
        if (Cursor.visible)
            Cursor.visible = false;

        // マウスカーソルの移動
        transform.position = Input.mousePosition;

        RawImage image = GetComponent<RawImage>();

        if (handsBoard != null)
        {
                transform.FindChild("BatuImage").gameObject.GetComponent<RawImage>().enabled = GameObject.Find("GameManager").GetComponent<GameManager>().GetGimmickFlag();
            if (GameObject.Find("MouseSystem").GetComponent<MouseSystem>().GetMouseHit(handsBoard) >= 0 || m_grabFlag)
            {

                if (Input.GetMouseButton(0))
                {
                    image.texture = m_cardGrabCursor;
                    m_grabFlag = true;
                }
                else if (!m_oldGrabFlag)
                {
                    image.texture = m_cardSelectCursor;
                    m_grabFlag = false;
                }
                else
                {
                    image.texture = m_defaultCursor;
                    m_grabFlag = false;
                }
            }
            else
                image.texture = m_defaultCursor;
        }
        m_oldGrabFlag = m_grabFlag;
    }
}
