Shader "Custom/holographic"
{
    Properties
    {
       _RimColor("Rim Color", Color) = (0, 0.5, 0.5, 0)
       _RimPower("Rim Power", Range(0.5, 8.0)) = 3.0
       _Tex("Texture", 2D) = "White"{}
    }
        SubShader
    {
        Tags { "Queue" = "Transparent" }

        Pass
        {
        ZWrite On
        ColorMask 0
        }


        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Lambert alpha:fade

        struct Input
        {
           float3 viewDir;
           float2 uv_myTex;
        };

        float4 _RimColor;
        float _RimPower;
        sampler2D _Tex;
     

        void surf (Input IN, inout SurfaceOutput o)
        {
            half rim = 1.0 - saturate(dot(normalize(IN.viewDir), o.Normal));
            o.Emission = _RimColor.rgb * pow(rim, _RimPower  ) * 10;
            o.Alpha = pow(rim, _RimPower);
            o.Albedo = tex2D(_Tex, IN.uv_myTex).rgb;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
