using FMBusiness.StorageModels;
using FMBusiness.ViewModels.DashboardVModels.FinancesVModels.IncomeVModels;
using FMBusiness.ViewModels.IncomeVModels;
using FMDataModel.DataModels;
using FMDataModel.Enums;
using log4net;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace FMBusiness.Managers
{
    public static class IncomesManager
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static bool SaveIncome(CreateIncomeVModel Income)
        {
            try
            {
                using (var model = new fmDbDataModel())
                {

                    fm_Incomes newIncome = new fm_Incomes()
                    {
                        UserId = UserSingleton.Instance.Id,
                        Name = Income.Name,
                        Type = (int)Enum.Parse(typeof(IncomeType), Income.IncomeType.ToString()),
                        Amount = Income.Amount,
                        //Currency = (int)Enum.Parse(typeof(CurrencyType), Income.Currency.ToString()),
                        InsertTime = DateTime.Now
                    };

                    model.fm_Incomes.Add(newIncome);
                    model.SaveChanges();
                }

                return true;
            }
            catch (Exception e)
            {
                _log.ErrorFormat("There was an error with saving the income. Message: {0}, Stacktrace: {1}", e.Message, e.StackTrace);
                return false;
            }

        }

        public static double SumIncomes()
        {
            using (var model = new fmDbDataModel())
            {
                var sum = model.fm_Incomes
                    .Where(i => i.UserId == UserSingleton.Instance.Id)
                    .Select(i => i.Amount ?? 0)
                    .DefaultIfEmpty()
                    .Sum();

                return sum;
            }
        }

        public static double SumIncomesSavings()
        {
            using (var model = new fmDbDataModel())
            {
                var sum = model.fm_Incomes
                    .Where(i => i.UserId == UserSingleton.Instance.Id && i.Type == (int)IncomeType.Saving)
                    .Select(i => i.Amount ?? 0)
                    .DefaultIfEmpty()
                    .Sum();

                return sum;
            }
        }

        public static List<Income> GetIncomes(string sOrderByColumn, SearchIncomeVModel searchIncomes)
        {
            using (var model = new fmDbDataModel())
            {
                try
                {
                    var props = typeof(Income).GetProperties();
                    List<string> queris = new List<string>();
                    var allQ = "SELECT * FROM fm_Incomes";

                    // SearchFields
                    if (searchIncomes != null)
                    {
                        PropertyInfo[] searchProps = typeof(SearchIncomeVModel).GetProperties();
                        if (searchProps != null)
                        {
                            foreach (var sProp in searchProps)
                            {
                                
                                switch(sProp.Name)
                                {
                                    case "Name":
                                        {
                                            if (!string.IsNullOrWhiteSpace(searchIncomes.Name))
                                            {
                                                var q1 = string.Format("UPPER(Name) like '%{0}%'", searchIncomes.Name);
                                                queris.Add(q1);
                                            }
                                        }
                                        break;

                                    case "Amount":
                                        {
                                            if (searchIncomes.Amount != null)
                                            {
                                                var q1 = string.Format("Amount = {0}", searchIncomes.Amount);
                                                queris.Add(q1);
                                            }
                                        }
                                        break;

                                    case "IncomeType":
                                        {
                                            if (searchIncomes.IncomeType != 0)
                                            {
                                                var q1 = string.Format("Type = {0}", (int)searchIncomes.IncomeType);
                                                queris.Add(q1);
                                            }
                                        }
                                        break;

                                    case "InsertTime":
                                        {
                                            if (searchIncomes.InsertTime != null)
                                            {
                                                var q1 = string.Format("InsertTime >= '{0}'", searchIncomes.InsertTime);
                                                queris.Add(q1);
                                            }
                                        }
                                        break;
                                }
                            }

                            if (queris.Any())
                                allQ = allQ + " WHERE " + string.Join(" AND ", queris);
                        }
                    }


                    var incomesFetched =
                        model.Database.SqlQuery<fm_Incomes>(allQ)
                        .ToList()
                        .Where(income => income.UserId == UserSingleton.Instance.Id)
                        .AsEnumerable()
                        .Select(o => new Income()
                        {
                            Name = o.Name,
                            Amount = o.Amount,
                            InsertTime = o.InsertTime,
                            IncomeType =
                                     ((IncomeType)o.Type).GetType()?
                                    .GetMember(((IncomeType)o.Type).ToString())?
                                    .First()?
                                    .GetCustomAttribute<DisplayAttribute>()?
                                    .Name
                        }).ToList();
                    
                    if (incomesFetched == null)
                    {
                        return new List<Income>();
                    }



                    // SortingReturned
                    string substr = "";
                    
                    if (!string.IsNullOrEmpty(sOrderByColumn))
                    {
                        foreach (var prop in props)
                        {
                            var columnName = prop.Name;
                            if (sOrderByColumn.Length >= 4)
                            {
                                substr = sOrderByColumn.Substring(sOrderByColumn.Length - 4);
                                if (substr == "Desc")
                                {
                                    if ((columnName + "Desc").ToString() == sOrderByColumn)
                                    {
                                        incomesFetched = incomesFetched.OrderByDescending(m => prop.GetValue(m, null)).ToList();
                                        break;
                                    }
                                }
                                else
                                {
                                    if (sOrderByColumn == columnName)
                                    {
                                        incomesFetched = incomesFetched.OrderBy(m => prop.GetValue(m, null)).ToList();
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    return incomesFetched;

                }
                catch (Exception e)
                {
                    _log.ErrorFormat("There was an error with fetching the incomes. Message: {0}, Stacktrace: {1}", e.Message, e.StackTrace);
                    return null;
                }
                
            }
        }
    }
}
