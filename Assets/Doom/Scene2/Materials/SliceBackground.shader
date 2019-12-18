Shader "Hidden/SliceBackground"
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
				fixed4 col = fixed4(0.0, 0.0, 0.0, 1.0);
				uv.y = frac(_Time.y - uv.y*10.0);
				float width = 0.1;

				if (length(uv.y - 0.2) < width) {
					col = fixed4(0.0, 0.0, 0.0, 1.0);
				}
				else {
					col = fixed4(1.0, 1.0, 1.0, 1.0)*0.75;
				}
				return col;

            }
            ENDCG
        }
    }
}
