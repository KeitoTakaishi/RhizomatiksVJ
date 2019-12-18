Shader "Hidden/DotBackground"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		[HDR]
		_Color("color",Color) = (0.0,0.0,0.0,0.0)
    }
    SubShader
    {
			Cull Off ZWrite Off ZTest Always
		Tags{
			"RenderType" = "Opaque"
			
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
			fixed4 _Color;

            fixed4 frag (v2f i) : SV_Target
            {
				float2 uv = i.uv;
				float num = 2.0;
				uv = frac(uv*num + float2(_Time.y*1.0, _Time.y*1.0));
				
				float2 center = float2(0.5, 0.5);
				//float radius = 0.2+0.2*sin(_Time.y);
				float radius = 0.2;
				
				float c = length(uv - center);
				if (c < radius) {
					c = 0.0;
				}
				else {
					c = 1.0;
				}

				fixed4 col = fixed4(1.0, 1.0, 1.0, 1.0)*_Color;
				col.rgb = fixed3(c, c, c)*_Color;
				return col;
            }
            ENDCG
        }
    }
}
