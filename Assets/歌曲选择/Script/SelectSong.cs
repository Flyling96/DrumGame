using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class SelectSong : MonoBehaviour, IDragHandler
{


    public AnimationCurve PositionCurve;//X位移曲线

    public string[] TextString;
    public Sprite[] TextImage;
    public Sprite[] BackgroundImage;

    public GameObject ItemDemo;
    public GameObject BackgroundDemo;
    public List<TextClass> ScrollViewItems = new List<TextClass>();
    static public int SongIndex = 0;

    private GameObject[] Item = new GameObject[2000];
    private GameObject[] Background = new GameObject[2000];
    private int CenterIndex = 0;//中心item的编号
    private float PositionCurveFactor = 0;//X位移曲线的调整参数

    private float ItemDistance = 100;//Item之间的垂直距离
    private float BackgroundDistance = 600;
    //private float TargetValue = -35;//以第一个text为参照，滑动后的位置
    private float SumDistance = 0;
    private float Distance = 0;
    private float DistanceScale = 6;//Background和item Y轴上距离的比例
    private float YCenterIndex = 0;
    private float ReturnTime = 0;
    private float PositionXFactor = -525;//位移偏量

    // Use this for initialization
    void Start () {

       // OriginValue = -35;
       // TargetValue = -35;
        Distance = 0;

        for (int i = 0; i < TextImage.Length;i++)
        {
            Item[i] = GameObject.Instantiate(ItemDemo);
            Item[i].transform.parent = this.transform;
            Item[i].GetComponent<TextClass>().ItemIndex = i;
            Item[i].GetComponent<Image>().sprite = TextImage[i];
            Item[i].GetComponent<RectTransform>().localPosition = new Vector3(PositionXFactor+(GetXPositionValue(0.5f+ 0.1f*i)),-ItemDistance * (i+1),0);
            Item[i].GetComponent<TextClass>().ItemLocalPosition = new Vector3(PositionXFactor+(GetXPositionValue(0.5f + 0.1f * i)), -ItemDistance * (i + 1), 0);
            Item[i].GetComponent<RectTransform>().localScale = Vector3.one;
            ScrollViewItems.Add(Item[i].GetComponent<TextClass>());

        }
        for (int i = 0; i < BackgroundImage.Length; i++)
        {
            Background[i] = GameObject.Instantiate(BackgroundDemo);
            Background[i].transform.parent = this.transform;
            Background[i].GetComponent<Image>().sprite = BackgroundImage[i];
            Background[i].GetComponent<RectTransform>().localPosition = new Vector3(-31.5f, -400.5f - BackgroundDistance * i , 0);
            Background[i].GetComponent<RectTransform>().localScale = Vector3.one;
        }
        CenterIndex = 0;


    }

    /// <summary>
    /// 更新Background的位置
    /// </summary>
    private void UpdateBackground()
    {
        for (int i = 0; i < BackgroundImage.Length; i++)
        {
            GameObject tmp = Background[i];
            tmp.GetComponent<RectTransform>().localPosition = new Vector3(tmp.GetComponent<RectTransform>().localPosition.x, tmp.GetComponent<RectTransform>().localPosition.y + Distance * DistanceScale, tmp.GetComponent<RectTransform>().localPosition.z);
        }
    }

    /// <summary>
    /// 更新位置
    /// </summary>
    private void UpdateItem()
    {
        for(int i = 0;i< TextImage.Length; i++)
        {
            TextClass TmpScript = ScrollViewItems[i];
            float TmpY;
            if (TmpScript.ItemLocalPosition.y >= - 35 - 4 * ItemDistance && TmpScript.ItemLocalPosition.y <= -35 + 4 * ItemDistance)
            {
                TmpY = (TmpScript.ItemLocalPosition.y - (- 35 + 4 * ItemDistance)) / (-8 * ItemDistance);
            }
            else if(TmpScript.ItemLocalPosition.y < (-35 - 4 * ItemDistance))
            {
                TmpY = 1;
            }
            else
            {
                TmpY = 0;
            }
            float XValue = PositionXFactor+GetXPositionValue(TmpY);
            float YValue = TmpScript.ItemLocalPosition.y + Distance;
            if(YValue>-62.5f&& YValue<17.5f)
            {
                CenterIndex = i;
            }
            TmpScript.UpdateScrollViewItems(XValue, YValue);
        }
     
    }

    /// <summary>
    /// 复位
    /// </summary>
    private void ReturnItem(int Index)
    {
      
        TextClass TmpScript = ScrollViewItems[Index];
        float TmpY = TmpScript.ItemLocalPosition.y;
        //Debug.Log(TmpY);
        if (TmpY > -34.85f && TmpY < -9f)
        {
            Distance = -0.3f;
            UpdateItem();
            UpdateBackground();
        }
        else if(TmpY <-35.15f && TmpY > -61f)
        {
            Distance = 0.5f;
            UpdateItem();
            UpdateBackground();
        }
        else if(TmpY > -19f)
        {
            Distance = -3;
            UpdateItem();
            UpdateBackground();
        }
        else if(TmpY < -51f)
        {
            Distance = 3;
            UpdateItem();
            UpdateBackground();
        }
        else
        {
            Distance = 0;
        }

    }

    // 拖动  
    public void OnDrag(PointerEventData EventData)
    {
        OnSlide(EventData.delta.y);
    }

    public void OnSlide(float distance)
    {
        Distance = distance;
        SumDistance += 0.3f * distance;
       // Item[0].GetComponent<RectTransform>().localPosition = new Vector3(Item[0].transform.localPosition.x, Item[0].transform.localPosition.y + 0.3f*distance, Item[0].transform.localPosition.z);
    }


        // Update is called once per frame
    void Update ()
    {
        
        if (Input.GetMouseButtonUp(0))
        {
          
            //Distance = 0;
            //确认上下限
            if (Item[0].transform.localPosition.y < - 35 )
            {
                CenterIndex = 0;
            }
            else if(Item[TextImage.Length - 1].transform.localPosition.y > - 35 )
            {
                CenterIndex = TextImage.Length - 1;
            }
            
           
          

        }

        if (!Input.GetMouseButton(0))//判断鼠标左键是否按下
        {
            ReturnItem(CenterIndex);
            Background[CenterIndex].GetComponent<Button>().enabled = true;
            Item[CenterIndex].GetComponent<Button>().enabled = true;
        }
        else
        {
            for (int i = 0; i < TextImage.Length; i++)
            {
                if (i != CenterIndex)
                {
                    Item[i].GetComponent<Button>().enabled = false;
                }
            }

            for (int i = 0; i < BackgroundImage.Length; i++)
            {
                if (i != CenterIndex)
                {
                    Background[i].GetComponent<Button>().enabled = false;
                }
            }

        }

        UpdateItem();
        UpdateBackground();
    }

    /// <summary>
    /// 位置曲线模拟当前x轴位置
    /// </summary>
    private float GetXPositionValue( float added)
    {
        float XPositionValue = PositionCurve.Evaluate(added)*120;
        return XPositionValue;
    }

    public void TextClick()
    {
        Debug.Log("text");
        SongIndex = CenterIndex;
        Application.LoadLevel("游戏界面");
    }

    public void ImageClick()
    {
        Debug.Log("Image");
        SongIndex = CenterIndex;
        Application.LoadLevel("游戏界面");
    }

    public void Back()
    {
        Application.LoadLevel("开始界面");
    }
}
