Shader "Custom/SIISurfaceOverlay"
{
    Properties
    {
        _OverlayColor ("Overlay Color", Color) = (0.2, 0.8, 1.0, 1.0)
        _OverlayStrength ("Overlay Strength", Range(0.0, 1.0)) = 0.35

        _ScanSpeed ("Scan Speed", Range(0.0, 5.0)) = 1.0
        _ScanDensity ("Scan Density", Range(1.0, 100.0)) = 30.0

        _PulseSpeed ("Pulse Speed", Range(0.0, 5.0)) = 1.0

        _RimPower ("Rim Power", Range(0.1, 8.0)) = 3.0
        _RimIntensity ("Rim Intensity", Range(0.0, 3.0)) = 0.8
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
        Cull Back

        Pass
        {
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 position : SV_POSITION;
                float2 uv : TEXCOORD0;

                float3 worldPos : TEXCOORD1;
                float3 worldNormal : TEXCOORD2;
            };

            float4 _OverlayColor;
            float _OverlayStrength;

            float _ScanSpeed;
            float _ScanDensity;

            float _PulseSpeed;

            float _RimPower;
            float _RimIntensity;

            v2f vert(appdata v)
            {
                v2f o;

                o.position = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;

                o.worldPos =
                    mul(unity_ObjectToWorld, v.vertex).xyz;

                o.worldNormal =
                    UnityObjectToWorldNormal(v.normal);

                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // -------------------------
                // Base overlay
                // -------------------------

                float3 colour = _OverlayColor.rgb;


                // -------------------------
                // Procedural scan lines
                // -------------------------

                float scanPosition =
                    i.uv.y * _ScanDensity
                    + _Time.y * _ScanSpeed;

                float scan =
                    sin(scanPosition * 6.2831853);

                scan = scan * 0.5 + 0.5;

                // sharpen the brighter lines slightly
                scan = smoothstep(0.55, 1.0, scan);


                // -------------------------
                // Time-based pulse
                // -------------------------

                float pulse =
                    0.75
                    + 0.25 *
                    sin(_Time.y * _PulseSpeed);


                // -------------------------
                // Fresnel rim light
                // -------------------------

                float3 normal =
                    normalize(i.worldNormal);

                float3 viewDirection =
                    normalize(
                        _WorldSpaceCameraPos.xyz
                        - i.worldPos
                    );

                float rim =
                    1.0 -
                    saturate(
                        dot(normal, viewDirection)
                    );

                rim =
                    pow(rim, _RimPower);

                rim *= _RimIntensity;


                // -------------------------
                // Combine effects
                // -------------------------

                float scanBrightness =
                    0.65 + scan * 0.35;

                float intensity =
                    _OverlayStrength
                    * pulse
                    * scanBrightness;

                intensity +=
                    rim * _OverlayStrength;

                float alpha =
                    saturate(
                        _OverlayColor.a
                        * intensity
                    );

                return fixed4(
                    colour * (intensity + rim),
                    alpha
                );
            }

            ENDCG
        }
    }
}
