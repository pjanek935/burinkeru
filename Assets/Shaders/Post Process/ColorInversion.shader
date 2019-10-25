Shader "PostProcess/ColorInversion"
{
	Properties
	{
		_MainTex("Base (RGB)", 2D) = "white" {}
		_Strength("Distortion Strength", Float) = 0.001
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
				fixed _Strength;

				fixed4 frag(v2f_img i) : COLOR
				{
					fixed4 base = tex2D(_MainTex, i.uv);

					return _Strength * (1 - base) + (1 - _Strength) * base;
				}

				ENDCG
			}
		}
}
