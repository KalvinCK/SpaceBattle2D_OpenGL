#version 460 core


out vec4 FragColor;

in vec2 TexCoords;

uniform sampler2D inputTexture;

uniform float LifeLevel;

uniform float alpha = 1.0f;

vec4 RemoveAlpha(vec4 tex)
{
    if(tex.a == 0)
        discard;

    return tex;
}


void main()
{
    const vec4 colors[10] =
    {
        vec4(1.0, 0.0, 0.0, 1.0),
        vec4(1.0, 0.0, 0.0, 1.0),
        vec4(1.0, 0.1, 0.0, 1.0),
        vec4(1.0, 0.2, 0.0, 1.0),
        vec4(1.0, 0.6, 0.0, 1.0),
        vec4(1.0, 0.8, 0.0, 1.0),
        vec4(0.8, 1.0, 0.0, 1.0),
        vec4(0.6, 1.0, 0.0, 1.0),
        vec4(0.7, 1.0, 0.0, 1.0),
        vec4(0.28, 1.0, 0.0, 1.0),
    };
 
    vec4 base = RemoveAlpha(texture(inputTexture, TexCoords));

    float level = LifeLevel * 0.01;

    vec4 lifeResult = vec4(1.0);
    if (TexCoords.x < level - 0.05 && TexCoords.y > 0.3 && TexCoords.y < 0.7 && TexCoords.x > 0.04 )
	{	
        float cont = 1.0;
        for(int i = colors.length() - 1; i > 0; i--)
        {
            if(level <= cont)
            {
                lifeResult = colors[i];
            }
            cont -= 0.1;
        }
	}
    
    lifeResult = lifeResult * 2.0; 

    FragColor = vec4(base.rgb * lifeResult.rgb, alpha);
}