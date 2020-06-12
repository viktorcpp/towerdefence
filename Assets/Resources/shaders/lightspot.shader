Shader "Ravenhill/Spot"
{
	Properties
	{
		_U( "U", Float ) = 0.5
		_V( "V", Float ) = 0.5
		_R( "R", Float ) = 0.09
	}

	Category
	{
		Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha 
    
		SubShader
		{
			Pass
			{
				Fog{ Mode Off }

				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag

				float _U;
				float _V;
				float _R;

				// vertex input: position, UV
				struct appdata
				{
					float4 vertex   : POSITION;
					float4 texcoord : TEXCOORD0;
				};

				struct v2f
				{
					float4 pos : SV_POSITION;
					float4 uv  : TEXCOORD0;
				};

				v2f vert ( appdata v )
				{
					v2f o;
					o.pos = mul( UNITY_MATRIX_MVP, v.vertex );
					o.uv  = float4( v.texcoord.xy, 0, 0 );
					return o;
				}
            
				half4 frag( v2f i ) : COLOR
				{
					if( _V < 1 )
					{
						_V = _V + 1;
					}
					if( _V > 2 )
					{
						_V = 2;
					}

					float4 f = float4( _U, _V, 0, 0 );
					float  d = distance( i.uv, f );
                
					if( d < _R)
					{
						return half4( 1, 1, 1, 0 );
					}
					else
					{
						return half4( 0, 0, 0, 0.1 + saturate((d - 0.1)* 20) * 0.9 );
					}
				}
				ENDCG
			}
		}
	}
}