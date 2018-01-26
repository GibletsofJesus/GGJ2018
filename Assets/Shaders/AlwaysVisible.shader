Shader "Custom/AlwaysVisible"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_VisTex("Visible Texture", 2D) = "white"{}
		_Colour("Main Colour", Color) = (0,0,0,0)
		_VisCol("Visible Colour", Color) = (0,0,0,0)

	}
	SubShader
	{
			Tags{ "Queue" = "Transparent" }
			LOD 100

			Pass
		{
			Cull Off
			ZWrite Off
			ZTest Always
			CGPROGRAM
#pragma vertex vert
#pragma fragment frag


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
		};

		sampler2D _VisTex;
		float4 _VisCol;
		float4 _VisTex_ST;

		v2f vert(appdata v)
		{
			v2f o;
			o.vertex = UnityObjectToClipPos(v.vertex);
			o.uv = TRANSFORM_TEX(v.uv, _VisTex);
			return o;
		}

		fixed4 frag(v2f i) : SV_Target
		{
			// sample the texture
			fixed4 col = tex2D(_VisTex, i.uv) *_VisCol;

		return col;
		}
			ENDCG
		}
		Pass
		{
		
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			
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
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float4 _Colour;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv) * _Colour;

				return col;
			}
			ENDCG
		}
	}
}
