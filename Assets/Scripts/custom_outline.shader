Shader "Custom/Outline" {
	Properties{
		_MainTex("MainTex", 2D) = "white" {}
		_Outline("_Outline", Range(0,0.1)) = 0.03
		_OutlineColor("_OutlineColor", Color) = (1, 1, 1, 1)
		_BumpMap("Bumpmap", 2D) = "bump" {}
	}
	SubShader {
		Pass {
			Tags { "RenderType" = "Opaque" }
			Name "OUTLINE"
			Cull Front
			
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			struct v2f {
				float4 pos : SV_POSITION;
			};

			float _Outline;
			float4 _OutlineColor;

			float4 vert(appdata_base v) : SV_POSITION {
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				float3 normal = mul((float3x3) UNITY_MATRIX_MV, v.normal);
				normal.x *= UNITY_MATRIX_P[0][0];
				normal.y *= UNITY_MATRIX_P[1][1];
				o.pos.xy += normal.xy * _Outline;
				return o.pos;
			}

			half4 frag(v2f i) : COLOR {
				return _OutlineColor;
			}
			ENDCG
		}

		CGPROGRAM
		#pragma surface surf Lambert

		sampler2D _MainTex;
		sampler2D _BumpMap;
		struct Input {
			float2 uv_MainTex;
			float2 uv_BumpMap;
		};
		void surf(Input IN, inout SurfaceOutput o) {
			o.Albedo = tex2D(_MainTex, IN.uv_MainTex);
			o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
		}
		ENDCG
	}

	SubShader {
		Tags{ "Queue" = "Transparent" }

		Pass {
			Name "OUTLINE"
			Tags{ "LightMode" = "Always" }
			Cull Front
			ZWrite Off
			ZTest Always
			Offset 15,15
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex vert
			#pragma exclude_renderers gles xbox360 ps3
			ENDCG
			SetTexture[_MainTex]{ combine primary }
		}

		CGPROGRAM
		#pragma surface surf Lambert
		struct Input {
			float2 uv_MainTex;
			float2 uv_BumpMap;
		};
		sampler2D _MainTex;
		sampler2D _BumpMap;
		uniform float3 _Color;
		void surf(Input IN, inout SurfaceOutput o) {
			o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb * _Color;
			o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
		}
		ENDCG
	}

	FallBack "Diffuse"
}