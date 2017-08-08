Shader "GuitarUnity/Diffuse" {
Properties {
	_Color ("Main Color", Color) = (1,1,1,1)
	_MainTex ("Base (RGB)", 2D) = "white" {}
}

// ---- Dual texture cards
SubShader {
	Tags { "RenderType"="Opaque" }
	LOD 80
	
	// Non-lightmapped
	Pass {
		Tags { "LightMode" = "Vertex" }
		
		Material {
			Diffuse (1,1,1,1)
			Ambient (1,1,1,1)
		} 
		Lighting On
		SetTexture [_MainTex] {
			constantColor [_Color]
			Combine texture lerp(texture) constant DOUBLE, texture
		} 
	}
	
	// Lightmapped, encoded as dLDR
	Pass {
		Tags { "LightMode" = "VertexLM" }
		
		BindChannels {
			Bind "Vertex", vertex
			Bind "normal", normal
			Bind "texcoord1", texcoord0 // lightmap uses 2nd uv
			Bind "texcoord", texcoord1 // main uses 1st uv
		}
		
		SetTexture [unity_Lightmap] {
			matrix [unity_LightmapMatrix]
			combine texture
		}
		SetTexture [_MainTex] {
			constantColor [_Color]
			combine texture lerp(texture) constant DOUBLE, texture
		}
	}
	
	// Lightmapped, encoded as RGBM
	Pass {
		Tags { "LightMode" = "VertexLMRGBM" }
		
		BindChannels {
			Bind "Vertex", vertex
			Bind "normal", normal
			Bind "texcoord1", texcoord0 // lightmap uses 2nd uv
			Bind "texcoord", texcoord1 // main uses 1st uv
		}
		
		SetTexture [unity_Lightmap] {
			matrix [unity_LightmapMatrix]
			combine texture * texture alpha DOUBLE
		}
		SetTexture [_MainTex] {
			constantColor [_Color]
			combine texture lerp(texture) constant DOUBLE, texture
		}
	}	
}

// ---- Single texture cards (requires 2 passes for lightmapped)
SubShader {
	Tags { "RenderType"="Opaque" }
	LOD 100

	// Non-lightmapped
	Pass {
		Tags { "LightMode" = "Vertex" }
		
		Material {
			Diffuse (1,1,1,1)
			Ambient (1,1,1,1)
		} 
		Lighting On
		SetTexture [_MainTex] {
			constantColor [_Color]
			Combine texture lerp(texture) constant DOUBLE, texture
		} 
	}	
	// Lightmapped, encoded as dLDR
	Pass {
		// 1st pass - sample Lightmap
		Tags { "LightMode" = "VertexLM" }

		BindChannels {
			Bind "Vertex", vertex
			Bind "texcoord1", texcoord0 // lightmap uses 2nd uv
		}		
		SetTexture [unity_Lightmap] {
			matrix [unity_LightmapMatrix]
			combine texture
		}
	}
	Pass {
		// 2nd pass - multiply with _MainTex
		Tags { "LightMode" = "VertexLM" }
		ZWrite Off
		Fog {Mode Off}
		Blend DstColor Zero
		SetTexture [_MainTex] {
			constantColor [_Color]
			combine texture
		}
	}
}
}