Shader "Hidden/Glitch"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_BlockSizeX("BlockSizeX", float) = 10.0
		_BlockSizeY("BlockSizeX", float) = 10.0
		_Threshold("Threshold", float) = 0.5
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"	
			#include "../NoiseUtils.cginc"


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
			float _BlockSizeX;
			float _BlockSizeY;
			float _Threshold;
            fixed4 frag (v2f i) : SV_Target
            {
				float2 uv = i.uv;
				//_BlockSizeY = rnd(float2(uv.y * 30.0, _Time.y)) * 2.5;
				float2 bUv = float2( floor(uv.x * _BlockSizeX) / _BlockSizeX, floor(uv.y * _BlockSizeY) / _BlockSizeY);
				
				//float noise = rnd(float2(bUv.x, _Time.y));
				float noise = rnd(float2(bUv.y, _Time.x));

				float2 offSet = float2(0.02, 0.0);
				fixed4 col = tex2D(_MainTex, uv);
				if (noise > _Threshold) {
					col.r = tex2D(_MainTex, uv + offSet).r;
					offSet = float2(-0.01, 0.0);
					col.g = tex2D(_MainTex, uv + offSet).g;
				}

				

                
                
                return col;
            }
            ENDCG
        }
    }
}
