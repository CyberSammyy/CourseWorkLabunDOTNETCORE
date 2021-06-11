using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DAL_DOTNETCORE;

namespace BLL_DOTNETCORE
{
    public static class UserProceduresProviderNONStatic
    {
        public static UserProceduresProvider instance = new UserProceduresProvider();
    }
    public class UserProceduresProvider
    {
        int extraInstancesCounter = 0;
        public UserProceduresProvider()
        {
            if (extraInstancesCounter > 0) throw new MoreThanOneINstanceException("Here can be only ONE instance!!!");
            extraInstancesCounter++;
        }
        public static IEnumerable<string> GetWalletsStrings(string ownerPassId, string ownerTin)
        {
            return DataBaseIOProvider.GetWalletsStrings(ownerPassId, ownerTin);
        }
        public (bool, string) CheckUser(string firstName, string lastName,
                                        string phoneNumber, string passwordHash,
                                        string email)
        {
            return DataBaseIOProviderNONStatic.instance.CheckUser(firstName, lastName, phoneNumber, passwordHash, email);
        }
        /// <summary>
        /// Returns passport id and tin by email
        /// </summary>
        /// <param name="email"></param>
        /// <returns>1 - TIN, 2 - PASSPORT ID</returns>
        public static (string, string) GetTINAndPassportID(string email)
        {
            return DataBaseIOProvider.GetTINAndPassportID(email);
        }
        public async Task<(bool, string)> ChangeUserData(string newUserLastName, string newUserFirstName,
                                                         string newUserCity, string newUserCountry,
                                                         string newUserPhoneNumber, string newUserTin,
                                                         string newUserPassportId, string newUserEmail,
                                                         string oldUserPasspId, string oldUserTin,
                                                         string passInput, string passConfirm)
        {
            return await DataBaseIOProviderNONStatic
                            .instance
                            .ChangeUserData(newUserLastName, newUserFirstName, 
                                            newUserCity, newUserCountry, 
                                            newUserPhoneNumber, newUserTin, 
                                            newUserPassportId, newUserEmail, 
                                            oldUserPasspId, oldUserTin, 
                                            passInput, passConfirm);
        }
        public void RemoveWallet(string name, string number)
        {
            DataBaseIOProviderNONStatic.instance.RemoveWallet(name, number);
        }
        public void CreateWallet(string walletName, string ownerPasspId, string ownerTin)
        {
            DataBaseIOProviderNONStatic
                .instance
                .CreateWallet(walletName, ownerPasspId, ownerTin);
        }
        public static IEnumerable<string> GetUsersStrings()
        {
            return DataBaseIOProvider.GetUsersStrings();
        }
        public static void RenameWallet(string oldName, string number, string newName)
        {
            DataBaseIOProvider.RenameWallet(oldName, number, newName);
        }
        public async Task<(bool, string)> AddRemoveMoney(decimal sum, string walletNumber)
        {
            return await DAL_DOTNETCORE
                            .DataBaseInteractionsNONStatic
                            .instance
                            .AddRemoveMoney(sum, walletNumber);
        }
        public async Task<(bool, string)> AddTransaction(string senderWalletNumber, string receiverWalletNumber, string category, decimal sum)
        {
            return await DAL_DOTNETCORE
                            .DataBaseInteractionsNONStatic
                            .instance
                            .SaveTransactionInfo(senderWalletNumber, receiverWalletNumber, category, sum);
        }
        public static void ChangeCategories(string oldCategory, string newCategory, string userWalletNumber = null)
        {
            DAL_DOTNETCORE
                .DataBaseInteractionsNONStatic
                .instance
                .ChangeCategories(oldCategory, newCategory, userWalletNumber);
        }
        public static IEnumerable<string> GetHistoryStrings(string senderWalletNumber = "All", string category = null)
        {
            return DAL_DOTNETCORE
                        .DataBaseInteractions
                        .GetHistoryStrings(senderWalletNumber, category);
        }
        public async Task<(bool, string)> PerformTransaction(decimal sum, string senderWalletNumber, string CVV, string dateMonth, string dateYear, string receiverWalletNumber = null)
        {
            return await DAL_DOTNETCORE
                            .DataBaseInteractionsNONStatic
                            .instance
                            .PerformTransaction(sum, receiverWalletNumber, CVV, dateMonth, dateYear, senderWalletNumber);
        }
        public static bool IsActiveWallets(string ownerPassId, string ownerTin)
        {
            var wallets = DAL_DOTNETCORE
                            .DataBaseInteractions
                            .GetWallets(ownerPassId, ownerTin);
            if ((wallets as List<Wallet>).Count > 0)
                return true;
            else return false;
        }
        public async void RemoveUser(string userPassId, string userTin)
        {
            await DAL_DOTNETCORE
                    .DataBaseInteractionsNONStatic
                    .instance
                    .RemoveUser(userPassId, userTin);
        }
        public async void RemoveWalletRange(List<(string, string)> nameNumbers, string ownerPassId, string ownerTin)
        {
            List<Wallet> walletsToRemove = new List<Wallet>();
            foreach(var nameNumber in nameNumbers)
            {
                walletsToRemove.Add(DAL_DOTNETCORE
                                        .DataBaseInteractions
                                        .GetWallet(nameNumber.Item2, nameNumber.Item1, ownerPassId, ownerTin));
            }
            await DAL_DOTNETCORE.DataBaseInteractionsNONStatic.instance.RemoveWallets(walletsToRemove);
        }
        public async Task<(bool, string)> RegisterUser(string firstNameInput,
                                                       string lastNameInput,
                                                       string countryInput,
                                                       string cityInput,
                                                       string phoneInput,
                                                       string idInput,
                                                       string tinInput,
                                                       string emailInput,
                                                       string passInput,
                                                       string passConfirm,
                                                       string passHash)
        {
            string firstName, lastName, country, city, phoneNumber, tin, passportId, email;
            string passwordHash;
            ulong temp1 = 0;
            ulong temp2 = 0;
            ulong temp3 = 0;

            if (firstNameInput.Length == 0 ||
                lastNameInput.Length == 0 ||
                phoneInput.Length == 0 ||
                tinInput.Length == 0 ||
                idInput.Length == 0 ||
                countryInput.Length == 0 ||
                cityInput.Length == 0)
            {
                //errorText = "Some required fields are empty.";
                return (false, "Some required fields are empty.");
            }
            else
            {
                firstName = (firstNameInput.Substring(0, 1).ToUpper() + (firstNameInput.Length > 1 ? firstNameInput.Substring(1) : "")).Trim();
                lastName = (lastNameInput.Substring(0, 1).ToUpper() + (lastNameInput.Length > 1 ? lastNameInput.Substring(1) : "")).Trim();
            }
            if (passInput.GetHashCode() != passConfirm.GetHashCode())
            {
                //errorText.Text = "Passwords dont match!";
                return (false, "Passwords dont match!");
            }
            else if (passConfirm.Length < 8 || passInput.Length < 8)
            {
                //errorText.Text = "Passwords should be more than 8 symbols long!";
                return (false, "Passwords should be more than 8 symbols long!");
            }
            if (ulong.TryParse(phoneInput.Trim(), out temp3))
            {
                phoneNumber = phoneInput;
            }
            else
            {
                //errorText.Text = "Incorrect phone number!";
                return (false, "Incorrect phone number!");
            }
            if (!emailInput.Contains("@") || !emailInput.Contains("."))
            {
                //errorText.Text = "Incorrect email format.";
                return (false, "Incorrect email format.");
            }
            else
            {
                email = emailInput;
            }
            if (ulong.TryParse(tinInput, out temp1) && ulong.TryParse(idInput, out temp2))
            {
                passportId = idInput;
                tin = tinInput;
            }
            else
            {
                //errorText.Text = "Incorrect tim/passport id.";
                return (false, "Incorrect tim/passport id.");
            }
            country = countryInput.Trim();
            city = cityInput.Trim();
            passwordHash = passConfirm.GetMD5Hash();
            passHash = passwordHash;
            firstNameInput = firstName;
            lastNameInput = lastName;
            phoneInput = phoneNumber;
            countryInput = country;
            cityInput = city;
            idInput = passportId;
            tinInput = tin;
            emailInput = email;



            return await DAL_DOTNETCORE
                          .DataBaseInteractionsNONStatic
                          .instance
                          .AddUser(new User(firstNameInput, lastNameInput,
                                            phoneInput, tinInput,
                                            idInput, countryInput, 
                                            cityInput, passHash, 
                                            emailInput));
        }
        /// <summary>
        /// Checks if data is correct and if user exists
        /// </summary>
        /// <param name="firstNameInput"></param>
        /// <param name="lastNameInput"></param>
        /// <param name="phoneNumberInput"></param>
        /// <param name="passwordInput"></param>
        /// <param name="emailInput"></param>
        /// <returns>1 - if exists, 2 - message from DAL checker method</returns>
        public (bool, string) LoginUser(ref string firstNameInput, ref string lastNameInput, string phoneNumberInput, string passwordInput, string emailInput)
        {
            string firstName, lastName, phoneNumber, email;
            string passwordHash;
            ulong temp3 = 0;

            if (firstNameInput.Length == 0 ||
                lastNameInput.Length == 0 ||
                phoneNumberInput.Length == 0)
            {
                //errorText = "Some required fields are empty.";
                return (false, "Some required fields are empty.");
            }
            else
            {
                firstName = (firstNameInput.Substring(0, 1).ToUpper() + (firstNameInput.Length > 1 ? firstNameInput.Substring(1) : "")).Trim();
                lastName = (lastNameInput.Substring(0, 1).ToUpper() + (lastNameInput.Length > 1 ? lastNameInput.Substring(1) : "")).Trim();
            }
            if (passwordInput.Length < 8 || passwordInput.Length < 8)
            {
                //errorText.Text = "Passwords should be more than 8 symbols long!";
                return (false, "Passwords should be more than 8 symbols long!");
            }
            if (ulong.TryParse(phoneNumberInput.Trim(), out temp3))
            {
                phoneNumber = phoneNumberInput;
            }
            else
            {
                //errorText.Text = "Incorrect phone number!";
                return (false, "Incorrect phone number!");
            }
            if (!emailInput.Contains("@") || !emailInput.Contains("."))
            {
                //errorText.Text = "Incorrect email format.";
                return (false, "Incorrect email format.");
            }
            else
            {
                email = emailInput;
            }
            passwordHash = passwordInput.GetMD5Hash();
            firstNameInput = firstName;
            lastNameInput = lastName;
            phoneNumberInput = phoneNumber;
            emailInput = email;

            return (DAL_DOTNETCORE
                    .DataBaseInteractionsNONStatic
                    .instance
                    .CheckUserExistance(firstNameInput, lastNameInput,
                                        phoneNumberInput, passwordHash,
                                        email));
        }
    }

}
