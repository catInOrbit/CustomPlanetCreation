Shader "Custom/Fresnel"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _DotProduct("Rim effect", Range(-1,1)) = 0.25 
         _Center("Center", Vector) = (100,0,100,0)
        _CircleColor("Circle color", Color) = (0,0,0,0)
        _Radius("Radius", Float) = 100
    }
    SubShader
    {
        Cull Off
        Tags 
{ 
  "Queue" = "Transparent" 
  "IgnoreProjector" = "True" 
  "RenderType" = "Transparent" 
} 

        CGPROGRAM
        
        #pragma surface surf Lambert alpha:fade nolighting 

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0
        

        sampler2D _MainTex;
        float _DotProduct; 
        float3 _Center;
        float4 _CircleColor;
        float _Radius;
        uniform float screenUV;

        struct Input 
        { 
            float2 uv_MainTex; 
            float3 worldNormal; 
            float3 viewDir;
            float3 worldPos;
            float3 screenPos;
        }; 

        fixed4 _Color;

        void surf (Input IN, inout SurfaceOutput o)
        {
             float4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color; 
           
            float d = distance(_Center, IN.worldPos);
              float border = 1 - (abs(dot(IN.viewDir, IN.worldNormal))); 
            float alpha = (border * (1 - _DotProduct) + _DotProduct); 

            if(d > _Radius)
            {
                 o.Albedo = _Color;
                 o.Alpha = c.a * alpha; 
                 o.Emission = _Color.rgb * pow(alpha, _Color);
            }

            else
            {
                o.Albedo = _CircleColor;
                 o.Alpha = _CircleColor.a; 
            }
        }
        ENDCG
    }
}
