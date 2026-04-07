using DCA_Padel_Club.Core.Domain.Common.Contracts;

namespace UnitTests.Fakes.ScheduleFakes;

internal class FakeActiveScheduleOnDate(bool exists) : IActiveScheduleOnDate
{
    public bool ExistsActiveScheduleOn(DateOnly date) => exists;
}
