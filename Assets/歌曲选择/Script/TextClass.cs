using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextClass : MonoBehaviour {

    public int ItemIndex = 0;//一个item从上到下的排位
    public Vector3 ItemLocalPosition = Vector3.zero;

    public void UpdateScrollViewItems(float XValue, float YValue)
    {
        ItemLocalPosition.x = XValue;
        ItemLocalPosition.y = YValue;
        this.transform.localPosition = ItemLocalPosition;
    }
}
