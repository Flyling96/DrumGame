using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;
using System;
//using UnityEditor;

public class WriteSong : MonoBehaviour {


    //从文件中读取数据
   // ArrayList infoall;

   // static SongData NewSongData;

   //// public SongPlayer songPlayer;

   // public InputField input;


   // public void buttonclick()
   // {
   //     NewSongData = new SongData();

   //     float time;

   //     infoall =LoadFile(Application.persistentDataPath, input.text);

   //     if (infoall != null)
   //     {

   //         print(infoall[1].ToString());
   //         NewSongData = ScriptableObject.CreateInstance<SongData>();

   //         NewSongData.Name = input.text;
   //         NewSongData.Band = "";
   //         NewSongData.BeatsPerMinute = (int)(60f/float.Parse(infoall[0].ToString()));
   //         NewSongData.AudioStartBeatOffset = 4.3f;
   //         NewSongData.BackgroundTrack = GameObject.Find("GameObject").GetComponent<AudioSource>().clip;

   //         print(GameObject.Find("GameObject").GetComponent<AudioSource>().clip);

   //         time = float.Parse(infoall[0].ToString());

   //         for (int i = 1; i < infoall.Count; i++)
   //         {
   //             print(infoall[i].ToString());
   //             CheckString(infoall[i].ToString());
   //         }

   //         CreateNewSongAsset(input.text);
   //         //SelectSong.TextString.Add(input.text);
   //         //SelectSong.TextImage.Add(SelectSong.TextImage[SelectSong.TextImage.Count - 1]);

   //        // GuitarGameplay.Playlist.Add(NewSongData);
   //     }
       

   // }

   // //解析

   // /**
   //  *a：曲谱文件中的一行
   //  */
   // void CheckString(string a)
   // {
   //     string tmp;

   //     float bate;

   //     tmp = a.Substring(0, 8);
   //     bate = float.Parse(a.Substring(10, 9));

   //     for(int i=0;i<tmp.Length;i++)
   //     {
   //        if(tmp[i]=='1')
   //        {
   //             print(tmp);
   //             print(bate);
   //             NewSongData.AddNote(bate, i);
   //        }
   //     }


   // }


   // /**
   //  *a：读取的文件名
   //  */
   // public static void CreateNewSongAsset(string a)
   // {

   //     //创建的默认位置
   //     AssetDatabase.CreateAsset(NewSongData, "Assets/Guitar Unity/Songs/" + a + ".asset");

   //     EditorUtility.FocusProjectWindow();

   //     //激活物体
   //     Selection.activeObject = NewSongData;
   // }


   // /**
   // * path：读取文件的路径
   // * name：读取文件的名称
   // */
   // ArrayList LoadFile(string path, string name)
   // {
   //     //使用流的形式读取
   //     StreamReader sr = null;
   //     try
   //     {
   //         sr = File.OpenText(path + "//" + name);
   //     }
   //     catch (Exception e)
   //     {
   //         print("未找到该文件");
   //         return null;
   //     }
   //     string line;
   //     ArrayList arrlist = new ArrayList();
   //     while ((line = sr.ReadLine()) != null)
   //     {
   //         //一行一行的读取
   //         arrlist.Add(line);
   //     }
   //     //关闭流
   //     sr.Close();
   //     //销毁流
   //     sr.Dispose();
       
   //     return arrlist;
   // }
}
