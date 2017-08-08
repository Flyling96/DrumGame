using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor( typeof( SongData ) )]
public class SongDataEditor : Editor
{
	//Needed to draw 2D lines and rectangles
	private static Texture2D _coloredLineTexture;   
	private static Color _coloredLineColor;

	private GameObject GuitarObject;
	private AudioSource MetronomeSource;
	private SongPlayer SongPlayer;

	//Dimensions of the editor
	//Song View is the one with the black background, where you can add notes etc.
	//ProgressBar, or Progress View is the small preview on the right, where you can navigate through the song
	private Rect SongViewRect;
	private float SongViewProgressBarWidth = 20f;
	private float SongViewHeight = 400f;

	//Metronome Vars
	private static bool UseMetronome;
	private float LastMetronomeBeat = Mathf.NegativeInfinity;

	//Helper vars to handle mouse clicks
	private Vector2 MouseUpPosition = Vector2.zero;
	private bool LastClickWasRightMouseButton;

	//Currently selected note index
	private int SelectedNote = -1;

    //歌曲概述纹理（一个在右边，你可以用来导航）
    private Texture2D ProgressViewTexture;

	//Timer to calculate editor performance
	Timer PerformanceTimer = new Timer();

    //私人定制菜单
	[MenuItem( "Assets/Create/Song" )]
	public static void CreateNewSongAsset()
	{
		SongData asset = ScriptableObject.CreateInstance<SongData>();

        //创建的默认位置
		AssetDatabase.CreateAsset( asset, "Assets/Guitar Unity/Songs/NewSong.asset");
		
		EditorUtility.FocusProjectWindow();

        //激活物体
		Selection.activeObject = asset;
	}


    //外部调用
    public static void CreateNewSongAsset(string a)
    {
        SongData asset = ScriptableObject.CreateInstance<SongData>();

        //创建的默认位置
        AssetDatabase.CreateAsset(asset, "Assets/Guitar Unity/Songs/"+a+".asset");

        EditorUtility.FocusProjectWindow();

        //激活物体
        Selection.activeObject = asset;
    }


    protected void OnEnable()
	{
        //设置对象引用
        GuitarObject = GameObject.Find( "Guitar" );

		if( GuitarObject == null )
		{
			return;
		}

		SongPlayer = GuitarObject.GetComponent<SongPlayer>();
        //获取音乐
		MetronomeSource = GameObject.Find( "Metronome" ).GetComponent<AudioSource>();

		//初始化
		SongPlayer.SetSong( (SongData)target );

        //向上进位取整
		LastMetronomeBeat = -Mathf.Ceil( SongPlayer.Song.AudioStartBeatOffset );


		RedrawProgressViewTexture();
	}


    //绘制图形
	protected void RedrawProgressViewTexture()
	{
		int width = (int)SongViewProgressBarWidth;
		int height = (int)SongViewHeight;

		if( !ProgressViewTexture )
		{
            //如果不存在，创建空的纹理
            ProgressViewTexture = new Texture2D( width, height );

            //循环模式
            ProgressViewTexture.wrapMode = TextureWrapMode.Clamp;

            //存储设置
			ProgressViewTexture.hideFlags = HideFlags.HideAndDontSave;
		}

		//绘制背景颜色
		Color[] BackgroundColor = new Color[ width * height ];
		for( int i = 0; i < width * height; ++i )
		{
			BackgroundColor[ i ] = new Color( 0.13f, 0.1f, 0.26f );
		}

        //绘制色彩块
		ProgressViewTexture.SetPixels( 0, 0, width, height, BackgroundColor );

        //计算Note的规模，让他们都融入到进度视图中
        float totalBeats = SongPlayer.Song.GetLengthInBeats();
		float heightOfOneBeat = 1f / totalBeats * (float)height;

        //绘制所有的Note

        //Color[] a = new Color[8];
        //a[0] = Color.green; a[1] = Color.red; a[2] = Color.yellow; a[3] = Color.blue;
        //a[4] = Color.gray; a[5] = Color.white; a[6] = Color.cyan; a[7] = Color.magenta;
        for ( int i = 0; i < SongPlayer.Song.Notes.Count; ++i )
		{
			
			int stringIndex = SongPlayer.Song.Notes[ i ].StringIndex;

            Color color = GuitarObject.GetComponent<GuitarGameplay>().GetColor(stringIndex);
          


            int y = (int)Mathf.Round( ( ( SongPlayer.Song.Notes[ i ].Time + SongPlayer.Song.AudioStartBeatOffset - 1 ) / totalBeats ) * height );
			int x = -4+(int)( width / 2 ) + ( stringIndex - 2 ) * 3;

			//绘制尾带
			float length = SongPlayer.Song.Notes[ i ].Length;

            //3x3像素绘制矩形
            for ( int j = -1; j < 1; ++j )
			{
				for( int k = -1; k < 1; ++k )
				{
					ProgressViewTexture.SetPixel( x + j, y + k, color );
				}
			}

			//绘制尾带
			if( length > 0 )
			{
				for( int lengthY = y; lengthY < y + length * heightOfOneBeat; ++lengthY )
				{
					ProgressViewTexture.SetPixel( x, lengthY, color );
				}
			}
		}
			
		ProgressViewTexture.Apply();
	}

