// Upgrade NOTE: replaced 'glstate_matrix_projection' with 'UNITY_MATRIX_P'

Shader "TextMeshPro/Distance Field (Surface)"
{
  Properties
  {
    _FaceTex ("Fill Texture", 2D) = "white" {}
    _FaceUVSpeedX ("Face UV Speed X", Range(-5, 5)) = 0
    _FaceUVSpeedY ("Face UV Speed Y", Range(-5, 5)) = 0
    _FaceColor ("Fill Color", Color) = (1,1,1,1)
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
    _BumpMap ("Normalmap", 2D) = "bump" {}
    _BumpOutline ("Bump Outline", Range(0, 1)) = 0.5
    _BumpFace ("Bump Face", Range(0, 1)) = 0.5
    _ReflectFaceColor ("Face Color", Color) = (0,0,0,1)
    _ReflectOutlineColor ("Outline Color", Color) = (0,0,0,1)
    _Cube ("Reflection Cubemap", Cube) = "black" {}
    _EnvMatrixRotation ("Texture Rotation", Vector) = (0,0,0,0)
    _SpecColor ("Specular Color", Color) = (0,0,0,1)
    _FaceShininess ("Face Shininess", Range(0, 1)) = 0
    _OutlineShininess ("Outline Shininess", Range(0, 1)) = 0
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
  }
  SubShader
  {
    Tags
    { 
      "IGNOREPROJECTOR" = "true"
      "QUEUE" = "Transparent"
      "RenderType" = "Transparent"
    }
    LOD 300
    Pass // ind: 1, name: FORWARD
    {
      Name "FORWARD"
      Tags
      { 
        "IGNOREPROJECTOR" = "true"
        "LIGHTMODE" = "FORWARDBASE"
        "QUEUE" = "Transparent"
        "RenderType" = "Transparent"
      }
      LOD 300
      ZWrite Off
      Cull Off
      Blend SrcAlpha OneMinusSrcAlpha
      ColorMask RGB
      GpuProgramID 38978
      // m_ProgramMask = 6
      //#ifdef DIRECTIONAL
      //  !!!!GLES
      CGPROGRAM
      #pragma multi_compile DIRECTIONAL
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
      uniform float4 unity_WorldTransformParams;
      uniform float4 UNITY_MATRIX_P[4];
      uniform float _FaceDilate;
      uniform float4 _EnvMatrix[4];
      uniform float _WeightNormal;
      uniform float _WeightBold;
      uniform float _ScaleRatioA;
      uniform float _VertexOffsetX;
      uniform float _VertexOffsetY;
      uniform float _GradientScale;
      uniform float _ScaleX;
      uniform float _ScaleY;
      uniform float _PerspectiveFilter;
      uniform float4 _MainTex_ST;
      uniform float4 _FaceTex_ST;
      uniform float4 _OutlineTex_ST;
      uniform float4 _LightColor0;
      uniform float4 _SpecColor;
      uniform float _FaceUVSpeedX;
      uniform float _FaceUVSpeedY;
      uniform float4 _FaceColor;
      uniform float _OutlineSoftness;
      uniform float _OutlineUVSpeedX;
      uniform float _OutlineUVSpeedY;
      uniform float4 _OutlineColor;
      uniform float _OutlineWidth;
      uniform float _Bevel;
      uniform float _BevelOffset;
      uniform float _BevelWidth;
      uniform float _BevelClamp;
      uniform float _BevelRoundness;
      uniform float _BumpOutline;
      uniform float _BumpFace;
      uniform float4 _ReflectFaceColor;
      uniform float4 _ReflectOutlineColor;
      uniform float _ShaderFlags;
      uniform float _TextureWidth;
      uniform float _TextureHeight;
      uniform float _FaceShininess;
      uniform float _OutlineShininess;
      uniform sampler2D _MainTex;
      uniform sampler2D _FaceTex;
      uniform sampler2D _OutlineTex;
      uniform sampler2D _BumpMap;
      uniform samplerCUBE _Cube;
      struct IN_Data_Vert
      {
          float4 in_POSITION0 :POSITION0;
          float4 in_TANGENT0 :TANGENT0;
          float3 in_NORMAL0 :NORMAL0;
          float4 in_TEXCOORD0 :TEXCOORD0;
          float4 in_TEXCOORD1 :TEXCOORD1;
          float4 in_COLOR0 :COLOR0;
      };
      
      struct OUT_Data_Vert
      {
          float4 vs_TEXCOORD0 :TEXCOORD0;
          float2 vs_TEXCOORD1 :TEXCOORD1;
          float2 vs_TEXCOORD5 :TEXCOORD5;
          float4 vs_TEXCOORD2 :TEXCOORD2;
          float4 vs_TEXCOORD3 :TEXCOORD3;
          float4 vs_TEXCOORD4 :TEXCOORD4;
          float4 vs_COLOR0 :COLOR0;
          float3 vs_TEXCOORD6 :TEXCOORD6;
          float3 vs_TEXCOORD7 :TEXCOORD7;
          float4 gl_Position :SV_POSITION;
      };
      
      struct IN_Data_Frag
      {
          float4 vs_TEXCOORD0 :TEXCOORD0;
          float2 vs_TEXCOORD1 :TEXCOORD1;
          float2 vs_TEXCOORD5 :TEXCOORD5;
          float4 vs_TEXCOORD2 :TEXCOORD2;
          float4 vs_TEXCOORD3 :TEXCOORD3;
          float4 vs_TEXCOORD4 :TEXCOORD4;
          float4 vs_COLOR0 :COLOR0;
          float3 vs_TEXCOORD6 :TEXCOORD6;
          float3 vs_TEXCOORD7 :TEXCOORD7;
      };
      
      struct OUT_Data_frag
      {
          float4 SV_Target0 :SV_Target0;
      };
      
      float4 u_xlat0;
      float4 u_xlat16_0;
      int u_xlati0;
      float4 u_xlat1;
      float4 u_xlat2;
      float4 u_xlat3;
      float3 u_xlat4;
      float u_xlat16_5;
      float3 u_xlat16_6;
      int u_xlati7;
      float u_xlat21;
      int u_xlatb21;
      float u_xlat22;
      OUT_Data_Vert vert(IN_Data_Vert in_v)
      {
          OUT_Data_Vert out_v;
          u_xlat0.xy = (in_v.in_POSITION0.xy + float2(_VertexOffsetX, _VertexOffsetY));
          u_xlat1 = (u_xlat0.yyyy * conv_mxt4x4_1(unity_ObjectToWorld));
          u_xlat1 = ((conv_mxt4x4_0(unity_ObjectToWorld) * u_xlat0.xxxx) + u_xlat1);
          u_xlat1 = ((conv_mxt4x4_2(unity_ObjectToWorld) * in_v.in_POSITION0.zzzz) + u_xlat1);
          u_xlat2 = (u_xlat1 + conv_mxt4x4_3(unity_ObjectToWorld));
          u_xlat1.xyz = ((conv_mxt4x4_3(unity_ObjectToWorld).xyz * in_v.in_POSITION0.www) + u_xlat1.xyz);
          u_xlat3 = (u_xlat2.yyyy * conv_mxt4x4_1(unity_MatrixVP));
          u_xlat3 = ((conv_mxt4x4_0(unity_MatrixVP) * u_xlat2.xxxx) + u_xlat3);
          u_xlat3 = ((conv_mxt4x4_2(unity_MatrixVP) * u_xlat2.zzzz) + u_xlat3);
          out_v.gl_Position = ((conv_mxt4x4_3(unity_MatrixVP) * u_xlat2.wwww) + u_xlat3);
          u_xlat21 = (in_v.in_TEXCOORD1.x * 0.000244140625);
          u_xlat3.x = floor(u_xlat21);
          u_xlat3.y = (((-u_xlat3.x) * 4096) + in_v.in_TEXCOORD1.x);
          u_xlat3.xy = (u_xlat3.xy * float2(0.001953125, 0.001953125));
          out_v.vs_TEXCOORD0.zw = ((u_xlat3.xy * _FaceTex_ST.xy) + _FaceTex_ST.zw);
          out_v.vs_TEXCOORD1.xy = ((u_xlat3.xy * _OutlineTex_ST.xy) + _OutlineTex_ST.zw);
          out_v.vs_TEXCOORD0.xy = ((in_v.in_TEXCOORD0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
          if((0>=in_v.in_TEXCOORD1.y))
          {
              u_xlatb21 = 1;
          }
          else u_xlatb21 = 0;
          u_xlat21 = u_xlatb21;
          u_xlat22 = ((-_WeightNormal) + _WeightBold);
          u_xlat21 = ((u_xlat21 * u_xlat22) + _WeightNormal);
          u_xlat21 = ((u_xlat21 * 0.25) + _FaceDilate);
          u_xlat21 = (u_xlat21 * _ScaleRatioA);
          out_v.vs_TEXCOORD5.x = (u_xlat21 * 0.5);
          u_xlat21 = (u_xlat2.y * conv_mxt4x4_1(unity_MatrixVP).w);
          u_xlat21 = ((conv_mxt4x4_0(unity_MatrixVP).w * u_xlat2.x) + u_xlat21);
          u_xlat21 = ((conv_mxt4x4_2(unity_MatrixVP).w * u_xlat2.z) + u_xlat21);
          u_xlat21 = ((conv_mxt4x4_3(unity_MatrixVP).w * u_xlat2.w) + u_xlat21);
          u_xlat2.xy = (_ScreenParams.yy * conv_mxt4x4_1(UNITY_MATRIX_P).xy);
          u_xlat2.xy = ((conv_mxt4x4_0(UNITY_MATRIX_P).xy * _ScreenParams.xx) + u_xlat2.xy);
          u_xlat2.xy = (u_xlat2.xy * float2(_ScaleX, _ScaleY));
          u_xlat2.xy = (float2(u_xlat21, u_xlat21) / u_xlat2.xy);
          u_xlat21 = dot(u_xlat2.xy, u_xlat2.xy);
          u_xlat21 = rsqrt(u_xlat21);
          u_xlat22 = (abs(in_v.in_TEXCOORD1.y) * _GradientScale);
          u_xlat21 = (u_xlat21 * u_xlat22);
          u_xlat22 = (u_xlat21 * 1.5);
          u_xlat2.x = ((-_PerspectiveFilter) + 1);
          u_xlat22 = (u_xlat22 * u_xlat2.x);
          u_xlat21 = ((u_xlat21 * 1.5) + (-u_xlat22));
          u_xlat2.xyz = (_WorldSpaceCameraPos.yyy * conv_mxt4x4_1(unity_WorldToObject).xyz);
          u_xlat2.xyz = ((conv_mxt4x4_0(unity_WorldToObject).xyz * _WorldSpaceCameraPos.xxx) + u_xlat2.xyz);
          u_xlat2.xyz = ((conv_mxt4x4_2(unity_WorldToObject).xyz * _WorldSpaceCameraPos.zzz) + u_xlat2.xyz);
          u_xlat2.xyz = (u_xlat2.xyz + conv_mxt4x4_3(unity_WorldToObject).xyz);
          u_xlat0.z = in_v.in_POSITION0.z;
          u_xlat0.xyz = ((-u_xlat0.xyz) + u_xlat2.xyz);
          u_xlat0.x = dot(in_v.in_NORMAL0.xyz, u_xlat0.xyz);
          if(int((0<u_xlat0.x)))
          {
              u_xlati7 = 1;
          }
          else u_xlati7 = 0;
          if(int((u_xlat0.x<0)))
          {
              u_xlati0 = 1;
          }
          else u_xlati0 = 0;
          u_xlati0 = ((-u_xlati7) + u_xlati0);
          u_xlat0.x = float(u_xlati0);
          u_xlat0.xyz = (u_xlat0.xxx * in_v.in_NORMAL0.xyz);
          u_xlat2.x = dot(u_xlat0.xyz, conv_mxt4x4_0(unity_WorldToObject).xyz);
          u_xlat2.y = dot(u_xlat0.xyz, conv_mxt4x4_1(unity_WorldToObject).xyz);
          u_xlat2.z = dot(u_xlat0.xyz, conv_mxt4x4_2(unity_WorldToObject).xyz);
          u_xlat0.x = dot(u_xlat2.xyz, u_xlat2.xyz);
          u_xlat0.x = rsqrt(u_xlat0.x);
          u_xlat2 = (u_xlat0.xxxx * u_xlat2.xyzz);
          u_xlat0.xyz = ((-u_xlat1.xyz) + _WorldSpaceCameraPos.xyz);
          u_xlat3.x = dot(u_xlat0.xyz, u_xlat0.xyz);
          u_xlat3.x = rsqrt(u_xlat3.x);
          u_xlat3.xyz = (u_xlat0.xyz * u_xlat3.xxx);
          u_xlat3.x = dot(u_xlat2.xyw, u_xlat3.xyz);
          out_v.vs_TEXCOORD5.y = ((abs(u_xlat3.x) * u_xlat21) + u_xlat22);
          out_v.vs_TEXCOORD2.w = u_xlat1.x;
          u_xlat3.xyz = (in_v.in_TANGENT0.yyy * conv_mxt4x4_1(unity_ObjectToWorld).yzx);
          u_xlat3.xyz = ((conv_mxt4x4_0(unity_ObjectToWorld).yzx * in_v.in_TANGENT0.xxx) + u_xlat3.xyz);
          u_xlat3.xyz = ((conv_mxt4x4_2(unity_ObjectToWorld).yzx * in_v.in_TANGENT0.zzz) + u_xlat3.xyz);
          u_xlat21 = dot(u_xlat3.xyz, u_xlat3.xyz);
          u_xlat21 = rsqrt(u_xlat21);
          u_xlat3.xyz = (float3(u_xlat21, u_xlat21, u_xlat21) * u_xlat3.xyz);
          u_xlat4.xyz = (u_xlat2.wxy * u_xlat3.xyz);
          u_xlat4.xyz = ((u_xlat2.ywx * u_xlat3.yzx) + (-u_xlat4.xyz));
          u_xlat21 = (in_v.in_TANGENT0.w * unity_WorldTransformParams.w);
          u_xlat4.xyz = (float3(u_xlat21, u_xlat21, u_xlat21) * u_xlat4.xyz);
          out_v.vs_TEXCOORD2.y = u_xlat4.x;
          out_v.vs_TEXCOORD2.z = u_xlat2.x;
          out_v.vs_TEXCOORD2.x = u_xlat3.z;
          out_v.vs_TEXCOORD3.x = u_xlat3.x;
          out_v.vs_TEXCOORD4.x = u_xlat3.y;
          out_v.vs_TEXCOORD3.w = u_xlat1.y;
          out_v.vs_TEXCOORD4.w = u_xlat1.z;
          out_v.vs_TEXCOORD3.z = u_xlat2.y;
          out_v.vs_TEXCOORD3.y = u_xlat4.y;
          out_v.vs_TEXCOORD4.y = u_xlat4.z;
          out_v.vs_TEXCOORD4.z = u_xlat2.w;
          out_v.vs_COLOR0 = in_v.in_COLOR0;
          u_xlat1.xyz = (u_xlat0.yyy * conv_mxt4x4_1(_EnvMatrix).xyz);
          u_xlat0.xyw = ((conv_mxt4x4_0(_EnvMatrix).xyz * u_xlat0.xxx) + u_xlat1.xyz);
          out_v.vs_TEXCOORD6.xyz = ((conv_mxt4x4_2(_EnvMatrix).xyz * u_xlat0.zzz) + u_xlat0.xyw);
          u_xlat16_5 = (u_xlat2.y * u_xlat2.y);
          u_xlat16_5 = ((u_xlat2.x * u_xlat2.x) + (-u_xlat16_5));
          u_xlat16_0 = (u_xlat2.ywzx * u_xlat2);
          u_xlat16_6.x = dot(unity_SHBr, u_xlat16_0);
          u_xlat16_6.y = dot(unity_SHBg, u_xlat16_0);
          u_xlat16_6.z = dot(unity_SHBb, u_xlat16_0);
          out_v.vs_TEXCOORD7.xyz = ((unity_SHC.xyz * float3(u_xlat16_5, u_xlat16_5, u_xlat16_5)) + u_xlat16_6.xyz);
          return out_v;
          (-1);
          0;
          (-1);
          0;
          1;
          float(0);
      }
      
      #define CODE_BLOCK_FRAGMENT
      float3 u_xlat10_0;
      float3 u_xlat16_1;
      float4 u_xlat16_2;
      float4 u_xlat16_3;
      float4 u_xlat10_3;
      float4 u_xlat16_4;
      float4 u_xlat10_4;
      float3 u_xlat6;
      float4 u_xlat16_7;
      float3 u_xlat8;
      float3 u_xlat16_8;
      float3 u_xlat16_9;
      float3 u_xlat16_11;
      float2 u_xlat12;
      float3 u_xlat16_12;
      int u_xlatb12;
      float u_xlat16_13;
      float u_xlat16;
      float u_xlat16_16;
      float3 u_xlat10_16;
      int u_xlatb22;
      float u_xlat30;
      float u_xlat16_30;
      float u_xlat10_30;
      float u_xlat16_31;
      float u_xlat16_36;
      OUT_Data_frag frag(IN_Data_Frag in_f)
      {
          OUT_Data_frag out_f;
          u_xlat0.x = in_f.vs_TEXCOORD2.w;
          u_xlat0.y = in_f.vs_TEXCOORD3.w;
          u_xlat0.z = in_f.vs_TEXCOORD4.w;
          u_xlat0.xyz = ((-u_xlat0.xyz) + _WorldSpaceCameraPos.xyz);
          u_xlat30 = dot(u_xlat0.xyz, u_xlat0.xyz);
          u_xlat30 = rsqrt(u_xlat30);
          u_xlat16_1.xyz = ((u_xlat0.xyz * float3(u_xlat30, u_xlat30, u_xlat30)) + _WorldSpaceLightPos0.xyz);
          u_xlat16_31 = dot(u_xlat16_1.xyz, u_xlat16_1.xyz);
          u_xlat16_31 = rsqrt(u_xlat16_31);
          u_xlat16_1.xyz = (float3(u_xlat16_31, u_xlat16_31, u_xlat16_31) * u_xlat16_1.xyz);
          u_xlat0.x = (in_f.vs_TEXCOORD5.x + _BevelOffset);
          u_xlat2.xy = (float2(1, 1) / float2(_TextureWidth, _TextureHeight));
          u_xlat2.z = 0;
          u_xlat3 = ((-u_xlat2.xzzy) + in_f.vs_TEXCOORD0.xyxy);
          u_xlat2 = (u_xlat2.xzzy + in_f.vs_TEXCOORD0.xyxy);
          u_xlat4.x = tex2D(_MainTex, u_xlat3.xy).w;
          u_xlat4.z = tex2D(_MainTex, u_xlat3.zw).w;
          u_xlat4.y = tex2D(_MainTex, u_xlat2.xy).w;
          u_xlat4.w = tex2D(_MainTex, u_xlat2.zw).w;
          u_xlat0 = (u_xlat0.xxxx + u_xlat4);
          u_xlat0 = (u_xlat0 + float4(-0.5, (-0.5), (-0.5), (-0.5)));
          u_xlat2.x = (_BevelWidth + _OutlineWidth);
          u_xlat2.x = max(u_xlat2.x, 0.00999999978);
          u_xlat0 = (u_xlat0 / u_xlat2.xxxx);
          u_xlat2.x = (u_xlat2.x * _Bevel);
          u_xlat2.x = (u_xlat2.x * _GradientScale);
          u_xlat2.x = (u_xlat2.x * (-2));
          u_xlat0 = (u_xlat0 + float4(0.5, 0.5, 0.5, 0.5));
          u_xlat0 = clamp(u_xlat0, 0, 1);
          u_xlat3 = ((u_xlat0 * float4(2, 2, 2, 2)) + float4(-1, (-1), (-1), (-1)));
      }
      
      
      //#endif // DIRECTIONAL
      ENDCG
      
    } // end phase
    Pass // ind: 2, name: FORWARD
    {
      Name "FORWARD"
      Tags
      { 
        "IGNOREPROJECTOR" = "true"
        "LIGHTMODE" = "FORWARDADD"
        "QUEUE" = "Transparent"
        "RenderType" = "Transparent"
      }
      LOD 300
      ZWrite Off
      Cull Off
      Blend SrcAlpha One
      ColorMask RGB
      GpuProgramID 70567
      // m_ProgramMask = 6
      //#ifdef POINT
      //  !!!!GLES
      CGPROGRAM
      #pragma multi_compile POINT
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
      uniform float4 unity_WorldTransformParams;
      uniform float4 UNITY_MATRIX_P[4];
      uniform float4 unity_WorldToLight[4];
      uniform float _FaceDilate;
      uniform float4 _EnvMatrix[4];
      uniform float _WeightNormal;
      uniform float _WeightBold;
      uniform float _ScaleRatioA;
      uniform float _VertexOffsetX;
      uniform float _VertexOffsetY;
      uniform float _GradientScale;
      uniform float _ScaleX;
      uniform float _ScaleY;
      uniform float _PerspectiveFilter;
      uniform float4 _MainTex_ST;
      uniform float4 _FaceTex_ST;
      uniform float4 _OutlineTex_ST;
      uniform float4 _LightColor0;
      uniform float4 _SpecColor;
      uniform float _FaceUVSpeedX;
      uniform float _FaceUVSpeedY;
      uniform float4 _FaceColor;
      uniform float _OutlineSoftness;
      uniform float _OutlineUVSpeedX;
      uniform float _OutlineUVSpeedY;
      uniform float4 _OutlineColor;
      uniform float _OutlineWidth;
      uniform float _Bevel;
      uniform float _BevelOffset;
      uniform float _BevelWidth;
      uniform float _BevelClamp;
      uniform float _BevelRoundness;
      uniform float _BumpOutline;
      uniform float _BumpFace;
      uniform float _ShaderFlags;
      uniform float _TextureWidth;
      uniform float _TextureHeight;
      uniform float _FaceShininess;
      uniform float _OutlineShininess;
      uniform sampler2D _MainTex;
      uniform sampler2D _FaceTex;
      uniform sampler2D _OutlineTex;
      uniform sampler2D _BumpMap;
      uniform sampler2D _LightTexture0;
      struct IN_Data_Vert
      {
          float4 in_POSITION0 :POSITION0;
          float4 in_TANGENT0 :TANGENT0;
          float3 in_NORMAL0 :NORMAL0;
          float4 in_TEXCOORD0 :TEXCOORD0;
          float4 in_TEXCOORD1 :TEXCOORD1;
          float4 in_COLOR0 :COLOR0;
      };
      
      struct OUT_Data_Vert
      {
          float4 vs_TEXCOORD0 :TEXCOORD0;
          float2 vs_TEXCOORD1 :TEXCOORD1;
          float2 vs_TEXCOORD6 :TEXCOORD6;
          float3 vs_TEXCOORD2 :TEXCOORD2;
          float3 vs_TEXCOORD3 :TEXCOORD3;
          float3 vs_TEXCOORD4 :TEXCOORD4;
          float3 vs_TEXCOORD5 :TEXCOORD5;
          float4 vs_COLOR0 :COLOR0;
          float3 vs_TEXCOORD7 :TEXCOORD7;
          float3 vs_TEXCOORD8 :TEXCOORD8;
          float4 gl_Position :SV_POSITION;
      };
      
      struct IN_Data_Frag
      {
          float4 vs_TEXCOORD0 :TEXCOORD0;
          float2 vs_TEXCOORD1 :TEXCOORD1;
          float2 vs_TEXCOORD6 :TEXCOORD6;
          float3 vs_TEXCOORD2 :TEXCOORD2;
          float3 vs_TEXCOORD3 :TEXCOORD3;
          float3 vs_TEXCOORD4 :TEXCOORD4;
          float3 vs_TEXCOORD5 :TEXCOORD5;
          float4 vs_COLOR0 :COLOR0;
      };
      
      struct OUT_Data_frag
      {
          float4 SV_Target0 :SV_Target0;
      };
      
      float3 u_xlat0;
      int u_xlati0;
      float4 u_xlat1;
      float4 u_xlat2;
      float4 u_xlat3;
      float3 u_xlat4;
      float u_xlat5;
      int u_xlati5;
      float3 u_xlat7;
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
          u_xlat3 = (u_xlat2.yyyy * conv_mxt4x4_1(unity_MatrixVP));
          u_xlat3 = ((conv_mxt4x4_0(unity_MatrixVP) * u_xlat2.xxxx) + u_xlat3);
          u_xlat3 = ((conv_mxt4x4_2(unity_MatrixVP) * u_xlat2.zzzz) + u_xlat3);
          out_v.gl_Position = ((conv_mxt4x4_3(unity_MatrixVP) * u_xlat2.wwww) + u_xlat3);
          u_xlat15 = (in_v.in_TEXCOORD1.x * 0.000244140625);
          u_xlat3.x = floor(u_xlat15);
          u_xlat3.y = (((-u_xlat3.x) * 4096) + in_v.in_TEXCOORD1.x);
          u_xlat3.xy = (u_xlat3.xy * float2(0.001953125, 0.001953125));
          out_v.vs_TEXCOORD0.zw = ((u_xlat3.xy * _FaceTex_ST.xy) + _FaceTex_ST.zw);
          out_v.vs_TEXCOORD1.xy = ((u_xlat3.xy * _OutlineTex_ST.xy) + _OutlineTex_ST.zw);
          out_v.vs_TEXCOORD0.xy = ((in_v.in_TEXCOORD0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
          u_xlat15 = (u_xlat2.y * conv_mxt4x4_1(unity_MatrixVP).w);
          u_xlat15 = ((conv_mxt4x4_0(unity_MatrixVP).w * u_xlat2.x) + u_xlat15);
          u_xlat15 = ((conv_mxt4x4_2(unity_MatrixVP).w * u_xlat2.z) + u_xlat15);
          u_xlat15 = ((conv_mxt4x4_3(unity_MatrixVP).w * u_xlat2.w) + u_xlat15);
          u_xlat2.xy = (_ScreenParams.yy * conv_mxt4x4_1(UNITY_MATRIX_P).xy);
          u_xlat2.xy = ((conv_mxt4x4_0(UNITY_MATRIX_P).xy * _ScreenParams.xx) + u_xlat2.xy);
          u_xlat2.xy = (u_xlat2.xy * float2(_ScaleX, _ScaleY));
          u_xlat2.xy = (float2(u_xlat15, u_xlat15) / u_xlat2.xy);
          u_xlat15 = dot(u_xlat2.xy, u_xlat2.xy);
          u_xlat15 = rsqrt(u_xlat15);
          u_xlat2.x = (abs(in_v.in_TEXCOORD1.y) * _GradientScale);
          u_xlat15 = (u_xlat15 * u_xlat2.x);
          u_xlat2.x = (u_xlat15 * 1.5);
          u_xlat7.x = ((-_PerspectiveFilter) + 1);
          u_xlat2.x = (u_xlat7.x * u_xlat2.x);
          u_xlat15 = ((u_xlat15 * 1.5) + (-u_xlat2.x));
          u_xlat7.xyz = (_WorldSpaceCameraPos.yyy * conv_mxt4x4_1(unity_WorldToObject).xyz);
          u_xlat7.xyz = ((conv_mxt4x4_0(unity_WorldToObject).xyz * _WorldSpaceCameraPos.xxx) + u_xlat7.xyz);
          u_xlat7.xyz = ((conv_mxt4x4_2(unity_WorldToObject).xyz * _WorldSpaceCameraPos.zzz) + u_xlat7.xyz);
          u_xlat7.xyz = (u_xlat7.xyz + conv_mxt4x4_3(unity_WorldToObject).xyz);
          u_xlat0.z = in_v.in_POSITION0.z;
          u_xlat0.xyz = ((-u_xlat0.xyz) + u_xlat7.xyz);
          u_xlat0.x = dot(in_v.in_NORMAL0.xyz, u_xlat0.xyz);
          if(int((0<u_xlat0.x)))
          {
              u_xlati5 = 1;
          }
          else u_xlati5 = 0;
          if(int((u_xlat0.x<0)))
          {
              u_xlati0 = 1;
          }
          else u_xlati0 = 0;
          u_xlati0 = ((-u_xlati5) + u_xlati0);
          u_xlat0.x = float(u_xlati0);
          u_xlat0.xyz = (u_xlat0.xxx * in_v.in_NORMAL0.xyz);
          u_xlat3.y = dot(u_xlat0.xyz, conv_mxt4x4_0(unity_WorldToObject).xyz);
          u_xlat3.z = dot(u_xlat0.xyz, conv_mxt4x4_1(unity_WorldToObject).xyz);
          u_xlat3.x = dot(u_xlat0.xyz, conv_mxt4x4_2(unity_WorldToObject).xyz);
          u_xlat0.x = dot(u_xlat3.xyz, u_xlat3.xyz);
          u_xlat0.x = rsqrt(u_xlat0.x);
          u_xlat0.xyz = (u_xlat0.xxx * u_xlat3.xyz);
          u_xlat7.xyz = ((conv_mxt4x4_3(unity_ObjectToWorld).xyz * in_v.in_POSITION0.www) + u_xlat1.xyz);
          u_xlat1 = ((conv_mxt4x4_3(unity_ObjectToWorld) * in_v.in_POSITION0.wwww) + u_xlat1);
          u_xlat3.xyz = ((-u_xlat7.xyz) + _WorldSpaceCameraPos.xyz);
          out_v.vs_TEXCOORD5.xyz = u_xlat7.xyz;
          u_xlat7.x = dot(u_xlat3.xyz, u_xlat3.xyz);
          u_xlat7.x = rsqrt(u_xlat7.x);
          u_xlat7.xyz = (u_xlat7.xxx * u_xlat3.xyz);
          u_xlat7.x = dot(u_xlat0.yzx, u_xlat7.xyz);
          out_v.vs_TEXCOORD6.y = ((abs(u_xlat7.x) * u_xlat15) + u_xlat2.x);
          if((0>=in_v.in_TEXCOORD1.y))
          {
              u_xlatb15 = 1;
          }
          else u_xlatb15 = 0;
          u_xlat15 = u_xlatb15;
          u_xlat2.x = ((-_WeightNormal) + _WeightBold);
          u_xlat15 = ((u_xlat15 * u_xlat2.x) + _WeightNormal);
          u_xlat15 = ((u_xlat15 * 0.25) + _FaceDilate);
          u_xlat15 = (u_xlat15 * _ScaleRatioA);
          out_v.vs_TEXCOORD6.x = (u_xlat15 * 0.5);
          out_v.vs_TEXCOORD2.z = u_xlat0.y;
          u_xlat2.xyz = (in_v.in_TANGENT0.yyy * conv_mxt4x4_1(unity_ObjectToWorld).yzx);
          u_xlat2.xyz = ((conv_mxt4x4_0(unity_ObjectToWorld).yzx * in_v.in_TANGENT0.xxx) + u_xlat2.xyz);
          u_xlat2.xyz = ((conv_mxt4x4_2(unity_ObjectToWorld).yzx * in_v.in_TANGENT0.zzz) + u_xlat2.xyz);
          u_xlat15 = dot(u_xlat2.xyz, u_xlat2.xyz);
          u_xlat15 = rsqrt(u_xlat15);
          u_xlat2.xyz = (float3(u_xlat15, u_xlat15, u_xlat15) * u_xlat2.xyz);
          u_xlat4.xyz = (u_xlat0.xyz * u_xlat2.xyz);
          u_xlat4.xyz = ((u_xlat0.zxy * u_xlat2.yzx) + (-u_xlat4.xyz));
          u_xlat5 = (in_v.in_TANGENT0.w * unity_WorldTransformParams.w);
          u_xlat4.xyz = (float3(u_xlat5, u_xlat5, u_xlat5) * u_xlat4.xyz);
          out_v.vs_TEXCOORD2.y = u_xlat4.x;
          out_v.vs_TEXCOORD2.x = u_xlat2.z;
          out_v.vs_TEXCOORD3.z = u_xlat0.z;
          out_v.vs_TEXCOORD4.z = u_xlat0.x;
          out_v.vs_TEXCOORD3.x = u_xlat2.x;
          out_v.vs_TEXCOORD4.x = u_xlat2.y;
          out_v.vs_TEXCOORD3.y = u_xlat4.y;
          out_v.vs_TEXCOORD4.y = u_xlat4.z;
          out_v.vs_COLOR0 = in_v.in_COLOR0;
          u_xlat0.xyz = (u_xlat3.yyy * conv_mxt4x4_1(_EnvMatrix).xyz);
          u_xlat0.xyz = ((conv_mxt4x4_0(_EnvMatrix).xyz * u_xlat3.xxx) + u_xlat0.xyz);
          out_v.vs_TEXCOORD7.xyz = ((conv_mxt4x4_2(_EnvMatrix).xyz * u_xlat3.zzz) + u_xlat0.xyz);
          u_xlat0.xyz = (u_xlat1.yyy * conv_mxt4x4_1(unity_WorldToLight).xyz);
          u_xlat0.xyz = ((conv_mxt4x4_0(unity_WorldToLight).xyz * u_xlat1.xxx) + u_xlat0.xyz);
          u_xlat0.xyz = ((conv_mxt4x4_2(unity_WorldToLight).xyz * u_xlat1.zzz) + u_xlat0.xyz);
          out_v.vs_TEXCOORD8.xyz = ((conv_mxt4x4_3(unity_WorldToLight).xyz * u_xlat1.www) + u_xlat0.xyz);
          return out_v;
          1;
          float(0);
          (-1);
          0;
          (-1);
          0;
      }
      
      #define CODE_BLOCK_FRAGMENT
      float3 u_xlat16_0;
      float4 u_xlat16_1;
      float4 u_xlat16_2;
      float4 u_xlat10_2;
      float4 u_xlat16_3;
      float4 u_xlat10_3;
      float4 u_xlat16_4;
      float4 u_xlat16_5;
      float3 u_xlat6;
      float4 u_xlat16_6;
      float4 u_xlat16_7;
      float3 u_xlat8;
      float u_xlat16_8;
      float2 u_xlat9;
      float3 u_xlat16_9;
      int u_xlatb9;
      float u_xlat16_10;
      float3 u_xlat16_12;
      float3 u_xlat10_13;
      int u_xlatb17;
      float u_xlat24;
      float u_xlat16_24;
      float u_xlat10_24;
      float u_xlat29;
      OUT_Data_frag frag(IN_Data_Frag in_f)
      {
          OUT_Data_frag out_f;
          u_xlat0.x = (in_f.vs_TEXCOORD6.x + _BevelOffset);
          u_xlat1.xy = (float2(1, 1) / float2(_TextureWidth, _TextureHeight));
          u_xlat1.z = 0;
          u_xlat2 = ((-u_xlat1.xzzy) + in_f.vs_TEXCOORD0.xyxy);
          u_xlat1 = (u_xlat1.xzzy + in_f.vs_TEXCOORD0.xyxy);
          u_xlat3.x = tex2D(_MainTex, u_xlat2.xy).w;
          u_xlat3.z = tex2D(_MainTex, u_xlat2.zw).w;
          u_xlat3.y = tex2D(_MainTex, u_xlat1.xy).w;
          u_xlat3.w = tex2D(_MainTex, u_xlat1.zw).w;
          u_xlat0 = (u_xlat0.xxxx + u_xlat3);
          u_xlat0 = (u_xlat0 + float4(-0.5, (-0.5), (-0.5), (-0.5)));
          u_xlat1.x = (_BevelWidth + _OutlineWidth);
          u_xlat1.x = max(u_xlat1.x, 0.00999999978);
          u_xlat0 = (u_xlat0 / u_xlat1.xxxx);
          u_xlat1.x = (u_xlat1.x * _Bevel);
          u_xlat1.x = (u_xlat1.x * _GradientScale);
          u_xlat1.x = (u_xlat1.x * (-2));
          u_xlat0 = (u_xlat0 + float4(0.5, 0.5, 0.5, 0.5));
          u_xlat0 = clamp(u_xlat0, 0, 1);
          u_xlat2 = ((u_xlat0 * float4(2, 2, 2, 2)) + float4(-1, (-1), (-1), (-1)));
      }
      
      
      //#endif // POINT
      ENDCG
      
    } // end phase
    Pass // ind: 3, name: Caster
    {
      Name "Caster"
      Tags
      { 
        "IGNOREPROJECTOR" = "true"
        "LIGHTMODE" = "SHADOWCASTER"
        "QUEUE" = "Transparent"
        "RenderType" = "Transparent"
        "SHADOWSUPPORT" = "true"
      }
      LOD 300
      Cull Off
      Offset 1, 1
      Fog
      { 
        Mode  Off
      } 
      ColorMask RGB
      GpuProgramID 132202
      // m_ProgramMask = 6
      //#ifdef SHADOWS_DEPTH
      //  !!!!GLES
      CGPROGRAM
      #pragma multi_compile SHADOWS_DEPTH
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
      uniform float4 _MainTex_ST;
      uniform float4 _OutlineTex_ST;
      uniform float _OutlineWidth;
      uniform float _FaceDilate;
      uniform float _ScaleRatioA;
      uniform sampler2D _MainTex;
      struct IN_Data_Vert
      {
          float4 in_POSITION0 :POSITION0;
          float4 in_TEXCOORD0 :TEXCOORD0;
      };
      
      struct OUT_Data_Vert
      {
          float2 vs_TEXCOORD1 :TEXCOORD1;
          float2 vs_TEXCOORD3 :TEXCOORD3;
          float vs_TEXCOORD2 :TEXCOORD2;
          float4 gl_Position :SV_POSITION;
      };
      
      struct IN_Data_Frag
      {
          float2 vs_TEXCOORD1 :TEXCOORD1;
          float vs_TEXCOORD2 :TEXCOORD2;
      };
      
      struct OUT_Data_frag
      {
          float4 SV_Target0 :SV_Target0;
      };
      
      float4 u_xlat0;
      float4 u_xlat1;
      float u_xlat4;
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
          u_xlat1.x = (unity_LightShadowBias.x / u_xlat0.w);
          u_xlat1.x = clamp(u_xlat1.x, 0, 1);
          u_xlat4 = (u_xlat0.z + u_xlat1.x);
          u_xlat1.x = max((-u_xlat0.w), u_xlat4);
          out_v.gl_Position.xyw = u_xlat0.xyw;
          u_xlat0.x = ((-u_xlat4) + u_xlat1.x);
          out_v.gl_Position.z = ((unity_LightShadowBias.y * u_xlat0.x) + u_xlat4);
          out_v.vs_TEXCOORD1.xy = ((in_v.in_TEXCOORD0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
          out_v.vs_TEXCOORD3.xy = ((in_v.in_TEXCOORD0.xy * _OutlineTex_ST.xy) + _OutlineTex_ST.zw);
          u_xlat0.x = (((-_OutlineWidth) * _ScaleRatioA) + 1);
          u_xlat0.x = (((-_FaceDilate) * _ScaleRatioA) + u_xlat0.x);
          out_v.vs_TEXCOORD2 = (u_xlat0.x * 0.5);
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      float u_xlat10_0;
      int u_xlatb0;
      OUT_Data_frag frag(IN_Data_Frag in_f)
      {
          OUT_Data_frag out_f;
          u_xlat10_0 = tex2D(_MainTex, in_f.vs_TEXCOORD1.xy).w;
          u_xlat0 = (u_xlat10_0 + (-in_f.vs_TEXCOORD2));
          if((u_xlat0.x < 0 && u_xlat0.y < 0 && u_xlat0.z < 0 && u_xlat0.w < 0))
          {
              u_xlatb0 = 1;
          }
          else u_xlatb0 = 0;
          if(((int(u_xlatb0) * (-1))!=0))
          {
              discard();
          }
          out_f.SV_Target0 = float4(0, 0, 0, 0);
          return out_f;
      }
      
      
      //#endif // SHADOWS_DEPTH
      ENDCG
      
    } // end phase
  }
  FallBack ""
}
