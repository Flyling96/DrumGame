using UnityEngine;
using System.Collections;
using System;
using System.IO;
using UnityEngine.UI;

public class ButtonChance : MonoBehaviour {

	// Use this for initialization

	string result;  //结果
	string[] IOresult=new string[10000];//纪录所有结果，从0开始
    int[] docount = new int[10000];//纪录每节有多少音符（add操作）
    int[] sumcount = new int[10000];//纪录前多少节总共有多少音符
	int count=0;    //第几拍
	int jiecount=1; //第几节
	float time=0;   //每拍多少时间
	float much=0;     //多少为一拍
	float jiemuch=0;  //多少拍为一节
	float sum=0;    //音符总数
	float tmp=0;    //临时的节拍数
	float stmp=0;   //临时的节拍总数
    int tmpp = 0;//临时操作数
	int i=0;


	public InputField t;
	public InputField y;
	public InputField u;
	public Button tb;
	public Button yb;
	public Button ub;

	public Button a;
	public Button b;
	public Button c;
    public Button d;
    public Button e;

    public Button b1;
	public Button b2;
	public Button b3;
	public Button b4;
	public Button b5;
	public Button b6;
	public Button b7;
	public Button b8;
	public Button b9;
	public Button b10;


	public Button add1;
    public Button find1;
    public Button back1;
    public Button deleteadd1;
    public Button delete1;
    public Button again1;

    public Button buttonparfeb;//修改

    //更改界面


	bool c1;
	bool c2;
	bool c3;
	bool c4;
	bool c5;
	bool c6;
	bool c7;
	bool c8;
	bool c9;
	bool c10;


	void Start () {


        jiecount = 1;
        tmpp = 0;
        i = 0;
        乐器数量 = 0;

        //初始化
        for (int x = 0; x < 10000; x++)
        {
            docount[x] = 0;
        }



		c1 = false;
		c2 = false;
		c3 = false;
		c4 = false;
		c5 = false;
		c6 = false;
		c7 = false;
		c8 = false;
		c9 = false;
		c10 = false;

        
		tb.onClick.AddListener (clickT);
		yb.onClick.AddListener (clickY);
	    ub.onClick.AddListener (clickU);

		a.onClick.AddListener (clickA);
		b.onClick.AddListener (clickB);
		c.onClick.AddListener (clickC);
        d.onClick.AddListener(clickD);
        e.onClick.AddListener(clickE);

        b1.onClick.AddListener (click1);
		b2.onClick.AddListener (click2);
		b3.onClick.AddListener (click3);
		b4.onClick.AddListener (click4);
		b5.onClick.AddListener (click5);
		b6.onClick.AddListener (click6);
		b7.onClick.AddListener (click7);
		b8.onClick.AddListener (click8);
		b9.onClick.AddListener (click9);
		b10.onClick.AddListener (click10);

		add1.onClick.AddListener (clickAdd);
        find1.onClick.AddListener(clickfind);
        back1.onClick.AddListener(clickbacknow);
        deleteadd1.interactable = false;
        deleteadd1.onClick.AddListener(clickchangeadd);

        next.onClick.AddListener(clicknext);
        last.onClick.AddListener(clicklast);

        delete1.onClick.AddListener(clickdelete);


        back.onClick.AddListener (clickback);

        返回界面.onClick.AddListener(返回);
        保存按键.onClick.AddListener(储存);
        确认保存.onClick.AddListener(保存);

        again1.onClick.AddListener(重来);
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(ischange)
        {
            add1.interactable = true;
            back1.interactable = true;
            find1.interactable = true;
            deleteadd1.interactable = false;
        }
        else
        {
            add1.interactable = false;
            back1.interactable = false;
            find1.interactable = false;
            deleteadd1.interactable = true;
        }
		
	}

	void clickback()
	{
		tishi.SetActive (false);
	}

