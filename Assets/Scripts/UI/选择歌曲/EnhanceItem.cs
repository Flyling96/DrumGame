using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EnhanceItem : MonoBehaviour 
{
    // 在ScrollViewitem中的索引
    // 定位当前的位置和缩放
    public int scrollViewItemIndex = 0;
    public bool inRightArea = false;

    private Vector3 targetPos = Vector3.one;
    private Vector3 targetScale = Vector3.one;

    void Awake()
    {
        //mTrs = this.transform;
        //mTexture = this.GetComponent<Image>();
    }

    /// <summary>
    /// 更新该Item的缩放和位移
    /// </summary>
    public void UpdateScrollViewItems(float xValue, float yValue, float scaleValue)
    {
        targetPos.x = xValue;
        targetPos.y = yValue;
        targetScale.x = targetScale.y = scaleValue;

        transform.localPosition = targetPos;
        transform.localScale = targetScale;
    }
}
