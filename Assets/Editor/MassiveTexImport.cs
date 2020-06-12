using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml;

public class MassiveTexImport : EditorWindow
{
    private static MassiveTexImport curr_component = null;

	private int                      aniso_level        = 1;
	private FilterMode               filter_mode        = FilterMode.Point;
	private TextureImporterType      tex_type           = TextureImporterType.Advanced;
	private TextureImporterNPOTScale npot_scale         = TextureImporterNPOTScale.ToNearest;
	private TextureImporterFormat    tex_format         = TextureImporterFormat.AutomaticTruecolor;
	private int                      max_tex_size       = 1024;
	private bool                     read_write_enabled = false;
	private bool                     mipmap_gen         = false;

	//****************************************************************
	[MenuItem("TowerDefence Utils/Tex Import For Selection", false, 3)]
	public static void init()
	{
		curr_component          = EditorWindow.GetWindow<MassiveTexImport>();
		curr_component.position = new Rect( Screen.width/2-300, 50, 400, 300 );
	}
    
    //****************************************************************
	private void OnGUI()
	{
        aniso_level        = (int)                     EditorGUI.IntSlider( new Rect( 20, 20, 300, 20 ),  aniso_level, 0, 9 );
        filter_mode        = (FilterMode)              EditorGUI.EnumPopup( new Rect( 20, 50, 300, 20 ),  "Filter Mode",        filter_mode );
        tex_type           = (TextureImporterType)     EditorGUI.EnumPopup( new Rect( 20, 80, 300, 20 ),  "Texture Type",       tex_type );
        npot_scale         = (TextureImporterNPOTScale)EditorGUI.EnumPopup( new Rect( 20, 110, 300, 20 ), "Non Power of 2",     npot_scale );
        tex_format         = (TextureImporterFormat)   EditorGUI.EnumPopup( new Rect( 20, 140, 300, 20 ), "Texture Format",     tex_format );
        max_tex_size       = (int)                     EditorGUI.IntField ( new Rect( 20, 170, 300, 20 ), "Max Tex Size",       max_tex_size );
        read_write_enabled = (bool)                    EditorGUI.Toggle   ( new Rect( 20, 200, 300, 20 ), "Read/Write Enabled", read_write_enabled );
        mipmap_gen         = (bool)                    EditorGUI.Toggle   ( new Rect( 20, 230, 330, 20 ), "Generate MipMap",    mipmap_gen );
        
        if( GUI.Button( new Rect( 20, 260, 150, 20 ), "Apply" ) )
        {
            int[] ids = Selection.instanceIDs;
            
            for( int i = 0; i < ids.Length; i++ )
            {
				AssetDatabase.ImportAsset( AssetDatabase.GetAssetPath( ids[i] ), ImportAssetOptions.ForceUpdate );

                Texture2D tex = (Texture2D)AssetDatabase.LoadAssetAtPath( AssetDatabase.GetAssetPath( ids[i] ), typeof( Texture2D ) );

                TextureImporter texture_importer = ( TextureImporter ) TextureImporter.GetAtPath( AssetDatabase.GetAssetPath( tex.GetInstanceID() ) );

				texture_importer.textureType     = tex_type;
				texture_importer.npotScale       = npot_scale;
				texture_importer.anisoLevel      = aniso_level;
				texture_importer.filterMode      = filter_mode;
				texture_importer.maxTextureSize  = max_tex_size;
				texture_importer.mipmapEnabled   = mipmap_gen;
				texture_importer.textureFormat   = tex_format;
				texture_importer.isReadable      = read_write_enabled;

				AssetDatabase.ImportAsset( AssetDatabase.GetAssetPath( ids[i] ), ImportAssetOptions.ForceUpdate );

				AssetDatabase.SaveAssets();
            }

			AssetDatabase.SaveAssets();
			Resources.UnloadUnusedAssets();
        }
    }

    //****************************************************************
    private void print( object str )
    {
        MonoBehaviour.print( str );
    }
}

