#ifndef DUALKAWASEBLUR_HLSL
#define DUALKAWASEBLUR_HLSL

TEXTURE2D(_MainTex);
SAMPLER(sampler_MainTex);
float4 _MainTex_TexelSize;
uniform half _Offset;

struct appdata
{
	float4 vertex: POSITION;
	float2 uv: TEXCOORD0;
};

struct v2f
{
	float4 pos: SV_POSITION;
	float2 uv: TEXCOORD0;
};

struct v2f_DownSample
{
	float4 vertex: SV_POSITION;
	float2 texcoord: TEXCOORD0;
	float2 uv: TEXCOORD1;
	float4 uv01: TEXCOORD2;
	float4 uv23: TEXCOORD3;
};

struct v2f_UpSample
{
	float4 vertex: SV_POSITION;
	float2 texcoord: TEXCOORD0;
	float4 uv01: TEXCOORD1;
	float4 uv23: TEXCOORD2;
	float4 uv45: TEXCOORD3;
	float4 uv67: TEXCOORD4;
};

v2f_DownSample Vert_DownSample(appdata v)
{
	v2f_DownSample o;
	o.vertex = TransformObjectToHClip(v.vertex.xyz);
	o.texcoord = v.uv;
	float2 uv = v.uv;

	float4 texelSize = _MainTex_TexelSize * 0.5;
	float2 offset = float2(1 + _Offset, 1 + _Offset) * _Offset;

	o.uv = uv;
	o.uv01.xy = uv - texelSize.xy * offset;
	o.uv01.zw = uv + texelSize.xy * offset;
	o.uv23.xy = uv - float2(texelSize.x, -texelSize.y) * offset;
	o.uv23.zw = uv + float2(texelSize.x, -texelSize.y) * offset;

	return o;
}

half4 Frag_DownSample(v2f_DownSample i) : SV_Target
{
	half4 sum = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv) * 4;
	sum += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv01.xy);
	sum += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv01.zw);
	sum += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv23.xy);
	sum += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv23.zw);

	return sum * 0.125;
}

v2f_UpSample Vert_UpSample(appdata v)
{
	v2f_UpSample o;
	o.vertex = TransformObjectToHClip(v.vertex.xyz);
	o.texcoord = v.uv;
	float2 uv = v.uv;

	float4 texelSize = _MainTex_TexelSize * 0.5;
	float2 offset = float2(1 + _Offset, 1 + _Offset) * _Offset;

	o.uv01.xy = uv + float2(-texelSize.x * 2, 0) * offset;
	o.uv01.zw = uv + float2(-texelSize.x, texelSize.y) * offset;
	o.uv23.xy = uv + float2(0, texelSize.y * 2) * offset;
	o.uv23.zw = uv + texelSize.xy * offset;
	o.uv45.xy = uv + float2(texelSize.x * 2, 0) * offset;
	o.uv45.zw = uv + float2(texelSize.x, -texelSize.y) * offset;
	o.uv67.xy = uv + float2(0, -texelSize.y * 2) * offset;
	o.uv67.zw = uv - texelSize.xy * offset;

	return o;
}

half4 Frag_UpSample(v2f_UpSample i) : SV_Target
{
	half4 sum = 0;
	sum += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv01.xy);
	sum += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv01.zw) * 2;
	sum += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv23.xy);
	sum += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv23.zw) * 2;
	sum += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv45.xy);
	sum += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv45.zw) * 2;
	sum += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv67.xy);
	sum += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv67.zw) * 2;

	return sum * 0.0833;
}
#endif