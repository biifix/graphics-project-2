Shader "Unresolved/Archive Reconstruction Simple"
{
    Properties
    {
        // Real photo
        _PhysicalTex ("Physical Photo", 2D) = "white" {}
        // Photo after reconstructed
        _RestoredTex ("AI Restored Photo", 2D) = "white" {}

        // Values controlled by the slider and C# script
        _Reconstruction ("Reconstruction Amount", Range(0, 1)) = 1
        _PreviousReconstruction ("Previous Amount", Range(0, 1)) = 1
        _ScanProgress ("Scan Progress", Range(0, 1)) = 1

        // Appearance of the scan line and particles
        _ScanWidth ("Scan Line Width", Range(0.001, 0.1)) = 0.02
        _ScanColour ("Scan Line Colour", Color) = (0, 0.7, 1, 1)
        _ScanActive ("Scan Active", Range(0, 1)) = 0
        _ParticleSize ("Particle Size", Range(0.05, 0.4)) = 0.18
        _ParticleBrightness ("Particle Brightness", Range(0, 3)) = 1.5
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
                // Read data from the Quad
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 position : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            sampler2D _PhysicalTex;
            sampler2D _RestoredTex;
            float _Reconstruction;
            float _PreviousReconstruction;
            float _ScanProgress;
            float _ScanWidth;
            fixed4 _ScanColour;
            float _ScanActive;
            float _ParticleSize;
            float _ParticleBrightness;

            // Place particles using random coordinates
            float randomValue(float2 cell)
            {
                float randomNumber =
                    sin(dot(cell, float2(12.9898, 78.233)));

                return frac(randomNumber * 43758.5453);
            }

            // Put the photo plane in the correct position on the screen
            v2f vert(appdata input)
            {
                v2f output;
                output.position =
                    UnityObjectToClipPos(input.vertex);

                output.uv = input.uv;

                return output;
            }

            fixed4 frag(v2f input) : SV_Target
            {
                // Read the two photo colours at the current UV position
                fixed4 physicalColour =
                    tex2D(_PhysicalTex, input.uv);

                fixed4 restoredColour =
                    tex2D(_RestoredTex, input.uv);

                fixed4 previousColour =
                    lerp(
                        physicalColour,
                        restoredColour,
                        _PreviousReconstruction
                    );

                fixed4 targetColour =
                    lerp(
                        physicalColour,
                        restoredColour,
                        _Reconstruction
                    );

                // Pixels on the left of the scan line show the new value
                float revealedArea =
                    step(input.uv.x, _ScanProgress);

                fixed4 finalColour =
                    lerp(
                        previousColour,
                        targetColour,
                        revealedArea
                    );

                float distanceFromLine =
                    abs(input.uv.x - _ScanProgress);

                // Make a soft glowing band around the current scan position
                float scanLine =
                    1.0 - smoothstep(
                        0.0,
                        _ScanWidth,
                        distanceFromLine
                    );

                finalColour.rgb +=
                    scanLine * _ScanColour.rgb * _ScanActive;

                // Divide the photo into small cells for particles
                float2 particleGrid =
                    input.uv * float2(35.0, 20.0);

                float2 particleCell = floor(particleGrid);
                float2 positionInCell = frac(particleGrid);

                float particleDistance =
                    length(positionInCell - float2(0.5, 0.5));

                float particleShape =
                    1.0 - step(_ParticleSize, particleDistance);

                float particleRandom =
                    randomValue(particleCell);

                float visibleParticle =
                    step(0.72, particleRandom);

                float particleNearScan =
                    1.0 - smoothstep(
                        _ScanWidth,
                        _ScanWidth * 5.0,
                        distanceFromLine
                    );

                // Use Unity time to make the selected particles flicker
                float particleFlicker =
                    0.5 + 0.5 * sin(
                        _Time.y * 8.0 + particleRandom * 6.28
                    );

                float particles =
                    particleShape *
                    visibleParticle *
                    particleNearScan *
                    particleFlicker *
                    _ScanActive;

                finalColour.rgb +=
                    particles *
                    _ScanColour.rgb *
                    _ParticleBrightness;

                return finalColour;
            }

            ENDCG
        }
    }
}
