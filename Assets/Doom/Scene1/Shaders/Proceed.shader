﻿Shader "Hidden/Transparent"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        // No culling or depth
        Cull off ZWrite Off ZTest Always

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

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;

            float4 frag (v2f i) : SV_Target
            {
				float2 uv = i.uv;
				float4 col;
                
				uv.y = frac(uv.y + _Time.y*3.0);

				if (uv.y > 0.1) {
					col = tex2D(_MainTex, uv);
				}
				else {
					col = tex2D(_MainTex, uv);
					col.r = 1.0 - col.r;
					col.a = 0.0;
				}
				
                return col;
            }
            ENDCG
        }
    }
}