	void clickT()
	{
		time =float.Parse(t.text.ToString());
		print (time);
	}
	void clickY()
	{
		much=float.Parse(y.text.ToString());
		print (much);
		yb.interactable = false;
		y.interactable = false;
	}
	void clickU()
	{
		jiemuch =float.Parse(u.text.ToString());
		print (jiemuch);
		ub.interactable = false;
		u.interactable = false;
	}


    int yi = 0;
	void clickA()
	{
        yi = 2;
        tmp = 0.25f;
		a.interactable  = false;
		b.interactable = true;
		c.interactable = true;
        d.interactable = true;
        e.interactable = true;
    }
	void clickB()
	{
        yi = 3;
        tmp = 0.125f;
		a.interactable = true;
		b.interactable = false;
		c.interactable = true;
        d.interactable = true;
        e.interactable = true;
    }
	void clickC()
	{
        yi = 4;
        tmp = 0.0625f;
		a.interactable = true;
		b.interactable = true;
		c.interactable = false;
        d.interactable = true;
        e.interactable = true;
    }
    void clickD()//二分音符
    {
        yi = 1;
        tmp = 0.5f;
        a.interactable = true;
        b.interactable = true;
        c.interactable = true;
        d.interactable = false;
        e.interactable = true;
    }
    void clickE()//全音符
    {
        yi = 0;
        tmp = 1f;
        a.interactable = true;
        b.interactable = true;
        c.interactable = true;
        d.interactable = true;
        e.interactable = false;
    }

    int 乐器数量 = 0;
    float[] 音符的y = new float[20];
    /// <summary>
    /// 选择乐器
    /// </summary>
    void click1()
	{

        if (!c1) {
			c1 = true;
			b1.image.color = new Color (0, 1, 1, 1);
		}
		else {
			c1 = false;
			b1.image.color = new Color (255, 255, 255, 1);
		}
	}
	void click2()
	{

        if (!c2) {
			c2 = true;
			b2.image.color =new Color (0, 1, 1, 1);
		}
		else {
			c2 = false;
			b2.image.color = new Color (255, 255, 255, 1);
		}
	}
	void click3()
	{

        if (!c3) {
			c3= true;
			b3.image.color =new Color (0, 1, 1, 1);
		}
		else {
			c3 = false;
			b3.image.color = new Color (255, 255, 255, 1);
		}
	}
	void click4()
	{

        if (!c4) {
			c4 = true;
			b4.image.color=new Color (0, 1, 1, 1);
		}
		else {
			c4 = false;
			b4.image.color = new Color (255, 255, 255, 1);
		}
	}
	void click5()
	{

        if (!c5) {
			c5= true;
			b5.image.color =new Color (0, 1, 1, 1);
		}
		else {
			c5 = false;
			b5.image.color= new Color (255, 255, 255, 1);
		}
	}
	void click6()
	{

        if (!c6) {
			c6= true;
			b6.image.color =new Color (0, 1, 1, 1);
		}
		else {
			c6 = false;
			b6.image.color = new Color (255, 255, 255, 1);
		}
	}
	void click7()
	{

        if (!c7) {
			c7= true;
			b7.image.color =new Color (0, 1, 1, 1);
		}
		else {
			c7 = false;
			b7.image.color = new Color (255, 255, 255, 1);
		}
	}
	void click8()
	{

        if (!c8) {
			c8= true;
			b8.image.color =new Color (0, 1, 1, 1);
		}
		else {
			c8 = false;
			b8.image.color = new Color (255, 255, 255, 1);
		}
	}
	void click9()
	{

        if (!c9) {
			c9= true;
			b9.image.color = new Color (0, 1, 1, 1);
		}
		else {
			c9 = false;
			b9.image.color = new Color (255, 255, 255, 1);
		}
	}
	void click10()
	{
        if (!c10) {
			c10= true;
			b10.image.color = new Color (0, 1, 1, 1);
		}
		else {
			c10= false;
			b10.image.color = new Color (255, 255, 255, 1);
		}
	}

	public GameObject tishi;
	public Text connent;
	public Button back;

