Shader "Custom/CustomFresnel"
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
        Pass
        {
            Tags
            { 
    "Queue" = "Transparent" 
    "IgnoreProjector" = "True" 
    "RenderType" = "Transparent" 
            }

            
        CGPROGRAM
// Upgrade NOTE: excluded shader from DX11; has structs without semantics (struct appdata members uv_MainTex,worldNormal,viewDir,worldPos)
#pragma exclude_renderers d3d11
        #pragma vertex vert            
        #pragma fragment frag            
        #include "UnityCG.cginc"            
        #include "UnityLightingCommon.cginc"

        
        sampler2D _MainTex;
        float _DotProduct; 
        float3 _Center;
        float4 _CircleColor;
        float _Radius;

        struct appdata
        {
              float2 uv_MainTex; 
             float3 worldNormal : ; 
             float3 viewDir;
            float3 worldPos;
        }

        v2f vert(appdata v)
        {
            v2f o;

        }





        ENDCG
        }
    }