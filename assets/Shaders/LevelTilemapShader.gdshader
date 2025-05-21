shader_type canvas_item;

uniform sampler2D viewport_texture;
uniform vec4 blockColor : source_color;  // The color of "dirt" or "block"
uniform vec4 grassColor : source_color;  // The color you want it to become

void fragment() {
    vec4 current_pixel_color = texture(viewport_texture, UV);
    vec2 texel_size = 1.0 / vec2(textureSize(viewport_texture, 0));

    // 8-connected neighbor offsets
    vec2 offsets[8] = vec2[](
        vec2(-1, -1), vec2(-1,  0), vec2(-1,  1),
        vec2( 0, -1),              vec2( 0,  1),
        vec2( 1, -1), vec2( 1,  0), vec2( 1,  1)
    );

    vec4 final_color = current_pixel_color;

    float closenessOfColor = 0.01;
    if (length(current_pixel_color.rgb - blockColor.rgb) < closenessOfColor) {
        for (int i = 0; i < 8; i++) {
            vec2 offset_uv = UV + offsets[i] * texel_size;
            offset_uv = clamp(offset_uv, vec2(0.0), vec2(1.0));

            vec4 neighbor_color = texture(viewport_texture, offset_uv);
            if (neighbor_color.a < 0.1) {
                final_color = grassColor;
                break;
            }
        }
    }

    COLOR = final_color;
}
