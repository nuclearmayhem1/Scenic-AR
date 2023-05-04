Shader "AR/OcclusionWithShadows"
{
    Properties
    {
        _ShadowIntensity("Shadow Intensity", Range(0, 1)) = 0.6
    }

    // Built-in Render Pipeline Subshader
    SubShader
    {

        Tags { "RenderType"="Opaque" }
        Tags { "Queue" = "Geometry-1" }

        ZWrite On
        ZTest LEqual

        Pass
        {
            LOD 200
            Tags {"LightMode" = "ForwardBase" }
            Cull Back
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fwdbase

            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            uniform float _ShadowIntensity;

            struct appdata
            {
                float4 vertex : POSITION;

                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;

                LIGHTING_COORDS(0,1)

                UNITY_VERTEX_OUTPUT_STEREO
            };

            v2f vert (appdata v)
            {
                v2f o;

                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_OUTPUT(v2f, o);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

                o.vertex = UnityObjectToClipPos(v.vertex);
                TRANSFER_VERTEX_TO_FRAGMENT(o);
                return o;
            } 
                
            fixed4 frag (v2f i) : SV_Target
            {

                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);
                float attenuation = LIGHT_ATTENUATION(i);

                return fixed4(0, 0, 0, (1 - attenuation) * _ShadowIntensity);
            }
            ENDCG
        }
    }
}