// Upgrade NOTE: replaced 'glstate_matrix_projection' with 'UNITY_MATRIX_P'

Shader "TextMeshPro/Bitmap"
{
  Properties
  {
    _MainTex ("Font Atlas", 2D) = "white" {}
    _FaceTex ("Font Texture", 2D) = "white" {}
    _FaceColor ("Text Color", Color) = (1,1,1,1)
    _VertexOffsetX ("Vertex OffsetX", float) = 0
    _VertexOffsetY ("Vertex OffsetY", float) = 0
    _MaskSoftnessX ("Mask SoftnessX", float) = 0
    _MaskSoftnessY ("Mask SoftnessY", float) = 0
    _ClipRect ("Clip Rect", Vector) = (-32767,-32767,32767,32767)
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
      "QUEUE" = "Transparent"
      "RenderType" = "Transparent"
    }
    Pass // ind: 1, name: 
    {
      Tags
      { 
        "IGNOREPROJECTOR" = "true"
        "QUEUE" = "Transparent"
        "RenderType" = "Transparent"
      }
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
      Blend SrcAlpha OneMinusSrcAlpha
      ColorMask 0
      GpuProgramID 47968
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
      uniform float4 _FaceTex_ST;
      uniform float4 _FaceColor;
      uniform float _VertexOffsetX;
      uniform float _VertexOffsetY;
      uniform float4 _ClipRect;
      uniform float _MaskSoftnessX;
      uniform float _MaskSoftnessY;
      uniform sampler2D _MainTex;
      uniform sampler2D _FaceTex;
      struct IN_Data_Vert
      {
          float4 in_POSITION0 :POSITION0;
          float4 in_COLOR0 :COLOR0;
          float2 in_TEXCOORD0 :TEXCOORD0;
          float2 in_TEXCOORD1 :TEXCOORD1;
      };
      
      struct OUT_Data_Vert
      {
          float4 vs_COLOR0 :COLOR0;
          float2 vs_TEXCOORD0 :TEXCOORD0;
          float2 vs_TEXCOORD1 :TEXCOORD1;
          float4 vs_TEXCOORD2 :TEXCOORD2;
          float4 gl_Position :SV_POSITION;
      };
      
      struct IN_Data_Frag
      {
          float4 vs_COLOR0 :COLOR0;
          float2 vs_TEXCOORD0 :TEXCOORD0;
          float2 vs_TEXCOORD1 :TEXCOORD1;
      };
      
      struct OUT_Data_frag
      {
          float4 SV_Target0 :SV_Target0;
      };
      
      float2 u_xlat0;
      float4 u_xlat1;
      float4 u_xlat2;
      float2 u_xlat6;
      #define __IL_START_FUN_roundEven__float____x_float
      #define __IL_START_FUN_roundEven__float2____a_float2
      #define __IL_START_FUN_roundEven__float3____a_float3
      float4 roundEven(float4 a)
      {
          a.x = roundEven(a.x);
          a.y = roundEven(a.y);
          a.z = roundEven(a.z);
          a.w = roundEven(a.w);
          return a;
      }
      
      OUT_Data_Vert vert(IN_Data_Vert in_v)
      {
          OUT_Data_Vert out_v;
          u_xlat0.x = (in_v.in_POSITION0.w * 0.5);
          u_xlat0.xy = (u_xlat0.xx / _ScreenParams.xy);
          u_xlat6.xy = (in_v.in_POSITION0.xy + float2(_VertexOffsetX, _VertexOffsetY));
          u_xlat0.xy = (u_xlat0.xy + u_xlat6.xy);
          u_xlat1 = (u_xlat0.yyyy * conv_mxt4x4_1(unity_ObjectToWorld));
          u_xlat1 = ((conv_mxt4x4_0(unity_ObjectToWorld) * u_xlat0.xxxx) + u_xlat1);
          u_xlat1 = ((conv_mxt4x4_2(unity_ObjectToWorld) * in_v.in_POSITION0.zzzz) + u_xlat1);
          u_xlat1 = (u_xlat1 + conv_mxt4x4_3(unity_ObjectToWorld));
          u_xlat2 = (u_xlat1.yyyy * conv_mxt4x4_1(unity_MatrixVP));
          u_xlat2 = ((conv_mxt4x4_0(unity_MatrixVP) * u_xlat1.xxxx) + u_xlat2);
          u_xlat2 = ((conv_mxt4x4_2(unity_MatrixVP) * u_xlat1.zzzz) + u_xlat2);
          u_xlat1 = ((conv_mxt4x4_3(unity_MatrixVP) * u_xlat1.wwww) + u_xlat2);
          u_xlat6.xy = (u_xlat1.xy / u_xlat1.ww);
          u_xlat1.xy = (_ScreenParams.xy * float2(0.5, 0.5));
          u_xlat6.xy = (u_xlat6.xy * u_xlat1.xy);
          u_xlat6.xy = roundEven(u_xlat6.xy);
          u_xlat6.xy = (u_xlat6.xy / u_xlat1.xy);
          out_v.gl_Position.xy = (u_xlat1.ww * u_xlat6.xy);
          out_v.gl_Position.zw = u_xlat1.zw;
          out_v.vs_COLOR0 = (in_v.in_COLOR0 * _FaceColor);
          u_xlat6.x = (in_v.in_TEXCOORD1.x * 0.000244140625);
          u_xlat6.x = floor(u_xlat6.x);
          u_xlat6.y = (((-u_xlat6.x) * 4096) + in_v.in_TEXCOORD1.x);
          u_xlat6.xy = (u_xlat6.xy * _FaceTex_ST.xy);
          out_v.vs_TEXCOORD1.xy = ((u_xlat6.xy * float2(0.001953125, 0.001953125)) + _FaceTex_ST.zw);
          out_v.vs_TEXCOORD0.xy = in_v.in_TEXCOORD0.xy;
      }
      
      #define CODE_BLOCK_FRAGMENT
      float3 u_xlat16_0;
      float3 u_xlat10_0;
      OUT_Data_frag frag(IN_Data_Frag in_f)
      {
          OUT_Data_frag out_f;
          u_xlat10_0.xyz = tex2D(_FaceTex, in_f.vs_TEXCOORD1.xy).xyz;
          u_xlat16_0.xyz = (u_xlat10_0.xyz * in_f.vs_COLOR0.xyz);
          out_f.SV_Target0.xyz = u_xlat16_0.xyz;
          u_xlat10_0.x = tex2D(_MainTex, in_f.vs_TEXCOORD0.xy).w;
          out_f.SV_Target0.w = (u_xlat10_0.x * in_f.vs_COLOR0.w);
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  FallBack ""
}
