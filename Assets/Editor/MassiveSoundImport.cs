using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml;

public class MassiveSoundImport : EditorWindow
{
    private static MassiveSoundImport curr_component = null;

	private AudioImporterFormat   audio_format     = AudioImporterFormat.Native;
    private bool                  is_3d_sound      = false;
    private bool                  is_force_to_mono = false;
    private AudioImporterLoadType load_type        = AudioImporterLoadType.CompressedInMemory;
    private int                   compression      = 128;

	//****************************************************************
	[MenuItem("TowerDefence Utils/Sound Import For Selection", false, 4)]
	public static void init()
	{
		curr_component          = EditorWindow.GetWindow<MassiveSoundImport>();
		curr_component.position = new Rect( Screen.width/2-300, 50, 400, 300 );
	}
    
    //****************************************************************
	private void OnGUI()
	{
        audio_format      = (AudioImporterFormat)  EditorGUI.EnumPopup( new Rect( 20, 30,  300, 20 ), "Audio Format",  audio_format      );
        is_3d_sound       = (bool)                 EditorGUI.Toggle   ( new Rect( 20, 70,  300, 20 ), "Is 3D sound",   is_3d_sound       );
        is_force_to_mono  = (bool)                 EditorGUI.Toggle   ( new Rect( 20, 110, 300, 20 ), "Force to Mono", is_force_to_mono  );
        load_type         = (AudioImporterLoadType)EditorGUI.EnumPopup( new Rect( 20, 150, 300, 20 ), "Load Type",     load_type         );
        compression       = (int)                  EditorGUI.IntSlider( new Rect( 20, 190, 300, 20 ),  compression, 0, 256               );

        if( GUI.Button( new Rect( 20, 260, 150, 20 ), "Apply" ) )
        {
            Object[] audio_clip_list = Selection.GetFiltered( typeof( AudioClip ), SelectionMode.DeepAssets );
            
            for( int i = 0; i < audio_clip_list.Length; i++ )
            {
                string asset_path = AssetDatabase.GetAssetPath( audio_clip_list[i] );
				
                AudioImporter audio_importer = (AudioImporter)AssetImporter.GetAtPath( asset_path );
                
                audio_importer.format             = audio_format;
                AssetDatabase.ImportAsset( asset_path );
                audio_importer.threeD             = is_3d_sound;
                AssetDatabase.ImportAsset( asset_path );
                audio_importer.forceToMono        = is_force_to_mono;
                AssetDatabase.ImportAsset( asset_path );
                audio_importer.loadType           = load_type;
                AssetDatabase.ImportAsset( asset_path );
                audio_importer.compressionBitrate = (int)compression + 1024;
                AssetDatabase.ImportAsset( asset_path );
				
				AssetDatabase.SaveAssets();
            }

			Resources.UnloadUnusedAssets();
        }
    }

    //****************************************************************
    private void print( object str )
    {
        MonoBehaviour.print( str );
    }
}

