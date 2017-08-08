using UnityEngine;
using System.Collections;

public class Cloud : MonoBehaviour {

    // Use this for initialization

    float FirstPosition;
    float Speed;
    float time;

    bool isup;

	void Start () {
        FirstPosition = this.transform.position.y;
        Speed = Random.Range(0, 100) / 30f;
        time = 0;
        isup = true;
    }
	
	// Update is called once per frame
	void Update () {

        if(Time.realtimeSinceStartup>time+1f)
        {
            SpeedChange();
            time = Time.realtimeSinceStartup;
        }

        if(this.transform.position.y>FirstPosition+3)
        {
            isup = false;
        }
        else if(this.transform.position.y<FirstPosition)
        {
            isup = true;
        }


        PositionChange();


    }

    void SpeedChange()
    {
        Speed = Random.Range(0, 100) / 30f;
    }
    
    void PositionChange()
    {
        if(isup)
        {
            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y+Speed*Time.deltaTime, this.transform.position.z);
        }
        else
        {
            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - Speed * Time.deltaTime, this.transform.position.z);
        }
    }
}
