using DCA_Padel_Club.Core.Domain.Common.Contracts;

namespace UnitTests.Helpers;

internal class FakeCurrentTime(TimeOnly now) : ICurrentTime
{
    public TimeOnly Now => now;

    public static FakeCurrentTime RealNow() => new FakeCurrentTime(TimeOnly.FromDateTime(DateTime.Now));
}
