Shader "Custom/JuliaSet"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _Zoom("Zoom", Float) = 1.0
        _Offset("Offset", Vector) = (0.0, 0.0, 0, 0)
        _Iterations("Iterations", Int) = 300 // 増やして詳細度を高める
        _Color1("Color 1", Color) = (0, 0, 0, 1)
        _Color2Start("Color 2 Start", Color) = (1, 1, 1, 1)
        _Color2End("Color 2 End", Color) = (1, 0, 0, 1)
        _Speed("Speed", Float) = 1.0 // パラメータ c の変化速度
        _ColorSpeed("Color Speed", Float) = 1.0 // 色の変化速度
    }
        SubShader
        {
            Tags { "RenderType" = "Opaque" }
            LOD 100

            Pass
            {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag

                #include "UnityCG.cginc"

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

                sampler2D _MainTex;
                float4 _MainTex_ST;
                float2 _Resolution;
                float _Zoom;
                float4 _Offset;
                int _Iterations;
                float4 _Color1;
                float4 _Color2Start;
                float4 _Color2End;
                float _Speed;
                float _ColorSpeed;

                v2f vert(appdata v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = v.uv;
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target
                {
                    float2 uv = i.uv * 2.0 - 1.0; // Normalize UV coordinates to [-1, 1]
                    uv /= _Zoom;
                    uv += _Offset.xy;

                    float2 z = uv;

                    // 時間に基づいてパラメータ c を変化させる
                    float2 c = float2(0.440, 0.440) + float2(sin(_Time.y * _Speed), cos(_Time.y * _Speed)) * 0.1;

                    int iterations = 0;

                    for (int n = 0; n < _Iterations; n++)
                    {
                        float x = (z.x * z.x - z.y * z.y) + c.x;
                        float y = (z.y * z.x + z.x * z.y) + c.y;

                        if ((x * x + y * y) > 4.0)
                            break;

                        z = float2(x, y);
                        iterations++;
                    }

                    float t = iterations / (float)_Iterations;

                    // 時間に基づいて色を変化させる
                    float4 dynamicColor = lerp(_Color2Start, _Color2End, abs(sin(_Time.y * _ColorSpeed)));

                    // 色のコントラストを強くするための調整
                    float4 finalColor = lerp(_Color1, dynamicColor, t * t);

                    return finalColor;
                }
                ENDCG
            }
        }
            FallBack "Diffuse"
}




