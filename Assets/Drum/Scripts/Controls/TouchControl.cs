using UnityEngine;
using System.Collections;

public class TouchControl : MonoBehaviour
{
    //´¥Ãþ¿ØÖÆ
#if !UNITY_EDITOR && (UNITY_IPHONE || UNITY_ANDROID)
	ControlInput ControlInput;
	bool[] StringChanges = new bool[ ControlInput.NumStrings ];

	//Use this for initialization
	void Start()
	{
		ControlInput = GetComponent<ControlInput>();
	}

	void Update()
	{
		for( int i = 0; i < ControlInput.NumStrings; ++i )
		{
			StringChanges[ i ] = false;
		}

		for( int i = 0; i < Input.touchCount; ++i )
		{
			Touch touch = Input.GetTouch( i );

			if( touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary )
			{
				Ray ray = Camera.main.ScreenPointToRay( touch.position );
				RaycastHit hit;

				for( int j = 0; j < ControlInput.NumStrings; ++j )
				{
					if( ControlInput.GetStringButton( j ).GetComponent<Collider>().Raycast( ray, out hit, Mathf.Infinity ) )
					{
						StringChanges[ j ] = true;
					}
				}
			}
		}

		for( int i = 0; i < ControlInput.NumStrings; ++i )
		{
			ControlInput.OnStringChange( i, StringChanges[ i ] );
		}
		
	}
#endif
}