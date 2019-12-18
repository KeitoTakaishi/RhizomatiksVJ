Shader "Hidden/Plane"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        // No culling or depth
        Cull Off
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

			float rand(float3 co) {
				return frac(sin(dot(co.xy, float3(12.9898, 78.233, 65.346))) * 43758.5453);
			}

            v2f vert (appdata v)
            {
                v2f o;
				float4 vert = v.vertex;
				float amp = rand(float3(vert.y, _Time.x, vert.z));
				vert.y = sin(_Time.w + (vert.x + vert.z) * 750.0 )*amp*5.0;
                o.vertex = UnityObjectToClipPos(vert);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
				float2 uv = frac(i.uv+float2(0.0, -_Time.y));
				float span = 0.01;
				float width = span * 0.25;
				/*if (fmod(uv.x, span) < width || fmod(uv.y, span) < width) {
					col.rgb = float3(0.0, 0.0, 0.0);
				}*/

				if (fmod(uv.y, span) < width) {
					col.rgb = float3(0.0, 0.0, 0.0);
				}
				else {
					col.rgb = float3(0.4, 0.4, 0.4);
				}
				
                return col;
            }
            ENDCG
        }
    }
}
