using FMBusiness.StorageModels;
using FMBusiness.ViewModels.AccountVModels;
using FMDataModel.DataModels;
using FMDataModel.Enums;
using HostCommunication.Managers;
using log4net;
using System;
using System.Linq;
using System.Reflection;

namespace FMBusiness.Managers
{
    public static class AccountManager 
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static LoginVModel UserSignIn(LoginVModel UserLogin)
        {
            fm_Users userExists = null;
            try
            {
                using (var model = new fmDbDataModel())
                {
                    userExists =
                        model.fm_Users.FirstOrDefault(user => user.Email.Equals(UserLogin.Email));

                    if (userExists == null)
                    {
                        return (LoginVModel)DataOperationManager.VerifyResult(new Func<LoginVModel, LoginVModel>(UserSignIn), new object[] { UserLogin }, HostCommunication.HostModels.MethodReturnStatus.Null);
                    }

                    string hashedPassword = SecurityManager.CalculateHash(UserLogin.Password, userExists.Salt);

                    if (SecurityManager.ComparePasswords(userExists.Password, hashedPassword))
                    {
                        bool isInitialized = UserManager.InitializeUserLogin(userExists);
                        if (isInitialized)
                        {
                            return new LoginVModel()
                            {
                                Email = UserLogin.Email,
                                Password = hashedPassword
                            };
                        }
                    }
                    _log.InfoFormat("User with {0} email could not login.", UserLogin.Email);
                    return null;
                }
            }
            catch (Exception e)
            {
                return (LoginVModel)DataOperationManager.VerifyResult(new Func<LoginVModel, LoginVModel>(UserSignIn), new object[] { UserLogin }, HostCommunication.HostModels.MethodReturnStatus.Error);
            }
            
        }

    }
}
