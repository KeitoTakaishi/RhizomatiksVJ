#ifndef SIMPLEX_NOISE_4D
#define SIMPLEX_NOISE_4D

// https://github.com/stegu/webgl-noise/blob/master/src/noise4D.glsl
//
// Description : Array and textureless GLSL 2D simplex noise function.
//      Author : Ian McEwan, Ashima Arts.
//  Maintainer : stegu
//     Lastmod : 20110822 (ijm)
//     License : Copyright (C) 2011 Ashima Arts. All rights reserved.
//               Distributed under the MIT License. See LICENSE file.
//               https://github.com/ashima/webgl-noise
//               https://github.com/stegu/webgl-noise

float4 mod2894D(float4 x)
{
	return x - floor(x * (1.0 / 289.0)) * 289.0;
}

float mod289(float x)
{
	return x - floor(x * (1.0 / 289.0)) * 289.0;
}

float4 permute4D(float4 x)
{
	return mod2894D(((x * 34.0) + 1.0) * x);
}

float permute(float x)
{
	return mod289(((x * 34.0) + 1.0) * x);
}

float taylorInvSqrt(float r)
{
	return 1.79284291400159 - 0.85373472095314 * r;
}

float4 taylorInvSqrt4D(float4 r)
{
	return 1.79284291400159 - 0.85373472095314 * r;
}

float4 grad4(float j, float4 ip)
{
	const float4 ones = float4(1.0, 1.0, 1.0, -1.0);
	float4 p, s;
	p.xyz = floor(frac((float3)j * ip.xyz) * 7.0) * ip.z - 1.0;
	p.w = 1.5 - dot(abs(p.xyz), ones.xyz);
	// lessthan GLSL -> HLSL
	// https://gist.github.com/fadookie/25adf86ae7e2753d717c
	s = float4(1.0 - step((float4)0.0, p));
	p.xyz = p.xyz + (s.xyz * 2.0 - 1.0) * s.www;
	return p;
}

#define F4 0.309016994374947451

float simplexNoise(float4 v)
{
	const float4 C = float4(
		0.138196601125011,   // (5 - sqrt(5))/20  G4
		0.276393202250021,   // 2 * G4
		0.414589803375032,   // 3 * G4
		-0.447213595499958); // -1 + 4 * G4

	float4 i = floor(v + dot(v, (float4)F4)); // 変形した座標の整数部
	float4 x0 = v - i + dot(i, C.xxxx);       // 単体1つめの頂点

	float4 i0;
	float3 isX = step(x0.yzw, x0.xxx);
	float3 isYZ = step(x0.zww, x0.yyz);
	i0.x = isX.x + isX.y + isX.z;
	i0.yzw = 1.0 - isX;
	i0.y += isYZ.x + isYZ.y;
	i0.zw += 1.0 - isYZ.xy;
	i0.z += isYZ.z;
	i0.w += 1.0 - isYZ.z;

	float4 i3 = clamp(i0, 0.0, 1.0);
	float4 i2 = clamp(i0 - 1.0, 0.0, 1.0);
	float4 i1 = clamp(i0 - 2.0, 0.0, 1.0);

	float4 x1 = x0 - i1 + 1.0 * C.xxxx;  // 単体2つめの頂点
	float4 x2 = x0 - i2 + 2.0 * C.xxxx;	 // 単体3つめの頂点
	float4 x3 = x0 - i3 + 3.0 * C.xxxx;	 // 単体4つめの頂点
	float4 x4 = x0 - 1. + 4.0 * C.xxxx;  // 単体5つめの頂点


	i = mod2894D(i);
	float  j0 = permute(permute(permute(permute(i.w) + i.z) + i.y) + i.x);
	float4 j1 = permute(permute(permute(permute(
		i.w + float4(i1.w, i2.w, i3.w, 1.0))
		+ i.z + float4(i1.z, i2.z, i3.z, 1.0))
		+ i.y + float4(i1.y, i2.y, i3.y, 1.0))
		+ i.x + float4(i1.x, i2.x, i3.x, 1.0));

	float4 ip = float4(1.0 / 294.0, 1.0 / 49.0, 1.0 / 7.0, 0.0);

	float4 p0 = grad4(j0, ip);
	float4 p1 = grad4(j1.x, ip);
	float4 p2 = grad4(j1.y, ip);
	float4 p3 = grad4(j1.z, ip);
	float4 p4 = grad4(j1.w, ip);

	float4 norm = taylorInvSqrt(float4(dot(p0, p0), dot(p1, p1), dot(p2, p2), dot(p3, p3)));
	p0 *= norm.x;
	p1 *= norm.y;
	p2 *= norm.z;
	p3 *= norm.w;
	p4 *= taylorInvSqrt(dot(p4, p4));

	float3 m0 = max(0.6 - float3(dot(x0, x0), dot(x1, x1), dot(x2, x2)), 0.0);
	float2 m1 = max(0.6 - float2(dot(x3, x3), dot(x4, x4)), 0.0);
	m0 = m0 * m0;
	m1 = m1 * m1;
	return 49.0 * (
		dot(m0 * m0, float3(dot(p0, x0), dot(p1, x1), dot(p2, x2)))
		+ dot(m1 * m1, float2(dot(p3, x3), dot(p4, x4)))
		);
}

#endif