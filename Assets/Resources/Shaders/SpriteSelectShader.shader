Shader "Star_Entitas/SpriteSelectShader"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        [MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
        [HideInInspector] _RendererColor ("RendererColor", Color) = (1,1,1,1)
        [HideInInspector] _Flip ("Flip", Vector) = (1,1,1,1)
        [PerRendererData] _AlphaTex ("External Alpha", 2D) = "white" {}
        [PerRendererData] _EnableExternalAlpha ("Enable External Alpha", Float) = 0

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
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend One OneMinusSrcAlpha

        Pass
        {
        CGPROGRAM
            #pragma vertex SpriteVert
            #pragma fragment SpriteFrag
            #pragma target 2.0
            #pragma multi_compile_instancing
            #pragma multi_compile _ PIXELSNAP_ON
            #pragma multi_compile _ ETC1_EXTERNAL_ALPHA

			#ifndef UNITY_SPRITES_INCLUDED
			#define UNITY_SPRITES_INCLUDED

			#include "UnityCG.cginc"

			#ifdef UNITY_INSTANCING_ENABLED

				UNITY_INSTANCING_BUFFER_START(PerDrawSprite)
					// SpriteRenderer.Color while Non-Batched/Instanced.
					UNITY_DEFINE_INSTANCED_PROP(fixed4, unity_SpriteRendererColorArray)
					// this could be smaller but that's how bit each entry is regardless of type
					UNITY_DEFINE_INSTANCED_PROP(fixed2, unity_SpriteFlipArray)
				UNITY_INSTANCING_BUFFER_END(PerDrawSprite)

				#define _RendererColor  UNITY_ACCESS_INSTANCED_PROP(PerDrawSprite, unity_SpriteRendererColorArray)
				#define _Flip           UNITY_ACCESS_INSTANCED_PROP(PerDrawSprite, unity_SpriteFlipArray)

			#endif // instancing

			CBUFFER_START(UnityPerDrawSprite)
			#ifndef UNITY_INSTANCING_ENABLED
				fixed4 _RendererColor;
				fixed2 _Flip;
			#endif
				float _EnableExternalAlpha;
			CBUFFER_END

			// Material Color.
			fixed4 _Color;

			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
				UNITY_VERTEX_OUTPUT_STEREO
			};

			v2f SpriteVert(appdata_t IN)
			{
				v2f OUT;

				UNITY_SETUP_INSTANCE_ID (IN);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);

			#ifdef UNITY_INSTANCING_ENABLED
				IN.vertex.xy *= _Flip;
			#endif

				OUT.vertex = UnityObjectToClipPos(IN.vertex);
				OUT.texcoord = IN.texcoord;
				OUT.color = IN.color * _Color * _RendererColor;

				#ifdef PIXELSNAP_ON
				OUT.vertex = UnityPixelSnap (OUT.vertex);
				#endif

				return OUT;
			}

			sampler2D _MainTex;
			sampler2D _AlphaTex;

			float _EdgeWidth;
			fixed4 _EdgeColor;
			float _ShowUpEdge;
			float _ShowLeftEdge;
			float _ShowDownEdge;
			float _ShowRightEdge;
			float _Flash;

			fixed4 SampleSpriteTexture (float2 uv)
			{
				fixed4 color = tex2D (_MainTex, uv);

			#if ETC1_EXTERNAL_ALPHA
				fixed4 alpha = tex2D (_AlphaTex, uv);
				color.a = lerp (color.a, alpha.r, _EnableExternalAlpha);
			#endif

				return color;
			}

			fixed4 SpriteFrag(v2f IN) : SV_Target
			{
				fixed4 c = SampleSpriteTexture (IN.texcoord) * IN.color;

				fixed x = IN.texcoord.x;
				fixed y = IN.texcoord.y;

				float l = lerp(1,step(_EdgeWidth,x),_ShowLeftEdge);
				float r = lerp(1,step(x,1 - _EdgeWidth),_ShowRightEdge);
				float u = lerp(1,step(y,1 - _EdgeWidth),_ShowUpEdge);
				float d = lerp(1,step( _EdgeWidth,y),_ShowDownEdge);

				float w = l*r*u*d;

				c.rgb = lerp(_EdgeColor.rgb * lerp(1,( abs(cos(_Time.w))*0.5+0.7),_Flash) ,c.rgb,w);
				c.a = lerp(1,c.a,w);

				c.rgb *= c.a;
				return c;
			}

			#endif // UNITY_SPRITES_INCLUDED

        ENDCG
        }
    }
}
