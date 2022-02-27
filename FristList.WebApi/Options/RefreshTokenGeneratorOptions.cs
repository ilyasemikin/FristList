using System;

namespace FristList.WebApi.Options;

public class RefreshTokenGeneratorOptions
{
    public TimeSpan ExpirePeriod { get; init; }
    public int Ok { get; init; }
}