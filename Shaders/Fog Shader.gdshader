shader_type spatial;
render_mode vertex_lighting;

uniform sampler2D PRIMARY_DEPTH: hint_default_white;
uniform sampler2D SECONDARY_DEPTH: hint_default_white;
uniform sampler2D DEPTH_TEXTURE: hint_depth_texture;
instance uniform float SECONDARY_STRENGTH: hint_range(0.0, 1.0) = 0;
instance uniform float HEIGHT_TEXTURE_STRENGTH: hint_range(0.0, 256.0) = 1.0;
instance uniform float DEPTH_FADE: hint_range(0.0, 8.0)=1.0;
instance uniform float MAX_TRANSPARENCY: hint_range(0.0, 1.0)=0.6;

#include "res://addons/shaderV/rgba/noise/perlin3d.gdshaderinc"

float displacement(vec2 uv){
	float noiseOffset = _perlin3dNoiseFunc(vec3(uv*3.0, TIME*0.3) )*0.15;
	float textureOffset = texture(PRIMARY_DEPTH, uv).r*
		mix(1.0, texture(SECONDARY_DEPTH, uv).r, SECONDARY_STRENGTH)*
		HEIGHT_TEXTURE_STRENGTH;
	return noiseOffset+textureOffset;
}

varying flat vec3 FLAT_NORMAL;

void vertex() {
	// Called for every vertex the material is visible on.
	VERTEX = vec3(VERTEX.x, VERTEX.y+displacement(VERTEX.xz), VERTEX.z);
	const vec2 e = vec2(0.01, 0);
	FLAT_NORMAL = normalize(vec3(displacement(VERTEX.xz-e)-displacement(VERTEX.xz+e), e.x*2.0, displacement(VERTEX.xz-e.yx)-displacement(VERTEX.xz+e.yx)));
}

void fragment() {
	// Called for every pixel the material is visible on.
	float depth = textureLod(DEPTH_TEXTURE, SCREEN_UV, 0.0).r;
	vec4 upos = INV_PROJECTION_MATRIX * vec4(SCREEN_UV*2.0 - 1.0, depth, 1.0);
	vec3 pixelPosition = upos.xyz/upos.w;
	NORMAL = FLAT_NORMAL;
	ALPHA = clamp(DEPTH_FADE*(VERTEX.z-pixelPosition.z), 0.0, MAX_TRANSPARENCY);
}

//void light() {
	// Called for every pixel for every light affecting the material.
	// Uncomment to replace the default light processing function with this one.
//}
