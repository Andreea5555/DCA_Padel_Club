using DCA_Padel_Club.Core.Domain.Common.Bases;
using DCA_Padel_Club.Core.Tools.OperationResult;

namespace DCA_Padel_Club.Core.Domain.Aggregates.Players;

public class Player : AggregateRoot<ViaId>
{
    public Name FirstName { get; internal set; }
    public Name LastName { get; internal set; }
    public Email Email { get; internal set; }
    public Password Password { get; internal set; }
    public ProfilePicture ProfilePicture { get; internal set; }
    public bool IsVip { get; internal set; }
    public bool Blacklisted { get; internal set; }
    public DateTime? CooldownExpiresAt { get; internal set; }
    public DateTime? QuarantineEndDate { get; internal set; }

    private Player(ViaId id, Name firstName, Name lastName, Email email, Password password, ProfilePicture profilePicture)
        : base(id)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Password = password;
        ProfilePicture = profilePicture;
        IsVip = false;
        Blacklisted = false;
    }

    public static Result<Player> Register(ViaId id, string firstName, string lastName, string email, string password, string profilePictureUri)
    {
        var errors = new List<OperationError>();

        var emailResult = Email.Create(email);
        if (emailResult.IsFailure)
        {
            errors.AddRange(emailResult.errorMessages);
        }

        var firstNameResult = Name.Create(firstName);
        if (firstNameResult.IsFailure)
        {
            errors.AddRange(firstNameResult.errorMessages);
        }

        var lastNameResult = Name.Create(lastName);
        if (lastNameResult.IsFailure)
        {
            errors.AddRange(lastNameResult.errorMessages);
        }

        var passwordResult = Password.Create(password);
        if (passwordResult.IsFailure)
        {
            errors.AddRange(passwordResult.errorMessages);
        }

        var profilePictureResult = ProfilePicture.Create(profilePictureUri);
        if (profilePictureResult.IsFailure)
        {
            errors.AddRange(profilePictureResult.errorMessages);
        }

        if (errors.Any())
        {
            return Result<Player>.Failure(errors);
        }

        var player = new Player(id, firstNameResult.value, lastNameResult.value, emailResult.value, passwordResult.value, profilePictureResult.value);
        return Result<Player>.Success(player);
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