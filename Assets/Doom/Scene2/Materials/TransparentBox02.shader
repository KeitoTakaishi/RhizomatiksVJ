Shader "Hidden/TransparentBox"
{
    Properties
    {
		[HDR]
		_Color("Color", Color) = (1,1,1,1)
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        // No culling or depth
        //Cull Off ZWrite Off ZTest Always

        Pass
        {
			Tags { "Queue" = "Transparent" }
			Blend SrcAlpha OneMinusSrcAlpha
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
			#include "noise4D.cginc" 
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
			float4 _Color;
			float width;
			fixed4 frag(v2f i) : SV_Target
			{
				float2 uv = i.uv;
				
				uv = floor(uv * float2(1.0, width)) / float2(1.0, width);
				uv *= 10.0;
				float noise = snoise(float4(uv.x, uv.y, 0.0, _Time.x * 20.0));
				fixed4 col = fixed4(0.0, 0.85, 0.7, 0.3) * _Color;
				if (noise > 0.5) {
					col = fixed4(1.0, 1.0, 1.0, 0.5) * _Color;
				}
				return col;
			}
            ENDCG
        }
    }
}
