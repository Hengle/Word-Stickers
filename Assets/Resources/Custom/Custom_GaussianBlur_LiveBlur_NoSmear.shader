Shader "Custom/GaussianBlur_LiveBlur_NoSmear"
{
  Properties
  {
    [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
    _Color ("Main Color", Color) = (1,1,1,1)
    _BlurSize ("BlurSize", Range(0, 250)) = 25
    _Lightness ("Lightness", Range(-1, 1)) = 0
    _Quality ("Quality", Range(0, 3)) = 3
    [Space] [Toggle] _WorldSpace ("WorldSpace", float) = 0
    _ScreenWidth ("ScreenWidth", float) = 0
    _ScreenHeight ("ScreenHeight", float) = 0
    _PanelWidth ("PanelWidth", float) = 0
    _PanelHeight ("PanelHeight", float) = 0
    _PanelX ("PanelX", float) = 0
    _PanelY ("PanelY", float) = 0
  }
  SubShader
  {
    Tags
    { 
      "CanUseSpriteAtlas" = "true"
      "PreviewType" = "Plane"
      "QUEUE" = "Transparent"
      "RenderType" = "Transparent"
    }
    Pass // ind: 1, name: 
    {
      Tags
      { 
      }
      ZClip Off
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
      GpuProgramID -1
      // m_ProgramMask = 0
      
    } // end phase
    Pass // ind: 2, name: 
    {
      Tags
      { 
        "CanUseSpriteAtlas" = "true"
        "LIGHTMODE" = "ALWAYS"
        "PreviewType" = "Plane"
        "QUEUE" = "Transparent"
        "RenderType" = "Transparent"
      }
      GpuProgramID 63858
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
      uniform float4 _GrabTexture_TexelSize;
      uniform float _BlurSize;
      uniform float _Lightness;
      uniform float4 _Color;
      uniform int _Quality;
      uniform float _ScreenWidth;
      uniform float _PanelX;
      uniform float _PanelWidth;
      uniform float _WorldSpace;
      uniform sampler2D _MainTex;
      uniform sampler2D _GrabTexture;
      struct IN_Data_Vert
      {
          float4 in_POSITION0 :POSITION0;
          float2 in_TEXCOORD0 :TEXCOORD0;
      };
      
      struct OUT_Data_Vert
      {
          float4 vs_TEXCOORD0 :TEXCOORD0;
          float2 vs_TEXCOORD1 :TEXCOORD1;
          float4 gl_Position :SV_POSITION;
      };
      
      struct IN_Data_Frag
      {
          float4 vs_TEXCOORD0 :TEXCOORD0;
          float2 vs_TEXCOORD1 :TEXCOORD1;
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
          u_xlat0 = ((conv_mxt4x4_3(unity_MatrixVP) * u_xlat0.wwww) + u_xlat1);
          out_v.gl_Position = u_xlat0;
          u_xlat0.xy = (u_xlat0.ww + u_xlat0.xy);
          out_v.vs_TEXCOORD0.zw = u_xlat0.zw;
          out_v.vs_TEXCOORD0.xy = (u_xlat0.xy * float2(0.5, 0.5));
          out_v.vs_TEXCOORD1.xy = ((in_v.in_TEXCOORD0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      float u_xlat10_0;
      int u_xlatb0;
      float4 u_xlat16_1;
      float4 u_xlat10_2;
      float4 u_xlat3;
      float4 u_xlat16_3;
      float4 u_xlat10_3;
      float2 u_xlat4;
      bool4 u_xlatb4;
      float4 u_xlat16_5;
      bool4 u_xlatb5;
      float2 u_xlat6;
      float4 u_xlat16_6;
      float4 u_xlat10_6;
      bool4 u_xlatb6;
      float2 u_xlat7;
      float4 u_xlat16_7;
      float4 u_xlat10_7;
      bool4 u_xlatb7;
      float2 u_xlat8;
      int u_xlatb8;
      float2 u_xlat9;
      float2 u_xlat10;
      int u_xlatb10;
      float u_xlat16_11;
      float2 u_xlat18;
      int u_xlatb18;
      float2 u_xlat20;
      int u_xlatb20;
      float u_xlat16_21;
      int u_xlatb28;
      int u_xlatb30;
      float u_xlat16_31;
      int u_xlatb38;
      OUT_Data_frag frag(IN_Data_Frag in_f)
      {
          OUT_Data_frag out_f;
          if((_WorldSpace==1))
          {
              u_xlatb0 = 1;
          }
          else u_xlatb0 = 0;
          u_xlat16_1.xy = int(u_xlatb0);
          u_xlat16_21 = u_xlatb0;
          u_xlat10_0 = tex2D(_MainTex, in_f.vs_TEXCOORD1.xy).w;
          u_xlat10.xy = (in_f.vs_TEXCOORD0.xy / in_f.vs_TEXCOORD0.ww);
          u_xlat10_2 = tex2D(_GrabTexture, u_xlat10.xy);
          u_xlat16_31 = ((u_xlat16_21 * 0.5) + u_xlat16_1.y);
          u_xlat16_31 = (u_xlat16_31 / u_xlat16_1.x);
          u_xlat16_11 = (((-u_xlat16_21) * 0.5) + u_xlat16_1.y);
          u_xlat16_1.x = (u_xlat16_11 / u_xlat16_1.x);
          if((_Quality==0))
          {
              u_xlat10.x = (_GrabTexture_TexelSize.x * _BlurSize);
              u_xlat3 = ((u_xlat10.xxxx * float4(-3, (-4), (-2), 3)) + in_f.vs_TEXCOORD0.xxxx);
              u_xlatb4 = lessThan(float4(u_xlat16_31, u_xlat16_31, u_xlat16_31, u_xlat16_31), u_xlat3.yxzw);
              u_xlatb5 = lessThan(u_xlat3.yxzw, u_xlat16_1.xxxx);
              u_xlatb4.x = (u_xlatb4.x || u_xlatb5.x);
              u_xlatb4.y = (u_xlatb4.y || u_xlatb5.y);
              u_xlatb4.z = (u_xlatb4.z || u_xlatb5.z);
              u_xlatb4.w = (u_xlatb4.w || u_xlatb5.w);
              if(u_xlatb4.x)
              {
                  u_xlat16_5.x = float(0);
                  u_xlat16_5.y = float(0);
                  u_xlat16_5.z = float(0);
                  u_xlat16_5.w = float(0);
                  u_xlat16_11 = 0;
              }
              else
              {
                  u_xlat6.x = u_xlat3.y;
                  u_xlat6.y = in_f.vs_TEXCOORD0.y;
                  u_xlat20.xy = (u_xlat6.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_6 = tex2D(_GrabTexture, u_xlat20.xy);
                  u_xlat16_6 = (u_xlat10_6 * float4(0.0500000007, 0.0500000007, 0.0500000007, 0.0500000007));
                  u_xlat16_5 = u_xlat16_6;
                  u_xlat16_11 = 0.0500000007;
              }
              if(!u_xlatb4.y)
              {
                  u_xlat3.y = in_f.vs_TEXCOORD0.y;
                  u_xlat20.xy = (u_xlat3.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_6 = tex2D(_GrabTexture, u_xlat20.xy);
                  u_xlat16_6 = ((u_xlat10_6 * float4(0.0900000036, 0.0900000036, 0.0900000036, 0.0900000036)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.0900000036);
                  u_xlat16_5 = u_xlat16_6;
              }
              if(!u_xlatb4.z)
              {
                  u_xlat3.y = in_f.vs_TEXCOORD0.y;
                  u_xlat20.xy = (u_xlat3.zy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_6 = tex2D(_GrabTexture, u_xlat20.xy);
                  u_xlat16_6 = ((u_xlat10_6 * float4(0.129999995, 0.129999995, 0.129999995, 0.129999995)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.129999995);
                  u_xlat16_5 = u_xlat16_6;
              }
              u_xlat4.x = (((-_GrabTexture_TexelSize.x) * _BlurSize) + in_f.vs_TEXCOORD0.x);
              if((u_xlat16_31<u_xlat4.x))
              {
                  u_xlatb20 = 1;
              }
              else u_xlatb20 = 0;
              if((u_xlat4.x<u_xlat16_1.x))
              {
                  u_xlatb30 = 1;
              }
              else u_xlatb30 = 0;
              u_xlatb20 = (u_xlatb30 || u_xlatb20);
              if(!u_xlatb20)
              {
                  u_xlat4.y = in_f.vs_TEXCOORD0.y;
                  u_xlat20.xy = (u_xlat4.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_6 = tex2D(_GrabTexture, u_xlat20.xy);
                  u_xlat16_6 = ((u_xlat10_6 * float4(0.169999987, 0.169999987, 0.169999987, 0.169999987)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.169999987);
                  u_xlat16_5 = u_xlat16_6;
              }
              if((u_xlat16_31<in_f.vs_TEXCOORD0.x))
              {
                  u_xlatb20 = 1;
              }
              else u_xlatb20 = 0;
              if((in_f.vs_TEXCOORD0.x<u_xlat16_1.x))
              {
                  u_xlatb30 = 1;
              }
              else u_xlatb30 = 0;
              u_xlatb20 = (u_xlatb30 || u_xlatb20);
              if(!u_xlatb20)
              {
                  u_xlat16_6 = ((u_xlat10_2 * float4(0.209999979, 0.209999979, 0.209999979, 0.209999979)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.209999979);
                  u_xlat16_5 = u_xlat16_6;
              }
              u_xlat4.x = ((_GrabTexture_TexelSize.x * _BlurSize) + in_f.vs_TEXCOORD0.x);
              if((u_xlat16_31<u_xlat4.x))
              {
                  u_xlatb20 = 1;
              }
              else u_xlatb20 = 0;
              if((u_xlat4.x<u_xlat16_1.x))
              {
                  u_xlatb30 = 1;
              }
              else u_xlatb30 = 0;
              u_xlatb20 = (u_xlatb30 || u_xlatb20);
              if(!u_xlatb20)
              {
                  u_xlat4.y = in_f.vs_TEXCOORD0.y;
                  u_xlat20.xy = (u_xlat4.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_6 = tex2D(_GrabTexture, u_xlat20.xy);
                  u_xlat16_6 = ((u_xlat10_6 * float4(0.169999987, 0.169999987, 0.169999987, 0.169999987)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.169999987);
                  u_xlat16_5 = u_xlat16_6;
              }
              u_xlat4.x = ((u_xlat10.x * 2) + in_f.vs_TEXCOORD0.x);
              if((u_xlat16_31<u_xlat4.x))
              {
                  u_xlatb20 = 1;
              }
              else u_xlatb20 = 0;
              if((u_xlat4.x<u_xlat16_1.x))
              {
                  u_xlatb30 = 1;
              }
              else u_xlatb30 = 0;
              u_xlatb20 = (u_xlatb30 || u_xlatb20);
              if(!u_xlatb20)
              {
                  u_xlat4.y = in_f.vs_TEXCOORD0.y;
                  u_xlat20.xy = (u_xlat4.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_6 = tex2D(_GrabTexture, u_xlat20.xy);
                  u_xlat16_6 = ((u_xlat10_6 * float4(0.129999995, 0.129999995, 0.129999995, 0.129999995)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.129999995);
                  u_xlat16_5 = u_xlat16_6;
              }
              if(!u_xlatb4.w)
              {
                  u_xlat3.y = in_f.vs_TEXCOORD0.y;
                  u_xlat20.xy = (u_xlat3.wy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_3 = tex2D(_GrabTexture, u_xlat20.xy);
                  u_xlat16_3 = ((u_xlat10_3 * float4(0.0899999961, 0.0899999961, 0.0899999961, 0.0899999961)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.0899999961);
                  u_xlat16_5 = u_xlat16_3;
              }
              u_xlat3.x = ((u_xlat10.x * 4) + in_f.vs_TEXCOORD0.x);
              if((u_xlat16_31<u_xlat3.x))
              {
                  u_xlatb10 = 1;
              }
              else u_xlatb10 = 0;
              if((u_xlat3.x<u_xlat16_1.x))
              {
                  u_xlatb20 = 1;
              }
              else u_xlatb20 = 0;
              u_xlatb10 = (u_xlatb20 || u_xlatb10);
              if(!u_xlatb10)
              {
                  u_xlat3.y = in_f.vs_TEXCOORD0.y;
                  u_xlat10.xy = (u_xlat3.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_3 = tex2D(_GrabTexture, u_xlat10.xy);
                  u_xlat16_3 = ((u_xlat10_3 * float4(0.049999997, 0.049999997, 0.049999997, 0.049999997)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.049999997);
                  u_xlat16_5 = u_xlat16_3;
              }
          }
          if((_Quality==1))
          {
              u_xlat10.x = (_GrabTexture_TexelSize.x * _BlurSize);
              u_xlat3 = ((u_xlat10.xxxx * float4(-3.5, (-4), (-3), (-2.5))) + in_f.vs_TEXCOORD0.xxxx);
              u_xlatb4 = lessThan(float4(u_xlat16_31, u_xlat16_31, u_xlat16_31, u_xlat16_31), u_xlat3.yxzw);
              u_xlatb6 = lessThan(u_xlat3.yxzw, u_xlat16_1.xxxx);
              u_xlatb4.x = (u_xlatb4.x || u_xlatb6.x);
              u_xlatb4.y = (u_xlatb4.y || u_xlatb6.y);
              u_xlatb4.z = (u_xlatb4.z || u_xlatb6.z);
              u_xlatb4.w = (u_xlatb4.w || u_xlatb6.w);
              if(u_xlatb4.x)
              {
                  u_xlat16_5.x = float(0);
                  u_xlat16_5.y = float(0);
                  u_xlat16_5.z = float(0);
                  u_xlat16_5.w = float(0);
                  u_xlat16_11 = 0;
              }
              else
              {
                  u_xlat6.x = u_xlat3.y;
                  u_xlat6.y = in_f.vs_TEXCOORD0.y;
                  u_xlat20.xy = (u_xlat6.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_6 = tex2D(_GrabTexture, u_xlat20.xy);
                  u_xlat16_6 = (u_xlat10_6 * float4(0.0500000007, 0.0500000007, 0.0500000007, 0.0500000007));
                  u_xlat16_5 = u_xlat16_6;
                  u_xlat16_11 = 0.0500000007;
              }
              if(!u_xlatb4.y)
              {
                  u_xlat3.y = in_f.vs_TEXCOORD0.y;
                  u_xlat20.xy = (u_xlat3.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_6 = tex2D(_GrabTexture, u_xlat20.xy);
                  u_xlat16_6 = ((u_xlat10_6 * float4(0.0700000003, 0.0700000003, 0.0700000003, 0.0700000003)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.0700000003);
                  u_xlat16_5 = u_xlat16_6;
              }
              if(!u_xlatb4.z)
              {
                  u_xlat3.y = in_f.vs_TEXCOORD0.y;
                  u_xlat20.xy = (u_xlat3.zy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_6 = tex2D(_GrabTexture, u_xlat20.xy);
                  u_xlat16_6 = ((u_xlat10_6 * float4(0.0900000036, 0.0900000036, 0.0900000036, 0.0900000036)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.0900000036);
                  u_xlat16_5 = u_xlat16_6;
              }
              if(!u_xlatb4.w)
              {
                  u_xlat3.y = in_f.vs_TEXCOORD0.y;
                  u_xlat20.xy = (u_xlat3.wy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_3 = tex2D(_GrabTexture, u_xlat20.xy);
                  u_xlat16_3 = ((u_xlat10_3 * float4(0.109999999, 0.109999999, 0.109999999, 0.109999999)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.109999999);
                  u_xlat16_5 = u_xlat16_3;
              }
              u_xlat3 = ((u_xlat10.xxxx * float4(-1.5, (-2), (-0.5), 0.5)) + in_f.vs_TEXCOORD0.xxxx);
              u_xlatb4 = lessThan(float4(u_xlat16_31, u_xlat16_31, u_xlat16_31, u_xlat16_31), u_xlat3.yxzw);
              u_xlatb6 = lessThan(u_xlat3.yxzw, u_xlat16_1.xxxx);
              u_xlatb4.x = (u_xlatb4.x || u_xlatb6.x);
              u_xlatb4.y = (u_xlatb4.y || u_xlatb6.y);
              u_xlatb4.z = (u_xlatb4.z || u_xlatb6.z);
              u_xlatb4.w = (u_xlatb4.w || u_xlatb6.w);
              if(!u_xlatb4.x)
              {
                  u_xlat6.x = u_xlat3.y;
                  u_xlat6.y = in_f.vs_TEXCOORD0.y;
                  u_xlat20.xy = (u_xlat6.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_6 = tex2D(_GrabTexture, u_xlat20.xy);
                  u_xlat16_6 = ((u_xlat10_6 * float4(0.129999995, 0.129999995, 0.129999995, 0.129999995)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.129999995);
                  u_xlat16_5 = u_xlat16_6;
              }
              if(!u_xlatb4.y)
              {
                  u_xlat3.y = in_f.vs_TEXCOORD0.y;
                  u_xlat20.xy = (u_xlat3.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_6 = tex2D(_GrabTexture, u_xlat20.xy);
                  u_xlat16_6 = ((u_xlat10_6 * float4(0.149999991, 0.149999991, 0.149999991, 0.149999991)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.149999991);
                  u_xlat16_5 = u_xlat16_6;
              }
              u_xlat4.x = (((-_GrabTexture_TexelSize.x) * _BlurSize) + in_f.vs_TEXCOORD0.x);
              if((u_xlat16_31<u_xlat4.x))
              {
                  u_xlatb20 = 1;
              }
              else u_xlatb20 = 0;
              if((u_xlat4.x<u_xlat16_1.x))
              {
                  u_xlatb30 = 1;
              }
              else u_xlatb30 = 0;
              u_xlatb20 = (u_xlatb30 || u_xlatb20);
              if(!u_xlatb20)
              {
                  u_xlat4.y = in_f.vs_TEXCOORD0.y;
                  u_xlat20.xy = (u_xlat4.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_6 = tex2D(_GrabTexture, u_xlat20.xy);
                  u_xlat16_6 = ((u_xlat10_6 * float4(0.169999987, 0.169999987, 0.169999987, 0.169999987)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.169999987);
                  u_xlat16_5 = u_xlat16_6;
              }
              if(!u_xlatb4.z)
              {
                  u_xlat3.y = in_f.vs_TEXCOORD0.y;
                  u_xlat20.xy = (u_xlat3.zy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_6 = tex2D(_GrabTexture, u_xlat20.xy);
                  u_xlat16_6 = ((u_xlat10_6 * float4(0.189999983, 0.189999983, 0.189999983, 0.189999983)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.189999983);
                  u_xlat16_5 = u_xlat16_6;
              }
              if((u_xlat16_31<in_f.vs_TEXCOORD0.x))
              {
                  u_xlatb20 = 1;
              }
              else u_xlatb20 = 0;
              if((in_f.vs_TEXCOORD0.x<u_xlat16_1.x))
              {
                  u_xlatb30 = 1;
              }
              else u_xlatb30 = 0;
              u_xlatb20 = (u_xlatb30 || u_xlatb20);
              if(!u_xlatb20)
              {
                  u_xlat16_6 = ((u_xlat10_2 * float4(0.209999979, 0.209999979, 0.209999979, 0.209999979)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.209999979);
                  u_xlat16_5 = u_xlat16_6;
              }
              if(!u_xlatb4.w)
              {
                  u_xlat3.y = in_f.vs_TEXCOORD0.y;
                  u_xlat20.xy = (u_xlat3.wy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_3 = tex2D(_GrabTexture, u_xlat20.xy);
                  u_xlat16_3 = ((u_xlat10_3 * float4(0.189999983, 0.189999983, 0.189999983, 0.189999983)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.189999983);
                  u_xlat16_5 = u_xlat16_3;
              }
              u_xlat3.x = ((_GrabTexture_TexelSize.x * _BlurSize) + in_f.vs_TEXCOORD0.x);
              if((u_xlat16_31<u_xlat3.x))
              {
                  u_xlatb20 = 1;
              }
              else u_xlatb20 = 0;
              if((u_xlat3.x<u_xlat16_1.x))
              {
                  u_xlatb30 = 1;
              }
              else u_xlatb30 = 0;
              u_xlatb20 = (u_xlatb30 || u_xlatb20);
              if(!u_xlatb20)
              {
                  u_xlat3.y = in_f.vs_TEXCOORD0.y;
                  u_xlat20.xy = (u_xlat3.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_3 = tex2D(_GrabTexture, u_xlat20.xy);
                  u_xlat16_3 = ((u_xlat10_3 * float4(0.169999987, 0.169999987, 0.169999987, 0.169999987)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.169999987);
                  u_xlat16_5 = u_xlat16_3;
              }
              u_xlat3 = ((u_xlat10.xxxx * float4(2.5, 1.5, 3, 3.5)) + in_f.vs_TEXCOORD0.xxxx);
              u_xlatb4 = lessThan(float4(u_xlat16_31, u_xlat16_31, u_xlat16_31, u_xlat16_31), u_xlat3.yxzw);
              u_xlatb6 = lessThan(u_xlat3.yxzw, u_xlat16_1.xxxx);
              u_xlatb4.x = (u_xlatb4.x || u_xlatb6.x);
              u_xlatb4.y = (u_xlatb4.y || u_xlatb6.y);
              u_xlatb4.z = (u_xlatb4.z || u_xlatb6.z);
              u_xlatb4.w = (u_xlatb4.w || u_xlatb6.w);
              if(!u_xlatb4.x)
              {
                  u_xlat6.x = u_xlat3.y;
                  u_xlat6.y = in_f.vs_TEXCOORD0.y;
                  u_xlat20.xy = (u_xlat6.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_6 = tex2D(_GrabTexture, u_xlat20.xy);
                  u_xlat16_6 = ((u_xlat10_6 * float4(0.149999991, 0.149999991, 0.149999991, 0.149999991)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.149999991);
                  u_xlat16_5 = u_xlat16_6;
              }
              u_xlat6.x = ((u_xlat10.x * 2) + in_f.vs_TEXCOORD0.x);
              if((u_xlat16_31<u_xlat6.x))
              {
                  u_xlatb20 = 1;
              }
              else u_xlatb20 = 0;
              if((u_xlat6.x<u_xlat16_1.x))
              {
                  u_xlatb30 = 1;
              }
              else u_xlatb30 = 0;
              u_xlatb20 = (u_xlatb30 || u_xlatb20);
              if(!u_xlatb20)
              {
                  u_xlat6.y = in_f.vs_TEXCOORD0.y;
                  u_xlat20.xy = (u_xlat6.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_6 = tex2D(_GrabTexture, u_xlat20.xy);
                  u_xlat16_6 = ((u_xlat10_6 * float4(0.129999995, 0.129999995, 0.129999995, 0.129999995)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.129999995);
                  u_xlat16_5 = u_xlat16_6;
              }
              if(!u_xlatb4.y)
              {
                  u_xlat3.y = in_f.vs_TEXCOORD0.y;
                  u_xlat20.xy = (u_xlat3.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_6 = tex2D(_GrabTexture, u_xlat20.xy);
                  u_xlat16_6 = ((u_xlat10_6 * float4(0.109999999, 0.109999999, 0.109999999, 0.109999999)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.109999999);
                  u_xlat16_5 = u_xlat16_6;
              }
              if(!u_xlatb4.z)
              {
                  u_xlat3.y = in_f.vs_TEXCOORD0.y;
                  u_xlat20.xy = (u_xlat3.zy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_6 = tex2D(_GrabTexture, u_xlat20.xy);
                  u_xlat16_6 = ((u_xlat10_6 * float4(0.0900000036, 0.0900000036, 0.0900000036, 0.0900000036)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.0900000036);
                  u_xlat16_5 = u_xlat16_6;
              }
              if(!u_xlatb4.w)
              {
                  u_xlat3.y = in_f.vs_TEXCOORD0.y;
                  u_xlat20.xy = (u_xlat3.wy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_3 = tex2D(_GrabTexture, u_xlat20.xy);
                  u_xlat16_3 = ((u_xlat10_3 * float4(0.0700000077, 0.0700000077, 0.0700000077, 0.0700000077)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.0700000077);
                  u_xlat16_5 = u_xlat16_3;
              }
              u_xlat3.x = ((u_xlat10.x * 4) + in_f.vs_TEXCOORD0.x);
              if((u_xlat16_31<u_xlat3.x))
              {
                  u_xlatb10 = 1;
              }
              else u_xlatb10 = 0;
              if((u_xlat3.x<u_xlat16_1.x))
              {
                  u_xlatb20 = 1;
              }
              else u_xlatb20 = 0;
              u_xlatb10 = (u_xlatb20 || u_xlatb10);
              if(!u_xlatb10)
              {
                  u_xlat3.y = in_f.vs_TEXCOORD0.y;
                  u_xlat4.xy = (u_xlat3.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_3 = tex2D(_GrabTexture, u_xlat4.xy);
                  u_xlat16_3 = ((u_xlat10_3 * float4(0.0500000082, 0.0500000082, 0.0500000082, 0.0500000082)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.0500000082);
                  u_xlat16_5 = u_xlat16_3;
              }
          }
          if((_Quality==2))
          {
              u_xlat4.x = (_GrabTexture_TexelSize.x * _BlurSize);
              u_xlat3 = ((u_xlat4.xxxx * float4(-3.75, (-4), (-3.5), (-3.25))) + in_f.vs_TEXCOORD0.xxxx);
              u_xlatb6 = lessThan(float4(u_xlat16_31, u_xlat16_31, u_xlat16_31, u_xlat16_31), u_xlat3.yxzw);
              u_xlatb7 = lessThan(u_xlat3.yxzw, u_xlat16_1.xxxx);
              u_xlatb6.x = (u_xlatb6.x || u_xlatb7.x);
              u_xlatb6.y = (u_xlatb6.y || u_xlatb7.y);
              u_xlatb6.z = (u_xlatb6.z || u_xlatb7.z);
              u_xlatb6.w = (u_xlatb6.w || u_xlatb7.w);
              if(u_xlatb6.x)
              {
                  u_xlat16_5.x = float(0);
                  u_xlat16_5.y = float(0);
                  u_xlat16_5.z = float(0);
                  u_xlat16_5.w = float(0);
                  u_xlat16_11 = 0;
              }
              else
              {
                  u_xlat7.x = u_xlat3.y;
                  u_xlat7.y = in_f.vs_TEXCOORD0.y;
                  u_xlat7.xy = (u_xlat7.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_7 = tex2D(_GrabTexture, u_xlat7.xy);
                  u_xlat16_7 = (u_xlat10_7 * float4(0.0500000007, 0.0500000007, 0.0500000007, 0.0500000007));
                  u_xlat16_5 = u_xlat16_7;
                  u_xlat16_11 = 0.0500000007;
              }
              if(!u_xlatb6.y)
              {
                  u_xlat3.y = in_f.vs_TEXCOORD0.y;
                  u_xlat8.xy = (u_xlat3.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_7 = tex2D(_GrabTexture, u_xlat8.xy);
                  u_xlat16_7 = ((u_xlat10_7 * float4(0.0600000024, 0.0600000024, 0.0600000024, 0.0600000024)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.0600000024);
                  u_xlat16_5 = u_xlat16_7;
              }
              if(!u_xlatb6.z)
              {
                  u_xlat3.y = in_f.vs_TEXCOORD0.y;
                  u_xlat8.xy = (u_xlat3.zy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_7 = tex2D(_GrabTexture, u_xlat8.xy);
                  u_xlat16_7 = ((u_xlat10_7 * float4(0.0700000003, 0.0700000003, 0.0700000003, 0.0700000003)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.0700000003);
                  u_xlat16_5 = u_xlat16_7;
              }
              if(!u_xlatb6.w)
              {
                  u_xlat3.y = in_f.vs_TEXCOORD0.y;
                  u_xlat8.xy = (u_xlat3.wy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_3 = tex2D(_GrabTexture, u_xlat8.xy);
                  u_xlat16_3 = ((u_xlat10_3 * float4(0.0799999982, 0.0799999982, 0.0799999982, 0.0799999982)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.0799999982);
                  u_xlat16_5 = u_xlat16_3;
              }
              u_xlat3 = ((u_xlat4.xxxx * float4(-2.75, (-3), (-2.5), (-2.25))) + in_f.vs_TEXCOORD0.xxxx);
              u_xlatb6 = lessThan(float4(u_xlat16_31, u_xlat16_31, u_xlat16_31, u_xlat16_31), u_xlat3.yxzw);
              u_xlatb7 = lessThan(u_xlat3.yxzw, u_xlat16_1.xxxx);
              u_xlatb6.x = (u_xlatb6.x || u_xlatb7.x);
              u_xlatb6.y = (u_xlatb6.y || u_xlatb7.y);
              u_xlatb6.z = (u_xlatb6.z || u_xlatb7.z);
              u_xlatb6.w = (u_xlatb6.w || u_xlatb7.w);
              if(!u_xlatb6.x)
              {
                  u_xlat8.x = u_xlat3.y;
                  u_xlat8.y = in_f.vs_TEXCOORD0.y;
                  u_xlat8.xy = (u_xlat8.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_7 = tex2D(_GrabTexture, u_xlat8.xy);
                  u_xlat16_7 = ((u_xlat10_7 * float4(0.0899999961, 0.0899999961, 0.0899999961, 0.0899999961)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.0899999961);
                  u_xlat16_5 = u_xlat16_7;
              }
              if(!u_xlatb6.y)
              {
                  u_xlat3.y = in_f.vs_TEXCOORD0.y;
                  u_xlat8.xy = (u_xlat3.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_7 = tex2D(_GrabTexture, u_xlat8.xy);
                  u_xlat16_7 = ((u_xlat10_7 * float4(0.099999994, 0.099999994, 0.099999994, 0.099999994)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.099999994);
                  u_xlat16_5 = u_xlat16_7;
              }
              if(!u_xlatb6.z)
              {
                  u_xlat3.y = in_f.vs_TEXCOORD0.y;
                  u_xlat8.xy = (u_xlat3.zy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_7 = tex2D(_GrabTexture, u_xlat8.xy);
                  u_xlat16_7 = ((u_xlat10_7 * float4(0.109999992, 0.109999992, 0.109999992, 0.109999992)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.109999992);
                  u_xlat16_5 = u_xlat16_7;
              }
              if(!u_xlatb6.w)
              {
                  u_xlat3.y = in_f.vs_TEXCOORD0.y;
                  u_xlat8.xy = (u_xlat3.wy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_3 = tex2D(_GrabTexture, u_xlat8.xy);
                  u_xlat16_3 = ((u_xlat10_3 * float4(0.11999999, 0.11999999, 0.11999999, 0.11999999)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.11999999);
                  u_xlat16_5 = u_xlat16_3;
              }
              u_xlat3 = ((u_xlat4.xxxx * float4(-1.75, (-2), (-1.5), (-1.25))) + in_f.vs_TEXCOORD0.xxxx);
              u_xlatb6 = lessThan(float4(u_xlat16_31, u_xlat16_31, u_xlat16_31, u_xlat16_31), u_xlat3.yxzw);
              u_xlatb7 = lessThan(u_xlat3.yxzw, u_xlat16_1.xxxx);
              u_xlatb6.x = (u_xlatb6.x || u_xlatb7.x);
              u_xlatb6.y = (u_xlatb6.y || u_xlatb7.y);
              u_xlatb6.z = (u_xlatb6.z || u_xlatb7.z);
              u_xlatb6.w = (u_xlatb6.w || u_xlatb7.w);
              if(!u_xlatb6.x)
              {
                  u_xlat8.x = u_xlat3.y;
                  u_xlat8.y = in_f.vs_TEXCOORD0.y;
                  u_xlat8.xy = (u_xlat8.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_7 = tex2D(_GrabTexture, u_xlat8.xy);
                  u_xlat16_7 = ((u_xlat10_7 * float4(0.129999995, 0.129999995, 0.129999995, 0.129999995)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.129999995);
                  u_xlat16_5 = u_xlat16_7;
              }
              if(!u_xlatb6.y)
              {
                  u_xlat3.y = in_f.vs_TEXCOORD0.y;
                  u_xlat8.xy = (u_xlat3.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_7 = tex2D(_GrabTexture, u_xlat8.xy);
                  u_xlat16_7 = ((u_xlat10_7 * float4(0.140000001, 0.140000001, 0.140000001, 0.140000001)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.140000001);
                  u_xlat16_5 = u_xlat16_7;
              }
              if(!u_xlatb6.z)
              {
                  u_xlat3.y = in_f.vs_TEXCOORD0.y;
                  u_xlat8.xy = (u_xlat3.zy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_7 = tex2D(_GrabTexture, u_xlat8.xy);
                  u_xlat16_7 = ((u_xlat10_7 * float4(0.150000006, 0.150000006, 0.150000006, 0.150000006)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.150000006);
                  u_xlat16_5 = u_xlat16_7;
              }
              if(!u_xlatb6.w)
              {
                  u_xlat3.y = in_f.vs_TEXCOORD0.y;
                  u_xlat8.xy = (u_xlat3.wy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_3 = tex2D(_GrabTexture, u_xlat8.xy);
                  u_xlat16_3 = ((u_xlat10_3 * float4(0.160000011, 0.160000011, 0.160000011, 0.160000011)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.160000011);
                  u_xlat16_5 = u_xlat16_3;
              }
              u_xlat8.x = (((-_GrabTexture_TexelSize.x) * _BlurSize) + in_f.vs_TEXCOORD0.x);
              if((u_xlat16_31<u_xlat8.x))
              {
                  u_xlatb28 = 1;
              }
              else u_xlatb28 = 0;
              if((u_xlat8.x<u_xlat16_1.x))
              {
                  u_xlatb38 = 1;
              }
              else u_xlatb38 = 0;
              u_xlatb28 = (u_xlatb38 || u_xlatb28);
              if(!u_xlatb28)
              {
                  u_xlat8.y = in_f.vs_TEXCOORD0.y;
                  u_xlat8.xy = (u_xlat8.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_3 = tex2D(_GrabTexture, u_xlat8.xy);
                  u_xlat16_3 = ((u_xlat10_3 * float4(0.170000017, 0.170000017, 0.170000017, 0.170000017)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.170000017);
                  u_xlat16_5 = u_xlat16_3;
              }
              u_xlat3 = ((u_xlat4.xxxx * float4(-0.5, (-0.75), (-0.25), 0.25)) + in_f.vs_TEXCOORD0.xxxx);
              u_xlatb6 = lessThan(float4(u_xlat16_31, u_xlat16_31, u_xlat16_31, u_xlat16_31), u_xlat3.yxzw);
              u_xlatb7 = lessThan(u_xlat3.yxzw, u_xlat16_1.xxxx);
              u_xlatb6.x = (u_xlatb6.x || u_xlatb7.x);
              u_xlatb6.y = (u_xlatb6.y || u_xlatb7.y);
              u_xlatb6.z = (u_xlatb6.z || u_xlatb7.z);
              u_xlatb6.w = (u_xlatb6.w || u_xlatb7.w);
              if(!u_xlatb6.x)
              {
                  u_xlat8.x = u_xlat3.y;
                  u_xlat8.y = in_f.vs_TEXCOORD0.y;
                  u_xlat8.xy = (u_xlat8.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_7 = tex2D(_GrabTexture, u_xlat8.xy);
                  u_xlat16_7 = ((u_xlat10_7 * float4(0.180000022, 0.180000022, 0.180000022, 0.180000022)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.180000022);
                  u_xlat16_5 = u_xlat16_7;
              }
              if(!u_xlatb6.y)
              {
                  u_xlat3.y = in_f.vs_TEXCOORD0.y;
                  u_xlat8.xy = (u_xlat3.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_7 = tex2D(_GrabTexture, u_xlat8.xy);
                  u_xlat16_7 = ((u_xlat10_7 * float4(0.190000027, 0.190000027, 0.190000027, 0.190000027)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.190000027);
                  u_xlat16_5 = u_xlat16_7;
              }
              if(!u_xlatb6.z)
              {
                  u_xlat3.y = in_f.vs_TEXCOORD0.y;
                  u_xlat8.xy = (u_xlat3.zy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_7 = tex2D(_GrabTexture, u_xlat8.xy);
                  u_xlat16_7 = ((u_xlat10_7 * float4(0.200000033, 0.200000033, 0.200000033, 0.200000033)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.200000033);
                  u_xlat16_5 = u_xlat16_7;
              }
              if((u_xlat16_31<in_f.vs_TEXCOORD0.x))
              {
                  u_xlatb8 = 1;
              }
              else u_xlatb8 = 0;
              if((in_f.vs_TEXCOORD0.x<u_xlat16_1.x))
              {
                  u_xlatb18 = 1;
              }
              else u_xlatb18 = 0;
              u_xlatb8 = (u_xlatb18 || u_xlatb8);
              if(!u_xlatb8)
              {
                  u_xlat16_7 = ((u_xlat10_2 * float4(0.210000038, 0.210000038, 0.210000038, 0.210000038)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.210000038);
                  u_xlat16_5 = u_xlat16_7;
              }
              if(!u_xlatb6.w)
              {
                  u_xlat3.y = in_f.vs_TEXCOORD0.y;
                  u_xlat8.xy = (u_xlat3.wy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_3 = tex2D(_GrabTexture, u_xlat8.xy);
                  u_xlat16_3 = ((u_xlat10_3 * float4(0.200000033, 0.200000033, 0.200000033, 0.200000033)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.200000033);
                  u_xlat16_5 = u_xlat16_3;
              }
              u_xlat3 = ((u_xlat4.xxxx * float4(0.75, 0.5, 1.25, 1.5)) + in_f.vs_TEXCOORD0.xxxx);
              u_xlatb6 = lessThan(float4(u_xlat16_31, u_xlat16_31, u_xlat16_31, u_xlat16_31), u_xlat3.yxzw);
              u_xlatb7 = lessThan(u_xlat3.yxzw, u_xlat16_1.xxxx);
              u_xlatb6.x = (u_xlatb6.x || u_xlatb7.x);
              u_xlatb6.y = (u_xlatb6.y || u_xlatb7.y);
              u_xlatb6.z = (u_xlatb6.z || u_xlatb7.z);
              u_xlatb6.w = (u_xlatb6.w || u_xlatb7.w);
              if(!u_xlatb6.x)
              {
                  u_xlat8.x = u_xlat3.y;
                  u_xlat8.y = in_f.vs_TEXCOORD0.y;
                  u_xlat8.xy = (u_xlat8.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_7 = tex2D(_GrabTexture, u_xlat8.xy);
                  u_xlat16_7 = ((u_xlat10_7 * float4(0.190000027, 0.190000027, 0.190000027, 0.190000027)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.190000027);
                  u_xlat16_5 = u_xlat16_7;
              }
              if(!u_xlatb6.y)
              {
                  u_xlat3.y = in_f.vs_TEXCOORD0.y;
                  u_xlat8.xy = (u_xlat3.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_7 = tex2D(_GrabTexture, u_xlat8.xy);
                  u_xlat16_7 = ((u_xlat10_7 * float4(0.180000022, 0.180000022, 0.180000022, 0.180000022)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.180000022);
                  u_xlat16_5 = u_xlat16_7;
              }
              u_xlat8.x = ((_GrabTexture_TexelSize.x * _BlurSize) + in_f.vs_TEXCOORD0.x);
              if((u_xlat16_31<u_xlat8.x))
              {
                  u_xlatb28 = 1;
              }
              else u_xlatb28 = 0;
              if((u_xlat8.x<u_xlat16_1.x))
              {
                  u_xlatb38 = 1;
              }
              else u_xlatb38 = 0;
              u_xlatb28 = (u_xlatb38 || u_xlatb28);
              if(!u_xlatb28)
              {
                  u_xlat8.y = in_f.vs_TEXCOORD0.y;
                  u_xlat8.xy = (u_xlat8.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_7 = tex2D(_GrabTexture, u_xlat8.xy);
                  u_xlat16_7 = ((u_xlat10_7 * float4(0.170000017, 0.170000017, 0.170000017, 0.170000017)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.170000017);
                  u_xlat16_5 = u_xlat16_7;
              }
              if(!u_xlatb6.z)
              {
                  u_xlat3.y = in_f.vs_TEXCOORD0.y;
                  u_xlat8.xy = (u_xlat3.zy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_7 = tex2D(_GrabTexture, u_xlat8.xy);
                  u_xlat16_7 = ((u_xlat10_7 * float4(0.160000011, 0.160000011, 0.160000011, 0.160000011)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.160000011);
                  u_xlat16_5 = u_xlat16_7;
              }
              if(!u_xlatb6.w)
              {
                  u_xlat3.y = in_f.vs_TEXCOORD0.y;
                  u_xlat8.xy = (u_xlat3.wy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_3 = tex2D(_GrabTexture, u_xlat8.xy);
                  u_xlat16_3 = ((u_xlat10_3 * float4(0.150000006, 0.150000006, 0.150000006, 0.150000006)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.150000006);
                  u_xlat16_5 = u_xlat16_3;
              }
              u_xlat3 = ((u_xlat4.xxxx * float4(2.25, 1.75, 2.5, 2.75)) + in_f.vs_TEXCOORD0.xxxx);
              u_xlatb6 = lessThan(float4(u_xlat16_31, u_xlat16_31, u_xlat16_31, u_xlat16_31), u_xlat3.yxzw);
              u_xlatb7 = lessThan(u_xlat3.yxzw, u_xlat16_1.xxxx);
              u_xlatb6.x = (u_xlatb6.x || u_xlatb7.x);
              u_xlatb6.y = (u_xlatb6.y || u_xlatb7.y);
              u_xlatb6.z = (u_xlatb6.z || u_xlatb7.z);
              u_xlatb6.w = (u_xlatb6.w || u_xlatb7.w);
              if(!u_xlatb6.x)
              {
                  u_xlat8.x = u_xlat3.y;
                  u_xlat8.y = in_f.vs_TEXCOORD0.y;
                  u_xlat8.xy = (u_xlat8.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_7 = tex2D(_GrabTexture, u_xlat8.xy);
                  u_xlat16_7 = ((u_xlat10_7 * float4(0.140000001, 0.140000001, 0.140000001, 0.140000001)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.140000001);
                  u_xlat16_5 = u_xlat16_7;
              }
              u_xlat8.x = ((u_xlat4.x * 2) + in_f.vs_TEXCOORD0.x);
              if((u_xlat16_31<u_xlat8.x))
              {
                  u_xlatb28 = 1;
              }
              else u_xlatb28 = 0;
              if((u_xlat8.x<u_xlat16_1.x))
              {
                  u_xlatb38 = 1;
              }
              else u_xlatb38 = 0;
              u_xlatb28 = (u_xlatb38 || u_xlatb28);
              if(!u_xlatb28)
              {
                  u_xlat8.y = in_f.vs_TEXCOORD0.y;
                  u_xlat8.xy = (u_xlat8.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_7 = tex2D(_GrabTexture, u_xlat8.xy);
                  u_xlat16_7 = ((u_xlat10_7 * float4(0.129999995, 0.129999995, 0.129999995, 0.129999995)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.129999995);
                  u_xlat16_5 = u_xlat16_7;
              }
              if(!u_xlatb6.y)
              {
                  u_xlat3.y = in_f.vs_TEXCOORD0.y;
                  u_xlat8.xy = (u_xlat3.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_7 = tex2D(_GrabTexture, u_xlat8.xy);
                  u_xlat16_7 = ((u_xlat10_7 * float4(0.119999997, 0.119999997, 0.119999997, 0.119999997)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.119999997);
                  u_xlat16_5 = u_xlat16_7;
              }
              if(!u_xlatb6.z)
              {
                  u_xlat3.y = in_f.vs_TEXCOORD0.y;
                  u_xlat8.xy = (u_xlat3.zy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_7 = tex2D(_GrabTexture, u_xlat8.xy);
                  u_xlat16_7 = ((u_xlat10_7 * float4(0.109999999, 0.109999999, 0.109999999, 0.109999999)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.109999999);
                  u_xlat16_5 = u_xlat16_7;
              }
              if(!u_xlatb6.w)
              {
                  u_xlat3.y = in_f.vs_TEXCOORD0.y;
                  u_xlat8.xy = (u_xlat3.wy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_3 = tex2D(_GrabTexture, u_xlat8.xy);
                  u_xlat16_3 = ((u_xlat10_3 * float4(0.100000001, 0.100000001, 0.100000001, 0.100000001)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.100000001);
                  u_xlat16_5 = u_xlat16_3;
              }
              u_xlat3 = ((u_xlat4.xxxx * float4(3.25, 3, 3.5, 3.75)) + in_f.vs_TEXCOORD0.xxxx);
              u_xlatb6 = lessThan(float4(u_xlat16_31, u_xlat16_31, u_xlat16_31, u_xlat16_31), u_xlat3.yxzw);
              u_xlatb7 = lessThan(u_xlat3.yxzw, u_xlat16_1.xxxx);
              u_xlatb6.x = (u_xlatb6.x || u_xlatb7.x);
              u_xlatb6.y = (u_xlatb6.y || u_xlatb7.y);
              u_xlatb6.z = (u_xlatb6.z || u_xlatb7.z);
              u_xlatb6.w = (u_xlatb6.w || u_xlatb7.w);
              if(!u_xlatb6.x)
              {
                  u_xlat8.x = u_xlat3.y;
                  u_xlat8.y = in_f.vs_TEXCOORD0.y;
                  u_xlat8.xy = (u_xlat8.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_7 = tex2D(_GrabTexture, u_xlat8.xy);
                  u_xlat16_7 = ((u_xlat10_7 * float4(0.0900000036, 0.0900000036, 0.0900000036, 0.0900000036)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.0900000036);
                  u_xlat16_5 = u_xlat16_7;
              }
              if(!u_xlatb6.y)
              {
                  u_xlat3.y = in_f.vs_TEXCOORD0.y;
                  u_xlat8.xy = (u_xlat3.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_7 = tex2D(_GrabTexture, u_xlat8.xy);
                  u_xlat16_7 = ((u_xlat10_7 * float4(0.0800000057, 0.0800000057, 0.0800000057, 0.0800000057)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.0800000057);
                  u_xlat16_5 = u_xlat16_7;
              }
              if(!u_xlatb6.z)
              {
                  u_xlat3.y = in_f.vs_TEXCOORD0.y;
                  u_xlat8.xy = (u_xlat3.zy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_7 = tex2D(_GrabTexture, u_xlat8.xy);
                  u_xlat16_7 = ((u_xlat10_7 * float4(0.0700000077, 0.0700000077, 0.0700000077, 0.0700000077)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.0700000077);
                  u_xlat16_5 = u_xlat16_7;
              }
              if(!u_xlatb6.w)
              {
                  u_xlat3.y = in_f.vs_TEXCOORD0.y;
                  u_xlat8.xy = (u_xlat3.wy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_3 = tex2D(_GrabTexture, u_xlat8.xy);
                  u_xlat16_3 = ((u_xlat10_3 * float4(0.0600000098, 0.0600000098, 0.0600000098, 0.0600000098)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.0600000098);
                  u_xlat16_5 = u_xlat16_3;
              }
              u_xlat8.x = ((u_xlat4.x * 4) + in_f.vs_TEXCOORD0.x);
              if((u_xlat16_31<u_xlat8.x))
              {
                  u_xlatb28 = 1;
              }
              else u_xlatb28 = 0;
              if((u_xlat8.x<u_xlat16_1.x))
              {
                  u_xlatb38 = 1;
              }
              else u_xlatb38 = 0;
              u_xlatb28 = (u_xlatb38 || u_xlatb28);
              if(!u_xlatb28)
              {
                  u_xlat8.y = in_f.vs_TEXCOORD0.y;
                  u_xlat8.xy = (u_xlat8.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_3 = tex2D(_GrabTexture, u_xlat8.xy);
                  u_xlat16_3 = ((u_xlat10_3 * float4(0.0500000119, 0.0500000119, 0.0500000119, 0.0500000119)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.0500000119);
                  u_xlat16_5 = u_xlat16_3;
              }
          }
          if((_Quality==3))
          {
              u_xlat8.x = (_GrabTexture_TexelSize.x * _BlurSize);
              u_xlat3 = ((u_xlat8.xxxx * float4(-3.875, (-4), (-3.75), (-3.625))) + in_f.vs_TEXCOORD0.xxxx);
              u_xlatb4 = lessThan(float4(u_xlat16_31, u_xlat16_31, u_xlat16_31, u_xlat16_31), u_xlat3.yxzw);
              u_xlatb6 = lessThan(u_xlat3.yxzw, u_xlat16_1.xxxx);
              u_xlatb4.x = (u_xlatb4.x || u_xlatb6.x);
              u_xlatb4.y = (u_xlatb4.y || u_xlatb6.y);
              u_xlatb4.z = (u_xlatb4.z || u_xlatb6.z);
              u_xlatb4.w = (u_xlatb4.w || u_xlatb6.w);
              if(u_xlatb4.x)
              {
                  u_xlat16_5.x = float(0);
                  u_xlat16_5.y = float(0);
                  u_xlat16_5.z = float(0);
                  u_xlat16_5.w = float(0);
                  u_xlat16_11 = 0;
              }
              else
              {
                  u_xlat9.x = u_xlat3.y;
                  u_xlat9.y = in_f.vs_TEXCOORD0.y;
                  u_xlat18.xy = (u_xlat9.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_6 = tex2D(_GrabTexture, u_xlat18.xy);
                  u_xlat16_6 = (u_xlat10_6 * float4(0.0500000007, 0.0500000007, 0.0500000007, 0.0500000007));
                  u_xlat16_5 = u_xlat16_6;
                  u_xlat16_11 = 0.0500000007;
              }
              if(!u_xlatb4.y)
              {
                  u_xlat3.y = in_f.vs_TEXCOORD0.y;
                  u_xlat18.xy = (u_xlat3.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_6 = tex2D(_GrabTexture, u_xlat18.xy);
                  u_xlat16_6 = ((u_xlat10_6 * float4(0.0549999997, 0.0549999997, 0.0549999997, 0.0549999997)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.0549999997);
                  u_xlat16_5 = u_xlat16_6;
              }
              if(!u_xlatb4.z)
              {
                  u_xlat3.y = in_f.vs_TEXCOORD0.y;
                  u_xlat18.xy = (u_xlat3.zy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_6 = tex2D(_GrabTexture, u_xlat18.xy);
                  u_xlat16_6 = ((u_xlat10_6 * float4(0.0599999987, 0.0599999987, 0.0599999987, 0.0599999987)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.0599999987);
                  u_xlat16_5 = u_xlat16_6;
              }
              if(!u_xlatb4.w)
              {
                  u_xlat3.y = in_f.vs_TEXCOORD0.y;
                  u_xlat18.xy = (u_xlat3.wy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_3 = tex2D(_GrabTexture, u_xlat18.xy);
                  u_xlat16_3 = ((u_xlat10_3 * float4(0.0649999976, 0.0649999976, 0.0649999976, 0.0649999976)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.0649999976);
                  u_xlat16_5 = u_xlat16_3;
              }
              u_xlat3 = ((u_xlat8.xxxx * float4(-3.375, (-3.5), (-3.25), (-3.125))) + in_f.vs_TEXCOORD0.xxxx);
              u_xlatb4 = lessThan(float4(u_xlat16_31, u_xlat16_31, u_xlat16_31, u_xlat16_31), u_xlat3.yxzw);
              u_xlatb6 = lessThan(u_xlat3.yxzw, u_xlat16_1.xxxx);
              u_xlatb4.x = (u_xlatb4.x || u_xlatb6.x);
              u_xlatb4.y = (u_xlatb4.y || u_xlatb6.y);
              u_xlatb4.z = (u_xlatb4.z || u_xlatb6.z);
              u_xlatb4.w = (u_xlatb4.w || u_xlatb6.w);
              if(!u_xlatb4.x)
              {
                  u_xlat9.x = u_xlat3.y;
                  u_xlat9.y = in_f.vs_TEXCOORD0.y;
                  u_xlat18.xy = (u_xlat9.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_6 = tex2D(_GrabTexture, u_xlat18.xy);
                  u_xlat16_6 = ((u_xlat10_6 * float4(0.0700000003, 0.0700000003, 0.0700000003, 0.0700000003)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.0700000003);
                  u_xlat16_5 = u_xlat16_6;
              }
              if(!u_xlatb4.y)
              {
                  u_xlat3.y = in_f.vs_TEXCOORD0.y;
                  u_xlat18.xy = (u_xlat3.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_6 = tex2D(_GrabTexture, u_xlat18.xy);
                  u_xlat16_6 = ((u_xlat10_6 * float4(0.075000003, 0.075000003, 0.075000003, 0.075000003)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.075000003);
                  u_xlat16_5 = u_xlat16_6;
              }
              if(!u_xlatb4.z)
              {
                  u_xlat3.y = in_f.vs_TEXCOORD0.y;
                  u_xlat18.xy = (u_xlat3.zy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_6 = tex2D(_GrabTexture, u_xlat18.xy);
                  u_xlat16_6 = ((u_xlat10_6 * float4(0.0800000057, 0.0800000057, 0.0800000057, 0.0800000057)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.0800000057);
                  u_xlat16_5 = u_xlat16_6;
              }
              if(!u_xlatb4.w)
              {
                  u_xlat3.y = in_f.vs_TEXCOORD0.y;
                  u_xlat18.xy = (u_xlat3.wy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_3 = tex2D(_GrabTexture, u_xlat18.xy);
                  u_xlat16_3 = ((u_xlat10_3 * float4(0.0850000083, 0.0850000083, 0.0850000083, 0.0850000083)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.0850000083);
                  u_xlat16_5 = u_xlat16_3;
              }
              u_xlat3 = ((u_xlat8.xxxx * float4(-2.875, (-3), (-2.75), (-2.625))) + in_f.vs_TEXCOORD0.xxxx);
              u_xlatb4 = lessThan(float4(u_xlat16_31, u_xlat16_31, u_xlat16_31, u_xlat16_31), u_xlat3.yxzw);
              u_xlatb6 = lessThan(u_xlat3.yxzw, u_xlat16_1.xxxx);
              u_xlatb4.x = (u_xlatb4.x || u_xlatb6.x);
              u_xlatb4.y = (u_xlatb4.y || u_xlatb6.y);
              u_xlatb4.z = (u_xlatb4.z || u_xlatb6.z);
              u_xlatb4.w = (u_xlatb4.w || u_xlatb6.w);
              if(!u_xlatb4.x)
              {
                  u_xlat9.x = u_xlat3.y;
                  u_xlat9.y = in_f.vs_TEXCOORD0.y;
                  u_xlat18.xy = (u_xlat9.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_6 = tex2D(_GrabTexture, u_xlat18.xy);
                  u_xlat16_6 = ((u_xlat10_6 * float4(0.090000011, 0.090000011, 0.090000011, 0.090000011)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.090000011);
                  u_xlat16_5 = u_xlat16_6;
              }
              if(!u_xlatb4.y)
              {
                  u_xlat3.y = in_f.vs_TEXCOORD0.y;
                  u_xlat18.xy = (u_xlat3.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_6 = tex2D(_GrabTexture, u_xlat18.xy);
                  u_xlat16_6 = ((u_xlat10_6 * float4(0.0950000137, 0.0950000137, 0.0950000137, 0.0950000137)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.0950000137);
                  u_xlat16_5 = u_xlat16_6;
              }
              if(!u_xlatb4.z)
              {
                  u_xlat3.y = in_f.vs_TEXCOORD0.y;
                  u_xlat18.xy = (u_xlat3.zy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_6 = tex2D(_GrabTexture, u_xlat18.xy);
                  u_xlat16_6 = ((u_xlat10_6 * float4(0.100000016, 0.100000016, 0.100000016, 0.100000016)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.100000016);
                  u_xlat16_5 = u_xlat16_6;
              }
              if(!u_xlatb4.w)
              {
                  u_xlat3.y = in_f.vs_TEXCOORD0.y;
                  u_xlat18.xy = (u_xlat3.wy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_3 = tex2D(_GrabTexture, u_xlat18.xy);
                  u_xlat16_3 = ((u_xlat10_3 * float4(0.105000019, 0.105000019, 0.105000019, 0.105000019)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.105000019);
                  u_xlat16_5 = u_xlat16_3;
              }
              u_xlat3 = ((u_xlat8.xxxx * float4(-2.375, (-2.5), (-2.25), (-2.125))) + in_f.vs_TEXCOORD0.xxxx);
              u_xlatb4 = lessThan(float4(u_xlat16_31, u_xlat16_31, u_xlat16_31, u_xlat16_31), u_xlat3.yxzw);
              u_xlatb6 = lessThan(u_xlat3.yxzw, u_xlat16_1.xxxx);
              u_xlatb4.x = (u_xlatb4.x || u_xlatb6.x);
              u_xlatb4.y = (u_xlatb4.y || u_xlatb6.y);
              u_xlatb4.z = (u_xlatb4.z || u_xlatb6.z);
              u_xlatb4.w = (u_xlatb4.w || u_xlatb6.w);
              if(!u_xlatb4.x)
              {
                  u_xlat9.x = u_xlat3.y;
                  u_xlat9.y = in_f.vs_TEXCOORD0.y;
                  u_xlat18.xy = (u_xlat9.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_6 = tex2D(_GrabTexture, u_xlat18.xy);
                  u_xlat16_6 = ((u_xlat10_6 * float4(0.110000022, 0.110000022, 0.110000022, 0.110000022)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.110000022);
                  u_xlat16_5 = u_xlat16_6;
              }
              if(!u_xlatb4.y)
              {
                  u_xlat3.y = in_f.vs_TEXCOORD0.y;
                  u_xlat18.xy = (u_xlat3.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_6 = tex2D(_GrabTexture, u_xlat18.xy);
                  u_xlat16_6 = ((u_xlat10_6 * float4(0.115000024, 0.115000024, 0.115000024, 0.115000024)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.115000024);
                  u_xlat16_5 = u_xlat16_6;
              }
              if(!u_xlatb4.z)
              {
                  u_xlat3.y = in_f.vs_TEXCOORD0.y;
                  u_xlat18.xy = (u_xlat3.zy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_6 = tex2D(_GrabTexture, u_xlat18.xy);
                  u_xlat16_6 = ((u_xlat10_6 * float4(0.120000027, 0.120000027, 0.120000027, 0.120000027)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.120000027);
                  u_xlat16_5 = u_xlat16_6;
              }
              if(!u_xlatb4.w)
              {
                  u_xlat3.y = in_f.vs_TEXCOORD0.y;
                  u_xlat18.xy = (u_xlat3.wy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_3 = tex2D(_GrabTexture, u_xlat18.xy);
                  u_xlat16_3 = ((u_xlat10_3 * float4(0.12500003, 0.12500003, 0.12500003, 0.12500003)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.12500003);
                  u_xlat16_5 = u_xlat16_3;
              }
              u_xlat3 = ((u_xlat8.xxxx * float4(-1.875, (-2), (-1.75), (-1.625))) + in_f.vs_TEXCOORD0.xxxx);
              u_xlatb4 = lessThan(float4(u_xlat16_31, u_xlat16_31, u_xlat16_31, u_xlat16_31), u_xlat3.yxzw);
              u_xlatb6 = lessThan(u_xlat3.yxzw, u_xlat16_1.xxxx);
              u_xlatb4.x = (u_xlatb4.x || u_xlatb6.x);
              u_xlatb4.y = (u_xlatb4.y || u_xlatb6.y);
              u_xlatb4.z = (u_xlatb4.z || u_xlatb6.z);
              u_xlatb4.w = (u_xlatb4.w || u_xlatb6.w);
              if(!u_xlatb4.x)
              {
                  u_xlat9.x = u_xlat3.y;
                  u_xlat9.y = in_f.vs_TEXCOORD0.y;
                  u_xlat18.xy = (u_xlat9.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_6 = tex2D(_GrabTexture, u_xlat18.xy);
                  u_xlat16_6 = ((u_xlat10_6 * float4(0.130000025, 0.130000025, 0.130000025, 0.130000025)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.130000025);
                  u_xlat16_5 = u_xlat16_6;
              }
              if(!u_xlatb4.y)
              {
                  u_xlat3.y = in_f.vs_TEXCOORD0.y;
                  u_xlat18.xy = (u_xlat3.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_6 = tex2D(_GrabTexture, u_xlat18.xy);
                  u_xlat16_6 = ((u_xlat10_6 * float4(0.13500002, 0.13500002, 0.13500002, 0.13500002)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.13500002);
                  u_xlat16_5 = u_xlat16_6;
              }
              if(!u_xlatb4.z)
              {
                  u_xlat3.y = in_f.vs_TEXCOORD0.y;
                  u_xlat18.xy = (u_xlat3.zy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_6 = tex2D(_GrabTexture, u_xlat18.xy);
                  u_xlat16_6 = ((u_xlat10_6 * float4(0.140000015, 0.140000015, 0.140000015, 0.140000015)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.140000015);
                  u_xlat16_5 = u_xlat16_6;
              }
              if(!u_xlatb4.w)
              {
                  u_xlat3.y = in_f.vs_TEXCOORD0.y;
                  u_xlat18.xy = (u_xlat3.wy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_3 = tex2D(_GrabTexture, u_xlat18.xy);
                  u_xlat16_3 = ((u_xlat10_3 * float4(0.145000011, 0.145000011, 0.145000011, 0.145000011)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.145000011);
                  u_xlat16_5 = u_xlat16_3;
              }
              u_xlat3 = ((u_xlat8.xxxx * float4(-1.375, (-1.5), (-1.25), (-1.125))) + in_f.vs_TEXCOORD0.xxxx);
              u_xlatb4 = lessThan(float4(u_xlat16_31, u_xlat16_31, u_xlat16_31, u_xlat16_31), u_xlat3.yxzw);
              u_xlatb6 = lessThan(u_xlat3.yxzw, u_xlat16_1.xxxx);
              u_xlatb4.x = (u_xlatb4.x || u_xlatb6.x);
              u_xlatb4.y = (u_xlatb4.y || u_xlatb6.y);
              u_xlatb4.z = (u_xlatb4.z || u_xlatb6.z);
              u_xlatb4.w = (u_xlatb4.w || u_xlatb6.w);
              if(!u_xlatb4.x)
              {
                  u_xlat9.x = u_xlat3.y;
                  u_xlat9.y = in_f.vs_TEXCOORD0.y;
                  u_xlat18.xy = (u_xlat9.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_6 = tex2D(_GrabTexture, u_xlat18.xy);
                  u_xlat16_6 = ((u_xlat10_6 * float4(0.150000006, 0.150000006, 0.150000006, 0.150000006)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.150000006);
                  u_xlat16_5 = u_xlat16_6;
              }
              if(!u_xlatb4.y)
              {
                  u_xlat3.y = in_f.vs_TEXCOORD0.y;
                  u_xlat18.xy = (u_xlat3.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_6 = tex2D(_GrabTexture, u_xlat18.xy);
                  u_xlat16_6 = ((u_xlat10_6 * float4(0.155000001, 0.155000001, 0.155000001, 0.155000001)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.155000001);
                  u_xlat16_5 = u_xlat16_6;
              }
              if(!u_xlatb4.z)
              {
                  u_xlat3.y = in_f.vs_TEXCOORD0.y;
                  u_xlat18.xy = (u_xlat3.zy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_6 = tex2D(_GrabTexture, u_xlat18.xy);
                  u_xlat16_6 = ((u_xlat10_6 * float4(0.159999996, 0.159999996, 0.159999996, 0.159999996)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.159999996);
                  u_xlat16_5 = u_xlat16_6;
              }
              if(!u_xlatb4.w)
              {
                  u_xlat3.y = in_f.vs_TEXCOORD0.y;
                  u_xlat18.xy = (u_xlat3.wy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_3 = tex2D(_GrabTexture, u_xlat18.xy);
                  u_xlat16_3 = ((u_xlat10_3 * float4(0.164999992, 0.164999992, 0.164999992, 0.164999992)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.164999992);
                  u_xlat16_5 = u_xlat16_3;
              }
              u_xlat9.x = (((-_GrabTexture_TexelSize.x) * _BlurSize) + in_f.vs_TEXCOORD0.x);
              if((u_xlat16_31<u_xlat9.x))
              {
                  u_xlatb18 = 1;
              }
              else u_xlatb18 = 0;
              if((u_xlat9.x<u_xlat16_1.x))
              {
                  u_xlatb28 = 1;
              }
              else u_xlatb28 = 0;
              u_xlatb18 = (u_xlatb28 || u_xlatb18);
              if(!u_xlatb18)
              {
                  u_xlat9.y = in_f.vs_TEXCOORD0.y;
                  u_xlat18.xy = (u_xlat9.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_3 = tex2D(_GrabTexture, u_xlat18.xy);
                  u_xlat16_3 = ((u_xlat10_3 * float4(0.169999987, 0.169999987, 0.169999987, 0.169999987)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.169999987);
                  u_xlat16_5 = u_xlat16_3;
              }
              u_xlat3 = ((u_xlat8.xxxx * float4(-0.75, (-0.875), (-0.625), (-0.5))) + in_f.vs_TEXCOORD0.xxxx);
              u_xlatb4 = lessThan(float4(u_xlat16_31, u_xlat16_31, u_xlat16_31, u_xlat16_31), u_xlat3.yxzw);
              u_xlatb6 = lessThan(u_xlat3.yxzw, u_xlat16_1.xxxx);
              u_xlatb4.x = (u_xlatb4.x || u_xlatb6.x);
              u_xlatb4.y = (u_xlatb4.y || u_xlatb6.y);
              u_xlatb4.z = (u_xlatb4.z || u_xlatb6.z);
              u_xlatb4.w = (u_xlatb4.w || u_xlatb6.w);
              if(!u_xlatb4.x)
              {
                  u_xlat9.x = u_xlat3.y;
                  u_xlat9.y = in_f.vs_TEXCOORD0.y;
                  u_xlat18.xy = (u_xlat9.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_6 = tex2D(_GrabTexture, u_xlat18.xy);
                  u_xlat16_6 = ((u_xlat10_6 * float4(0.174999982, 0.174999982, 0.174999982, 0.174999982)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.174999982);
                  u_xlat16_5 = u_xlat16_6;
              }
              if(!u_xlatb4.y)
              {
                  u_xlat3.y = in_f.vs_TEXCOORD0.y;
                  u_xlat18.xy = (u_xlat3.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_6 = tex2D(_GrabTexture, u_xlat18.xy);
                  u_xlat16_6 = ((u_xlat10_6 * float4(0.179999977, 0.179999977, 0.179999977, 0.179999977)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.179999977);
                  u_xlat16_5 = u_xlat16_6;
              }
              if(!u_xlatb4.z)
              {
                  u_xlat3.y = in_f.vs_TEXCOORD0.y;
                  u_xlat18.xy = (u_xlat3.zy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_6 = tex2D(_GrabTexture, u_xlat18.xy);
                  u_xlat16_6 = ((u_xlat10_6 * float4(0.184999973, 0.184999973, 0.184999973, 0.184999973)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.184999973);
                  u_xlat16_5 = u_xlat16_6;
              }
              if(!u_xlatb4.w)
              {
                  u_xlat3.y = in_f.vs_TEXCOORD0.y;
                  u_xlat18.xy = (u_xlat3.wy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_3 = tex2D(_GrabTexture, u_xlat18.xy);
                  u_xlat16_3 = ((u_xlat10_3 * float4(0.189999968, 0.189999968, 0.189999968, 0.189999968)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.189999968);
                  u_xlat16_5 = u_xlat16_3;
              }
              u_xlat3 = ((u_xlat8.xxxx * float4(-0.25, (-0.375), (-0.125), 0.125)) + in_f.vs_TEXCOORD0.xxxx);
              u_xlatb4 = lessThan(float4(u_xlat16_31, u_xlat16_31, u_xlat16_31, u_xlat16_31), u_xlat3.yxzw);
              u_xlatb6 = lessThan(u_xlat3.yxzw, u_xlat16_1.xxxx);
              u_xlatb4.x = (u_xlatb4.x || u_xlatb6.x);
              u_xlatb4.y = (u_xlatb4.y || u_xlatb6.y);
              u_xlatb4.z = (u_xlatb4.z || u_xlatb6.z);
              u_xlatb4.w = (u_xlatb4.w || u_xlatb6.w);
              if(!u_xlatb4.x)
              {
                  u_xlat9.x = u_xlat3.y;
                  u_xlat9.y = in_f.vs_TEXCOORD0.y;
                  u_xlat18.xy = (u_xlat9.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_6 = tex2D(_GrabTexture, u_xlat18.xy);
                  u_xlat16_6 = ((u_xlat10_6 * float4(0.194999963, 0.194999963, 0.194999963, 0.194999963)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.194999963);
                  u_xlat16_5 = u_xlat16_6;
              }
              if(!u_xlatb4.y)
              {
                  u_xlat3.y = in_f.vs_TEXCOORD0.y;
                  u_xlat18.xy = (u_xlat3.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_6 = tex2D(_GrabTexture, u_xlat18.xy);
                  u_xlat16_6 = ((u_xlat10_6 * float4(0.199999958, 0.199999958, 0.199999958, 0.199999958)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.199999958);
                  u_xlat16_5 = u_xlat16_6;
              }
              if(!u_xlatb4.z)
              {
                  u_xlat3.y = in_f.vs_TEXCOORD0.y;
                  u_xlat18.xy = (u_xlat3.zy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_6 = tex2D(_GrabTexture, u_xlat18.xy);
                  u_xlat16_6 = ((u_xlat10_6 * float4(0.204999954, 0.204999954, 0.204999954, 0.204999954)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.204999954);
                  u_xlat16_5 = u_xlat16_6;
              }
              if((u_xlat16_31<in_f.vs_TEXCOORD0.x))
              {
                  u_xlatb18 = 1;
              }
              else u_xlatb18 = 0;
              if((in_f.vs_TEXCOORD0.x<u_xlat16_1.x))
              {
                  u_xlatb28 = 1;
              }
              else u_xlatb28 = 0;
              u_xlatb18 = (u_xlatb28 || u_xlatb18);
              if(!u_xlatb18)
              {
                  u_xlat16_6 = ((u_xlat10_2 * float4(0.209999949, 0.209999949, 0.209999949, 0.209999949)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.209999949);
                  u_xlat16_5 = u_xlat16_6;
              }
              if(!u_xlatb4.w)
              {
                  u_xlat3.y = in_f.vs_TEXCOORD0.y;
                  u_xlat18.xy = (u_xlat3.wy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_3 = tex2D(_GrabTexture, u_xlat18.xy);
                  u_xlat16_3 = ((u_xlat10_3 * float4(0.204999954, 0.204999954, 0.204999954, 0.204999954)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.204999954);
                  u_xlat16_5 = u_xlat16_3;
              }
              u_xlat3 = ((u_xlat8.xxxx * float4(0.375, 0.25, 0.5, 0.625)) + in_f.vs_TEXCOORD0.xxxx);
              u_xlatb4 = lessThan(float4(u_xlat16_31, u_xlat16_31, u_xlat16_31, u_xlat16_31), u_xlat3.yxzw);
              u_xlatb6 = lessThan(u_xlat3.yxzw, u_xlat16_1.xxxx);
              u_xlatb4.x = (u_xlatb4.x || u_xlatb6.x);
              u_xlatb4.y = (u_xlatb4.y || u_xlatb6.y);
              u_xlatb4.z = (u_xlatb4.z || u_xlatb6.z);
              u_xlatb4.w = (u_xlatb4.w || u_xlatb6.w);
              if(!u_xlatb4.x)
              {
                  u_xlat9.x = u_xlat3.y;
                  u_xlat9.y = in_f.vs_TEXCOORD0.y;
                  u_xlat18.xy = (u_xlat9.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_6 = tex2D(_GrabTexture, u_xlat18.xy);
                  u_xlat16_6 = ((u_xlat10_6 * float4(0.199999958, 0.199999958, 0.199999958, 0.199999958)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.199999958);
                  u_xlat16_5 = u_xlat16_6;
              }
              if(!u_xlatb4.y)
              {
                  u_xlat3.y = in_f.vs_TEXCOORD0.y;
                  u_xlat18.xy = (u_xlat3.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_6 = tex2D(_GrabTexture, u_xlat18.xy);
                  u_xlat16_6 = ((u_xlat10_6 * float4(0.194999963, 0.194999963, 0.194999963, 0.194999963)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.194999963);
                  u_xlat16_5 = u_xlat16_6;
              }
              if(!u_xlatb4.z)
              {
                  u_xlat3.y = in_f.vs_TEXCOORD0.y;
                  u_xlat18.xy = (u_xlat3.zy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_6 = tex2D(_GrabTexture, u_xlat18.xy);
                  u_xlat16_6 = ((u_xlat10_6 * float4(0.189999968, 0.189999968, 0.189999968, 0.189999968)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.189999968);
                  u_xlat16_5 = u_xlat16_6;
              }
              if(!u_xlatb4.w)
              {
                  u_xlat3.y = in_f.vs_TEXCOORD0.y;
                  u_xlat18.xy = (u_xlat3.wy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_3 = tex2D(_GrabTexture, u_xlat18.xy);
                  u_xlat16_5 = ((u_xlat10_3 * float4(0.184999973, 0.184999973, 0.184999973, 0.184999973)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.184999973);
                  u_xlat16_5 = u_xlat16_5;
              }
              u_xlat3 = ((u_xlat8.xxxx * float4(0.875, 0.75, 1.125, 1.25)) + in_f.vs_TEXCOORD0.xxxx);
              u_xlatb4 = lessThan(float4(u_xlat16_31, u_xlat16_31, u_xlat16_31, u_xlat16_31), u_xlat3.yxzw);
              u_xlatb6 = lessThan(u_xlat3.yxzw, u_xlat16_1.xxxx);
              u_xlatb4.x = (u_xlatb4.x || u_xlatb6.x);
              u_xlatb4.y = (u_xlatb4.y || u_xlatb6.y);
              u_xlatb4.z = (u_xlatb4.z || u_xlatb6.z);
              u_xlatb4.w = (u_xlatb4.w || u_xlatb6.w);
              if(!u_xlatb4.x)
              {
                  u_xlat9.x = u_xlat3.y;
                  u_xlat9.y = in_f.vs_TEXCOORD0.y;
                  u_xlat18.xy = (u_xlat9.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_6 = tex2D(_GrabTexture, u_xlat18.xy);
                  u_xlat16_5 = ((u_xlat10_6 * float4(0.179999977, 0.179999977, 0.179999977, 0.179999977)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.179999977);
                  u_xlat16_5 = u_xlat16_5;
              }
              if(!u_xlatb4.y)
              {
                  u_xlat3.y = in_f.vs_TEXCOORD0.y;
                  u_xlat18.xy = (u_xlat3.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_6 = tex2D(_GrabTexture, u_xlat18.xy);
                  u_xlat16_5 = ((u_xlat10_6 * float4(0.174999982, 0.174999982, 0.174999982, 0.174999982)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.174999982);
                  u_xlat16_5 = u_xlat16_5;
              }
              u_xlat9.x = ((_GrabTexture_TexelSize.x * _BlurSize) + in_f.vs_TEXCOORD0.x);
              if((u_xlat16_31<u_xlat9.x))
              {
                  u_xlatb18 = 1;
              }
              else u_xlatb18 = 0;
              if((u_xlat9.x<u_xlat16_1.x))
              {
                  u_xlatb28 = 1;
              }
              else u_xlatb28 = 0;
              u_xlatb18 = (u_xlatb28 || u_xlatb18);
              if(!u_xlatb18)
              {
                  u_xlat9.y = in_f.vs_TEXCOORD0.y;
                  u_xlat18.xy = (u_xlat9.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_6 = tex2D(_GrabTexture, u_xlat18.xy);
                  u_xlat16_5 = ((u_xlat10_6 * float4(0.169999987, 0.169999987, 0.169999987, 0.169999987)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.169999987);
                  u_xlat16_5 = u_xlat16_5;
              }
              if(!u_xlatb4.z)
              {
                  u_xlat3.y = in_f.vs_TEXCOORD0.y;
                  u_xlat18.xy = (u_xlat3.zy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_6 = tex2D(_GrabTexture, u_xlat18.xy);
                  u_xlat16_5 = ((u_xlat10_6 * float4(0.164999992, 0.164999992, 0.164999992, 0.164999992)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.164999992);
                  u_xlat16_5 = u_xlat16_5;
              }
              if(!u_xlatb4.w)
              {
                  u_xlat3.y = in_f.vs_TEXCOORD0.y;
                  u_xlat18.xy = (u_xlat3.wy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_3 = tex2D(_GrabTexture, u_xlat18.xy);
                  u_xlat16_5 = ((u_xlat10_3 * float4(0.159999996, 0.159999996, 0.159999996, 0.159999996)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.159999996);
                  u_xlat16_5 = u_xlat16_5;
              }
              u_xlat3 = ((u_xlat8.xxxx * float4(1.5, 1.375, 1.625, 1.75)) + in_f.vs_TEXCOORD0.xxxx);
              u_xlatb4 = lessThan(float4(u_xlat16_31, u_xlat16_31, u_xlat16_31, u_xlat16_31), u_xlat3.yxzw);
              u_xlatb6 = lessThan(u_xlat3.yxzw, u_xlat16_1.xxxx);
              u_xlatb4.x = (u_xlatb4.x || u_xlatb6.x);
              u_xlatb4.y = (u_xlatb4.y || u_xlatb6.y);
              u_xlatb4.z = (u_xlatb4.z || u_xlatb6.z);
              u_xlatb4.w = (u_xlatb4.w || u_xlatb6.w);
              if(!u_xlatb4.x)
              {
                  u_xlat9.x = u_xlat3.y;
                  u_xlat9.y = in_f.vs_TEXCOORD0.y;
                  u_xlat18.xy = (u_xlat9.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_6 = tex2D(_GrabTexture, u_xlat18.xy);
                  u_xlat16_5 = ((u_xlat10_6 * float4(0.155000001, 0.155000001, 0.155000001, 0.155000001)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.155000001);
                  u_xlat16_5 = u_xlat16_5;
              }
              if(!u_xlatb4.y)
              {
                  u_xlat3.y = in_f.vs_TEXCOORD0.y;
                  u_xlat18.xy = (u_xlat3.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_6 = tex2D(_GrabTexture, u_xlat18.xy);
                  u_xlat16_5 = ((u_xlat10_6 * float4(0.150000006, 0.150000006, 0.150000006, 0.150000006)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.150000006);
                  u_xlat16_5 = u_xlat16_5;
              }
              if(!u_xlatb4.z)
              {
                  u_xlat3.y = in_f.vs_TEXCOORD0.y;
                  u_xlat18.xy = (u_xlat3.zy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_6 = tex2D(_GrabTexture, u_xlat18.xy);
                  u_xlat16_5 = ((u_xlat10_6 * float4(0.145000011, 0.145000011, 0.145000011, 0.145000011)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.145000011);
                  u_xlat16_5 = u_xlat16_5;
              }
              if(!u_xlatb4.w)
              {
                  u_xlat3.y = in_f.vs_TEXCOORD0.y;
                  u_xlat18.xy = (u_xlat3.wy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_3 = tex2D(_GrabTexture, u_xlat18.xy);
                  u_xlat16_5 = ((u_xlat10_3 * float4(0.140000015, 0.140000015, 0.140000015, 0.140000015)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.140000015);
                  u_xlat16_5 = u_xlat16_5;
              }
              u_xlat3 = ((u_xlat8.xxxx * float4(2.125, 1.875, 2.25, 2.375)) + in_f.vs_TEXCOORD0.xxxx);
              u_xlatb4 = lessThan(float4(u_xlat16_31, u_xlat16_31, u_xlat16_31, u_xlat16_31), u_xlat3.yxzw);
              u_xlatb6 = lessThan(u_xlat3.yxzw, u_xlat16_1.xxxx);
              u_xlatb4.x = (u_xlatb4.x || u_xlatb6.x);
              u_xlatb4.y = (u_xlatb4.y || u_xlatb6.y);
              u_xlatb4.z = (u_xlatb4.z || u_xlatb6.z);
              u_xlatb4.w = (u_xlatb4.w || u_xlatb6.w);
              if(!u_xlatb4.x)
              {
                  u_xlat9.x = u_xlat3.y;
                  u_xlat9.y = in_f.vs_TEXCOORD0.y;
                  u_xlat18.xy = (u_xlat9.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_6 = tex2D(_GrabTexture, u_xlat18.xy);
                  u_xlat16_5 = ((u_xlat10_6 * float4(0.13500002, 0.13500002, 0.13500002, 0.13500002)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.13500002);
                  u_xlat16_5 = u_xlat16_5;
              }
              u_xlat9.x = ((u_xlat8.x * 2) + in_f.vs_TEXCOORD0.x);
              if((u_xlat16_31<u_xlat9.x))
              {
                  u_xlatb18 = 1;
              }
              else u_xlatb18 = 0;
              if((u_xlat9.x<u_xlat16_1.x))
              {
                  u_xlatb28 = 1;
              }
              else u_xlatb28 = 0;
              u_xlatb18 = (u_xlatb28 || u_xlatb18);
              if(!u_xlatb18)
              {
                  u_xlat9.y = in_f.vs_TEXCOORD0.y;
                  u_xlat18.xy = (u_xlat9.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_6 = tex2D(_GrabTexture, u_xlat18.xy);
                  u_xlat16_5 = ((u_xlat10_6 * float4(0.130000025, 0.130000025, 0.130000025, 0.130000025)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.130000025);
                  u_xlat16_5 = u_xlat16_5;
              }
              if(!u_xlatb4.y)
              {
                  u_xlat3.y = in_f.vs_TEXCOORD0.y;
                  u_xlat18.xy = (u_xlat3.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_6 = tex2D(_GrabTexture, u_xlat18.xy);
                  u_xlat16_5 = ((u_xlat10_6 * float4(0.12500003, 0.12500003, 0.12500003, 0.12500003)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.12500003);
                  u_xlat16_5 = u_xlat16_5;
              }
              if(!u_xlatb4.z)
              {
                  u_xlat3.y = in_f.vs_TEXCOORD0.y;
                  u_xlat18.xy = (u_xlat3.zy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_6 = tex2D(_GrabTexture, u_xlat18.xy);
                  u_xlat16_5 = ((u_xlat10_6 * float4(0.120000027, 0.120000027, 0.120000027, 0.120000027)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.120000027);
                  u_xlat16_5 = u_xlat16_5;
              }
              if(!u_xlatb4.w)
              {
                  u_xlat3.y = in_f.vs_TEXCOORD0.y;
                  u_xlat18.xy = (u_xlat3.wy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_3 = tex2D(_GrabTexture, u_xlat18.xy);
                  u_xlat16_5 = ((u_xlat10_3 * float4(0.115000024, 0.115000024, 0.115000024, 0.115000024)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.115000024);
                  u_xlat16_5 = u_xlat16_5;
              }
              u_xlat3 = ((u_xlat8.xxxx * float4(2.625, 2.5, 2.75, 2.875)) + in_f.vs_TEXCOORD0.xxxx);
              u_xlatb4 = lessThan(float4(u_xlat16_31, u_xlat16_31, u_xlat16_31, u_xlat16_31), u_xlat3.yxzw);
              u_xlatb6 = lessThan(u_xlat3.yxzw, u_xlat16_1.xxxx);
              u_xlatb4.x = (u_xlatb4.x || u_xlatb6.x);
              u_xlatb4.y = (u_xlatb4.y || u_xlatb6.y);
              u_xlatb4.z = (u_xlatb4.z || u_xlatb6.z);
              u_xlatb4.w = (u_xlatb4.w || u_xlatb6.w);
              if(!u_xlatb4.x)
              {
                  u_xlat9.x = u_xlat3.y;
                  u_xlat9.y = in_f.vs_TEXCOORD0.y;
                  u_xlat18.xy = (u_xlat9.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_6 = tex2D(_GrabTexture, u_xlat18.xy);
                  u_xlat16_5 = ((u_xlat10_6 * float4(0.110000022, 0.110000022, 0.110000022, 0.110000022)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.110000022);
                  u_xlat16_5 = u_xlat16_5;
              }
              if(!u_xlatb4.y)
              {
                  u_xlat3.y = in_f.vs_TEXCOORD0.y;
                  u_xlat18.xy = (u_xlat3.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_6 = tex2D(_GrabTexture, u_xlat18.xy);
                  u_xlat16_5 = ((u_xlat10_6 * float4(0.105000019, 0.105000019, 0.105000019, 0.105000019)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.105000019);
                  u_xlat16_5 = u_xlat16_5;
              }
              if(!u_xlatb4.z)
              {
                  u_xlat3.y = in_f.vs_TEXCOORD0.y;
                  u_xlat18.xy = (u_xlat3.zy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_6 = tex2D(_GrabTexture, u_xlat18.xy);
                  u_xlat16_5 = ((u_xlat10_6 * float4(0.100000016, 0.100000016, 0.100000016, 0.100000016)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.100000016);
                  u_xlat16_5 = u_xlat16_5;
              }
              if(!u_xlatb4.w)
              {
                  u_xlat3.y = in_f.vs_TEXCOORD0.y;
                  u_xlat18.xy = (u_xlat3.wy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_3 = tex2D(_GrabTexture, u_xlat18.xy);
                  u_xlat16_5 = ((u_xlat10_3 * float4(0.0950000137, 0.0950000137, 0.0950000137, 0.0950000137)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.0950000137);
                  u_xlat16_5 = u_xlat16_5;
              }
              u_xlat3 = ((u_xlat8.xxxx * float4(3.125, 3, 3.25, 3.375)) + in_f.vs_TEXCOORD0.xxxx);
              u_xlatb4 = lessThan(float4(u_xlat16_31, u_xlat16_31, u_xlat16_31, u_xlat16_31), u_xlat3.yxzw);
              u_xlatb6 = lessThan(u_xlat3.yxzw, u_xlat16_1.xxxx);
              u_xlatb4.x = (u_xlatb4.x || u_xlatb6.x);
              u_xlatb4.y = (u_xlatb4.y || u_xlatb6.y);
              u_xlatb4.z = (u_xlatb4.z || u_xlatb6.z);
              u_xlatb4.w = (u_xlatb4.w || u_xlatb6.w);
              if(!u_xlatb4.x)
              {
                  u_xlat9.x = u_xlat3.y;
                  u_xlat9.y = in_f.vs_TEXCOORD0.y;
                  u_xlat18.xy = (u_xlat9.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_6 = tex2D(_GrabTexture, u_xlat18.xy);
                  u_xlat16_5 = ((u_xlat10_6 * float4(0.090000011, 0.090000011, 0.090000011, 0.090000011)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.090000011);
                  u_xlat16_5 = u_xlat16_5;
              }
              if(!u_xlatb4.y)
              {
                  u_xlat3.y = in_f.vs_TEXCOORD0.y;
                  u_xlat18.xy = (u_xlat3.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_6 = tex2D(_GrabTexture, u_xlat18.xy);
                  u_xlat16_5 = ((u_xlat10_6 * float4(0.0850000083, 0.0850000083, 0.0850000083, 0.0850000083)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.0850000083);
                  u_xlat16_5 = u_xlat16_5;
              }
              if(!u_xlatb4.z)
              {
                  u_xlat3.y = in_f.vs_TEXCOORD0.y;
                  u_xlat18.xy = (u_xlat3.zy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_6 = tex2D(_GrabTexture, u_xlat18.xy);
                  u_xlat16_5 = ((u_xlat10_6 * float4(0.0800000057, 0.0800000057, 0.0800000057, 0.0800000057)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.0800000057);
                  u_xlat16_5 = u_xlat16_5;
              }
              if(!u_xlatb4.w)
              {
                  u_xlat3.y = in_f.vs_TEXCOORD0.y;
                  u_xlat18.xy = (u_xlat3.wy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_3 = tex2D(_GrabTexture, u_xlat18.xy);
                  u_xlat16_5 = ((u_xlat10_3 * float4(0.075000003, 0.075000003, 0.075000003, 0.075000003)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.075000003);
                  u_xlat16_5 = u_xlat16_5;
              }
              u_xlat3 = ((u_xlat8.xxxx * float4(3.625, 3.5, 3.75, 3.875)) + in_f.vs_TEXCOORD0.xxxx);
              u_xlatb4 = lessThan(float4(u_xlat16_31, u_xlat16_31, u_xlat16_31, u_xlat16_31), u_xlat3.yxzw);
              u_xlatb6 = lessThan(u_xlat3.yxzw, u_xlat16_1.xxxx);
              u_xlatb4.x = (u_xlatb4.x || u_xlatb6.x);
              u_xlatb4.y = (u_xlatb4.y || u_xlatb6.y);
              u_xlatb4.z = (u_xlatb4.z || u_xlatb6.z);
              u_xlatb4.w = (u_xlatb4.w || u_xlatb6.w);
              if(!u_xlatb4.x)
              {
                  u_xlat9.x = u_xlat3.y;
                  u_xlat9.y = in_f.vs_TEXCOORD0.y;
                  u_xlat18.xy = (u_xlat9.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_6 = tex2D(_GrabTexture, u_xlat18.xy);
                  u_xlat16_5 = ((u_xlat10_6 * float4(0.0700000003, 0.0700000003, 0.0700000003, 0.0700000003)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.0700000003);
                  u_xlat16_5 = u_xlat16_5;
              }
              if(!u_xlatb4.y)
              {
                  u_xlat3.y = in_f.vs_TEXCOORD0.y;
                  u_xlat18.xy = (u_xlat3.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_6 = tex2D(_GrabTexture, u_xlat18.xy);
                  u_xlat16_5 = ((u_xlat10_6 * float4(0.0649999976, 0.0649999976, 0.0649999976, 0.0649999976)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.0649999976);
                  u_xlat16_5 = u_xlat16_5;
              }
              if(!u_xlatb4.z)
              {
                  u_xlat3.y = in_f.vs_TEXCOORD0.y;
                  u_xlat18.xy = (u_xlat3.zy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_6 = tex2D(_GrabTexture, u_xlat18.xy);
                  u_xlat16_5 = ((u_xlat10_6 * float4(0.0599999987, 0.0599999987, 0.0599999987, 0.0599999987)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.0599999987);
                  u_xlat16_5 = u_xlat16_5;
              }
              if(!u_xlatb4.w)
              {
                  u_xlat3.y = in_f.vs_TEXCOORD0.y;
                  u_xlat18.xy = (u_xlat3.wy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_3 = tex2D(_GrabTexture, u_xlat18.xy);
                  u_xlat16_5 = ((u_xlat10_3 * float4(0.0549999997, 0.0549999997, 0.0549999997, 0.0549999997)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.0549999997);
                  u_xlat16_5 = u_xlat16_5;
              }
              u_xlat8.x = ((u_xlat8.x * 4) + in_f.vs_TEXCOORD0.x);
              if((u_xlat16_31<u_xlat8.x))
              {
                  u_xlatb28 = 1;
              }
              else u_xlatb28 = 0;
              if((u_xlat8.x<u_xlat16_1.x))
              {
                  u_xlatb38 = 1;
              }
              else u_xlatb38 = 0;
              u_xlatb28 = (u_xlatb38 || u_xlatb28);
              if(!u_xlatb28)
              {
                  u_xlat8.y = in_f.vs_TEXCOORD0.y;
                  u_xlat8.xy = (u_xlat8.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_3 = tex2D(_GrabTexture, u_xlat8.xy);
                  u_xlat16_5 = ((u_xlat10_3 * float4(0.0500000007, 0.0500000007, 0.0500000007, 0.0500000007)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.0500000007);
                  u_xlat16_5 = u_xlat16_5;
              }
          }
          else
          {
              u_xlat16_5.x = float(0);
              u_xlat16_5.y = float(0);
              u_xlat16_5.z = float(0);
              u_xlat16_5.w = float(0);
              u_xlat16_11 = 0;
          }
          u_xlat8.x = (((-u_xlat10_0) * _Lightness) + 1);
          u_xlat8.x = (u_xlat16_11 * u_xlat8.x);
          u_xlat1 = (u_xlat16_5 / u_xlat8.xxxx);
          u_xlat8.x = (u_xlat10_0 * _Color.w);
          u_xlat1.xyz = ((u_xlat8.xxx * _Color.xyz) + u_xlat1.xyz);
          u_xlat16_1 = ((-u_xlat10_2) + u_xlat1);
          out_f.SV_Target0 = ((float4(u_xlat10_0, u_xlat10_0, u_xlat10_0, u_xlat10_0) * u_xlat16_1) + u_xlat10_2);
          return out_f;
          0.5;
          _PanelWidth;
          float2(0, 0);
          float2(_ScreenWidth, _PanelX);
      }
      
      
      ENDCG
      
    } // end phase
    Pass // ind: 3, name: 
    {
      Tags
      { 
      }
      ZClip Off
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
      GpuProgramID -1
      // m_ProgramMask = 0
      
    } // end phase
    Pass // ind: 4, name: 
    {
      Tags
      { 
        "CanUseSpriteAtlas" = "true"
        "LIGHTMODE" = "ALWAYS"
        "PreviewType" = "Plane"
        "QUEUE" = "Transparent"
        "RenderType" = "Transparent"
      }
      GpuProgramID 73180
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
      uniform float4 _GrabTexture_TexelSize;
      uniform float _BlurSize;
      uniform float _Lightness;
      uniform float4 _Color;
      uniform int _Quality;
      uniform float _ScreenHeight;
      uniform float _PanelY;
      uniform float _PanelHeight;
      uniform float _WorldSpace;
      uniform sampler2D _MainTex;
      uniform sampler2D _GrabTexture;
      struct IN_Data_Vert
      {
          float4 in_POSITION0 :POSITION0;
          float2 in_TEXCOORD0 :TEXCOORD0;
      };
      
      struct OUT_Data_Vert
      {
          float4 vs_TEXCOORD0 :TEXCOORD0;
          float2 vs_TEXCOORD1 :TEXCOORD1;
          float4 gl_Position :SV_POSITION;
      };
      
      struct IN_Data_Frag
      {
          float4 vs_TEXCOORD0 :TEXCOORD0;
          float2 vs_TEXCOORD1 :TEXCOORD1;
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
          u_xlat0 = ((conv_mxt4x4_3(unity_MatrixVP) * u_xlat0.wwww) + u_xlat1);
          out_v.gl_Position = u_xlat0;
          u_xlat0.xy = (u_xlat0.ww + u_xlat0.xy);
          out_v.vs_TEXCOORD0.zw = u_xlat0.zw;
          out_v.vs_TEXCOORD0.xy = (u_xlat0.xy * float2(0.5, 0.5));
          out_v.vs_TEXCOORD1.xy = ((in_v.in_TEXCOORD0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      float u_xlat10_0;
      int u_xlatb0;
      float4 u_xlat16_1;
      float4 u_xlat10_2;
      float4 u_xlat3;
      float4 u_xlat16_3;
      float4 u_xlat10_3;
      float2 u_xlat4;
      bool4 u_xlatb4;
      float4 u_xlat16_5;
      bool4 u_xlatb5;
      float2 u_xlat6;
      float4 u_xlat16_6;
      float4 u_xlat10_6;
      bool4 u_xlatb6;
      float2 u_xlat7;
      float4 u_xlat16_7;
      float4 u_xlat10_7;
      bool4 u_xlatb7;
      float2 u_xlat8;
      int u_xlatb8;
      float2 u_xlat9;
      float2 u_xlat10;
      int u_xlatb10;
      float u_xlat16_11;
      float2 u_xlat18;
      int u_xlatb18;
      float2 u_xlat20;
      int u_xlatb20;
      float u_xlat16_21;
      int u_xlatb28;
      int u_xlatb30;
      float u_xlat16_31;
      int u_xlatb38;
      OUT_Data_frag frag(IN_Data_Frag in_f)
      {
          OUT_Data_frag out_f;
          if((_WorldSpace==1))
          {
              u_xlatb0 = 1;
          }
          else u_xlatb0 = 0;
          u_xlat16_1.xy = int(u_xlatb0);
          u_xlat16_21 = u_xlatb0;
          u_xlat10_0 = tex2D(_MainTex, in_f.vs_TEXCOORD1.xy).w;
          u_xlat10.xy = (in_f.vs_TEXCOORD0.xy / in_f.vs_TEXCOORD0.ww);
          u_xlat10_2 = tex2D(_GrabTexture, u_xlat10.xy);
          u_xlat16_31 = ((u_xlat16_21 * 0.5) + u_xlat16_1.y);
          u_xlat16_31 = (u_xlat16_31 / u_xlat16_1.x);
          u_xlat16_11 = (((-u_xlat16_21) * 0.5) + u_xlat16_1.y);
          u_xlat16_1.x = (u_xlat16_11 / u_xlat16_1.x);
          if((_Quality==0))
          {
              u_xlat10.x = (_GrabTexture_TexelSize.y * _BlurSize);
              u_xlat3 = ((u_xlat10.xxxx * float4(-4, (-3), (-2), 3)) + in_f.vs_TEXCOORD0.yyyy);
              u_xlatb4 = lessThan(float4(u_xlat16_31, u_xlat16_31, u_xlat16_31, u_xlat16_31), u_xlat3);
              u_xlatb5 = lessThan(u_xlat3, u_xlat16_1.xxxx);
              u_xlatb4.x = (u_xlatb4.x || u_xlatb5.x);
              u_xlatb4.y = (u_xlatb4.y || u_xlatb5.y);
              u_xlatb4.z = (u_xlatb4.z || u_xlatb5.z);
              u_xlatb4.w = (u_xlatb4.w || u_xlatb5.w);
              if(u_xlatb4.x)
              {
                  u_xlat16_5.x = float(0);
                  u_xlat16_5.y = float(0);
                  u_xlat16_5.z = float(0);
                  u_xlat16_5.w = float(0);
                  u_xlat16_11 = 0;
              }
              else
              {
                  u_xlat6.x = in_f.vs_TEXCOORD0.x;
                  u_xlat6.y = u_xlat3.x;
                  u_xlat20.xy = (u_xlat6.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_6 = tex2D(_GrabTexture, u_xlat20.xy);
                  u_xlat16_6 = (u_xlat10_6 * float4(0.0500000007, 0.0500000007, 0.0500000007, 0.0500000007));
                  u_xlat16_5 = u_xlat16_6;
                  u_xlat16_11 = 0.0500000007;
              }
              if(!u_xlatb4.y)
              {
                  u_xlat3.x = in_f.vs_TEXCOORD0.x;
                  u_xlat20.xy = (u_xlat3.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_6 = tex2D(_GrabTexture, u_xlat20.xy);
                  u_xlat16_6 = ((u_xlat10_6 * float4(0.0900000036, 0.0900000036, 0.0900000036, 0.0900000036)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.0900000036);
                  u_xlat16_5 = u_xlat16_6;
              }
              if(!u_xlatb4.z)
              {
                  u_xlat3.x = in_f.vs_TEXCOORD0.x;
                  u_xlat20.xy = (u_xlat3.xz / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_6 = tex2D(_GrabTexture, u_xlat20.xy);
                  u_xlat16_6 = ((u_xlat10_6 * float4(0.129999995, 0.129999995, 0.129999995, 0.129999995)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.129999995);
                  u_xlat16_5 = u_xlat16_6;
              }
              u_xlat4.y = (((-_GrabTexture_TexelSize.y) * _BlurSize) + in_f.vs_TEXCOORD0.y);
              if((u_xlat16_31<u_xlat4.y))
              {
                  u_xlatb20 = 1;
              }
              else u_xlatb20 = 0;
              if((u_xlat4.y<u_xlat16_1.x))
              {
                  u_xlatb30 = 1;
              }
              else u_xlatb30 = 0;
              u_xlatb20 = (u_xlatb30 || u_xlatb20);
              if(!u_xlatb20)
              {
                  u_xlat4.x = in_f.vs_TEXCOORD0.x;
                  u_xlat20.xy = (u_xlat4.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_6 = tex2D(_GrabTexture, u_xlat20.xy);
                  u_xlat16_6 = ((u_xlat10_6 * float4(0.169999987, 0.169999987, 0.169999987, 0.169999987)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.169999987);
                  u_xlat16_5 = u_xlat16_6;
              }
              if((u_xlat16_31<in_f.vs_TEXCOORD0.y))
              {
                  u_xlatb20 = 1;
              }
              else u_xlatb20 = 0;
              if((in_f.vs_TEXCOORD0.y<u_xlat16_1.x))
              {
                  u_xlatb30 = 1;
              }
              else u_xlatb30 = 0;
              u_xlatb20 = (u_xlatb30 || u_xlatb20);
              if(!u_xlatb20)
              {
                  u_xlat16_6 = ((u_xlat10_2 * float4(0.209999979, 0.209999979, 0.209999979, 0.209999979)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.209999979);
                  u_xlat16_5 = u_xlat16_6;
              }
              u_xlat4.y = ((_GrabTexture_TexelSize.y * _BlurSize) + in_f.vs_TEXCOORD0.y);
              if((u_xlat16_31<u_xlat4.y))
              {
                  u_xlatb20 = 1;
              }
              else u_xlatb20 = 0;
              if((u_xlat4.y<u_xlat16_1.x))
              {
                  u_xlatb30 = 1;
              }
              else u_xlatb30 = 0;
              u_xlatb20 = (u_xlatb30 || u_xlatb20);
              if(!u_xlatb20)
              {
                  u_xlat4.x = in_f.vs_TEXCOORD0.x;
                  u_xlat20.xy = (u_xlat4.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_6 = tex2D(_GrabTexture, u_xlat20.xy);
                  u_xlat16_6 = ((u_xlat10_6 * float4(0.169999987, 0.169999987, 0.169999987, 0.169999987)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.169999987);
                  u_xlat16_5 = u_xlat16_6;
              }
              u_xlat4.y = ((u_xlat10.x * 2) + in_f.vs_TEXCOORD0.y);
              if((u_xlat16_31<u_xlat4.y))
              {
                  u_xlatb20 = 1;
              }
              else u_xlatb20 = 0;
              if((u_xlat4.y<u_xlat16_1.x))
              {
                  u_xlatb30 = 1;
              }
              else u_xlatb30 = 0;
              u_xlatb20 = (u_xlatb30 || u_xlatb20);
              if(!u_xlatb20)
              {
                  u_xlat4.x = in_f.vs_TEXCOORD0.x;
                  u_xlat20.xy = (u_xlat4.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_6 = tex2D(_GrabTexture, u_xlat20.xy);
                  u_xlat16_6 = ((u_xlat10_6 * float4(0.129999995, 0.129999995, 0.129999995, 0.129999995)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.129999995);
                  u_xlat16_5 = u_xlat16_6;
              }
              if(!u_xlatb4.w)
              {
                  u_xlat3.x = in_f.vs_TEXCOORD0.x;
                  u_xlat20.xy = (u_xlat3.xw / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_3 = tex2D(_GrabTexture, u_xlat20.xy);
                  u_xlat16_3 = ((u_xlat10_3 * float4(0.0899999961, 0.0899999961, 0.0899999961, 0.0899999961)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.0899999961);
                  u_xlat16_5 = u_xlat16_3;
              }
              u_xlat3.y = ((u_xlat10.x * 4) + in_f.vs_TEXCOORD0.y);
              if((u_xlat16_31<u_xlat3.y))
              {
                  u_xlatb10 = 1;
              }
              else u_xlatb10 = 0;
              if((u_xlat3.y<u_xlat16_1.x))
              {
                  u_xlatb20 = 1;
              }
              else u_xlatb20 = 0;
              u_xlatb10 = (u_xlatb20 || u_xlatb10);
              if(!u_xlatb10)
              {
                  u_xlat3.x = in_f.vs_TEXCOORD0.x;
                  u_xlat10.xy = (u_xlat3.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_3 = tex2D(_GrabTexture, u_xlat10.xy);
                  u_xlat16_3 = ((u_xlat10_3 * float4(0.049999997, 0.049999997, 0.049999997, 0.049999997)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.049999997);
                  u_xlat16_5 = u_xlat16_3;
              }
          }
          if((_Quality==1))
          {
              u_xlat10.x = (_GrabTexture_TexelSize.y * _BlurSize);
              u_xlat3 = ((u_xlat10.xxxx * float4(-4, (-3.5), (-3), (-2.5))) + in_f.vs_TEXCOORD0.yyyy);
              u_xlatb4 = lessThan(float4(u_xlat16_31, u_xlat16_31, u_xlat16_31, u_xlat16_31), u_xlat3);
              u_xlatb6 = lessThan(u_xlat3, u_xlat16_1.xxxx);
              u_xlatb4.x = (u_xlatb4.x || u_xlatb6.x);
              u_xlatb4.y = (u_xlatb4.y || u_xlatb6.y);
              u_xlatb4.z = (u_xlatb4.z || u_xlatb6.z);
              u_xlatb4.w = (u_xlatb4.w || u_xlatb6.w);
              if(u_xlatb4.x)
              {
                  u_xlat16_5.x = float(0);
                  u_xlat16_5.y = float(0);
                  u_xlat16_5.z = float(0);
                  u_xlat16_5.w = float(0);
                  u_xlat16_11 = 0;
              }
              else
              {
                  u_xlat6.x = in_f.vs_TEXCOORD0.x;
                  u_xlat6.y = u_xlat3.x;
                  u_xlat20.xy = (u_xlat6.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_6 = tex2D(_GrabTexture, u_xlat20.xy);
                  u_xlat16_6 = (u_xlat10_6 * float4(0.0500000007, 0.0500000007, 0.0500000007, 0.0500000007));
                  u_xlat16_5 = u_xlat16_6;
                  u_xlat16_11 = 0.0500000007;
              }
              if(!u_xlatb4.y)
              {
                  u_xlat3.x = in_f.vs_TEXCOORD0.x;
                  u_xlat20.xy = (u_xlat3.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_6 = tex2D(_GrabTexture, u_xlat20.xy);
                  u_xlat16_6 = ((u_xlat10_6 * float4(0.0700000003, 0.0700000003, 0.0700000003, 0.0700000003)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.0700000003);
                  u_xlat16_5 = u_xlat16_6;
              }
              if(!u_xlatb4.z)
              {
                  u_xlat3.x = in_f.vs_TEXCOORD0.x;
                  u_xlat20.xy = (u_xlat3.xz / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_6 = tex2D(_GrabTexture, u_xlat20.xy);
                  u_xlat16_6 = ((u_xlat10_6 * float4(0.0900000036, 0.0900000036, 0.0900000036, 0.0900000036)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.0900000036);
                  u_xlat16_5 = u_xlat16_6;
              }
              if(!u_xlatb4.w)
              {
                  u_xlat3.x = in_f.vs_TEXCOORD0.x;
                  u_xlat20.xy = (u_xlat3.xw / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_3 = tex2D(_GrabTexture, u_xlat20.xy);
                  u_xlat16_3 = ((u_xlat10_3 * float4(0.109999999, 0.109999999, 0.109999999, 0.109999999)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.109999999);
                  u_xlat16_5 = u_xlat16_3;
              }
              u_xlat3 = ((u_xlat10.xxxx * float4(-2, (-1.5), (-0.5), 0.5)) + in_f.vs_TEXCOORD0.yyyy);
              u_xlatb4 = lessThan(float4(u_xlat16_31, u_xlat16_31, u_xlat16_31, u_xlat16_31), u_xlat3);
              u_xlatb6 = lessThan(u_xlat3, u_xlat16_1.xxxx);
              u_xlatb4.x = (u_xlatb4.x || u_xlatb6.x);
              u_xlatb4.y = (u_xlatb4.y || u_xlatb6.y);
              u_xlatb4.z = (u_xlatb4.z || u_xlatb6.z);
              u_xlatb4.w = (u_xlatb4.w || u_xlatb6.w);
              if(!u_xlatb4.x)
              {
                  u_xlat6.x = in_f.vs_TEXCOORD0.x;
                  u_xlat6.y = u_xlat3.x;
                  u_xlat20.xy = (u_xlat6.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_6 = tex2D(_GrabTexture, u_xlat20.xy);
                  u_xlat16_6 = ((u_xlat10_6 * float4(0.129999995, 0.129999995, 0.129999995, 0.129999995)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.129999995);
                  u_xlat16_5 = u_xlat16_6;
              }
              if(!u_xlatb4.y)
              {
                  u_xlat3.x = in_f.vs_TEXCOORD0.x;
                  u_xlat20.xy = (u_xlat3.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_6 = tex2D(_GrabTexture, u_xlat20.xy);
                  u_xlat16_6 = ((u_xlat10_6 * float4(0.149999991, 0.149999991, 0.149999991, 0.149999991)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.149999991);
                  u_xlat16_5 = u_xlat16_6;
              }
              u_xlat4.y = (((-_GrabTexture_TexelSize.y) * _BlurSize) + in_f.vs_TEXCOORD0.y);
              if((u_xlat16_31<u_xlat4.y))
              {
                  u_xlatb20 = 1;
              }
              else u_xlatb20 = 0;
              if((u_xlat4.y<u_xlat16_1.x))
              {
                  u_xlatb30 = 1;
              }
              else u_xlatb30 = 0;
              u_xlatb20 = (u_xlatb30 || u_xlatb20);
              if(!u_xlatb20)
              {
                  u_xlat4.x = in_f.vs_TEXCOORD0.x;
                  u_xlat20.xy = (u_xlat4.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_6 = tex2D(_GrabTexture, u_xlat20.xy);
                  u_xlat16_6 = ((u_xlat10_6 * float4(0.169999987, 0.169999987, 0.169999987, 0.169999987)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.169999987);
                  u_xlat16_5 = u_xlat16_6;
              }
              if(!u_xlatb4.z)
              {
                  u_xlat3.x = in_f.vs_TEXCOORD0.x;
                  u_xlat20.xy = (u_xlat3.xz / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_6 = tex2D(_GrabTexture, u_xlat20.xy);
                  u_xlat16_6 = ((u_xlat10_6 * float4(0.189999983, 0.189999983, 0.189999983, 0.189999983)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.189999983);
                  u_xlat16_5 = u_xlat16_6;
              }
              if((u_xlat16_31<in_f.vs_TEXCOORD0.y))
              {
                  u_xlatb20 = 1;
              }
              else u_xlatb20 = 0;
              if((in_f.vs_TEXCOORD0.y<u_xlat16_1.x))
              {
                  u_xlatb30 = 1;
              }
              else u_xlatb30 = 0;
              u_xlatb20 = (u_xlatb30 || u_xlatb20);
              if(!u_xlatb20)
              {
                  u_xlat16_6 = ((u_xlat10_2 * float4(0.209999979, 0.209999979, 0.209999979, 0.209999979)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.209999979);
                  u_xlat16_5 = u_xlat16_6;
              }
              if(!u_xlatb4.w)
              {
                  u_xlat3.x = in_f.vs_TEXCOORD0.x;
                  u_xlat20.xy = (u_xlat3.xw / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_3 = tex2D(_GrabTexture, u_xlat20.xy);
                  u_xlat16_3 = ((u_xlat10_3 * float4(0.189999983, 0.189999983, 0.189999983, 0.189999983)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.189999983);
                  u_xlat16_5 = u_xlat16_3;
              }
              u_xlat3.y = ((_GrabTexture_TexelSize.y * _BlurSize) + in_f.vs_TEXCOORD0.y);
              if((u_xlat16_31<u_xlat3.y))
              {
                  u_xlatb20 = 1;
              }
              else u_xlatb20 = 0;
              if((u_xlat3.y<u_xlat16_1.x))
              {
                  u_xlatb30 = 1;
              }
              else u_xlatb30 = 0;
              u_xlatb20 = (u_xlatb30 || u_xlatb20);
              if(!u_xlatb20)
              {
                  u_xlat3.x = in_f.vs_TEXCOORD0.x;
                  u_xlat20.xy = (u_xlat3.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_3 = tex2D(_GrabTexture, u_xlat20.xy);
                  u_xlat16_3 = ((u_xlat10_3 * float4(0.169999987, 0.169999987, 0.169999987, 0.169999987)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.169999987);
                  u_xlat16_5 = u_xlat16_3;
              }
              u_xlat3 = ((u_xlat10.xxxx * float4(1.5, 2.5, 3, 3.5)) + in_f.vs_TEXCOORD0.yyyy);
              u_xlatb4 = lessThan(float4(u_xlat16_31, u_xlat16_31, u_xlat16_31, u_xlat16_31), u_xlat3);
              u_xlatb6 = lessThan(u_xlat3, u_xlat16_1.xxxx);
              u_xlatb4.x = (u_xlatb4.x || u_xlatb6.x);
              u_xlatb4.y = (u_xlatb4.y || u_xlatb6.y);
              u_xlatb4.z = (u_xlatb4.z || u_xlatb6.z);
              u_xlatb4.w = (u_xlatb4.w || u_xlatb6.w);
              if(!u_xlatb4.x)
              {
                  u_xlat6.x = in_f.vs_TEXCOORD0.x;
                  u_xlat6.y = u_xlat3.x;
                  u_xlat20.xy = (u_xlat6.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_6 = tex2D(_GrabTexture, u_xlat20.xy);
                  u_xlat16_6 = ((u_xlat10_6 * float4(0.149999991, 0.149999991, 0.149999991, 0.149999991)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.149999991);
                  u_xlat16_5 = u_xlat16_6;
              }
              u_xlat6.y = ((u_xlat10.x * 2) + in_f.vs_TEXCOORD0.y);
              if((u_xlat16_31<u_xlat6.y))
              {
                  u_xlatb20 = 1;
              }
              else u_xlatb20 = 0;
              if((u_xlat6.y<u_xlat16_1.x))
              {
                  u_xlatb30 = 1;
              }
              else u_xlatb30 = 0;
              u_xlatb20 = (u_xlatb30 || u_xlatb20);
              if(!u_xlatb20)
              {
                  u_xlat6.x = in_f.vs_TEXCOORD0.x;
                  u_xlat20.xy = (u_xlat6.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_6 = tex2D(_GrabTexture, u_xlat20.xy);
                  u_xlat16_6 = ((u_xlat10_6 * float4(0.129999995, 0.129999995, 0.129999995, 0.129999995)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.129999995);
                  u_xlat16_5 = u_xlat16_6;
              }
              if(!u_xlatb4.y)
              {
                  u_xlat3.x = in_f.vs_TEXCOORD0.x;
                  u_xlat20.xy = (u_xlat3.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_6 = tex2D(_GrabTexture, u_xlat20.xy);
                  u_xlat16_6 = ((u_xlat10_6 * float4(0.109999999, 0.109999999, 0.109999999, 0.109999999)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.109999999);
                  u_xlat16_5 = u_xlat16_6;
              }
              if(!u_xlatb4.z)
              {
                  u_xlat3.x = in_f.vs_TEXCOORD0.x;
                  u_xlat20.xy = (u_xlat3.xz / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_6 = tex2D(_GrabTexture, u_xlat20.xy);
                  u_xlat16_6 = ((u_xlat10_6 * float4(0.0900000036, 0.0900000036, 0.0900000036, 0.0900000036)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.0900000036);
                  u_xlat16_5 = u_xlat16_6;
              }
              if(!u_xlatb4.w)
              {
                  u_xlat3.x = in_f.vs_TEXCOORD0.x;
                  u_xlat20.xy = (u_xlat3.xw / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_3 = tex2D(_GrabTexture, u_xlat20.xy);
                  u_xlat16_3 = ((u_xlat10_3 * float4(0.0700000077, 0.0700000077, 0.0700000077, 0.0700000077)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.0700000077);
                  u_xlat16_5 = u_xlat16_3;
              }
              u_xlat3.y = ((u_xlat10.x * 4) + in_f.vs_TEXCOORD0.y);
              if((u_xlat16_31<u_xlat3.y))
              {
                  u_xlatb10 = 1;
              }
              else u_xlatb10 = 0;
              if((u_xlat3.y<u_xlat16_1.x))
              {
                  u_xlatb20 = 1;
              }
              else u_xlatb20 = 0;
              u_xlatb10 = (u_xlatb20 || u_xlatb10);
              if(!u_xlatb10)
              {
                  u_xlat3.x = in_f.vs_TEXCOORD0.x;
                  u_xlat4.xy = (u_xlat3.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_3 = tex2D(_GrabTexture, u_xlat4.xy);
                  u_xlat16_3 = ((u_xlat10_3 * float4(0.0500000082, 0.0500000082, 0.0500000082, 0.0500000082)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.0500000082);
                  u_xlat16_5 = u_xlat16_3;
              }
          }
          if((_Quality==2))
          {
              u_xlat4.x = (_GrabTexture_TexelSize.y * _BlurSize);
              u_xlat3 = ((u_xlat4.xxxx * float4(-4, (-3.75), (-3.5), (-3.25))) + in_f.vs_TEXCOORD0.yyyy);
              u_xlatb6 = lessThan(float4(u_xlat16_31, u_xlat16_31, u_xlat16_31, u_xlat16_31), u_xlat3);
              u_xlatb7 = lessThan(u_xlat3, u_xlat16_1.xxxx);
              u_xlatb6.x = (u_xlatb6.x || u_xlatb7.x);
              u_xlatb6.y = (u_xlatb6.y || u_xlatb7.y);
              u_xlatb6.z = (u_xlatb6.z || u_xlatb7.z);
              u_xlatb6.w = (u_xlatb6.w || u_xlatb7.w);
              if(u_xlatb6.x)
              {
                  u_xlat16_5.x = float(0);
                  u_xlat16_5.y = float(0);
                  u_xlat16_5.z = float(0);
                  u_xlat16_5.w = float(0);
                  u_xlat16_11 = 0;
              }
              else
              {
                  u_xlat7.x = in_f.vs_TEXCOORD0.x;
                  u_xlat7.y = u_xlat3.x;
                  u_xlat7.xy = (u_xlat7.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_7 = tex2D(_GrabTexture, u_xlat7.xy);
                  u_xlat16_7 = (u_xlat10_7 * float4(0.0500000007, 0.0500000007, 0.0500000007, 0.0500000007));
                  u_xlat16_5 = u_xlat16_7;
                  u_xlat16_11 = 0.0500000007;
              }
              if(!u_xlatb6.y)
              {
                  u_xlat3.x = in_f.vs_TEXCOORD0.x;
                  u_xlat8.xy = (u_xlat3.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_7 = tex2D(_GrabTexture, u_xlat8.xy);
                  u_xlat16_7 = ((u_xlat10_7 * float4(0.0600000024, 0.0600000024, 0.0600000024, 0.0600000024)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.0600000024);
                  u_xlat16_5 = u_xlat16_7;
              }
              if(!u_xlatb6.z)
              {
                  u_xlat3.x = in_f.vs_TEXCOORD0.x;
                  u_xlat8.xy = (u_xlat3.xz / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_7 = tex2D(_GrabTexture, u_xlat8.xy);
                  u_xlat16_7 = ((u_xlat10_7 * float4(0.0700000003, 0.0700000003, 0.0700000003, 0.0700000003)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.0700000003);
                  u_xlat16_5 = u_xlat16_7;
              }
              if(!u_xlatb6.w)
              {
                  u_xlat3.x = in_f.vs_TEXCOORD0.x;
                  u_xlat8.xy = (u_xlat3.xw / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_3 = tex2D(_GrabTexture, u_xlat8.xy);
                  u_xlat16_3 = ((u_xlat10_3 * float4(0.0799999982, 0.0799999982, 0.0799999982, 0.0799999982)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.0799999982);
                  u_xlat16_5 = u_xlat16_3;
              }
              u_xlat3 = ((u_xlat4.xxxx * float4(-3, (-2.75), (-2.5), (-2.25))) + in_f.vs_TEXCOORD0.yyyy);
              u_xlatb6 = lessThan(float4(u_xlat16_31, u_xlat16_31, u_xlat16_31, u_xlat16_31), u_xlat3);
              u_xlatb7 = lessThan(u_xlat3, u_xlat16_1.xxxx);
              u_xlatb6.x = (u_xlatb6.x || u_xlatb7.x);
              u_xlatb6.y = (u_xlatb6.y || u_xlatb7.y);
              u_xlatb6.z = (u_xlatb6.z || u_xlatb7.z);
              u_xlatb6.w = (u_xlatb6.w || u_xlatb7.w);
              if(!u_xlatb6.x)
              {
                  u_xlat8.x = in_f.vs_TEXCOORD0.x;
                  u_xlat8.y = u_xlat3.x;
                  u_xlat8.xy = (u_xlat8.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_7 = tex2D(_GrabTexture, u_xlat8.xy);
                  u_xlat16_7 = ((u_xlat10_7 * float4(0.0899999961, 0.0899999961, 0.0899999961, 0.0899999961)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.0899999961);
                  u_xlat16_5 = u_xlat16_7;
              }
              if(!u_xlatb6.y)
              {
                  u_xlat3.x = in_f.vs_TEXCOORD0.x;
                  u_xlat8.xy = (u_xlat3.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_7 = tex2D(_GrabTexture, u_xlat8.xy);
                  u_xlat16_7 = ((u_xlat10_7 * float4(0.099999994, 0.099999994, 0.099999994, 0.099999994)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.099999994);
                  u_xlat16_5 = u_xlat16_7;
              }
              if(!u_xlatb6.z)
              {
                  u_xlat3.x = in_f.vs_TEXCOORD0.x;
                  u_xlat8.xy = (u_xlat3.xz / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_7 = tex2D(_GrabTexture, u_xlat8.xy);
                  u_xlat16_7 = ((u_xlat10_7 * float4(0.109999992, 0.109999992, 0.109999992, 0.109999992)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.109999992);
                  u_xlat16_5 = u_xlat16_7;
              }
              if(!u_xlatb6.w)
              {
                  u_xlat3.x = in_f.vs_TEXCOORD0.x;
                  u_xlat8.xy = (u_xlat3.xw / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_3 = tex2D(_GrabTexture, u_xlat8.xy);
                  u_xlat16_3 = ((u_xlat10_3 * float4(0.11999999, 0.11999999, 0.11999999, 0.11999999)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.11999999);
                  u_xlat16_5 = u_xlat16_3;
              }
              u_xlat3 = ((u_xlat4.xxxx * float4(-2, (-1.75), (-1.5), (-1.25))) + in_f.vs_TEXCOORD0.yyyy);
              u_xlatb6 = lessThan(float4(u_xlat16_31, u_xlat16_31, u_xlat16_31, u_xlat16_31), u_xlat3);
              u_xlatb7 = lessThan(u_xlat3, u_xlat16_1.xxxx);
              u_xlatb6.x = (u_xlatb6.x || u_xlatb7.x);
              u_xlatb6.y = (u_xlatb6.y || u_xlatb7.y);
              u_xlatb6.z = (u_xlatb6.z || u_xlatb7.z);
              u_xlatb6.w = (u_xlatb6.w || u_xlatb7.w);
              if(!u_xlatb6.x)
              {
                  u_xlat8.x = in_f.vs_TEXCOORD0.x;
                  u_xlat8.y = u_xlat3.x;
                  u_xlat8.xy = (u_xlat8.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_7 = tex2D(_GrabTexture, u_xlat8.xy);
                  u_xlat16_7 = ((u_xlat10_7 * float4(0.129999995, 0.129999995, 0.129999995, 0.129999995)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.129999995);
                  u_xlat16_5 = u_xlat16_7;
              }
              if(!u_xlatb6.y)
              {
                  u_xlat3.x = in_f.vs_TEXCOORD0.x;
                  u_xlat8.xy = (u_xlat3.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_7 = tex2D(_GrabTexture, u_xlat8.xy);
                  u_xlat16_7 = ((u_xlat10_7 * float4(0.140000001, 0.140000001, 0.140000001, 0.140000001)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.140000001);
                  u_xlat16_5 = u_xlat16_7;
              }
              if(!u_xlatb6.z)
              {
                  u_xlat3.x = in_f.vs_TEXCOORD0.x;
                  u_xlat8.xy = (u_xlat3.xz / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_7 = tex2D(_GrabTexture, u_xlat8.xy);
                  u_xlat16_7 = ((u_xlat10_7 * float4(0.150000006, 0.150000006, 0.150000006, 0.150000006)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.150000006);
                  u_xlat16_5 = u_xlat16_7;
              }
              if(!u_xlatb6.w)
              {
                  u_xlat3.x = in_f.vs_TEXCOORD0.x;
                  u_xlat8.xy = (u_xlat3.xw / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_3 = tex2D(_GrabTexture, u_xlat8.xy);
                  u_xlat16_3 = ((u_xlat10_3 * float4(0.160000011, 0.160000011, 0.160000011, 0.160000011)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.160000011);
                  u_xlat16_5 = u_xlat16_3;
              }
              u_xlat8.y = (((-_GrabTexture_TexelSize.y) * _BlurSize) + in_f.vs_TEXCOORD0.y);
              if((u_xlat16_31<u_xlat8.y))
              {
                  u_xlatb28 = 1;
              }
              else u_xlatb28 = 0;
              if((u_xlat8.y<u_xlat16_1.x))
              {
                  u_xlatb38 = 1;
              }
              else u_xlatb38 = 0;
              u_xlatb28 = (u_xlatb38 || u_xlatb28);
              if(!u_xlatb28)
              {
                  u_xlat8.x = in_f.vs_TEXCOORD0.x;
                  u_xlat8.xy = (u_xlat8.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_3 = tex2D(_GrabTexture, u_xlat8.xy);
                  u_xlat16_3 = ((u_xlat10_3 * float4(0.170000017, 0.170000017, 0.170000017, 0.170000017)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.170000017);
                  u_xlat16_5 = u_xlat16_3;
              }
              u_xlat3 = ((u_xlat4.xxxx * float4(-0.75, (-0.5), (-0.25), 0.25)) + in_f.vs_TEXCOORD0.yyyy);
              u_xlatb6 = lessThan(float4(u_xlat16_31, u_xlat16_31, u_xlat16_31, u_xlat16_31), u_xlat3);
              u_xlatb7 = lessThan(u_xlat3, u_xlat16_1.xxxx);
              u_xlatb6.x = (u_xlatb6.x || u_xlatb7.x);
              u_xlatb6.y = (u_xlatb6.y || u_xlatb7.y);
              u_xlatb6.z = (u_xlatb6.z || u_xlatb7.z);
              u_xlatb6.w = (u_xlatb6.w || u_xlatb7.w);
              if(!u_xlatb6.x)
              {
                  u_xlat8.x = in_f.vs_TEXCOORD0.x;
                  u_xlat8.y = u_xlat3.x;
                  u_xlat8.xy = (u_xlat8.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_7 = tex2D(_GrabTexture, u_xlat8.xy);
                  u_xlat16_7 = ((u_xlat10_7 * float4(0.180000022, 0.180000022, 0.180000022, 0.180000022)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.180000022);
                  u_xlat16_5 = u_xlat16_7;
              }
              if(!u_xlatb6.y)
              {
                  u_xlat3.x = in_f.vs_TEXCOORD0.x;
                  u_xlat8.xy = (u_xlat3.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_7 = tex2D(_GrabTexture, u_xlat8.xy);
                  u_xlat16_7 = ((u_xlat10_7 * float4(0.190000027, 0.190000027, 0.190000027, 0.190000027)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.190000027);
                  u_xlat16_5 = u_xlat16_7;
              }
              if(!u_xlatb6.z)
              {
                  u_xlat3.x = in_f.vs_TEXCOORD0.x;
                  u_xlat8.xy = (u_xlat3.xz / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_7 = tex2D(_GrabTexture, u_xlat8.xy);
                  u_xlat16_7 = ((u_xlat10_7 * float4(0.200000033, 0.200000033, 0.200000033, 0.200000033)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.200000033);
                  u_xlat16_5 = u_xlat16_7;
              }
              if((u_xlat16_31<in_f.vs_TEXCOORD0.y))
              {
                  u_xlatb8 = 1;
              }
              else u_xlatb8 = 0;
              if((in_f.vs_TEXCOORD0.y<u_xlat16_1.x))
              {
                  u_xlatb18 = 1;
              }
              else u_xlatb18 = 0;
              u_xlatb8 = (u_xlatb18 || u_xlatb8);
              if(!u_xlatb8)
              {
                  u_xlat16_7 = ((u_xlat10_2 * float4(0.210000038, 0.210000038, 0.210000038, 0.210000038)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.210000038);
                  u_xlat16_5 = u_xlat16_7;
              }
              if(!u_xlatb6.w)
              {
                  u_xlat3.x = in_f.vs_TEXCOORD0.x;
                  u_xlat8.xy = (u_xlat3.xw / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_3 = tex2D(_GrabTexture, u_xlat8.xy);
                  u_xlat16_3 = ((u_xlat10_3 * float4(0.200000033, 0.200000033, 0.200000033, 0.200000033)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.200000033);
                  u_xlat16_5 = u_xlat16_3;
              }
              u_xlat3 = ((u_xlat4.xxxx * float4(0.5, 0.75, 1.25, 1.5)) + in_f.vs_TEXCOORD0.yyyy);
              u_xlatb6 = lessThan(float4(u_xlat16_31, u_xlat16_31, u_xlat16_31, u_xlat16_31), u_xlat3);
              u_xlatb7 = lessThan(u_xlat3, u_xlat16_1.xxxx);
              u_xlatb6.x = (u_xlatb6.x || u_xlatb7.x);
              u_xlatb6.y = (u_xlatb6.y || u_xlatb7.y);
              u_xlatb6.z = (u_xlatb6.z || u_xlatb7.z);
              u_xlatb6.w = (u_xlatb6.w || u_xlatb7.w);
              if(!u_xlatb6.x)
              {
                  u_xlat8.x = in_f.vs_TEXCOORD0.x;
                  u_xlat8.y = u_xlat3.x;
                  u_xlat8.xy = (u_xlat8.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_7 = tex2D(_GrabTexture, u_xlat8.xy);
                  u_xlat16_7 = ((u_xlat10_7 * float4(0.190000027, 0.190000027, 0.190000027, 0.190000027)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.190000027);
                  u_xlat16_5 = u_xlat16_7;
              }
              if(!u_xlatb6.y)
              {
                  u_xlat3.x = in_f.vs_TEXCOORD0.x;
                  u_xlat8.xy = (u_xlat3.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_7 = tex2D(_GrabTexture, u_xlat8.xy);
                  u_xlat16_7 = ((u_xlat10_7 * float4(0.180000022, 0.180000022, 0.180000022, 0.180000022)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.180000022);
                  u_xlat16_5 = u_xlat16_7;
              }
              u_xlat8.y = ((_GrabTexture_TexelSize.y * _BlurSize) + in_f.vs_TEXCOORD0.y);
              if((u_xlat16_31<u_xlat8.y))
              {
                  u_xlatb28 = 1;
              }
              else u_xlatb28 = 0;
              if((u_xlat8.y<u_xlat16_1.x))
              {
                  u_xlatb38 = 1;
              }
              else u_xlatb38 = 0;
              u_xlatb28 = (u_xlatb38 || u_xlatb28);
              if(!u_xlatb28)
              {
                  u_xlat8.x = in_f.vs_TEXCOORD0.x;
                  u_xlat8.xy = (u_xlat8.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_7 = tex2D(_GrabTexture, u_xlat8.xy);
                  u_xlat16_7 = ((u_xlat10_7 * float4(0.170000017, 0.170000017, 0.170000017, 0.170000017)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.170000017);
                  u_xlat16_5 = u_xlat16_7;
              }
              if(!u_xlatb6.z)
              {
                  u_xlat3.x = in_f.vs_TEXCOORD0.x;
                  u_xlat8.xy = (u_xlat3.xz / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_7 = tex2D(_GrabTexture, u_xlat8.xy);
                  u_xlat16_7 = ((u_xlat10_7 * float4(0.160000011, 0.160000011, 0.160000011, 0.160000011)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.160000011);
                  u_xlat16_5 = u_xlat16_7;
              }
              if(!u_xlatb6.w)
              {
                  u_xlat3.x = in_f.vs_TEXCOORD0.x;
                  u_xlat8.xy = (u_xlat3.xw / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_3 = tex2D(_GrabTexture, u_xlat8.xy);
                  u_xlat16_3 = ((u_xlat10_3 * float4(0.150000006, 0.150000006, 0.150000006, 0.150000006)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.150000006);
                  u_xlat16_5 = u_xlat16_3;
              }
              u_xlat3 = ((u_xlat4.xxxx * float4(1.75, 2.25, 2.5, 2.75)) + in_f.vs_TEXCOORD0.yyyy);
              u_xlatb6 = lessThan(float4(u_xlat16_31, u_xlat16_31, u_xlat16_31, u_xlat16_31), u_xlat3);
              u_xlatb7 = lessThan(u_xlat3, u_xlat16_1.xxxx);
              u_xlatb6.x = (u_xlatb6.x || u_xlatb7.x);
              u_xlatb6.y = (u_xlatb6.y || u_xlatb7.y);
              u_xlatb6.z = (u_xlatb6.z || u_xlatb7.z);
              u_xlatb6.w = (u_xlatb6.w || u_xlatb7.w);
              if(!u_xlatb6.x)
              {
                  u_xlat8.x = in_f.vs_TEXCOORD0.x;
                  u_xlat8.y = u_xlat3.x;
                  u_xlat8.xy = (u_xlat8.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_7 = tex2D(_GrabTexture, u_xlat8.xy);
                  u_xlat16_7 = ((u_xlat10_7 * float4(0.140000001, 0.140000001, 0.140000001, 0.140000001)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.140000001);
                  u_xlat16_5 = u_xlat16_7;
              }
              u_xlat8.y = ((u_xlat4.x * 2) + in_f.vs_TEXCOORD0.y);
              if((u_xlat16_31<u_xlat8.y))
              {
                  u_xlatb28 = 1;
              }
              else u_xlatb28 = 0;
              if((u_xlat8.y<u_xlat16_1.x))
              {
                  u_xlatb38 = 1;
              }
              else u_xlatb38 = 0;
              u_xlatb28 = (u_xlatb38 || u_xlatb28);
              if(!u_xlatb28)
              {
                  u_xlat8.x = in_f.vs_TEXCOORD0.x;
                  u_xlat8.xy = (u_xlat8.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_7 = tex2D(_GrabTexture, u_xlat8.xy);
                  u_xlat16_7 = ((u_xlat10_7 * float4(0.129999995, 0.129999995, 0.129999995, 0.129999995)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.129999995);
                  u_xlat16_5 = u_xlat16_7;
              }
              if(!u_xlatb6.y)
              {
                  u_xlat3.x = in_f.vs_TEXCOORD0.x;
                  u_xlat8.xy = (u_xlat3.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_7 = tex2D(_GrabTexture, u_xlat8.xy);
                  u_xlat16_7 = ((u_xlat10_7 * float4(0.119999997, 0.119999997, 0.119999997, 0.119999997)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.119999997);
                  u_xlat16_5 = u_xlat16_7;
              }
              if(!u_xlatb6.z)
              {
                  u_xlat3.x = in_f.vs_TEXCOORD0.x;
                  u_xlat8.xy = (u_xlat3.xz / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_7 = tex2D(_GrabTexture, u_xlat8.xy);
                  u_xlat16_7 = ((u_xlat10_7 * float4(0.109999999, 0.109999999, 0.109999999, 0.109999999)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.109999999);
                  u_xlat16_5 = u_xlat16_7;
              }
              if(!u_xlatb6.w)
              {
                  u_xlat3.x = in_f.vs_TEXCOORD0.x;
                  u_xlat8.xy = (u_xlat3.xw / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_3 = tex2D(_GrabTexture, u_xlat8.xy);
                  u_xlat16_3 = ((u_xlat10_3 * float4(0.100000001, 0.100000001, 0.100000001, 0.100000001)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.100000001);
                  u_xlat16_5 = u_xlat16_3;
              }
              u_xlat3 = ((u_xlat4.xxxx * float4(3, 3.25, 3.5, 3.75)) + in_f.vs_TEXCOORD0.yyyy);
              u_xlatb6 = lessThan(float4(u_xlat16_31, u_xlat16_31, u_xlat16_31, u_xlat16_31), u_xlat3);
              u_xlatb7 = lessThan(u_xlat3, u_xlat16_1.xxxx);
              u_xlatb6.x = (u_xlatb6.x || u_xlatb7.x);
              u_xlatb6.y = (u_xlatb6.y || u_xlatb7.y);
              u_xlatb6.z = (u_xlatb6.z || u_xlatb7.z);
              u_xlatb6.w = (u_xlatb6.w || u_xlatb7.w);
              if(!u_xlatb6.x)
              {
                  u_xlat8.x = in_f.vs_TEXCOORD0.x;
                  u_xlat8.y = u_xlat3.x;
                  u_xlat8.xy = (u_xlat8.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_7 = tex2D(_GrabTexture, u_xlat8.xy);
                  u_xlat16_7 = ((u_xlat10_7 * float4(0.0900000036, 0.0900000036, 0.0900000036, 0.0900000036)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.0900000036);
                  u_xlat16_5 = u_xlat16_7;
              }
              if(!u_xlatb6.y)
              {
                  u_xlat3.x = in_f.vs_TEXCOORD0.x;
                  u_xlat8.xy = (u_xlat3.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_7 = tex2D(_GrabTexture, u_xlat8.xy);
                  u_xlat16_7 = ((u_xlat10_7 * float4(0.0800000057, 0.0800000057, 0.0800000057, 0.0800000057)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.0800000057);
                  u_xlat16_5 = u_xlat16_7;
              }
              if(!u_xlatb6.z)
              {
                  u_xlat3.x = in_f.vs_TEXCOORD0.x;
                  u_xlat8.xy = (u_xlat3.xz / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_7 = tex2D(_GrabTexture, u_xlat8.xy);
                  u_xlat16_7 = ((u_xlat10_7 * float4(0.0700000077, 0.0700000077, 0.0700000077, 0.0700000077)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.0700000077);
                  u_xlat16_5 = u_xlat16_7;
              }
              if(!u_xlatb6.w)
              {
                  u_xlat3.x = in_f.vs_TEXCOORD0.x;
                  u_xlat8.xy = (u_xlat3.xw / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_3 = tex2D(_GrabTexture, u_xlat8.xy);
                  u_xlat16_3 = ((u_xlat10_3 * float4(0.0600000098, 0.0600000098, 0.0600000098, 0.0600000098)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.0600000098);
                  u_xlat16_5 = u_xlat16_3;
              }
              u_xlat8.y = ((u_xlat4.x * 4) + in_f.vs_TEXCOORD0.y);
              if((u_xlat16_31<u_xlat8.y))
              {
                  u_xlatb28 = 1;
              }
              else u_xlatb28 = 0;
              if((u_xlat8.y<u_xlat16_1.x))
              {
                  u_xlatb38 = 1;
              }
              else u_xlatb38 = 0;
              u_xlatb28 = (u_xlatb38 || u_xlatb28);
              if(!u_xlatb28)
              {
                  u_xlat8.x = in_f.vs_TEXCOORD0.x;
                  u_xlat8.xy = (u_xlat8.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_3 = tex2D(_GrabTexture, u_xlat8.xy);
                  u_xlat16_3 = ((u_xlat10_3 * float4(0.0500000119, 0.0500000119, 0.0500000119, 0.0500000119)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.0500000119);
                  u_xlat16_5 = u_xlat16_3;
              }
          }
          if((_Quality==3))
          {
              u_xlat8.x = (_GrabTexture_TexelSize.y * _BlurSize);
              u_xlat3 = ((u_xlat8.xxxx * float4(-4, (-3.875), (-3.75), (-3.625))) + in_f.vs_TEXCOORD0.yyyy);
              u_xlatb4 = lessThan(float4(u_xlat16_31, u_xlat16_31, u_xlat16_31, u_xlat16_31), u_xlat3);
              u_xlatb6 = lessThan(u_xlat3, u_xlat16_1.xxxx);
              u_xlatb4.x = (u_xlatb4.x || u_xlatb6.x);
              u_xlatb4.y = (u_xlatb4.y || u_xlatb6.y);
              u_xlatb4.z = (u_xlatb4.z || u_xlatb6.z);
              u_xlatb4.w = (u_xlatb4.w || u_xlatb6.w);
              if(u_xlatb4.x)
              {
                  u_xlat16_5.x = float(0);
                  u_xlat16_5.y = float(0);
                  u_xlat16_5.z = float(0);
                  u_xlat16_5.w = float(0);
                  u_xlat16_11 = 0;
              }
              else
              {
                  u_xlat9.x = in_f.vs_TEXCOORD0.x;
                  u_xlat9.y = u_xlat3.x;
                  u_xlat18.xy = (u_xlat9.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_6 = tex2D(_GrabTexture, u_xlat18.xy);
                  u_xlat16_6 = (u_xlat10_6 * float4(0.0500000007, 0.0500000007, 0.0500000007, 0.0500000007));
                  u_xlat16_5 = u_xlat16_6;
                  u_xlat16_11 = 0.0500000007;
              }
              if(!u_xlatb4.y)
              {
                  u_xlat3.x = in_f.vs_TEXCOORD0.x;
                  u_xlat18.xy = (u_xlat3.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_6 = tex2D(_GrabTexture, u_xlat18.xy);
                  u_xlat16_6 = ((u_xlat10_6 * float4(0.0549999997, 0.0549999997, 0.0549999997, 0.0549999997)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.0549999997);
                  u_xlat16_5 = u_xlat16_6;
              }
              if(!u_xlatb4.z)
              {
                  u_xlat3.x = in_f.vs_TEXCOORD0.x;
                  u_xlat18.xy = (u_xlat3.xz / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_6 = tex2D(_GrabTexture, u_xlat18.xy);
                  u_xlat16_6 = ((u_xlat10_6 * float4(0.0599999987, 0.0599999987, 0.0599999987, 0.0599999987)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.0599999987);
                  u_xlat16_5 = u_xlat16_6;
              }
              if(!u_xlatb4.w)
              {
                  u_xlat3.x = in_f.vs_TEXCOORD0.x;
                  u_xlat18.xy = (u_xlat3.xw / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_3 = tex2D(_GrabTexture, u_xlat18.xy);
                  u_xlat16_3 = ((u_xlat10_3 * float4(0.0649999976, 0.0649999976, 0.0649999976, 0.0649999976)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.0649999976);
                  u_xlat16_5 = u_xlat16_3;
              }
              u_xlat3 = ((u_xlat8.xxxx * float4(-3.5, (-3.375), (-3.25), (-3.125))) + in_f.vs_TEXCOORD0.yyyy);
              u_xlatb4 = lessThan(float4(u_xlat16_31, u_xlat16_31, u_xlat16_31, u_xlat16_31), u_xlat3);
              u_xlatb6 = lessThan(u_xlat3, u_xlat16_1.xxxx);
              u_xlatb4.x = (u_xlatb4.x || u_xlatb6.x);
              u_xlatb4.y = (u_xlatb4.y || u_xlatb6.y);
              u_xlatb4.z = (u_xlatb4.z || u_xlatb6.z);
              u_xlatb4.w = (u_xlatb4.w || u_xlatb6.w);
              if(!u_xlatb4.x)
              {
                  u_xlat9.x = in_f.vs_TEXCOORD0.x;
                  u_xlat9.y = u_xlat3.x;
                  u_xlat18.xy = (u_xlat9.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_6 = tex2D(_GrabTexture, u_xlat18.xy);
                  u_xlat16_6 = ((u_xlat10_6 * float4(0.0700000003, 0.0700000003, 0.0700000003, 0.0700000003)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.0700000003);
                  u_xlat16_5 = u_xlat16_6;
              }
              if(!u_xlatb4.y)
              {
                  u_xlat3.x = in_f.vs_TEXCOORD0.x;
                  u_xlat18.xy = (u_xlat3.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_6 = tex2D(_GrabTexture, u_xlat18.xy);
                  u_xlat16_6 = ((u_xlat10_6 * float4(0.075000003, 0.075000003, 0.075000003, 0.075000003)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.075000003);
                  u_xlat16_5 = u_xlat16_6;
              }
              if(!u_xlatb4.z)
              {
                  u_xlat3.x = in_f.vs_TEXCOORD0.x;
                  u_xlat18.xy = (u_xlat3.xz / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_6 = tex2D(_GrabTexture, u_xlat18.xy);
                  u_xlat16_6 = ((u_xlat10_6 * float4(0.0800000057, 0.0800000057, 0.0800000057, 0.0800000057)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.0800000057);
                  u_xlat16_5 = u_xlat16_6;
              }
              if(!u_xlatb4.w)
              {
                  u_xlat3.x = in_f.vs_TEXCOORD0.x;
                  u_xlat18.xy = (u_xlat3.xw / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_3 = tex2D(_GrabTexture, u_xlat18.xy);
                  u_xlat16_3 = ((u_xlat10_3 * float4(0.0850000083, 0.0850000083, 0.0850000083, 0.0850000083)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.0850000083);
                  u_xlat16_5 = u_xlat16_3;
              }
              u_xlat3 = ((u_xlat8.xxxx * float4(-3, (-2.875), (-2.75), (-2.625))) + in_f.vs_TEXCOORD0.yyyy);
              u_xlatb4 = lessThan(float4(u_xlat16_31, u_xlat16_31, u_xlat16_31, u_xlat16_31), u_xlat3);
              u_xlatb6 = lessThan(u_xlat3, u_xlat16_1.xxxx);
              u_xlatb4.x = (u_xlatb4.x || u_xlatb6.x);
              u_xlatb4.y = (u_xlatb4.y || u_xlatb6.y);
              u_xlatb4.z = (u_xlatb4.z || u_xlatb6.z);
              u_xlatb4.w = (u_xlatb4.w || u_xlatb6.w);
              if(!u_xlatb4.x)
              {
                  u_xlat9.x = in_f.vs_TEXCOORD0.x;
                  u_xlat9.y = u_xlat3.x;
                  u_xlat18.xy = (u_xlat9.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_6 = tex2D(_GrabTexture, u_xlat18.xy);
                  u_xlat16_6 = ((u_xlat10_6 * float4(0.090000011, 0.090000011, 0.090000011, 0.090000011)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.090000011);
                  u_xlat16_5 = u_xlat16_6;
              }
              if(!u_xlatb4.y)
              {
                  u_xlat3.x = in_f.vs_TEXCOORD0.x;
                  u_xlat18.xy = (u_xlat3.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_6 = tex2D(_GrabTexture, u_xlat18.xy);
                  u_xlat16_6 = ((u_xlat10_6 * float4(0.0950000137, 0.0950000137, 0.0950000137, 0.0950000137)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.0950000137);
                  u_xlat16_5 = u_xlat16_6;
              }
              if(!u_xlatb4.z)
              {
                  u_xlat3.x = in_f.vs_TEXCOORD0.x;
                  u_xlat18.xy = (u_xlat3.xz / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_6 = tex2D(_GrabTexture, u_xlat18.xy);
                  u_xlat16_6 = ((u_xlat10_6 * float4(0.100000016, 0.100000016, 0.100000016, 0.100000016)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.100000016);
                  u_xlat16_5 = u_xlat16_6;
              }
              if(!u_xlatb4.w)
              {
                  u_xlat3.x = in_f.vs_TEXCOORD0.x;
                  u_xlat18.xy = (u_xlat3.xw / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_3 = tex2D(_GrabTexture, u_xlat18.xy);
                  u_xlat16_3 = ((u_xlat10_3 * float4(0.105000019, 0.105000019, 0.105000019, 0.105000019)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.105000019);
                  u_xlat16_5 = u_xlat16_3;
              }
              u_xlat3 = ((u_xlat8.xxxx * float4(-2.5, (-2.375), (-2.25), (-2.125))) + in_f.vs_TEXCOORD0.yyyy);
              u_xlatb4 = lessThan(float4(u_xlat16_31, u_xlat16_31, u_xlat16_31, u_xlat16_31), u_xlat3);
              u_xlatb6 = lessThan(u_xlat3, u_xlat16_1.xxxx);
              u_xlatb4.x = (u_xlatb4.x || u_xlatb6.x);
              u_xlatb4.y = (u_xlatb4.y || u_xlatb6.y);
              u_xlatb4.z = (u_xlatb4.z || u_xlatb6.z);
              u_xlatb4.w = (u_xlatb4.w || u_xlatb6.w);
              if(!u_xlatb4.x)
              {
                  u_xlat9.x = in_f.vs_TEXCOORD0.x;
                  u_xlat9.y = u_xlat3.x;
                  u_xlat18.xy = (u_xlat9.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_6 = tex2D(_GrabTexture, u_xlat18.xy);
                  u_xlat16_6 = ((u_xlat10_6 * float4(0.110000022, 0.110000022, 0.110000022, 0.110000022)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.110000022);
                  u_xlat16_5 = u_xlat16_6;
              }
              if(!u_xlatb4.y)
              {
                  u_xlat3.x = in_f.vs_TEXCOORD0.x;
                  u_xlat18.xy = (u_xlat3.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_6 = tex2D(_GrabTexture, u_xlat18.xy);
                  u_xlat16_6 = ((u_xlat10_6 * float4(0.115000024, 0.115000024, 0.115000024, 0.115000024)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.115000024);
                  u_xlat16_5 = u_xlat16_6;
              }
              if(!u_xlatb4.z)
              {
                  u_xlat3.x = in_f.vs_TEXCOORD0.x;
                  u_xlat18.xy = (u_xlat3.xz / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_6 = tex2D(_GrabTexture, u_xlat18.xy);
                  u_xlat16_6 = ((u_xlat10_6 * float4(0.120000027, 0.120000027, 0.120000027, 0.120000027)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.120000027);
                  u_xlat16_5 = u_xlat16_6;
              }
              if(!u_xlatb4.w)
              {
                  u_xlat3.x = in_f.vs_TEXCOORD0.x;
                  u_xlat18.xy = (u_xlat3.xw / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_3 = tex2D(_GrabTexture, u_xlat18.xy);
                  u_xlat16_3 = ((u_xlat10_3 * float4(0.12500003, 0.12500003, 0.12500003, 0.12500003)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.12500003);
                  u_xlat16_5 = u_xlat16_3;
              }
              u_xlat3 = ((u_xlat8.xxxx * float4(-2, (-1.875), (-1.75), (-1.625))) + in_f.vs_TEXCOORD0.yyyy);
              u_xlatb4 = lessThan(float4(u_xlat16_31, u_xlat16_31, u_xlat16_31, u_xlat16_31), u_xlat3);
              u_xlatb6 = lessThan(u_xlat3, u_xlat16_1.xxxx);
              u_xlatb4.x = (u_xlatb4.x || u_xlatb6.x);
              u_xlatb4.y = (u_xlatb4.y || u_xlatb6.y);
              u_xlatb4.z = (u_xlatb4.z || u_xlatb6.z);
              u_xlatb4.w = (u_xlatb4.w || u_xlatb6.w);
              if(!u_xlatb4.x)
              {
                  u_xlat9.x = in_f.vs_TEXCOORD0.x;
                  u_xlat9.y = u_xlat3.x;
                  u_xlat18.xy = (u_xlat9.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_6 = tex2D(_GrabTexture, u_xlat18.xy);
                  u_xlat16_6 = ((u_xlat10_6 * float4(0.130000025, 0.130000025, 0.130000025, 0.130000025)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.130000025);
                  u_xlat16_5 = u_xlat16_6;
              }
              if(!u_xlatb4.y)
              {
                  u_xlat3.x = in_f.vs_TEXCOORD0.x;
                  u_xlat18.xy = (u_xlat3.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_6 = tex2D(_GrabTexture, u_xlat18.xy);
                  u_xlat16_6 = ((u_xlat10_6 * float4(0.13500002, 0.13500002, 0.13500002, 0.13500002)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.13500002);
                  u_xlat16_5 = u_xlat16_6;
              }
              if(!u_xlatb4.z)
              {
                  u_xlat3.x = in_f.vs_TEXCOORD0.x;
                  u_xlat18.xy = (u_xlat3.xz / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_6 = tex2D(_GrabTexture, u_xlat18.xy);
                  u_xlat16_6 = ((u_xlat10_6 * float4(0.140000015, 0.140000015, 0.140000015, 0.140000015)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.140000015);
                  u_xlat16_5 = u_xlat16_6;
              }
              if(!u_xlatb4.w)
              {
                  u_xlat3.x = in_f.vs_TEXCOORD0.x;
                  u_xlat18.xy = (u_xlat3.xw / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_3 = tex2D(_GrabTexture, u_xlat18.xy);
                  u_xlat16_3 = ((u_xlat10_3 * float4(0.145000011, 0.145000011, 0.145000011, 0.145000011)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.145000011);
                  u_xlat16_5 = u_xlat16_3;
              }
              u_xlat3 = ((u_xlat8.xxxx * float4(-1.5, (-1.375), (-1.25), (-1.125))) + in_f.vs_TEXCOORD0.yyyy);
              u_xlatb4 = lessThan(float4(u_xlat16_31, u_xlat16_31, u_xlat16_31, u_xlat16_31), u_xlat3);
              u_xlatb6 = lessThan(u_xlat3, u_xlat16_1.xxxx);
              u_xlatb4.x = (u_xlatb4.x || u_xlatb6.x);
              u_xlatb4.y = (u_xlatb4.y || u_xlatb6.y);
              u_xlatb4.z = (u_xlatb4.z || u_xlatb6.z);
              u_xlatb4.w = (u_xlatb4.w || u_xlatb6.w);
              if(!u_xlatb4.x)
              {
                  u_xlat9.x = in_f.vs_TEXCOORD0.x;
                  u_xlat9.y = u_xlat3.x;
                  u_xlat18.xy = (u_xlat9.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_6 = tex2D(_GrabTexture, u_xlat18.xy);
                  u_xlat16_6 = ((u_xlat10_6 * float4(0.150000006, 0.150000006, 0.150000006, 0.150000006)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.150000006);
                  u_xlat16_5 = u_xlat16_6;
              }
              if(!u_xlatb4.y)
              {
                  u_xlat3.x = in_f.vs_TEXCOORD0.x;
                  u_xlat18.xy = (u_xlat3.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_6 = tex2D(_GrabTexture, u_xlat18.xy);
                  u_xlat16_6 = ((u_xlat10_6 * float4(0.155000001, 0.155000001, 0.155000001, 0.155000001)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.155000001);
                  u_xlat16_5 = u_xlat16_6;
              }
              if(!u_xlatb4.z)
              {
                  u_xlat3.x = in_f.vs_TEXCOORD0.x;
                  u_xlat18.xy = (u_xlat3.xz / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_6 = tex2D(_GrabTexture, u_xlat18.xy);
                  u_xlat16_6 = ((u_xlat10_6 * float4(0.159999996, 0.159999996, 0.159999996, 0.159999996)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.159999996);
                  u_xlat16_5 = u_xlat16_6;
              }
              if(!u_xlatb4.w)
              {
                  u_xlat3.x = in_f.vs_TEXCOORD0.x;
                  u_xlat18.xy = (u_xlat3.xw / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_3 = tex2D(_GrabTexture, u_xlat18.xy);
                  u_xlat16_3 = ((u_xlat10_3 * float4(0.164999992, 0.164999992, 0.164999992, 0.164999992)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.164999992);
                  u_xlat16_5 = u_xlat16_3;
              }
              u_xlat9.y = (((-_GrabTexture_TexelSize.y) * _BlurSize) + in_f.vs_TEXCOORD0.y);
              if((u_xlat16_31<u_xlat9.y))
              {
                  u_xlatb18 = 1;
              }
              else u_xlatb18 = 0;
              if((u_xlat9.y<u_xlat16_1.x))
              {
                  u_xlatb28 = 1;
              }
              else u_xlatb28 = 0;
              u_xlatb18 = (u_xlatb28 || u_xlatb18);
              if(!u_xlatb18)
              {
                  u_xlat9.x = in_f.vs_TEXCOORD0.x;
                  u_xlat18.xy = (u_xlat9.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_3 = tex2D(_GrabTexture, u_xlat18.xy);
                  u_xlat16_3 = ((u_xlat10_3 * float4(0.169999987, 0.169999987, 0.169999987, 0.169999987)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.169999987);
                  u_xlat16_5 = u_xlat16_3;
              }
              u_xlat3 = ((u_xlat8.xxxx * float4(-0.875, (-0.75), (-0.625), (-0.5))) + in_f.vs_TEXCOORD0.yyyy);
              u_xlatb4 = lessThan(float4(u_xlat16_31, u_xlat16_31, u_xlat16_31, u_xlat16_31), u_xlat3);
              u_xlatb6 = lessThan(u_xlat3, u_xlat16_1.xxxx);
              u_xlatb4.x = (u_xlatb4.x || u_xlatb6.x);
              u_xlatb4.y = (u_xlatb4.y || u_xlatb6.y);
              u_xlatb4.z = (u_xlatb4.z || u_xlatb6.z);
              u_xlatb4.w = (u_xlatb4.w || u_xlatb6.w);
              if(!u_xlatb4.x)
              {
                  u_xlat9.x = in_f.vs_TEXCOORD0.x;
                  u_xlat9.y = u_xlat3.x;
                  u_xlat18.xy = (u_xlat9.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_6 = tex2D(_GrabTexture, u_xlat18.xy);
                  u_xlat16_6 = ((u_xlat10_6 * float4(0.174999982, 0.174999982, 0.174999982, 0.174999982)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.174999982);
                  u_xlat16_5 = u_xlat16_6;
              }
              if(!u_xlatb4.y)
              {
                  u_xlat3.x = in_f.vs_TEXCOORD0.x;
                  u_xlat18.xy = (u_xlat3.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_6 = tex2D(_GrabTexture, u_xlat18.xy);
                  u_xlat16_6 = ((u_xlat10_6 * float4(0.179999977, 0.179999977, 0.179999977, 0.179999977)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.179999977);
                  u_xlat16_5 = u_xlat16_6;
              }
              if(!u_xlatb4.z)
              {
                  u_xlat3.x = in_f.vs_TEXCOORD0.x;
                  u_xlat18.xy = (u_xlat3.xz / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_6 = tex2D(_GrabTexture, u_xlat18.xy);
                  u_xlat16_6 = ((u_xlat10_6 * float4(0.184999973, 0.184999973, 0.184999973, 0.184999973)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.184999973);
                  u_xlat16_5 = u_xlat16_6;
              }
              if(!u_xlatb4.w)
              {
                  u_xlat3.x = in_f.vs_TEXCOORD0.x;
                  u_xlat18.xy = (u_xlat3.xw / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_3 = tex2D(_GrabTexture, u_xlat18.xy);
                  u_xlat16_3 = ((u_xlat10_3 * float4(0.189999968, 0.189999968, 0.189999968, 0.189999968)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.189999968);
                  u_xlat16_5 = u_xlat16_3;
              }
              u_xlat3 = ((u_xlat8.xxxx * float4(-0.375, (-0.25), (-0.125), 0.125)) + in_f.vs_TEXCOORD0.yyyy);
              u_xlatb4 = lessThan(float4(u_xlat16_31, u_xlat16_31, u_xlat16_31, u_xlat16_31), u_xlat3);
              u_xlatb6 = lessThan(u_xlat3, u_xlat16_1.xxxx);
              u_xlatb4.x = (u_xlatb4.x || u_xlatb6.x);
              u_xlatb4.y = (u_xlatb4.y || u_xlatb6.y);
              u_xlatb4.z = (u_xlatb4.z || u_xlatb6.z);
              u_xlatb4.w = (u_xlatb4.w || u_xlatb6.w);
              if(!u_xlatb4.x)
              {
                  u_xlat9.x = in_f.vs_TEXCOORD0.x;
                  u_xlat9.y = u_xlat3.x;
                  u_xlat18.xy = (u_xlat9.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_6 = tex2D(_GrabTexture, u_xlat18.xy);
                  u_xlat16_6 = ((u_xlat10_6 * float4(0.194999963, 0.194999963, 0.194999963, 0.194999963)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.194999963);
                  u_xlat16_5 = u_xlat16_6;
              }
              if(!u_xlatb4.y)
              {
                  u_xlat3.x = in_f.vs_TEXCOORD0.x;
                  u_xlat18.xy = (u_xlat3.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_6 = tex2D(_GrabTexture, u_xlat18.xy);
                  u_xlat16_6 = ((u_xlat10_6 * float4(0.199999958, 0.199999958, 0.199999958, 0.199999958)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.199999958);
                  u_xlat16_5 = u_xlat16_6;
              }
              if(!u_xlatb4.z)
              {
                  u_xlat3.x = in_f.vs_TEXCOORD0.x;
                  u_xlat18.xy = (u_xlat3.xz / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_6 = tex2D(_GrabTexture, u_xlat18.xy);
                  u_xlat16_6 = ((u_xlat10_6 * float4(0.204999954, 0.204999954, 0.204999954, 0.204999954)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.204999954);
                  u_xlat16_5 = u_xlat16_6;
              }
              if((u_xlat16_31<in_f.vs_TEXCOORD0.y))
              {
                  u_xlatb18 = 1;
              }
              else u_xlatb18 = 0;
              if((in_f.vs_TEXCOORD0.y<u_xlat16_1.x))
              {
                  u_xlatb28 = 1;
              }
              else u_xlatb28 = 0;
              u_xlatb18 = (u_xlatb28 || u_xlatb18);
              if(!u_xlatb18)
              {
                  u_xlat16_6 = ((u_xlat10_2 * float4(0.209999949, 0.209999949, 0.209999949, 0.209999949)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.209999949);
                  u_xlat16_5 = u_xlat16_6;
              }
              if(!u_xlatb4.w)
              {
                  u_xlat3.x = in_f.vs_TEXCOORD0.x;
                  u_xlat18.xy = (u_xlat3.xw / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_3 = tex2D(_GrabTexture, u_xlat18.xy);
                  u_xlat16_3 = ((u_xlat10_3 * float4(0.204999954, 0.204999954, 0.204999954, 0.204999954)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.204999954);
                  u_xlat16_5 = u_xlat16_3;
              }
              u_xlat3 = ((u_xlat8.xxxx * float4(0.25, 0.375, 0.5, 0.625)) + in_f.vs_TEXCOORD0.yyyy);
              u_xlatb4 = lessThan(float4(u_xlat16_31, u_xlat16_31, u_xlat16_31, u_xlat16_31), u_xlat3);
              u_xlatb6 = lessThan(u_xlat3, u_xlat16_1.xxxx);
              u_xlatb4.x = (u_xlatb4.x || u_xlatb6.x);
              u_xlatb4.y = (u_xlatb4.y || u_xlatb6.y);
              u_xlatb4.z = (u_xlatb4.z || u_xlatb6.z);
              u_xlatb4.w = (u_xlatb4.w || u_xlatb6.w);
              if(!u_xlatb4.x)
              {
                  u_xlat9.x = in_f.vs_TEXCOORD0.x;
                  u_xlat9.y = u_xlat3.x;
                  u_xlat18.xy = (u_xlat9.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_6 = tex2D(_GrabTexture, u_xlat18.xy);
                  u_xlat16_6 = ((u_xlat10_6 * float4(0.199999958, 0.199999958, 0.199999958, 0.199999958)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.199999958);
                  u_xlat16_5 = u_xlat16_6;
              }
              if(!u_xlatb4.y)
              {
                  u_xlat3.x = in_f.vs_TEXCOORD0.x;
                  u_xlat18.xy = (u_xlat3.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_6 = tex2D(_GrabTexture, u_xlat18.xy);
                  u_xlat16_6 = ((u_xlat10_6 * float4(0.194999963, 0.194999963, 0.194999963, 0.194999963)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.194999963);
                  u_xlat16_5 = u_xlat16_6;
              }
              if(!u_xlatb4.z)
              {
                  u_xlat3.x = in_f.vs_TEXCOORD0.x;
                  u_xlat18.xy = (u_xlat3.xz / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_6 = tex2D(_GrabTexture, u_xlat18.xy);
                  u_xlat16_6 = ((u_xlat10_6 * float4(0.189999968, 0.189999968, 0.189999968, 0.189999968)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.189999968);
                  u_xlat16_5 = u_xlat16_6;
              }
              if(!u_xlatb4.w)
              {
                  u_xlat3.x = in_f.vs_TEXCOORD0.x;
                  u_xlat18.xy = (u_xlat3.xw / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_3 = tex2D(_GrabTexture, u_xlat18.xy);
                  u_xlat16_5 = ((u_xlat10_3 * float4(0.184999973, 0.184999973, 0.184999973, 0.184999973)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.184999973);
                  u_xlat16_5 = u_xlat16_5;
              }
              u_xlat3 = ((u_xlat8.xxxx * float4(0.75, 0.875, 1.125, 1.25)) + in_f.vs_TEXCOORD0.yyyy);
              u_xlatb4 = lessThan(float4(u_xlat16_31, u_xlat16_31, u_xlat16_31, u_xlat16_31), u_xlat3);
              u_xlatb6 = lessThan(u_xlat3, u_xlat16_1.xxxx);
              u_xlatb4.x = (u_xlatb4.x || u_xlatb6.x);
              u_xlatb4.y = (u_xlatb4.y || u_xlatb6.y);
              u_xlatb4.z = (u_xlatb4.z || u_xlatb6.z);
              u_xlatb4.w = (u_xlatb4.w || u_xlatb6.w);
              if(!u_xlatb4.x)
              {
                  u_xlat9.x = in_f.vs_TEXCOORD0.x;
                  u_xlat9.y = u_xlat3.x;
                  u_xlat18.xy = (u_xlat9.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_6 = tex2D(_GrabTexture, u_xlat18.xy);
                  u_xlat16_5 = ((u_xlat10_6 * float4(0.179999977, 0.179999977, 0.179999977, 0.179999977)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.179999977);
                  u_xlat16_5 = u_xlat16_5;
              }
              if(!u_xlatb4.y)
              {
                  u_xlat3.x = in_f.vs_TEXCOORD0.x;
                  u_xlat18.xy = (u_xlat3.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_6 = tex2D(_GrabTexture, u_xlat18.xy);
                  u_xlat16_5 = ((u_xlat10_6 * float4(0.174999982, 0.174999982, 0.174999982, 0.174999982)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.174999982);
                  u_xlat16_5 = u_xlat16_5;
              }
              u_xlat9.y = ((_GrabTexture_TexelSize.y * _BlurSize) + in_f.vs_TEXCOORD0.y);
              if((u_xlat16_31<u_xlat9.y))
              {
                  u_xlatb18 = 1;
              }
              else u_xlatb18 = 0;
              if((u_xlat9.y<u_xlat16_1.x))
              {
                  u_xlatb28 = 1;
              }
              else u_xlatb28 = 0;
              u_xlatb18 = (u_xlatb28 || u_xlatb18);
              if(!u_xlatb18)
              {
                  u_xlat9.x = in_f.vs_TEXCOORD0.x;
                  u_xlat18.xy = (u_xlat9.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_6 = tex2D(_GrabTexture, u_xlat18.xy);
                  u_xlat16_5 = ((u_xlat10_6 * float4(0.169999987, 0.169999987, 0.169999987, 0.169999987)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.169999987);
                  u_xlat16_5 = u_xlat16_5;
              }
              if(!u_xlatb4.z)
              {
                  u_xlat3.x = in_f.vs_TEXCOORD0.x;
                  u_xlat18.xy = (u_xlat3.xz / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_6 = tex2D(_GrabTexture, u_xlat18.xy);
                  u_xlat16_5 = ((u_xlat10_6 * float4(0.164999992, 0.164999992, 0.164999992, 0.164999992)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.164999992);
                  u_xlat16_5 = u_xlat16_5;
              }
              if(!u_xlatb4.w)
              {
                  u_xlat3.x = in_f.vs_TEXCOORD0.x;
                  u_xlat18.xy = (u_xlat3.xw / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_3 = tex2D(_GrabTexture, u_xlat18.xy);
                  u_xlat16_5 = ((u_xlat10_3 * float4(0.159999996, 0.159999996, 0.159999996, 0.159999996)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.159999996);
                  u_xlat16_5 = u_xlat16_5;
              }
              u_xlat3 = ((u_xlat8.xxxx * float4(1.375, 1.5, 1.625, 1.75)) + in_f.vs_TEXCOORD0.yyyy);
              u_xlatb4 = lessThan(float4(u_xlat16_31, u_xlat16_31, u_xlat16_31, u_xlat16_31), u_xlat3);
              u_xlatb6 = lessThan(u_xlat3, u_xlat16_1.xxxx);
              u_xlatb4.x = (u_xlatb4.x || u_xlatb6.x);
              u_xlatb4.y = (u_xlatb4.y || u_xlatb6.y);
              u_xlatb4.z = (u_xlatb4.z || u_xlatb6.z);
              u_xlatb4.w = (u_xlatb4.w || u_xlatb6.w);
              if(!u_xlatb4.x)
              {
                  u_xlat9.x = in_f.vs_TEXCOORD0.x;
                  u_xlat9.y = u_xlat3.x;
                  u_xlat18.xy = (u_xlat9.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_6 = tex2D(_GrabTexture, u_xlat18.xy);
                  u_xlat16_5 = ((u_xlat10_6 * float4(0.155000001, 0.155000001, 0.155000001, 0.155000001)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.155000001);
                  u_xlat16_5 = u_xlat16_5;
              }
              if(!u_xlatb4.y)
              {
                  u_xlat3.x = in_f.vs_TEXCOORD0.x;
                  u_xlat18.xy = (u_xlat3.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_6 = tex2D(_GrabTexture, u_xlat18.xy);
                  u_xlat16_5 = ((u_xlat10_6 * float4(0.150000006, 0.150000006, 0.150000006, 0.150000006)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.150000006);
                  u_xlat16_5 = u_xlat16_5;
              }
              if(!u_xlatb4.z)
              {
                  u_xlat3.x = in_f.vs_TEXCOORD0.x;
                  u_xlat18.xy = (u_xlat3.xz / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_6 = tex2D(_GrabTexture, u_xlat18.xy);
                  u_xlat16_5 = ((u_xlat10_6 * float4(0.145000011, 0.145000011, 0.145000011, 0.145000011)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.145000011);
                  u_xlat16_5 = u_xlat16_5;
              }
              if(!u_xlatb4.w)
              {
                  u_xlat3.x = in_f.vs_TEXCOORD0.x;
                  u_xlat18.xy = (u_xlat3.xw / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_3 = tex2D(_GrabTexture, u_xlat18.xy);
                  u_xlat16_5 = ((u_xlat10_3 * float4(0.140000015, 0.140000015, 0.140000015, 0.140000015)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.140000015);
                  u_xlat16_5 = u_xlat16_5;
              }
              u_xlat3 = ((u_xlat8.xxxx * float4(1.875, 2.125, 2.25, 2.375)) + in_f.vs_TEXCOORD0.yyyy);
              u_xlatb4 = lessThan(float4(u_xlat16_31, u_xlat16_31, u_xlat16_31, u_xlat16_31), u_xlat3);
              u_xlatb6 = lessThan(u_xlat3, u_xlat16_1.xxxx);
              u_xlatb4.x = (u_xlatb4.x || u_xlatb6.x);
              u_xlatb4.y = (u_xlatb4.y || u_xlatb6.y);
              u_xlatb4.z = (u_xlatb4.z || u_xlatb6.z);
              u_xlatb4.w = (u_xlatb4.w || u_xlatb6.w);
              if(!u_xlatb4.x)
              {
                  u_xlat9.x = in_f.vs_TEXCOORD0.x;
                  u_xlat9.y = u_xlat3.x;
                  u_xlat18.xy = (u_xlat9.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_6 = tex2D(_GrabTexture, u_xlat18.xy);
                  u_xlat16_5 = ((u_xlat10_6 * float4(0.13500002, 0.13500002, 0.13500002, 0.13500002)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.13500002);
                  u_xlat16_5 = u_xlat16_5;
              }
              u_xlat9.y = ((u_xlat8.x * 2) + in_f.vs_TEXCOORD0.y);
              if((u_xlat16_31<u_xlat9.y))
              {
                  u_xlatb18 = 1;
              }
              else u_xlatb18 = 0;
              if((u_xlat9.y<u_xlat16_1.x))
              {
                  u_xlatb28 = 1;
              }
              else u_xlatb28 = 0;
              u_xlatb18 = (u_xlatb28 || u_xlatb18);
              if(!u_xlatb18)
              {
                  u_xlat9.x = in_f.vs_TEXCOORD0.x;
                  u_xlat18.xy = (u_xlat9.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_6 = tex2D(_GrabTexture, u_xlat18.xy);
                  u_xlat16_5 = ((u_xlat10_6 * float4(0.130000025, 0.130000025, 0.130000025, 0.130000025)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.130000025);
                  u_xlat16_5 = u_xlat16_5;
              }
              if(!u_xlatb4.y)
              {
                  u_xlat3.x = in_f.vs_TEXCOORD0.x;
                  u_xlat18.xy = (u_xlat3.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_6 = tex2D(_GrabTexture, u_xlat18.xy);
                  u_xlat16_5 = ((u_xlat10_6 * float4(0.12500003, 0.12500003, 0.12500003, 0.12500003)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.12500003);
                  u_xlat16_5 = u_xlat16_5;
              }
              if(!u_xlatb4.z)
              {
                  u_xlat3.x = in_f.vs_TEXCOORD0.x;
                  u_xlat18.xy = (u_xlat3.xz / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_6 = tex2D(_GrabTexture, u_xlat18.xy);
                  u_xlat16_5 = ((u_xlat10_6 * float4(0.120000027, 0.120000027, 0.120000027, 0.120000027)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.120000027);
                  u_xlat16_5 = u_xlat16_5;
              }
              if(!u_xlatb4.w)
              {
                  u_xlat3.x = in_f.vs_TEXCOORD0.x;
                  u_xlat18.xy = (u_xlat3.xw / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_3 = tex2D(_GrabTexture, u_xlat18.xy);
                  u_xlat16_5 = ((u_xlat10_3 * float4(0.115000024, 0.115000024, 0.115000024, 0.115000024)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.115000024);
                  u_xlat16_5 = u_xlat16_5;
              }
              u_xlat3 = ((u_xlat8.xxxx * float4(2.5, 2.625, 2.75, 2.875)) + in_f.vs_TEXCOORD0.yyyy);
              u_xlatb4 = lessThan(float4(u_xlat16_31, u_xlat16_31, u_xlat16_31, u_xlat16_31), u_xlat3);
              u_xlatb6 = lessThan(u_xlat3, u_xlat16_1.xxxx);
              u_xlatb4.x = (u_xlatb4.x || u_xlatb6.x);
              u_xlatb4.y = (u_xlatb4.y || u_xlatb6.y);
              u_xlatb4.z = (u_xlatb4.z || u_xlatb6.z);
              u_xlatb4.w = (u_xlatb4.w || u_xlatb6.w);
              if(!u_xlatb4.x)
              {
                  u_xlat9.x = in_f.vs_TEXCOORD0.x;
                  u_xlat9.y = u_xlat3.x;
                  u_xlat18.xy = (u_xlat9.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_6 = tex2D(_GrabTexture, u_xlat18.xy);
                  u_xlat16_5 = ((u_xlat10_6 * float4(0.110000022, 0.110000022, 0.110000022, 0.110000022)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.110000022);
                  u_xlat16_5 = u_xlat16_5;
              }
              if(!u_xlatb4.y)
              {
                  u_xlat3.x = in_f.vs_TEXCOORD0.x;
                  u_xlat18.xy = (u_xlat3.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_6 = tex2D(_GrabTexture, u_xlat18.xy);
                  u_xlat16_5 = ((u_xlat10_6 * float4(0.105000019, 0.105000019, 0.105000019, 0.105000019)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.105000019);
                  u_xlat16_5 = u_xlat16_5;
              }
              if(!u_xlatb4.z)
              {
                  u_xlat3.x = in_f.vs_TEXCOORD0.x;
                  u_xlat18.xy = (u_xlat3.xz / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_6 = tex2D(_GrabTexture, u_xlat18.xy);
                  u_xlat16_5 = ((u_xlat10_6 * float4(0.100000016, 0.100000016, 0.100000016, 0.100000016)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.100000016);
                  u_xlat16_5 = u_xlat16_5;
              }
              if(!u_xlatb4.w)
              {
                  u_xlat3.x = in_f.vs_TEXCOORD0.x;
                  u_xlat18.xy = (u_xlat3.xw / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_3 = tex2D(_GrabTexture, u_xlat18.xy);
                  u_xlat16_5 = ((u_xlat10_3 * float4(0.0950000137, 0.0950000137, 0.0950000137, 0.0950000137)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.0950000137);
                  u_xlat16_5 = u_xlat16_5;
              }
              u_xlat3 = ((u_xlat8.xxxx * float4(3, 3.125, 3.25, 3.375)) + in_f.vs_TEXCOORD0.yyyy);
              u_xlatb4 = lessThan(float4(u_xlat16_31, u_xlat16_31, u_xlat16_31, u_xlat16_31), u_xlat3);
              u_xlatb6 = lessThan(u_xlat3, u_xlat16_1.xxxx);
              u_xlatb4.x = (u_xlatb4.x || u_xlatb6.x);
              u_xlatb4.y = (u_xlatb4.y || u_xlatb6.y);
              u_xlatb4.z = (u_xlatb4.z || u_xlatb6.z);
              u_xlatb4.w = (u_xlatb4.w || u_xlatb6.w);
              if(!u_xlatb4.x)
              {
                  u_xlat9.x = in_f.vs_TEXCOORD0.x;
                  u_xlat9.y = u_xlat3.x;
                  u_xlat18.xy = (u_xlat9.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_6 = tex2D(_GrabTexture, u_xlat18.xy);
                  u_xlat16_5 = ((u_xlat10_6 * float4(0.090000011, 0.090000011, 0.090000011, 0.090000011)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.090000011);
                  u_xlat16_5 = u_xlat16_5;
              }
              if(!u_xlatb4.y)
              {
                  u_xlat3.x = in_f.vs_TEXCOORD0.x;
                  u_xlat18.xy = (u_xlat3.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_6 = tex2D(_GrabTexture, u_xlat18.xy);
                  u_xlat16_5 = ((u_xlat10_6 * float4(0.0850000083, 0.0850000083, 0.0850000083, 0.0850000083)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.0850000083);
                  u_xlat16_5 = u_xlat16_5;
              }
              if(!u_xlatb4.z)
              {
                  u_xlat3.x = in_f.vs_TEXCOORD0.x;
                  u_xlat18.xy = (u_xlat3.xz / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_6 = tex2D(_GrabTexture, u_xlat18.xy);
                  u_xlat16_5 = ((u_xlat10_6 * float4(0.0800000057, 0.0800000057, 0.0800000057, 0.0800000057)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.0800000057);
                  u_xlat16_5 = u_xlat16_5;
              }
              if(!u_xlatb4.w)
              {
                  u_xlat3.x = in_f.vs_TEXCOORD0.x;
                  u_xlat18.xy = (u_xlat3.xw / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_3 = tex2D(_GrabTexture, u_xlat18.xy);
                  u_xlat16_5 = ((u_xlat10_3 * float4(0.075000003, 0.075000003, 0.075000003, 0.075000003)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.075000003);
                  u_xlat16_5 = u_xlat16_5;
              }
              u_xlat3 = ((u_xlat8.xxxx * float4(3.5, 3.625, 3.75, 3.875)) + in_f.vs_TEXCOORD0.yyyy);
              u_xlatb4 = lessThan(float4(u_xlat16_31, u_xlat16_31, u_xlat16_31, u_xlat16_31), u_xlat3);
              u_xlatb6 = lessThan(u_xlat3, u_xlat16_1.xxxx);
              u_xlatb4.x = (u_xlatb4.x || u_xlatb6.x);
              u_xlatb4.y = (u_xlatb4.y || u_xlatb6.y);
              u_xlatb4.z = (u_xlatb4.z || u_xlatb6.z);
              u_xlatb4.w = (u_xlatb4.w || u_xlatb6.w);
              if(!u_xlatb4.x)
              {
                  u_xlat9.x = in_f.vs_TEXCOORD0.x;
                  u_xlat9.y = u_xlat3.x;
                  u_xlat18.xy = (u_xlat9.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_6 = tex2D(_GrabTexture, u_xlat18.xy);
                  u_xlat16_5 = ((u_xlat10_6 * float4(0.0700000003, 0.0700000003, 0.0700000003, 0.0700000003)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.0700000003);
                  u_xlat16_5 = u_xlat16_5;
              }
              if(!u_xlatb4.y)
              {
                  u_xlat3.x = in_f.vs_TEXCOORD0.x;
                  u_xlat18.xy = (u_xlat3.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_6 = tex2D(_GrabTexture, u_xlat18.xy);
                  u_xlat16_5 = ((u_xlat10_6 * float4(0.0649999976, 0.0649999976, 0.0649999976, 0.0649999976)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.0649999976);
                  u_xlat16_5 = u_xlat16_5;
              }
              if(!u_xlatb4.z)
              {
                  u_xlat3.x = in_f.vs_TEXCOORD0.x;
                  u_xlat18.xy = (u_xlat3.xz / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_6 = tex2D(_GrabTexture, u_xlat18.xy);
                  u_xlat16_5 = ((u_xlat10_6 * float4(0.0599999987, 0.0599999987, 0.0599999987, 0.0599999987)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.0599999987);
                  u_xlat16_5 = u_xlat16_5;
              }
              if(!u_xlatb4.w)
              {
                  u_xlat3.x = in_f.vs_TEXCOORD0.x;
                  u_xlat18.xy = (u_xlat3.xw / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_3 = tex2D(_GrabTexture, u_xlat18.xy);
                  u_xlat16_5 = ((u_xlat10_3 * float4(0.0549999997, 0.0549999997, 0.0549999997, 0.0549999997)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.0549999997);
                  u_xlat16_5 = u_xlat16_5;
              }
              u_xlat8.y = ((u_xlat8.x * 4) + in_f.vs_TEXCOORD0.y);
              if((u_xlat16_31<u_xlat8.y))
              {
                  u_xlatb28 = 1;
              }
              else u_xlatb28 = 0;
              if((u_xlat8.y<u_xlat16_1.x))
              {
                  u_xlatb38 = 1;
              }
              else u_xlatb38 = 0;
              u_xlatb28 = (u_xlatb38 || u_xlatb28);
              if(!u_xlatb28)
              {
                  u_xlat8.x = in_f.vs_TEXCOORD0.x;
                  u_xlat8.xy = (u_xlat8.xy / in_f.vs_TEXCOORD0.ww);
                  u_xlat10_3 = tex2D(_GrabTexture, u_xlat8.xy);
                  u_xlat16_5 = ((u_xlat10_3 * float4(0.0500000007, 0.0500000007, 0.0500000007, 0.0500000007)) + u_xlat16_5);
                  u_xlat16_11 = (u_xlat16_11 + 0.0500000007);
                  u_xlat16_5 = u_xlat16_5;
              }
          }
          else
          {
              u_xlat16_5.x = float(0);
              u_xlat16_5.y = float(0);
              u_xlat16_5.z = float(0);
              u_xlat16_5.w = float(0);
              u_xlat16_11 = 0;
          }
          u_xlat8.x = (((-u_xlat10_0) * _Lightness) + 1);
          u_xlat8.x = (u_xlat16_11 * u_xlat8.x);
          u_xlat1 = (u_xlat16_5 / u_xlat8.xxxx);
          u_xlat8.x = (u_xlat10_0 * _Color.w);
          u_xlat1.xyz = ((u_xlat8.xxx * _Color.xyz) + u_xlat1.xyz);
          u_xlat16_1 = ((-u_xlat10_2) + u_xlat1);
          out_f.SV_Target0 = ((float4(u_xlat10_0, u_xlat10_0, u_xlat10_0, u_xlat10_0) * u_xlat16_1) + u_xlat10_2);
          return out_f;
          0.5;
          _PanelHeight;
          float2(0, 0);
          float2(_ScreenHeight, _PanelY);
      }
      
      
      ENDCG
      
    } // end phase
  }
  FallBack ""
}
