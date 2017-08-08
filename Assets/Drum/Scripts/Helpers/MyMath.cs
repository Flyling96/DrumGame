using UnityEngine;
using System.Collections;

public static class MyMath
{
	//Convert seconds to beats with the given beats per minute
	public static float SecondsToBeats( float seconds, float bpm )
	{
		return seconds * bpm / 60;
	}

	//Convert beats to seconds in the given beats per minute
	public static float BeatsToSeconds( float beats, float bpm )
	{
		return ( beats * 60 ) / bpm;
	}
}