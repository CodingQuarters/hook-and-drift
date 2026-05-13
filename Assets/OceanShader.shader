Shader "Unlit/OceanAnimated_2D"
{
    Properties
    {
        _Color ("Main Color", Color) = (1,1,1,1)
        _MainTex ("Texture", 2D) = "white" {}
        _ScrollSpeed ("Scroll Speed", Float) = 0.1
        _WaveIntensity ("Wave Intensity", Float) = 0.01
        _CurrentDirection ("Current Direction", Vector ) = (1,0,0,0)
    }
    SubShader
    {
        Tags { 
            "RenderType"="Transparent" 
            "Queue"="Transparent" 
            "IgnoreProjector"="True" 
            "PreviewType"="Plane"
        }
        
        LOD 100
        
        // CHANGE 2: ZWrite Off is required for 2D sorting to work
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha // Allows for transparency if your texture has it

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
            
            uniform float4 _PlayerPosition; 
            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Color;
            float _ScrollSpeed;
            float _WaveIntensity;
            float4 _CurrentDirection;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target //Logic code right here
            {

                float2 playerOffSet = _PlayerPosition.xy * 0.1; //movement offset
                float2 animatedUV = i.uv;                

                animatedUV.x += _CurrentDirection.xy * (_Time.y * _ScrollSpeed) * 0.7; //constant water current with the current direction vector 
                animatedUV += playerOffSet * 0.7; 
                animatedUV.y += sin(_Time.y + i.uv.x * 10) * _WaveIntensity;

                fixed4 col = tex2D(_MainTex, animatedUV);
                col *= _Color;
                
                return col;
            }
            ENDCG
        }
    }
}
