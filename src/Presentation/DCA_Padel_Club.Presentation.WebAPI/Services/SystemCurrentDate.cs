using DCA_Padel_Club.Core.Domain.Common.Contracts;

namespace DCA_Padel_Club.Presentation.WebAPI.Services;

public class SystemCurrentDate : ICurrentDate
{
    public DateOnly Now => DateOnly.FromDateTime(DateTime.Now);
}