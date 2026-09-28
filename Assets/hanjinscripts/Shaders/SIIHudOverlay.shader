Shader "Custom/SIIHudOverlay"
{
    Properties

    {
        _HUDColor ("HUD Color", Color) = (0.2, 0.8, 1.0, 1.0)

        _BorderThickness ("Border Thickness", Range(0.001, 0.05)) = 0.006
        _CornerLength ("Corner Length", Range(0.02, 0.3)) = 0.10
        _BorderTransparency ("Border Transparency", Range(0, 1)) = 0.55
        _CornerTransparency ("Corner Transparency", Range(0, 1)) = 1.0

        _ScanEffectStrength ("Scan Effect Strength", Range(0, 0.3)) = 0.04
        _ScanEffectSpeed ("Scan Effect Speed", Range(0, 10)) = 1.5
        _ScanDensity ("Scanline Density", Range(10, 200)) = 70
    }

    SubShader
    {
        Tags
        {
            "Queue" = "Transparent"
            "RenderType" = "Transparent"
        }

        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off

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
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            float4 _HUDColor;
            float _BorderThickness;
            float _CornerLength;
            float _BorderTransparency;
            float _CornerTransparency;
            float _ScanEffectSpeed;
            float _ScanEffectStrength;
            float _ScanDensity;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            /*
            fragment shader
                - hud border
                - corner brackets
                - scan effects
                - transparency
            */

            fixed4 frag(v2f i) : SV_Target
            {
                float2 uv = i.uv;

                // hud boarder

                float left = step(uv.x, _BorderThickness);
                float right = step(1.0 - _BorderThickness, uv.x);
                float bottom = step(uv.y, _BorderThickness);
                float top = step(1.0 - _BorderThickness, uv.y);
                float border = saturate(left + right + top + bottom);

                // corner brackets

                float nearLeft = step(uv.x, _CornerLength);
                float nearRight = step(1.0 - _CornerLength, uv.x);
                float nearBottom = step(uv.y, _CornerLength);
                float nearTop = step(1.0 - _CornerLength, uv.y);

                float horizontalCorners = (top + bottom) * (nearLeft + nearRight);
                float verticalCorners = (left + right) * (nearTop + nearBottom);
                float corners = saturate(horizontalCorners + verticalCorners);

                // scan effect lines

                float scanPattern = sin(uv.y * _ScanDensity * UNITY_PI + _Time.y * _ScanEffectSpeed);
                float scanLine = smoothstep(0.55, 0.95, scanPattern);

                // transparency

                float intensity = border * _BorderTransparency;
                intensity += corners * _CornerTransparency;
                intensity += scanLine * _ScanEffectStrength;

                return fixed4(_HUDColor.rgb, saturate(intensity));
            }
            ENDCG
        }
    }
}