    public GameObject[] 音符;

    public GameObject parent;

    public Text 节数;

    float[] 位置 = { -56.3f, -63.8f, -70.3f, -83.3f, -85.7f, -93f, -104.7f, -118f, -125f };

    /// <summary>
    /// 增加
    /// </summary>

    void clickAdd()
	{
        //换节
        
        if(stmp==0)
        {
            节数.text =  jiecount.ToString() ;
            foreach (Transform a in parent.transform)
            {
                Destroy(a.gameObject);
            }
           
        }

		sum = jiemuch / much;
        乐器数量 = 0;
        if (tmp != 0)
        {
            if (stmp + tmp <= sum)
            {

                // tmpg.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                tmpp++;
                stmp += tmp;
          
                if (!c10)
                {
                    if (c1)
                    {
                        result += "1";
                        音符的y[乐器数量] = 位置[0];
                        乐器数量++;
                    }
                    else
                    {
                        
                        result += "0";
                    }

                    if (c2)
                    {
                        result += "1";
                        音符的y[乐器数量] = 位置[1];
                        乐器数量++;
                    }
                    else
                    {
                        result += "0";
                    }

                    if (c3)
                    {
                        result += "1";
                        音符的y[乐器数量] = 位置[2];
                        乐器数量++;
                    }
                    else
                    {
                        result += "0";
                    }

                    if (c4)
                    {
                        result += "1";
                        音符的y[乐器数量] = 位置[3];
                        乐器数量++;
                    }
                    else
                    {
                        result += "0";
                    }

                    if (c5)
                    {
                        result += "1";
                        音符的y[乐器数量] = 位置[4];
                        乐器数量++;
                    }
                    else
                    {
                        result += "0";
                    }

                    if (c6)
                    {
                        result += "1";
                        音符的y[乐器数量] = 位置[5];
                        乐器数量++;
                    }
                    else
                    {
                        result += "0";
                    }

                    if (c7)
                    {
                        result += "1";
                        音符的y[乐器数量] = 位置[6];
                        乐器数量++;
                    }
                    else
                    {
                        result += "0";
                    }

                    if (c8)
                    {
                        result += "1";
                        音符的y[乐器数量] = 位置[7];
                        乐器数量++;
                    }
                    else
                    {
                        result += "0";
                    }

                    if (c9)
                    {
                        result += "1";
                        音符的y[乐器数量] = 位置[8];
                        乐器数量++;
                    }
                    else
                    {
                        result += "0";
                    }
                }
                else
                {
                    result = "000000000";
                }

                //纪录结果
                result += " " + 统一格式((jiecount - 1 + stmp / sum)* jiemuch);//统一格式
                result += " " + (1 / tmp).ToString();


                for (int q = 0; q < 乐器数量; q++)
                {
                    GameObject tmpg = Instantiate(音符[yi], 音符[yi].transform.position, 音符[yi].transform.rotation) as GameObject;
                    tmpg.transform.parent = parent.transform;
                    tmpg.name = "y" + (docount[jiecount]+1).ToString();
                    tmpg.GetComponent<RectTransform>().localPosition = new Vector3(-220 + 450 * stmp/ sum, 音符的y[q], 0);
                    tmpg.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                }

                GameObject tmpb = Instantiate(buttonparfeb.gameObject, buttonparfeb.transform.position, buttonparfeb.transform.rotation) as GameObject;
                tmpb.transform.parent = parent.transform;
                tmpb.name = tmpp.ToString();
                tmpb.GetComponent<RectTransform>().localPosition = new Vector3(-220 + 450 * stmp / sum, -43, -5);
                tmpb.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                tmpb.GetComponent<Button>().onClick.AddListener(delegate () { int x = int.Parse(tmpb.name); clickchange(x, tmpb.GetComponent<RectTransform>().localPosition.x); });


                print(result);
                IOresult[i] = result;
                count = i;
                i++;
                result = "";
                docount[jiecount]++;

                //换节
                if (stmp == sum)
                {
                    tmpp = 0;
                    stmp = 0;
                    if (jiecount == 1)
                    {
                        sumcount[jiecount] = docount[jiecount];
                    }
                    else
                    {
                        sumcount[jiecount] = sumcount[jiecount - 1] + docount[jiecount];
                    }
                    jiecount++;
                }

            }
            else
            {
                tishi.SetActive(true);
                connent.text = "超过该小节的拍数";
            }
        }
        else
        {
            tishi.SetActive(true);
            connent.text = "请选择音符类型";
        }
	}


