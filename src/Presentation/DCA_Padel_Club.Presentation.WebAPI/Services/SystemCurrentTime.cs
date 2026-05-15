using DCA_Padel_Club.Core.Domain.Common.Contracts;

namespace DCA_Padel_Club.Presentation.WebAPI.Services;

public class SystemCurrentTime : ICurrentTime
{
    public DateTime Now => DateTime.Now;
    public TimeOnly TimeOfDay => TimeOnly.FromDateTime(DateTime.Now);
}