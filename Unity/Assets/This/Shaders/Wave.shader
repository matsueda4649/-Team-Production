Shader "Custom/Wave"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _Height("Height", Range(0, 1.0)) = 0.5
        _Speed("Speed", Range(0, 500.0)) = 100
        _Range("Range", Range(0, 500.0)) = 100
    }
    SubShader
    {
        Tags
        {
            "Queue" = "Transparent"
            "RenderType" = "Transparent"
        }

        Pass 
        {
            Blend SrcAlpha OneMinusSrcAlpha
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f 
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            float _Height;
            float _Speed;
            float _Range;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                float y = _Height * sin(_Time * _Speed + v.vertex.x * _Range);
                o.vertex.xyz = float3(o.vertex.x, o.vertex.y + y, o.vertex.z);
                return o;
            }

            fixed4 frag(v2f i) :SV_Target
            {
                fixed4 c = tex2D(_MainTex, i.uv);
                return c;
            }
             ENDCG
        }
    }
}