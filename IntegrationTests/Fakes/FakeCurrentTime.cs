using DCA_Padel_Club.Core.Domain.Common.Contracts;

namespace IntegrationTests.Fakes;

public class FakeCurrentTime : ICurrentTime
{
    private readonly TimeOnly _timeOnly;
    private readonly DateTime _dateTime;

    public FakeCurrentTime(DateTime now)
    {
        _dateTime = now;
        _timeOnly = TimeOnly.FromDateTime(now);
    }

    public DateTime Now => _dateTime;
    public TimeOnly TimeOfDay => _timeOnly;
}
