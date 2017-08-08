using UnityEngine;
using System.Collections;

public class SceneChange : MonoBehaviour {

    // Use this for initialization

    float time;

	void Start () {
        time = 0;
	}
	
	// Update is called once per frame
	void Update () {
        if(Time.realtimeSinceStartup>time+1f)
        {
            time = Time.realtimeSinceStartup;
            SunHalo();
        }
	
	}


    public GameObject Halo;

    void SunHalo()
    {
        Halo.transform.Rotate(0, 0, 30);
    }
}
