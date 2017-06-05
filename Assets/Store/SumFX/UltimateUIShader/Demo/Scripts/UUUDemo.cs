using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class UUUDemo : MonoBehaviour
{
    public RectTransform UIWidgetsContainer;
    private bool isMoving = false;
    private Vector3 panelPosition;
    private Vector3 panelInitialPosition;
    private Vector3 dragPosition;
    // Use this for initialization
    void Start()
    {
        panelPosition = UIWidgetsContainer.anchoredPosition;
        panelInitialPosition = panelPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            isMoving = true;
            dragPosition = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
        {
            isMoving = false;
            panelInitialPosition = UIWidgetsContainer.anchoredPosition;
        }
        if (isMoving)
        {
            panelPosition.x = panelInitialPosition.x + (Input.mousePosition.x - dragPosition.x);
            panelPosition.y = panelInitialPosition.y + (Input.mousePosition.y - dragPosition.y);
            UIWidgetsContainer.anchoredPosition = panelPosition;
        }
    }
}
