﻿// スクロールするシェーダー
Shader "Custom/UVScroll"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}

        // オフセット
        _XOffset("Xuv Offset", Range(-1.0, 1.0)) = 0.1

        _YOffset("Yuv Offset", Range(-1.0, 1.0)) = 0.1

        // 速度
        _XSpeed("X Scroll Speed", Range(0.0, 100.0)) = 10.0

        _YSpeed("Y Scroll Speed", Range(0.0, 100.0)) = 10.0
    }
    SubShader
    {
        Tags 
        {
            "Queue" = "Transparent"
            "RenderType" = "Transparent"
        }

        Blend SrcAlpha OneMinusSrcAlpha
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float _XOffset;
            float _YOffset;
            float _XSpeed;
            float _YSpeed;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                _XOffset = _XOffset * _XSpeed;
                _YOffset = _YOffset * _YSpeed;

                i.uv.x = i.uv.x + _XOffset * _Time;
                i.uv.y = i.uv.y + _YOffset * _Time;

                fixed4 col = tex2D(_MainTex, i.uv);
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
