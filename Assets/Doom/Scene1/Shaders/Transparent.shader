Shader "Custom/Transoarent"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0

		[HDR]
		_SupportColor("SupportColor", Color) = (1,1,1,1)
		_randomScale("RandomScale", float) = 1.0
    }
    SubShader
    {
        Tags { "Queue" = "Transparent" "RenderType"="Transparent" }
        LOD 200
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha
        CGPROGRAM
       
		
        #pragma surface surf Standard fullforwardshadows alpha:fade 

        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
        };

        half _Glossiness;
        half _Metallic;
		fixed4 _Color;
		fixed4 _SupportColor;
        UNITY_INSTANCING_BUFFER_START(Props)
        UNITY_INSTANCING_BUFFER_END(Props)


		float rand(float2 texCoord, int Seed)
		{
			return frac(sin(dot(texCoord.xy, float2(12.9898, 78.233)) + Seed) * 43758.5453);
		}

		int mode = 1;
		float _randomScale;
        void surf (Input IN, inout SurfaceOutputStandard o)
        {
			mode = 0;
            fixed4 c =  _Color;
			float2 uv = IN.uv_MainTex;
			if(mode == 0){
				float rs = _randomScale * 10.0 + 1.0;
				uv = floor(uv *  rs )/ rs;
			}
			else if (mode == 1) {
				uv.x = floor(uv.x * 50.0) / 50.0;
			}
			
			float random = rand(uv, _Time.y*100.0);
            o.Albedo = float3(random, random, random) * _SupportColor.rgb;
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
