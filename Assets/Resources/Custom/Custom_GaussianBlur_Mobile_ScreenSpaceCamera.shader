Shader "Custom/GaussianBlur_Mobile_ScreenSpaceCamera"
{
  Properties
  {
    [PerRendererData] _MainTex ("_MainTex", 2D) = "white" {}
    _Lightness ("_Lightness", Range(0, 2)) = 1
    _Saturation ("_Saturation", Range(-10, 10)) = 1
    _TintColor ("_TintColor", Color) = (1,1,1,0)
  }
  SubShader
  {
    Tags
    { 
      "DisableBatching" = "true"
      "PreviewType" = "Plane"
      "QUEUE" = "Transparent"
    }
    Pass // ind: 1, name: 
    {
      Tags
      { 
        "DisableBatching" = "true"
        "PreviewType" = "Plane"
        "QUEUE" = "Transparent"
      }
      ZWrite Off
      Cull Off
      Blend SrcAlpha OneMinusSrcAlpha
      GpuProgramID 60280
      // m_ProgramMask = 6
      //  !!!!GLES
      CGPROGRAM
      //#pragma target 4.0
      
      #pragma vertex vert
      #pragma fragment frag
      
      #include "UnityCG.cginc"
      
      #define conv_mxt4x4_0(mat4x4) float4(mat4x4[0].x,mat4x4[1].x,mat4x4[2].x,mat4x4[3].x)
      #define conv_mxt4x4_1(mat4x4) float4(mat4x4[0].y,mat4x4[1].y,mat4x4[2].y,mat4x4[3].y)
      #define conv_mxt4x4_2(mat4x4) float4(mat4x4[0].z,mat4x4[1].z,mat4x4[2].z,mat4x4[3].z)
      #define conv_mxt4x4_3(mat4x4) float4(mat4x4[0].w,mat4x4[1].w,mat4x4[2].w,mat4x4[3].w)
      #define conv_mxt3x3_0(mat4x4) float3(mat4x4[0].x,mat4x4[1].x,mat4x4[2].x)
      #define conv_mxt3x3_1(mat4x4) float3(mat4x4[0].y,mat4x4[1].y,mat4x4[2].y)
      #define conv_mxt3x3_2(mat4x4) float3(mat4x4[0].z,mat4x4[1].z,mat4x4[2].z)
      
      #define CODE_BLOCK_VERTEX
      uniform float _Lightness;
      uniform float _Saturation;
      uniform float4 _TintColor;
      uniform float4 _MobileBlur_ST;
      uniform sampler2D _MainTex;
      uniform sampler2D _MobileBlur;
      struct IN_Data_Vert
      {
          float4 in_POSITION0 :POSITION0;
          float2 in_TEXCOORD0 :TEXCOORD0;
          float4 in_COLOR0 :COLOR0;
      };
      
      struct OUT_Data_Vert
      {
          float2 vs_TEXCOORD0 :TEXCOORD0;
          float2 vs_TEXCOORD1 :TEXCOORD1;
          float4 vs_COLOR0 :COLOR0;
          float2 vs_TEXCOORD2 :TEXCOORD2;
          float4 gl_Position :SV_POSITION;
      };
      
      struct IN_Data_Frag
      {
          float2 vs_TEXCOORD0 :TEXCOORD0;
          float2 vs_TEXCOORD1 :TEXCOORD1;
          float4 vs_COLOR0 :COLOR0;
      };
      
      struct OUT_Data_frag
      {
          float4 SV_Target0 :SV_Target0;
      };
      
      float4 u_xlat0;
      float4 u_xlat1;
      float u_xlat2;
      OUT_Data_Vert vert(IN_Data_Vert in_v)
      {
          OUT_Data_Vert out_v;
          u_xlat0 = (in_v.in_POSITION0.yyyy * conv_mxt4x4_1(unity_ObjectToWorld));
          u_xlat0 = ((conv_mxt4x4_0(unity_ObjectToWorld) * in_v.in_POSITION0.xxxx) + u_xlat0);
          u_xlat0 = ((conv_mxt4x4_2(unity_ObjectToWorld) * in_v.in_POSITION0.zzzz) + u_xlat0);
          u_xlat0 = (u_xlat0 + conv_mxt4x4_3(unity_ObjectToWorld));
          u_xlat1 = (u_xlat0.yyyy * conv_mxt4x4_1(unity_MatrixVP));
          u_xlat1 = ((conv_mxt4x4_0(unity_MatrixVP) * u_xlat0.xxxx) + u_xlat1);
          u_xlat1 = ((conv_mxt4x4_2(unity_MatrixVP) * u_xlat0.zzzz) + u_xlat1);
          u_xlat0 = ((conv_mxt4x4_3(unity_MatrixVP) * u_xlat0.wwww) + u_xlat1);
          out_v.gl_Position = u_xlat0;
          u_xlat1.xy = (u_xlat0.xy / u_xlat0.ww);
          u_xlat1.xy = (u_xlat1.xy + float2(1, 1));
          out_v.vs_TEXCOORD1.xy = (u_xlat1.xy * float2(0.5, 0.5));
          out_v.vs_TEXCOORD0.xy = in_v.in_TEXCOORD0.xy;
          out_v.vs_COLOR0 = in_v.in_COLOR0;
          u_xlat2 = (u_xlat0.y * _ProjectionParams.x);
          u_xlat0.xz = (u_xlat0.xw * float2(0.5, 0.5));
          u_xlat0.w = (u_xlat2 * 0.5);
          out_v.vs_TEXCOORD2.xy = (u_xlat0.zz + u_xlat0.xw);
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      float3 u_xlat16_0;
      float4 u_xlat10_0;
      float u_xlat16_2;
      OUT_Data_frag frag(IN_Data_Frag in_f)
      {
          OUT_Data_frag out_f;
          u_xlat0.xy = ((in_f.vs_TEXCOORD1.xy * _MobileBlur_ST.xy) + _MobileBlur_ST.zw);
          u_xlat10_0 = tex2D(_MobileBlur, u_xlat0.xy);
          u_xlat16_0.xyz = (u_xlat10_0.xyz * in_f.vs_COLOR0.xyz);
          u_xlat0.xyz = (u_xlat16_0.xyz * float3(_Lightness, _Lightness, _Lightness));
          u_xlat1.xyz = (u_xlat0.xyz * _TintColor.xyz);
          u_xlat1.x = dot(u_xlat1.xyz, float3(0.298999995, 0.587000012, 0.114));
          u_xlat0.xyz = ((u_xlat0.xyz * _TintColor.xyz) + (-u_xlat1.xxx));
          float _tmp_dvx_34 = float3(_Saturation, _Saturation, _Saturation);
          out_f.SV_Target0.xyz = ((float3(_tmp_dvx_34, _tmp_dvx_34, _tmp_dvx_34) * u_xlat0.xyz) + u_xlat1.xxx);
          u_xlat10_0.x = tex2D(_MainTex, in_f.vs_TEXCOORD0.xy).w;
          u_xlat16_2 = (u_xlat10_0.x * u_xlat10_0.w);
          u_xlat16_0.x = (u_xlat10_0.x * in_f.vs_COLOR0.w);
          out_f.SV_Target0.w = (u_xlat16_2 * u_xlat16_0.x);
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  FallBack "Sprites/Default"
}