    public InputField 查找节数;

    string[] tmpresult = new string[100];//记录下跳转的节的所有操作
    string[] addresult = new string[100];//记录下清空后添加的操作

    void clickfind()
    {
        try {
            int jieshu = int.Parse(查找节数.text);
            if (jieshu > 0 && jieshu < jiecount)
            {
                find(jieshu);
            }
            else if (jieshu == jiecount)
            {
                clickbacknow();
            }
            else
            {
                tishi.SetActive(true);
                connent.text = "请注意输入范围";
            }
        }
        catch
        {
            tishi.SetActive(true);
            connent.text = "输入类型错误";
        }
    }
    void clickfindnear(int jieshu)
    {
 
            if (jieshu > 0 && jieshu < jiecount)
            {
                find2(jieshu);
            }
            else if (jieshu == jiecount)
            {
                clickbacknow();
            }
       
    }
    /// <summary>
    /// 节数寻找
    /// </summary>
    void find(int jieshu)
    {
        //初始化
        for (int t = 0; t < 100; t++)
        {
            tmpresult[t] = null;
        }
        foreach (Transform a in parent.transform)
        {
            Destroy(a.gameObject);
        }

        string tmps = "";
        string 鼓位 = "";
        string 实时拍数 = "";
        string 音符类型 = "";
        节数.text =  查找节数.text ;
        float 初始节拍数 =(jieshu - 1) * jiemuch;
        int addcount =sumcount[jieshu-1];//查找的节数之前的操作和
        for(int q=0;q<docount[jieshu];q++)
        {
            鼓位 = "";
            实时拍数 = "";
            音符类型 = "";
            tmps = IOresult[addcount + q];
            tmpresult[q] = tmps;//记录下跳转的节的所有操作，为更改操作做准备
            for (int s=0;s < tmps.Length;s++)
            {
               
                if (s<9)
                {
                    鼓位 += tmps[s];
                }
                else if(s>9&&s<19)
                {
                    实时拍数+= tmps[s];
                }
                else if(s>19)
                {
                    音符类型 += tmps[s];
                }
            }
            xianshi(鼓位, 实时拍数, 音符类型, 初始节拍数,q);
        }




        

    }
    void find2(int jieshu)
    {
        //初始化
        for (int t = 0; t < 100; t++)
        {
            tmpresult[t] = null;
        }
        foreach (Transform a in parent.transform)
        {
            Destroy(a.gameObject);
        }

        string tmps = "";
        string 鼓位 = "";
        string 实时拍数 = "";
        string 音符类型 = "";
        float 初始节拍数 = (jieshu - 1) * jiemuch;
        int addcount = sumcount[jieshu - 1];//查找的节数之前的操作和
        for (int q = 0; q < docount[jieshu]; q++)
        {
            鼓位 = "";
            实时拍数 = "";
            音符类型 = "";
            tmps = IOresult[addcount + q];
            tmpresult[q] = tmps;//记录下跳转的节的所有操作，为更改操作做准备
            for (int s = 0; s < tmps.Length; s++)
            {

                if (s < 9)
                {
                    鼓位 += tmps[s];
                }
                else if (s > 9 && s < 19)
                {
                    实时拍数 += tmps[s];
                }
                else if (s > 19)
                {
                    音符类型 += tmps[s];
                }
            }
            xianshi(鼓位, 实时拍数, 音符类型, 初始节拍数, q);
        }
    }

