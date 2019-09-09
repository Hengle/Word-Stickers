Shader "TextMeshPro/Mobile/Bitmap"
{
  Properties
  {
    _MainTex ("Font Atlas", 2D) = "white" {}
    _Color ("Text Color", Color) = (1,1,1,1)
    _DiffusePower ("Diffuse Power", Range(1, 4)) = 1
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
      GpuProgramID 62068
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
      uniform float4 _Color;
      uniform float _DiffusePower;
      uniform float _VertexOffsetX;
      uniform float _VertexOffsetY;
      uniform float4 _ClipRect;
      uniform float _MaskSoftnessX;
      uniform float _MaskSoftnessY;
      uniform sampler2D _MainTex;
      struct IN_Data_Vert
      {
          float4 in_POSITION0 :POSITION0;
          float4 in_COLOR0 :COLOR0;
          float2 in_TEXCOORD0 :TEXCOORD0;
      };
      
      struct OUT_Data_Vert
      {
          float4 vs_COLOR0 :COLOR0;
          float2 vs_TEXCOORD0 :TEXCOORD0;
          float4 vs_TEXCOORD2 :TEXCOORD2;
          float4 gl_Position :SV_POSITION;
      };
      
      struct IN_Data_Frag
      {
          float4 vs_COLOR0 :COLOR0;
          float2 vs_TEXCOORD0 :TEXCOORD0;
      };
      
      struct OUT_Data_frag
      {
          float4 SV_Target0 :SV_Target0;
      };
      
      float2 u_xlat0;
      float4 u_xlat1;
      float4 u_xlat16_1;
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
          u_xlat6.xy = ((float2(_MaskSoftnessX, _MaskSoftnessY) * float2(0.25, 0.25)) + u_xlat1.ww);
          out_v.vs_TEXCOORD2.zw = (float2(0.25, 0.25) / u_xlat6.xy);
          u_xlat16_1 = (in_v.in_COLOR0 * _Color);
          u_xlat2.xyz = (u_xlat16_1.xyz * float3(_DiffusePower, _DiffusePower, _DiffusePower));
          out_v.vs_COLOR0.w = u_xlat16_1.w;
          out_v.vs_COLOR0.xyz = u_xlat2.xyz;
          out_v.vs_TEXCOORD0.xy = in_v.in_TEXCOORD0.xy;
      }
      
      #define CODE_BLOCK_FRAGMENT
      float u_xlat16_0;
      float u_xlat10_0;
      OUT_Data_frag frag(IN_Data_Frag in_f)
      {
          OUT_Data_frag out_f;
          u_xlat10_0 = tex2D(_MainTex, in_f.vs_TEXCOORD0.xy).w;
          u_xlat16_0 = (u_xlat10_0 * in_f.vs_COLOR0.w);
          out_f.SV_Target0.w = u_xlat16_0;
          out_f.SV_Target0.xyz = in_f.vs_COLOR0.xyz;
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
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
      ZTest Always
      ZWrite Off
      Cull Off
      Fog
      { 
        Mode  Off
      } 
      Blend SrcAlpha OneMinusSrcAlpha
      GpuProgramID 117148
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
      uniform float4 _MainTex_ST;
      uniform float4 _Color;
      uniform sampler2D _MainTex;
      struct IN_Data_Vert
      {
          float3 in_POSITION0 :POSITION0;
          float4 in_COLOR0 :COLOR0;
          float3 in_TEXCOORD0 :TEXCOORD0;
      };
      
      struct OUT_Data_Vert
      {
          float4 vs_COLOR0 :COLOR0;
          float2 vs_TEXCOORD0 :TEXCOORD0;
          float4 gl_Position :SV_POSITION;
      };
      
      struct IN_Data_Frag
      {
          float4 vs_COLOR0 :COLOR0;
          float2 vs_TEXCOORD0 :TEXCOORD0;
      };
      
      struct OUT_Data_frag
      {
          float4 SV_Target0 :SV_Target0;
      };
      
      float4 u_xlat0;
      float4 u_xlat1;
      OUT_Data_Vert vert(IN_Data_Vert in_v)
      {
          OUT_Data_Vert out_v;
          out_v.vs_COLOR0 = in_v.in_COLOR0;
          out_v.vs_COLOR0 = clamp(out_v.vs_COLOR0, 0, 1);
          out_v.vs_TEXCOORD0.xy = ((in_v.in_TEXCOORD0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
          u_xlat0 = (in_v.in_POSITION0.yyyy * conv_mxt4x4_1(unity_ObjectToWorld));
          u_xlat0 = ((conv_mxt4x4_0(unity_ObjectToWorld) * in_v.in_POSITION0.xxxx) + u_xlat0);
          u_xlat0 = ((conv_mxt4x4_2(unity_ObjectToWorld) * in_v.in_POSITION0.zzzz) + u_xlat0);
          u_xlat0 = (u_xlat0 + conv_mxt4x4_3(unity_ObjectToWorld));
          u_xlat1 = (u_xlat0.yyyy * conv_mxt4x4_1(unity_MatrixVP));
          u_xlat1 = ((conv_mxt4x4_0(unity_MatrixVP) * u_xlat0.xxxx) + u_xlat1);
          u_xlat1 = ((conv_mxt4x4_2(unity_MatrixVP) * u_xlat0.zzzz) + u_xlat1);
          out_v.gl_Position = ((conv_mxt4x4_3(unity_MatrixVP) * u_xlat0.wwww) + u_xlat1);
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      float u_xlat10_0;
      OUT_Data_frag frag(IN_Data_Frag in_f)
      {
          OUT_Data_frag out_f;
          u_xlat10_0 = tex2D(_MainTex, in_f.vs_TEXCOORD0.xy).w;
          out_f.SV_Target0.w = (u_xlat10_0 * _Color.w);
          out_f.SV_Target0.xyz = (in_f.vs_COLOR0.xyz * _Color.xyz);
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  FallBack ""
}
