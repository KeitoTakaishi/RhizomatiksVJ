Shader "Hidden/SinWave"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        // No culling or depth
        //Cull Off ZWrite Off ZTest Always

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
			float Power;
            fixed4 frag (v2f i) : SV_Target
            {
				float2 uv = i.uv;
				uv.x += Power*sin(uv.y*20.0 + _Time.y);
                fixed4 col = tex2D(_MainTex, uv);
                // just invert the colors
                
				
				if (rnd(float2(Power, _Time.y)) > 0.5) {
					col.rgb = 1 - col.rgb;
				}
				
                
				
				return col;
            }
            ENDCG
        }
    }
}
