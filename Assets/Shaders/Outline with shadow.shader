
Shader "Custom/Toon" {
	Properties {
		_Colour ("Colour", Color) = (1,1,1,1)
		_AmbientColour("Ambient Colour", Color)= (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "black" {}
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0
		_OutLineColour("Outline Colour",Color) = (0,0,0,1)
		_OutLineWidth("Outline width", Range(0,1)) = .07
		_Threshold("Cel Threshold", Range(0,20)) = 5
		_Ambient("Ambient Intensity", Range(0,0.5)) = 0.1
		[MaterialToggle] _Is2D("Is 2D?", Float) = 0
	}
		CGINCLUDE
#include "UnityCG.cginc"
			// 3.) Reference the Unity library that includes all the lighting shadow macros
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
		
		};
		float _OutLineWidth;
		float4 _OutLineColour;
		float _Threshold;
		float _Is2D;
		float4 _AmbientColour;
		float LightToonShading(float3 normal, float3 lightDir)
		{
			
			float NdotL = ((/*max(0.0,*/ dot(normalize(normal), normalize(lightDir))));
			float temp = NdotL *_Threshold;
			return floor(temp) / (_Threshold - 0.5);
		}
		sampler2D _MainTex;
		float4 _MainTex_ST;
		v2f vert(appdata v)
		{
			v.vertex.xyz *= (_OutLineWidth/10)+1;
			v2f o;
			o.position = UnityObjectToClipPos(v.vertex);
			o.uv = TRANSFORM_TEX(v.uv, _MainTex);
			o.normal =  mul(v.normal.xyz,  unity_WorldToObject);
			return o;
		}
		ENDCG
			SubShader{
			//Tags { "RenderType" = "Opaque" }
			LOD 200
				Tags{ "Queue" = "Transparent" }
		
					Pass // render outline
				{
					ZWrite Off
					CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag 
					half4 frag(v2f i) : COLOR
				{
			
					return  _OutLineColour;
				}

					ENDCG
				}
			Pass //toonShader
				{
					CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#pragma multi_compile_fwdbase

				//	Tags{ "LightMode" = "ForwardBase" }

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
				//	LIGHTING_COORDS(1,2)
					//	TRANSFER_VERTEX_TO_FRAGMENT(o);
				    return o;
				}

				fixed4 _LightColor0;
				half _Ambient;

				fixed4 frag(v2f i) : SV_Target
				{
					
				
					fixed4 col = tex2D(_MainTex, i.uv)+_Colour ;
				if (_Is2D==0)
				{
					col.rgb *= (saturate(LightToonShading(i.normal, _WorldSpaceLightPos0.xyz) + _Ambient) * col);//_LightColor0.rgb);
				}
				else
				{
					col.rgb *= (saturate(i.normal + _Ambient))+_LightColor0.rgb;
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

	FallBack "VertexLit"
}
