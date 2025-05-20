using ProfilesService.Domain.Model.Commands;
using ProfilesService.Domain.Model.ValueObjects;

namespace ProfilesService.Domain.Model.Aggregates;

public partial class Profile
{
    public Profile()
    {
        Name = new PersonName();
        Birth = new DateOfBirth();
        PhoneN = new Phone();
        DocumentN = new DocumentNumber();
        UserId = 0;
        BankAccountNumber = "";
        InterbankAccountNumber = "";
    }
    
    public Profile(CreateProfileCommand command)
    {
        Name = new PersonName(command.Name, command.FatherName, command.MotherName);
        Birth = new DateOfBirth(command.DateOfBirth);
        PhoneN = new Phone(command.Phone);
        DocumentN = new DocumentNumber(command.DocumentNumber);
        UserId = command.UserId;
        BankAccountNumber = "";
        InterbankAccountNumber = "";
    }
    
    public void Update(UpdateProfileCommand command)
    {
        Name = new PersonName(command.Name, command.FatherName, command.MotherName);
        Birth = new DateOfBirth(command.DateOfBirth);
        PhoneN = new Phone(command.Phone);
        DocumentN = new DocumentNumber(command.DocumentNumber);
        BankAccountNumber = command.BankAccountNumber;
        InterbankAccountNumber = command.InterbankAccountNumber;
    }

    public int Id { get; set; }
    public PersonName Name { get; private set; }
    public DateOfBirth Birth { get; private set; }
    public Phone PhoneN { get; private set; }
    public DocumentNumber DocumentN { get; private set; }
    public string BankAccountNumber { get; private set; }         
    public string InterbankAccountNumber { get; private set; } 
    
    public int UserId { get; set; }
    public string FullName => Name.FullName;
    public string BirthDate => Birth.BirthDate;
    public string PhoneNumber => PhoneN.PhoneNumber;
    public string NumberDocument => DocumentN.NumberDocument;
}