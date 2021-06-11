using System;
using System.Collections.Generic;
using System.Text;
using DAL_DOTNETCORE.Models;
using System.Reflection;
using System.Linq;
using System.Threading.Tasks;

namespace DAL_DOTNETCORE
{
    public static class DataBaseInteractionsNONStatic
    {
        public static DataBaseInteractions instance = new DataBaseInteractions();
    }
    public class DataBaseInteractions
    {
        int extraInstancesCounter = 0;
        public DataBaseInteractions()
        {
            if (extraInstancesCounter > 0) throw new MoreThanOneInstanceDALException("Only one instance allowed!");
            extraInstancesCounter++;
        }
        public (bool, string) CheckUserExistance(string fName, string lName, string pNumber, string passwordHash, string email)
        {
            using (var context = new PaymentsManagerDBContext())
            {
                var foundUser = context.Users.Where(x => x.firstName == fName &&
                                                         x.lastName == lName &&
                                                         x.phoneNumber == pNumber &&
                                                         x.passwordHash == passwordHash &&
                                                         x.email == email)
                                             .AsParallel()
                                             .FirstOrDefault();
                if (foundUser != null)
                {
                    return (true, "User exists!");
                }
                else return (false, "User doesn't exist!");
            }
        }
        /// <summary>
        /// Method to get tin and passport id by email only
        /// </summary>
        /// <param name="email"></param>
        /// <returns>1 - TIN, 2 - PASSPORT ID</returns>
        public static (string, string) GetTINAndPasspIDByEmail(string email)
        {
            using (var context = new PaymentsManagerDBContext())
            {
                var foundUser = context.Users.Where(x => x.email == email)
                                             .AsParallel()
                                             .FirstOrDefault();
                return (foundUser.tin, foundUser.passportId);
            }
        }
        public static User GetUser(string tin, string passpId)
        {
            User gotUser = null;
            using (var context = new PaymentsManagerDBContext())
            {
                gotUser = context.Users.Where(x => x.passportId == passpId && 
                                                   x.tin == tin)
                                       .AsParallel()
                                       .FirstOrDefault();
                return gotUser;
            }

        }
        public static Wallet GetWallet(string walletNumber, string walletName, string ownerPasspId, string ownerTin)
        {
            Wallet gotWallet = null;
            using (var context = new PaymentsManagerDBContext())
            {
                gotWallet = context.Wallets
                    .Where(x => x.walletNumber == walletNumber && x.walletName == walletName &&
                        x.walletOwnerPassId == ownerPasspId && x.walletOwnerTIN == ownerTin)
                    .AsParallel()
                    .FirstOrDefault();
                return gotWallet;
            }
        }
        public static IEnumerable<User> GetUsers()
        {
            using (var context = new PaymentsManagerDBContext())
            {
                return context.Users.ToList().AsParallel();
            }
        }
        public static IEnumerable<Wallet> GetWallets(string ownerPassId, string ownerTin)
        {
            using (var context = new PaymentsManagerDBContext())
            {
                return context.Wallets
                    .Where(x => x.walletOwnerPassId == ownerPassId && x.walletOwnerTIN == ownerTin)
                    .AsParallel()
                    .ToList();
            }
        }
        public async Task<(bool, string)> AddUser(User user)
        {
            using (var context = new PaymentsManagerDBContext())
            {
                try
                {
                    await context.Users.AddAsync(user);
                    await context.SaveChangesAsync();
                }
                catch (Exception)
                {
                    return (false, "User with this email or phone already exists!");
                }
            }
            return (true, "User added successfully!");
        }
        public async Task<string> AddWallet(Wallet wallet)
        {
            using (var context = new PaymentsManagerDBContext())
            {
                try
                {
                    await context.Wallets.AddAsync(wallet);
                    await context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
            return "Wallet added successfully!";
        }
        public async Task<string> AddUsers(IEnumerable<User> users)
        {
            using (var context = new PaymentsManagerDBContext())
            {
                try
                {
                    await context.Users.AddRangeAsync(users);
                    await context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
                return "Range of users has been added successfully!";
            }
        }
        public async void ChangeCategories(string oldCategory, string newCategory, string userWalletNumber = null)
        {
            using(var context = new PaymentsManagerDBContext())
            {
                var transactions = context.Transactions;
                Transaction transaction = null;
                if(userWalletNumber != null)
                {
                    transaction = transactions.Where(x => x.Category == oldCategory &&
                                                      x.SenderWalletNumber == userWalletNumber)
                                          .AsParallel()
                                          .FirstOrDefault();
                }
                else
                {
                    transaction = transactions.Where(x => x.Category == oldCategory)
                                          .AsParallel()
                                          .FirstOrDefault();
                }
                if (transaction != null)
                    transaction.Category = newCategory;
                else
                    return;
                await context.SaveChangesAsync();
            }
            if (userWalletNumber == null)
                ChangeCategories(oldCategory, newCategory);
            else
                ChangeCategories(oldCategory, newCategory, userWalletNumber);
        }
        public static IEnumerable<string> GetHistoryStrings(string senderWalletNumber, string category = null)
        {
            using (var context = new PaymentsManagerDBContext())
            {
                List<string> transactions = new List<string>();
                if (senderWalletNumber == "All")
                {
                    if(category == null)
                    {
                        foreach (var transaction in context.Transactions)
                        {
                            transactions.Add(transaction.ToString());
                        }
                    }
                    else
                    {
                        foreach (var transaction in context.Transactions)
                        {
                            if(transaction.Category == category)
                                transactions.Add(transaction.ToString());
                        }
                    }
                    return transactions;
                }
                try
                {
                    if(category == null)
                    {
                        foreach (var transaction in context.Transactions)
                        {
                            if (transaction.SenderWalletNumber == senderWalletNumber)
                            {
                                transactions.Add(transaction.ToString());
                            }
                        }
                    }
                    else
                    {
                        foreach (var transaction in context.Transactions)
                        {
                            if (transaction.SenderWalletNumber == senderWalletNumber &&
                                transaction.Category == category)
                            {
                                transactions.Add(transaction.ToString());
                            }
                        }
                    }
                }
                catch(Exception ex)
                {
                    transactions.Add(ex.Message);
                }
                return transactions;
            }
        }
        public async Task<string> AddWallets(IEnumerable<Wallet> wallets)
        {
            using (var context = new PaymentsManagerDBContext())
            {
                try
                {
                    await context.Wallets.AddRangeAsync(wallets);
                    await context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
                return "Range of wallets has been added successfully!";
            }
        }
        public async Task<string> RemoveUser(string passpId, string tin)
        {
            using (var context = new PaymentsManagerDBContext())
            {
                try
                {
                    context.Users.Remove(context.Users.Where(x => x.passportId == passpId && x.tin == tin)
                                                      .AsParallel()
                                                      .FirstOrDefault());
                    await context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
            return "User has been removed successfully!";
        }
        public async Task<string> RemoveWallet(string walletNumber, string walletName)
        {
            using (var context = new PaymentsManagerDBContext())
            {
                try
                {
                    context.Wallets.Remove(context.Wallets.Where(x => x.walletName == walletName && 
                                                                      x.walletNumber == walletNumber)
                                                          .AsParallel()
                                                          .FirstOrDefault());
                    await context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
            return "Wallet has been removed successfully!";
        }
        public async Task<string> RemoveUsers(IEnumerable<User> users)
        {
            using (var context = new PaymentsManagerDBContext())
            {
                try
                {
                    context.Users.RemoveRange(users);
                    await context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
            return "Range of users has been removed successfully!";
        }
        public async Task<string> RemoveWallets(IEnumerable<Wallet> wallets)
        {
            using (var context = new PaymentsManagerDBContext())
            {
                try
                {
                    context.Wallets.RemoveRange(wallets);
                    await context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
            return "Range of wallets has been removed successfully!";
        }
        public async Task<(bool, string)> ChangeUserData(string newUserLastName, string newUserFirstName,
                                                         string newUserCity, string newUserCountry,
                                                         string newUserPhoneNumber, string newUserTin,
                                                         string newUserPassportId, string newUserEmail,
                                                         string oldUserPasspId, string oldUserTin, 
                                                         string passwordHash)
        {
            using (var context = new PaymentsManagerDBContext())
            {
                var oldUser = context.Users
                        .Where(x => x.passportId == oldUserPasspId && x.tin == oldUserTin)
                        .AsParallel()
                        .FirstOrDefault();
                ChangeWalletOwnerData(newUserPassportId, newUserTin, 
                                      oldUserPasspId, oldUserTin);
                oldUser.lastName = newUserLastName;
                oldUser.firstName = newUserFirstName;
                oldUser.city = newUserCity;
                oldUser.country = newUserCountry;
                oldUser.phoneNumber = newUserPhoneNumber;
                oldUser.tin = newUserTin;
                oldUser.passportId = newUserPassportId;
                oldUser.email = newUserEmail;
                oldUser.passwordHash = passwordHash;

                await context.SaveChangesAsync();
                return (true, "User data successfully changed!");
            }
        }
        private async void ChangeWalletOwnerData(string newOwnerPassId, string newOwnerTin,
                                                  string oldOwnerPassId, string oldOwnerTin)
        {
            using (var context = new PaymentsManagerDBContext())
            {
                var oldWallets = context.Wallets
                        .Where(x => x.walletOwnerPassId == oldOwnerPassId && 
                                    x.walletOwnerTIN == oldOwnerTin)
                        .AsParallel()
                        .ToList();
                for (int i = 0; i < oldWallets.Count; i++)
                {
                    oldWallets[i].walletOwnerPassId = newOwnerPassId;
                    oldWallets[i].walletOwnerTIN = newOwnerTin;
                }
                await context.SaveChangesAsync();
            }
        }
        public async Task<(bool, string)> AddRemoveMoney(decimal sum, string walletNumber)
        {
            using (var context = new PaymentsManagerDBContext())
            {
                Wallet wallet = context.Wallets.Where(x => x.walletNumber == walletNumber)
                                               .AsParallel()
                                               .FirstOrDefault();
                wallet.walletBalance += sum;
                await context.SaveChangesAsync();
                return (true, "Transaction completed!");
            }
        }
        public async Task<(bool, string)> SaveTransactionInfo(string senderWalletNumber, 
                                                              string receiverWalletNumber, 
                                                              string category, decimal sum)
        {
            using(var context = new PaymentsManagerDBContext())
            {
                await context.Transactions.AddAsync(new Transaction(DateTime.Now.ToString(), senderWalletNumber, 
                                                                    receiverWalletNumber, category, sum));
                await context.SaveChangesAsync();
                return (true, "Successfull added transaction!");
            }
        }
        public async Task<(bool, string)> PerformTransaction(decimal sum, string receiverWalletNumber, 
                                                             string CVV = null, string dateMonth = null, 
                                                             string dateYear = null, string senderWalletNumber = null)
        {
            using (var context = new PaymentsManagerDBContext())
            {
                if(senderWalletNumber == null)
                {
                    var wallet = context.Wallets.Where(x => x.walletNumber == receiverWalletNumber &&
                                                            x.walletCVV == CVV)
                                                .AsParallel()
                                                .FirstOrDefault();
                    try
                    {
                        wallet.walletBalance += sum;
                    }
                    catch(Exception e)
                    {
                        return (false, e.Message);
                    }
                    await context.SaveChangesAsync();
                    return (true, "Transaction successfull!");
                }
                else
                {
                    if(sum <= 0)
                    {
                        return (false, "Incorrect amount of money!");
                    }
                    var receiverWallet = context.Wallets.Where(x => x.walletNumber == receiverWalletNumber)
                                                        .AsParallel()
                                                        .FirstOrDefault();
                    var senderWallet = context.Wallets.Where(x => x.walletNumber == senderWalletNumber &&
                                                                  x.walletCVV == CVV)
                                                      .AsParallel()
                                                      .FirstOrDefault();
                    if(senderWallet.walletBalance < sum)
                    {
                        return (false, "Not enough money!");
                    }
                    else
                    {
                        senderWallet.walletBalance -= sum;
                        receiverWallet.walletBalance += sum;
                        await context.SaveChangesAsync();
                        return (true, "Transaction successfull!");
                    }
                }
            }
        }
        public async void ChangeWalletName(string newWalletName, string oldWalletName, string oldWalletNumber)
        {
            using (var context = new PaymentsManagerDBContext())
            {
                var oldWallet = context.Wallets.Where(x => x.walletName == oldWalletName && 
                                                           x.walletNumber == oldWalletNumber)
                                               .AsParallel()
                                               .FirstOrDefault();
                if (newWalletName.Length < 4)
                {
                    newWalletName = "TempWalletName" + new Random().Next(0, 999);
                }
                oldWallet.walletName = newWalletName;
                await context.SaveChangesAsync();
            }
        }
    }
}