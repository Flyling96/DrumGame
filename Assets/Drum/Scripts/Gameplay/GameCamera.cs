using UnityEngine;
using System.Collections;

public class GameCamera : MonoBehaviour 
{
	Vector3 WidePosition = new Vector3(1.57f, 4.6f, -1.44f);
	Vector3 WideRotation = new Vector3( 29.668f, 0f, 0f );
	Vector3 TallPosition = new Vector3( 0, 6.891119f, -1.746322f );
	Vector3 TallRotation = new Vector3( 43.54943f, 0f, 0f );

	Camera Camera;

	void Start()
	{
		Camera = GetComponent<Camera>();
	}

	void Update ()
	{
		if( IsTallView() == true )
		{
			MoveTowardsTallView();
		}
		else
		{
			MoveTowardsWideView();
		}
	}

	void MoveTowardsTallView()
	{
		transform.position = Vector3.MoveTowards( transform.position, TallPosition, 30f * Time.deltaTime );
		transform.rotation = Quaternion.RotateTowards( transform.rotation, Quaternion.Euler( TallRotation ), 45f * Time.deltaTime );
	}

	void MoveTowardsWideView()
	{
		transform.position = Vector3.MoveTowards( transform.position, WidePosition, 30f * Time.deltaTime );
		transform.rotation = Quaternion.RotateTowards( transform.rotation, Quaternion.Euler( WideRotation ), 45f * Time.deltaTime );
	}

	bool IsTallView()
	{
		return Camera.aspect < 1;
	}
}
