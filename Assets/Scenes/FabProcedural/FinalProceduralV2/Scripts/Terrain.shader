Shader "Custom/Terrain_URP" {
    Properties {
        testTexture("Texture", 2D) = "white" {}
        testScale("Scale", Range(0.1, 10)) = 1.0
    }

    SubShader {
        Tags { "RenderType"="Opaque"}
        LOD 200

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f {
                float3 worldPos : TEXCOORD0;
                float3 worldNormal : TEXCOORD1;
                float4 vertex : SV_POSITION;
            };

            sampler2D testTexture;
            float testScale;

			const static int maxLayerCount = 8;
			const static float epsilon = 1E-4;

			int layerCount;
			float3 baseColours[maxLayerCount];
			float baseStartHeights[maxLayerCount];
			float baseBlends[maxLayerCount];
			float baseColourStrength[maxLayerCount];
			float baseTextureScales[maxLayerCount];

			float minHeight;
			float maxHeight;

            v2f vert(appdata_t v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.worldNormal = mul((float3x3)unity_WorldToObject, v.normal);
                return o;
            }

			float3 triplanar(float3 worldPos, float scale, float3 blendAxes, int textureIndex) {
                float3 scaledWorldPos = worldPos / scale;
                float3 xProjection = tex2D(testTexture, scaledWorldPos.yz).rgb * blendAxes.x;
                float3 yProjection = tex2D(testTexture, scaledWorldPos.xz).rgb * blendAxes.y;
                float3 zProjection = tex2D(testTexture, scaledWorldPos.xy).rgb * blendAxes.z;
                return xProjection + yProjection + zProjection;
            }

			float inverseLerp(float a, float b, float value) {
			return saturate((value-a)/(b-a));
			}

            float3 frag(v2f i) : SV_Target {
				float heightPercent = inverseLerp(minHeight,maxHeight, i.worldPos.y);
                float3 blendAxes = abs(i.worldNormal) / (i.worldNormal.x + i.worldNormal.y + i.worldNormal.z);


				float3 finalAlbedo = float3(0, 0, 0); // Initialize final albedo to black (you can set it to your default value)

				for (int layerIndex = 0; layerIndex < layerCount; layerIndex++) {
					
					float drawStrength = inverseLerp(-baseBlends[layerIndex]/2 - epsilon, baseBlends[layerIndex]/2, heightPercent - baseStartHeights[layerIndex]);

					float3 baseColor = baseColours[layerIndex] * baseColourStrength[layerIndex];
					float3 textureSample = triplanar(i.worldPos, baseTextureScales[layerIndex], blendAxes, layerIndex) * (1-baseColourStrength[layerIndex]);


					finalAlbedo = finalAlbedo * (1-drawStrength) + (baseColor+textureSample) * drawStrength;
				}
				return finalAlbedo;
            }
            ENDCG
        }
    }

    FallBack "Diffuse"
}
