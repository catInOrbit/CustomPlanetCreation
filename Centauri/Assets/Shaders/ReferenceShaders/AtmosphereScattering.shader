Shader "Cusom/AstmosphericScattering"
{
    Properties
    {
        _Color("Color", Color) = (0,0,0,0)
        _MainTex("Aldebo", 2D) = "white" {}
        _Glossiness("Smoothness", Range(0,1)) = 0.5
        _Metallic("Metallic", Range(0,1)) = 0.0
        _AtmosphereRadius("Atmosphere", float) = 0
        _PlanetRadius("Atmosphere", float) = 0

    }

    SubShader
    {
        Tags{"RenderType" = "Opage"}
        LOD 200
        Pass
        {
            CGPROGRAM
            #pragma vertex:vert

            float _AtmosphereRadius;
            float _PlanetRadius;
            
            struct Input
            {
                float2 uv_MainTex;
                float3 worldPos;
                float3 center;
                
            };

            void vert(inout appdata_full v, out Input o)    
            {
                UNITY_INITIALIZE_OUTPUT(Input, o);
                v.vertex.xyz += v.normal * (_AtmosphereRadius - _PlanetRadius);
                o.center = mul(unity_ObjectToWorld, half4(0,0,0,1));
            }

            ENDCG
        }
            

        Tags{"RenderType" = "Transparent" "Queue"="Transparent"}
        LOD 200

        Pass
        {
            Cull Back
            Blend One One

            CGPROGRAM

            #include "UnityPBSLighting.cginc"
            inline fixed4 LightingStandardScattering(SurfaceOutputStandard s, fixed3 viewDir, UnityGI gi)
            {
                float3 L = gi.light.dir;
                float3 V = viewDir;
                float3 N = s.Normal;

                float3 S = L;
                float3 D = -V;

                bool rayIntersect
                (
                    float3 O,
                    float3 D,

                    float3 C,
                    float3 R,

                    out float AO,
                    out float BO
                )

                {
                    float3 L = C - O;
                    float DT = dot (L, D);
                    float R2 = R * R;
                    
                    float CT2 = dot(L,L) - DT*DT;
                    
                    // Intersection point outside the circle
                    if (CT2 > R2)
                    return false;
                    
                    float AT = sqrt(R2 - CT2);
                    float BT = AT;
                    
                    AO = DT - AT;
                    BO = DT + BT;
                    return true;
                }

                // Intersections with the atmospheric sphere
                float tA;	// Atmosphere entry point (worldPos + V * tA)
                float tB;	// Atmosphere exit point  (worldPos + V * tB)
                if (!rayIntersect(O, D, _PlanetCentre, _AtmosphereRadius, tA, tB))
                    return fixed4(0,0,0,0); // The view rays is looking into deep space

                // Is the ray passing through the planet core?
                float pA, pB;
                if (rayIntersect(O, D, _PlanetCentre, _PlanetRadius, pA, pB))
                    tB = pA;

                // Numerical integration to calculate
                // the light contribution of each point P in AB
                float3 totalViewSamples = 0;
                float time = tA;
                float ds = (tB-tA) / (float)(_ViewSamples);
                for (int i = 0; i < _ViewSamples; i ++)
                {
                    // Point position
                    // (sampling in the middle of the view sample segment)
                    float3 P = O + D * (time + ds * 0.5);

                    // T(CP) * T(PA) * ρ(h) * ds
                    totalViewSamples += viewSampling(P, ds);

                    time += ds;
                }

                // I = I_S * β(λ) * γ(θ) * totalViewSamples
                float3 I = _SunIntensity *  _ScatteringCoefficient * phase * totalViewSamples;

                float opticalDepthPA = 0;

                // Numerical integration to calculate
                // the light contribution of each point P in AB
                float time = tA;
                float ds = (tB-tA) / (float)(_ViewSamples);
                for (int i = 0; i < _ViewSamples; i ++)
                {
                    // Point position
                    // (sampling in the middle of the view sample segment)
                    float3 P = O + D * (time + viewSampleSize*0.5);

                    // Optical depth of current segment
                    // ρ(h) * ds
                    float height = distance(C, P) - _PlanetRadius;
                    float opticalDepthSegment = exp(-height / _ScaleHeight) * ds;

                    // Accumulates the optical depths
                    // D(PA)
                    opticalDepthPA += opticalDepthSegment;

                    bool lightSampling
                    (	float3 P,	// Current point within the atmospheric sphere
                        float3 S,	// Direction towards the sun
                        out float opticalDepthCA
                    )
                    {
                        float _; // don't care about this one
                        float C;
                        rayInstersect(P, S, _PlanetCentre, _AtmosphereRadius, _, C);

                        // Samples on the segment PC
                        float time = 0;
                        float ds = distance(P, P + S * C) / (float)(_LightSamples);
                        for (int i = 0; i < _LightSamples; i ++)
                        {
                            float3 Q = P + S * (time + lightSampleSize*0.5);
                            float height = distance(_PlanetCentre, Q) - _PlanetRadius;
                            // Inside the planet
                            if (height < 0)
                                return false;

                            // Optical depth for the light ray
                            opticalDepthCA += exp(-height / _RayScaleHeight) * ds;

                            time += ds;

                        }	

                    }
                 }
            }


            void LightingStandardScattering_GI(SurfaceOutputStandard s, UnityGIInput data, inout UnityGI gi)
            {
                LightingStandard_GI(s, data, gi);
            }
            ENDCG
        }
        
    }
}
