// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Retro3D/LightResoulution"
{
	Properties
	{
		_MainTex("Base", 2D) = "white" {}
		_GeoRes("Geometric Resolution", Float) = 40
		_AmbientColor("Ambient Color", Color) = (1, 1, 1, 1)
		_LightRes("Light Resolution", Float) = 4
	}

		SubShader
		{
			Pass
			{
				Tags { "LightMode" = "ForwardBase" }

				CGPROGRAM

				#include "UnityCG.cginc"
				#include "AutoLight.cginc"

				#pragma vertex vert
				#pragma fragment frag
				#pragma multi_compile_fwdbase

				struct vertOutput
				{
					float4 pos : SV_POSITION;
					float3 texcoord : TEXCOORD;
					half4 color : COLOR;
					float2 ScreenPos : TEXCOORD1;
					LIGHTING_COORDS(3, 4)
				};

				sampler2D _MainTex;
				float4 _MainTex_ST;
				float _GeoRes;
				uniform float4 _LightColor0;
				float4 _AmbientColor;
				float _LightRes;

				vertOutput vert(appdata_base v)
				{
					vertOutput o;


					float4 wp = mul(UNITY_MATRIX_MV, v.vertex);
					wp.xyz = floor(wp.xyz * _GeoRes) / _GeoRes;

					float4 sp = mul(UNITY_MATRIX_P, wp);
					o.pos = sp;

					float3 n = normalize(mul(v.normal, unity_WorldToObject));
					float3 l = normalize(_WorldSpaceLightPos0);
					float NdotL = max(0.0, dot(n, l));
					NdotL = floor(NdotL * _LightRes) / _LightRes;
					float3 color = NdotL * _LightColor0.rgba + _AmbientColor;
					o.color = half4 (color, 1.0);

					float2 uv = TRANSFORM_TEX(v.texcoord, _MainTex);
					o.texcoord = float3(uv * sp.w, sp.w);

					o.ScreenPos = ComputeScreenPos(UnityObjectToClipPos(v.vertex));

					TRANSFER_VERTEX_TO_FRAGMENT(o);
					return o;
				}

				fixed4 frag(vertOutput vo) : SV_Target
				{
					float2 uv = vo.texcoord.xy / vo.texcoord.z;
					float atten = LIGHT_ATTENUATION(vo);
					atten = floor(atten * _LightRes) / _LightRes;
					atten = (( atten) * float4 (1,1, 1, 1));
					
					
					return tex2D(_MainTex, uv)  * vo.color;
				}

				ENDCG
			}

		}

			Fallback "VertexLit"
}