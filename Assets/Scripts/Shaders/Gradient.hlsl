void SampleGradient_float(float inputValue,
                   float4 color1,
                   float4 color2,
                   float location1,
                   float location2,
                   out float4 outFloat)
{
    if(inputValue<location1)
    {
        outFloat = color1;
    }
    else
    {
        outFloat = color2;
    }
}