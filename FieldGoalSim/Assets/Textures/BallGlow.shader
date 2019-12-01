Shader "SChiraz/BallGlow"
{
	Properties
	{
		_MainTex("Main Texture", 2D) = "white" {}
		_OutlineColor("Outline Color", Color) = (0,0,0,1)
		_OutlineWidth("Outline width", Range(0, 1)) = .1
	}

	CGINCLUDE
	#include "UnityCG.cginc"

	struct appdata
	{
		float4 vertex : POSITION;
	};

	struct v2f
	{
		float4 pos : POSITION;
	};

	uniform float _OutlineWidth;
	uniform float4 _OutlineColor;
	ENDCG

	SubShader // Redo outline shader
	{
	Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" }

	Pass //Outline
	{
		ZWrite Off
		Cull Back
		CGPROGRAM

		#pragma vertex vert
		#pragma fragment frag

		v2f vert(appdata v)
		{
			v.vertex.xyz += _OutlineWidth * normalize(v.vertex.xyz);

			v2f o;
			o.pos = UnityObjectToClipPos(v.vertex);
			return o;
		}

		half4 frag(v2f i) : COLOR
		{
			return _OutlineColor;
		}
		ENDCG
	}

	Tags{"Queue" = "Geometry"}

	CGPROGRAM
	#pragma surface surf Lambert

	struct Input
	{
		float2 uv_MainTex;
	};

	sampler2D _MainTex;

	void surf(Input IN, inout SurfaceOutput o)
	{
		fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
		o.Albedo = c.rgb;
		o.Alpha = c.a;
	}
	ENDCG

						
	}
	Fallback "Diffuse"
}