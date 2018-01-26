Shader "Custom/Printing"
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
	_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0
		_PCol("Print Colour", Color) = (1,1,1,1)
		_PrintY("Cutoff Point", float) = 1
		_PrintGap("Thickness",float) = 0.1
		_OnOrOff("Effect Enabled", int) = 0
	}
		SubShader
	{
		Tags{ "RenderType" = "Opaque" }
		Cull Off
		LOD 200


		CGPROGRAM
#include "UnityPBSLighting.cginc"
#include "UnityCG.cginc"
		// Physically based Standard lighting model, and enable shadows on all light types
#pragma surface surf Custom fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
#pragma target 3.0
#define IF(a, b, c) lerp(b, c, step((fixed) (a), 0))

		sampler2D _MainTex;

	struct Input {
		float2 uv_MainTex;
		float3 worldPos;
		float3 viewDir;
	};

	half _Glossiness;
	half _Metallic;
	fixed4 _Color;
	fixed4 _PCol;
	float _PrintY;
	float _PrintGap;
	int building;
	int _OnOrOff;
	float3 viewDir;

	void surf(Input IN, inout SurfaceOutputStandard o)
	{
		_OnOrOff = IF(_OnOrOff > 0.5, 1, 0);
		viewDir = IN.viewDir;
		// Albedo comes from a texture tinted by color
		fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
		if (_OnOrOff) {
			if (IN.worldPos.y > _PrintY + _PrintGap)
				discard;
		}
		o.Albedo = IF(_OnOrOff, IF(IN.worldPos.y > _PrintY, _PCol.rgb, c.rgb), c.rgb);
		building = IF(_OnOrOff, IF(IN.worldPos.y > _PrintY, 1, 0), 0);


		// Metallic and smoothness come from slider variables			
		o.Metallic = _Metallic;
		o.Smoothness = _Glossiness;
		//o.Alpha = c.a;
	}


	inline half4 LightingCustom(SurfaceOutputStandard s, half3 lightDir, UnityGI gi)
	{
		if (dot(s.Normal, viewDir) < 0)
			return _PCol;
		if (!building)
			return LightingStandard(s, lightDir, gi); // Unity5 PBR

		return _PCol; // Unlit
	}

	inline void LightingCustom_GI(SurfaceOutputStandard s, UnityGIInput data, inout UnityGI gi)
	{
		LightingStandard_GI(s, data, gi);
	}

	ENDCG
	}
		FallBack "Diffuse"
}
