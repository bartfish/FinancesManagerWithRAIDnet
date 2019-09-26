using FMBusiness.ViewModels.AccountVModels;
using System;
using System.Linq;
using FMBusiness.StorageModels;
using System.Web;
using FMDataModel.Enums;
using FMDataModel.DataModels;
using log4net;
using System.Reflection;

namespace FMBusiness.Managers
{
    public static class UserManager
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static bool InitializeUserLogin(fm_Users User)
        {
            try
            {
                using (var model = new fmDbDataModel())
                {
                    UserSingleton.CreateUserSingleton(User);
                    
                    HttpContext.Current.Session["USER_ID"] = User.Id;
                    HttpContext.Current.Session["USER_EMAIL"] = UserSingleton.Instance.Email;
                    
                    User.LastSuccessfullLogin = DateTime.Now;
                    User.IsOnline = (int) UserCurrentStatus.Online;
                    model.Entry<fm_Users>(User).State = System.Data.Entity.EntityState.Modified;
                    model.SaveChanges();
                }

                return true;
            }
            catch (Exception e)
            {
                _log.ErrorFormat("There was an error with initializing user login. Message: {0}, Stacktrace: {1}", e.Message, e.StackTrace);
                return false;
            }
        }

        public static bool ResetUserOnLogout()
        {
            try
            {
                using (var model = new fmDbDataModel())
                {
                    int userId = Convert.ToInt32(HttpContext.Current.Session["USER_ID"]);

                    var userToLogout = model.fm_Users.FirstOrDefault(
                        user => user.Id == UserSingleton.Instance.Id
                        &&
                        user.Email == UserSingleton.Instance.Email);
                    
                    userToLogout.LastSuccessfullLogin = DateTime.Now;
                    userToLogout.IsOnline = (int)UserCurrentStatus.Offine;
                    model.Entry<fm_Users>(userToLogout).State = System.Data.Entity.EntityState.Modified;
                    model.SaveChanges();
                    
                    UserSingleton.Reset();
                    
                    HttpContext.Current.Session["USER_ID"] = null;
                    HttpContext.Current.Session["USER_EMAIL"] = null;
                }

                return true;
            }
            catch (Exception e)
            {
                _log.ErrorFormat("There was an error with resetting user data while loggin out. Message: {0}, Stacktrace: {1}", e.Message, e.StackTrace);
                return false;
            }
        }

        public static bool IsEmailAddressAvailable(string email)
        {
            using (var model = new fmDbDataModel())
            {
                var data = model.fm_Users.FirstOrDefault(user => user.Email.Equals(email));
                return data == null ? false : true;
            }
        }
        
        public static bool InsertUser(RegisterVModel userRegister)
        {
            using (var model = new fmDbDataModel())
            {
                try
                {
                    string hashedPassword = SecurityManager.CalculateHash(userRegister.Password);

                    fm_Users newUser = new fm_Users()
                    {
                        Name = userRegister.Name,
                        Email = userRegister.Email,
                        Password = hashedPassword,
                        Salt = SecurityManager.GetSalt(), // later will be taken from database and fetched with new password to compare with the old one
                        
                        InsertTime = DateTime.Now,
                        AccountStatus = (int)AccountStatus.Active
                    };

                    model.fm_Users.Add(newUser);
                    model.SaveChanges();

                    return true;
                }
                catch (Exception e)
                {
                    _log.ErrorFormat("There was an error with inserting user to db. Message: {0}, Stacktrace: {1}", e.Message, e.StackTrace);
                    return false;
                }
            }
        }

        public static void SwitchCurrency(CurrencyType? currency)
        {
            if (currency == null) return;

            using (var model = new fmDbDataModel())
            {
                var user = model.fm_Users.FirstOrDefault(u => u.Id == UserSingleton.Instance.Id);
                if (user != null)
                {
                    UserSingleton.Instance.DefaultCurrency = (int)currency;
                    user.DefaultCurrency = (int)currency;
                    model.SaveChanges();
                }
            }
        }
    }
}