        void clickbacknow()
    {
        //初始化
        for(int t=0;t<100;t++)
        {
            tmpresult[t] = null;
        }
        
        foreach (Transform a in parent.transform)
        {
            Destroy(a.gameObject);
        }

        int jieshu = jiecount - 1;
        节数.text =  jiecount.ToString() ;
        print(sumcount[jieshu]);
        int caozuo = count- sumcount[jieshu]+1;//操作数
        print(caozuo);
        string tmps = "";
        string 鼓位 = "";
        string 实时拍数 = "";
        string 音符类型 = "";
        float 初始节拍数 = jieshu * jiemuch;
        int addcount = sumcount[jieshu];//查找的节数之前的操作和
        for (int q = 0; q < caozuo; q++)
        {
            鼓位 = "";
            实时拍数 = "";
            音符类型 = "";
            tmps = IOresult[addcount + q];
            tmpresult[q] = tmps;
            for (int s = 0; s < tmps.Length; s++)
            {

                if (s < 9)
                {
                    鼓位 += tmps[s];
                }
                else if (s > 9 && s < 19)
                {
                    实时拍数 += tmps[s];
                }
                else if (s > 19)
                {
                    音符类型 += tmps[s];
                }
            }
            xianshi(鼓位, 实时拍数, 音符类型, 初始节拍数,q);
        }


    }
    void xianshi(string 鼓位,string 实时拍数,string 音符类型,float 初始节拍数,int q1)
    {
        
        int 音符数 = int.Parse(音符类型);
        int tmpy = 0;
        float 拍数 = float.Parse(实时拍数);    
        if (音符数==1)
        {
            tmpy = 0;
        }
        else if(音符数==2)
        {
            tmpy = 1;
        }
        else if (音符数 == 4)
        {
            tmpy = 2;
        }
        else if (音符数 == 8)
        {
            tmpy = 3;
        }
        else if (音符数 == 16)
        {
            tmpy = 4;
        }
       
        //生成
        for (int q=0;q<9;q++)
        {
            if(鼓位[q]=='1')
            {
                GameObject tmpg = Instantiate(音符[tmpy], 音符[tmpy].transform.position, 音符[tmpy].transform.rotation) as GameObject;
                tmpg.transform.parent = parent.transform;
                tmpg.name = "y" + (q1 + 1).ToString();
                tmpg.GetComponent<RectTransform>().localPosition = new Vector3(-220 + 450 * (拍数 - 初始节拍数) / jiemuch, 位置[q], 0);
                tmpg.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            }
        }
        GameObject tmpb = Instantiate(buttonparfeb.gameObject, buttonparfeb.transform.position, buttonparfeb.transform.rotation) as GameObject;
        tmpb.transform.parent = parent.transform;
        tmpb.name = (q1 + 1).ToString();
        tmpb.GetComponent<RectTransform>().localPosition = new Vector3(-220 + 450 * (拍数 - 初始节拍数) / jiemuch, -43, -5);
        tmpb.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        tmpb.GetComponent<Button>().onClick.AddListener(delegate () { int x = int.Parse(tmpb.name); clickchange(x, tmpb.GetComponent<RectTransform>().localPosition.x); });

    }
    //下一节
    public Button next;
    public Button last;
    void clicknext()
    {
        int jieshu1 = int.Parse(节数.text)+1;
        if (jieshu1+1 > jiecount)
            next.interactable = false;
        节数.text = jieshu1.ToString();
        clickfindnear(jieshu1);
        last.interactable = true;
    }
    //上一节
    void clicklast()
    {
        int jieshu1 = int.Parse(节数.text) -1;
        if (jieshu1-1 < 1)
            last.interactable = false;
        节数.text = jieshu1.ToString();
        clickfindnear(jieshu1);
        next.interactable = true;
    }
    /// <summary>
    /// 更改.....害怕
    /// </summary>

