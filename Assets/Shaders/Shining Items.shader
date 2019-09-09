Shader "Custom/Shining Items"
{
  Properties
  {
    [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
    _Color ("Tint", Color) = (1,1,1,1)
    _ShininessColor ("Shininess Color", Color) = (1,1,1,1)
    _Width ("Width", float) = 0.1
    _TimeController ("Time", Range(0, 1)) = 0
    _Intensity ("Intensity", Range(0, 1)) = 0
    [MaterialToggle] PixelSnap ("Pixel snap", float) = 0
    [HideInInspector] _RendererColor ("RendererColor", Color) = (1,1,1,1)
    [HideInInspector] _Flip ("Flip", Vector) = (1,1,1,1)
    [PerRendererData] _AlphaTex ("External Alpha", 2D) = "white" {}
    [PerRendererData] _EnableExternalAlpha ("Enable External Alpha", float) = 0
  }
  SubShader
  {
    Tags
    { 
      "CanUseSpriteAtlas" = "true"
      "IGNOREPROJECTOR" = "true"
      "PreviewType" = "Plane"
      "QUEUE" = "Transparent"
      "RenderType" = "Transparent"
    }
    Pass // ind: 1, name: 
    {
      Tags
      { 
        "CanUseSpriteAtlas" = "true"
        "IGNOREPROJECTOR" = "true"
        "PreviewType" = "Plane"
        "QUEUE" = "Transparent"
        "RenderType" = "Transparent"
      }
      ZWrite Off
      Cull Off
      Blend One OneMinusSrcAlpha
      GpuProgramID 6066
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
      uniform float4 _RendererColor;
      uniform float4 _Color;
      uniform float4 _ShininessColor;
      uniform float _Width;
      uniform float _TimeController;
      uniform float _Intensity;
      uniform sampler2D _MainTex;
      struct IN_Data_Vert
      {
          float4 in_POSITION0 :POSITION0;
          float2 in_TEXCOORD0 :TEXCOORD0;
          float4 in_COLOR0 :COLOR0;
      };
      
      struct OUT_Data_Vert
      {
          float2 vs_TEXCOORD0 :TEXCOORD0;
          float4 vs_COLOR0 :COLOR0;
          float4 gl_Position :SV_POSITION;
      };
      
      struct IN_Data_Frag
      {
          float2 vs_TEXCOORD0 :TEXCOORD0;
          float4 vs_COLOR0 :COLOR0;
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
          u_xlat0 = (in_v.in_POSITION0.yyyy * conv_mxt4x4_1(unity_ObjectToWorld));
          u_xlat0 = ((conv_mxt4x4_0(unity_ObjectToWorld) * in_v.in_POSITION0.xxxx) + u_xlat0);
          u_xlat0 = ((conv_mxt4x4_2(unity_ObjectToWorld) * in_v.in_POSITION0.zzzz) + u_xlat0);
          u_xlat0 = (u_xlat0 + conv_mxt4x4_3(unity_ObjectToWorld));
          u_xlat1 = (u_xlat0.yyyy * conv_mxt4x4_1(unity_MatrixVP));
          u_xlat1 = ((conv_mxt4x4_0(unity_MatrixVP) * u_xlat0.xxxx) + u_xlat1);
          u_xlat1 = ((conv_mxt4x4_2(unity_MatrixVP) * u_xlat0.zzzz) + u_xlat1);
          out_v.gl_Position = ((conv_mxt4x4_3(unity_MatrixVP) * u_xlat0.wwww) + u_xlat1);
          out_v.vs_TEXCOORD0.xy = in_v.in_TEXCOORD0.xy;
          u_xlat0 = (in_v.in_COLOR0 * _Color);
          u_xlat0 = (u_xlat0 * _RendererColor);
          out_v.vs_COLOR0 = u_xlat0;
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      float4 u_xlat16_0;
      float4 u_xlat10_0;
      float4 u_xlat2;
      bool2 u_xlatb4;
      OUT_Data_frag frag(IN_Data_Frag in_f)
      {
          OUT_Data_frag out_f;
          u_xlat10_0 = tex2D(_MainTex, in_f.vs_TEXCOORD0.xy);
          u_xlat16_0 = (u_xlat10_0 * in_f.vs_COLOR0);
          u_xlat16_0.xyz = (u_xlat16_0.www * u_xlat16_0.xyz);
          u_xlat1.x = (in_f.vs_TEXCOORD0.y + in_f.vs_TEXCOORD0.x);
          u_xlat1.y = ((_TimeController * 2) + _Width);
          u_xlat1.z = ((_TimeController * 2) + (-_Width));
          //u_xlatb4.xy = lessThan(u_xlat1.xzxx, u_xlat1.yxyy).xy;
          //u_xlatb4.x = (u_xlatb4.y && u_xlatb4.x);
          if(u_xlatb4.x)
          {
              u_xlat1.x = ((_TimeController * 2) + (-u_xlat1.x));
              u_xlat1.x = (abs(u_xlat1.x) / _Width);
              u_xlat1.x = ((-u_xlat1.x) + 1);
              u_xlat1.x = (u_xlat1.x * _Intensity);
              u_xlat2 = ((_ShininessColor * u_xlat16_0.wwww) + (-u_xlat16_0));
              out_f.SV_Target0 = ((u_xlat1.xxxx * u_xlat2) + u_xlat16_0);
              //return;
          }
          out_f.SV_Target0 = u_xlat16_0;
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  FallBack "Diffuse"
}
