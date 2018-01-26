Shader "Custom/Outline"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_OutLineColour("Outline Colour",Color) = (255,255,255,255)
		_OutLineWidth("Outline width", Range(0,1)) = .01
		_Colour("Main Colour", Color) = (0.5,0.5,0.5,1)
		_OutLineIntensity("Outline Intensity", Range(0,1)) = 1
			[MaterialToggle] _Pulse("Pulse",Float) = 0
			_PulseFreq("Pulse Frequency", Range(10,1000)) = 100
			_MinPulseVal("Glow Depth", Range(0, 1)) = 0.5
			_GlowColour("Glow Colour", Color) = (1,1,1,1)
	}

		CGINCLUDE 
#include "UnityCG.cginc"
#include "AutoLight.cginc"
#include "Lighting.cginc"
			float _OutLineWidth;
		float4 _OutLineColour;
		float _Pulse;
		float _PulseFreq;
		float _MinPulseVal;
		float4 _GlowColour;
			half4 Pulse(float  _minPulse, float4 _pulseCol, float _freq)
		{
			float pulse = _minPulse;
			half posSin = 0.5 * sin(_freq * _Time.x) + 0.5;
			half pulseMultiplier = posSin * (1 - pulse) + _minPulse;
			return (_pulseCol * pulseMultiplier);
		}


		ENDCG
	SubShader
		{
				Tags { "Queue" = "Transparent"}
				CGINCLUDE
				struct appdata
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float4 colour :COLOR;

			};

			struct v2f
			{
				float4 position : POSITION;
				float4 colour : COLOR;
				float3 normal : NORMAL;
			};

		
			v2f vert(appdata v)
			{
				float3 vert = v.vertex.xyz;
				v.vertex.xyz *= ((_OutLineWidth) / 10) + 1;
				v2f o;
				o.colour = _OutLineColour;
				o.position = UnityObjectToClipPos(v.vertex);
				return o;
			}
			ENDCG
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

			Pass //object
			{

			
			
				ZWrite On
	
		
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
		
			FallBack "VertexLit"
}

