Shader "Custom/SIIOverlay" {
    Properties {
        _OverlayColor ("Overlay Color", Color) = (0.2, 0.8, 1.0, 1.0)
        _Opacity ("Opacity", Range (0.0, 1.0)) = 0.3
        _PatternDensity ("Pattern Density", Range(1, 100)) = 20
        _ScanSpeed ("Scan Speed", Range(0, 1)) = 0.3
        _ScanIntensity ("Scan Intensity", Range(0.0, 1.0)) = 0.3
    }

    SubShader {
        Tags {
            "Queue" = "Transparent"
            "RenderType" = "Transparent"
        }

        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off

        Pass {

        CGPROGRAM

        #pragma vertex vert
        #pragma fragment frag

        #include "UnityCG.cginc"

        struct appdata {
            float4 vertex : POSITION;
            float2 uv : TEXCOORD0;
        };

        struct v2f {
            float2 uv : TEXCOORD0;
            float4 position : SV_POSITION;
        };

        float4 _OverlayColor;
        float _Opacity;
        float _PatternDensity;
        float _ScanSpeed;
        float _ScanIntensity;

        v2f vert (appdata v) {
            v2f o;
            o.position = UnityObjectToClipPos(v.vertex);
            o.uv = v.uv;
            return o;
        }

        fixed4 frag (v2f i) : SV_Target {
            // move scan lines vertically
            float scanLinePos = i.uv.y * _ScanIntensity + _Time.y * _ScanSpeed;

            // create scan line pattern
            float scanLine = sin(scanLinePos * _PatternDensity * 6.2831853);

            scanLine = scanLine * 0.5 + 0.5;

            // scan intensity setting
            float brightness = 1.0 + scanLine * _ScanIntensity;
            fixed4 finalColor = _OverlayColor * brightness * _Opacity;

            return finalColor;
        }
        ENDCG
        }
    }
}
