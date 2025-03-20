shader_type canvas_item;

void fragment() {
	vec4 input_color = texture(TEXTURE,UV);
	COLOR=vec4(input_color.x,input_color.y,input_color.z,abs(sin(TIME)));
}