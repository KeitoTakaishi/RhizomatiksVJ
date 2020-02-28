Shader "Unlit/PolygonThorns"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		[HDR]
		_topColor("TopColor", Color) = (0, 0, 0, 0)
		[HDR]
		_bottomColor("BottomColor", Color) = (0, 0, 0, 0)

		_Threshold("_Threshold", Range(0.0, 1.0)) = 0.75
		_NoisePeriod("NoisePeriod", Range(0.0, 1000.0)) = 1.0
		_TimeSpeed("TimeSpeed", Range(0.0, 1.0)) = 1.0
		_Amp("Amp", Range(0.0, 1.0)) = 1.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
			#pragma geometry geom
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"
			#include "../NoiseUtils.cginc"
			#include "../Noise4d.cginc"
            
		
			 struct appdata
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float2 uv : TEXCOORD0;
			};

			struct v2g
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float2 uv : TEXCOORD0;
				
			};

			struct g2f
			{
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
				float3 normal : TEXCOORD1;
				float4 color : TEXCOORD2;

			};



            sampler2D _MainTex;
            float4 _MainTex_ST;

			v2g vert(appdata v)
			{
				v2g o;
				o.vertex = v.vertex;
				o.normal = v.normal;
				o.uv = v.uv;
				return o;
			}


			float _extrude;
			float _frequence;
			fixed4 _color;
			fixed4 _bottomColor;
			fixed4 _topColor;
			float _timeSpeed;
			float _Threshold;
			float _NoisePeriod;
			float _TimeSpeed;
			float _Amp;
			[maxvertexcount(21)]
			void geom(triangle v2g input[3], inout TriangleStream<g2f> OutputStream)
			{
				//面ごとのoffSetを計算
				float r = snoise(float4((input[0].vertex.xyz * _NoisePeriod + 
										 input[1].vertex.xyz * _NoisePeriod + 
										 input[2].vertex.xyz  * _NoisePeriod),
										 _Time.y * _TimeSpeed));
				float mode = rnd(float2(_Time.x, _Time.x)) * 1.0;

				float3 n;
				

				float offSetAndExtrude = 1.0;
				//上面
				float4 upPos[3];
				g2f o;
				for (int i = 0; i < 3; i++) {


					if (r > _Threshold) {
						if (mode < 1.0) {
							n = float3(rnd(float2(_Time.y, _Time.y)), 0.0, 0.0) * _Amp;
						}
						else if (1 < mode && mode < 3) {
							n = float3(0.0, rnd(float2(_Time.y, _Time.y)), 0.0) * _Amp;
						}else if(mode > 10) {
							n = normalize(mul(input[i].normal, (float3x3)unity_WorldToObject)) * _Amp;
						}

					}
					else {
						n = float3(0.0.xxx);
					}
					
					
					
					float3 extrudeVal = n * offSetAndExtrude * r;
					float4 v = float4(float3(input[i].vertex.xyz + extrudeVal), 1.0);
					upPos[i] = v;
					o.vertex = UnityObjectToClipPos(v);
					o.uv = input[i].uv;
					o.normal = input[i].normal;
					o.color = _topColor;
					OutputStream.Append(o);
				}
				OutputStream.RestartStrip();



				//side--------------------------------------
				for (int i = 0; i < 3; i++) {

					o.vertex = UnityObjectToClipPos(input[i].vertex);
					o.uv = input[i].uv;
					o.color = _bottomColor;
					o.normal = input[i].normal;
					OutputStream.Append(o);

					o.vertex = UnityObjectToClipPos(input[fmod(i + 1, 3)].vertex);
					o.uv = input[fmod(i + 1, 3)].uv;
					o.color = _bottomColor;
					o.normal = input[fmod(i + 1, 3)].normal;
					OutputStream.Append(o);

					o.vertex = UnityObjectToClipPos(upPos[i]);
					o.uv = input[i].uv;
					o.color = _topColor;
					o.normal = input[i].normal;
					OutputStream.Append(o);

					OutputStream.RestartStrip();
					//--------------------------------

					o.vertex = UnityObjectToClipPos(input[fmod(i + 1, 3)].vertex);
					o.uv = input[fmod(i + 1, 3)].uv;
					o.color = _bottomColor;
					o.normal = input[fmod(i + 1, 3)].normal;
					OutputStream.Append(o);

					o.vertex = UnityObjectToClipPos(upPos[fmod(i + 1, 3)]);
					o.uv = input[fmod(i + 1, 3)].uv;
					o.color = _topColor;
					o.normal = input[fmod(i + 1, 3)].normal;
					OutputStream.Append(o);

					o.vertex = UnityObjectToClipPos(upPos[i]);
					o.uv = input[i].uv;
					o.color = _topColor;
					o.normal = input[i].normal;
					OutputStream.Append(o);

					OutputStream.RestartStrip();
					//--------------------------------

				}

				
			}

            fixed4 frag (g2f i) : SV_Target
            {
                // sample the texture
				//fixed4 col = tex2D(_MainTex, i.uv);
				fixed4 col = i.color;
				col.rgb = i.normal;
                // apply fog
                //UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}