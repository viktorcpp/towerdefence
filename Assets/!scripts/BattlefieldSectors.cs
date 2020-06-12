#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml;

public class BattlefieldSectors : Singleton<BattlefieldSectors>
{
    [SerializeField]
    private Vector3                 pos_visible     = new Vector3( 12, 0, 100 );
    [SerializeField]
    private Vector3                 pos_hidden      = new Vector3( 0, 0, 2001 );
    [SerializeField]
    private BattlefieldSectorItem[] item_top_list   = null;
    [SerializeField]
    private BattlefieldSectorItem[] item_right_list = null;
    [SerializeField]
    private BattlefieldSectorItem[] item_bot_list   = null;
    [SerializeField]
    private BattlefieldSectorItem[] item_left_list  = null;
    [SerializeField]
    private Transform               t_sector_busy   = null;

    private bool                    is_visible      = false;

    //****************************************************************
    public BattlefieldSectorItem GetSectorRndTop()
    {
        return item_top_list[ Utils.Random( 0, item_top_list.Length ) ];
    }

    //****************************************************************
    public BattlefieldSectorItem GetSectorRndRight()
    {
        return item_right_list[ Utils.Random( 0, item_right_list.Length ) ];
    }

    //****************************************************************
    public BattlefieldSectorItem GetSectorRndBot()
    {
        return item_bot_list[ Utils.Random( 0, item_bot_list.Length ) ];
    }

    //****************************************************************
    public BattlefieldSectorItem GetSectorRndLeft()
    {
        return item_left_list[ Utils.Random( 0, item_left_list.Length ) ];
    }

    //****************************************************************
    public Transform GetSectorBusyTmplt()
    {
        return ((GameObject)Instantiate( t_sector_busy.gameObject )).transform;
    }

    //****************************************************************
    public BattlefieldSectorItem GetSectorItem( string sector_name )
    {
        return node_list.Find( delegate( BattlefieldSectorItem bsi ){ return bsi.name == sector_name; } );
    }

    //****************************************************************
    public void Show()
    {
        if( is_visible ) return;

        transform.localPosition = pos_visible;
        is_visible              = true;
    }

    //****************************************************************
    public void Hide()
    {
        if( !is_visible ) return;

        transform.localPosition = pos_hidden;
        is_visible              = false;
    }

	//****************************************************************
	public void _Start()
	{
		this.Hide();

        node_list = GetComponentsInChildren<BattlefieldSectorItem>().ToList();
        this._CreateDict();
	}

    //********************************************************************************************************************************
    //********************************************************************************************************************************
    //********************************************************************************************************************************

    private float[,]                               matrix     = null;
    private List<BattlefieldSectorItem>            node_list  = new List<BattlefieldSectorItem>();
    private Dictionary<BattlefieldSectorItem, int> nodes_dict = new Dictionary<BattlefieldSectorItem, int>();

    //****************************************************************
    public List<BattlefieldSectorItem> GetPath( BattlefieldSectorItem start_node, BattlefieldSectorItem end_node )
    {
        return GetPath( start_node.name, end_node.name );
    }

    //****************************************************************
	public List<BattlefieldSectorItem> GetPath( string start_node_name, string end_node_name )
	{
		BattlefieldSectorItem start_node = _FindNode( start_node_name );
		BattlefieldSectorItem end_node   = _FindNode( end_node_name   );
		
		if( start_node == null || end_node == null ) return null;

		this._CreateAdjMatrix();
		this._HighlightAdjMatrix( start_node_name );

		Dijkstra d = new Dijkstra( matrix, nodes_dict[ start_node ] );
		int[] nodes_index = d.GetPathTo( nodes_dict[ end_node ] );

		List<BattlefieldSectorItem> path = new List<BattlefieldSectorItem>();

		foreach( int i in nodes_index )
			foreach( var j in node_list )
				if( nodes_dict[j] == i )
					path.Add( j );

        //path_test = path;

		return path;
	}

    //********************************************************************************************************************************
    private void _CreateDict()
    {
        for( int x = 0; x < node_list.Count; x++ )
        {
            nodes_dict.Add( node_list[x], x );
        }
    }

    //****************************************************************
	private float[,] _CreateAdjMatrix()
	{
        if( matrix == null )
		    matrix = new float[ node_list.Count, node_list.Count ];

		for( int x = 0; x < node_list.Count; x++ )
			for( int y = 0; y < node_list.Count; y++ )
				matrix[ x, y ] = 0;

		return matrix;
	}

