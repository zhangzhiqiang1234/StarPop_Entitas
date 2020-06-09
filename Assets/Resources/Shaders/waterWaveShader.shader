// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Shaders/waterWaveEffect"
{
	Properties
	{
		_MainTex("MainTexture",2D) = "white" {}
	}

	SubShader
	{
		Tags {"Queue" = "Transparent" "RenderType" = "Transparent"}
		ZWrite Off
		ZTest Always
		Cull Off
		Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "unityCG.cginc"

			sampler2D _MainTex;
			float4 _MainTex_ST;
			 
			uniform float _distanceFactor;
			uniform float _totalFactor;
			uniform float _timeFactor;

			uniform float _curWaveDis;
			uniform float _waveWidth;

			uniform float _rectWave;

			uniform fixed4 _startPos;

			struct a2v
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
			};

			v2f vert(a2v v)
			{
				v2f o;
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.vertex = UnityObjectToClipPos(v.vertex);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				float2 dv = _startPos.xy - i.uv;
				//_ScreenParams.x 屏幕的像素宽度，_ScreenParams.x 屏幕的像素高度
				//确保波纹是个圆形
				dv = dv * float2(_ScreenParams.x / _ScreenParams.y, 1);
				//uv点到波浪中心点的距离
				float dis = sqrt(dv.x * dv.x + dv.y * dv.y);
				//计算一个波纹sin因子
				float sinF = sin(dis * _distanceFactor + _Time.y * _timeFactor)* _totalFactor *0.01;

				float2 dv1 = normalize(dv);

				float discardFactor = clamp(_waveWidth - abs(_curWaveDis - dis), 0, 1);

				//计算uv偏移值
				float2 offsetuv = dv1 * sinF * discardFactor;
				//计算传播范围
				float rectFactor = step(dis, _rectWave);

				float2 uv = offsetuv* rectFactor + i.uv;

				fixed4 col = tex2D(_MainTex,uv);
				return col;
			}

			ENDCG
		}
	}
}