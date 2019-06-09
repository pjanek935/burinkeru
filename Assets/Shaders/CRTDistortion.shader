Shader "Custom/CRTDistortion"
{
	Properties
	{
		_MainTex("Base (RGB)", 2D) = "white" {}
		_DisplacementTex("Displacement Texture", 2D) = "white" {}
		_Strength("Distortion Strength", Float) = 0.4
	}

		SubShader
		{
			Pass
			{
				CGPROGRAM
				#pragma vertex vert_img
				#pragma fragment frag
				#include "UnityCG.cginc"

				uniform sampler _MainTex;
				uniform sampler2D _MaskTex;

				fixed _maskBlend;
				fixed _maskSize;

				uniform sampler2D _DisplacementTex;
				half _Strength;

				fixed4 frag(v2f_img i) : COLOR
				{
					half2 n = tex2D(_DisplacementTex, i.uv);
					half2 d = n * 2 - 1;
					i.uv += d * _Strength;
					i.uv = saturate(i.uv);

					float4 c = tex2D(_MainTex, i.uv);
					return c;
				}

				ENDCG
			}
		}
}
