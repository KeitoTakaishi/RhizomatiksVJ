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

            fixed4 frag (v2f i) : SV_Target
            {
				float2 uv = i.uv;
				//float d = length(float2(0.5.xx) - uv);
				//uv.y += 0.01 * sin(  (_Time.x * 10.0+(uv.x*2.0-1.0)*30.0) * (1.0 - d) );
				
				float threshold = 0.3 + 0.3 * sin(_Time.y);
				float isTwist = 0.0;
				if (uv.y > threshold && uv.y < threshold + 0.3) isTwist = 1.0;
				float amp = 0.05;
				float period = 15.0;
				uv.x += amp * sin(_Time.x + uv.y * period) * isTwist;
				
				fixed4 col = tex2D(_MainTex, uv);
				
                return col;
            }
            ENDCG
        }
    }
}
