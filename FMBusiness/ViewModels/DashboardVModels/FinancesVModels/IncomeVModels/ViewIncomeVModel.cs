using FMBusiness.Managers;
using FMBusiness.ViewModels.IncomeVModels;
using PagedList;

namespace FMBusiness.ViewModels.DashboardVModels.FinancesVModels.IncomeVModels
{
    public class ViewIncomeVModel
    {
        private long Id { get; set; }
        
        private long UserId { get; set; }

        public string UserName { get; set; }

        public IPagedList<Income> IncomesList { get; set; }

        public SearchIncomeVModel SearchIncomes { get; set; }

        public int? Page { get; set; }
        public int PageSize { get; set; }
        public string SOrderByColumn { get; set; }
    }
}

