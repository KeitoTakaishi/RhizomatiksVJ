Shader "Hidden/BlockGlitch"
{
    Properties
    {
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
		_BlockSize("blockSize", Vector) = (10.0, 10.0, 0.0, 0.0)//thresold
		_Speed("speed", float) = 3.0
    }
    SubShader
    {
		Tags {"Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent"}
		LOD 100 

		ZWrite Off                                 
		Blend SrcAlpha OneMinusSrcAlpha
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
			#include "Noise4d.cginc"
			#include "NoiseUtils.cginc"

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
			float4 _BlockSize;
			float _Speed;
            fixed4 frag (v2f i) : SV_Target
            {
				float2 uv = i.uv;
				uv.y = frac(uv.y + _Time.y);
				fixed4 col = tex2D(_MainTex, uv);
				uv.y = frac(uv.y + _Time.x);
				float2 buv = floor(uv * _BlockSize.xy) / _BlockSize.xy;
				float noise = snoise(float4(buv.x*100.0, buv.y*1.0, 0.0, _Time.y * _Speed));
				if (noise > _BlockSize.z) {
					col.r = tex2D(_MainTex, uv + float2(-0.14*noise, rnd(float2(_Time.y, noise*noise)))).r;
					col.g = tex2D(_MainTex, uv + float2(0.01*noise, rnd(float2(_Time.y, noise*noise)))).g ;
					col.r += rnd(float2(_Time.x, 3240.0));
					col.g += rnd(float2(_Time.x*2.02, 340.0));
					//col.b += rnd(float2(_Time.x*0.987, 2440.0));
					
					col.a = 1.0;
				}
				col.rgb = float3(1.0, 1.0, 1.0) - col.rgb;
				return col;
            }
            ENDCG
        }
    }
}
