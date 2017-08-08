using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class tmp : MonoBehaviour {

    // Use this for initialization

    public GameObject item;
	void Start () {

        item.transform.GetComponent<Image>().sprite = Resources.Load<Sprite>("Image1");
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