	public override void OnInspectorGUI()
	{
		DrawInspector();

		//判断鼠标点击
		if( Event.current.isMouse )
		{
			if( Event.current.type == EventType.mouseDown )
			{
				OnMouseDown( Event.current );
			}
			else if( Event.current.type == EventType.mouseUp )
			{
				OnMouseUp( Event.current );
			}
		}

		//判断键盘敲击
		if( Event.current.isKey )
		{
			if( Event.current.type == EventType.keyDown )
			{
				OnKeyDown( Event.current );
			}
		}

		if( Event.current.type == EventType.ValidateCommand )
		{
			switch( Event.current.commandName )
			{
				case "UndoRedoPerformed":
					RedrawProgressViewTexture();
					break;
			}
		}

        //对曲谱进行更新
		if( GUI.changed )
		{
			SongData targetData = target as SongData;
			if( targetData.BackgroundTrack != null && SongPlayer.Song != targetData )
			{
				SongPlayer.SetSong( targetData );
			}
		}

		UpdateMetronome();
		RepaintGui();
	}

    //键盘控制
	protected void OnKeyDown( Event e )
	{
		switch( e.keyCode )
		{
			case KeyCode.UpArrow:
				//Up arrow advances the song by one beat
				GuitarObject.GetComponent<AudioSource>().time += MyMath.BeatsToSeconds( 1f, SongPlayer.Song.BeatsPerMinute );
				e.Use();
				break;
			case KeyCode.DownArrow:
				//Down arrow rewinds the song by one beat
				if( GuitarObject.GetComponent<AudioSource>().time >= MyMath.BeatsToSeconds( 1f, SongPlayer.Song.BeatsPerMinute ) )
				{
					GuitarObject.GetComponent<AudioSource>().time -= MyMath.BeatsToSeconds( 1f, SongPlayer.Song.BeatsPerMinute );
				}
				else
				{
					GuitarObject.GetComponent<AudioSource>().time = 0;
				}

				e.Use();
				break;
			case KeyCode.RightControl:
				//右Ctrl控制暂停开始
				OnPlayPauseClicked();
				e.Use();
				break;
			case KeyCode.LeftArrow:
				//Left arrow selects the previous note
				if( SelectedNote != -1 && SelectedNote > 0 )
				{
					SelectedNote--;
					Repaint();
				}
				break;
			case KeyCode.RightArrow:
				//Right arrow selects the next note
				if( SelectedNote != -1 && SelectedNote < SongPlayer.Song.Notes.Count )
				{
					SelectedNote++;
					Repaint();
				}
				break;
			case KeyCode.Delete:
				//DEL removes the currently selected note
				if( SelectedNote != -1 )
				{
					Undo.RegisterUndo( SongPlayer.Song, "Remove Note" );

					SongPlayer.Song.RemoveNote( SelectedNote );
					SelectedNote = -1;
					EditorUtility.SetDirty( target );
					RedrawProgressViewTexture();

					Repaint();
				}
				break;
			case KeyCode.Alpha1:
				AddNewNoteAtCurrentTime( 0 );
				break;
			case KeyCode.Alpha2:
				AddNewNoteAtCurrentTime( 1 );
				break;
			case KeyCode.Alpha3:
				AddNewNoteAtCurrentTime( 2 );
				break;
			case KeyCode.Alpha4:
				AddNewNoteAtCurrentTime( 3 );
				break;
			case KeyCode.Alpha5:
				AddNewNoteAtCurrentTime( 4 );
                break;
            case KeyCode.Alpha6:
                AddNewNoteAtCurrentTime(5);
                break;
            case KeyCode.Alpha7:
                AddNewNoteAtCurrentTime(6);
                break;
            case KeyCode.Alpha8:
                AddNewNoteAtCurrentTime(7);
                break;
        }
	}

