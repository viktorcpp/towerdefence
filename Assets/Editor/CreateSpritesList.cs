using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class CreateSpritesList : EditorWindow
{
	private static CreateSpritesList curr_component = null;
	private        Material          material       = null;
	private        Vector2           pos_scroll     = Vector2.zero;
	private        List<Texture2D>   textures       = new List<Texture2D>();
	private        string            mat_name       = "if new material - set name";

	//****************************************************************
	[MenuItem("TowerDefence Utils/Create Packed Sprites List", false, 2)]
	public static void init()
	{
		curr_component          = EditorWindow.GetWindow<CreateSpritesList>();
		curr_component.position = new Rect( Screen.width/2-300, 50, 600, 400 );
	}

	//****************************************************************
	void OnGUI()
	{
		// texture list from selection
		EditorGUILayout.BeginVertical();
		pos_scroll = EditorGUILayout.BeginScrollView( pos_scroll );
		for (int x = 0; x < textures.Count; x++)
		{
            textures[x] = (Texture2D)EditorGUI.ObjectField( new Rect( x * 50, 10, 50, 50 ), textures[x], typeof( Texture2D ), true);
        }
		EditorGUILayout.EndScrollView();
        EditorGUILayout.BeginVertical();

		// material selection field
		material = (Material)EditorGUI.ObjectField(new Rect(10, 100, 120, 100), material, typeof(Material), true);

		// add textures from selection
		if( GUI.Button( new Rect( 10, 210, 150, 20 ), "Add from Selection" ) )
		{
            int[] ids = Selection.instanceIDs;
            for( int i = 0; i < ids.Length; i++ )
			{
                Texture2D tex = (Texture2D)AssetDatabase.LoadAssetAtPath( AssetDatabase.GetAssetPath( ids[i] ), typeof( Texture2D ) );
                textures.Add( tex );
            }
        }

		// new Material creating
		mat_name = EditorGUI.TextField(new Rect(200, 100, 200, 20), mat_name);
		if( GUI.Button( new Rect( 200, 130, 150, 20 ), "Create NEW material" ) )
		{
			material       = new Material( Shader.Find( "Transparent/Vertex Colored" ) );
			material.color = new Color( 143.0f, 143.0f, 143.0f, 255.0f );
			AssetDatabase.CreateAsset( material, "Assets/Resources/Materials/" + mat_name + ".mat" );
		}

		// prevent from == null
		textures = textures.Where(st => st != null).ToList();
		// create go+PackedSprite
		if( GUI.Button(new Rect(10, 240, 100, 30), "[ Create ]") )
		{
			MonoBehaviour.print(textures.Count);
			for (int x = 0; x < textures.Count; x++)
			{   
                PackedSprite _sprite = PackedSprite.Create( textures[x].name, new Vector3( 0.0f, 0.0f, -1.0f ) );
                
                AssetDatabase.ImportAsset( AssetDatabase.GetAssetPath( textures[x].GetInstanceID() ) );
                
                float width  = textures[x].width;
                float height = textures[x].height;

                AssetDatabase.ImportAsset( AssetDatabase.GetAssetPath( textures[x].GetInstanceID() ) );

                _sprite.SetSize( width, height );
                _sprite.renderer.sharedMaterial = material;
                _sprite.staticTexPath = AssetDatabase.GetAssetPath( textures[x].GetInstanceID() );
                _sprite.staticTexGUID = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath( textures[x]) );

				MeshRenderer mr   = _sprite.GetComponent<MeshRenderer>();
				mr.castShadows    = false;
				mr.receiveShadows = false;
            }
		}

		if( GUI.Button( new Rect( 130, 240, 100, 30 ), "[ Reset ]" ) )
		{
			textures.Clear();
		}
	}

    //****************************************************************
    private void print( object str )
    {
        MonoBehaviour.print( str );
    }
}

