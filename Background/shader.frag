#version 460 core

in vec2 TexCoords;

out vec4 FragColor;

uniform sampler2D inputTexture;;


uniform float gamma = 1.0;
uniform float interpolation= 0.9;

vec3 CorrectionGamma(vec3 base)
{
    return pow(base, vec3(1.0 /(gamma + 1.0)));
}
vec3 Luminous(vec3 base)
{
    vec3 envColor = pow(base, vec3(1.0/ (gamma + 1.0) ));
    vec3 envColorLum = pow(base, vec3(1.0/gamma));

    return mix(envColor, envColorLum, interpolation);
}
void main()
{
    lowp vec3 base = texture(inputTexture, TexCoords).rgb;

    FragColor = vec4(Luminous(base), 1.0);

}