    //添加新的Note
	void AddNewNoteAtCurrentTime( int stringIndex )
	{
        //四舍五入进行正确节拍的调整
		float currentBeat = Mathf.Round( ( SongPlayer.GetCurrentBeat( true ) + 1 ) * 4 ) / 4;

        //获取最后一个Note
		Note note = SongPlayer.Song.Notes.Find( item => item.Time == currentBeat && item.StringIndex == stringIndex );


		if( note == null )
		{
            //添加Note
			SongPlayer.Song.AddNote( currentBeat, stringIndex );
		}
		else
		{
			Debug.Log( "There is already a note at " + currentBeat + " on string " + stringIndex );
		}
	}

    //绘制警告窗口
	protected GUIStyle GetWarningBoxStyle()
	{
		GUIStyle box = new GUIStyle( "box" );

		box.normal.textColor = EditorStyles.miniLabel.normal.textColor;
		box.imagePosition = ImagePosition.ImageLeft;
		box.stretchWidth = true;
		box.alignment = TextAnchor.UpperLeft;

		return box;
	}

	protected void WarningBox( string text, string tooltip = "" )
	{
		GUIStyle box = GetWarningBoxStyle();

		Texture2D warningIcon = (Texture2D)Resources.Load( "Warning", typeof( Texture2D ) );
		GUIContent content = new GUIContent( " " + text, warningIcon, tooltip );
		GUILayout.Label( content, box );
	}

