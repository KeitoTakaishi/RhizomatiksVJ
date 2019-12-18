Shader "Hidden/Doom_MultiLight_FlatShading"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_BumpTex("BumpTexture", 2D) = "white" {}
		[HDR]
		_Color("Color", Color) = (1.0, 1.0, 1.0, 1.0)
		
		_FinnessY("FinnessY", Range(0.0, 50.0)) = 1.0
		_TimeSpeed("TimeSpeed", Range(0.0, 10.0)) = 1.0
		_AttenuationCoef("AttenuationCoef", Range(0.0, 300.0)) = 10.0
		_Amp("Amp", Range(0.0, 100.0)) = 1.0
		_AmpScaleX("AmpScaleX", Range(0.001, 1.0)) = 1.0
		_AmpScaleY("AmpScaleY", Range(0.001, 1.0)) = 1.0
		_AmpScaleZ("AmpScaleZ", Range(0.001, 1.0)) = 1.0
		_LargeAmp("LargeAmp", Range(0.0, 100.0)) = 1.0
		_TimeScaleLarge("TimeScaleLarge", Range(0.0, 10.0)) = 1.0
		_AmpLargeScaleX("AmpScaleX", Range(0.001, 1.0)) = 1.0
		_AmpLargeScaleY("AmpScaleY", Range(0.001, 1.0)) = 1.0
		_AmpLargeScaleZ("AmpScaleZ", Range(0.001, 1.0)) = 1.0
    }
    SubShader
    {
		Tags { "RenderType" = "Opaque" }
		Cull Off 
        Pass
        {

			//FrowardBaseにすることでLightの情報がshaderに流される
			//Light RenderMode importantはmadxで4つまでで，ピクセルライティングされる
			Tags { "LightMode" = "ForwardBase" }
			Cull Off 
            CGPROGRAM
            #pragma vertex vert
			#pragma geometry geom
            #pragma fragment frag
			#pragma multi_compile_fwdbase

            #include "UnityCG.cginc"
			#include "Noise4D.cginc"

			//-----------------------------------
			//noise
			float3 snoise3D(float4 x)
			{
				float s = snoise(x);
				float s1 = snoise(float4(x.y - 19.1, x.z + 33.4, x.x + 47.2, x.w));
				float s2 = snoise(float4(x.z + 74.2, x.x - 124.5, x.y + 99.4, x.w));
				float3 c = float3(s, s1, s2);
				return c;
			}

			float3 curlNoise(float4 p) {

				const float e = 0.0009765625;
				float4 dx = float4(e, 0.0, 0.0, 0.0);
				float4 dy = float4(0.0, e, 0.0, 0.0);
				float4 dz = float4(0.0, 0.0, e, 0.0);

				float3 p_x0 = snoise3D(p - dx);
				float3 p_x1 = snoise3D(p + dx);
				float3 p_y0 = snoise3D(p - dy);
				float3 p_y1 = snoise3D(p + dy);
				float3 p_z0 = snoise3D(p - dz);
				float3 p_z1 = snoise3D(p + dz);

				float x = p_y1.z - p_y0.z - p_z1.y + p_z0.y;
				float y = p_z1.x - p_z0.x - p_x1.z + p_x0.z;
				float z = p_x1.y - p_x0.y - p_y1.x + p_y0.x;

				const float divisor = 1.0 / (2.0 * e);
				return normalize(float3(x, y, z) * divisor);
			}
			//-----------------------------------


            struct appdata
            {
                float4 vertex : POSITION;
				float3 normal : NORMAL;
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
				float3 ambient: TEXCOORD1;
				
				
			};

            v2g vert (appdata v)
            {
                v2g o;
                o.vertex = v.vertex ;
				o.normal = v.normal;
                return o;
            }

			float4 _Color;
			float _Amp;
			float _FinnessY;
			float _TimeSpeed;
			float _AttenuationCoef;
			float _AmpScaleX, _AmpScaleY, _AmpScaleZ;
			float _LargeAmp;
			float _TimeScaleLarge;
			float _AmpLargeScaleX, _AmpLargeScaleY, _AmpLargeScaleZ;
			[maxvertexcount(3)]
			void geom(triangle v2g input[3], inout TriangleStream<g2f> OutputStream)
			{

				float3 vecA = input[1].vertex - input[0].vertex;
				float3 vecB = input[2].vertex - input[0].vertex;
				float3 normal = cross(vecA.xyz, vecB.xyz);
				normal = normalize(mul(normal, (float3x3)unity_WorldToObject));
				
				float offSet = 100.0;
				//float interval = 3000.0;
				//float t = _Time.z * 50.0;

				float amp = _Amp * curlNoise(float4(input[0].vertex.x * _AmpScaleX, input[0].vertex.y * _AmpScaleY, input[0].vertex.z * _AmpScaleZ, _Time.y * _TimeSpeed));
				//float largeAmp = _LargeAmp * curlNoise(float4(input[0].vertex.x * _AmpLargeScaleX, input[0].vertex.y * _AmpLargeScaleY, input[0].vertex.z * _AmpLargeScaleZ, _Time.y * _TimeSpeed));
				//float largeNoise = curlNoise(float4(input[0].vertex.x * _AmpLargeScaleX, input[0].vertex.y * _AmpLargeScaleY, input[0].vertex.z * _AmpLargeScaleZ, _Time.y * _TimeScaleLarge)) * _LargeAmp;
				float3 sinAmp = _LargeAmp * normal * sin(_Time.y + input[0].vertex.y * _AmpLargeScaleY);
				float3 v0 = input[0].vertex.xyz + curlNoise(float4(input[0].vertex.xyz * _FinnessY, _Time.y * _TimeSpeed)) * amp + sinAmp;

				amp = _Amp * curlNoise(float4(input[1].vertex.x * _AmpScaleX, input[1].vertex.y * _AmpScaleY, input[1].vertex.z * _AmpScaleZ, _Time.y * _TimeSpeed));
				sinAmp = _LargeAmp * normal * sin(_Time.y + input[1].vertex.y * _AmpLargeScaleY);
				//largeAmp = _LargeAmp * curlNoise(float4(input[0].vertex.x * _AmpLargeScaleX, input[0].vertex.y * _AmpLargeScaleY, input[0].vertex.z * _AmpLargeScaleZ, _Time.y * _TimeSpeed));
				//largeNoise = curlNoise(float4(input[1].vertex.x * _AmpLargeScaleX, input[1].vertex.y * _AmpLargeScaleY, input[1].vertex.z * _AmpLargeScaleZ, _Time.y * _TimeScaleLarge)) * _LargeAmp;
				float3 v1 = input[1].vertex.xyz + curlNoise(float4(input[1].vertex.xyz * _FinnessY, _Time.y * _TimeSpeed)) * amp + sinAmp;

				amp = _Amp * curlNoise(float4(input[2].vertex.x * _AmpScaleX, input[2].vertex.y * _AmpScaleY, input[2].vertex.z * _AmpScaleZ, _Time.y * _TimeSpeed));
				sinAmp = _LargeAmp * normal * sin(_Time.y + input[2].vertex.y * _AmpLargeScaleY);
				//largeAmp = _LargeAmp * curlNoise(float4(input[0].vertex.x * _AmpLargeScaleX, input[0].vertex.y * _AmpLargeScaleY, input[0].vertex.z * _AmpLargeScaleZ, _Time.y * _TimeSpeed));
				//largeNoise = curlNoise(float4(input[2].vertex.x * _AmpLargeScaleX, input[2].vertex.y * _AmpLargeScaleY, input[2].vertex.z * _AmpLargeScaleZ, _Time.y * _TimeScaleLarge)) * _LargeAmp;
				float3 v2 = input[2].vertex.xyz + curlNoise(float4(input[2].vertex.xyz * _FinnessY, _Time.y * _TimeSpeed)) * amp + sinAmp;
				

				vecA = v1 - v0;
				vecB = v2 - v0;
				normal = cross(vecA.xyz, vecB.xyz);
				normal = normalize(mul(normal, (float3x3)unity_WorldToObject));



				float3 center = (v0 + v1 + v2) / 3.0;
				float3 worldCenter = mul(unity_ObjectToWorld, center);


				float3 lightDir0 = normalize(_WorldSpaceLightPos0.xyz);
				//float3 lightDir1 = normalize(_WorldSpaceLightPos1.xyz - worldCenter);


				half3 ambient = half3(0.0, 0.0, 0.0);
				#if UNITY_SHOULD_SAMPLE_SH

					#if defined(VERTEXLIGHT_ON)

					ambient = Shade4PointLights(
					unity_4LightPosX0, unity_4LightPosY0, unity_4LightPosZ0,
					unity_LightColor[0].rgb, unity_LightColor[1].rgb,
					unity_LightColor[2].rgb, unity_LightColor[3].rgb,
					unity_4LightAtten0, worldCenter, normal
					);
					#endif

					//ambient += max(0, ShadeSH9(float4(normal, 1)));
				#else
				ambient = 0;
				#endif

				float dist = length(_WorldSpaceLightPos0.xyz - worldCenter);

				
				float4 col = max(0.0, dot(normal, lightDir0)) * _AttenuationCoef * 1.0/dist;
				//col = max(0.0, dot(normal, lightDir1));



				g2f o;

				//o.vertex = UnityObjectToClipPos(input[0].vertex);
				o.vertex = UnityObjectToClipPos(float4(v0, 1.0));
				o.color = col;
				o.ambient = ambient;
				OutputStream.Append(o);
				
				//o.vertex = UnityObjectToClipPos(input[1].vertex);
				o.vertex = UnityObjectToClipPos(float4(v1, 1.0));
				o.color = col;
				o.ambient = ambient;
				OutputStream.Append(o);
				
				//o.vertex = UnityObjectToClipPos(input[2].vertex);
				o.vertex = UnityObjectToClipPos(float4(v2, 1.0));
				o.color = col;
				o.ambient = ambient;
				OutputStream.Append(o);

				OutputStream.RestartStrip();

				//o.color.xyz += float3(curlNoise(float4(center, _Time.x)));
			}

			half4 _LightColor0;
			fixed4 frag(g2f i) : SV_Target
			{
				//fixed4 col = i.color * _LightColor0;
				fixed4 col = _Color * i.color * _LightColor0;
				col.rgb += i.ambient;
				
				return col;
			}
            ENDCG
        }

		Pass {

			Tags { "LightMode" = "ForwardAdd" }

			Blend One One
			Cull Off 

			CGPROGRAM
		   #pragma vertex vert
		   #pragma geometry geom
		   #pragma fragment frag

		   #pragma multi_compile_fwdadd

		   #include "UnityCG.cginc"
		   #include "AutoLight.cginc"
		   //#include "SimplexNoise4D.cginc"
		   #include "Noise4D.cginc"



			//-----------------------------------
			//noise
			float3 snoise3D(float4 x)
			{
				float s = snoise(x);
				float s1 = snoise(float4(x.y - 19.1, x.z + 33.4, x.x + 47.2, x.w));
				float s2 = snoise(float4(x.z + 74.2, x.x - 124.5, x.y + 99.4, x.w));
				float3 c = float3(s, s1, s2);
				return c;
			}

			float3 curlNoise(float4 p) {

				const float e = 0.0009765625;
				float4 dx = float4(e, 0.0, 0.0, 0.0);
				float4 dy = float4(0.0, e, 0.0, 0.0);
				float4 dz = float4(0.0, 0.0, e, 0.0);

				float3 p_x0 = snoise3D(p - dx);
				float3 p_x1 = snoise3D(p + dx);
				float3 p_y0 = snoise3D(p - dy);
				float3 p_y1 = snoise3D(p + dy);
				float3 p_z0 = snoise3D(p - dz);
				float3 p_z1 = snoise3D(p + dz);

				float x = p_y1.z - p_y0.z - p_z1.y + p_z0.y;
				float y = p_z1.x - p_z0.x - p_x1.z + p_x0.z;
				float z = p_x1.y - p_x0.y - p_y1.x + p_y0.x;

				const float divisor = 1.0 / (2.0 * e);
				return normalize(float3(x, y, z) * divisor);
			}
			//-----------------------------------

			struct appdata
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
			};

			struct v2g
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;

			};

			struct g2f
			{
				float4 vertex : SV_POSITION;
				float3 diff : TEXCOORD0;

			};


			sampler2D _MainTex;
			half4 _MainTex_ST;
			half4 _LightColor0;

			v2g vert(appdata v)
			{
				v2g o;

				o.vertex = v.vertex;
				o.normal = v.normal;

				return o;
			}
			float4 _Color;
			float _Amp;
			float _FinnessY;
			float _TimeSpeed;
			float _AmpScaleX, _AmpScaleY, _AmpScaleZ;
			float _LargeAmp;
			float _TimeScaleLarge;
			float _AmpLargeScaleX, _AmpLargeScaleY, _AmpLargeScaleZ;
			[maxvertexcount(3)]
			void geom(triangle v2g input[3], inout TriangleStream<g2f> OutputStream)
			{

				float3 vecA = input[1].vertex - input[0].vertex;
				float3 vecB = input[2].vertex - input[0].vertex;
				float3 normal = cross(vecA.xyz, vecB.xyz);
				normal = normalize(mul(normal, (float3x3)unity_WorldToObject));

				
				float offSet = 100.0;
				//float interval = 3000.0;
				//float t = _Time.z * 50.0;
				float amp = _Amp * curlNoise(float4(input[0].vertex.x * _AmpScaleX, input[0].vertex.y * _AmpScaleY, input[0].vertex.z * _AmpScaleZ, _Time.y * _TimeSpeed));
				//float largeAmp = _LargeAmp * curlNoise(float4(input[0].vertex.x * _AmpLargeScaleX, input[0].vertex.y * _AmpLargeScaleY, input[0].vertex.z * _AmpLargeScaleZ, _Time.y * _TimeSpeed));
				//float largeNoise = curlNoise(float4(input[0].vertex.x * _AmpLargeScaleX, input[0].vertex.y * _AmpLargeScaleY, input[0].vertex.z * _AmpLargeScaleZ, _Time.y * _TimeScaleLarge)) * _LargeAmp;
				float3 sinAmp = _LargeAmp * normal * sin(_Time.y + input[0].vertex.y * _AmpLargeScaleY);
				float3 v0 = input[0].vertex.xyz + curlNoise(float4(input[0].vertex.xyz * _FinnessY, _Time.y * _TimeSpeed)) * amp + sinAmp;

				amp = _Amp * curlNoise(float4(input[1].vertex.x * _AmpScaleX, input[1].vertex.y * _AmpScaleY, input[1].vertex.z * _AmpScaleZ, _Time.y * _TimeSpeed));
				sinAmp = _LargeAmp * normal * sin(_Time.y + input[1].vertex.y * _AmpLargeScaleY);
				//largeAmp = _LargeAmp * curlNoise(float4(input[0].vertex.x * _AmpLargeScaleX, input[0].vertex.y * _AmpLargeScaleY, input[0].vertex.z * _AmpLargeScaleZ, _Time.y * _TimeSpeed));
				//largeNoise = curlNoise(float4(input[1].vertex.x * _AmpLargeScaleX, input[1].vertex.y * _AmpLargeScaleY, input[1].vertex.z * _AmpLargeScaleZ, _Time.y * _TimeScaleLarge)) * _LargeAmp;
				float3 v1 =input[1].vertex.xyz + curlNoise(float4(input[1].vertex.xyz * _FinnessY, _Time.y * _TimeSpeed)) * amp  + sinAmp;

				amp = _Amp * curlNoise(float4(input[2].vertex.x * _AmpScaleX, input[2].vertex.y * _AmpScaleY, input[2].vertex.z * _AmpScaleZ, _Time.y * _TimeSpeed));
				sinAmp = _LargeAmp * normal * sin(_Time.y + input[2].vertex.y * _AmpLargeScaleY);
				//largeAmp = _LargeAmp * curlNoise(float4(input[0].vertex.x * _AmpLargeScaleX, input[0].vertex.y * _AmpLargeScaleY, input[0].vertex.z * _AmpLargeScaleZ, _Time.y * _TimeSpeed));
				//largeNoise = curlNoise(float4(input[2].vertex.x * _AmpLargeScaleX, input[2].vertex.y * _AmpLargeScaleY, input[2].vertex.z * _AmpLargeScaleZ, _Time.y * _TimeScaleLarge)) * _LargeAmp;
				float3 v2 = input[2].vertex.xyz + curlNoise(float4(input[2].vertex.xyz * _FinnessY, _Time.y * _TimeSpeed)) * amp + sinAmp;


				vecA = v1 - v0;
				vecB = v2 - v0;
				normal = cross(vecA.xyz, vecB.xyz);
				normal = normalize(mul(normal, (float3x3)unity_WorldToObject));
				
				float3 center = (v0 + v1 +v2) / 3.0;
				float3 worldCenter = mul(unity_ObjectToWorld, center );


				half3 lightDir;
				if (_WorldSpaceLightPos0.w > 0) {
					lightDir = _WorldSpaceLightPos0.xyz - worldCenter;
				}
				else {
					lightDir = _WorldSpaceLightPos0.xyz;
				}
				lightDir = normalize(lightDir);

				half3 diff = max(0, dot(normal, lightDir)) * _LightColor0;
				
				



				g2f o;
				//o.vertex = UnityObjectToClipPos(input[0].vertex);
				o.vertex = UnityObjectToClipPos(float4(v0 + float3(0.0, 0.0, 0.0), 1.0));
				o.diff = diff;
				OutputStream.Append(o);

				//o.vertex = UnityObjectToClipPos(input[1].vertex);
				o.vertex = UnityObjectToClipPos(float4(v1 + float3(0.0, 0.0, 0.0), 1.0));
				o.diff = diff;
				OutputStream.Append(o);

				//o.vertex = UnityObjectToClipPos(input[2].vertex);
				o.vertex = UnityObjectToClipPos(float4(v2 + float3(0.0, 0.0, 0.0), 1.0));
				o.diff = diff;
				OutputStream.Append(o);

				OutputStream.RestartStrip();

				//o.diff.xyz += float3(curlNoise(float4(center*100.0, _Time.x)));

			}

			half4 frag(g2f i) : COLOR
			{
				half4 col = _Color;
				// _WorldSpaceLightPos0.wはディレクショナルライトだったら0、それ以外は1となる
				
				float3 v = i.vertex.xyz;
				col.rgb *= i.diff;

				//col.rgb *= i.diff;
				return col;
			}
			ENDCG
		}
	}
}
