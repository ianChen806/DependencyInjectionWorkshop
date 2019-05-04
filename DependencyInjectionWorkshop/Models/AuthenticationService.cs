﻿using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Text;
using Dapper;

namespace DependencyInjectionWorkshop.Models
{
    public class AuthenticationService
    {
        public bool Verify(string account, string inputPassword, string otp)
        {
            string passwordDb;
            using (var connection = new SqlConnection("my connection string"))
            {
                var value = connection.Query<string>("spGetUserPassword", new {Id = account},
                    commandType: CommandType.StoredProcedure).SingleOrDefault();

                passwordDb = value;
            }
            var crypt = new System.Security.Cryptography.SHA256Managed();
            var hash1 = new StringBuilder();
            var crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(inputPassword));
            foreach (var theByte in crypto)
            {
                hash1.Append(theByte.ToString("x2"));
            }
            var hash = hash1.ToString();
            if (passwordDb != hash)
            {
                return false;
            }

            var httpClient = new HttpClient() {BaseAddress = new Uri("http://joey.com/")};
            var response = httpClient.PostAsJsonAsync("api/otps", account).Result;
            if (response.IsSuccessStatusCode)
            {
            }
            else
            {
                throw new Exception($"web api error, accountId:{account}");
            }
            var newOtp = response.Content.ReadAsAsync<string>().Result;
            if (newOtp != otp)
            {
                return false;
            }

            return true;
        }
    }
}