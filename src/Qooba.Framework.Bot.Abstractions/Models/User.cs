namespace Qooba.Framework.Bot.Abstractions.Models
{
    public class User
    {
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string ProfilePicture { get; set; }

        public string Locale { get; set; }

        public int Timezone { get; set; }

        public Gender Gender { get; set; }
    }
}