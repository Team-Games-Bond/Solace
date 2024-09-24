shader_type spatial;
uniform sampler2D WallGradient;

varying vec3 modelspace_vert;
void vertex() {
	// Called for every vertex the material is visible on.
	modelspace_vert = VERTEX;
}

void fragment() {
	// Called for every pixel the material is visible on.
	ALBEDO = textureLod(WallGradient, modelspace_vert.xy, 1.0).rgb;
}

//void light() {
	// Called for every pixel for every light affecting the material.
	// Uncomment to replace the default light processing function with this one.
//}
