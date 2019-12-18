Shader "Hidden/DotBackground"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
		Tags{
			"RenderType" = "Background"
			"Queue" = "Background"
			"PreviewType" = "SkyBox"
		}

        Pass
        {
			ZWrite Off
			Cull Off
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

            fixed4 frag (v2f i) : SV_Target
            {
				float2 uv = i.uv;
				float num = 2.0;
				uv = frac(uv*num + float2(_Time.y*2.0, _Time.y*-3.0));
				
				float2 center = float2(0.5, 0.5);
				float radius = 0.2+0.2*sin(_Time.y);
				float c = length(uv - center);
				if (c < radius) {
					c = 0.0;
				}
				else {
					c = 0.75;
				}

				fixed4 col = fixed4(1.0, 1.0, 1.0, 1.0);
				col.rgb = fixed3(c, c, c);
				return col;
            }
            ENDCG
        }
    }
}
