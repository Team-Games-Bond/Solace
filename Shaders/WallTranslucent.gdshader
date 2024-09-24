shader_type spatial;
uniform sampler2D WallGradient;
group_uniforms DistanceFade;
uniform float MIN_DISTANCE = 18.0f;
uniform float MAX_DISTANCE = 21.0f;
group_uniforms;

varying vec3 modelspace_vert;
void vertex() {
	// Called for every vertex the material is visible on.
	modelspace_vert = VERTEX;
}

void fragment() {
	// Called for every pixel the material is visible on.
	// Replicate Godot Dithering
	const vec3 magic = vec3(0.06711056, 0.00583715, 52.9829189);
	float fade = clamp(smoothstep(MIN_DISTANCE, MAX_DISTANCE, length(VERTEX)), 0.0, 1.0);
	if (fade < 0.001 || fade < fract(magic.z * fract(dot(FRAGCOORD.xy, magic.xy)))) {
		discard;
	}
	ALBEDO = textureLod(WallGradient, modelspace_vert.xy, 1.0).rgb;
}

//void light() {
	// Called for every pixel for every light affecting the material.
	// Uncomment to replace the default light processing function with this one.
//}
