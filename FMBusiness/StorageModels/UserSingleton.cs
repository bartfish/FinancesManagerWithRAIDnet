using FMDataModel.DataModels;
using FMDataModel.Enums;
using System;

namespace FMBusiness.StorageModels
{
    public class UserSingleton : fm_Users
    {
        private static Lazy<UserSingleton> _instance = null;
        public static UserSingleton Instance => _instance.Value;

        private UserSingleton(fm_Users fetched)
        {
            Id = fetched.Id;
            Name = fetched.Name;
            Email = fetched.Email;
            DefaultCurrency = fetched.DefaultCurrency ?? (int)CurrencyType.PLN;
        }

        public static void CreateUserSingleton(fm_Users fetched)
        {
            if (_instance == null)
            {
                _instance = new Lazy<UserSingleton>(() => new UserSingleton(fetched));
            }
        }

        public static void Reset()
        {
            _instance = null;
        }
        
    }
}
