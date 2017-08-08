using UnityEngine;
using System.Collections;

public class ControlInput : MonoBehaviour 
{
	//This constant is only used in this class and cannot be adjusted to work with more or less strings
    //按钮个数
	public const int NumStrings = 8;

	//判断按钮是否按住
	protected bool[] ButtonsPressed;
    
	//判断按钮数
	protected bool[] ButtonsJustPressed;

	//The five button objects in the scene
	protected GameObject[] StringButtons;

	void Start()
	{
        //初始化
		ButtonsPressed = new bool[ NumStrings ];
		ButtonsJustPressed = new bool[ NumStrings ];

		for( int i = 0; i < NumStrings; ++i )
		{
			ButtonsPressed[ i ] = false;
			ButtonsJustPressed[ i ] = false;
		}
        //寻找对应的按钮
        SaveReferencesToStringButtons();
	}

	void Update()
	{
		ResetButtonsJustPressedArray();
	}


    //寻找对应的按钮
	void SaveReferencesToStringButtons()
	{
		StringButtons = new GameObject[ NumStrings ];

		for( int i = 0; i < NumStrings; ++i )
		{
			StringButtons[ i ] = GameObject.Find( "StringButton" + ( i + 1 ) );
		}
	}

    //重置按钮状态
	protected void ResetButtonsJustPressedArray()
	{
		for( int i = 0; i < NumStrings; ++i )
		{
			ButtonsJustPressed[ i ] = false;
		}
	}

    //获取按钮按下的个数
	protected int GetNumButtonsPressed()
	{
		int pressed = 0;

		for( int i = 0; i < NumStrings; ++i )
		{
			if( ButtonsPressed[ i ] )
			{
				pressed++;
			}
		}

		return pressed;
	}

    //判断按钮是否按下
	public bool IsButtonPressed( int index )
	{
		return ButtonsPressed[ index ];
	}

    //判断按钮是否按住
	public bool WasButtonJustPressed( int index )
	{
		return ButtonsJustPressed[ index ];
	}


	public void OnStringChange( int stringIndex, bool pressed )
	{
		if( pressed == IsButtonPressed( stringIndex ) )
		{
			return;
		}

        //获取对应的按钮的位置信息
		Vector3 stringButtonPosition = StringButtons[ stringIndex ].transform.position;

		if( pressed )
		{
            //键数限制
			if( GetNumButtonsPressed() < 3)
			{
				//按钮上升动画效果
				stringButtonPosition.y = 0.16f;

				//触发特效
				//StringButtons[ stringIndex ].transform.Find( "Light" ).GetComponent<Light>().enabled = true;

				//更新键位信息
				ButtonsPressed[ stringIndex ] = true;
				ButtonsJustPressed[ stringIndex ] = true;
			}
		}
		else
		{
            //按钮下降动画效果
            stringButtonPosition.y = -0.06f;

			//不触发特效
			//StringButtons[ stringIndex ].transform.Find( "Light" ).GetComponent<Light>().enabled = false;

			//没有按下按钮
			ButtonsPressed[ stringIndex ] = false;
		}

		//给位置赋值，做到位置保持
		StringButtons[ stringIndex ].transform.position = stringButtonPosition;
	}

    //获取按钮信息
	public GameObject GetStringButton( int index )
	{
		return StringButtons[ index ];
	}
}
