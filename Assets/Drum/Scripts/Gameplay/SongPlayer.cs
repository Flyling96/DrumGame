using UnityEngine;
using System.Collections;

public class SongPlayer : MonoBehaviour
{
	public SongData Song;

	protected float SmoothAudioTime = 0f;
	protected bool AudioStopEventFired = false;

    //判断是否开始游戏
	protected bool WasPlaying = false;
    //判断音乐是否开始播放
	protected bool IsSongPlaying = false;

    public bool IsOver = false;

	void Update()
	{
		if( IsPlaying() )
		{
			AudioStopEventFired = false;
			WasPlaying = true;
			UpdateSmoothAudioTime();
		}
	}

    //停止播放
	protected void OnSongStopped()
	{
		if( !GetComponent<AudioSource>().clip )
		{
			return;
		}



        //检查这首歌是否已经自动完成播放了。
        //有时它已经被重置为歌曲的开始.
        if ( GetComponent<AudioSource>().time == GetComponent<AudioSource>().clip.length
		 || ( WasPlaying && GetComponent<AudioSource>().time == 0 ) )
		{
			IsSongPlaying = false;
            IsOver = true;

            GetComponent<GuitarGameplay>().OnSongFinished();
		}
        else
        {
           // IsOver = false;
        }
	}

	protected void UpdateSmoothAudioTime()
	{
        //歌曲播放时间
		SmoothAudioTime += Time.deltaTime;

		if( SmoothAudioTime >= GetComponent<AudioSource>().clip.length )
		{
			SmoothAudioTime = GetComponent<AudioSource>().clip.length;
			OnSongStopped();
		}

        //有时音频跳或滞后，这检查如果顺利的音频时间是关闭和纠正它
        //使音符跳转或滞后
        if ( IsSmoothAudioTimeOff() )
		{
			CorrectSmoothAudioTime();
		}
	}

	protected bool IsSmoothAudioTimeOff()
	{
		//错误判断
		if( SmoothAudioTime < 0 )
		{
			return false;
		}

        //在这首歌的结尾不要检查
        if ( SmoothAudioTime > GetComponent<AudioSource>().clip.length - 3f )
		{
			return false;
		}

		//检查滑动时间和音乐时间的误差是否大于0.1
		return Mathf.Abs( SmoothAudioTime - GetComponent<AudioSource>().time ) > 0.1f;
	}



    //矫正音频
	protected void CorrectSmoothAudioTime()
	{
		SmoothAudioTime = GetComponent<AudioSource>().time;
	}





	public void Play()
	{
		IsSongPlaying = true;

        //延迟播放
		if( SmoothAudioTime < 0 )
		{
			StartCoroutine( PlayDelayed( Mathf.Abs( SmoothAudioTime ) ) );
		}
		else
		{
			GetComponent<AudioSource>().Play();
            //初始矫正
			SmoothAudioTime = GetComponent<AudioSource>().time;
		}
	}



	protected IEnumerator PlayDelayed( float delay )
	{
		yield return new WaitForSeconds( delay );

		GetComponent<AudioSource>().Play();
	}

    //暂停
	public void Pause()
	{
		IsSongPlaying = false;
		GetComponent<AudioSource>().Pause();
	}

    //结束
	public void Stop()
	{
		GetComponent<AudioSource>().Stop();
		WasPlaying = false;
		IsSongPlaying = false;
	}

	public bool IsPlaying()
	{
		return IsSongPlaying;
	}

	public void SetSong( SongData song )
	{
		Song = song;
		gameObject.GetComponent<AudioSource>().time = 0;
		gameObject.GetComponent<AudioSource>().clip = Song.BackgroundTrack;
		gameObject.GetComponent<AudioSource>().pitch = 1;

        //将节拍化为时间
        //此时的时间为初始时间
		//SmoothAudioTime = MyMath.BeatsToSeconds( -Song.AudioStartBeatOffset, Song.BeatsPerMinute );
	}

    //更正节拍
	public float GetCurrentBeat( bool songDataEditor = false )
	{
		if( songDataEditor )
		{
			SmoothAudioTime = GetComponent<AudioSource>().time;
		}

        //将当前播放的时间转换成节拍-开始的拍偏移量
        //得出从开始游戏到现在的拍数
		return MyMath.SecondsToBeats( SmoothAudioTime, Song.BeatsPerMinute ) - Song.AudioStartBeatOffset;
	}
}