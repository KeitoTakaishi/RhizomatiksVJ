Shader "Hidden/NoiseSkyBox"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        // No culling or depth
        //Cull Off ZWrite Off ZTest Always
		Tags{
			"RenderType" = "Background"
			"Queue" = "Background"
			"PreviewType" = "SkyBox"
		}
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
			#include "Noise4d.cginc"
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
				float period = 2.0;
				uv *= period;
				float4 color = float4(1.0, 0.0, 0.8, 1.0);
				col.rgb = snoise3D(float4(0.0, uv.x, uv.y, _Time.y * 1.0));
				col = col * color;
                return col;
            }
            ENDCG
        }
    }
}
