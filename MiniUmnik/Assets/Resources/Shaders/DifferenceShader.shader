Shader "Custom/DifferenceShader"
{
	Properties
	{
		_Color("Border color", Color) = (1,1,1,1)
		_AlbedoColor("Albedo color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_BorderWidth("Frame width", Range(0,0.5)) = 0.01
			_Answered("IsAnswered",float) = 0.0
	}
		SubShader
		{
				Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
	LOD 200

	Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			// Physically based Standard lighting model, and enable shadows on all light types
			#pragma surface surf Standard fullforwardshadows 

			// Use shader model 3.0 target, to get nicer looking lighting
			#pragma target 3.0

			sampler2D _MainTex;

			struct Input
			{
				float2 uv_MainTex;
			};

			float _BorderWidth;
			fixed4 _Color;
			fixed4 _AlbedoColor;
			float _Answered;
			// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
			// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
			// #pragma instancing_options assumeuniformscaling
			UNITY_INSTANCING_BUFFER_START(Props)
				// put more per-instance properties here
			UNITY_INSTANCING_BUFFER_END(Props)

			void surf(Input IN, inout SurfaceOutputStandard o)
			{

				// Albedo comes from a texture tinted by color
				fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _AlbedoColor;
				float2 b2 = float2(_BorderWidth,_BorderWidth);
				float4 color = _Color;
				float2 k2 = step(b2, IN.uv_MainTex) * step(b2,1.0 - IN.uv_MainTex);
				float k = k2.x * k2.y;
				o.Albedo = c.rgba;
				o.Albedo = lerp(_Color, o.Albedo.rgb, k);

				o.Alpha = c.a;/* *step(_Answered, 0.5);*/
			}
			ENDCG
		}
			FallBack "Diffuse"
}