    void clickchange(int a,float x)
    {
        print(a);
        int 更改操作数 = a;
        float tmpwhere = x;
        int jieshu = int.Parse (节数.text);
        print(jieshu);
        int qishi = sumcount[jieshu - 1];
        string tresult = "";

        foreach (Transform d in parent.transform)
        {
            if (d.name == "y" + a.ToString())
            {
                print(1);
                Destroy(d.gameObject);
            }
        }
        乐器数量 = 0;
        if (!c10)
        {
            if (c1)
            {
                tresult += "1";
                音符的y[乐器数量] = 位置[0];
                乐器数量++;
            }
            else
            {

                tresult += "0";
            }

            if (c2)
            {
                tresult += "1";
                音符的y[乐器数量] = 位置[1];
                乐器数量++;
            }
            else
            {
                tresult += "0";
            }

            if (c3)
            {
                tresult += "1";
                音符的y[乐器数量] = 位置[2];
                乐器数量++;
            }
            else
            {
                tresult += "0";
            }

            if (c4)
            {
                tresult += "1";
                音符的y[乐器数量] = 位置[3];
                乐器数量++;
            }
            else
            {
                tresult += "0";
            }

            if (c5)
            {
                tresult += "1";
                音符的y[乐器数量] = 位置[4];
                乐器数量++;
            }
            else
            {
                tresult += "0";
            }

            if (c6)
            {
                tresult += "1";
                音符的y[乐器数量] = 位置[5];
                乐器数量++;
            }
            else
            {
                tresult += "0";
            }

            if (c7)
            {
                tresult += "1";
                音符的y[乐器数量] = 位置[6];
                乐器数量++;
            }
            else
            {
                tresult += "0";
            }

            if (c8)
            {
                tresult += "1";
                音符的y[乐器数量] = 位置[7];
                乐器数量++;
            }
            else
            {
                tresult += "0";
            }

            if (c9)
            {
                tresult += "1";
                音符的y[乐器数量] = 位置[8];
                乐器数量++;
            }
            else
            {
                tresult += "0";
            }
        }
        else
        {
            tresult = "000000000";
        }
        print(乐器数量);
        for(int q=9;q< IOresult[qishi + 更改操作数 - 1].Length;q++)
        {
            tresult += IOresult[qishi + 更改操作数 - 1][q];
        }
        IOresult[qishi + 更改操作数 - 1] = tresult;
        print(tresult);

        for (int q = 0; q < 乐器数量; q++)
        {
            GameObject tmpg = Instantiate(音符[yi], 音符[yi].transform.position, 音符[yi].transform.rotation) as GameObject;
            tmpg.transform.parent = parent.transform;
            tmpg.name = "y" + a.ToString();
            tmpg.GetComponent<RectTransform>().localPosition = new Vector3(tmpwhere, 音符的y[q], 0);
            tmpg.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        }

        GameObject tmpb = Instantiate(buttonparfeb.gameObject, buttonparfeb.transform.position, buttonparfeb.transform.rotation) as GameObject;
        tmpb.transform.parent = parent.transform;
        tmpb.name = a.ToString();
        tmpb.GetComponent<RectTransform>().localPosition = new Vector3(tmpwhere, -43, -5);
        tmpb.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        tmpb.GetComponent<Button>().onClick.AddListener(delegate () { int c = int.Parse(tmpb.name); clickchange(c, tmpb.GetComponent<RectTransform>().localPosition.x); });



    }


    int DeleteAddCount = 0;
    //清空
    void clickdelete()
    {
        for (int t = 0; t < 100; t++)
        {
            addresult[t] = null;
        }
        ischange = false;
        DeleteAddCount = 0;
        atmp = 0;
        foreach (Transform a in parent.transform)
        {
            Destroy(a.gameObject);
        }

        int jieshu = int.Parse(节数.text);
        int caozuoshu = docount[jieshu];       
    }

