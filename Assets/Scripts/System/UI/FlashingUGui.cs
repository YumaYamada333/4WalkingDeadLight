//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;

//public class FlashingUGui : MonoBehaviour {
//    private GameObject playButton;  //obj
//    private float step = 0.01f;     //点滅する速度
//    static private GameObject playButtonS;
//    static float alpha;
//    static float f;
//	// Use this for initialization
//	void Start () {
//    }

//    // Update is called once per frame
//    void Update () {
//        //重すぎバロタ　解決策求
//        //if(playButton.SetActive() == false)
//        playButton = playButtonS/*GameObject.Find("PlayButton")*/;
//        alpha = playButton.GetComponent<Image>().color.a;

//        //αブレンド
//        float toClor = playButton.GetComponent<Image>().color.a;
//        //切り替え
//        if (toClor < 0 || toClor > 1)
//            step = step * -1;

//        f += 0.01f;

//        playButton.GetComponent<Image>().color = new Color(255, 255, 255, /*alpha + */Mathf.Sin(f));
//	}
//    static public void SetPlayButton(GameObject obj)
//    {
//        playButtonS = obj;
//    }
//}
