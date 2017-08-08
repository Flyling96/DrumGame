using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GuitarGameplay : MonoBehaviour
{
    //��¼ÿһ�л��еĴ���
    int[] StringCount = new int[8];

    //��¼ÿ���ܹ��ж��ٸ�
    int[] StringNoteCount = new int[8];

	public GameObject NotePrefab;
    public GameObject[] NewNotePrefab;
    public List<SongData> Playlist;


	protected GameObject GuitarNeckObject;
	protected ControlInput ControlInput;
	protected SongPlayer Player;

    //Note��
	protected List<GameObject> NoteObjects;
	protected Color[] Colors;

	//����
	public float Score = 0f;
    public Text ScoreText;
   
	protected float Multiplier = 1f;
	protected float Streak = 0;
	protected float MaxStreak = 0;
	protected float NumNotesHit = 0;
	protected float NumNotesMissed = 0;

	protected bool[] HasHitNoteOnStringIndexThisFrame;



	void Start()
	{
      
   
        //�ⲿ����/�����ʼ������
        ControlInput = GameObject.Find( "Guitar" ).GetComponent<ControlInput>();
		GuitarNeckObject = transform.Find( "Guitar Neck" ).gameObject;
		Player = GetComponent<SongPlayer>();
		NoteObjects = new List<GameObject>();
        StartPlaying(SelectSong.SongIndex);

        Score = 0;
        ScoreText.text = Score.ToString();

        HasHitNoteOnStringIndexThisFrame = new bool[ 8 ];
        perfactcount = 0;
        //���д�����ʼ��
        for (int i=0;i<8;i++)
        {
            StringCount[i] = 0;
        }
	

		//UpdateColorsArray();
	}

	void Update()
	{

        ScoreText.text = Score.ToString();

        if ( Player.IsPlaying() )
		{
			//����Ƿ���ESC
			ShowInGameMenuOnKeypress();


			ResetHasHitNoteOnStringIndexArray();

			UpdateNeckTextureOffset();
			UpdateNotes();

			UpdateGuiScore();
			UpdateGuiMultiplier();

            //StrikingEffect();

        }
	}


    //������ɫ
    public void UpdateColorsArray()
	{
        //�����ɫ����û�г�ʼ��
        if ( Colors == null || Colors.Length != 8 )
		{
			Colors = new Color[ 8 ];
		}

      
		//��ȡ�˸�����
		for( int i = 0; i < 8; ++i )
		{
			GameObject stringButton = GameObject.Find( "StringButton" + ( i + 1 ) );
			Color color = stringButton.GetComponent<StringButton>().Color;

			if( color != Colors[ i ] )
			{
				Colors[ i ] = color;
				//stringButton.transform.Find( "Paddle" ).GetComponent<Renderer>().sharedMaterial.color = color;
				//stringButton.transform.Find( "Socket" ).GetComponent<Renderer>().sharedMaterial.color = color;
				stringButton.transform.Find( "Light" ).GetComponent<Light>().color = color;

#if !UNITY_EDITOR && (UNITY_IPHONE || UNITY_ANDROID)
				stringButton.transform.Find( "KeyCode Label" ).GetComponent<Renderer>().enabled = false;
#endif
            }
        }
	}



    //��ť����
	public void StartPlaying( int playlistIndex )
	{

        //���Գ�ʼ��
		ResetGameStateValues();

        //��ϷUI
		SetInGameUserInterfaceVisibility( true );

        //���и����ļ�
		Player.SetSong( Playlist[ playlistIndex ] );

		CreateNoteObjects();
        //DragonHP();

        Player.Play();
		StartCoroutine( "DisplayCountdown" );
	}

    ////ϵ�����Գ�ʼ��
    //float HPCount;
    //float HumanHPCount;
    //float Humantmp;
    //float tmp;
    //void DragonHP()
    //{
    //     HPCount = (StringNoteCount[2] + StringNoteCount[3] + StringNoteCount[4])/5-1;
    //     HumanHPCount = (StringNoteCount[5] + StringNoteCount[6] + StringNoteCount[7]) * 0.2f;
    //     tmp = HPCount;
    //     Humantmp = HumanHPCount;
    //}



	public void StopPlaying()
	{
		SetInGameUserInterfaceVisibility( false );

		DestroyNoteObjects();

		StopAllCoroutines();
	}

	protected void SetInGameUserInterfaceVisibility( bool show )
	{
		//GameObject.Find( "GUI Score" ).GetComponent<GUIText>().enabled = show;
		//GameObject.Find( "GUI Multiplier" ).GetComponent<GUIText>().enabled = show;
	}

	protected IEnumerator StartAudio( float delay )
	{
		yield return new WaitForSeconds( delay );

		Player.Play();
	}

    //Note�Ĵ���
	protected void CreateNoteObjects()
	{

        for(int i=0;i<8;i++)
        {
            StringNoteCount[i] = 0;
        }
        //�����б�
		NoteObjects.Clear();

		for( int i = 0; i < Player.Song.Notes.Count; ++i )
		{
			//if( Player.Song.Notes[ i ].StringIndex != 0 && Player.Song.Notes[ i ].StringIndex != 4 )
			{
				//����Note��β��
				GameObject note = InstantiateNoteFromPrefab( Player.Song.Notes[ i ].StringIndex );
				CreateTrailObject( note, Player.Song.Notes[ i ] );

				
				note.GetComponent<Renderer>().enabled = false;

                note.SetActive(false);

				NoteObjects.Add( note );
			}
		}
	}

    //���������
	protected void DestroyNoteObjects()
	{
		for( int i = 0; i < NoteObjects.Count; ++i )
		{
			Destroy( NoteObjects[ i ] );
		}

		NoteObjects.Clear();
	}



	public List<SongData> GetPlaylist()
	{
		return Playlist;
	}


    //��ʼ����ʱ����
	protected IEnumerator DisplayCountdown()
	{

		yield return new WaitForSeconds( MyMath.BeatsToSeconds( 0.5f, Player.Song.BeatsPerMinute ) );

		StartCoroutine( DisplayText( "4", 1f, 0f ) );
		yield return new WaitForSeconds( MyMath.BeatsToSeconds( 1f, Player.Song.BeatsPerMinute ) );

		StartCoroutine( DisplayText( "3", 1f, 0f ) );
		yield return new WaitForSeconds( MyMath.BeatsToSeconds( 1f, Player.Song.BeatsPerMinute ) );

		StartCoroutine( DisplayText( "2", 1f, 0f ) );
		yield return new WaitForSeconds( MyMath.BeatsToSeconds( 1f, Player.Song.BeatsPerMinute ) );

		StartCoroutine( DisplayText( "1", 1f, 0f ) );
		yield return new WaitForSeconds( MyMath.BeatsToSeconds( 1f, Player.Song.BeatsPerMinute ) );

		StartCoroutine( DisplayText( "Go", MyMath.BeatsToSeconds( 1.5f, Player.Song.BeatsPerMinute ), MyMath.BeatsToSeconds( 1f, Player.Song.BeatsPerMinute ) ) );
	}

	protected void StopCountdown()
	{
		StopCoroutine( "DisplayCountdown" );
		GameObject.Find( "GUI Text" ).GetComponent<GUIText>().text = "";
	}

	protected void ResetHasHitNoteOnStringIndexArray()
	{
		for( int i = 0; i < 8; ++i )
		{
			HasHitNoteOnStringIndexThisFrame[ i ] = false;
		}
	}

	protected void ShowInGameMenuOnKeypress()
	{
		if( Input.GetKeyDown( KeyCode.Escape ) )
		{
			StopCountdown();
			GetComponent<InGameMenu>().ShowMenu();
		}
	}

    public Text a;
    int perfactcount;
	protected void UpdateNotes()
	{
		for( int i = 0; i < NoteObjects.Count; ++i )
		{
            //����Noteλ��
			UpdateNotePosition( i );

            //�ж�Note�Ƿ񱻻���
			if( IsNoteHit( i ) )
			{
                //�ж�����
                if (IsPerfactZone(NoteObjects[i]))
                {
                    perfactcount++;
                    //ֻ���ó�Perfact��Great���ܼӴ���
                    //StringCount[(int)NoteObjects[i].transform.position.x + 2]++;
                    a.text = "Perfact";
                    Score += 5;

                    Debug.Log("Perfact");
                }
                else if (IsGreatZone(NoteObjects[i]))
                {
                    //StringCount[(int)NoteObjects[i].transform.position.x + 2]++;
                    a.text = "Great";
                    Score += 3;

                    Debug.Log("Great");
                }
                else if (IsGoodZone(NoteObjects[i]))
                {
                    a.text = "Good";
                    Score += 2;

                    Debug.Log("Good");
                }
                else if (IsMissZone(NoteObjects[i]))
                {
                    //if((int)NoteObjects[i].transform.position.x + 2>4)
                    //{
                     
                    //    Humantmp--;
                    //}
                    a.text = "Miss";
                    Debug.Log("Miss");
                }

                HideNote( i );
				//Score += 10f * Multiplier;
				Streak++;
				NumNotesHit++;

				if( Streak > MaxStreak )
				{
					MaxStreak = Streak;
				}

				//������е�Ϊ������
				if( Player.Song.Notes[ i ].Length > 0f )
				{
					StartTrailHitRoutineForNote( i );
				}
				else
				{
					ShowFireParticlesForNote( i );
				}
			}

			if( WasNoteMissed( i ) )
			{
                //if ((int)NoteObjects[i].transform.position.x + 2 > 4)
                //{
                //    Humantmp--;
                //}
                a.text = "Miss";
                HideNote( i );

				Streak = 0;
				Multiplier = 1;

				NumNotesMissed++;
			}
		}
	}

	protected void StartTrailHitRoutineForNote( int index )
	{
		GameObject trail = NoteObjects[ index ].transform.Find( "Trail" ).gameObject;

		StartCoroutine( TrailHitRoutine( index, trail ) );
	}

	protected void ShowFireParticlesForNote( int index )
	{
		StartCoroutine( ShowFireParticles( Player.Song.Notes[ index ].StringIndex, index));
	}

    public GameObject[] Particles;
	protected IEnumerator ShowFireParticles( int stringIndex ,int index)
	{
        //������Ч
        GameObject tmp = Instantiate(Particles[stringIndex], NoteObjects[index].transform.position, Particles[stringIndex].transform.rotation);
        yield return new WaitForSeconds(2);
        Destroy(tmp);
       //ControlInput.GetStringButton( stringIndex ).transform.Find( "Flame" ).GetComponent<ParticleEmitter>().ClearParticles();
       //ControlInput.GetStringButton( stringIndex ).transform.Find( "Flame" ).GetComponent<ParticleEmitter>().emit = true;

       yield return null;

		//ControlInput.GetStringButton( stringIndex ).transform.Find( "Flame" ).GetComponent<ParticleEmitter>().emit = false;
	}

	protected IEnumerator TrailHitRoutine( int noteIndex, GameObject trail )
	{
		Note note = Player.Song.Notes[ noteIndex ];

		//����β������ɫ
		trail.GetComponent<Renderer>().material.color = Colors[ note.StringIndex ];

		//��ס����Ч
		ControlInput.GetStringButton( note.StringIndex ).transform.Find( "Sparks" ).GetComponent<ParticleEmitter>().emit = true;
		ControlInput.GetStringButton( note.StringIndex ).transform.Find( "Sparks" ).GetComponent<ParticleEmitter>().GetComponent<Renderer>().enabled = true;

		Vector3 trailScale = trail.transform.localScale;
		Vector3 trailPosition = trail.transform.localPosition;

        //ֻҪ�ض��İ�ť�����£�����һֱ����ֱ��β��������������ò���
        while ( ControlInput.IsButtonPressed( note.StringIndex )
			&& Player.GetCurrentBeat() + 1 <= note.Time + note.Length )
		{
			//����β����ʵ�ʾ���
			float progress = Mathf.Clamp01( ( 1 + Player.GetCurrentBeat() - note.Time ) / note.Length );

			//����β��������λ��
			trail.transform.localScale = new Vector3( trailScale.x, trailScale.y, trailScale.z * ( 1 - progress ) );
			trail.transform.localPosition = new Vector3( trailPosition.x, trailPosition.y + trailPosition.y * progress, trailPosition.z );

            

			yield return null;
		}

		//��������û��������
		trail.GetComponent<Renderer>().enabled = false;

		//ȡ����Ч
		ControlInput.GetStringButton( note.StringIndex ).transform.Find( "Sparks" ).GetComponent<ParticleEmitter>().emit = false;
		ControlInput.GetStringButton( note.StringIndex ).transform.Find( "Sparks" ).GetComponent<ParticleEmitter>().GetComponent<Renderer>().enabled = false;
	}

	protected void HideNote( int index )
	{
		NoteObjects[ index ].GetComponent<Renderer>().enabled = false;
	}

    //�ж�Note�Ƿ񱻻���
	protected bool IsNoteHit( int index )
	{
		Note note = Player.Song.Notes[ index ];

        //���û�а�����������������ܱ�����
        if ( !ControlInput.WasButtonJustPressed( note.StringIndex ) )
		{
			return false;
		}

		//������Ѿ�������һ���ˣ������ж�Ϊ����
		if( HasHitNoteOnStringIndexThisFrame[ note.StringIndex ] )
		{
			return false;
		}

		//���Renderer״̬�仯��֮ǰ�ͱ�������
		if( NoteObjects[ index ].GetComponent<Renderer>().enabled == false )
		{
			return false;
		}

		//���Note�Ƿ��ڻ�������
		if( IsInHitZone( NoteObjects[ index ] ) )
		{
            //���ô˱�־��Ϊ�˷�ֹ����������ͬһ����ť����
            HasHitNoteOnStringIndexThisFrame[ note.StringIndex ] = true;
			return true;
		}

		//���û�ڷ�Χ�ڣ��򷵻�û������
		return false;
	}

    //�жϰ�ť�Ƿ�Miss
	protected bool WasNoteMissed( int index )
	{
		//ͨ��λ���ж�
		if( NoteObjects[ index ].transform.position.z > 0 )
		{
			return false;
		}

		//ͨ��renderer��״̬�ж�
		if( NoteObjects[ index ].GetComponent<Renderer>().enabled == false )
		{
			return false;
		}

		return true;
	}

    //������Ϸ����
	protected void ResetGameStateValues()
	{
		Score = 0;
		Streak = 0;
		MaxStreak = 0;
		Multiplier = 1;
		NumNotesMissed = 0;
		NumNotesHit = 0;
	}

    //����Note��λ��
	protected void UpdateNotePosition( int index )
	{
		Note note = Player.Song.Notes[ index ];

        //��button����6��֮����ʾ
        if ( note.Time < Player.GetCurrentBeat() + 6 )
		{
			//һ��ʼ����Note��active=false
            //ʱ����ʳ���
            //�ڽ�����ͳһ����
			if( !NoteObjects[ index ].activeSelf )
			{

				NoteObjects[ index ].SetActive( true );

				NoteObjects[ index ].GetComponent<Renderer>().enabled = true;

				//���Ϊһ����Note����ʾ����
				if( Player.Song.Notes[ index ].Length > 0f )
				{
					NoteObjects[ index ].transform.Find( "Trail" ).GetComponent<Renderer>().enabled = true;
				}
			}


			//����Note��Button�ľ���
            //��ȡ����������ӳ��ȵ�λ��
			float progress = ( note.Time - Player.GetCurrentBeat() - 0.5f ) / 6f;

			//����Note��λ��
			Vector3 position = NoteObjects[ index ].transform.position;
            //���λ��*����=����λ��
			position.z = progress * GetGuitarNeckLength();
            //����Note��λ��
			NoteObjects[ index ].transform.position = position;
		}
	}



    //����GUI
	protected void UpdateGuiScore()
	{
		//GameObject.Find( "GUI Score" ).GetComponent<GUIText>().text = Mathf.Floor( Score ).ToString();
	}

	protected void UpdateGuiMultiplier()
	{
		Multiplier = Mathf.Ceil( Streak / 10 );

		Multiplier = Mathf.Clamp( Multiplier, 1, 10 );

		GameObject.Find( "GUI Multiplier" ).GetComponent<GUIText>().text = "x" + Mathf.Floor( Multiplier ).ToString();
	}


	protected void UpdateNeckTextureOffset()
	{
		Vector2 offset = GuitarNeckObject.GetComponent<Renderer>().material.GetTextureOffset( "_MainTex" );

		offset.y = 1 - ( Player.GetCurrentBeat() - 0.5f ) / 6f;

		GuitarNeckObject.GetComponent<Renderer>().material.SetTextureOffset( "_MainTex", offset );
	}

	protected float GetNeckMoveOffset()
	{
		return Time.deltaTime * Player.Song.BeatsPerMinute * ( 1f / 6f / 60f );
	}

	protected bool IsInHitZone( GameObject note )
	{
		return note.transform.position.z < GetHitZoneBeginning()
			&& note.transform.position.z > GetHitZoneEnd();
	}

    protected bool IsPerfactZone(GameObject note)
    {
        return note.transform.position.z < GetHitPerfactBeginning()
            && note.transform.position.z > GetHitPerfactEnd();
    }

    protected bool IsGreatZone(GameObject note)
    {
        return note.transform.position.z < GetHitGreatBeginning()
            && note.transform.position.z > GetHitGreatEnd();
    }

    protected bool IsGoodZone(GameObject note)
    {
        return note.transform.position.z < GetHitGoodBeginning()
            && note.transform.position.z > GetHitGoodEnd();
    }

    //�簴Miss
    protected bool IsMissZone(GameObject note)
    {
        return note.transform.position.z < GetHitMissBeginning()
            && note.transform.position.z > GetHitMissEnd();
    }

    //��ȡ���Ӿ���ĳ���
    protected float GetGuitarNeckLength()
	{
		return 20f;
	}

    //��ȡ����Ӧ��stringindex����ɫ
	public Color GetColor( int index )
	{
		if( Colors == null || Colors.Length != 8 )
		{
			UpdateColorsArray();
		}

		return Colors[ index ];
	}

    //��ȡ����
	public float GetScore()
	{
		return Score;
	}


	public float GetMaximumStreak()
	{
		return MaxStreak;
	}

	public float GetNumNotesHit()
	{
		return NumNotesHit;
	}

	public float GetNumNotesMissed()
	{
		return NumNotesMissed;
	}

    //��ȡ�����ߵ���ʼ��
	protected Vector3 GetStartPosition( int stringIndex )
	{
		return new Vector3( (float)( stringIndex - 2 ), 0f, GetGuitarNeckLength() );
	}


    //����Ԥ��Note
	protected GameObject InstantiateNoteFromPrefab( int stringIndex )
	{
        StringNoteCount[stringIndex]++;
        GameObject note = Instantiate(NewNotePrefab[stringIndex]
                                     , GetStartPosition( stringIndex )
									 , Quaternion.identity
									 ) as GameObject;

		//note.GetComponent<Renderer>().material.color = Colors[ stringIndex ];
		note.transform.Rotate( new Vector3( -90, 0, 0 ) );

		return note;
	}

    //������Note
	protected GameObject CreateTrailObject( GameObject noteObject, Note note )
	{
		if( note.Length == 0 )
		{
			return null;
		}

		GameObject trail = GameObject.CreatePrimitive( PrimitiveType.Plane );

		
		Destroy( trail.GetComponent<MeshCollider>() );

		//�������򳤶�Ϊ20
		//β���ĳ�ʼ����Ϊ10
		//1/6�Ŀ������򳤶�Ϊһ��, ����1/3Ϊβ���ĳ���
		float scaleZ = 0.33f * note.Length;

		trail.transform.localScale = new Vector3( 0.03f, 1f, scaleZ );

		trail.transform.parent = noteObject.transform;

		//�Գ�Note�Ĵ��ӽ����ض�λ
		trail.transform.localPosition = new Vector3( 0f, -10f * scaleZ / 2f, 0.01f );

		//�Գ�Note�Ĵ��ӽ�����Ⱦ
		trail.GetComponent<Renderer>().material.color = Colors[ note.StringIndex ] * 0.2f;
		trail.GetComponent<Renderer>().material.shader = Shader.Find( "Self-Illumin/Diffuse" );
		trail.GetComponent<Renderer>().enabled = false;

		trail.name = "Trail";

		return trail;
	}

    //�жϻ�������
	protected float GetHitZoneBeginning()
	{
		return 9f;
	}

	protected float GetHitZoneEnd()
	{
	
		return 0.95f;
	}

    //Perfact
    protected float GetHitPerfactBeginning()
    {
        return 2.4f;
    }

    protected float GetHitPerfactEnd()
    {
        return 0.95f;
    }
    //Great
    protected float GetHitGreatBeginning()
    {
        return 4.3f;
    }

    protected float GetHitGreatEnd()
    {
        return 2.4f;
    }
    //Good
    protected float GetHitGoodBeginning()
    {
        return 6f;
    }

    protected float GetHitGoodEnd()
    {
        return 4.3f;
    }
    //Miss
    protected float GetHitMissBeginning()
    {
        return 9f;
    }

    protected float GetHitMissEnd()
    {
        return 6f;
    }


    
    //��������ʱ
    public void OnSongFinished()
	{
		//GameObject.Find( "GUI Score" ).GetComponent<GUIText>().enabled = false;
		//GameObject.Find( "GUI Multiplier" ).GetComponent<GUIText>().enabled = false;

		//GetComponent<EndOfSongMenu>().ShowMenu();
	}

    //����UI
	protected IEnumerator DisplayText( string text, float duration, float fade = 0f )
	{

		GameObject guiTextObject = GameObject.Find( "GUI Text" );

		guiTextObject.GetComponent<GUIText>().text = text;

		Color newColor = guiTextObject.GetComponent<GUIText>().material.color;
		newColor.a = 1;
		guiTextObject.GetComponent<GUIText>().material.color = newColor;

		yield return new WaitForSeconds( MyMath.BeatsToSeconds( duration - fade, Player.Song.BeatsPerMinute ) );

		float fadeTime = MyMath.BeatsToSeconds( fade, Player.Song.BeatsPerMinute );
		float totalFadeTime = fadeTime;

		while( fadeTime > 0 && guiTextObject.GetComponent<GUIText>().text == text )
		{
			newColor = guiTextObject.GetComponent<GUIText>().material.color;
			newColor.a = fadeTime / totalFadeTime;
			guiTextObject.GetComponent<GUIText>().material.color = newColor;

			fadeTime -= Time.deltaTime;

			yield return null;
		}

		if( guiTextObject.GetComponent<GUIText>().text == text )
		{
			guiTextObject.GetComponent<GUIText>().text = "";
		}
	}
}