using UnityEngine;
using System.Collections;

public class KeyboardControl : MonoBehaviour
{
    //按键控制
	//KeyCodes that control the five string buttons
	protected KeyCode[] StringKeys;

	ControlInput ControlInput;

	//Use this for initialization
	void Start()
	{
		ControlInput = GetComponent<ControlInput>();

		UpdateStringKeyArray();
	}

	protected void UpdateStringKeyArray()
	{
		StringKeys = new KeyCode[ ControlInput.NumStrings ];

		for( int i = 0; i < ControlInput.NumStrings; ++i )
		{
			StringKeys[ i ] = GameObject.Find( "StringButton" + ( i + 1 ) ).GetComponent<StringButton>().Key;
		}

	}

	void Update()
	{
        //每个按钮检测过去
		for( int i = 0; i < ControlInput.NumStrings; ++i )
		{
			CheckKeyCode( StringKeys[ i ], i );
		}
	}

	void CheckKeyCode( KeyCode code, int stringIndex )
	{

        if(Input.GetKeyDown(KeyCode.Space))
        {
            for (int i = 0; i < 8; i++)
            {
                ControlInput.OnStringChange(i, true);
            }
        }

        if(Input.GetKeyUp(KeyCode.Space))
        {
            for (int i = 0; i < 8; i++)
            {
                ControlInput.OnStringChange(i, false);
            }
        }

		if( Input.GetKeyDown( code ) )
		{
			ControlInput.OnStringChange( stringIndex, true );
		}
		if( Input.GetKeyUp( code ) )
		{
			ControlInput.OnStringChange( stringIndex, false );
		}
		if( Input.GetKey( code ) && ControlInput.IsButtonPressed( stringIndex ) == false )
		{
			ControlInput.OnStringChange( stringIndex, true );
		}
	}
}