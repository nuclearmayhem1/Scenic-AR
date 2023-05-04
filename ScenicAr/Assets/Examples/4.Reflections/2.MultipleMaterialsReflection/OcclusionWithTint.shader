Shader "AR/OcclusionWithTint"
{
    Properties
    {
        _Color("Main Color", Color) = (1,1,1,1)
        _MainTex("Texture", 2D) = "white" {}
        _Cutout("Cutout", Range(0, 1)) = 0.6
        _ShadowIntensity("Shadow Intensity", Range(0, 1)) = 0.6
        _ReflectionReduction("Reflection Reduction", Range(0, 1)) = 0.6
    }
        SubShader
        {
            Tags {
                "RenderType" = "Opaque"
                "Queue" = "Geometry-1"
            }
            LOD 100

            Pass
            {
                ZWrite On
                Blend SrcAlpha OneMinusSrcAlpha
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #include "UnityCG.cginc"
                #pragma multi_compile_fwdbase
                #include "AutoLight.cginc"
                #include "Lighting.cginc"

                uniform float _ShadowIntensity;
                fixed4 _Color;

                struct v2f
                {
                    float4 pos : SV_POSITION;
                    LIGHTING_COORDS(0, 1)
                };

                v2f vert(appdata_base v)
                {
                    v2f o; o.pos = UnityObjectToClipPos(v.vertex); TRANSFER_VERTEX_TO_FRAGMENT(o);
                    return o;
                }

                fixed4 frag(v2f i) : COLOR
                {

                     half atten = 1;// LIGHT_ATTENUATION(i);
                     fixed3 diffuseReflection = _LightColor0.rgb * atten * _Color.rgb;

                     fixed3 finalColor = UNITY_LIGHTMODEL_AMBIENT.xyz + diffuseReflection;
                     fixed4 finalColorAlpha = float4(finalColor, 1.0) * _Color.a;
                     //fixed4 shadowDarkening = fixed4(0, 0, 0, (1 - atten) * _ShadowIntensity);
                     //finalColorAlpha = lerp(finalColorAlpha, shadowDarkening, 1 - atten);

                     return finalColorAlpha;
                }
                ENDCG
            }
        }
            Fallback "Transparent/Cutout/VertexLit"
}
