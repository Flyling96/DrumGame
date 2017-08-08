using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Security.Cryptography;
using System.Text;

public class Login : MonoBehaviour {

    // Use this for initialization

    public InputField 账号;
    public InputField 密码;

    public Text Login提示;

    //建立连接
    SendMessage correspond;

    void Start () {
        IsClickSign = false;
    }


    public Text Sign账号预设提示;

	// Update is called once per frame
	void Update () {
        ClickContorller();



    }

    

    /// <summary>
    /// 登陆
    /// </summary>
    public void login()
    {
        try
        {
            string passwordkry = SHA512ToHex(密码.text);
            string tmp = "1　" + 账号.text + " " + passwordkry;
            correspond.Send(tmp);
            string tmpRecieve = correspond.Recieve();
            if(tmpRecieve=="登陆成功")
            {
                Application.LoadLevel(1);
            }
            else
            {
                Login提示.text = "账号或密码错误，请重试";
            }
        }
        catch(Exception e)
        {
            print(e.ToString());
        }
    }


    public InputField Sign账号;
    public InputField Sign密码;

    public Text Sign提示;
     
    public Button SignButton;

    //检查账号是否重复
    public void CheckAccound()
    {
        string tmp = "2.1 " + Sign账号.text;
        correspond.Send(tmp);
        string tmpRecieve = correspond.Recieve();
        if (tmpRecieve == "不存在该账号")
        {
            SignButton.enabled = true;
            Sign提示.text = "该账号可以使用";
        }
        else
        {
            SignButton.enabled = false;
            Sign提示.text = "已存在该账号，请更换一个账号";
        }
    }

    /// <summary>
    /// 注册
    /// </summary>
    public void Sign()
    {
        try
        {
            string tmp = "2.2 " + Sign账号.text + " " + Sign密码.text;
            correspond.Send(tmp);
            string tmpRecieve = correspond.Recieve();
            print(tmpRecieve);
        }
        catch(Exception e)
        {
            print(e.ToString());
        }
    }

    /// <summary>
    /// 对密码进行加密
    /// </summary>
   	public static string SHA512ToHex(string str)
    {
        byte[] SHA512Data = Encoding.UTF8.GetBytes(str);
        SHA512Managed Sha512 = new SHA512Managed();
        byte[] Result = Sha512.ComputeHash(SHA512Data);
        return BitConverter.ToString(Result).Replace("-", "").ToLower();  //返回长度为128字节的16进制字符串
    }









    /// <summary>
    /// 按键动画
    /// </summary>
    public GameObject LoginButtonALL;
    public GameObject SignButtonALL;
    bool IsClickSign = false;
    bool IsClickBack = false;
    public void ClickSign()
    {
        IsClickSign = true;
    }
    public void ClickBack()
    {
        IsClickBack = true;
    }


    bool ClickChange(GameObject b,float c)
    {
        Transform tmptrans = b.transform;
        foreach(Transform a in b.transform)
        {
            tmptrans = a;
            Color tmp = a.transform.GetComponent<Image>().color;
            a.transform.GetComponent<Image>().color = new Color(tmp.r, tmp.g, tmp.b, tmp.a + c);
            a.transform.FindChild("Text").GetComponent<Text>().color = new Color(0, 0, 0, tmp.a - 0.2f);

        }

        if (tmptrans.transform.GetComponent<Image>().color.a < 0.05f)
        {
            return false;
        }
        else
        {
            return true;
        }
    }


    /// <summary>
    /// 按钮动画控制
    /// </summary>
    void ClickContorller()
    {
        if (IsClickSign)
        {
            IsClickSign = ClickChange(LoginButtonALL, -0.02f);
            SignButtonALL.SetActive(true);
            ClickChange(SignButtonALL, 0.02f);
            if (!IsClickSign)
            {
                LoginButtonALL.SetActive(false);
            }
        }
        else if (IsClickBack)
        {
            IsClickBack = ClickChange(SignButtonALL, -0.02f);
            LoginButtonALL.SetActive(true);
            ClickChange(LoginButtonALL, 0.02f);
            if (!IsClickBack)
            {
                SignButtonALL.SetActive(false);
            }
        }
    }
}
