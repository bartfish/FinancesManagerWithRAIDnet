using FMDataModel.Enums;

namespace FMBusiness.ViewModels.DashboardVModels.UserVModels
{
    public partial class UserVModel
    {
        public long Id { get; set; }
        
        public string Name { get; set; }
        
        public string Email { get; set; }

        public CurrencyType DefaultCurrency { get; set; }
    }
}
