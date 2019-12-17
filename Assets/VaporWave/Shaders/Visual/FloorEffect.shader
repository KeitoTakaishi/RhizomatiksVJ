Shader "Custom/FloorEffect"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0
		_ScrollSpeed("ScrollSpeed", Range(-1.0,1.0)) = -0.4
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows vertex:vert
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
        };

		float low;
		void vert(inout appdata_full v) {
			float3 normal = v.normal;
			float4 vert =  v.vertex;

			float period = 10.0;
			vert.y = low * normal.y * sin(_Time.y * 1.0 + vert.x * period + vert.z * period);
			v.vertex = vert;

		}

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;
		float _ScrollSpeed;
        UNITY_INSTANCING_BUFFER_START(Props)
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
			float2 uv = IN.uv_MainTex;
			uv.y = frac(uv.y + _Time.y * _ScrollSpeed);
            fixed4 c = tex2D (_MainTex, uv) * _Color;
            o.Albedo = c.rgb;
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
