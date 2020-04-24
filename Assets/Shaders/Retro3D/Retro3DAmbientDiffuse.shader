Shader "Retro3D/AmbientDiffuse"
{
	Properties
	{
		_MainTex("Base", 2D) = "white" {}
		_GeoRes("Geometric Resolution", Float) = 40
		_TintColor("Tint Color", Color) = (1, 1, 1, 1)
	}

		SubShader
		{
			Pass
			{
				Tags { "LightMode" = "ForwardBase" }

				CGPROGRAM

				#include "UnityCG.cginc"

				#pragma vertex vert
				#pragma fragment frag

				struct vertOutput
				{
					float4 position : SV_POSITION;
					float3 texcoord : TEXCOORD;
					half4 color : COLOR;
				};

				sampler2D _MainTex;
				float4 _MainTex_ST;
				float _GeoRes;
				uniform float4 _LightColor0;
				float3 _TintColor;
				float _LightIntensity;

				vertOutput vert(appdata_base v)
				{
					vertOutput o;

					float3 n = normalize(mul(v.normal, unity_WorldToObject));
					float3 l = normalize(_WorldSpaceLightPos0);
					float NdotL = max(0.0, dot(n, l));
					float3 color = NdotL * _LightColor0.rgb + _TintColor;
					o.color = half4 (color, 1.0);

					float4 wp = mul(UNITY_MATRIX_MV, v.vertex);
					wp.xyz = floor(wp.xyz * _GeoRes) / _GeoRes;

					float4 sp = mul(UNITY_MATRIX_P, wp);
					o.position = sp;

					float2 uv = TRANSFORM_TEX(v.texcoord, _MainTex);
					o.texcoord = float3(uv * sp.w, sp.w);

					return o;
				}

				fixed4 frag(vertOutput vo) : SV_Target
				{
					float2 uv = vo.texcoord.xy / vo.texcoord.z;
					return tex2D(_MainTex, uv)  * vo.color;
				}

				ENDCG
			}


		}

			Fallback "VertexLit"
}