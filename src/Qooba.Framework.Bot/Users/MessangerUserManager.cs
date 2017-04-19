﻿using Qooba.Framework.Bot.Abstractions;
using System.Threading.Tasks;
using Qooba.Framework.Bot.Abstractions.Models;
using System.Net.Http;
using Qooba.Framework.Serialization.Abstractions;

namespace Qooba.Framework.Bot.Users
{
    public class MessangerUserManager : IUserManager
    {
        private readonly IBotConfig config;

        private readonly ISerializer serializer;

        public MessangerUserManager(IBotConfig botConfig, ISerializer serializer)
        {
            this.config = botConfig;
            this.serializer = serializer;
        }

        public async Task<User> GetUserAsync(string userId)
        {
            var user = new User
            {
                Id = userId
            };

            var userAdditionalData = await GetAdditionalDataAsync(userId);
            if (userAdditionalData != null)
            {
                user.FirstName = userAdditionalData.First_name;
                user.LastName = userAdditionalData.Last_name;
                user.ProfilePicture = userAdditionalData.Profile_pic;
                user.Locale = userAdditionalData.Locale;
                user.Timezone = userAdditionalData.Timezone;
                user.Gender = userAdditionalData.Gender;
            }

            return user;
        }

        private async Task<UserResponse> GetAdditionalDataAsync(string userId)
        {
            using (var client = new HttpClient())
            {
                var accessToken = this.config.MessangerAccessToken;
                var response = await client.GetAsync($"https://graph.facebook.com/v2.6/{userId}?fields=first_name,last_name,profile_pic,locale,timezone,gender&access_token={accessToken}");
                var responseString = await response.Content.ReadAsStringAsync();
                return this.serializer.Deserialize<UserResponse>(responseString);
            }
        }

        class UserResponse
        {
            public string First_name { get; set; }

            public string Last_name { get; set; }

            public string Profile_pic { get; set; }

            public string Locale { get; set; }

            public int Timezone { get; set; }

            public Gender Gender { get; set; }
        }
    }
}