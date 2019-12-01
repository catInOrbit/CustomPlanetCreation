Shader "Unlit/SpecularMultipleLightPBR"
{

    Properties
    {
        _Color("Color", Color) = (1,0,0,1)
        _DiffuseTex("Texture", 2D) = "white"{}
        _Ambient("Ambient", Range(0,1)) = 0.25
        _SpecColor("Specular Material Color1", Color) = (1,1,1,1)
        _Shininess("Shininess", Float) = 10
    }

    SubShader
    {
        Tags{"LightMode" = "ForwardBase"}
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
             #include "UnityCG.cginc"
            #include "UnityLightingCommon.cginc"

            sampler2D _DiffuseTex;
            float4 _Color;
            float4 _DiffuseTex_ST;
            float _Ambient;
            float _Shininess;

            struct appdata
            {
                float4 vertex: POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 vertexWorld : TEXCOORD1;
                float3 worldNormal : TEXCOORD2;
            };

            v2f vert(appdata v)
            {
                v2f o;

                o.vertex = UnityObjectToClipPos(v.vertex);

                //vertexWorld = vertex in worldspace
                o.vertexWorld = mul(unity_ObjectToWorld, v.vertex);

                float3 worldNormal = UnityObjectToWorldNormal(v.normal);
                o.worldNormal = worldNormal;
                o.uv = TRANSFORM_TEX(v.uv, _DiffuseTex);
                return o;
            }

            float4 frag (v2f i) : SV_TARGET
            {
                float3 normalDir = normalize(i.worldNormal);
                float3 viewDir = normalize(UnityWorldSpaceViewDir(i.vertexWorld));
                float3 lightDirection = normalize(UnityWorldSpaceLightDir(i.vertexWorld));

                float4 tex = tex2D(_DiffuseTex, i.uv);
                float dotNormalLight = max(_Ambient, dot(normalDir, _WorldSpaceLightPos0.xyz));
                float4 diffuseTerm = dotNormalLight * _Color * tex *_LightColor0;

                float3 reflectionDirection = reflect(-lightDirection, normalDir);
                float3 specularDotProduct = max(0.0, dot(viewDir, reflectionDirection));
                float3 specular = pow(specularDotProduct, _Shininess);
                float4 specularTerm = float4(specular, 1) *_SpecColor * _LightColor0;

                float4 finalColor = diffuseTerm + specularTerm;

                return finalColor;
            }

            ENDCG
        }

         Pass
        {
            Tags{"LightMode" = "ForwardAdd"}
            Blend One One


            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fwdadd
             #include "UnityCG.cginc"
            #include "UnityLightingCommon.cginc"

            sampler2D _DiffuseTex;
            float4 _Color;
            float4 _DiffuseTex_ST;
            float _Ambient;
            float _Shininess;

            struct appdata
            {
                float4 vertex: POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 vertexWorld : TEXCOORD1;
                float3 worldNormal : TEXCOORD2;
            };

            v2f vert(appdata v)
            {
                v2f o;

                o.vertex = UnityObjectToClipPos(v.vertex);

                //vertexWorld = vertex in worldspace
                o.vertexWorld = mul(unity_ObjectToWorld, v.vertex);

                float3 worldNormal = UnityObjectToWorldNormal(v.normal);
                o.worldNormal = worldNormal;
                o.uv = TRANSFORM_TEX(v.uv, _DiffuseTex);
                return o;
            }

            float4 frag (v2f i) : SV_TARGET
            {
                float3 normalDir = normalize(i.worldNormal);
                float3 viewDir = normalize(UnityWorldSpaceViewDir(i.vertexWorld));
                float3 lightDirection = normalize(UnityWorldSpaceLightDir(i.vertexWorld));

                float4 tex = tex2D(_DiffuseTex, i.uv);
                float dotNormalLight = max(_Ambient, dot(normalDir, _WorldSpaceLightPos0.xyz));
                float4 diffuseTerm = dotNormalLight * _Color * tex *_LightColor0;

                float3 reflectionDirection = reflect(-lightDirection, normalDir);
                float3 specularDotProduct = max(0.0, dot(viewDir, reflectionDirection));
                float3 specular = pow(specularDotProduct, _Shininess);
                float4 specularTerm = float4(specular, 1) *_SpecColor * _LightColor0;

                float4 finalColor = diffuseTerm + specularTerm;

                return finalColor;
            }

            ENDCG
        }
    }
}