using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor( typeof( StringButton ) )]

public class StringButtonEditor : Editor
{
	protected float LightIntensity = 0;
	protected Color OldColor;

	void Awake()
	{
		OnInspectorOpen();
	}

	void OnDisable()
	{
		OnInspectorClose();
	}

	protected void OnInspectorOpen()
	{
		EnableLightOfTargetObject( true );
		LightIntensity = GetTargetsLightObject().GetComponent<Light>().intensity;
	}

	protected void OnInspectorClose()
	{
		EnableLightOfTargetObject( false );
	}

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();
		DrawLightIntensitySlider();

		if( HasChanged() )
		{
			OnInspectorChanged();
		}

		RememberColorToCheckForChange();
	}

	protected void OnInspectorChanged()
	{
		//UpdateGuitarButtonColor();
		UpdateLightIntensity();
		UpdateKeyCodeLabel();
	}

	protected bool HasChanged()
	{
		if( GUI.changed )
		{
			return true;
		}

		if( GetTarget().Color != OldColor )
		{
			return true;
		}

		return false;
	}

	protected void RememberColorToCheckForChange()
	{
		OldColor = GetTarget().Color;
	}

	protected void DrawLightIntensitySlider()
	{
		EditorGUILayout.BeginHorizontal();
			GUILayout.Space( 15 );
			GUILayout.Label( "Light Intensity", EditorStyles.label );
			LightIntensity = GUILayout.HorizontalSlider( LightIntensity, 0, 8 );
		EditorGUILayout.EndHorizontal();
	}

	protected void UpdateGuitarButtonColor()
	{
		GameObject.Find( "Guitar" ).GetComponent<GuitarGameplay>().UpdateColorsArray();
	}

	protected void UpdateLightIntensity()
	{
		GetTargetsLightObject().GetComponent<Light>().intensity = LightIntensity;
	}

	protected void UpdateKeyCodeLabel()
	{
		KeyCode key = GetTarget().Key;
		string keyString = key.ToString();

		if( IsKeyAlphaNumeric( key ) )
		{
			keyString = keyString.Substring( 5, 1 ); //Strip String of "Alpha" pretext
		}

		SetTargetsKeyCodeLabel( keyString );
	}

	protected void SetTargetsKeyCodeLabel( string text )
	{
		GetTargetsGameObject().transform.Find( "KeyCode Label" ).GetComponent<TextMesh>().text = text;
	}

	protected bool IsKeyAlphaNumeric( KeyCode key )
	{
		return key >= KeyCode.Alpha0 && key <= KeyCode.Alpha9;
	}

	protected void EnableLightOfTargetObject( bool enabled )
	{
		GetTargetsLightObject().GetComponent<Light>().enabled = enabled;
	}

	protected GameObject GetTargetsGameObject()
	{
		return GetTarget().gameObject;
	}

	protected GameObject GetTargetsLightObject()
	{
		return GetTargetsGameObject().transform.Find( "Light" ).gameObject;
	}

	protected StringButton GetTarget()
	{
		return ( target as StringButton );
	}
    
}