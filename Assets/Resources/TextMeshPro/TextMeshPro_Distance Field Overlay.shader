// Upgrade NOTE: replaced 'glstate_matrix_projection' with 'UNITY_MATRIX_P'

Shader "TextMeshPro/Distance Field Overlay"
{
  Properties
  {
    _FaceTex ("Face Texture", 2D) = "white" {}
    _FaceUVSpeedX ("Face UV Speed X", Range(-5, 5)) = 0
    _FaceUVSpeedY ("Face UV Speed Y", Range(-5, 5)) = 0
    _FaceColor ("Face Color", Color) = (1,1,1,1)
    _FaceDilate ("Face Dilate", Range(-1, 1)) = 0
    _OutlineColor ("Outline Color", Color) = (0,0,0,1)
    _OutlineTex ("Outline Texture", 2D) = "white" {}
    _OutlineUVSpeedX ("Outline UV Speed X", Range(-5, 5)) = 0
    _OutlineUVSpeedY ("Outline UV Speed Y", Range(-5, 5)) = 0
    _OutlineWidth ("Outline Thickness", Range(0, 1)) = 0
    _OutlineSoftness ("Outline Softness", Range(0, 1)) = 0
    _Bevel ("Bevel", Range(0, 1)) = 0.5
    _BevelOffset ("Bevel Offset", Range(-0.5, 0.5)) = 0
    _BevelWidth ("Bevel Width", Range(-0.5, 0.5)) = 0
    _BevelClamp ("Bevel Clamp", Range(0, 1)) = 0
    _BevelRoundness ("Bevel Roundness", Range(0, 1)) = 0
    _LightAngle ("Light Angle", Range(0, 6.283185)) = 3.1416
    _SpecularColor ("Specular", Color) = (1,1,1,1)
    _SpecularPower ("Specular", Range(0, 4)) = 2
    _Reflectivity ("Reflectivity", Range(5, 15)) = 10
    _Diffuse ("Diffuse", Range(0, 1)) = 0.5
    _Ambient ("Ambient", Range(1, 0)) = 0.5
    _BumpMap ("Normal map", 2D) = "bump" {}
    _BumpOutline ("Bump Outline", Range(0, 1)) = 0
    _BumpFace ("Bump Face", Range(0, 1)) = 0
    _ReflectFaceColor ("Reflection Color", Color) = (0,0,0,1)
    _ReflectOutlineColor ("Reflection Color", Color) = (0,0,0,1)
    _Cube ("Reflection Cubemap", Cube) = "black" {}
    _EnvMatrixRotation ("Texture Rotation", Vector) = (0,0,0,0)
    _UnderlayColor ("Border Color", Color) = (0,0,0,0.5)
    _UnderlayOffsetX ("Border OffsetX", Range(-1, 1)) = 0
    _UnderlayOffsetY ("Border OffsetY", Range(-1, 1)) = 0
    _UnderlayDilate ("Border Dilate", Range(-1, 1)) = 0
    _UnderlaySoftness ("Border Softness", Range(0, 1)) = 0
    _GlowColor ("Color", Color) = (0,1,0,0.5)
    _GlowOffset ("Offset", Range(-1, 1)) = 0
    _GlowInner ("Inner", Range(0, 1)) = 0.05
    _GlowOuter ("Outer", Range(0, 1)) = 0.05
    _GlowPower ("Falloff", Range(1, 0)) = 0.75
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
    _MaskCoord ("Mask Coordinates", Vector) = (0,0,32767,32767)
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
      GpuProgramID 13897
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
      uniform float _FaceDilate;
      uniform float _OutlineSoftness;
      uniform float _OutlineWidth;
      uniform float4 _EnvMatrix[4];
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
      uniform float4 _FaceTex_ST;
      uniform float4 _OutlineTex_ST;
      uniform float _FaceUVSpeedX;
      uniform float _FaceUVSpeedY;
      uniform float4 _FaceColor;
      uniform float _OutlineUVSpeedX;
      uniform float _OutlineUVSpeedY;
      uniform float4 _OutlineColor;
      uniform sampler2D _MainTex;
      uniform sampler2D _FaceTex;
      uniform sampler2D _OutlineTex;
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
          float2 vs_TEXCOORD0 :TEXCOORD0;
          float4 vs_TEXCOORD1 :TEXCOORD1;
          float4 vs_TEXCOORD2 :TEXCOORD2;
          float3 vs_TEXCOORD3 :TEXCOORD3;
          float4 vs_TEXCOORD5 :TEXCOORD5;
          float4 gl_Position :SV_POSITION;
      };
      
      struct IN_Data_Frag
      {
          float4 vs_COLOR0 :COLOR0;
          float2 vs_TEXCOORD0 :TEXCOORD0;
          float4 vs_TEXCOORD1 :TEXCOORD1;
          float4 vs_TEXCOORD5 :TEXCOORD5;
      };
      
      struct OUT_Data_frag
      {
          float4 SV_Target0 :SV_Target0;
      };
      
      float3 u_xlat0;
      float4 u_xlat1;
      float4 u_xlat2;
      float4 u_xlat3;
      float3 u_xlat6;
      float2 u_xlat8;
      int u_xlatb8;
      float u_xlat12;
      int u_xlatb12;
      float u_xlat13;
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
          out_v.vs_COLOR0 = in_v.in_COLOR0;
          out_v.vs_TEXCOORD0.xy = in_v.in_TEXCOORD0.xy;
          u_xlat2.x = dot(in_v.in_NORMAL0.xyz, conv_mxt4x4_0(unity_WorldToObject).xyz);
          u_xlat2.y = dot(in_v.in_NORMAL0.xyz, conv_mxt4x4_1(unity_WorldToObject).xyz);
          u_xlat2.z = dot(in_v.in_NORMAL0.xyz, conv_mxt4x4_2(unity_WorldToObject).xyz);
          u_xlat8.x = dot(u_xlat2.xyz, u_xlat2.xyz);
          u_xlat8.x = rsqrt(u_xlat8.x);
          u_xlat2.xyz = (u_xlat8.xxx * u_xlat2.xyz);
          u_xlat8.x = dot(u_xlat1.xyz, u_xlat1.xyz);
          u_xlat8.x = rsqrt(u_xlat8.x);
          u_xlat3.xyz = (u_xlat8.xxx * u_xlat1.xyz);
          u_xlat8.x = dot(u_xlat2.xyz, u_xlat3.xyz);
          u_xlat2.xy = (_ScreenParams.yy * conv_mxt4x4_1(UNITY_MATRIX_P).xy);
          u_xlat2.xy = ((conv_mxt4x4_0(UNITY_MATRIX_P).xy * _ScreenParams.xx) + u_xlat2.xy);
          u_xlat2.xy = (abs(u_xlat2.xy) * float2(_ScaleX, _ScaleY));
          u_xlat2.xy = (u_xlat2.ww / u_xlat2.xy);
          u_xlat12 = dot(u_xlat2.xy, u_xlat2.xy);
          u_xlat2.xy = ((float2(_MaskSoftnessX, _MaskSoftnessY) * float2(0.25, 0.25)) + u_xlat2.xy);
          out_v.vs_TEXCOORD2.zw = (float2(0.25, 0.25) / u_xlat2.xy);
          u_xlat12 = rsqrt(u_xlat12);
          u_xlat13 = (abs(in_v.in_TEXCOORD1.y) * _GradientScale);
          u_xlat12 = (u_xlat12 * u_xlat13);
          u_xlat13 = (u_xlat12 * 1.5);
          u_xlat2.x = ((-_PerspectiveFilter) + 1);
          u_xlat2.x = (abs(u_xlat13) * u_xlat2.x);
          u_xlat12 = ((u_xlat12 * 1.5) + (-u_xlat2.x));
          u_xlat8.x = ((abs(u_xlat8.x) * u_xlat12) + u_xlat2.x);
          if((conv_mxt4x4_3(UNITY_MATRIX_P).w==0))
          {
              u_xlatb12 = 1;
          }
          else u_xlatb12 = 0;
          u_xlat6.x = u_xlatb12;
          if((0>=in_v.in_TEXCOORD1.y))
          {
              u_xlatb8 = 1;
          }
          else u_xlatb8 = 0;
          u_xlat8.x = u_xlatb8;
          u_xlat12 = ((-_WeightNormal) + _WeightBold);
          u_xlat8.x = ((u_xlat8.x * u_xlat12) + _WeightNormal);
          u_xlat8.x = ((u_xlat8.x * 0.25) + _FaceDilate);
          u_xlat8.x = (u_xlat8.x * _ScaleRatioA);
          u_xlat6.z = (u_xlat8.x * 0.5);
          out_v.vs_TEXCOORD1.yw = u_xlat6.xz;
          u_xlat12 = (0.5 / u_xlat6.x);
          u_xlat13 = (((-_OutlineWidth) * _ScaleRatioA) + 1);
          u_xlat13 = (((-_OutlineSoftness) * _ScaleRatioA) + u_xlat13);
          u_xlat13 = ((u_xlat13 * 0.5) + (-u_xlat12));
          out_v.vs_TEXCOORD1.x = (((-u_xlat8.x) * 0.5) + u_xlat13);
          u_xlat8.x = (((-u_xlat8.x) * 0.5) + 0.5);
          out_v.vs_TEXCOORD1.z = (u_xlat12 + u_xlat8.x);
          u_xlat2 = max(_ClipRect, float4((10 + 1), float(0), u_xlat8.x, u_xlat13));
      }
      
      #define CODE_BLOCK_FRAGMENT
      float4 u_xlat16_0;
      float4 u_xlat10_0;
      float u_xlat16_1;
      float4 u_xlat16_2;
      float3 u_xlat16_3;
      float u_xlat4;
      float u_xlat16_4;
      float4 u_xlat10_4;
      float u_xlat5;
      int u_xlatb5;
      float u_xlat16_6;
      float u_xlat9;
      float u_xlat16_11;
      OUT_Data_frag frag(IN_Data_Frag in_f)
      {
          OUT_Data_frag out_f;
          u_xlat10_0.x = tex2D(_MainTex, in_f.vs_TEXCOORD0.xy).w;
          u_xlat5 = (u_xlat10_0.x + (-in_f.vs_TEXCOORD1.x));
          u_xlat0.x = ((-u_xlat10_0.x) + in_f.vs_TEXCOORD1.z);
          if((u_xlat5<0))
          {
              u_xlatb5 = 1;
          }
          else u_xlatb5 = 0;
          if(((int(u_xlatb5) * (-1))!=0))
          {
              discard();
          }
          u_xlat5 = (_OutlineWidth * _ScaleRatioA);
          u_xlat5 = (u_xlat5 * in_f.vs_TEXCOORD1.y);
          u_xlat16_1 = min(u_xlat5, 1);
          u_xlat16_6 = (u_xlat5 * 0.5);
          u_xlat16_1 = sqrt(u_xlat16_1);
          u_xlat16_11 = ((u_xlat0.x * in_f.vs_TEXCOORD1.y) + u_xlat16_6);
          u_xlat16_11 = clamp(u_xlat16_11, 0, 1);
          u_xlat16_6 = ((u_xlat0.x * in_f.vs_TEXCOORD1.y) + (-u_xlat16_6));
          u_xlat16_1 = (u_xlat16_1 * u_xlat16_11);
          u_xlat0.xy = ((float2(_OutlineUVSpeedX, _OutlineUVSpeedY) * _Time.yy) + in_f.vs_TEXCOORD5.zw);
          u_xlat10_0 = tex2D(_OutlineTex, u_xlat0.xy);
          u_xlat16_2 = (u_xlat10_0 * _OutlineColor);
          u_xlat16_3.xyz = (in_f.vs_COLOR0.xyz * _FaceColor.xyz);
          u_xlat0.xy = ((float2(_FaceUVSpeedX, _FaceUVSpeedY) * _Time.yy) + in_f.vs_TEXCOORD5.xy);
          u_xlat10_4 = tex2D(_FaceTex, u_xlat0.xy);
          u_xlat16_0.xyz = (u_xlat16_3.xyz * u_xlat10_4.xyz);
          u_xlat16_4 = (u_xlat10_4.w * _FaceColor.w);
          u_xlat16_3.xyz = (u_xlat16_0.xyz * float3(u_xlat16_4, u_xlat16_4, u_xlat16_4));
          u_xlat16_2.xyz = ((u_xlat16_2.xyz * u_xlat16_2.www) + (-u_xlat16_3.xyz));
          u_xlat16_2.w = ((_OutlineColor.w * u_xlat10_0.w) + (-u_xlat16_4));
          u_xlat16_2 = (float4(u_xlat16_1, u_xlat16_1, u_xlat16_1, u_xlat16_1) * u_xlat16_2);
          u_xlat16_0.xyz = ((u_xlat16_0.xyz * float3(u_xlat16_4, u_xlat16_4, u_xlat16_4)) + u_xlat16_2.xyz);
          u_xlat16_0.w = ((_FaceColor.w * u_xlat10_4.w) + u_xlat16_2.w);
          u_xlat4 = (_OutlineSoftness * _ScaleRatioA);
          u_xlat9 = (u_xlat4 * in_f.vs_TEXCOORD1.y);
          u_xlat16_1 = ((u_xlat4 * in_f.vs_TEXCOORD1.y) + 1);
          u_xlat16_6 = ((u_xlat9 * 0.5) + u_xlat16_6);
          u_xlat16_1 = (u_xlat16_6 / u_xlat16_1);
          u_xlat16_1 = clamp(u_xlat16_1, 0, 1);
          u_xlat16_1 = ((-u_xlat16_1) + 1);
          u_xlat16_0 = (u_xlat16_0 * float4(u_xlat16_1, u_xlat16_1, u_xlat16_1, u_xlat16_1));
          out_f.SV_Target0 = (u_xlat16_0 * in_f.vs_COLOR0.wwww);
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  FallBack "TextMeshPro/Mobile/Distance Field"
}
