Shader "PostProcess/BlinkShade"
{
	Properties
	{
		_MainTex("Base (RGB)", 2D) = "white" {}
		_DisplacementTex("Displacement Texture", 2D) = "white" {}
		_DisplacementTex2("Displacement Texture2", 2D) = "white" {}
		_LineTex("Line Texture", 2D) = "white" {}
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
				uniform sampler2D _DisplacementTex;
				uniform sampler2D _DisplacementTex2;
				uniform sampler2D _LineTex;
				fixed _Strength;

				fixed4 frag(v2f_img i) : COLOR
				{
					fixed n = (1.0 - tex2D(_DisplacementTex, i.uv)) * _Strength;
					fixed3 vec = tex2D(_DisplacementTex2, i.uv);
					vec -= fixed3(0.5, 0.5, 0.5);
					vec *= 0.5 * _Strength;
					fixed4 base = tex2D(_MainTex, i.uv - vec);

					fixed lines = tex2D(_LineTex, i.uv);
					
					fixed4 color1 = n * (1 - base);
					fixed4 color2 = (1 - n * n) * base;
					color1.b /= 2;
					fixed4 result = color1 + color2;
					result -= (lines / 20) * (_Strength);

					return result;
				}

				ENDCG
			}
		}
}
