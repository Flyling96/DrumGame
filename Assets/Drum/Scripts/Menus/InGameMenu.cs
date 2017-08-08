using UnityEngine;
using System.Collections;

public class InGameMenu : BaseMenu
{
	protected override void OnShowMenu()
	{
		GetComponent<SongPlayer>().Pause();
	}

	//void OnGUI()
	//{
	//	if( isVisible == false )
	//	{
	//		return;
	//	}

	//	if( CustomSkin )
	//	{
	//		GUI.skin = CustomSkin;
	//	}

	//	DrawBackground();
	//	DrawText();
	//	DrawButtons();
	//}

	void DrawText()
	{
		//Draw Pause String
		//TextStyle.fontSize = 106;
		GUI.Label( new Rect( 0, 50, Screen.width, 110 ), "Pause", TextStyle );

		//Draw Song Text
		//TextStyle.fontSize = 48;
		GUI.Label( new Rect( 0, 210, Screen.width, 50 ), GetComponent<SongPlayer>().Song.Name, TextStyle );
	}

	protected void DrawButtons()
	{
		//Calculate total height of two buttons to center them on sceen
		float height = CustomSkin.button.fixedHeight * 2 + CustomSkin.button.margin.vertical;

		GUILayout.BeginArea( new Rect( Screen.width / 2f - 250, Screen.height / 2f - height / 2f, 500, height ) );

		if( GUILayout.Button( "Resume" ) )
		{
			HideMenu();
			GetComponent<SongPlayer>().Play();
		}

		if( GUILayout.Button( "Back to Main Menu" ) )
		{
			HideMenu();
			GetComponent<GuitarGameplay>().StopPlaying();
			GetComponent<MainMenu>().ShowMenu();
		}

		GUILayout.EndArea();
	}
}