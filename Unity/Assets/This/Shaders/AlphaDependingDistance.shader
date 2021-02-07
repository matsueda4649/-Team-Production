/*
カメラ距離に処理透過
*/
Shader "Custom/AlphaDependingDistance"
{
	Properties
	{
		// テクスチャ
		_MainTex("Texture", 2D) = "white"{}
		// 透過値
		_Radius("Radius", Range(0.001, 500)) = 10
	}
	SubShader
	{
		Tags{"Queue" = "Transparent" "RenderType" = "Transparent"}

		Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			struct v2f{
				float4 pos : SV_POSITION;
				float2 uv  : TEXCOORD0;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;

			v2f vert(appdata_base v) {
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
				return o;
			}

			float _Radius;
			float _Distance;

			fixed4 frag(v2f i) : SV_Target{
				fixed4 col = tex2D(_MainTex, i.uv);
				col.a =  saturate(_Distance / _Radius);
				return col;
			}

			ENDCG

		}
	}
}
