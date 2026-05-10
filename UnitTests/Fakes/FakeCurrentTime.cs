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

    public DateTime Now => _dateTime;
    public TimeOnly TimeOfDay => _timeOnly;

    public static FakeCurrentTime RealNow() => new(DateTime.Now);
}
