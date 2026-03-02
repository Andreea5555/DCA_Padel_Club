using DCA_Padel_Club.Core.Domain.Common.Bases;
using DCA_Padel_Club.Core.Tools.OperationResult;

namespace DCA_Padel_Club.Core.Domain.Aggregates.Players;

public class Player : AggregateRoot<ViaId>
{
    public string FirstName { get; internal set; }
    public string LastName { get; internal set; }
    public Email Email { get; internal set; }
    public Password Password { get; internal set; }
    public bool IsVip { get; internal set; }
    public bool Blacklisted { get; internal set; }
    public DateTime? CooldownExpiresAt { get; internal set; }
    public DateTime? QuarantineEndDate { get; internal set; }

    private Player(ViaId id, string firstName, string lastName, Email email, Password password)
        : base(id)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Password = password;
        IsVip = false;
        Blacklisted = false;
    }

    public static Player Register(ViaId id, string firstName, string lastName, Email email, Password password)
    {
        return new Player(id, firstName, lastName, email, password);
    }

    public void BlackListPlayer()
    {
        this.Blacklisted = true;
    }
    
    public void UnblacklistPlayer() 
    {
        this.Blacklisted = false;
    }
    
    public void QuarantinePlayer(int days)
    {
        this.QuarantineEndDate = DateTime.Now.AddDays(days);
    }
    
    public void RenewVip(int months)
    {
       
        this.IsVip = true;
    }
    public void RevokeVip()
    {
        this.IsVip = false;
    }
    public bool IsEligibleForVipCourt()
    {
        return this.IsVip && !this.Blacklisted;
    }
    
    public Result<bool> ChangePassword(string newPassword)
    {
        var passwordResult = Password.Create(newPassword);
        
        if (passwordResult.IsFailure)
        {
            return Result<bool>.Failure(new List<OperationError>(passwordResult.errorMessages));
        }

        this.Password = passwordResult.value;
        return Result<bool>.Success(true);
    }
    
    public void ChangeEmail(Email newEmail)
    {
        this.Email = newEmail;
    }
    
    public bool IsEligibleToBook()
    {
        if (this.Blacklisted)
        {
            return false;
        }

        if (this.QuarantineEndDate.HasValue && this.QuarantineEndDate.Value > DateTime.Now)
        {
            return false;
        }

        if (this.CooldownExpiresAt.HasValue && this.CooldownExpiresAt.Value > DateTime.Now)
        {
            return false;
        }
        return true;
    }
}