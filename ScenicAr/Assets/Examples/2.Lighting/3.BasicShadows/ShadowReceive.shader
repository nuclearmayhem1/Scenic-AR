Shader "AR/ShadowReceive" 
{
	Properties
	{
		_ShadowIntensity("Shadow Intensity", Range(0, 1)) = 0.6
	}
	SubShader
	{ 
		
		Pass 
		{
			Blend SrcAlpha OneMinusSrcAlpha
			ZWrite Off
			Tags { "LightMode" = "ForwardBase" }

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			#include "AutoLight.cginc"
			#pragma multi_compile_fwdbase

			uniform float _ShadowIntensity;

			struct v2f {
				float4 pos : SV_POSITION; 
				LIGHTING_COORDS(0,1)
			};
			v2f vert(appdata_base v) {
				v2f o; 
				o.pos = UnityObjectToClipPos(v.vertex); 
				TRANSFER_VERTEX_TO_FRAGMENT(o);
				return o;
			}
			fixed4 frag(v2f i) : COLOR{
				float attenuation = LIGHT_ATTENUATION(i);
				return fixed4(0,0,0, (1 - attenuation) * _ShadowIntensity);
			} 
			ENDCG 
		} 
	} 
	Fallback "Transparent/Cutout/VertexLit" 
}