    //****************************************************************
	private void _HighlightAdjMatrix( string current_sector_name )
	{
		foreach( BattlefieldSectorItem node in node_list )
		{
            //
            if( node.NodeTop != null && node.BuildingBusy == null )
                if( node.NodeTop.name != current_sector_name && node.NodeTop.BuildingBusy == null )
                {
                    matrix[ nodes_dict[ node ],         nodes_dict[ node.NodeTop ] ] = 1;
					matrix[ nodes_dict[ node.NodeTop ], nodes_dict[ node ]         ] = 1;
                }

            if( node.NodeTopRight != null && node.BuildingBusy == null )
                if( node.NodeTopRight.name != current_sector_name && node.NodeTopRight.BuildingBusy == null )
                {
                    matrix[ nodes_dict[ node ],              nodes_dict[ node.NodeTopRight ] ] = 1;
					matrix[ nodes_dict[ node.NodeTopRight ], nodes_dict[ node ]              ] = 1;
                }

            if( node.NodeRight != null && node.BuildingBusy == null )
                if( node.NodeRight.name != current_sector_name && node.NodeRight.BuildingBusy == null )
                {
                    matrix[ nodes_dict[ node ],           nodes_dict[ node.NodeRight ] ] = 1;
					matrix[ nodes_dict[ node.NodeRight ], nodes_dict[ node ]           ] = 1;
                }

            if( node.NodeBotRight != null && node.BuildingBusy == null )
                if( node.NodeBotRight.name != current_sector_name && node.NodeBotRight.BuildingBusy == null )
                {
                    matrix[ nodes_dict[ node ],              nodes_dict[ node.NodeBotRight ] ] = 1;
					matrix[ nodes_dict[ node.NodeBotRight ], nodes_dict[ node ]              ] = 1;
                }

            if( node.NodeBot != null && node.BuildingBusy == null )
                if( node.NodeBot.name != current_sector_name && node.NodeBot.BuildingBusy == null )
                {
                    matrix[ nodes_dict[ node ],         nodes_dict[ node.NodeBot ] ] = 1;
					matrix[ nodes_dict[ node.NodeBot ], nodes_dict[ node ]         ] = 1;
                }

            if( node.NodeBotLeft != null && node.BuildingBusy == null )
                if( node.NodeBotLeft.name != current_sector_name && node.NodeBotLeft.BuildingBusy == null )
                {
                    matrix[ nodes_dict[ node ],             nodes_dict[ node.NodeBotLeft ] ] = 1;
					matrix[ nodes_dict[ node.NodeBotLeft ], nodes_dict[ node ]             ] = 1;
                }

            if( node.NodeLeft != null && node.BuildingBusy == null )
                if( node.NodeLeft.name != current_sector_name && node.NodeLeft.BuildingBusy == null )
                {
                    matrix[ nodes_dict[ node ],          nodes_dict[ node.NodeLeft ] ] = 1;
					matrix[ nodes_dict[ node.NodeLeft ], nodes_dict[ node ]          ] = 1;
                }

            if( node.NodeLeftTop != null && node.BuildingBusy == null )
                if( node.NodeLeftTop.name != current_sector_name && node.NodeLeftTop.BuildingBusy == null )
                {
                    matrix[ nodes_dict[ node ],             nodes_dict[ node.NodeLeftTop ] ] = 1;
					matrix[ nodes_dict[ node.NodeLeftTop ], nodes_dict[ node ]             ] = 1;
                }
		}
	}

    //****************************************************************
	private BattlefieldSectorItem _FindNode( string node_name )
	{
		return node_list.Find( delegate( BattlefieldSectorItem si ){ return si.name == node_name; } );
	}

    //****************************************************************
	private BattlefieldSectorItem _FindNode( BattlefieldSectorItem node )
	{
		return node_list.Find( delegate( BattlefieldSectorItem si ){ return si == node; } );
	}

    //********************************************************************************************************************************
    //List<BattlefieldSectorItem> path_test          = new List<BattlefieldSectorItem>();
    //private string              tf_start_node_name = "a-left-2";
    //private string              tf_end_node_name   = "a-right-2";
    //private void OnGUI()
    //{
    //    tf_start_node_name = GUI.TextField( new Rect( 20, 125, 100, 30 ), tf_start_node_name );
    //    tf_end_node_name   = GUI.TextField( new Rect( 125, 125, 100, 30 ), tf_end_node_name );

