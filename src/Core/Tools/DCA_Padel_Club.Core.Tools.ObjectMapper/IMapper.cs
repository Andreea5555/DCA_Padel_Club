namespace DCA_Padel_Club.Core.Tools.ObjectMapper;

public interface IMapper
{
    TOutput Map<TOutput>(object input) where TOutput : class;
}
