using UnityEngine;
using System.Collections;

public class EndOfSongMenu : BaseMenu
{
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
		DrawBackToMainButton();
	}

	protected void DrawText()
	{
		//Draw Finished String
		//TextStyle.fontSize = 106;
		GUI.Label( new Rect( 0, 50, Screen.width, 110 ), "Finished", TextStyle );

		//Draw Song Name
		//TextStyle.fontSize = 48;
		GUI.Label( new Rect( 0, 210, Screen.width, 50 ), GetComponent<SongPlayer>().Song.Name, TextStyle );

		//Gather Stats
		float numNotesHit = GetComponent<GuitarGameplay>().GetNumNotesHit();
		float numNotesMissed = GetComponent<GuitarGameplay>().GetNumNotesMissed();
		float totalNotes = numNotesHit + numNotesMissed;
		float percentageOfHitNotes = Mathf.Floor( numNotesHit / totalNotes * 100 );

		//Draw Stats
		GUI.Label( new Rect( 0, 300, Screen.width, 50 ), "Score: " + GetComponent<GuitarGameplay>().GetScore().ToString( "0" ), TextStyle );
		GUI.Label( new Rect( 0, 350, Screen.width, 50 ), "Notes: " + numNotesHit.ToString( "0" ) + " / " + totalNotes.ToString( "0" ) + " (" + percentageOfHitNotes.ToString( "0" ) + "%)", TextStyle );
		GUI.Label( new Rect( 0, 400, Screen.width, 50 ), "Streak: " + GetComponent<GuitarGameplay>().GetMaximumStreak().ToString( "0" ), TextStyle );
	}

	protected void DrawBackToMainButton()
	{
		GUILayout.BeginArea( new Rect( Screen.width / 2f - 250, 500, 500, 30 ) );

		if( GUILayout.Button( "Back to Main Menu" ) )
		{
			HideMenu();
			GetComponent<GuitarGameplay>().StopPlaying();
			GetComponent<MainMenu>().ShowMenu();
		}

		GUILayout.EndArea();
	}
}