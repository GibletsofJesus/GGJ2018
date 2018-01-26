
Shader "Custom/Mask"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
	}
		SubShader
	{
		Tags{ "RenderType" = "Opaque" "Queue" = "Geometry-1" }
		ColorMask 0
		ZWrite off
		LOD 100

		Stencil
	{
		Ref 1
		Comp always
		Pass replace
	}

		Pass
	{
		Cull back
		ZTest Less

		CGPROGRAM
#pragma vertex vert
#pragma fragment frag


#include "UnityCG.cginc"

		struct appdata
	{
		float4 vertex : POSITION;
	};

	struct v2f
	{
		float4 vertex : SV_POSITION;
	};

	sampler2D _MainTex;
	float4 _MainTex_ST;

	v2f vert(appdata v)
	{
		v2f o;
		o.vertex = UnityObjectToClipPos(v.vertex);
		return o;
	}

	fixed4 frag(v2f i) : SV_Target
	{
		return half4(0,1,1,1);
	}
		ENDCG
	}
	}
}
