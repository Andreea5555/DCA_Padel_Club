namespace DCA_Padel_Club.Core.Domain.Common.Contracts;

public interface ICurrentTime
{
    TimeOnly Now { get; }
}