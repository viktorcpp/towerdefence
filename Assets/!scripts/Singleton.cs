using UnityEngine;
using System.Collections;

//****************************************************************
// Singleton, Pattern, The
//****************************************************************

public class Singleton<T> : MonoBehaviour
{
    private static T instance = default( T );

    //****************************************************************
    public static T Instance
    {
        get
        {
            if( instance == null )
            {
                instance = Utils.FindComp<T>();

                if( instance == null )
                {
                    //Debug.LogError( "Singleton ERROR: class " + typeof( T ) + " not found" );
                }
            }

            return instance;
        }
    }

	//****************************************************************
	private void OnDestroy()
	{
		instance = default( T );
	}
}
