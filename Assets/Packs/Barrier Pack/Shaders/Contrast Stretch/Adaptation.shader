

Shader "Hidden/Contrast Stretch Adaptation" {
Properties {
	_MainTex ("Base (RGB)", 2D) = "white" {}
	_CurTex ("Base (RGB)", 2D) = "white" {}
}

Category {
	SubShader {
		Pass {
			ZTest Always Cull Off ZWrite Off
				
CGPROGRAM
#pragma vertex vert_img
#pragma fragment frag
#include "UnityCG.cginc"

uniform sampler2D _MainTex;
uniform sampler2D _CurTex;
uniform float4 _AdaptParams; 

float4 frag (v2f_img i) : SV_Target  {
	float2 valAdapted = tex2D(_MainTex, i.uv).xy;
	float2 valCur = tex2D(_CurTex, i.uv).xy;
	const float kMinChange = 1.0/255.0;
	float2 delta = (valCur-valAdapted) * _AdaptParams.x;
	delta.x = sign(delta.x) * max( kMinChange, abs(delta.x) );
	delta.y = sign(delta.y) * max( kMinChange, abs(delta.y) );

	float4 valNew;
	valNew.xy = valAdapted + delta;
	
	valNew.x = max( valNew.x, _AdaptParams.z );
	valNew.y = min( valNew.y, _AdaptParams.y );
	
	valNew.z = valNew.x - valNew.y + 0.01;
	valNew.w = valNew.y / valNew.z;
	
	return valNew;
}
ENDCG

		}
	}
}

Fallback off

}
