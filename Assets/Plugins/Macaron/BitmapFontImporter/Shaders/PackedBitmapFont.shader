Shader "Macaron/UI/Packed Bitmap Font"
{
    Properties
    {
        [PerRendererData] _MainTex ("Font Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)

        _StencilComp ("Stencil Comparison", Float) = 8
        _Stencil ("Stencil ID", Float) = 0
        _StencilOp ("Stencil Operation", Float) = 0
        _StencilWriteMask ("Stencil Write Mask", Float) = 255
        _StencilReadMask ("Stencil Read Mask", Float) = 255

        _ColorMask ("Color Mask", Float) = 15

        [Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }

        Cull Off
        Lighting Off
        ZWrite Off
        ZTest [unity_GUIZTestMode]
        Blend SrcAlpha OneMinusSrcAlpha
        ColorMask [_ColorMask]

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0

            #include "UnityCG.cginc"
            #include "UnityUI.cginc"

            #pragma multi_compile __ UNITY_UI_CLIP_RECT
            #pragma multi_compile __ UNITY_UI_ALPHACLIP

            struct appdata_t
            {
                float4 vertex : POSITION;
                float4 color : COLOR;
                float2 texcoord : TEXCOORD0;

                #if UNITY_VERSION >= 550
                UNITY_VERTEX_INPUT_INSTANCE_ID
                #endif
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
                float2 texcoord : TEXCOORD0;
                float4 worldPosition : TEXTCOORD1;
                fixed4 channel : TEXCOORD2;

                #if UNITY_VERSION >= 550
                UNITY_VERTEX_OUTPUT_STEREO
                #endif
            };

            sampler2D _MainTex;
            fixed4 _Color;
            float4 _ClipRect;

            static fixed4 channels[4] =
            {
                fixed4(1, 0, 0, 0),
                fixed4(0, 1, 0, 0),
                fixed4(0, 0, 1, 0),
                fixed4(0, 0, 0, 1)
            };

            v2f vert (appdata_t v)
            {
                v2f OUT;

                #if UNITY_VERSION >= 550
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
                #endif

                OUT.channel = channels[dot((sign(v.texcoord) + float2(1, 1)) * float2(0.5, 0.5), float2(1, 2))];

                OUT.worldPosition = v.vertex;
                OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);
                OUT.texcoord = abs(v.texcoord);

                #if UNITY_VERSION < 550 && defined(UNITY_HALF_TEXEL_OFFSET)
                OUT.vertex.xy += (_ScreenParams.zw-1.0) * float2(-1,1) * OUT.vertex.w;
                #endif

                OUT.color = v.color * _Color;
                return OUT;
            }

            fixed4 frag (v2f IN) : SV_Target
            {
                half4 color = IN.color;
                color.a *= dot(tex2D(_MainTex, IN.texcoord), IN.channel);

                #if UNITY_VERSION < 201720 || defined(UNITY_UI_CLIP_RECT)
                color.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);
                #endif

                #ifdef UNITY_UI_ALPHACLIP
                clip(color.a - 0.001);
                #endif

                return color;
            }
            ENDCG
        }
    }
}
