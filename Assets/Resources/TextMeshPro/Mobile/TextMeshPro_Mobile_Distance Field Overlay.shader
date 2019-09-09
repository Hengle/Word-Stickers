// Upgrade NOTE: replaced 'glstate_matrix_projection' with 'UNITY_MATRIX_P'

Shader "TextMeshPro/Mobile/Distance Field Overlay"
{
  Properties
  {
    _FaceColor ("Face Color", Color) = (1,1,1,1)
    _FaceDilate ("Face Dilate", Range(-1, 1)) = 0
    _OutlineColor ("Outline Color", Color) = (0,0,0,1)
    _OutlineWidth ("Outline Thickness", Range(0, 1)) = 0
    _OutlineSoftness ("Outline Softness", Range(0, 1)) = 0
    _UnderlayColor ("Border Color", Color) = (0,0,0,0.5)
    _UnderlayOffsetX ("Border OffsetX", Range(-1, 1)) = 0
    _UnderlayOffsetY ("Border OffsetY", Range(-1, 1)) = 0
    _UnderlayDilate ("Border Dilate", Range(-1, 1)) = 0
    _UnderlaySoftness ("Border Softness", Range(0, 1)) = 0
    _WeightNormal ("Weight Normal", float) = 0
    _WeightBold ("Weight Bold", float) = 0.5
    _ShaderFlags ("Flags", float) = 0
    _ScaleRatioA ("Scale RatioA", float) = 1
    _ScaleRatioB ("Scale RatioB", float) = 1
    _ScaleRatioC ("Scale RatioC", float) = 1
    _MainTex ("Font Atlas", 2D) = "white" {}
    _TextureWidth ("Texture Width", float) = 512
    _TextureHeight ("Texture Height", float) = 512
    _GradientScale ("Gradient Scale", float) = 5
    _ScaleX ("Scale X", float) = 1
    _ScaleY ("Scale Y", float) = 1
    _PerspectiveFilter ("Perspective Correction", Range(0, 1)) = 0.875
    _VertexOffsetX ("Vertex OffsetX", float) = 0
    _VertexOffsetY ("Vertex OffsetY", float) = 0
    _ClipRect ("Clip Rect", Vector) = (-32767,-32767,32767,32767)
    _MaskSoftnessX ("Mask SoftnessX", float) = 0
    _MaskSoftnessY ("Mask SoftnessY", float) = 0
    _StencilComp ("Stencil Comparison", float) = 8
    _Stencil ("Stencil ID", float) = 0
    _StencilOp ("Stencil Operation", float) = 0
    _StencilWriteMask ("Stencil Write Mask", float) = 255
    _StencilReadMask ("Stencil Read Mask", float) = 255
    _ColorMask ("Color Mask", float) = 15
  }
  SubShader
  {
    Tags
    { 
      "IGNOREPROJECTOR" = "true"
      "QUEUE" = "Overlay"
      "RenderType" = "Transparent"
    }
    Pass // ind: 1, name: 
    {
      Tags
      { 
        "IGNOREPROJECTOR" = "true"
        "QUEUE" = "Overlay"
        "RenderType" = "Transparent"
      }
      ZTest Always
      ZWrite Off
      Cull Off
      Stencil
      { 
        Ref 0
        ReadMask 0
        WriteMask 0
        Comp Disabled
        Pass Keep
        Fail Keep
        ZFail Keep
        CompFront Disabled
        PassFront Keep
        FailFront Keep
        ZFailFront Keep
        CompBack Disabled
        PassBack Keep
        FailBack Keep
        ZFailBack Keep
      } 
      Fog
      { 
        Mode  Off
      } 
      Blend One OneMinusSrcAlpha
      ColorMask 0
      GpuProgramID 62710
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
      uniform float4 UNITY_MATRIX_P[4];
      uniform float4 _FaceColor;
      uniform float _FaceDilate;
      uniform float _OutlineSoftness;
      uniform float4 _OutlineColor;
      uniform float _OutlineWidth;
      uniform float _WeightNormal;
      uniform float _WeightBold;
      uniform float _ScaleRatioA;
      uniform float _VertexOffsetX;
      uniform float _VertexOffsetY;
      uniform float4 _ClipRect;
      uniform float _MaskSoftnessX;
      uniform float _MaskSoftnessY;
      uniform float _GradientScale;
      uniform float _ScaleX;
      uniform float _ScaleY;
      uniform float _PerspectiveFilter;
      uniform sampler2D _MainTex;
      struct IN_Data_Vert
      {
          float4 in_POSITION0 :POSITION0;
          float3 in_NORMAL0 :NORMAL0;
          float4 in_COLOR0 :COLOR0;
          float2 in_TEXCOORD0 :TEXCOORD0;
          float2 in_TEXCOORD1 :TEXCOORD1;
      };
      
      struct OUT_Data_Vert
      {
          float4 vs_COLOR0 :COLOR0;
          float4 vs_COLOR1 :COLOR1;
          float4 vs_TEXCOORD0 :TEXCOORD0;
          float4 vs_TEXCOORD1 :TEXCOORD1;
          float4 vs_TEXCOORD2 :TEXCOORD2;
          float4 gl_Position :SV_POSITION;
      };
      
      struct IN_Data_Frag
      {
          float4 vs_COLOR0 :COLOR0;
          float4 vs_TEXCOORD0 :TEXCOORD0;
          float4 vs_TEXCOORD1 :TEXCOORD1;
      };
      
      struct OUT_Data_frag
      {
          float4 SV_Target0 :SV_Target0;
      };
      
      float2 u_xlat0;
      int u_xlatb0;
      float4 u_xlat1;
      float4 u_xlat2;
      float4 u_xlat3;
      float4 u_xlat16_3;
      float4 u_xlat4;
      float u_xlat5;
      float u_xlat7;
      float u_xlat10;
      float u_xlat15;
      int u_xlatb15;
      OUT_Data_Vert vert(IN_Data_Vert in_v)
      {
          OUT_Data_Vert out_v;
          u_xlat0.xy = (in_v.in_POSITION0.xy + float2(_VertexOffsetX, _VertexOffsetY));
          u_xlat1 = (u_xlat0.yyyy * conv_mxt4x4_1(unity_ObjectToWorld));
          u_xlat1 = ((conv_mxt4x4_0(unity_ObjectToWorld) * u_xlat0.xxxx) + u_xlat1);
          u_xlat1 = ((conv_mxt4x4_2(unity_ObjectToWorld) * in_v.in_POSITION0.zzzz) + u_xlat1);
          u_xlat2 = (u_xlat1 + conv_mxt4x4_3(unity_ObjectToWorld));
          u_xlat1.xyz = ((conv_mxt4x4_3(unity_ObjectToWorld).xyz * in_v.in_POSITION0.www) + u_xlat1.xyz);
          u_xlat1.xyz = ((-u_xlat1.xyz) + _WorldSpaceCameraPos.xyz);
          u_xlat3 = (u_xlat2.yyyy * conv_mxt4x4_1(unity_MatrixVP));
          u_xlat3 = ((conv_mxt4x4_0(unity_MatrixVP) * u_xlat2.xxxx) + u_xlat3);
          u_xlat3 = ((conv_mxt4x4_2(unity_MatrixVP) * u_xlat2.zzzz) + u_xlat3);
          u_xlat2 = ((conv_mxt4x4_3(unity_MatrixVP) * u_xlat2.wwww) + u_xlat3);
          out_v.gl_Position = u_xlat2;
          u_xlat16_3 = (in_v.in_COLOR0 * _FaceColor);
          u_xlat16_3.xyz = (u_xlat16_3.www * u_xlat16_3.xyz);
          out_v.vs_COLOR0 = u_xlat16_3;
          u_xlat10 = dot(u_xlat1.xyz, u_xlat1.xyz);
          u_xlat10 = rsqrt(u_xlat10);
          u_xlat1.xyz = (float3(u_xlat10, u_xlat10, u_xlat10) * u_xlat1.xyz);
          u_xlat2.x = dot(in_v.in_NORMAL0.xyz, conv_mxt4x4_0(unity_WorldToObject).xyz);
          u_xlat2.y = dot(in_v.in_NORMAL0.xyz, conv_mxt4x4_1(unity_WorldToObject).xyz);
          u_xlat2.z = dot(in_v.in_NORMAL0.xyz, conv_mxt4x4_2(unity_WorldToObject).xyz);
          u_xlat10 = dot(u_xlat2.xyz, u_xlat2.xyz);
          u_xlat10 = rsqrt(u_xlat10);
          u_xlat2.xyz = (float3(u_xlat10, u_xlat10, u_xlat10) * u_xlat2.xyz);
          u_xlat10 = dot(u_xlat2.xyz, u_xlat1.xyz);
          u_xlat1.xy = (_ScreenParams.yy * conv_mxt4x4_1(UNITY_MATRIX_P).xy);
          u_xlat1.xy = ((conv_mxt4x4_0(UNITY_MATRIX_P).xy * _ScreenParams.xx) + u_xlat1.xy);
          u_xlat1.xy = (abs(u_xlat1.xy) * float2(_ScaleX, _ScaleY));
          u_xlat1.xy = (u_xlat2.ww / u_xlat1.xy);
          u_xlat15 = dot(u_xlat1.xy, u_xlat1.xy);
          u_xlat1.xy = ((float2(_MaskSoftnessX, _MaskSoftnessY) * float2(0.25, 0.25)) + u_xlat1.xy);
          u_xlat1.zw = (float2(0.25, 0.25) / u_xlat1.xy);
          u_xlat15 = rsqrt(u_xlat15);
          u_xlat2.x = (abs(in_v.in_TEXCOORD1.y) * _GradientScale);
          u_xlat15 = (u_xlat15 * u_xlat2.x);
          u_xlat2.x = (u_xlat15 * 1.5);
          u_xlat7 = ((-_PerspectiveFilter) + 1);
          u_xlat7 = (u_xlat7 * abs(u_xlat2.x));
          u_xlat15 = ((u_xlat15 * 1.5) + (-u_xlat7));
          u_xlat10 = ((abs(u_xlat10) * u_xlat15) + u_xlat7);
          if((conv_mxt4x4_3(UNITY_MATRIX_P).w==0))
          {
              u_xlatb15 = 1;
          }
          else u_xlatb15 = 0;
          u_xlat10 = u_xlatb15;
          u_xlat15 = (_OutlineSoftness * _ScaleRatioA);
          u_xlat15 = ((u_xlat15 * u_xlat10) + 1);
          u_xlat2.x = (u_xlat10 / u_xlat15);
          u_xlat10 = (_OutlineWidth * _ScaleRatioA);
          u_xlat10 = (u_xlat2.x * u_xlat10);
          u_xlat15 = min(u_xlat10, 1);
          u_xlat15 = sqrt(u_xlat15);
          u_xlat4.x = (in_v.in_COLOR0.w * _OutlineColor.w);
          u_xlat4.xyz = ((_OutlineColor.xyz * u_xlat4.xxx) + (-u_xlat16_3.xyz));
          u_xlat4.w = ((_OutlineColor.w * in_v.in_COLOR0.w) + (-u_xlat16_3.w));
          u_xlat3 = ((float4(u_xlat15, u_xlat15, u_xlat15, u_xlat15) * u_xlat4) + u_xlat16_3);
          out_v.vs_COLOR1 = u_xlat3;
      }
      
      #define CODE_BLOCK_FRAGMENT
      float u_xlat10_0;
      float u_xlat16_1;
      OUT_Data_frag frag(IN_Data_Frag in_f)
      {
          OUT_Data_frag out_f;
          u_xlat10_0 = tex2D(_MainTex, in_f.vs_TEXCOORD0.xy).w;
          u_xlat16_1 = ((u_xlat10_0 * in_f.vs_TEXCOORD1.x) + (-in_f.vs_TEXCOORD1.w));
          u_xlat16_1 = clamp(u_xlat16_1, 0, 1);
          out_f.SV_Target0 = (float4(u_xlat16_1, u_xlat16_1, u_xlat16_1, u_xlat16_1) * in_f.vs_COLOR0);
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  FallBack ""
}
