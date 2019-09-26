using FMBusiness.Managers;
using FMBusiness.StorageModels;
using FMBusiness.ViewModels.DashboardVModels.UserVModels;
using FMDataModel.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace FMBusiness.ViewModels.DashboardVModels
{
    public partial class DashboardVModel
    {
        public UserVModel User { get; set; }
        
        public double CurrentBudget { get; set; }

        public double IncomesSum { get; set; }

        public double IncomesSumSavings { get; set; }

        public double OutcomesSum { get; set; }

        public string CurrencyLabel { get; set; }

        public DashboardVModel()
        {
            User = new UserVModel()
            {
                Email = UserSingleton.Instance.Email,
                Id = UserSingleton.Instance.Id,
                Name = UserSingleton.Instance.Name,
                DefaultCurrency = (CurrencyType) Enum.ToObject(typeof(CurrencyType), UserSingleton.Instance.DefaultCurrency),
            };

            CurrentBudget = 0;

            OutcomesSum = OutcomesManager.SumOutcomes();
            IncomesSum = IncomesManager.SumIncomes();
            IncomesSumSavings = IncomesManager.SumIncomesSavings();
            CurrentBudget = (IncomesSum - OutcomesSum > 0 ? IncomesSum - OutcomesSum : 0);
            CurrencyLabel = User.DefaultCurrency.GetType()
                        .GetMember(User.DefaultCurrency.ToString())
                        .First()
                        .GetCustomAttribute<DisplayAttribute>()
                        .GetName();
        }

    }
}