	protected void DrawInspector()
	{
		if( GuitarObject == null )
		{
			WarningBox( "Guitar Object could not be found." );
			WarningBox( "Did you load the GuitarUnity scene?" );
			return;
		}

		//Time the performance of the editor window
		PerformanceTimer.Clear();
		PerformanceTimer.StartTimer( "Draw Inspector" );

		GUILayout.Label( "Song Data", EditorStyles.boldLabel );

		DrawDefaultInspector();

		if( SongPlayer.Song.BackgroundTrack == null )
		{
			WarningBox( "Please set a background track!" );
			return;
		}

		if( SongPlayer.Song.BeatsPerMinute == 0 )
		{
			WarningBox( "Please set the beats per minute!" );
		}

		if( SelectedNote >= SongPlayer.Song.Notes.Count )
		{
			SelectedNote = -1;
		}

		if( SelectedNote == -1 )
		{
			//如果没有Note可选择，还是一片灰色

			GUI.enabled = false;
			GUILayout.Label( "No Note selected", EditorStyles.boldLabel );

			EditorGUILayout.FloatField( "Time", 0 );
			EditorGUILayout.IntField( "String", 0 );
			EditorGUILayout.FloatField( "Length", 0 );

			EditorGUILayout.BeginHorizontal();
				GUILayout.Space( 15 );
				GUILayout.Button( "Remove Note" );
			EditorGUILayout.EndHorizontal();

			GUI.enabled = true;
		}
		else
		{
			//绘制Note信息以及下个Note和上个Note的图案
			EditorGUILayout.BeginHorizontal();

				GUILayout.Label( "Note " + SelectedNote.ToString(), EditorStyles.boldLabel );
				
				if( SelectedNote == 0 )
				{
					GUI.enabled = false;
				}
				if( GUILayout.Button( "<" ) )
				{
					SelectedNote--;
				}
				GUI.enabled = true;

				if( SelectedNote == SongPlayer.Song.Notes.Count - 1 )
				{
					GUI.enabled = false;
				}
				if( GUILayout.Button( ">" ) )
				{
					SelectedNote++;
				}
				GUI.enabled = true;

			EditorGUILayout.EndHorizontal();

			//Draw note data
			float newTime = EditorGUILayout.FloatField( "Time", SongPlayer.Song.Notes[ SelectedNote ].Time );
			int newStringIndex = EditorGUILayout.IntField( "String", SongPlayer.Song.Notes[ SelectedNote ].StringIndex );
			float newLength = EditorGUILayout.FloatField( "Length", SongPlayer.Song.Notes[ SelectedNote ].Length );

            //限制范围
			newStringIndex = Mathf.Clamp( newStringIndex, 0, 7 );
            //Debug.Log(newStringIndex);
            //如果Note信息变化则对界面进行更新
            if ( newTime != SongPlayer.Song.Notes[ SelectedNote ].Time
				|| newStringIndex != SongPlayer.Song.Notes[ SelectedNote ].StringIndex
				|| newLength != SongPlayer.Song.Notes[ SelectedNote ].Length )
			{
				Undo.RegisterUndo( SongPlayer.Song, "Edit Note" );

				SongPlayer.Song.Notes[ SelectedNote ].Time = newTime;
				SongPlayer.Song.Notes[ SelectedNote ].StringIndex = newStringIndex;
				SongPlayer.Song.Notes[ SelectedNote ].Length = newLength;

				RedrawProgressViewTexture();

				Repaint();
			}

			//Remove Note Button
			//15px Space is added to the front to match the default unity style
			EditorGUILayout.BeginHorizontal();
            //空距为15
			GUILayout.Space( 15 );
			if( GUILayout.Button( "Remove Note" ) )
			{
				Undo.RegisterUndo( SongPlayer.Song, "Remove Note" );

				SongPlayer.Song.RemoveNote( SelectedNote );
				SelectedNote = -1;
				RedrawProgressViewTexture();
				EditorUtility.SetDirty( target );

				Repaint();
			}
			EditorGUILayout.EndHorizontal();
		}
			
		GUILayout.Label( "Song", EditorStyles.boldLabel );

		//Draw song player controls
		EditorGUILayout.BeginHorizontal();
			GUILayout.Space( 15 );

			string buttonLabel = "Play Song";
			if( IsPlaying() )
			{
				buttonLabel = "Pause Song";
			}

			if( GUILayout.Button( buttonLabel ) )
			{
				OnPlayPauseClicked();
			}
			if( GUILayout.Button( "Stop Song" ) )
			{
				OnStopClicked();
			}
		EditorGUILayout.EndHorizontal();

		//添加速度选项
		EditorGUILayout.BeginHorizontal();
			GUILayout.Space( 15 );
			GUILayout.Label( "Playback Speed", EditorStyles.label );
			GuitarObject.GetComponent<AudioSource>().pitch = GUILayout.HorizontalSlider( GuitarObject.GetComponent<AudioSource>().pitch, 0, 1 );
		EditorGUILayout.EndHorizontal();

        //绘制节拍器切换
        UseMetronome = EditorGUILayout.Toggle( "Metronome", UseMetronome );

		//绘制曲谱编辑器
		SongViewRect = GUILayoutUtility.GetRect( GUILayoutUtility.GetLastRect().width, SongViewHeight );

		//PerformanceTimer.StartTimer( "Draw Background" );
		DrawRectangle( 0, SongViewRect.yMin, SongViewRect.width*0.95f, SongViewRect.height, Color.black );

		//PerformanceTimer.StartTimer( "Draw Progress View" );
		DrawProgressView();

		//PerformanceTimer.StartTimer( "Draw Main View" );
		DrawMainView();

		PerformanceTimer.StopTimer();

		DrawAddNotesHint();
		DrawEditorPerformancePanel();
	}

	protected void DrawAddNotesHint()
	{
		WarningBox( "Use number keys 1-5 to add notes while playing" );
	}

	protected void DrawEditorPerformancePanel()
	{
		List<TimerData> Timers = PerformanceTimer.GetTimers();

		GUILayout.Label( "Editor Performance", EditorStyles.boldLabel );

		for( int i = 0; i < Timers.Count; ++i )
		{
			float displayMs = Mathf.Round( Timers[ i ].Time * 10000 ) / 10;
			GUILayout.Label( Timers[ i ].Name + " " + displayMs + "ms" );
		}

		float deltaTime = PerformanceTimer.GetTotalTime();
		float fps = Mathf.Round( 10 / deltaTime ) / 10;
		float msPerFrame = Mathf.Round( deltaTime * 10000 ) / 10;
		GUILayout.Label( "Total " + msPerFrame + "ms/frame (" + fps + "FPS)" );
	}

