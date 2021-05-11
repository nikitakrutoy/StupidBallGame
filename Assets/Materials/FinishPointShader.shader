Shader "Unlit/FinishPointShader"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _Direction ("Direction", Vector) = (0, 1, 0, 0)
        _Height ("Height", float) = 1
    }
    SubShader
    {
        Tags { "Queue" = "Transparent" "IGNOREPROJECTOR"="true" "RenderType"="Transparent"}
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        ZWrite Off
        LOD 100
        Pass
        {
            CGPROGRAM
// Upgrade NOTE: excluded shader from DX11; has structs without semantics (struct v2f members localPos)
#pragma exclude_renderers d3d11
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            fixed4 _Color;
            float4 _Direction;
            float _Height;

            
            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float3 localPos : TEXCOORD2;
                float4 vertex : SV_POSITION;
            };


            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.localPos = v.vertex.xyz;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // float h = dot(i.localPos, _Direction.xyz);
                UNITY_APPLY_FOG(i.fogCoord, col);
                float t = 0.2 * sin(_Time * 100) + 0.2;
                _Color.w = 1 - clamp(i.localPos.y / (_Height - t)  + 0.5 , 0, 1);
                return _Color;
            }
            ENDCG
        }
    }
}
