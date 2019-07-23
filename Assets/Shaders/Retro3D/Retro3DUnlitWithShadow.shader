Shader "Retro3D/UnlitWithShadow"
{
	Properties
	{
		_MainTex("Base", 2D) = "white" {}
		_GeoRes("Geometric Resolution", Float) = 40
	}

		SubShader
	{
		Pass
		{
			Tags { "LightMode" = "ForwardBase" }

			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			#pragma multi_compile_fwdbase
			#include "AutoLight.cginc"

			struct v2f
			{
				float4 pos : SV_POSITION;
				float3 texcoord : TEXCOORD;
				SHADOW_COORDS(1)
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float _GeoRes;

			v2f vert(appdata_base v)
			{
				v2f o;

				float4 wp = mul(UNITY_MATRIX_MV, v.vertex);
				wp.xyz = floor(wp.xyz * _GeoRes) / _GeoRes;

				float4 sp = mul(UNITY_MATRIX_P, wp);
				o.pos = sp;

				float2 uv = TRANSFORM_TEX(v.texcoord, _MainTex);
				o.texcoord = float3(uv * sp.w, sp.w);

				TRANSFER_SHADOW(o);

				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				float attenuation = SHADOW_ATTENUATION(i);
				float2 uv = i.texcoord.xy / i.texcoord.z;
				return tex2D(_MainTex, uv) * attenuation;
			}

			ENDCG
		}

			Pass {
				Name "ShadowCaster"
				Tags { "LightMode" = "ShadowCaster" }

				ZWrite On ZTest LEqual

				CGPROGRAM
				#pragma target 2.0

				#pragma multi_compile_shadowcaster

				#pragma vertex vertShadowCaster
				#pragma fragment fragShadowCaster

				#include "UnityStandardShadow.cginc"

				ENDCG
			}
	}

			Fallback "VertexLit"
}