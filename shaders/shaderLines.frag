#version 460 core

out vec4 FragColor;

in vec2 TexCoords;


uniform vec4 color = vec4(1.0);
uniform float alpha;

void main()
{

    FragColor = vec4(color.rgb, alpha);
}
