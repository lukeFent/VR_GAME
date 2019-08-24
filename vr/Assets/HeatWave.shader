Shader "Unlit/HeatWave"
{
    Properties
    {
        _NoiseTex ("Noise Texture", 2D) = "white" {}
		_Scale("Scale", Range(1,200)) = 1
			_NoiseScale("Noise Scale", Range(0,10)) = 1
			_StrengthFilter("Strength Filter",2D) = "white" {}

    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
	/*	ZTest Always*/
		
        LOD 100

		GrabPass
	{
		"_BackgroundTexture"
	}

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
				float4 grabPos : TEXCOORD1;
				float4 strengthUV : TEXCOORD2;
                float4 vertex : SV_POSITION;
            };

            sampler2D _NoiseTex;
            float4 _NoiseTex_ST;
			float _Scale;
			float _NoiseScale;
			sampler2D _StrengthFilter;
			float4 _StrengthFilter_ST;
			sampler2D _BackgroundTexture;


            v2f vert (appdata v)
            {
                v2f o;
				float4 originInViewSpace = float4(0,0,0,1);
				originInViewSpace.xyz = UnityObjectToViewPos(float3(0, 0, 0));
				//float4 originInViewSpace = mul(UNITY_MATRIX_MV, float4(0,0,0,1));
				float4 vertexInViewSpace = originInViewSpace + v.vertex * _Scale;
				o.vertex = mul(UNITY_MATRIX_P, vertexInViewSpace);
				o.uv = TRANSFORM_TEX(v.uv, _NoiseTex);// * 1 * _Time;
				o.grabPos = ComputeGrabScreenPos(o.vertex);
				return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
				float strength = tex2D(_StrengthFilter, i.uv).r;
				float4 noise = tex2D(_NoiseTex, i.uv).r;
				//i.grabPos.x += noise * _NoiseScale * strength * sin(_Time.x * 10);
				//i.grabPos.y += noise * _NoiseScale * strength * sin(_Time.y * 10);
				half4 bgcolor = tex2Dproj(_BackgroundTexture, i.grabPos);
				//fixed4 col = tex2D(_MainTex, i.uv);
                // apply fog
                return bgcolor;
            }
            ENDCG
        }
    }
}
