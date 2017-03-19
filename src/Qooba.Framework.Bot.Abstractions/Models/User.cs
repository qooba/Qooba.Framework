namespace Qooba.Framework.Bot.Abstractions.Models
{
    public class User
    {
        string Id { get; set; }

        string FirstName { get; set; }

        string LastName { get; set; }

        string ProfilePicture { get; set; }

        string Locale { get; set; }

        int Timezone { get; set; }

        Gender Gender { get; set; }
    }
}