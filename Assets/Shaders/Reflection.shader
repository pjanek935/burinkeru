// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Reflection"
{
	Properties{
		_MainTex("Main Texture", 2D) = "white" {}
		_OffsetTex("OffsetTexture", 2D) = "black" {}
		_AmbientTex("Ambient Reflection Texture", 2D) = "black" {}
	}

		SubShader{

		Tags{
			"Queue" = "Transparent"
			"PreviewType" = "Plane"
			"DisableBatching" = "True"
		}

		Pass{

		ZWrite Off
		Cull Off
		Blend SrcAlpha OneMinusSrcAlpha

		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag

		#include "UnityCG.cginc"

	struct appdata
	{
		float4 vertex : POSITION;
		float2 uv : TEXCOORD0;
		half4 color : COLOR;
	};

	struct v2f
	{
		float4 vertex : SV_POSITION;
		float2 uv : TEXCOORD0;
		float2 screenuv : TEXCOORD1;
		half4 color : COLOR;
	};

	v2f vert(appdata v)
	{
		v2f o;
		o.vertex = UnityObjectToClipPos(v.vertex);
		o.uv = v.uv;
		o.screenuv = ((o.vertex.xy / o.vertex.w) + 1) * 0.5;
		o.color = v.color;
		return o;
	}

	sampler2D _MainTex;

	uniform sampler2D _GlobalRefractionTex;
	uniform sampler2D _OffsetTex;
	uniform sampler2D _AmbientTex;

	float4 frag(v2f i) : SV_Target
	{
		float4 color = tex2D(_MainTex, i.uv) * i.color;
		float2 offset = mul(unity_ObjectToWorld, tex2D(_OffsetTex, i.uv).xy * 2 - 1);
		float4 ambient = tex2D(_AmbientTex, (i.screenuv + offset * 0.008 * 5) * 2);

		float4 br = tex2D(_GlobalRefractionTex, i.screenuv + offset * 0.03);

		color.rgb = (color.rgb + ambient.rgb) * (1.0 - br.a * 0.1) + br.rgb * br.a * 0.1;

		return color;
	}
		ENDCG
		}
	}
}
