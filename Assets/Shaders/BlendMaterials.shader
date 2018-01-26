Shader "Custom/BlendMaterials"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_BlendTex("Blend Texture", 2D) = "white"{}
		_Target("Target Position", vector) = (0,0,0,0)
			_Lerp("lerp val",Float) = 0.5
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
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
				float dist : TEXCOORD1;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			sampler2D _BlendTex;
			float4 _BlendTex_ST;
			float4 _Target;
			float _Lerp;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.dist = normalize(distance(mul(unity_ObjectToWorld, v.vertex).xyz,_Target.xyz));
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
			fixed4 col = lerp(tex2D(_MainTex, i.uv), tex2D(_BlendTex, i.uv),i.dist);	
				return col;
			}
			ENDCG
		}
	}
}
