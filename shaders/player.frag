#version 460 core

out vec4 FragColor;

in vec2 TexCoords;

uniform sampler2D inputTexture;
uniform float LightForce;
uniform float alpha;

vec4 RemoveAlpha(sampler2D tex)
{
    vec4 result = texture(tex, TexCoords);

    if(result.a == 0)
        discard;

    return result;
}
vec3 EmissiveLuminous(sampler2D texEmissive, float force)
{
    vec4 emissive = texture(texEmissive, TexCoords);
    if(emissive.a <= 0.1)
        discard;

    return emissive.rgb * force;
}

void main()
{
    vec4 result = RemoveAlpha(inputTexture);

    result.rgb = result.rgb * LightForce;

    FragColor = vec4(result.rgb, alpha);
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