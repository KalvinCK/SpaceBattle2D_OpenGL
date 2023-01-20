#version 460 core

out vec4 FragColor;

in vec2 TexCoords;
uniform sampler2D inputTexture;

uniform float LightForce;

uniform vec4 color = vec4(1.0);
uniform float alpha = 1.0;

uniform bool disableAlpha = false;

vec4 RemoveAlpha(vec4 tex)
{
    if(tex.a == 0)
        discard;

    return tex;
}

void main()
{
    vec4 result = RemoveAlpha(texture(inputTexture, TexCoords));
    result.rgb = result.rgb * color.rgb;    

    result.rgb = result.rgb * LightForce;

    if(disableAlpha)
    {
        FragColor = result;
    }
    else
    {
        FragColor = vec4(result.rgb, alpha);
    }
}

// vec3 getNormalFromMap()
// {
//     vec3 tangentNormal = texture(maps.NormalMap, TexCoords).xyz * 2.0 - 1.0;

//     vec3 Q1  = dFdx(WorldPos);
//     vec3 Q2  = dFdy(WorldPos);
//     vec2 st1 = dFdx(TexCoords);
//     vec2 st2 = dFdy(TexCoords);

//     vec3 N   = normalize(Normal);
//     vec3 T  = normalize(Q1*st2.t - Q2*st1.t);
//     vec3 B  = -normalize(cross(N, T));
//     mat3 TBN = mat3(T, B, N);

//     return normalize(TBN * tangentNormal);
// }