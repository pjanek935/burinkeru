Shader "Retro3D/Unlit"
{
	Properties
	{
		_MainTex("Base", 2D) = "white" {}
		_GeoRes("Geometric Resolution", Float) = 40
		_RampTex("Ramp Texture", 2D) = "white" {}
	}

		SubShader
	{
		Pass
		{

			CGPROGRAM

			#include "UnityCG.cginc"

			#pragma vertex vert
			#pragma fragment frag

			struct v2f
			{
				float4 pos : SV_POSITION;
				float3 texcoord : TEXCOORD;
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

				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				float2 uv = i.texcoord.xy / i.texcoord.z;
				return tex2D(_MainTex, uv);
			}

			ENDCG
		}

	}
}