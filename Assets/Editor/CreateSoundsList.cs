using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class CreateSoundsList : EditorWindow
{
	private static CreateSoundsList curr_component = null;
	private        AudioClip        material       = null;
	private        Vector2          pos_scroll     = Vector2.zero;
	private        List<AudioClip>  sounds         = new List<AudioClip>();

	//****************************************************************
	[MenuItem("TowerDefence Utils/Create Sounds List", false, 1)]
	public static void init()
	{
		curr_component          = EditorWindow.GetWindow<CreateSoundsList>();
		curr_component.position = new Rect( Screen.width/2-300, 50, 600, 400 );
	}

	//****************************************************************
	void OnGUI()
	{
		// texture list from selection
		EditorGUILayout.BeginVertical();
		pos_scroll = EditorGUILayout.BeginScrollView( pos_scroll );
		for (int x = 0; x < sounds.Count; x++)
		{
            sounds[x] = (AudioClip)EditorGUI.ObjectField( new Rect( x * 50, 10, 50, 50 ), sounds[x], typeof( AudioClip ), true);
        }
		EditorGUILayout.EndScrollView();
        EditorGUILayout.BeginVertical();

		// material selection field
		material = (AudioClip)EditorGUI.ObjectField(new Rect(10, 100, 120, 100), material, typeof(AudioClip), true);

		// add textures from selection
		if( GUI.Button( new Rect( 10, 210, 150, 20 ), "Add from Selection" ) )
		{
            int[] ids = Selection.instanceIDs;
            for( int i = 0; i < ids.Length; i++ )
			{
                AudioClip snd = (AudioClip)AssetDatabase.LoadAssetAtPath( AssetDatabase.GetAssetPath( ids[i] ), typeof( AudioClip ) );
                sounds.Add( snd );
            }
        }

		// prevent from == null
		sounds = sounds.Where(st => st != null).ToList();
		// create go+PackedSprite
		if( GUI.Button(new Rect(10, 240, 100, 30), "[ Create ]") )
		{
			GameObject  go_new    = null;
			AudioSource new_asrc  = null;
			AudioClip   clip_curr = null;

			for( int x = 0; x < sounds.Count; x++ )
			{
				clip_curr = sounds[ x ];

				go_new      = null;
                go_new      = new GameObject();
				go_new.name = clip_curr.name;
				
				new_asrc      = go_new.AddComponent<AudioSource>();
				new_asrc.clip = clip_curr;

				new_asrc.mute          = false;
				new_asrc.bypassEffects = false;
				new_asrc.playOnAwake   = false;
				new_asrc.loop          = false;
            }
		}

		if( GUI.Button( new Rect( 130, 240, 100, 30 ), "[ Reset ]" ) )
		{
			sounds.Clear();
		}
	}

    //****************************************************************
    private void print( object str )
    {
        MonoBehaviour.print( str );
    }
}

