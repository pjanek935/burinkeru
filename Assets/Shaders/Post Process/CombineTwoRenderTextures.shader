Shader "PostProcess/CombineTwoRenderTextures"
{
    Properties
    {
		_MainTex("Base (RGB)", 2D) = "white" {}

		_FrontTex("Base (RGB)", 2D) = "white" {}
		_BackTex("Base (RGB)", 2D) = "white" {}
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

			uniform sampler2D _MainTex;

			uniform sampler2D _FrontTex;
			uniform sampler2D _BackTex;

			struct Input
			{
				float4 pos : SV_POSITION;
				half2 uv   : TEXCOORD0;
			};

			float4 frag(Input i) : COLOR
			{
				half4 frontColor = tex2D(_FrontTex, i.uv);
				half4 backColor = tex2D(_BackTex, i.uv);
				half4 output = half4 (backColor.rgb * (1.0 - frontColor.a) + frontColor.rgb, backColor.a);

				return output;
			}

            ENDCG
        }
    }
}
