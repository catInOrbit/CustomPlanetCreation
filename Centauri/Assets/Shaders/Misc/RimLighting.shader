Shader "Example/RimLighting" 
{
	Properties
	{
	  _MainTex("Texture", 2D) = "white" {}
	  _BumpMap("Bumpmap", 2D) = "bump" {}
	  _RimColor("Rim Color", Color) = (0.26,0.19,0.16,0.0)
	  _RimPower("Rim Power", Range(0.5,8.0)) = 3.0
	  _TintColor("Tint color", Color) = (1,1,1,1)
	}
		SubShader
	  {
		  Tags { "RenderType" = "Transparent" }

		  CGPROGRAM
		  #pragma surface VertexFunc Lambert
		  #pragma fragment frag alpha

		  #include "UnityCG.cginc"

		  struct Input 
		 {
			  float2 uv_MainTex;
			  float2 uv_BumpMap;
			  float3 viewDir;
		  };

		  struct v2f
		  {
			  float4 vertex  : SV_POSITION;
			  half2 texcoord : TEXCOORD0;
		  };

		  sampler2D _MainTex;
		  sampler2D _BumpMap;
		  float4 _RimColor;
		  float _RimPower;
		  float4 _TintColor;

		  void VertexFunc(Input IN, inout SurfaceOutput o) 
		  {
			  o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb;
			  o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
			  half rim = 1.0 - saturate(dot(normalize(IN.viewDir), o.Normal));
			  o.Emission = _RimColor.rgb * pow(rim, _RimPower);
			  o.Alpha = _TintColor.a;
		  }

		  fixed4 frag(v2f i) : SV_Target
		  {
			  fixed4 col = tex2D(_MainTex, i.texcoord) * _TintColor; // multiply by _Color
			  return col;
		  }

		 
		  ENDCG
	  }
		  Fallback "Diffuse"
}