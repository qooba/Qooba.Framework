namespace Qooba.Framework.Bot.Abstractions.Models
{
    public class User
    {
        string FirstName { get; set; }

        string LastName { get; set; }

        string ProfilePicture { get; set; }

        string Locale { get; set; }

        int Timezone { get; set; }

        Gender Gender { get; set; }
    }
}