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
    }

    // Update is called once per frame
    void Update () {
        // カーソルの削除
        if (Cursor.visible)
            Cursor.visible = false;

        handsBoard = GameObject.Find("HandsBord");
        // マウスカーソルの移動
        transform.position = Input.mousePosition;

        RawImage image = GetComponent<RawImage>();

        if (true)
        {
            bool canGrap = false;
            GameObject gamemanager = null;
            if (gamemanager = GameObject.Find("GameManager") as GameObject)
            {
                canGrap = gamemanager.GetComponent<GameManager>().GetGimmickFlag();
            }

            transform.FindChild("BatuImage").gameObject.GetComponent<RawImage>().enabled = canGrap;
            bool isHit = false;
            if (handsBoard)
            {
                if (GameObject.Find("MouseSystem").GetComponent<MouseSystem>().GetMouseHit(handsBoard) >= 0)
                {
                    isHit = true;
                }
            }

            if (isHit || m_grabFlag)
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

            bool isActtion = false;
            if (gamemanager)
            {
                isActtion = GameManager.GameState.Acttion == gamemanager.GetComponent<GameManager>().GetGameState();
            }

            if (isActtion)
            {
                image.texture = m_cardGrabCursor;
                m_grabFlag = false;
                transform.FindChild("BatuImage").gameObject.GetComponent<RawImage>().enabled = false;
                if (gamemanager != null ? gamemanager.GetComponent<GameManager>().OverFlag() : false)
                {
                    image.texture = m_defaultCursor;
                }
            }
        }
        m_oldGrabFlag = m_grabFlag;
    }
}
