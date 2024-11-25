shader_type spatial;
render_mode vertex_lighting;

uniform sampler2D PRIMARY_DEPTH: hint_default_white, repeat_disable;
uniform sampler2D SECONDARY_DEPTH: hint_default_white, repeat_disable;
uniform sampler2D DEPTH_TEXTURE: hint_depth_texture;
uniform float WAVE_HEIGHT = 3.0f;
uniform float NORMAL_STRENGTH = 0.06f;
uniform float NOISE_DENSITY = 12;
const int Directions = 8;
const int Quality = 3;
const vec2 Radius = vec2(5.0/256.0);
instance uniform float SECONDARY_STRENGTH: hint_range(0.0, 1.0) = 0;
instance uniform float HEIGHT_TEXTURE_STRENGTH: hint_range(0.0, 256.0) = 9.0;
instance uniform float DEPTH_FADE: hint_range(0.0, 8.0)=0.16;
instance uniform float MAX_TRANSPARENCY: hint_range(0.0, 1.0)=0.75;

#include "res://addons/shaderV/rgba/noise/perlin3d.gdshaderinc"
#include "res://addons/shaderV/rgba/noise/fractal/worley2d_fractal.gdshaderinc"

float displacement(vec2 uv){
	/*float noiseOffset = _perlin3dNoiseFunc(vec3(uv*3.0, TIME*0.3) )*0.15;
	/float textureOffset = texture(PRIMARY_DEPTH, uv).r*
		mix(1.0, texture(SECONDARY_DEPTH, uv).r, SECONDARY_STRENGTH)*
		HEIGHT_TEXTURE_STRENGTH;
	return noiseOffset+textureOffset;*/
	float noiseOffset = _perlin3dNoiseFunc(vec3(uv*NOISE_DENSITY, TIME*0.3) );
	return noiseOffset;
}

varying flat vec3 FLAT_NORMAL;

void vertex() {
	// Called for every vertex the material is visible on.
	VERTEX = vec3(VERTEX.x, VERTEX.y+(displacement(UV)*WAVE_HEIGHT), VERTEX.z);
	const vec2 e = vec2(0.01, 0);
	NORMAL = normalize(vec3(displacement(UV-e)-displacement(UV+e), e.x*2.0/NORMAL_STRENGTH, displacement(UV-e.yx)-displacement(UV+e.yx)));
	FLAT_NORMAL = MODELVIEW_NORMAL_MATRIX*NORMAL;
}

float blur(sampler2D sampler, vec2 uv){
	float transparency = texture(sampler, uv).r;
	for (float d=0.0; d<PI; d+=PI/float(Directions)){
		for (float i=1.0/float(Quality); i<=1.0; i+=1.0/float(Quality)){
			transparency += texture(sampler, uv+vec2(cos(d), sin(d))*Radius*i).r;
		}
	}
	transparency /= float(Quality*Directions+1);
	return transparency;
}

void fragment() {
    // Called for every pixel the material is visible on.
    float depth = textureLod(DEPTH_TEXTURE, SCREEN_UV, 0.0).r;
    vec4 upos = INV_PROJECTION_MATRIX * vec4(SCREEN_UV*2.0 - 1.0, depth, 1.0);
    vec3 pixelPosition = upos.xyz/upos.w;
    NORMAL = FLAT_NORMAL;
    //ALPHA = clamp(DEPTH_FADE*(VERTEX.z-pixelPosition.z), 0.0, MAX_TRANSPARENCY);

	float textureTransparency = blur(PRIMARY_DEPTH, UV)*
		mix(1.0, blur(SECONDARY_DEPTH, UV), SECONDARY_STRENGTH);
	float depthTransparency = clamp(DEPTH_FADE*(VERTEX.z-pixelPosition.z), 0.0, MAX_TRANSPARENCY);
	float noise = (_cellularNoise2DFBM(UV, 6, vec2(2), 2, 0.8, 0.5, 0.3, vec2(0.5), 1, false)-0.5)*0.2;
	ALPHA = clamp(depthTransparency*textureTransparency, 0.0, 1.0);//+noise;
}

//void light() {
	// Called for every pixel for every light affecting the material.
	// Uncomment to replace the default light processing function with this one.
//}