	protected void UpdateMetronome()
	{
		if( !UseMetronome )
		{
			return;
		}

		if( !IsPlaying() )
		{
			return;
		}

		float currentWholeBeat = Mathf.Floor( SongPlayer.GetCurrentBeat( true ) + 0.05f );
		if( currentWholeBeat > LastMetronomeBeat )
		{
			LastMetronomeBeat = currentWholeBeat;

			MetronomeSource.Stop();
			MetronomeSource.time = 0f;

			MetronomeSource.Play();
			MetronomeSource.Pause();
			MetronomeSource.Play();
		}
	}

	protected void RepaintGui()
	{
		if( IsPlaying() )
		{
			Repaint();
		}
	}

	protected Rect GetProgressViewRect()
	{
		return new Rect( SongViewRect.width - SongViewProgressBarWidth, SongViewRect.yMin, SongViewProgressBarWidth*1.6f, SongViewRect.height );
	}

	protected bool IsPlaying()
	{
		if( GuitarObject == null )
		{
			return false;
		}

		return GuitarObject.GetComponent<AudioSource>().isPlaying;
	}

    //绘制界面
	protected void DrawMainView()
	{
		float totalWidth = SongViewRect.width - SongViewProgressBarWidth;

		if( totalWidth < 0 )
		{
			return;
		}
        //绘制节拍
		DrawBeats();
        //绘制线
		DrawStrings();
        //绘制进度条
		DrawTimeMarker();
        //绘制grid
		DrawGridNotesAndHandleMouseClicks();
        //绘制Note
		DrawNotes();
	}

	protected void DrawTimeMarker()
	{
		float heightOfOneBeat = SongViewRect.height / 6f;

		DrawLine( new Vector2( SongViewRect.xMin, SongViewRect.yMax - heightOfOneBeat )
				, new Vector2( SongViewRect.xMax - SongViewProgressBarWidth, SongViewRect.yMax - heightOfOneBeat )
				, new Color( 1f, 0f, 0f )
				, 4 );
	}

	protected void DrawStrings()
	{
		float totalWidth = SongViewRect.width - SongViewProgressBarWidth;
		float stringDistance = totalWidth / 6;

		for( int i = 0; i < 8; ++i )
		{
			float x = stringDistance * ( i + 1 );
			DrawVerticalLine( new Vector2( x/1.6f, SongViewRect.yMin )
							, new Vector2( x / 1.6f, SongViewRect.yMax )
							, new Color( 0.4f, 0.4f, 0.4f )
							, 3 );
		}
	}

    //绘制Note
	protected void DrawNotes()
	{
		//Calculate positioning variables
		float heightOfOneBeat = SongViewRect.height / 6f;
		float totalWidth = SongViewRect.width - SongViewProgressBarWidth;
		float stringDistance = totalWidth / 6;
		
		Note note;

		for( int i = 0; i < SongPlayer.Song.Notes.Count; ++i )
		{
			note = SongPlayer.Song.Notes[ i ];

			if( note.Time > SongPlayer.GetCurrentBeat( true ) + 6.5f )
			{
                //如果Note是不可见的，跳过它，并提请下一个Note
                continue;
			}

			if( note.Time + note.Length < SongPlayer.GetCurrentBeat( true ) - 0.5f )
			{
                //如果Note是不可见的，跳过它，并提请下一个Note
                continue;
			}

			//Note走了多远
			float progressOnNeck = 1 - ( note.Time - SongPlayer.GetCurrentBeat( true ) ) / 6f;

			//获取Note的颜色
			Color color = GuitarObject.GetComponent<GuitarGameplay>().GetColor( note.StringIndex );

			//获取Note的位置
			float y = SongViewRect.yMin + progressOnNeck * SongViewRect.height;
            //对x进行调整
			float x = (SongViewRect.xMin + ( note.StringIndex + 1 ) * stringDistance)/1.6f;

            //如果选中Note，请在它周围画一个白色矩形
            if ( SelectedNote == i )
			{
				DrawRectangle( x - 9, y - 9, 17, 17, new Color( 1f, 1f, 1f ), SongViewRect );
			}

		
			DrawRectangle( x - 7, y - 7, 13, 13, color, SongViewRect );

			//绘制尾带
			if( note.Length > 0 )
			{
				float trailYTop = y - note.Length * heightOfOneBeat;
				float trailYBot = y;

				DrawVerticalLine( new Vector2( x, trailYBot ), new Vector2( x, trailYTop ), color, 7, SongViewRect );
			}
		}
	}

