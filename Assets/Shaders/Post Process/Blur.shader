Shader "PostProcess/Blur"
{
	Properties
	{
		_MainTex("Base (RGB)", 2D) = "white" {}
		_BlurOffset ("Blur Offset", Float) = 4
	}

		SubShader
		{
			Pass
			{

			CGPROGRAM

			#pragma vertex vert_img
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest

			#include "UnityCG.cginc"

			struct Input
			{		
				float4 pos : SV_POSITION;
				half2 uv   : TEXCOORD0;
			};

			uniform sampler2D _MainTex;
			uniform float _BlurOffset;

			float4 frag(Input i) : COLOR
			{
				float2 coords = i.uv.xy;
				float4 worldPos = mul(unity_ObjectToWorld, i.pos);

				_BlurOffset /= 300.0f;
				float4 colorSum = (0, 0, 0, 0);
				float index = 0;

				for (int x = -1; x <= 1; x++)
				{
					for (int y = -1; y <= 1; y++)
					{
						colorSum += tex2D(_MainTex, coords.xy + float2 (x * _BlurOffset, y * _BlurOffset));
						index++;
					}
				}

				colorSum /= index;

				return colorSum;
			}

			ENDCG
		}
	}
}