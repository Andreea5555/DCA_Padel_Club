namespace UnitTests.Helpers;

internal static class TestDefaults
{
    public static readonly FakeCurrentDate Now = FakeCurrentDate.RealNow();
    public static readonly FakeCurrentTime Midnight = new(new TimeOnly(0, 0));
    public static readonly FakeActiveScheduleOnDate NoConflict = new(false);
}
