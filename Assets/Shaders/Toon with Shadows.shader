Shader "Custom/cel" {
	Properties{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
	_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0
		_Cel("Cel Amount", Range(1,20)) = 5
	}
		SubShader{
		Tags{ "RenderType" = "Opaque" }
		LOD 200

		CGPROGRAM #pragma surface surf Standard CelShadingForward #pragma target 3.0
		float _Cel;
		half4 LightingCelShadingForward(SurfaceOutput s, half3 lightDir, half atten)
	{
		half NdotL = (dot(s.Normal, lightDir)*0.5+0.5)*atten;
		NdotL = smoothstep(0, 0.025f, NdotL);
		NdotL = 1 + clamp(floor(NdotL), -1, 1);
		if (NdotL <= 0.0)
		{
			NdotL = 0;
		}
		else if (NdotL>0&&NdotL<1)
		{
			NdotL = 0.5;
		}
		else
		{
			NdotL = 1;
		}
		half4 c;
		c.rgb = s.Albedo * _LightColor0.rgb * (NdotL * atten * 2);
		c.a = s.Alpha;
		return c;

	}
	sampler2D _MainTex;
	fixed4 _Color;


	struct Input
	{
		float2 uv_MainTex;
	};

	half _Glossiness;
	half _Metallic;
	//fixed4 _Color;

	void surf(Input IN, inout SurfaceOutputStandard o)
	{
		// Albedo comes from a texture tinted by color
		fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
		o.Albedo = c.rgb;
		// Metallic and smoothness come from slider variables
		o.Alpha = c.a;
	}
	ENDCG
	}
		FallBack "Diffuse"
}


//Shader "Custom/Toon with Shadows" {
//	Properties{
//		_Color("Main Color", Color) = (0.5,0.5,0.5,1)
//		_MainTex("Base (RGB)", 2D) = "white" {}
//	_Ramp("Toon Ramp (RGB)", 2D) = "black" {}
//	_CelVal("Cel Value", Range(0,20))= 5
//	}
//
//		SubShader{
//		Tags{ "RenderType" = "Opaque" }
//		LOD 200
//
//		CGPROGRAM
//#pragma surface surf ToonRamp
//
//		sampler2D _Ramp;
//	float _CelVal;
//
//	// custom lighting function that uses a texture ramp based
//	// on angle between light direction and normal
//#pragma lighting ToonRamp exclude_path:prepass
//	inline half4 LightingToonRamp(SurfaceOutput s, half3 lightDir, half atten)
//	{
//#ifndef USING_DIRECTIONAL_LIGHT
//		lightDir = normalize(lightDir);
//#endif
//		half NdotL = dot(s.Normal, lightDir);
//		NdotL = smoothstep(0, 0.025f, NdotL);
//		NdotL = 1 + clamp(floor(NdotL), -1, 0);
//		/*if (NdotL <= 0.0)
//		{
//			NdotL = 0;
//		}
//		else
//		{
//			NdotL = 1;
//		}*/
//		half d =( dot(s.Normal, lightDir)*0.5 + 0.5)* atten;
//		half3 ramp = tex2D(_Ramp, float2(d,d)).rgb*_CelVal;
//
//		half4 c;
//		c.rgb = (s.Albedo * _LightColor0.rgb * ramp * ((atten) * 2));//*(Ndotl*_CelVal);
//		//c.a = 0;
//		return c;
//	}
//
//
//	sampler2D _MainTex;
//	float4 _Color;
//
//	struct Input {
//		float2 uv_MainTex : TEXCOORD0;
//	};
//
//	void surf(Input IN, inout SurfaceOutput o) {
//		half4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
//		o.Albedo = c.rgb;
//		o.Alpha = c.a;
//	}
//	ENDCG
//
//	}
//
//		Fallback "Diffuse"
//}
