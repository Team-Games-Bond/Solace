shader_type spatial;
render_mode vertex_lighting;

uniform sampler2D PRIMARY_DEPTH: hint_default_white;
uniform sampler2D SECONDARY_DEPTH: hint_default_white;
uniform sampler2D DEPTH_TEXTURE: hint_depth_texture;
uniform float WAVE_HEIGHT = 3.0f;
uniform float NORMAL_STRENGTH = 0.06f;
uniform float NOISE_DENSITY = 12;
instance uniform float SECONDARY_STRENGTH: hint_range(0.0, 1.0) = 0;
instance uniform float HEIGHT_TEXTURE_STRENGTH: hint_range(0.0, 256.0) = 9.0;
instance uniform float DEPTH_FADE: hint_range(0.0, 8.0)=1.0;
instance uniform float MAX_TRANSPARENCY: hint_range(0.0, 1.0)=0.6;

#include "res://addons/shaderV/rgba/noise/perlin3d.gdshaderinc"

float displacement(vec2 uv){
	float noiseOffset = _perlin3dNoiseFunc(vec3(uv*NOISE_DENSITY, TIME*0.3) );
	return noiseOffset;
}

varying flat vec3 FLAT_NORMAL;

void vertex() {
	// Called for every vertex the material is visible on.
	VERTEX = vec3(VERTEX.x, VERTEX.y+displacement(UV), VERTEX.z)*WAVE_HEIGHT;
	const vec2 e = vec2(0.01, 0);
	FLAT_NORMAL = normalize(vec3(displacement(UV-e)-displacement(UV+e), e.x*2.0/NORMAL_STRENGTH, displacement(UV-e.yx)-displacement(UV+e.yx)));
}

void fragment() {
    // Called for every pixel the material is visible on.
    float depth = textureLod(DEPTH_TEXTURE, SCREEN_UV, 0.0).r;
    vec4 upos = INV_PROJECTION_MATRIX * vec4(SCREEN_UV*2.0 - 1.0, depth, 1.0);
    vec3 pixelPosition = upos.xyz/upos.w;
    NORMAL = FLAT_NORMAL;
	float textureTransparency = texture(PRIMARY_DEPTH, UV).r*
		mix(1.0, texture(SECONDARY_DEPTH, UV).r, SECONDARY_STRENGTH);
	float depthTransparency = clamp(DEPTH_FADE*(VERTEX.z-pixelPosition.z), 0.0, MAX_TRANSPARENCY);
	ALPHA = depthTransparency*textureTransparency;
}

//void light() {
	// Called for every pixel for every light affecting the material.
	// Uncomment to replace the default light processing function with this one.
//}