    //鼠标点击选中
	protected void DrawGridNotesAndHandleMouseClicks()
	{
		
		if( IsPlaying() )
		{
			return;
		}

		float heightOfOneBeat = SongViewRect.height / 6f;
		float totalWidth = SongViewRect.width - SongViewProgressBarWidth;
		float stringDistance = totalWidth / 6;
		float numNotesPerBeat = 4f;

        //计算偏移量（从0到1）到当前的节拍有多远
        float beatOffset = SongPlayer.GetCurrentBeat( true );
		beatOffset -= (int)beatOffset;

		//获取灰色圆圈
		Texture2D GridNoteTexture = (Texture2D)UnityEngine.Resources.Load( "GridNote", typeof( Texture2D ) );

		//绘制线
		for( int i = 0; i < 8; ++i )
		{
			float x = stringDistance * ( i + 1 )/1.6f;

            //歌曲一般在七分钟内
			for( int j = 0; j < 7 * numNotesPerBeat; ++j )
			{
				float y = SongViewRect.yMax - ( j / numNotesPerBeat - beatOffset ) * heightOfOneBeat;

                //计算这个gird位置的节拍值
                float beat = (float)j / numNotesPerBeat + Mathf.Ceil( SongPlayer.GetCurrentBeat( true ) );

                //gird的范围
				Rect rect = new Rect( x - 7, y - 7, 13, 13 );

				if( beat > SongPlayer.Song.GetLengthInBeats() )
				{
					//超过长度不绘制
					continue;
				}

				if( rect.yMin < SongViewRect.yMin && rect.yMax < SongViewRect.yMin )
				{
                    //没有正确的空闲位置不绘制
                    continue;
				}

				if( rect.yMin > SongViewRect.yMax && rect.yMax > SongViewRect.yMax )
				{
					//没有正确的空闲位置不绘制
					continue;
				}

                //将矩形绘制到歌曲视图中
                rect.yMin = Mathf.Clamp( rect.yMin, SongViewRect.yMin, SongViewRect.yMax );
				rect.yMax = Mathf.Clamp( rect.yMax, SongViewRect.yMin, SongViewRect.yMax );
				
				GUI.DrawTexture( rect, GridNoteTexture, ScaleMode.ScaleAndCrop, true );

                //正确的鼠标偏移
                y -= heightOfOneBeat;

				//点击鼠标当grid和Note想匹配
				//在一个范围内点击算选中
				if( rect.Contains( MouseUpPosition ) )
				{
					//Correct beat offset in positive space
					if( SongPlayer.GetCurrentBeat( true ) > 0 )
					{
						beat -= 1;
					}

					//检查Note是不是已经存在
					SelectedNote = SongPlayer.Song.GetNoteIndex( beat, i );

                    //若Note不存在
					if( SelectedNote == -1 )
					{

                        // 点击左键
                        if ( LastClickWasRightMouseButton == false )
						{
							Undo.RegisterUndo( SongPlayer.Song, "Add Note" );

							SelectedNote = SongPlayer.Song.AddNote( beat, i );
							EditorUtility.SetDirty( target );
							RedrawProgressViewTexture();
						}
					}
					else
					{
						//如果Note不存在点击右键删除
						if( LastClickWasRightMouseButton )
						{
							Undo.RegisterUndo( SongPlayer.Song, "Remove Note" );

							SongPlayer.Song.RemoveNote( SelectedNote );
							SelectedNote = -1;
							EditorUtility.SetDirty( target );
							RedrawProgressViewTexture();
						}
					}

                    //重新绘制
					Repaint();
				}
			}
		}

		//重置Mouse的值
		MouseUpPosition = new Vector2( -1337, -1337 );
		LastClickWasRightMouseButton = false;
	}

	protected void DrawBeats()
	{
		float heightOfOneBeat = SongViewRect.height / 6f;

        //计算偏移量（从0到1）到当前的节拍有多远
        float beatOffset = SongPlayer.GetCurrentBeat( true );
		beatOffset -= (int)beatOffset;

		for( int i = 0; i < 7; ++i )
		{
			float y = SongViewRect.yMax - ( i - beatOffset ) * heightOfOneBeat;
			DrawLine( new Vector2( SongViewRect.xMin*1.6f, y )
					, new Vector2( SongViewRect.xMax - SongViewProgressBarWidth, y )
					, new Color( 0.1f, 0.1f, 0.1f )
					, 2, SongViewRect );
		}
	}

