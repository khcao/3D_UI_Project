Shader "Custom/Shader Texture 3D" {
	Properties
	{
		_Volume("Color (RGB) Alpha (A)", 3D) = "" {}
		_Position("Position", Vector) = (0, 0, 0, 0)
		//_Scale("Scale", Vector) = (1, 1, 1, 1)
		_Depth("Depth", Float) = 1
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
			vector _Position;
			vector _Scale;
			float _Depth;

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
				o.pos = (mul(UNITY_MATRIX_MVP, v.vertex) + _Position.xyzw);// *_Scale;
				o.uv = v.vertex.xyz;

				return o;
			}

			float4 frag(ps_input i) : Color
			{
				float4 col = tex3D(_Volume, i.uv); //float4(1,0,0,0.1); 
				col.a /= _Depth / 6;
				return col;
			}



			ENDCG

		}
	}
	Fallback "VertexLit"
}