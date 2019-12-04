Shader "Hidden/GradetionSkyBox"
{
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


			fixed4 frag(v2f i) : SV_Target
			{

				return fixed4(lerp(fixed3(1, 0, 0), fixed3(0, 0, 1), i.uv.y * 0.5 + 0.5), 1.0);
			}
            ENDCG
        }
    }
}
