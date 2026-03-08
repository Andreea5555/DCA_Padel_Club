namespace DCA_Padel_Club.Core.Domain.Aggregates.Schedule;

public class PadelCourt
{
    internal CourtId Number { get; set; }
    internal Boolean isVIPEnabled;
    internal Boolean isOccupied;

    public PadelCourt(CourtId number)
    {
     Number = number;
     isVIPEnabled = false;
     isOccupied = false;
    }

    public void MarkAsOccupied()
    {
        isOccupied = true;
    }

    public void MarkAsAvailable()
    {
        isOccupied = false;
    }

    public void EnableVIPAccess()
    {
        isVIPEnabled = true;
    }

    public void DisableVIPAccess()
    {
        isVIPEnabled = false;
    }

}