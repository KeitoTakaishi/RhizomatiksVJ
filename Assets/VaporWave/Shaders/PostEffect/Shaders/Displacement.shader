Shader "Hidden/Displacement"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_Tex1("Tex", 2D) = "white" {}
		_power("Power", Float) = 1.0
		_pivotX("PivotX", Float) = 0.5
		_pivotY("PivotX", Float) = 0.5
		_PeriodX("PeriodX", Range(0.0, 100.0)) = 1.0
		_PeriodY("PeriodY", Range(0.0, 100.0)) = 1.0
		_Scale("Scale", Range(0.0, 1.0)) = 1.0
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

				v2f vert(appdata v)
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
				float _Scale;
				fixed4 frag(v2f i) : SV_Target
				{
					float2 uv = i.uv;
					fixed4 col = tex2D(_MainTex, uv);
					float gray = dot(col.rgb, col.rgb);
					fixed4 ncol = tex2D(_MainTex, uv + float2(1.0 / _width, 0.0));
					float ngray = dot(ncol.rgb, ncol.rgb);

					float threshold = 0.95;
					gray = step(threshold, gray);
					ngray = step(threshold, ngray);
					
					
					float3 mono = float3(0.0, 0.0, 0.0);
					//
					if (gray == 1.0) {
						/*
						float blockSize = 100.0;
						float disp = rnd(float2(floor(uv.y * blockSize) / blockSize, 1.0)) * _power;
						for (int i = 0; i < 5.0; i++) {
							//uv.x *= _power;
							uv.x += disp * gray * 0.1;
							col += tex2D(_MainTex, uv);
						}
						//col.rgb = float3(1.0, 1.0, 1.0) - col.rgb;
						*/

						float scale = 30.0;
						mono = rnd3d(float2( floor(uv.x * 10.0) / 10.0, floor(uv.y * scale) / scale));
						//col.rgb = mono;
						uv.x += _power * mono * 0.5;
						col += tex2D(_MainTex, uv);
					}
					
					

					

					
					//mono = rnd3d(float2(0.0, floor(uv.y * 10.0) / 10.0));
					//col.rgb = mono;
					
					//col = tex2D(_MainTex, float2(uv.x * _Scale, uv.y));
					return col;
				}
				ENDCG
			}
		}
}
