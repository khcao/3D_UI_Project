Shader "Custom/Shader Texture 3D" {
	Properties
	{
		_Volume("Color (RGB) Alpha (A)", 3D) = "" {}
	}
	SubShader{
		Tags
		{
			"Queue" = "Transparent"
			"RenderType" = "Transparent"
		}
		Blend SrcAlpha OneMinusSrcAlpha
		Cull Off
		ZWrite Off
		Pass{

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag alpha
			#pragma enable_d3d11_debug_symbols

			#include "UnityCG.cginc"

			sampler3D _Volume;

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
				o.uv = v.vertex.xyz; //* 0.5 + 0.5;

				return o;
			}

			float4 frag(ps_input i) : Color
			{
				return tex3D(_Volume, i.uv); //float4(1,0,0,0.1); 
			}



			ENDCG

		}
	}
	Fallback "VertexLit"
}