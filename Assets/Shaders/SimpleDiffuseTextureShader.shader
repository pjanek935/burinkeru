Shader "Example/SimpleDiffuseTextureShader"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
	}

	SubShader
	{
		Tags { "RenderType" = "Opaque" }
		CGPROGRAM
		#pragma surface surf Lambert

		struct Input
		{
			float2 uv_MainTex;
			float3 worldPos;
		};

		sampler2D _MainTex;

		void surf(Input IN, inout SurfaceOutput o)
		{
			float d = step(0, IN.worldPos.y);
			o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb * d + ((1 - d) * (tex2D(_MainTex, IN.uv_MainTex).rgb - half3 (0.5, 0.5, 0.5)));
		}

		ENDCG
	}
		Fallback "Diffuse"
}