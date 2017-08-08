using UnityEngine;
using System.Collections;

public class ControlInput : MonoBehaviour 
{
	//This constant is only used in this class and cannot be adjusted to work with more or less strings
    //��ť����
	public const int NumStrings = 8;

	//�жϰ�ť�Ƿ�ס
	protected bool[] ButtonsPressed;
    
	//�жϰ�ť��
	protected bool[] ButtonsJustPressed;

	//The five button objects in the scene
	protected GameObject[] StringButtons;

	void Start()
	{
        //��ʼ��
		ButtonsPressed = new bool[ NumStrings ];
		ButtonsJustPressed = new bool[ NumStrings ];

		for( int i = 0; i < NumStrings; ++i )
		{
			ButtonsPressed[ i ] = false;
			ButtonsJustPressed[ i ] = false;
		}
        //Ѱ�Ҷ�Ӧ�İ�ť
        SaveReferencesToStringButtons();
	}

	void Update()
	{
		ResetButtonsJustPressedArray();
	}


    //Ѱ�Ҷ�Ӧ�İ�ť
	void SaveReferencesToStringButtons()
	{
		StringButtons = new GameObject[ NumStrings ];

		for( int i = 0; i < NumStrings; ++i )
		{
			StringButtons[ i ] = GameObject.Find( "StringButton" + ( i + 1 ) );
		}
	}

    //���ð�ť״̬
	protected void ResetButtonsJustPressedArray()
	{
		for( int i = 0; i < NumStrings; ++i )
		{
			ButtonsJustPressed[ i ] = false;
		}
	}

    //��ȡ��ť���µĸ���
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

    //�жϰ�ť�Ƿ���
	public bool IsButtonPressed( int index )
	{
		return ButtonsPressed[ index ];
	}

    //�жϰ�ť�Ƿ�ס
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

        //��ȡ��Ӧ�İ�ť��λ����Ϣ
		Vector3 stringButtonPosition = StringButtons[ stringIndex ].transform.position;

		if( pressed )
		{
            //��������
			if( GetNumButtonsPressed() < 3)
			{
				//��ť��������Ч��
				stringButtonPosition.y = 0.16f;

				//������Ч
				//StringButtons[ stringIndex ].transform.Find( "Light" ).GetComponent<Light>().enabled = true;

				//���¼�λ��Ϣ
				ButtonsPressed[ stringIndex ] = true;
				ButtonsJustPressed[ stringIndex ] = true;
			}
		}
		else
		{
            //��ť�½�����Ч��
            stringButtonPosition.y = -0.06f;

			//��������Ч
			//StringButtons[ stringIndex ].transform.Find( "Light" ).GetComponent<Light>().enabled = false;

			//û�а��°�ť
			ButtonsPressed[ stringIndex ] = false;
		}

		//��λ�ø�ֵ������λ�ñ���
		StringButtons[ stringIndex ].transform.position = stringButtonPosition;
	}

    //��ȡ��ť��Ϣ
	public GameObject GetStringButton( int index )
	{
		return StringButtons[ index ];
	}
}
