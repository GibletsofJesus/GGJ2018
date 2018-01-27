Shader "Custom/Spotlight"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Radius("Radius", Range(0,20)) = 10
		_BlendSize("Blend Size", Range(0,10)) = 5
		_Tint("Shadow Colour", Color) = (0,0,0,1)
		[MaterialToggle] _Flick("Flicker?", Float) = 0
		_Flicker("Flicker Frequency", Range(1,20)) = 20
		_Min("Flicker Intensity", Range(0,1)) = 0
		_LightColour("Light Colour", Color) = (0,0,0,0)
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
// Upgrade NOTE: excluded shader from DX11, OpenGL ES 2.0 because it uses unsized arrays
#pragma exclude_renderers d3d11 gles
			#pragma vertex vert
			#pragma fragment frag
#pragma multi_compile_fwdbase
			
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
				float3 worldPos : TEXCOORD1;
				float dist : TEXCOORD2;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float4 _TargetPosition[50];
				float _Radius;
				float _BlendSize;
				float4 _Tint;
				float _Flick;
				float _Flicker;
				float _Min;
				float4 _LightColour;
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);

				o.dist = distance(mul(unity_ObjectToWorld, v.vertex).xyz, _TargetPosition[0].xyz);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col =0;
			float sinRad = 1;
			half4 lightCol =  _LightColour - (i.dist - _Radius)/10;
			if (i.dist < _Radius)
			{
				col = tex2D(_MainTex, i.uv) * lightCol;
			}
			else if (i.dist > _Radius && i.dist < _Radius + _BlendSize)
			{
				if (_Flick > 0)
				{
					sinRad = clamp(sin(_BlendSize + ((_CosTime.w)*_Flicker)), 1 - _Min, 1);

				}
				float blend = (i.dist - _Radius);//*sinRad;
				
				col = lerp(tex2D(_MainTex, i.uv)*_LightColour, _Tint,blend/(_BlendSize*sinRad));// *blend);
			}
			else
				col = _Tint;
				return col;
			}
			ENDCG
		}
			
	}
			Fallback "VertexLit"

}
