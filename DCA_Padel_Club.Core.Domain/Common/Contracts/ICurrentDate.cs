namespace DCA_Padel_Club.Core.Domain.Common.Contracts;

public interface ICurrentDate
{
    DateOnly Now { get; }
}