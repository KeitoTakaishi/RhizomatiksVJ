Shader "Hidden/GDistortion"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_Tex1("Tex", 2D) = "white" {}
		_power("Power", Float) = 1.0
		_pivotX("PivotX", Float) = 0.5
		_pivotY("PivotX", Float) = 0.5
		_PeriodX("PeriodX", Range(0.0, 100.0)) = 1.0
		_PeriodY("PeriodY", Range(0.0, 100.0)) = 1.0
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
			#include "../NoiseUtils.cginc"


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
			sampler2D _Tex1;
			float _power;
			float _pivotX, _pivotY;
			float _PeriodX, _PeriodY;


			float _coef[9];
			float _width;
			float _height;
			fixed4 frag(v2f i) : SV_Target
			{
				//float2 uv = i.uv;
				//float2 pivot = float2(_pivotX, _pivotY);
				
				//float speed = 10.0;
				
				
				//pivot.x = pivot.x*cos(uv.x*_PeriodX + _Time.y* speed);
				//pivot.y = pivot.y*sin(uv.y*_PeriodY + _Time.y * speed);
				
				//uv = uv - float2(pivot.x, pivot.y);
				//uv *= pow(length(uv), _power);
				//uv = uv + float2(pivot.x, pivot.y);


				
				
				float2 uv = i.uv;
				for (float i = 0.0; i < 9.0; i += 1.0) {
					_coef[(int)i] = 1.0;
				}
				_coef[4] = -8.0;


				float2 offset[9];
				float b = 3.0;
				offset[0] = float2(-b, -b);
				offset[1] = float2(0.0, -b);
				offset[2] = float2(b, -b);
				offset[3] = float2(-b, 0.0);
				offset[4] = float2(0.0, 0.0);
				offset[5] = float2(b, 0.0);
				offset[6] = float2(-b, b);
				offset[7] = float2(0.0, b);
				offset[8] = float2(b, b);

				fixed4 col = fixed4(0.0, 0.0, 0.0, 0.0);


				float2 fc = uv * float2(_width, _height);
				float2 tFrag = float2(1.0 / _width, 1.0 / _height);
				col += tex2D(_MainTex, (fc + offset[0]) * tFrag) * _coef[0];
				col += tex2D(_MainTex, (fc + offset[1]) * tFrag) * _coef[1];
				col += tex2D(_MainTex, (fc + offset[2]) * tFrag) * _coef[2];
				col += tex2D(_MainTex, (fc + offset[3]) * tFrag) * _coef[3];
				col += tex2D(_MainTex, (fc + offset[4]) * tFrag) * _coef[4];
				col += tex2D(_MainTex, (fc + offset[5]) * tFrag) * _coef[5];
				col += tex2D(_MainTex, (fc + offset[6]) * tFrag) * _coef[6];
				col += tex2D(_MainTex, (fc + offset[7]) * tFrag) * _coef[7];
				col += tex2D(_MainTex, (fc + offset[8]) * tFrag) * _coef[8];


				
				
				//float2 newUV = float2( min(uv.x, frac(_Time.x)), 0.0) + uv;
				//newUV = floor(newUV * 5.0) / 5.0;
				float disp = tex2D(_Tex1, float2(uv.x, frac(uv.y * 5.0)));
				
				
				//float dir = step(_power, 1.5);
				//uv += disp * float2(dir*frac(_Time.y), (1.0 - dir) * frac(_Time.y))*_power;
				
				for (int i = 0; i < 10.0; i++) {
					uv.x += disp * _power;
					col *= 0.8;
					col += tex2D(_MainTex, uv);
				}
				col /= 2.0;
				
				
				return col;
			}
            ENDCG
        }
    }
}
