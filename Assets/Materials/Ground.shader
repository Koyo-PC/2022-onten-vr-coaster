Shader "Custom/Ground"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags
        {
            "RenderType"="Transparent" "Queue"="Transparent"
        }
        Blend SrcAlpha OneMinusSrcAlpha 
        LOD 100
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float3 worldPos : WORLD_POS;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert(appdata v)
            {
                v2f o;
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                o.vertex = UnityObjectToClipPos(v.vertex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float cameraToObjLength = length(_WorldSpaceCameraPos - i.worldPos);
                // fixed4 col = tex2D(_MainTex, i.uv);
                fixed4 col = tex2D(_MainTex, float2(i.worldPos.x / 100, i.worldPos.z / 100));
                col = fixed4(col.r, col.g, col.b,- col.a * cameraToObjLength / 2500 + 1.125);
                // fixed4 col = lerp(fixed4(tex2D(_MainTex, i.uv)), fixed4(1,1,1,1), cameraToObjLength /2500);
                return col;
            }
            ENDCG
        }
    }
}