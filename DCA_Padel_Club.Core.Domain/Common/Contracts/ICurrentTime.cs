namespace DCA_Padel_Club.Core.Domain.Common.Contracts;

public interface ICurrentTime
{
    DateTime Now { get; }
    TimeOnly TimeOfDay { get; }
}
