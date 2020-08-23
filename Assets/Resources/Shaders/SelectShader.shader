// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Star_Entitas/SelectShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_EdgeWidth("Width",Range(0,0.5)) = 0.01
		_EdgeColor ("Color", Color) = (1, 0, 0, 1)

		//只能填0或1，0表示关闭，1表示开启
		_ShowUpEdge("UpEdge",Float) =  0
		_ShowLeftEdge("LeftEdge",Float) =  0
		_ShowDownEdge("DownEdge",Float) =  0
		_ShowRightEdge("RightEdge",Float) =  0

		_Flash("Flash",Float) = 0
	}

	SubShader
	{
		Tags
		{
			"Queue"="Transparent" 
			"RenderType"="Transparent" 
			"IgnoreProjector"="True"
		}

		Pass
		{
			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			sampler2D _MainTex;
			float _EdgeWidth;
			fixed4 _EdgeColor;
			float _ShowUpEdge;
			float _ShowLeftEdge;
			float _ShowDownEdge;
			float _ShowRightEdge;
			float _Flash;
			

			struct appdata
			{
				float4 vertex : POSITION;
				fixed2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex :SV_POSITION;
				fixed2 uv : TEXCOORD0;
			};

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex,i.uv);

				fixed x = i.uv.x;
				fixed y = i.uv.y;

				float l = lerp(1,step(_EdgeWidth,x),_ShowLeftEdge);
				float r = lerp(1,step(x,1 - _EdgeWidth),_ShowRightEdge);
				float u = lerp(1,step(y,1 - _EdgeWidth),_ShowUpEdge);
				float d = lerp(1,step( _EdgeWidth,y),_ShowDownEdge);

				float w = l*r*u*d;

				col.rgba = lerp(_EdgeColor.rgba * lerp(1,( abs(cos(_Time.w))*0.5+0.7),_Flash) ,col.rgba,w);
				return col;
			}


			ENDCG
		}
	}
}
