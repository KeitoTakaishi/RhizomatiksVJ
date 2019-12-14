Shader "Hidden/Twist"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
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
			float4 Size_Center;
			float radius;
			float strength;
            fixed4 frag (v2f i) : SV_Target
            {
				float4 c = float4(0.0.xxxx);
				float2 uv = i.uv;
				//uv = uv * 2.0 - float2(1.0, 1.0);
				//float2 pos = (uv * Size_Center.xy) - Size_Center.zw;
				float2 pos = uv * 2.0 - float2(1.0, 1.0);
				float len = length(pos);
				if (len >= radius) {
					c  = tex2D(_MainTex, uv);
					return c;
				}
				float twist = min(max(1.0 - (len / radius), 0.0), 1.0) * strength;
				float x = pos.x * cos(twist) - pos.y * sin(twist);
				float y = pos.x * sin(twist) + pos.y * cos(twist);
				//float2 retPos = (float2(x, y) + Size_Center.zw) / Size_Center.xy;
				c = tex2D(_MainTex, uv + float2(x, y));
				
                return c;
            }
            ENDCG
        }
    }
}
