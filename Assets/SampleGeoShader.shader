Shader "Custom/GeometryShaderTest1" {
	Properties{
		_Color("Color", Color) = (1,1,1,1)
		_Amount("Height Adjustment", Float) = 0.0
	}
		SubShader{
		Pass{
		Tags{ "RenderType" = "Opaque" }
		LOD 200

		CGPROGRAM
#pragma target 5.0
		//#pragma addshadow
#pragma vertex vert
#pragma geometry GS_Main
#pragma fragment frag
#include "UnityCG.cginc"

		// Vert to geo
		struct v2g
	{
		float4 pos: POSITION;
	};

	// geo to frag
	struct g2f
	{
		float4 pos: POSITION;
	};


	// Vars
	fixed4 _Color;
	float _Amount;


	// Vertex modifier function
	v2g vert(appdata_base v) {

		v2g output = (v2g)0;
		output.pos = mul(_Object2World, v.vertex);
		// Just testing whether all vertices affected
		output.pos.y -= _Amount;

		return output;
	}

	// GS_Main(point v2g p[1], inout TriangleStream<g2f> triStream)
	// GS_Main(line v2g p[2], inout TriangleStream<g2f> triStream)
	// GS_Main(triangle v2g p[3], inout TriangleStream<g2f> triStream)
	[maxvertexcount(4)]
	void GS_Main(point v2g p[1], inout TriangleStream<g2f> triStream)
	{
		float4 v[4];

		float3 asdfUp = float3(0, 1, 0);
		float3 asdfRight = float3(0, 0, 1);
		v[0] = float4(p[0].pos + 0.25 * asdfRight - 0.25 * asdfUp, 1.0f);
		v[1] = float4(p[0].pos + 0.25 * asdfRight + 0.25 * asdfUp, 1.0f);
		v[2] = float4(p[0].pos - 0.25 * asdfRight - 0.25 * asdfUp, 1.0f);
		v[3] = float4(p[0].pos - 0.25 * asdfRight + 0.25 * asdfUp, 1.0f);

		float4x4 vp = mul(UNITY_MATRIX_MVP, _World2Object);
		g2f pIn;
		pIn.pos = mul(vp, v[0]);
		triStream.Append(pIn);

		pIn.pos = mul(vp, v[1]);
		triStream.Append(pIn);

		pIn.pos = mul(vp, v[2]);
		triStream.Append(pIn);

		pIn.pos = mul(vp, v[3]);
		triStream.Append(pIn);
	}

	fixed4 frag(g2f input) : COLOR{
		return float4(1.0, 0.0, 0.0, 1.0);
	}

		ENDCG
	}


	}

		FallBack "Diffuse"
}