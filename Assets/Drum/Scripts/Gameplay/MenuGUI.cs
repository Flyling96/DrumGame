using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuGUI : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        IsOver();

    }


    public GameObject PauseWindows;
    public GameObject ParentGameObject;
    public void IsPause()
    {
        Time.timeScale = 0;
        ParentGameObject.GetComponent<SongPlayer>().Pause();
        PauseWindows.SetActive(true);
    }

    public void IsResume()
    {
        Time.timeScale = 1;
        ParentGameObject.GetComponent<SongPlayer>().Play();
        PauseWindows.SetActive(false);
    }

    public void IsRestart()
    {
        Time.timeScale = 1;
        SceneManager.LoadSceneAsync("游戏界面");
    }

    public void Quit()
    {
        Time.timeScale = 1;
        SceneManager.LoadSceneAsync("选择歌曲");
    }

    public GameObject OverWindows;
    public Text OverScore;
    void IsOver()
    {
        if (ParentGameObject.GetComponent<SongPlayer>().IsOver)
        {
            OverWindows.SetActive(true);
            OverScore.text = "分数： " + ParentGameObject.GetComponent<GuitarGameplay>().Score.ToString();
            //Time.timeScale = 0;
        }
    }
}
