using DCA_Padel_Club.Core.Domain.Common.Contracts;

namespace UnitTests.Helpers;

internal class FakeCurrentDate(DateOnly now) : ICurrentDate
{
    public DateOnly Now => now;

    public static FakeCurrentDate RealNow() => new FakeCurrentDate(DateOnly.FromDateTime(DateTime.Now));
}
