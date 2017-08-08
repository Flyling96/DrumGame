using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public struct TimerData
{
	public float Time;
	public string Name;
}

public class Timer
{
	List<TimerData> Timers;

	public Timer()
	{
		Timers = new List<TimerData>();
	}

	public void StartTimer( string name )
	{
        //���һ����ʱ���Ѿ����У�ֹͣ��������������ʱ��
        StopTimer();

		TimerData timer = new TimerData();
		timer.Time = Time.realtimeSinceStartup;
		timer.Name = name;

		Timers.Add( timer );
	}

	public void StopTimer()
	{
		if( Timers.Count == 0 )
		{
			return;
		}

		TimerData lastTimer = Timers[ Timers.Count - 1 ];
		lastTimer.Time = Time.realtimeSinceStartup - lastTimer.Time;

		Timers.RemoveAt( Timers.Count - 1 );
		Timers.Add( lastTimer );
	}

	public float GetTime( string name )
	{
		for( int i = 0; i < Timers.Count; ++i )
		{
			if( Timers[ i ].Name == name )
			{
				return Timers[ i ].Time;
			}
		}

		return 0f;
	}

	public List<TimerData> GetTimers()
	{
		return Timers;
	}

	public void Clear()
	{
		Timers.Clear();
	}

	public float GetTotalTime()
	{
		float totalTime = 0f;

		for( int i = 0; i < Timers.Count; ++i )
		{
			totalTime += Timers[ i ].Time;
		}

		return totalTime;
	}


}