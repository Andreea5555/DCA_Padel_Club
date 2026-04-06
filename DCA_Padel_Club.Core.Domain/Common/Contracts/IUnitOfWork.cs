
namespace DCA_Padel_Club.Core.Domain.Common.Contracts;
using System.Threading.Tasks;

public interface IUnitOfWork
{
    Task SaveChangesAsync();
}