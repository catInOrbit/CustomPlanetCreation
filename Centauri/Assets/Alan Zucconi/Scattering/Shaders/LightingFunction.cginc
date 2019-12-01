#ifndef CVGLIGHTING
#define CVGLIGHTING

float3 normalFromColor(float4 colorValue)
{
#if defined(UNITY_NO_DXT5nm)
        return colorValue.xyz * 2 -1; //Color read from color channel are from 0-1
                                      //-1 to 1 is what Unity uses, increase range to 2 units, and shift 1 
    //Account for compression
#else 
        // R --> X --> A
        // G --> Y --> 
        // B --> Z --> ignored

    float3 normalVal;
    normalVal = float3(colorValue.a * 2.0 - 1, colorValue.g * 2.0 - 1, 0.0);
    normalVal.z = sqrt(1.0 - dot(normalVal, normalVal));
    return normalVal;
#endif
}

float3 WorldNormalFromNormalMap(sampler2D normalMap, float2 normalTexcoord, float3 tangentWorld, float3 binormalWorld, float3 normalWorld)
{
    float4 colorAtPixel = tex2D(normalMap, normalTexcoord);

    //Normal converted from normal channel value
    float3 normalAtPixel = normalFromColor(colorAtPixel);

    //TBN 
    float3x3 TBNWorld = float3x3(tangentWorld.xyz, binormalWorld.xyz, normalWorld.xyz);
    float3 worldNormalAtPixel = normalize(mul(normalAtPixel, TBNWorld));
    return half4(worldNormalAtPixel, 1);

}

#endif
