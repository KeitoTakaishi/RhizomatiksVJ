Shader "Hidden/ExtrudeNormal"
{
    Properties
    {
		[HDR]
		_topColor("TopColor", Color) = (0, 0, 0, 0)
		[HDR]
		_bottomColor("BottomColor", Color) = (0, 0, 0, 0)
		_color("Color", Color) = (0, 0, 0, 0)

		_colorMode("ColorMode", Int) = 0
		_extrude("Extrude", Range(-10.0, 10.0)) = 1.0
		_frequence("Frequence", Range(0.0, 10.0)) = 1.0
		
		_timeSpeed("TimeSpeed", Range(0.0, 10.0)) = 1.0
    }

    SubShader
    {
        //Cull Off ZWrite On ZTest Always

        Pass
        {
            CGPROGRAM
			#include "UnityCG.cginc"
			#include "SimplexNoise4D.cginc"

            #pragma vertex vert
			#pragma geometry geom
			#pragma fragment frag

            struct appdata
            {
                float4 vertex : POSITION;
				float3 normal : NORMAL;
				//float2 uv : TEXCOORD0;
            };

            struct v2g
            {
                float4 vertex : POSITION;
				float3 normal : NORMAL;
            };

			struct g2f
			{
				float4 vertex : SV_POSITION;
				float4 color : TEXCOORD0;
			};

			float rand(float3 co) {
				return frac(sin(dot(co.xyz, float3(12.9898, 78.233, 34.2378))) * 43758.5453);
			}

            v2g vert (appdata v)
            {
                v2g o;
				o.vertex = v.vertex;
				o.normal = v.normal;
                return o;
            }

			
			float _extrude;
			float _frequence;
			fixed4 _color;
			fixed4 _bottomColor;
			fixed4 _topColor;
			float _timeSpeed;
			[maxvertexcount(21)]
			void geom(triangle v2g input[3], inout TriangleStream<g2f> OutputStream)
			{

				//float rnd = rand(3.0*(input[0].vertex.xyz + input[1].vertex.xyz + input[2].vertex.xyz));
				
				//面ごとのoffSetを計算
				float rnd = simplexNoise(float4((input[0].vertex.xyz + input[1].vertex.xyz + input[2].vertex.xyz)*1.0, 1.0));
				//sinで揺らす
				float time = _Time.y * _timeSpeed;
				rnd += 0.5*(1.0 + 1.0*sin(time + rnd*100.0));
				
				
				//音量とリンク，最大振幅
				float offSetAndExtrude = _extrude;
				

				//上面
				float4 upPos[3];
				g2f o;
				for (int i = 0; i < 3; i++) {
					float3 n = normalize(mul(input[i].normal, (float3x3)unity_WorldToObject));
					float3 extrudeVal = n * offSetAndExtrude * sin(rnd  + time) * rnd;
					//float4 v = float4(float3(input[i].vertex.xyz + n * 0.5*offSetAndExtrude * sin(_Time.z*_frequence)  * rnd), 1.0);
					float4 v = float4(float3(input[i].vertex.xyz + extrudeVal) , 1.0);
					upPos[i] = v;
					o.vertex = UnityObjectToClipPos(v);
					o.color = _topColor;
					OutputStream.Append(o);
				}
				OutputStream.RestartStrip();

				for (int i = 0; i < 3; i++) {
					//side
					o.vertex = UnityObjectToClipPos(input[i].vertex);
					o.color = _bottomColor;
					OutputStream.Append(o);

					o.vertex = UnityObjectToClipPos(input[ fmod(i+1, 3) ].vertex);
					o.color = _bottomColor;
					OutputStream.Append(o);

					o.vertex = UnityObjectToClipPos(upPos[i]);
					o.color = _topColor;
					OutputStream.Append(o);

					OutputStream.RestartStrip();
					//--------------------------------

					o.vertex = UnityObjectToClipPos(input[fmod(i + 1, 3)].vertex);
					o.color = _bottomColor;
					OutputStream.Append(o);
				
					o.vertex = UnityObjectToClipPos(upPos[fmod(i + 1, 3)]);
					o.color = _topColor;
					OutputStream.Append(o);

					o.vertex = UnityObjectToClipPos(upPos[i]);
					o.color = _topColor;
					OutputStream.Append(o);

					OutputStream.RestartStrip();
					//--------------------------------

				}
			}

			int _colorMode;
            fixed4 frag (g2f i) : SV_Target
            {
				fixed4 col;
			if (_colorMode == 0) {
				col = i.color;
			}
			else {
				col = i.color * _color;
			}
               
                return col;
            }
            ENDCG
        }
    }
}
