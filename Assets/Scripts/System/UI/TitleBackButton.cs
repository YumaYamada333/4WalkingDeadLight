using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleBackButton : MonoBehaviour
{

    bool isend;

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void TitleBack()
    {
        GetComponent<AudioSource>().Play();
        isend = true;
        StageSelectDirector.SetisEnd(isend);
    }
}
