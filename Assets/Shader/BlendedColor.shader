Shader "Custom/BlendedColor"
{
    Properties
    {
        _ColorA("Color A", Color) = (1,1,1,1) // colorAの初期値
        _ColorB("Color B", Color) = (0,0,0,1) // colorBの初期値
        _BlendFactor("Blend Factor", Range(0,1)) = 0.5 // blendFactorの初期値
    }
        SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 200

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            // シェーダーで使用するプロパティ
            fixed4 _ColorA;
            fixed4 _ColorB;
            float _BlendFactor;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            // ブレンドされた色を計算する
            fixed4 frag(v2f i) : SV_Target
            {
                // Color.Lerpのシェーダー版
                fixed4 blendedColor = lerp(_ColorA, _ColorB, _BlendFactor);
                return blendedColor;
            }
            ENDCG
        }
    }
        FallBack "Diffuse"
}
