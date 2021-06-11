using System;
using System.Collections.Generic;
using System.Text;

namespace DAL_DOTNETCORE
{
    public class Wallet
    {
        public int id { get; set; }
        public string walletName { get; set; }
        public string walletNumber { get; set; }
        public decimal walletBalance { get; set; }
        public string walletOwnerPassId { get; set; }
        public string walletOwnerTIN { get; set; }
        public string walletDate { get; set; }
        public string walletCVV { get; set; }

        public Wallet() { }
        public Wallet(string ownerPassportId, string ownerTin, string w_name, string w_numb, DateTime w_date, string w_cvv)
        {
            walletName = w_name;
            walletOwnerPassId = ownerPassportId;
            walletOwnerTIN = ownerTin;
            walletNumber = w_numb;
            walletDate = Convert.ToString(w_date);
            walletCVV = w_cvv;
            walletBalance = 0m;
        }
        public override string ToString()
        {
            return $"Wallet name: {walletName}, wallet owner's Passport ID: {walletOwnerPassId} and TIN: {walletOwnerTIN}, Wallet number: {walletNumber}, wallet date: {walletDate}, wallet CVV: {walletCVV}. Balance: {walletBalance} UAH.";
        }
    }
}
