Shader "AR/Tint"
{
    Properties
    {
        _Color("Main Color", Color) = (1,1,1,1)
    }
        SubShader
        {
            Tags {
                "RenderType" = "Opaque"
                "Queue" = "Geometry"
            }
            LOD 100

            Pass
            {
                ZWrite Off
                Blend SrcAlpha OneMinusSrcAlpha
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #include "UnityCG.cginc"

                uniform float _ShadowIntensity;
                fixed4 _Color;

                struct v2f
                {
                    float4 pos : SV_POSITION;
                };

                v2f vert(appdata_base v)
                {
                    v2f o; o.pos = UnityObjectToClipPos(v.vertex); 
                    return o;
                }

                fixed4 frag(v2f i) : COLOR
                {
                     return _Color;
                }
                ENDCG
            }
        }
            Fallback "Transparent/Cutout/VertexLit"
}
