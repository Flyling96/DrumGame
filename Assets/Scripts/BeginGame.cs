using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeginGame : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Begin()
    {
        Application.LoadLevel("选择歌曲");
    }

    public void Edit()
    {
        Application.LoadLevel("制谱端");
    }
}
