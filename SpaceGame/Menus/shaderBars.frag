#version 460 core

in vec2 TexCoords;

out vec4 FragColor;

uniform sampler2D inputTexture;
uniform float Light = 1.0;
uniform vec4 color = vec4(1.0);


vec4 RemoveAlpha(sampler2D texBase)
{
    vec4 tex = texture(texBase, TexCoords);

    if(tex.a == 0)
        discard;

    return tex;
}
vec4 setColor( vec4 tex, vec4 color4)
{
    return vec4(tex.rgb * color4.rgb, tex.a);
}
vec4 ForceLight(vec4 base, float force)
{
    base.rgb = base.rgb * force;

    return base;
}

void main()
{
 
    vec4 base = RemoveAlpha(inputTexture);
    base = setColor(base, color);
    base = ForceLight(base, Light);
    
    FragColor = base;
}