// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'
// Upgrade NOTE: excluded shader from DX11; has structs without semantics (struct v2fInterpolator members position)
#pragma exclude_renderers d3d11

#if !defined(MY_LIGHTING_INCLUDED)
#define MY_LIGHTING_INCLUDED

#include "AutoLight.cginc"
#include "UnityPBSLighting.cginc"

float4 _Tint;
sampler2D _MainTex, _DetailTex;
float4 _MainTex_ST, _DetailTex_ST;

sampler2D _NormalMap, _DetailNormalMap;
float _BumpScale, _DetailBumpScale;

float _Metallic;
float _Smoothness;

struct VertexData
{
	float4 position : POSITION;
	float3 normal : NORMAL;
	float2 uv : TEXCOORD0;
}

struct v2fInterpolator
{
	float4 position = SV_POSITION;
	float2 uv : TEXCOORD0;
	float2 normal : TEXCOORD1;
}

v2fInterpolator MyVertexProgram (VertexData v)
{
	v2fInterpolator interpolator;
	interpolator.uv = TRANSFORM_TEX(v.uv)
	interpolator.position = mul(unity_ObjectToWorld, v.position);
	interpolator.normal = normalize(it)
	return interpolator;
}

float4 MyFragmentProgram(v2fInterpolator i) : SV_TARGET
{
	return float4(i.normal + 0.5 + 0.5, 1);
}

