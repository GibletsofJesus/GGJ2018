Shader "Custom/ToonSurface"
{
	Properties{
		_Color("Main Color", Color) = (1,1,1,1)
		_MainTex("Texture", 2D) = "white" {}
	_BumpMap("Normalmap", 2D) = "bump" {}
	_Ramp("Shading Ramp", 2D) = "gray" {}
//	_ReflectColor("Reflection Color", Color) = (1,1,1,0.5)
//	_Cube("Reflection Cubemap", Cube) = "_Skybox" { TexGen CubeReflect }
	}

		SubShader{
			Tags{ "RenderType" = "Opaque" }
			//Added Double sided normal support
		//	Cull off
			CGPROGRAM
		#pragma surface surf Ramp

			sampler2D _Ramp;

		half4 LightingRamp(SurfaceOutput s, half3 lightDir, half atten) {
			//Reduced some lines of code and added better shadow support
			half d = (dot(s.Normal, lightDir)*0.5 + 0.5) * atten;
			half3 ramp = tex2D(_Ramp, float2(d,d)).rgb;

			half4 c;
			c.rgb = s.Albedo * _LightColor0.rgb * ramp * (atten * 2);
			c.a = s.Alpha;
			return c;
		}

		fixed4 _Color;
		fixed4 _ReflectColor;

		struct Input {
			float2 uv_MainTex;
			float2 uv_BumpMap;
//			float3 worldRefl;
			INTERNAL_DATA
		};

		sampler2D _MainTex;
		sampler2D _BumpMap;
		samplerCUBE _Cube;

		void surf(Input IN, inout SurfaceOutput o) {
			fixed4 tex = tex2D(_MainTex, IN.uv_MainTex);
			fixed4 c = tex * _Color;
			o.Albedo = c.rgb;

			o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));

	/*	float3 worldRefl = WorldReflectionVector(IN, o.Normal);
		fixed4 reflcol = texCUBE(_Cube, worldRefl);
		reflcol *= tex.a;
		o.Emission = reflcol.rgb * _ReflectColor.rgb;
		o.Alpha = reflcol.a * _ReflectColor.a;*/
		}

		ENDCG

	}

		Fallback "Diffuse Bumped"
}