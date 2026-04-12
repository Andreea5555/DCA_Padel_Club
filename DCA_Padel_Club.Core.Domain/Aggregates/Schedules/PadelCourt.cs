using DCA_Padel_Club.Core.Domain.Aggregates.Schedules;

namespace DCA_Padel_Club.Core.Domain.Aggregates.Schedules;

public class PadelCourt
{
    internal CourtId Number { get; set; }
    internal Boolean isOccupied;

    public PadelCourt(CourtId number)
    {
     Number = number;
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
    
    
    //after a discussion it has been decided to delete the VIP use cases from the codebase
    // public void EnableVIPAccess()
    // {
    //     isVIPEnabled = true;
    // }
    //
    // public void DisableVIPAccess()
    // {
    //     isVIPEnabled = false;
    // }

    public string GetID()
    {
        return Number.GetValue();
    }
}