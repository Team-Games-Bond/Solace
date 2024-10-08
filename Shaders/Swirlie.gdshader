shader_type spatial;
render_mode cull_disabled;
#include "res://addons/shaderV/rgba/noise/fractal/perlin4d_fractal.gdshaderinc"
uniform sampler2D noise;
uniform float shear = 3.0;
uniform float threshold = 0.5;
uniform float grow = 0.1;
uniform vec2 time_offset = vec2(0.2, 0.5);

void vertex() {
	// Called for every vertex the material is visible on.
	VERTEX=VERTEX+NORMAL*grow;
}

void fragment() {
	//ALBEDO = vec3(_perlin2dNoiseFuncFractal(UV));
	// Called for every pixel the material is visible on.
	vec2 offset= time_offset*TIME;
	vec2 uv=(UV)* mat2(vec2(1.0,shear), vec2(0.0, 1.0))+offset;
	//ALBEDO=vec3(_perlin3dPeriodicNoiseFunc(vec3(UV*10.0, 0.0), vec3(1.2312432, PI, 1)));
	if (texture(noise, uv).r>=threshold) discard;
	EMISSION=vec3(0.35);
}

//void light() {
	// Called for every pixel for every light affecting the CanvasItem.
	// Uncomment to replace the default light processing function with this one.
//}
