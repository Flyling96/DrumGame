using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class EnhancelScrollView : MonoBehaviour
{
    // 缩放曲线
    public AnimationCurve scaleCurve;
    // 位移曲线
    public AnimationCurve positionCurve;
    // 位移系数
    public float posCurveFactor = 5.0f;
    // y轴坐标固定值(所有的item的y坐标一致)
    public float yPositionValue = 0f;

    // 添加到EnhanceScrollView的目标对象
    public List<EnhanceItem> scrollViewItems;

    // 目标对象Widget脚本，用于depth排序
    private List<GameObject> textureTargets;

    // 计算差值系数
    public float dFactor = 0.2f;

    // 点击目标移动的横向目标值
    private float[] moveHorizontalValues;
    // 对象之间的差值数组(根据差值系数算出)
    private float[] dHorizontalValues;

    // 横向变量值
    public float horizontalValue = 0.0f;
    // 目标值
    public float horizontalTargetValue = 0.86f;

    // 移动动画参数
    private float originHorizontalValue = 0.86f;
   
    public float durationx = 5.0f;
    private float currentDuration = 0.0f;


    public float LeftLimit;
    public float RightLimit;


    float posdvalud = 1f / 300000.0f;
    //private static EnhancelScrollView instance;
    //public static EnhancelScrollView GetInstance()
    //{
    //    return instance;
    //}

    bool isInit = false;

    bool isFirstMove = true;

    void Awake()
    {
        //instance = this;
    }

    void Start()
    {
        horizontalTargetValue = 1.22f;
        originHorizontalValue = 1.22f;
    }

    public void InitScrollViewData(List<EnhanceItem> enhanceList)
    {
        horizontalTargetValue = 1.22f;
        originHorizontalValue = 1.22f;
        if(enhanceList != null)
        {
            for(int i = 0;i< enhanceList.Count;++i)
            {
                scrollViewItems.Add(enhanceList[i]);
            }
        }

        if (moveHorizontalValues == null)
            moveHorizontalValues = new float[scrollViewItems.Count];

        if (dHorizontalValues == null)
            dHorizontalValues = new float[scrollViewItems.Count];

        if (textureTargets == null)
            textureTargets = new List<GameObject>();

        posdvalud = 1f / 2750;

        int centerIndex = scrollViewItems.Count / 2;
        float centerIndexFloat = scrollViewItems.Count / 2.0f;
        dFactor = 1.0f / 6;
        for (int i = scrollViewItems.Count-1; i > -1; i--)
        {
            scrollViewItems[i].scrollViewItemIndex = i;
            GameObject tmpTexture = scrollViewItems[i].gameObject;
            textureTargets.Add(tmpTexture);

            dHorizontalValues[i] = dFactor * (centerIndexFloat - i);
        }
        currentDuration = 0;
        isInit = true;
    }

    public void UpdateEnhanceScrollView(float fValue)
    {
        for (int i = 0; i < scrollViewItems.Count; i++)
        {
            EnhanceItem itemScript = scrollViewItems[i];
            float xValue = GetXPosValue(fValue, dHorizontalValues[itemScript.scrollViewItemIndex]);
            float scaleValue = GetScaleValue(fValue, dHorizontalValues[itemScript.scrollViewItemIndex]);
            itemScript.UpdateScrollViewItems(xValue, yPositionValue, scaleValue);
        }
    }

    public GameObject BackGround;

    void Update()
    {
     
        //选择归位
        if(Input.GetMouseButtonUp(0))
        {
            originHorizontalValue = horizontalTargetValue;

            if (horizontalTargetValue > LeftLimit)
            {
                horizontalTargetValue = LeftLimit;
            }
            else if (horizontalTargetValue < RightLimit)
            {
                horizontalTargetValue = RightLimit;
            }

            currentDuration = 0f;


                if (horizontalTargetValue + 0.04f >= 0)
                {
                    if ((horizontalTargetValue + 0.04f) % 0.18f > 0.1f)
                    {
                        horizontalTargetValue = (horizontalTargetValue - (horizontalTargetValue + 0.04f) % 0.18f) + 0.18f;
                    }
                    else
                    {
                        horizontalTargetValue = horizontalTargetValue - (horizontalTargetValue + 0.04f) % 0.18f;
                    }
                }
                else
                {
                    if ((horizontalTargetValue + 0.04f) % 0.18f < -0.08f)
                    {
                        horizontalTargetValue = (horizontalTargetValue - (horizontalTargetValue + 0.04f) % 0.18f) - 0.18f;
                    }
                    else
                    {
                        horizontalTargetValue = horizontalTargetValue - (horizontalTargetValue + 0.04f) % 0.18f;
                    }
                }
            

        }

        //激活按钮
        for(int i = 0; i < textureTargets.Count - 1;i++)
        {
            textureTargets[i].GetComponent<Button>().enabled = false;
        }
        textureTargets[textureTargets.Count - 1].GetComponent<Button>().enabled = true;

        BackGround.GetComponent<SpriteRenderer>().sprite = textureTargets[textureTargets.Count - 1].GetComponent<Image>().sprite;

        if (!isInit)
        {
            return;
        }
        currentDuration += Time.deltaTime;
        if (currentDuration > duration)
        {
            // 更新完毕设置选中item的对象即可
            currentDuration = duration;
        }
        if (Time.frameCount % 5 != 0)
        {
            return;
        }
        SortDepth();

        if (isFirstMove)
        {
            currentDuration = 0;
            isFirstMove = false;
        }


        percent = currentDuration * durationx;
       // print(horizontalTargetValue);
  

        horizontalValue = Mathf.Lerp(originHorizontalValue, horizontalTargetValue, percent);

        //if(horizontalTargetValue >= 0.6f)
        //{
        //    horizontalTargetValue -= 100f;
        //}
        print(horizontalTargetValue);
        UpdateEnhanceScrollView(horizontalValue);
    }
    float percent = 0f;

    /// <summary>
    /// 缩放曲线模拟当前缩放值
    /// </summary>
    private float GetScaleValue(float sliderValue, float added)
    {
        float scaleValue = scaleCurve.Evaluate(sliderValue + added);
        return scaleValue;
    }

    /// <summary>
    /// 位置曲线模拟当前x轴位置
    /// </summary>
    private float GetXPosValue(float sliderValue, float added)
    {
        float evaluateValue = positionCurve.Evaluate(sliderValue + added) * posCurveFactor;
        return evaluateValue;
    }

    public void SortDepth()
    {
        textureTargets.Sort(new CompareShowMethod());
        for (int i = 0; i < textureTargets.Count; i++)
            textureTargets[i].transform.SetAsLastSibling();
    }

    public class CompareShowMethod:IComparer<GameObject>
    {
        public int Compare(GameObject left, GameObject right)
        {
            if (left.transform.localScale.x > right.transform.localScale.x)
                return 1;
            else if (left.transform.localScale.x < right.transform.localScale.x)
                return -1;
            else
                return 0;
        }
    }

    /// <summary>
    /// 获得当前要移动到中心的Item需要移动的factor间隔数
    /// </summary>
    private int GetMoveCurveFactorCount(float targetXPos)
    {
        int centerIndex = scrollViewItems.Count / 2;
        for (int i = 0; i < scrollViewItems.Count; i++)
        {
            float factor = (0.5f - dFactor * (centerIndex - i));

            float tempPosX = positionCurve.Evaluate(factor) * posCurveFactor;
            if (Mathf.Abs(targetXPos - tempPosX) < 0.01f)
                return Mathf.Abs(i - centerIndex);
        }
        return -1;
    }


    float dvalue = 0f;
    public void OnSlide(float distance)
    {
        print(distance);
        dvalue = posdvalud * distance;


        originHorizontalValue = horizontalTargetValue;
        // 更改target数值，平滑移动
        horizontalTargetValue += dvalue;
        currentDuration = 0.0f;
        //originHorizontalValue = horizontalValue;
    }
}
