using DCA_Padel_Club.Core.Domain.Common.Contracts;

namespace IntegrationTests.Fakes;

public class FakeCurrentDate: ICurrentDate
{
    public DateOnly Now => DateOnly.FromDateTime(DateTime.Now);

}