    bool ischange = true;//是否可以跳转
    bool isback = true;//是否回到最新位置
    //清空后添加
    float atmp = 0;
    void clickchangeadd()
    {
        //初始化

        string tresult = "";
        sum = jiemuch / much;
        乐器数量 = 0;
        if (tmp != 0)
        {
            if (atmp + tmp <= sum)
            {
                DeleteAddCount++;
                // tmpg.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                tmpp++;
                atmp += tmp;
                if (!c10)
                {
                    if (c1)
                    {
                        tresult += "1";
                        音符的y[乐器数量] = 位置[0];
                        乐器数量++;
                    }
                    else
                    {

                        tresult += "0";
                    }

                    if (c2)
                    {
                        tresult += "1";
                        音符的y[乐器数量] = 位置[1];
                        乐器数量++;
                    }
                    else
                    {
                        tresult += "0";
                    }

                    if (c3)
                    {
                        tresult += "1";
                        音符的y[乐器数量] = 位置[2];
                        乐器数量++;
                    }
                    else
                    {
                        tresult += "0";
                    }

                    if (c4)
                    {
                        tresult += "1";
                        音符的y[乐器数量] = 位置[3];
                        乐器数量++;
                    }
                    else
                    {
                        tresult += "0";
                    }

                    if (c5)
                    {
                        tresult += "1";
                        音符的y[乐器数量] = 位置[4];
                        乐器数量++;
                    }
                    else
                    {
                        tresult += "0";
                    }

                    if (c6)
                    {
                        tresult += "1";
                        音符的y[乐器数量] = 位置[5];
                        乐器数量++;
                    }
                    else
                    {
                        tresult += "0";
                    }

                    if (c7)
                    {
                        tresult += "1";
                        音符的y[乐器数量] = 位置[6];
                        乐器数量++;
                    }
                    else
                    {
                        tresult += "0";
                    }

                    if (c8)
                    {
                        tresult += "1";
                        音符的y[乐器数量] = 位置[7];
                        乐器数量++;
                    }
                    else
                    {
                        tresult += "0";
                    }

                    if (c9)
                    {
                        tresult += "1";
                        音符的y[乐器数量] = 位置[8];
                        乐器数量++;
                    }
                    else
                    {
                        tresult += "0";
                    }
                }
                else
                {
                    tresult = "000000000";
                }

                //纪录结果
                tresult += " " + 统一格式((int.Parse(节数.text) - 1 + atmp / sum) * jiemuch);//统一格式
                tresult += " " + (1 / tmp).ToString();

                for (int q = 0; q < 乐器数量; q++)
                {
                  
                    GameObject tmpg = Instantiate(音符[yi], 音符[yi].transform.position, 音符[yi].transform.rotation) as GameObject;
                    tmpg.transform.parent = parent.transform;
                    tmpg.name = "y" + DeleteAddCount.ToString();
                    tmpg.GetComponent<RectTransform>().localPosition = new Vector3(-220 + 450 * atmp / sum, 音符的y[q], 0);
                    tmpg.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                }
                GameObject tmpb = Instantiate(buttonparfeb.gameObject, buttonparfeb.transform.position, buttonparfeb.transform.rotation) as GameObject;
                tmpb.transform.parent = parent.transform;
                tmpb.name = tmpp.ToString();
                tmpb.GetComponent<RectTransform>().localPosition = new Vector3(-220 + 450 * atmp / sum, -43, -5);
                tmpb.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                tmpb.GetComponent<Button>().onClick.AddListener(delegate () { int x = int.Parse(tmpb.name); clickchange(x, tmpb.GetComponent<RectTransform>().localPosition.x); });


                print(tresult);
                
                addresult[DeleteAddCount] = tresult;
                tresult = "";

                if (atmp==sum)//进行Ioresult的调整
                {
                    atmp = 0;
                    ischange = true;
                    int jieshu = int.Parse(节数.text);
                    int subcount = DeleteAddCount - docount[jieshu];
                    if(jieshu==jiecount)//如果清空的节是最新的节则
                    {
                        int q = sumcount[jieshu - 1];
                        i = q + DeleteAddCount; //i和count纪录最新的位置
                        count = i - 1;
                        for(int e=1;e<DeleteAddCount+1;e++)
                        {
                            IOresult[q - 1 + e] = addresult[e];
                        }
                        jiecount++;
                        stmp = 0;
                    }
                    else
                    {
                        docount[jieshu] += subcount;
                        for(int e=jieshu;e<jiecount;e++)
                        {
                            sumcount[e] += subcount;
                        }
                        if(subcount>0)//矫正
                        {
                            int tmpi = sumcount[jieshu]-subcount;
                            for(int e=i-1;e>tmpi-1;e--)//后移
                            {
                                IOresult[e + subcount] = IOresult[e];
                            }
                            for(int e=1;e<docount[jieshu]+1;e++)
                            {
                                IOresult[sumcount[jieshu - 1] + e - 1] = addresult[e];
                            }
                            i += subcount;
                            count += subcount;
                            print(IOresult[count]);
                        }
                        else if(subcount<0)
                        {
                            int tmpi = sumcount[jieshu];
                            for(int e=tmpi;e< i;e++)//前移
                            {
                                IOresult[e + subcount] = IOresult[e];
                            }
                            for (int e = 1; e < docount[jieshu]+1; e++)
                            {
                                IOresult[sumcount[jieshu - 1] + e - 1] = addresult[e];
                            }
                            i += subcount;
                            count += subcount;
                        }
                        else
                        {
                            for (int e = 1; e < docount[jieshu]+1; e++)
                            {
                                IOresult[sumcount[jieshu - 1] + e - 1] = addresult[e];
                            }
                        }

                    }

                }
                
                

            }
            else
            {
                tishi.SetActive(true);
                connent.text = "超过该小节的拍数";
            }
        }
        else
        {
            tishi.SetActive(true);
            connent.text = "请选择音符类型";
        }
    }


   

