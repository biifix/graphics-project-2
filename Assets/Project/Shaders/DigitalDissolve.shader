Shader "Custom/DigitalDissolve"
{
    Properties
    {
        _Color ("Base Color", Color) = (0.2, 0.7, 1, 1)
        _DissolveAmount ("Dissolve Amount", Range(0, 1)) = 0
        _NoiseScale ("Noise Scale", Range(1, 50)) = 12
        _EdgeWidth ("Edge Width", Range(0.001, 0.2)) = 0.08
        [HDR] _EdgeColor ("Edge Color", Color) = (0, 1, 1, 1)
        _EdgeIntensity ("Edge Intensity", Range(0, 5)) = 2
        _DissolveSpeed ("Dissolve Speed", Range(0.1, 2)) = 0.5
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" }

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
                float4 position : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            fixed4 _Color;
            float _DissolveAmount;
            float _NoiseScale;
            float _EdgeWidth;
            fixed4 _EdgeColor;
            float _EdgeIntensity;

            v2f vert(appdata v)
            {
                v2f o;
                o.position = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float randomNoise(float2 position)
            {
                return frac(
                    sin(dot(position, float2(127.1, 311.7)))
                    * 43758.5453
                );
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 noiseCell = floor(i.uv * _NoiseScale);
                float noise = randomNoise(noiseCell);

                float dissolveDifference =
                    noise - _DissolveAmount;

                clip(dissolveDifference);

                float edge = 1.0 - smoothstep(
                    0.0,
                    _EdgeWidth,
                    dissolveDifference
                );

                fixed4 finalColor = _Color;

                finalColor.rgb +=
                    _EdgeColor.rgb * edge * _EdgeIntensity;

                return finalColor;
            }

            ENDCG
        }
    }
}