	protected void DrawProgressView()
	{
		GUI.DrawTexture( GetProgressViewRect(), ProgressViewTexture );
		DrawProgressViewTimeMarker();
	}

	protected void DrawProgressViewBackground()
	{
		Rect rect  = GetProgressViewRect();
		DrawRectangle( rect.xMin, rect.yMin, rect.width, rect.height, new Color( 0.13f, 0.1f, 0.26f ) );
	}

    //绘制进度条
	protected void DrawProgressViewTimeMarker()
	{
		Rect rect  = GetProgressViewRect();

		float previewProgress = 0f;
		if( GuitarObject && GuitarObject.GetComponent<AudioSource>().clip )
		{
			previewProgress = GuitarObject.GetComponent<AudioSource>().time / GuitarObject.GetComponent<AudioSource>().clip.length;
		}

		float previewProgressTop = rect.yMin + rect.height * ( 1 - previewProgress );
        //绘制进度线
		DrawLine( new Vector2( rect.xMin, previewProgressTop ), new Vector2( rect.xMax + rect.width, previewProgressTop ), Color.red, 2 );
	}

	protected void OnMouseDown( Event e )
	{
		if( GetProgressViewRect().Contains( e.mousePosition ) )
		{
            //进度条
			OnProgressViewClicked( e.mousePosition );
		}
	}

	protected void OnMouseUp( Event e )
	{
		if( SongViewRect.Contains( e.mousePosition ) && !GetProgressViewRect().Contains( e.mousePosition ) )
		{
            //歌曲视图
			OnSongViewMouseUp( e.mousePosition );

			if( e.button == 1 )
			{
				LastClickWasRightMouseButton = true;
			}
		}
	}

	protected void OnSongViewMouseUp( Vector2 mousePosition )
	{
		MouseUpPosition = mousePosition;
      
		Repaint();
	}

	protected void OnProgressViewClicked( Vector2 mousePosition )
	{
		float progress = 1 - (float)( mousePosition.y - SongViewRect.yMin ) / SongViewRect.height;

		GuitarObject.GetComponent<AudioSource>().time = GuitarObject.GetComponent<AudioSource>().clip.length * progress;
	}

	protected void OnPlayPauseClicked()
	{
		EditorGUIUtility.keyboardControl = 0;

		if( IsPlaying() )
		{
			GuitarObject.GetComponent<AudioSource>().Pause();
			EditorUtility.SetDirty( target );
		}
		else
		{
			GuitarObject.GetComponent<AudioSource>().Play();
			GuitarObject.GetComponent<AudioSource>().Pause();
			GuitarObject.GetComponent<AudioSource>().Play();
		}
	}

	protected void OnStopClicked()
	{
		if( !GuitarObject )
		{
			return;
		}

		GuitarObject.GetComponent<AudioSource>().Stop();
		GuitarObject.GetComponent<AudioSource>().time = 0f;
		LastMetronomeBeat = -Mathf.Ceil( SongPlayer.Song.AudioStartBeatOffset );
		EditorUtility.SetDirty( target );
	}

	//2D Draw Functions
	//Found on the unity forums: http://forum.unity3d.com/threads/17066-How-to-draw-a-GUI-2D-quot-line-quot/page2
	//Added clipping rectangle
	public static void DrawLine( Vector2 lineStart, Vector2 lineEnd, Color color, int thickness, Rect clip )
	{
		if( ( lineStart.y < clip.yMin && lineEnd.y < clip.yMin )
		 || ( lineStart.y > clip.yMax && lineEnd.y > clip.yMax )
		 || ( lineStart.x < clip.xMin && lineEnd.x < clip.xMin )
		 || ( lineStart.x > clip.xMax && lineEnd.x > clip.xMax ) )
		{
			return;
		}

		lineStart.x = Mathf.Clamp( lineStart.x, clip.xMin, clip.xMax );
		lineStart.y = Mathf.Clamp( lineStart.y, clip.yMin, clip.yMax );

		lineEnd.x = Mathf.Clamp( lineEnd.x, clip.xMin, clip.xMax );
		lineEnd.y = Mathf.Clamp( lineEnd.y, clip.yMin, clip.yMax );

		DrawLine( lineStart, lineEnd, color, thickness );
	}

