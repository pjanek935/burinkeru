Shader "PostProcess/CRTDistortion"
{
	Properties
	{
		_MainTex("Base (RGB)", 2D) = "white" {}
		_DisplacementTex("Displacement Texture", 2D) = "white" {}
		_Strength("Distortion Strength", Float) = 0.001
		_MaskSize("Mask Size", Float) = 1
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
				fixed _MaskSize;

				uniform sampler2D _DisplacementTex;
				fixed _Strength;

				fixed4 frag(v2f_img i) : COLOR
				{
					fixed n = tex2D(_DisplacementTex, i.uv * _MaskSize);
					fixed d = step(0.1, n);
					fixed2 co = i.uv;
					co.x += 0.01 * _Strength * d;

					return tex2D(_MainTex, co);
				}

				ENDCG
			}
		}
}
