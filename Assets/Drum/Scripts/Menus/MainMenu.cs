using UnityEngine;
using System.Collections;

public class MainMenu : BaseMenu
{
	//Use this for initialization
	void Start()
	{
		ShowMenu();
	}

	void OnGUI()
	{
		if( isVisible == false )
		{
			return;
		}

		if( CustomSkin )
		{
			GUI.skin = CustomSkin;
		}

		DrawBackground();
		DrawText();
		DrawSongButtons();
	}

	void DrawText()
	{
		//TextStyle.fontSize = 106;
		GUI.Label( new Rect( 0, 50, Screen.width, 110 ), "Guitar Unity", TextStyle );

		//TextStyle.fontSize = 48;
		GUI.Label( new Rect( 0, 230, Screen.width, 50 ), "Select your Song", TextStyle );
	}

    //初始按钮
	void DrawSongButtons()
	{
		GUILayout.BeginArea( new Rect( Screen.width / 2f - 250, Screen.height / 2f - 20, 500, 400 ) );

		//Draw all songs in the playlist
	     // SongData playlist = GetComponent<GuitarGameplay>().GetPlaylist();

        //获取歌曲名单
		//for( int i = 0; i < playlist.Length; ++i )
		//{
		//	string buttonLabel = playlist[ i ].Band + " - " + playlist[ i ].Name;

		//	if( GUILayout.Button( buttonLabel ) )
		//	{
		//		GetComponent<GuitarGameplay>().StartPlaying( i );
		//		HideMenu();
		//	}
		//}

		GUILayout.EndArea();
	}
}