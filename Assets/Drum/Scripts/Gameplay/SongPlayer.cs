using UnityEngine;
using System.Collections;

public class SongPlayer : MonoBehaviour
{
	public SongData Song;

	protected float SmoothAudioTime = 0f;
	protected bool AudioStopEventFired = false;

    //�ж��Ƿ�ʼ��Ϸ
	protected bool WasPlaying = false;
    //�ж������Ƿ�ʼ����
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

    //ֹͣ����
	protected void OnSongStopped()
	{
		if( !GetComponent<AudioSource>().clip )
		{
			return;
		}



        //������׸��Ƿ��Ѿ��Զ���ɲ����ˡ�
        //��ʱ���Ѿ�������Ϊ�����Ŀ�ʼ.
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
        //��������ʱ��
		SmoothAudioTime += Time.deltaTime;

		if( SmoothAudioTime >= GetComponent<AudioSource>().clip.length )
		{
			SmoothAudioTime = GetComponent<AudioSource>().clip.length;
			OnSongStopped();
		}

        //��ʱ��Ƶ�����ͺ��������˳������Ƶʱ���ǹرպ;�����
        //ʹ������ת���ͺ�
        if ( IsSmoothAudioTimeOff() )
		{
			CorrectSmoothAudioTime();
		}
	}

	protected bool IsSmoothAudioTimeOff()
	{
		//�����ж�
		if( SmoothAudioTime < 0 )
		{
			return false;
		}

        //�����׸�Ľ�β��Ҫ���
        if ( SmoothAudioTime > GetComponent<AudioSource>().clip.length - 3f )
		{
			return false;
		}

		//��黬��ʱ�������ʱ�������Ƿ����0.1
		return Mathf.Abs( SmoothAudioTime - GetComponent<AudioSource>().time ) > 0.1f;
	}



    //������Ƶ
	protected void CorrectSmoothAudioTime()
	{
		SmoothAudioTime = GetComponent<AudioSource>().time;
	}





	public void Play()
	{
		IsSongPlaying = true;

        //�ӳٲ���
		if( SmoothAudioTime < 0 )
		{
			StartCoroutine( PlayDelayed( Mathf.Abs( SmoothAudioTime ) ) );
		}
		else
		{
			GetComponent<AudioSource>().Play();
            //��ʼ����
			SmoothAudioTime = GetComponent<AudioSource>().time;
		}
	}



	protected IEnumerator PlayDelayed( float delay )
	{
		yield return new WaitForSeconds( delay );

		GetComponent<AudioSource>().Play();
	}

    //��ͣ
	public void Pause()
	{
		IsSongPlaying = false;
		GetComponent<AudioSource>().Pause();
	}

    //����
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

        //�����Ļ�Ϊʱ��
        //��ʱ��ʱ��Ϊ��ʼʱ��
		//SmoothAudioTime = MyMath.BeatsToSeconds( -Song.AudioStartBeatOffset, Song.BeatsPerMinute );
	}

    //��������
	public float GetCurrentBeat( bool songDataEditor = false )
	{
		if( songDataEditor )
		{
			SmoothAudioTime = GetComponent<AudioSource>().time;
		}

        //����ǰ���ŵ�ʱ��ת���ɽ���-��ʼ����ƫ����
        //�ó��ӿ�ʼ��Ϸ�����ڵ�����
		return MyMath.SecondsToBeats( SmoothAudioTime, Song.BeatsPerMinute ) - Song.AudioStartBeatOffset;
	}
}