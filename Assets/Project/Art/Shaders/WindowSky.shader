// The sky seen through a window. Unlit, and it casts no shadow, so the sun shines straight through the glass.
// Colors come from HouseLighting (global _HouseSky* values) so every window follows the time of day.
// Height is measured in world space, so upstairs windows show more sky and ground-floor windows show more trees.
Shader "TroUble/WindowSky" {
    Properties {
        _Brightness ("Brightness", Float) = 1
        _HorizonY ("Horizon height (world Y)", Float) = 0.8
        _SkyHeight ("Height of the gradient (m)", Float) = 5
        _Curve ("Gradient curve", Range(0.2, 3)) = 0.7
        _TreeBase ("Treeline lowest point (world Y)", Float) = 0.7
        _TreeHeight ("Treeline height variation (m)", Float) = 0.9
        _TreeScale ("Treeline bumpiness", Float) = 2.2
        _HorizonFallback ("Horizon (no HouseLighting)", Color) = (1.2, 0.5, 0.25, 1)
        _ZenithFallback ("Zenith (no HouseLighting)", Color) = (0.25, 0.15, 0.3, 1)
        _SilhouetteFallback ("Trees (no HouseLighting)", Color) = (0.03, 0.02, 0.04, 1)
    }

    SubShader {
        Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" "Queue" = "Geometry" }

        HLSLINCLUDE
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

        CBUFFER_START(UnityPerMaterial)
            float _Brightness;
            float _HorizonY;
            float _SkyHeight;
            float _Curve;
            float _TreeBase;
            float _TreeHeight;
            float _TreeScale;
            float4 _HorizonFallback;
            float4 _ZenithFallback;
            float4 _SilhouetteFallback;
        CBUFFER_END

        // Set by HouseLighting. _HouseSkyActive is 0 when nothing has set them yet.
        float4 _HouseSkyHorizon;
        float4 _HouseSkyZenith;
        float4 _HouseSkySilhouette;
        float _HouseSkyActive;

        float Hash( float x ) { return frac( sin( x * 127.1 ) * 43758.5453 ); }

        float ValueNoise( float x ) {
            float i = floor( x );
            float f = frac( x );
            return lerp( Hash( i ), Hash( i + 1.0 ), f * f * ( 3.0 - 2.0 * f ) );
        }
        ENDHLSL

        Pass {
            Name "Unlit"
            Tags { "LightMode" = "UniversalForward" }

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag

            struct Attributes { float4 positionOS : POSITION; };
            struct Varyings { float4 positionCS : SV_POSITION; float3 positionWS : TEXCOORD0; };

            Varyings Vert( Attributes input ) {
                Varyings o;
                o.positionWS = TransformObjectToWorld( input.positionOS.xyz );
                o.positionCS = TransformWorldToHClip( o.positionWS );
                return o;
            }

            half4 Frag( Varyings input ) : SV_Target {
                float3 horizon = lerp( _HorizonFallback.rgb, _HouseSkyHorizon.rgb, _HouseSkyActive );
                float3 zenith = lerp( _ZenithFallback.rgb, _HouseSkyZenith.rgb, _HouseSkyActive );
                float3 trees = lerp( _SilhouetteFallback.rgb, _HouseSkySilhouette.rgb, _HouseSkyActive );

                float t = saturate( ( input.positionWS.y - _HorizonY ) / _SkyHeight );
                float3 sky = lerp( horizon, zenith, pow( t, _Curve ) );

                // A bumpy treeline and rooftops. Uses position along the wall so neighboring windows line up.
                float along = ( input.positionWS.x + input.positionWS.z ) * _TreeScale;
                float n = ValueNoise( along ) * 0.6 + ValueNoise( along * 2.7 + 13.0 ) * 0.3 + ValueNoise( along * 7.1 + 41.0 ) * 0.1;
                float line_ = _TreeBase + n * _TreeHeight;
                float below = 1.0 - smoothstep( line_ - 0.02, line_ + 0.02, input.positionWS.y );

                return half4( lerp( sky, trees, below ) * _Brightness, 1.0 );
            }
            ENDHLSL
        }

        Pass {
            Name "DepthOnly"
            Tags { "LightMode" = "DepthOnly" }
            ZWrite On
            ColorMask R

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag

            struct Attributes { float4 positionOS : POSITION; };
            struct Varyings { float4 positionCS : SV_POSITION; };

            Varyings Vert( Attributes input ) {
                Varyings o;
                o.positionCS = TransformObjectToHClip( input.positionOS.xyz );
                return o;
            }

            half Frag( Varyings input ) : SV_Target { return 0; }
            ENDHLSL
        }

        Pass {
            Name "DepthNormalsOnly"
            Tags { "LightMode" = "DepthNormalsOnly" }
            ZWrite On

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag

            struct Attributes { float4 positionOS : POSITION; float3 normalOS : NORMAL; };
            struct Varyings { float4 positionCS : SV_POSITION; float3 normalWS : TEXCOORD0; };

            Varyings Vert( Attributes input ) {
                Varyings o;
                o.positionCS = TransformObjectToHClip( input.positionOS.xyz );
                o.normalWS = TransformObjectToWorldNormal( input.normalOS );
                return o;
            }

            half4 Frag( Varyings input ) : SV_Target { return half4( NormalizeNormalPerPixel( input.normalWS ), 0.0 ); }
            ENDHLSL
        }
    }
}
