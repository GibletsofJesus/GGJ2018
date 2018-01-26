Shader "Custom/Deform"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	_WaveHeight ("Wave Height", Range(0,20)) = 2 
		_WaveEdge ("Wave Edge", Range(0,20)) = 1
	}
	SubShader
	{
	//	Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
#pragma multi_compile_fwdadd

			// make fog work
#include"AutoLight.cginc"			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
					float4 vertex : SV_POSITION;
					LIGHTING_COORDS(1, 2)

			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float _WaveHeight;
			float _WaveEdge;
			float3 worldPos;
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);

				worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				o.vertex.y -= sin(worldPos.x + _Time.w)*_WaveHeight;
				o.vertex.x -= sin(worldPos.z + _Time.w) *_WaveEdge;
				
				TRANSFER_VERTEX_TO_FRAGMENT(o);

				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);
			if (i.vertex.y < ( _WaveHeight+ worldPos.y))
			{
				float dif = i.vertex.y - (worldPos.y + _WaveHeight);
				col *= dif;
			}
			float attenuation = LIGHT_ATTENUATION(i);
				// apply fog
				return col*attenuation;
			}
			ENDCG
		}
	}
		FallBack "Diffuse"

	
}
