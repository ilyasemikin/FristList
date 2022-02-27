using System;

namespace FristList.WebApi.Options;

public class JwtGeneratorOptions
{
    public TimeSpan ExpirePeriod { get; init; }
}