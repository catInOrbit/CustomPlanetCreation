Shader "Custom/CircleShader"
{
    Properties
    {
        _Center("Center", Vector) = (100,0,100,0)
        _Radius("Radius", Float) = 100
        _InnerColor("Inner color", Color) = (0,0,0,0)
        _RadiusWidth("Radius Width", Float) = 10
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
    }

    SubShader
    {
        Tags
        {
            "RenderType"="Opaque"
        }

        CGPROGRAM

           #pragma surface surf Standard fullforwardshadows        
           #pragma target 3.0

            sampler2D _MainTex;
            uniform float3 _Center;
            uniform float _Radius;
            uniform float4 _InnerColor;
            uniform float _RadiusWidth;
            
            struct Input
            {
                //UV texture from outside circle
                float2 uv_MainTex;

                float3 worldPos;
            };

            void surf (Input IN, inout SurfaceOutputStandard o)
            {
                float d = distance(_Center, IN.worldPos);

                if((d > _Radius) && (d<(_Radius + _RadiusWidth)))
                {
                    o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb;
                }

                else
                {
                    o.Albedo = _InnerColor;
                }
            }

        ENDCG
    }
}