void SampleGradient_float(float inputValue,
                   float4 color1,
                   float4 color2,
                   float4 color3,
                   float location1,
                   float location2,
                   float location3,
                   out float4 outFloat)
{
    if(inputValue < location1)
    {
        outFloat = color1;
    }
    else if (inputValue < location2)
    {
        outFloat = color2;
    }
    else
    {
        outFloat = color3;
    }
}