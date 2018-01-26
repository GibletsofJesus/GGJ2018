
Shader "Custom/ToonDeform" {
	Properties{
		_Colour("Colour", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "black" {}
	_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0
		_OutLineColour("Outline Colour",Color) = (0,0,0,1)
		_OutLineWidth("Outline width", Range(0,1)) = .07
		_Threshold("Cel Threshold", Range(0,20)) = 5
		_Ambient("Ambient Intensity", Range(0,0.5)) = 0.1
		[MaterialToggle] _Is2D("Is 2D?", Float) = 0
		[MaterialToggle] _Pulse("Pulse",Float) = 0
		[MaterialToggle] _Wave("Wave material?",Float) = 0
		_PulseFreq("Pulse Frequency", Range(10,1000)) = 100
		_MinPulseVal("Glow Depth", Range(0, 1)) = 0.5
		_GlowColour("Glow Colour", Color) = (1,1,1,1)
		_OutLineTex("Outline Texture", 2D) = "black"{}
		_WaveHeight("Wave Height", Range(0,0.1)) = 2
		_WaveEdge("Wave Edge", Range(0,0.1)) = 1
	}
		CGINCLUDE
#include "UnityCG.cginc"
#include "AutoLight.cginc"


		struct appdata
	{
		float4 vertex : POSITION;
		float3 normal : NORMAL;
		float2 uv : TEXCOORD0;
		float4 colour:COLOR;

	};
	struct v2f
	{
		float4 position : POSITION;
		float4 colour : COLOR;
		float2 uv : TEXCOORD0;
		float4 normal :NORMAL;
		LIGHTING_COORDS(1, 2)


	};
	float _OutLineWidth;
	float4 _OutLineColour;
	float4 _GlowColour;
	float _Threshold;
	float _Is2D;
	float _Pulse;
	float _Wave;
	float _PulseFreq;
	float _MinPulseVal;
	sampler2D _OutLineTex;
	float4 _OutLineTex_ST;
	float _WaveHeight;
	float _WaveEdge;
	float3 worldPos;

	float LightToonShading(float3 normal, float3 lightDir)
	{

		float NdotL = max(0.0, dot(normalize(normal), normalize(lightDir)));
		float temp = NdotL *_Threshold;
		return(floor(temp) / (_Threshold - 0.5));
	}
	half4 Pulse(float  _minPulse, float4 _pulseCol, float _freq)
	{
		float pulse = _minPulse;
		half posSin = 0.5 * sin(_freq * _Time.x) + 0.5;
		half pulseMultiplier = posSin * (1 - pulse) + _minPulse;
		return (_pulseCol * pulseMultiplier);
	}
	float Wave(float _pos, float _amount)
	{
		float ret = sin(_pos + _Time.w)*(_amount +(0.001 * sin( _Time.z) + 0.01));
		if (_Wave == 0)
			ret = 0;
		return ret;

	}
	sampler2D _MainTex;
	float4 _MainTex_ST;
	v2f vert(appdata v)
	{
		v.vertex.xyz *= (_OutLineWidth / 10) + 1;
		v2f o;
		o.position = UnityObjectToClipPos(v.vertex);
		worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
		o.position.y -= Wave(worldPos.x, _WaveHeight);
		o.position.x -= Wave(worldPos.z, _WaveEdge);
		o.uv = TRANSFORM_TEX(v.uv, _OutLineTex);
		o.normal = mul(v.normal.xyz, unity_WorldToObject);

		return o;
	}
	ENDCG
		SubShader{
		//Tags { "RenderType" = "Opaque" }
		LOD 200
		Tags{ "Queue" = "Transparent" }
		////////////////////////////////
		//First Pass OutLine Shader//
		////////////////////////////////
		Pass // render outline
	{
		ZWrite Off
		CGPROGRAM
#pragma vertex vert
#pragma fragment frag 

		half4 frag(v2f i) : COLOR
	{

		half4 colour = _OutLineColour;
		if (_Pulse > 0)
		{
			colour += Pulse(_MinPulseVal, _GlowColour, _PulseFreq);
		}

		return colour;
	}

		ENDCG
	}
		/////////////////////////
		//Second pass Toon ramp//
		//////////////////////////
		Pass //toonShader
	{
		Tags{ "LightMode" = "ForwardBase" }
		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#pragma multi_compile_fwdadd nolightmap nodirlightmap nodynlightmap novertexlight

		struct Input {
		float2 uv_MainTex;
		float4 color : COLOR;
	};

	float4 _Colour;

	v2f vert(appdata_full v)
	{
		v2f o;
		o.position = UnityObjectToClipPos(v.vertex);
		
		o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
		o.normal = mul(v.normal.xyz,  unity_WorldToObject);
		o.colour = _Colour;
		worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
		o.position.y -= Wave(worldPos.x ,_WaveHeight);
		o.position.x -= Wave(worldPos.z ,_WaveEdge);

		//TRANSFER_VERTEX_TO_FRAGMENT(o);

		return o;
	}

	fixed4 _LightColor0;
	half _Ambient;

	fixed4 frag(v2f i) : SV_Target
	{

		float attenuation = LIGHT_ATTENUATION(i);
	fixed4 col = (tex2D(_MainTex, i.uv) + _Colour);
	if (_Is2D == 0)
	{
		col.rgb *= (saturate(LightToonShading(i.normal, _WorldSpaceLightPos0.xyz) + _Ambient) * col)*_LightColor0.rgb;
	}
	else
	{
		col.rgb *= (saturate(i.normal + _Ambient)) + _LightColor0.rgb;
	}

	return col;
	}


		ENDCG


		Material
	{
		Diffuse[_Colour]
			Ambient[_Colour]
	}


	Lighting On

		SetTexture[_MainTex]
	{
		ConstantColor[_Colour]
	}
		SetTexture[_MainTex]
	{
		Combine previous * primary DOUBLE
	}
	}


	}




		FallBack "Diffuse"
}
