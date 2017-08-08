using UnityEngine;
using System.Collections.Generic;
using System.Collections;

//These were used when I stored the song data in Xml files
//I left them here if someone wants to switch
//See unused code at the bottom

/*using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Text;*/

//
[System.Serializable]
public class Note
{
    //���ֵ�����
	public float Time;
    //��һ��
	public int StringIndex;
    //����
	public float Length = 0;
}

//�̳и��������ڴ洢�������ڹ��õ�����
public class SongData : ScriptableObject
{
    //��������
	public string Name;
	public string Band;

    //ÿ���ӵĽ�����
	public float BeatsPerMinute;
    //��Ƶ��ʼ��ƫ��
    public float AudioStartBeatOffset;

    //���ŵ�����
	public AudioClip BackgroundTrack;

    //Note���б�
	[HideInInspector]
	public List<Note> Notes = new List<Note>();

	public SongData()
	{

	}


    //�ж�Note��ʱ����Ϣ
	public int GetNoteIndex( float time, int stringIndex )
	{
		for( int i = 0; i < Notes.Count; ++i )
		{
			if( Notes[ i ].Time < time )
			{
				continue;
			}

            //��ͷ��ʼ���ң�������ͬ�ķ���i
			if( Notes[ i ].Time == time && Notes[ i ].StringIndex == stringIndex )
			{
				return i;
			}
            //�����ڣ��򲻴��ڣ�����-1
			if( Notes[ i ].Time > time )
			{
				return -1;
			}
		}
		return -1;
	}

    //�Ƴ�Note
	public void RemoveNote( int index )
	{
		if( index >= 0 && index < Notes.Count )
		{
			Notes.Remove( Notes[ index ] );
		}
	}

	public void RemoveNote( float time, int stringIndex )
	{
		int index = GetNoteIndex( time, stringIndex );

		if( index != -1 )
		{
			RemoveNote( index );
		}
	}

    //���Note
	public int AddNote( float time, int stringIndex, float length = 0f )
	{
		if( time > GetLengthInBeats() )
		{
			return -1;
		}
        
		Note newNote = new Note();

		newNote.Time = time;
		newNote.StringIndex = stringIndex;
		newNote.Length = length;

		//Find correct position in the list so that the list remains ordered
		for( var i = 0; i < Notes.Count; ++i )
		{
			if( Notes[ i ].Time > time )
			{
				Notes.Insert( i, newNote );
                //Debug.Log(Notes[i].StringIndex);
				return i;
			}
		}

		//If note wasn't inserted in the list, it will be added at the end
		Notes.Add( newNote );
		return Notes.Count - 1;
	}

    
	public float GetLengthInSeconds()
	{
		if( BackgroundTrack )
		{
			return BackgroundTrack.length;
		}

		return 0;
	}

    //���һ�������ǵڼ���
	public float GetLengthInBeats()
	{
		return GetLengthInSeconds() * BeatsPerMinute / 60;
	}


	/* Unused code since I switched to a custom asset format
	 * But maybe useful if somebody wants to store song data in an XML File
	 * 
	 * public void SortNotesList()
	{
		for( int j = 0; j < Notes.Count; ++j )
		{
			for( int i = 0; i < Notes.Count - 1; ++i )
			{
				if( Notes[ i ].Time > Notes[ i + 1 ].Time )
				{
					Note temp = Notes[ i ];
					Notes[ i ] = Notes[ i + 1 ];
					Notes[ i + 1 ] = temp;
				}
			}
		}
	}

	public void WriteToXml( string filename )
	{
		XmlWriterSettings ws = new XmlWriterSettings();
		ws.NewLineHandling = NewLineHandling.Entitize;
		ws.Encoding = Encoding.UTF8;
		ws.Indent = true;

		XmlSerializer xs = new XmlSerializer( typeof( SongData ) );
		XmlWriter xmlTextWriter = XmlWriter.Create( filename, ws );

		xs.Serialize( xmlTextWriter, this );
		xmlTextWriter.Close();
	}

	protected void LoadFromXml( string xmlData )
	{
		StringReader reader = new StringReader( xmlData );
		
		XmlSerializer xdsg = new XmlSerializer( typeof( SongData ) );

		SongData newSong = (SongData)xdsg.Deserialize( reader );
		reader.Close();

		Name = newSong.Name;
		Band = newSong.Band;

		Bpm = newSong.Bpm;
		StartBeatOffset = newSong.StartBeatOffset;

		Notes = newSong.Notes;
	}*/
}