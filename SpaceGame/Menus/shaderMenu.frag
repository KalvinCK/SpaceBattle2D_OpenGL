#version 460 core

in vec2 TexCoords;

out vec4 FragColor;

uniform sampler2D inputTexture;
uniform vec4 color = vec4(1.0);

uniform bool moveTexture;
uniform float Timer;

vec4 RemoveAlpha(sampler2D texBase)
{
    vec4 tex = texture(texBase, TexCoords);

    if(tex.a == 0)
        discard;

    return tex;
}
vec4 ForceLight(vec4 base, float force)
{
    base.rgb = base.rgb * force;

    return base;
}
vec4 ForceColor(vec4 base, vec4 color4)
{
    base.rgb = base.rgb * color4.rgb;
    return base;
}
void main()
{
    vec4 base;

    if(moveTexture)
    {
        base = texture(inputTexture, vec2(TexCoords.x + Timer, TexCoords.y));
    }
    else
    {
        base = RemoveAlpha(inputTexture);
    }
    base = ForceColor(base, color);
    base = ForceLight(base, 1.0f);

    FragColor = base;
}