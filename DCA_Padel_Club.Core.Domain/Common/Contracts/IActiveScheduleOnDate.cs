namespace DCA_Padel_Club.Core.Domain.Common.Contracts;

public interface IActiveScheduleOnDate
{
    bool ExistsActiveScheduleOn(DateOnly date);
}
