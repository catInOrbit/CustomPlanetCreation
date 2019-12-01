Shader "Custom/AtmosphericShader"
{
	Properties{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0
		_AtmosphereRadius("AtmosphereRadius", Float) = 1;
		_PlanetRadius("PlanetRadius", Float) = 1;

	}
		SubShader
		{
			// --- First pass ---
		Tags { "RenderType" = "Opaque" }
		LOD 200
		Pass
		{
			CGPROGRAM
			#include "UnityPBSLighting.cginc"

			#pragma surface surf StandardScattering vertex:vert

			sampler2D _MainTex;
			float4 _Color;
			float _Glossiness;
			float _Metallic;
			float _AtmosphereRadius;
			float _PlanetRadius;


			struct Input
			{
				float2 uv_MainTex;
				float3 worldPos; // Initialised automatically by Unity
				float3 centre;   // Initialised in the vertex function
			};


			void vert(inout appdata_full v, out Input o)
			{
				UNITY_INITIALIZE_OUTPUT(Input, o);
				v.vertex.xyz += v.normal * (_AtmosphereRadius - _PlanetRadius);
				o.centre = mul(unity_ObjectToWorld, half4(0, 0, 0, 1));
			}

			inline fixed4 LightingStandardScattering(SurfaceOutputStandard s, fixed3 viewDir, UnityGI gi)
			{
				float3 L = gi.light.dir;
				float3 V = viewDir;
				float3 N = s.Normal;

				float3 S = L; // Direction of light from the sun
				float3 D = -V;  // Direction of view ray piercing the atmosphere

			}

			void LightingStandardScattering_GI(SurfaceOutputStandard s, UnityGIInput data, inout UnityGI gi)
			{
				LightingStandard_GI(s, data, gi);
			}

			bool rayIntersect
			(
				// Ray
				float3 O, // Origin
				float3 D, // Direction

				// Sphere
				float3 C, // Centre
				float R,	// Radius
				out float AO, // First intersection time
				out float BO  // Second intersection time
			)
			{
				float3 L = C - O;
				float DT = dot(L, D);
				float R2 = R * R;

				float CT2 = dot(L, L) - DT * DT;

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
				return fixed4(0, 0, 0, 0); // The view rays is looking into deep space

			// Is the ray passing through the planet core?
			float pA, pB;
			if (rayIntersect(O, D, _PlanetCentre, _PlanetRadius, pA, pB))
				tB = pA;
			ENDCG
		}
		
	
	}
	FallBack "Diffuse"
}
