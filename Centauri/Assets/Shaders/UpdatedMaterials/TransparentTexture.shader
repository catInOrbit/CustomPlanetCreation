Shader "Custom/TransparentTexture"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
    }
    SubShader
    {
        Tags 
{ 
  "Queue" = "Transparent" 
  "IgnoreProjector" = "True" 
  "RenderType" = "Transparent" 
} 

        CGPROGRAM
        
        #pragma surface surf Standard alpha:fade nolighting

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0
        sampler2D _MainTex;
        fixed4 _Color;

        struct Input
        {
            float2 uv_MainTex;
        };

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            float4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color; 
            o.Albedo = c.rgb;
            o.Alpha = c.a;
            o.Emission = _Color.a;
        }
        ENDCG
    }
}
