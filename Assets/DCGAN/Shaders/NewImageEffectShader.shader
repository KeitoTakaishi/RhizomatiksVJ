Shader "Hidden/NewImageEffectShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_Tex2Dlod("Tex2DlodSample", 2D) = "white" {}
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
				float2 texcoord : TEXCOORD1;
			};

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

			sampler2D _Tex2Dlod;
			float4    _Tex2Dlod_ST;
            v2f vert (appdata v)
            {
                v2f o;
				float d = tex2Dlod(_Tex2Dlod, float4(v.texcoord.xy, 0, 0));
				v.vertex.y += d;
				o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;

				
                return o;
            }

            sampler2D _MainTex;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                // just invert the colors
				//col.rgb = 1 - col.rgb;

				float2 uv = i.uv;
				uv.y = 1.0 - uv.y;
				col = tex2D(_MainTex, uv);
				return col;
            }
            ENDCG
        }
    }
}