	public static void DrawLine( Vector2 lineStart, Vector2 lineEnd, Color color, int thickness )
	{
		if( lineStart.x == lineStart.y )
		{
			DrawVerticalLine( lineStart, lineEnd, color, thickness );
		}

		if( !_coloredLineTexture )
		{
			_coloredLineTexture = new Texture2D( 1, 1 );
			_coloredLineTexture.wrapMode = TextureWrapMode.Repeat;
			_coloredLineTexture.hideFlags = HideFlags.HideAndDontSave;
		}

		if( _coloredLineColor != color )
		{
			_coloredLineColor = color;
            //设置像素颜色
			_coloredLineTexture.SetPixel( 0, 0, _coloredLineColor );
			_coloredLineTexture.Apply();
		}
		DrawLineStretched( lineStart, lineEnd, _coloredLineTexture, thickness );
	}

	public static void DrawVerticalLine( Vector2 lineStart, Vector2 lineEnd, Color color, int thickness, Rect clip )
	{
		if( ( lineStart.y < clip.yMin && lineEnd.y < clip.yMin )
		 || ( lineStart.y > clip.yMax && lineEnd.y > clip.yMax )
		 || ( lineStart.x < clip.xMin && lineEnd.x < clip.xMin )
		 || ( lineStart.x > clip.xMax && lineEnd.x > clip.xMax ) )
		{
			return;
		}

		lineStart.x = Mathf.Clamp( lineStart.x, clip.xMin, clip.xMax );
		lineStart.y = Mathf.Clamp( lineStart.y, clip.yMin, clip.yMax );

		lineEnd.x = Mathf.Clamp( lineEnd.x, clip.xMin, clip.xMax );
		lineEnd.y = Mathf.Clamp( lineEnd.y, clip.yMin, clip.yMax );

		DrawVerticalLine( lineStart, lineEnd, color, thickness );
	}

	public static void DrawVerticalLine( Vector2 lineStart, Vector2 lineEnd, Color color, int thickness )
	{
		if( lineStart.x != lineEnd.x )
		{
			DrawLine( lineStart, lineEnd, color, thickness );
			return;
		}

		float x = lineStart.x;
		float xOffset = (float)thickness;
		float y = lineStart.y + ( lineEnd.y - lineStart.y ) / 2;
		int newThickness = (int)( Mathf.Abs( Mathf.Floor( lineStart.y - lineEnd.y ) ) );

		DrawLine( new Vector2( x - xOffset / 2, y ), new Vector2( x + xOffset / 2, y ), color, newThickness );
	}

	public static void DrawLineStretched( Vector2 lineStart, Vector2 lineEnd, Texture2D texture, int thickness )
	{
		Vector2 lineVector = lineEnd - lineStart;

		if( lineVector.x == 0 )
		{
			return;
		}

		float angle = Mathf.Rad2Deg * Mathf.Atan( lineVector.y / lineVector.x );

		if( lineVector.x < 0 )
		{
			angle += 180;
		}

		if( thickness < 1 )
		{
			thickness = 1;
		}

		// The center of the line will always be at the center
		// regardless of the thickness.
		int thicknessOffset = (int)Mathf.Ceil( thickness / 2 );

		GUIUtility.RotateAroundPivot( angle, lineStart );

		GUI.DrawTexture( new Rect( lineStart.x,
								 lineStart.y - thicknessOffset,
								 lineVector.magnitude,
								 thickness ),
						texture );

		GUIUtility.RotateAroundPivot( -angle, lineStart );
	}

	private void DrawRectangle( float left, float top, float width, float height, Color color )
	{
		DrawRectangle( new Rect( left, top, width, height ), color );
	}

	private void DrawRectangle( float left, float top, float width, float height, Color color, Rect clip )
	{
		DrawRectangle( new Rect( left, top, width, height ), color, clip );
	}

	private void DrawRectangle( Rect rect, Color color, Rect clip )
	{
		DrawVerticalLine( new Vector2( rect.xMin + rect.width / 2, rect.yMin ), new Vector2( rect.xMin + rect.width / 2, rect.yMax ), color, (int)rect.width, clip );
	}

	private void DrawRectangle( Rect rect, Color color )
	{
		DrawRectangle( rect, color, rect );
	}
}