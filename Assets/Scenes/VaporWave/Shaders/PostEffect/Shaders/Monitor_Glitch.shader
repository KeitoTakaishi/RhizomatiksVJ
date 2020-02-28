Shader "Hidden/Monitor_Glitch"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        // No culling or depth
        //Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
			
            #include "UnityCG.cginc"	
		#include "../../Noise4d.cginc"

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

            fixed4 frag (v2f i) : SV_Target
            {
				float2 uv = i.uv;
                fixed4 col = tex2D(_MainTex, uv);

				float2 period = float2(100.0, 10.0);
				float2 block = floor(uv * period) / period;
				float noise = snoise3D(float4(block.x, block.y, 0.0, _Time.y*3.0));
				if (noise > 0.2) {
					col.r = tex2D(_MainTex, uv + float2(0.01 * noise, 0.0)).r;
					col.g = tex2D(_MainTex, uv + float2(-0.02 * noise, 0.0)).g;
					//col.rgb = float3(1.0, 1.0, 1.0) - col.rgb;
				}

                return col;
            }
            ENDCG
        }
    }
}