    //    if( GUI.Button( new Rect( 20, 100, 100, 25 ), "Find Path" ) )
    //    {
    //        path_test = GetPath( tf_start_node_name, tf_end_node_name );
    //        Core.Log = "path nodes count: " + path_test.Count;
    //    }
    //}
    //private void OnDrawGizmos()
    //{
    //    if( path_test.Count > 0 )
    //    {
    //        // draw path
    //        for( int x = 0; x < path_test.Count; x++ )
    //        {
    //            if( x+1 < path_test.Count )
    //            {
    //                Gizmos.color = Color.magenta;
    //                Gizmos.DrawSphere( path_test[x].transform.position, 5 );
    //                Gizmos.DrawSphere( path_test[x+1].transform.position, 5 );

    //                Gizmos.color = Color.blue;
    //                Gizmos.DrawLine( path_test[x].transform.position, path_test[x+1].transform.position );
    //            }
    //        }
    //    }
    //}
}

public class Dijkstra
{
    /* Takes adjacency matrix in the following format, for a directed graph (2-D array)
     * Ex. node 1 to 3 is accessible at a cost of 4
     *        0  1  2  3  4 
     *   0  { 0, 2, 5, 0, 0},
     *   1  { 0, 0, 0, 4, 0},
     *   2  { 0, 6, 0, 0, 8},
     *   3  { 0, 0, 0, 0, 9},
     *   4  { 0, 0, 0, 0, 0}
     */

    /* Resulting arrays with distances to nodes and how to get there */
    public float[] dist { get; private set; }
    public int[]   path { get; private set; }

    /* Holds queue for the nodes to be evaluated */
    private List<int> queue = new List<int>();

	//****************************************************************
	public Dijkstra( float[,] G, int s )
    {
        /* Check graph format and that the graph actually contains something */
        if( G.GetLength( 0 ) < 1 || G.GetLength( 0 ) != G.GetLength( 1 ) )
        {
            throw new System.ArgumentException( "Graph error, wrong format or no nodes to compute" );
        }

        int len = G.GetLength( 0 );

        Initialize( s, len );

        while( queue.Count > 0 )
        {
            int u = GetNextVertex();

            /* Find the nodes that u connects to and perform relax */
            for( int v = 0; v < len; v++ )
            {
                /* Checks for edges with negative weight */
                if( G[ u, v ] < 0 )
                {
                    throw new System.ArgumentException( "Graph contains negative edge(s)" );
                }

                /* Check for an edge between u and v */
                if( G[ u, v ] > 0 )
                {
                    /* Edge exists, relax the edge */
                    if( dist[ v ] > dist[ u ] + G[ u, v ] )
                    {
                        dist[ v ] = dist[ u ] + G[ u, v ];
                        path[ v ] = u;
                    }
                }
            }
        }
    }

	//****************************************************************
    public bool IsPathExists( int to )
	{
        if( float.IsPositiveInfinity( dist[ to ] ) )
            return false;
        else
            return true;
    }

	//****************************************************************
    public int[] GetPathTo( int to )
	{
        if( !IsPathExists( to ) )
		{
            return new int[]{};
		}
        else
		{
            List<int> inverse_path = new List<int>();
            int i = to;
            inverse_path.Add( i );
            while( path[i] != -1 )
			{
                inverse_path.Add( path[ i ] );
                i = path[ i ];
            }
            inverse_path.Reverse();
            inverse_path.RemoveAt( 0 );

            return inverse_path.ToArray();
        }
    }

    /* Sets up initial settings */
    private void Initialize( int s, int len )
    {
        dist = new float[ len ];
        path = new int[ len ];

        /* Set distance to all nodes to infinity - alternatively use Int.MaxValue for use of Int type instead */
        for( int i = 0; i < len; i++ )
        {
            dist[ i ] = float.PositiveInfinity;
            queue.Add( i );
        }

        /* Set distance to 0 for starting point and the previous node to null (-1) */
        dist[ s ] =  0;
        path[ s ] = -1;
    }

    /* Retrives next node to evaluate from the queue */
    private int GetNextVertex()
    {
        float min = float.PositiveInfinity;
        int Vertex = -1;

        /* Search through queue to find the next node having the smallest distance */
        foreach( int j in queue )
        {
            if( dist[ j ] <= min )
            {
                min = dist[ j ];
                Vertex = j;
            }
        }
        queue.Remove( Vertex );

        return Vertex;
    }
}
