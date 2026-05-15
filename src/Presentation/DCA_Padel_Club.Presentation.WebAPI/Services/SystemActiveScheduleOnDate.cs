using DCA_Padel_Club.Core.Domain.Common.Contracts;

namespace DCA_Padel_Club.Presentation.WebAPI.Services;

public class SystemActiveScheduleOnDate : IActiveScheduleOnDate
{
    public bool ExistsActiveScheduleOn(DateOnly date)
    {
        return false;
    }
}