Shader "Instanced/GPUPartcileSystem_CustomShader" {
	Properties{
		_MainTex("Albedo (RGB)", 2D) = "white" {}
	}
		SubShader{
			Tags { "RenderType" = "Opaque" }


			CGINCLUDE
			#include "UnityCG.cginc"
			#include "UnityLightingCommon.cginc"
		//#include "AutoLight.cginc"
		#pragma multi_compile_fog

			sampler2D _MainTex;

			#if SHADER_TARGET >= 45
			StructuredBuffer<float4> positionBuffer;
			#endif

			struct v2f
			{
				float4 pos : SV_POSITION;
				float2 uv_MainTex : TEXCOORD0;
				float3 ambient : TEXCOORD1;
				float3 diffuse : TEXCOORD2;
				float3 color : TEXCOORD3;
				UNITY_FOG_COORDS(4)
					//SHADOW_COORDS(4)
				};

				void rotate2D(inout float2 v, float r)
				{
					float s, c;
					sincos(r, s, c);
					v = float2(v.x * c - v.y * s, v.x * s + v.y * c);
				}

				//----------------------------------------------------------------
				// 1pass
				//ポジションを posという名前にしなければ計算できない
				//----------------------------------------------------------------
				v2f vert(appdata_full v, uint instanceID : SV_InstanceID)
				{
				#if SHADER_TARGET >= 45
					float4 data = positionBuffer[instanceID];
				#else
					float4 data = 0;
				#endif

					float rotation = data.w * data.w * _Time.x * 0.5f;
					rotate2D(data.xz, rotation);


					//モデルの大きさがn倍になればローカル座標もn倍になるのは普通
					float3 localPosition = v.vertex.xyz * data.w;
					float3 worldPosition = data.xyz + localPosition;
					float3 worldNormal = v.normal;



					//法線とライトの内積計算, saturateは0 ~ 1でclampしてくれるやつ
					float3 ndotl = saturate(dot(worldNormal, _WorldSpaceLightPos0.xyz));
					//環境光
					float3 ambient = ShadeSH9(float4(worldNormal, 1.0f));
					//拡散
					float3 diffuse = (ndotl * _LightColor0.rgb);
					float3 color = v.color;

					v2f o;
					o.pos = mul(UNITY_MATRIX_VP, float4(worldPosition, 1.0f));
					o.uv_MainTex = v.texcoord;
					o.ambient = ambient;
					o.diffuse = diffuse;
					o.color = color;
					//陰影計算の結果が入る？
					//TRANSFER_SHADOW(o);
					UNITY_TRANSFER_FOG(o, o.pos);

					return o;
				}
				//----------------------------------------------------------------
				fixed4 frag(v2f i) : SV_Target
				{
					//fixed shadow = SHADOW_ATTENUATION(i);
					fixed4 albedo = tex2D(_MainTex, i.uv_MainTex);
				//float3 lighting = i.diffuse * shadow + i.ambient;
				float3 lighting = i.diffuse + i.ambient;
				fixed4 output = fixed4(albedo.rgb * i.color * lighting, albedo.w);
				UNITY_APPLY_FOG(i.fogCoord, output);
				return output;
			}

					//----------------------------------------------------------------
					// 2pass
					//----------------------------------------------------------------

					struct v2f_shadow {
						V2F_SHADOW_CASTER;
					};

					v2f_shadow vert_shadow(appdata_base v, uint instanceID : SV_InstanceID)
					{
						//InsObj obj;
					#if SHADER_TARGET >= 45
						float4 data = positionBuffer[instanceID];
					#endif
						float rotation = data.w * data.w * _Time.x * 0.5f;
						rotate2D(data.xz, rotation);
						float3 localPosition = v.vertex.xyz * data.w;
						float3 worldPosition = data.xyz + localPosition;

						v2f_shadow o;
						//??
						TRANSFER_SHADOW_CASTER_NORMALOFFSET(o)
						o.pos = mul(UNITY_MATRIX_VP, float4(worldPosition, 1.0f));
						return o;
					}

					float4 frag_shadow(v2f_shadow i) : SV_Target
					{
						//??
						SHADOW_CASTER_FRAGMENT(i)
					}

				ENDCG

					Pass
					{
						Tags{ "LightMode" = "ForwardBase" }
					CGPROGRAM
					#pragma vertex vert
					#pragma fragment frag
					#pragma multi_compile_fwdbase nolightmap nodirlightmap nodynlightmap novertexlight
					#pragma target 4.5
					ENDCG
					}


					Pass
					{
						Tags {"LightMode" = "ShadowCaster"}

						CGPROGRAM
						#pragma vertex vert_shadow
						#pragma fragment frag_shadow
						#pragma multi_compile_shadowcaster
						#pragma target 4.5
						ENDCG
					}



	}
}