using Microsoft.EntityFrameworkCore;
using System;

namespace DAL_DOTNETCORE
{
    public class User
    {
        public int id { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string phoneNumber { get; set; }
        public string tin { get; set; }
        public string passportId { get; set; }
        public string country { get; set; }
        public string city { get; set; }
        public string passwordHash { get; set; }
        public string email { get; set; }
        public User() { }
        public User(string f_name, string l_name, string p_number, string tin_numb, string passId, string country, string city, string passwHash, string email)
        {
            firstName = f_name;
            lastName = l_name;
            phoneNumber = p_number;
            tin = tin_numb;
            passportId = passId;
            this.country = country;
            this.city = city;
            passwordHash = passwHash;
            this.email = email;
        }
        public override string ToString()
        {
            return $"User: {firstName} {lastName}, email: {email}, TIN: {tin}, ID: {passportId}.";
        }
    }
}
