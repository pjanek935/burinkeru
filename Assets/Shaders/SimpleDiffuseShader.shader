Shader "Example/SimpleDiffuseShader"
{
	SubShader
	{
		Tags { "RenderType" = "Opaque" }
		CGPROGRAM
		#pragma surface surf Lambert

		struct Input
		{
			float4 color : COLOR;
		};

		void surf(Input IN, inout SurfaceOutput o)
		{
			o.Albedo = fixed3(1, 1, 0);
		}

		ENDCG
	}
		Fallback "Diffuse"
}