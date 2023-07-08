Shader "Hidden/CustomPostEffect/DualKawaseBlur"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" { }
	}

	HLSLINCLUDE
	#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
	#include "DualKawaseBlur.hlsl"
	ENDHLSL

	SubShader
	{
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			HLSLPROGRAM
			#pragma vertex Vert_DownSample
			#pragma fragment Frag_DownSample
			ENDHLSL
		}

		Pass
		{
			HLSLPROGRAM
			#pragma vertex Vert_UpSample
			#pragma fragment Frag_UpSample
			ENDHLSL
		}
	}
}