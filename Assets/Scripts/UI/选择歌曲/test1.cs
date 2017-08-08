using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.UI;

public class test1 : MonoBehaviour, IDragHandler
{

    private EnhancelScrollView enhanceScroll;

    private Transform mContent;

    private GameObject mItem;

    void Awake()
    {
        enhanceScroll = transform.Find("EnhanceScrollViewController").GetComponent<EnhancelScrollView>();
        mContent = transform.Find("EnhanceScrollViewController/Content");
        mItem = transform.Find("EnhanceScrollViewController/item").gameObject;
        InitItem();
    }

    // 拖动  
    public void OnDrag(PointerEventData eventData)
    {
        if (enhanceScroll != null)
        {
            enhanceScroll.OnSlide(eventData.delta.x);
        }
    }

    private void InitItem()
    {
        print("开始");
        List<EnhanceItem> list = new List<EnhanceItem>();
        for(int i = 0;i< 10;++i)
        {
            GameObject item = GameObject.Instantiate(mItem);
            item.name = "item" + i.ToString();
            item.SetActive(true);
            item.transform.SetParent(mContent, false);
            item.transform.GetComponent<Image>().sprite = Resources.Load<Sprite>("Image" + (i+1).ToString());
            EnhanceItem enitem = item.GetComponent<EnhanceItem>();
            enitem.scrollViewItemIndex = i;
            item.GetComponent<RectTransform>().SetSiblingIndex(i);
            list.Add(enitem);
        }

        enhanceScroll.InitScrollViewData(list);
    }
}