    string 统一格式(double dblQ)
    {
        string q="";
        if (dblQ < 10)
        {
            q = dblQ.ToString("#0.0000000");
        }
        else if(dblQ < 100)
        {
            q = dblQ.ToString("#0.000000");
        }
        else if (dblQ < 1000)
        {
            q = dblQ.ToString("#0.00000");
        }
        else if (dblQ < 10000)
        {
            q = dblQ.ToString("#0.0000");
        }
        return q;
    }

    public InputField 保存文件名;
    public GameObject 保存界面;
    public Button 返回界面;
    public Button 保存按键;
    public Button 确认保存;


    void 储存()
    {
        保存界面.SetActive(true);
    }
    void 返回()
    {
        保存界面.SetActive(false);
    }
    void 保存()
    {

        try {
            string name = 保存文件名.text;
            DeleteFile(Application.persistentDataPath, name);
            CreateFile(Application.persistentDataPath, name, time.ToString());
            for (int p = 0; p < i; p++)
            {
                CreateFile(Application.persistentDataPath, name, IOresult[p]);
            }

            保存界面.SetActive(false);
            tishi.SetActive(true);
            connent.text = "保存成功";
        }
        catch
        {
            tishi.SetActive(true);
            connent.text = "保存不成功";
        }

    }

    void CreateFile(string path, string name, string info)
    {
        //文件流信息
        StreamWriter sw;
        FileInfo t = new FileInfo(path + "//" + name);
        if (!t.Exists)
        {
            //如果此文件不存在则创建
            sw = t.CreateText();
        }
        else
        {
            //如果此文件存在则打开
            sw = t.AppendText();
        }
        //以行的形式写入信息
        sw.WriteLine(info);
        //关闭流
        sw.Close();
        //销毁流
        sw.Dispose();
    }

    //删除
    void DeleteFile(string path, string name)
    {
        File.Delete(path + "//" + name);

    }


    void 重来()
    {
        Application.LoadLevel("制谱端");
    }

    public void Quit()
    {
        Application.LoadLevel("开始界面");
    }
}