using DCA_Padel_Club.Core.Domain.Common.Contracts;

namespace UnitTests.Fakes;

internal class FakeCurrentTime : ICurrentTime
{
    private readonly TimeOnly _timeOnly;
    private readonly DateTime _dateTime;

    public FakeCurrentTime(TimeOnly now)
    {
        _timeOnly = now;
        _dateTime = DateTime.Today.Add(now.ToTimeSpan());
    }

    public FakeCurrentTime(DateTime now)
    {
        _dateTime = now;
        _timeOnly = TimeOnly.FromDateTime(now);
    }

    public TimeOnly Now => _timeOnly;
    public DateTime CurrentTime() => _dateTime;

    public static FakeCurrentTime RealNow() => new(DateTime.Now);
}
