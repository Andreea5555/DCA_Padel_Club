namespace DCA_Padel_Club.Core.Domain.Aggregates.Schedule;

public class CourtId
{
    private static int nextInt = 1;
    private int number;
    private CourtType Type;

    public CourtId(CourtType type)
    {
        Type = type;
        number = nextInt;
        nextInt++;

    }
}