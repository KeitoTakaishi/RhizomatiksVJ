float rnd(float2 p){
    return frac(sin(dot(p ,float2(12.9898,78.233))) * 43758.5453);
}

// 乱数生成器（その２）
float rnd2(float2 n){
    float a = 0.129898;
    float b = 0.78233;
    float c = 437.585453;
    float dt= dot(n ,float2(a, b));
    float sn= fmod(dt, 3.14);
    return frac(sin(sn) * c);
}


float3 rnd3d(float2 p) {
	return  float3( frac(sin(dot(p, float2(12.9898, 78.233))) * 43758.5453),
					frac(sin(dot(p, float2(14754.9898, 78.233))) * 9676758.5453),
					frac(sin(dot(p, float2(452.9898, 78.233))) * 2488.5453));
}