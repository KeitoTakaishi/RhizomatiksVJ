Shader "Hidden/BlockNoise"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_AudioSeed("AudioSeed", Float) = 0.0

    }
    SubShader
    {
        // No culling or depth
		
		Cull Off
        Pass
        {
			Tags { "Queue" = "Transparent" }
			Blend SrcAlpha OneMinusSrcAlpha
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
			#include "noise4d.cginc"

			//-----------------------------------
			//noise
			float3 snoise3D(float4 x)
			{
				float s = snoise(x);
				float s1 = snoise(float4(x.y - 19.1, x.z + 33.4, x.x + 47.2, x.w));
				float s2 = snoise(float4(x.z + 74.2, x.x - 124.5, x.y + 99.4, x.w));
				float3 c = float3(s, s1, s2);
				return c;
			}

			float3 curlNoise(float4 p) {

				const float e = 0.0009765625;
				float4 dx = float4(e, 0.0, 0.0, 0.0);
				float4 dy = float4(0.0, e, 0.0, 0.0);
				float4 dz = float4(0.0, 0.0, e, 0.0);

				float3 p_x0 = snoise3D(p - dx);
				float3 p_x1 = snoise3D(p + dx);
				float3 p_y0 = snoise3D(p - dy);
				float3 p_y1 = snoise3D(p + dy);
				float3 p_z0 = snoise3D(p - dz);
				float3 p_z1 = snoise3D(p + dz);

				float x = p_y1.z - p_y0.z - p_z1.y + p_z0.y;
				float y = p_z1.x - p_z0.x - p_x1.z + p_x0.z;
				float z = p_x1.y - p_x0.y - p_y1.x + p_y0.x;

				const float divisor = 1.0 / (2.0 * e);
				return normalize(float3(x, y, z) * divisor);
			}
			//-----------------------------------
			float rand(float2 co) {
				return frac(sin(dot(co.xy ,float2(12.9898,78.233))) * 43758.5453);
			}

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
				//float4 worldPos = v.vertex;
				//worldPos.xyz += v.normal * sin(_Time.y + worldPos.x + worldPos.y + worldPos.z) * 10.0;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
			float _AudioSeed;
			float _Snare;
            fixed4 frag (v2f i) : SV_Target
            {
				float2 uv = i.uv;
				float random = rand(float2(_Time.y, _Snare));
				float2 blockSize = float2(_Snare+1.0, _Snare+1.0);
			

				//midi操作
				blockSize *= 1.0 ;
				
				float2 blockUv = float2(floor(uv * blockSize) / blockSize);
				float noise = snoise(float4(blockUv.x, blockUv.y, _Time.x * 10.0, _AudioSeed * 1.0));
				fixed4 col = float4(noise, noise, noise, 1.0) * 3.0;
				
				
				col.a = 0.5;
			
				
				return col;
            }
            ENDCG
        }
    }
}
