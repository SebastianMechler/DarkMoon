Shader "FX/GrayScale" 
{
	Properties
	{
		_MainTex("Base (RGB) Trans (A)", 2D) = "white" {}
		_EffectAmount("Effect Amount", Range(0, 1)) = 1.0
	}

	SubShader
	{
		Tags { "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
		LOD 200
		Blend SrcAlpha OneMinusSrcAlpha

	Pass
	{
		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		#include "UnityCG.cginc"

		struct appdata_t 
		{
			float4 vertex : POSITION;
			float2 texcoord : TEXCOORD0;
		};

		struct v2f 
		{
			float4 vertex : SV_POSITION;
			half2 texcoord : TEXCOORD0;
		};

		sampler2D _MainTex;
		float4 _MainTex_ST;
		uniform float _EffectAmount;

		// Vertex shader
		v2f vert(appdata_t v)
		{
			v2f o;
			o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
			o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
			return o;
		}

		// Fragment shader
		fixed4 frag(v2f i) : SV_Target
		{
			fixed4 color = tex2D(_MainTex, i.texcoord);
			color.rgb = lerp(color.rgb, dot(color.rgb, float3(0.3, 0.59, 0.11)), _EffectAmount);
			return color;
		}
			ENDCG
		}
	}
	Fallback "Standard"
}