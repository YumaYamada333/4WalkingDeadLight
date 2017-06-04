using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlashingUGui : MonoBehaviour {
    private GameObject playButton;  //obj
    private float step = 0.01f;     //点滅する速度
	// Use this for initialization
	void Start () {
        playButton = GameObject.Find("PlayButton");
    }

    // Update is called once per frame
    void Update () {
        //重すぎバロタ　解決策求
        //if(playButton.SetActive(false))
        //αブレンド
        float toClor = playButton.GetComponent<Image>().color.a;
        //切り替え
        if (toClor < 0 || toClor > 1)
            step = step * -1;
        playButton.GetComponent<Image>().color = new Color(255, 255, 255, toClor + step);
	}
}
