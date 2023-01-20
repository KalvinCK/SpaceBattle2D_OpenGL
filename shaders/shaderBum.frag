#version 460 core

out vec4 FragColor;

in vec2 TexCoords;

uniform sampler2D inputTexture;
uniform float LightForce = 1.0f;
uniform vec4 color = vec4(1.0);

uniform float Timer;
uniform float alpha;

uniform bool enableAlpha = false;

vec4 RemoveAlpha(vec4 tex)
{
    if(tex.a == 0)
        discard;

    return tex;
}

const float velWaves = 0.01; // 0.0004 < - >  0.01

vec4 ondulation()
{
    const float spacingX = 50.0;
    const float spacingY = 50.0;
    
    vec4 result = texture( inputTexture, TexCoords + velWaves * vec2( sin(Timer + spacingX * TexCoords.x),cos(Timer + spacingY * TexCoords.y)) );
    if(enableAlpha)
    {
        result = RemoveAlpha(result);
    }

    return vec4(result.rgb, 1.0);
}

vec4 removeBlack(vec4 base)
{
    if(base.r == 0.0 && base.g == 0 && base.b == 0)
        discard;


    return base;
}
void main()
{

    vec4 result = ondulation();
    result = removeBlack(result);

    result.rgb = result.rgb * color.rgb;    

    result.rgb = result.rgb * LightForce;
    FragColor = vec4(result.rgb, alpha);
}
