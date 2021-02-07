Shader "Custom/Lliquid"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color("Main Color", Color) = (1, 0.5, 0.2)
        _Radius("Radius", Range(0.0, 20)) = 4    // 波の半径
        _Decrease_R("Decrease Radius", Range(0.0, 50)) = 10  // 速度
        _Height("Height", Range(0, 100.0)) = 30  // 波の高さ
        _Speed("Speed", Range(0.0, 0.85)) = 0.5  // 速度
        _Adj_R("Adj R", Range(-3.0, 3)) = 1  // Red
        _Adj_G("Adj G", Range(-3.0, 3)) = 1  // Green
        _Adj_B("Adj B", Range(-3.0, 3)) = 1  // Blue
    }

    CGINCLUDE

    #include "UnityCG.cginc"

    ENDCG

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog
            #pragma target 2.5

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

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 _Color;
            float _Radius;
            float _Decrease_R;
            float _Height;
            float _Speed;
            float _Adj_R;
            float _Adj_G;
            float _Adj_B;

            fixed4 frag(v2f i) : SV_Target
            {
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);

                half2 pos = i.uv.xy;

                pos.y = _ProjectionParams.x > 0 ? 1 - pos.y : pos.y;

                float2 size = pos * _Radius;
                size.y -= _Time.w * _Speed;

                half2 f_size = frac(size);
                size -= f_size;
                f_size = f_size * f_size * (3.0 - 2.0 * f_size);

                float4 f_sin = sin((size.x + size.y * 1000) + half4(0, 1, 1000, 1001));

                half4 f = frac(f_sin * 10000) * _Height / pos.y;

                half lerp_x_y = lerp(f.x, f.y, f_size.x);
                half lerp_z_w = lerp(f.z, f.w, f_size.x);

                half clamp_lerp = clamp(lerp(lerp_x_y, lerp_z_w, f_size.y) - _Decrease_R, -0.2, 1.0);

                fixed r = pos.y * _Adj_R + _Color.r * clamp_lerp;
                fixed g = pos.y * _Adj_G + _Color.g * clamp_lerp;
                fixed b = pos.y * _Adj_B + _Color.b * clamp_lerp;

                return fixed4(r, g, b, 1);
            }
            
            ENDCG
        }
    }
}
