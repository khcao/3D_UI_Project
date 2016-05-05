Shader "DX11/Sample 3D Texture" {
	Properties{
		_Volume("Texture", 3D) = "" {}
	}
		SubShader{
		Pass{

		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#pragma exclude_renderers flash gles

#include "UnityCG.cginc"

	struct vs_input {
		float4 vertex : POSITION;
	};

	struct ps_input {
		float4 pos : SV_POSITION;
		float3 uv : TEXCOORD0;
	};


	ps_input vert(vs_input v)
	{
		ps_input o;
		o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
		o.uv = v.vertex.xyz * 0.5 + 0.5;
		return o;
	}

	sampler3D _Volume;

	[maxvertexcount(3)]
	void geom(triangle v2f input[3], inout TriangleStream<v2f> OutputStream)
	{
		v2f test = (v2f)0;
		float3 normal = normalize(cross(input[1].worldPosition.xyz - input[0].worldPosition.xyz, input[2].worldPosition.xyz - input[0].worldPosition.xyz));
		for (int i = 0; i < 3; i++)
		{
			test.normal = normal;
			test.vertex = input[i].vertex;
			test.uv = input[i].uv;
			OutputStream.Append(test);
		}
	}

	float4 frag(ps_input i) : COLOR
	{
		return float4(1,0,0,0.1); // tex3D(_Volume, i.uv);
	}



		ENDCG

	}
	}

		Fallback "VertexLit"
}