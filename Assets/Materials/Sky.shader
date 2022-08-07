// https://qiita.com/ELIXIR/items/a5988ca21f38fabca7b0

Shader "Unlit/Sky"
{
    Properties
    {
        _MainTex ("DayShader", 2D) = "blue" {}
        _NightTex ("StarShader", 2D) = "white" {}
        _Blend("Blend",Range (0, 1)) = 1
        _Round("Round",Range (0, 1)) = 0
    }
    SubShader
    {
        Tags
        {
            "RenderType"="Opaque"
        }
        Cull Front // add
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            // 緯度
            // const float LATITUDE = 35.1;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                float3 viewDir : TEXCOORD1;
            };

            sampler2D _MainTex;
            sampler2D _NightTex;
            float _Blend;
            float4 _MainTex_ST;
            float _Round;

            v2f vert(appdata v)
            {
                v.uv.x = 1 - v.uv.x; // add
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.viewDir = -normalize(_WorldSpaceCameraPos - mul(unity_ObjectToWorld, v.vertex));
                UNITY_TRANSFER_FOG(o, o.vertex);
                return o;
            }

            float4 quaternion(float rad, float3 axis)
            {
                return float4(normalize(axis) * sin(rad * 0.5), cos(rad * 0.5));
            }

            float3 rotateQuaternion(float rad, float3 axis, float3 pos)
            {
                float4 q = quaternion(rad, axis);
                return (q.w * q.w - dot(q.xyz, q.xyz)) * pos + 2.0 * q.xyz * dot(q.xyz, pos) + 2 * q.w * cross(
                    q.xyz, pos);
            }

            fixed4 frag(v2f i) : SV_Target
            {
                const float PI = 3.14159;

                const float LATITUDE = 34.76; // Koyo

                // day
                fixed4 day = tex2D(_MainTex, i.uv);

                // star
                // ベクトル回転
                // https://qiita.com/metaaa/items/a38112efb4499cb7e908
                // float star_x = i.uv.x;
                // float3 eye_vec = normalize(float3(cos(star_x * 2 * PI) * sin(i.uv.y * PI), -cos(i.uv.y * PI), -sin(star_x * 2 * PI) * sin(i.uv.y * PI)));
                float3 rotated = rotateQuaternion((90 - LATITUDE) / 180 * PI, float3(1, 0, 0), i.viewDir);
                float2 starUV = float2((atan2(rotated.x, rotated.z) + PI) / (2 * PI),
                                       -atan2(sqrt(pow(rotated.x, 2) + pow(rotated.z, 2)), rotated.y) / PI + 1);

                // starUV.x -= _Time;
                starUV.x -= _Round;

                fixed4 night = tex2D(_NightTex, starUV);
                // if(starUV.y > 1 - 1.0/32) return fixed4(0,1,0,0);

                // marge
                fixed4 col = day * (1 - _Blend) + night * _Blend; // lerp?
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
                // return fixed4(i.viewDir.x, i.viewDir.y, i.viewDir.z, 0);
            }
            ENDCG
        }
    }
}