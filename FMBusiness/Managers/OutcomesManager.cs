using FMBusiness.StorageModels;
using FMBusiness.ViewModels.DashboardVModels.FinancesVModels.OutcomeVModels;
using FMBusiness.ViewModels.IncomeVModels;
using FMBusiness.ViewModels.OutcomeVModels;
using FMDataModel.DataModels;
using FMDataModel.Enums;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FMBusiness.Managers
{
    public static class OutcomesManager
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static bool SaveOutcome(CreateOutcomeVModel Outcome)
        {
            try
            {
                using (var model = new fmDbDataModel())
                {
                    fm_Outcomes newOutcome = new fm_Outcomes()
                    {
                        UserId = UserSingleton.Instance.Id,
                        Name = Outcome.Name,
                        Type = (int)Enum.Parse(typeof(OutcomeType), Outcome.OutcomeType.ToString()),
                        Amount = Outcome.Amount,
                        //Currency = (int)Enum.Parse(typeof(CurrencyType), Outcome.Currency.ToString()),
                        InsertTime = DateTime.Now
                    };

                    model.fm_Outcomes.Add(newOutcome);
                    model.SaveChanges();
                }

                return true;
            }
            catch (Exception e)
            {
                _log.ErrorFormat("There was an error with saving the outcome. Message: {0}, Stacktrace: {1}", e.Message, e.StackTrace);
                return false;
            }

        }

        public static double SumOutcomes()
        {
            using (var model = new fmDbDataModel())
            {
                var sum = model.fm_Outcomes
                    .Where(o => o.UserId == UserSingleton.Instance.Id)
                    .Select(o => o.Amount ?? 0)
                    .DefaultIfEmpty()
                    .Sum();

                return sum;
            }
        }

        public static List<Outcome> GetOutcomes(string sOrderByColumn, SearchOutcomeVModel searchOutcomes)
        {
            using (var model = new fmDbDataModel())
            {
                try
                {
                    var props = typeof(Outcome).GetProperties();
                    List<string> queris = new List<string>();
                    var allQ = "SELECT * FROM fm_Outcomes";

                    // SearchFields
                    if (searchOutcomes != null)
                    {
                        PropertyInfo[] searchProps = typeof(SearchOutcomeVModel).GetProperties();
                        if (searchProps != null)
                        {
                            foreach (var sProp in searchProps)
                            {

                                switch (sProp.Name)
                                {
                                    case "Name":
                                        {
                                            if (!string.IsNullOrWhiteSpace(searchOutcomes.Name))
                                            {
                                                var q1 = string.Format("UPPER(Name) like '%{0}%'", searchOutcomes.Name);
                                                queris.Add(q1);
                                            }
                                        }
                                        break;

                                    case "Amount":
                                        {
                                            if (searchOutcomes.Amount != 0)
                                            {
                                                var q1 = string.Format("Amount = {0}", searchOutcomes.Amount);
                                                queris.Add(q1);
                                            }
                                        }
                                        break;

                                    case "Type":
                                        {
                                            if (searchOutcomes.OutcomeType != 0)
                                            {
                                                var q1 = string.Format("Type = {0}", searchOutcomes.OutcomeType);
                                                queris.Add(q1);
                                            }
                                        }
                                        break;

                                    case "InsertTime":
                                        {
                                            if (searchOutcomes.InsertTime != null)
                                            {
                                                var q1 = string.Format("InsertTime >= '{0}'", searchOutcomes.InsertTime);
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


                    var outcomesFetched =
                        model.Database.SqlQuery<fm_Outcomes>(allQ)
                        .ToList()
                        .Where(outcome => outcome.UserId == UserSingleton.Instance.Id)
                        .AsEnumerable()
                        .Select(o => new Outcome()
                        {
                            Name = o.Name,
                            Amount = o.Amount,
                            InsertTime = o.InsertTime,
                            OutcomeType = Enum.GetName(typeof(OutcomeType), o.Type).ToString()
                        }).ToList();
                    
                    if (outcomesFetched == null)
                    {
                        return new List<Outcome>();
                    }
                    
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
                                        outcomesFetched = outcomesFetched.OrderByDescending(m => prop.GetValue(m, null)).ToList();
                                        break;
                                    }
                                }
                                else
                                {
                                    if (sOrderByColumn == columnName)
                                    {
                                        outcomesFetched = outcomesFetched.OrderBy(m => prop.GetValue(m, null)).ToList();
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    return outcomesFetched;

                }
                catch (Exception e)
                {
                    _log.ErrorFormat("There was an error with fetching the outcomes. Message: {0}, Stacktrace: {1}", e.Message, e.StackTrace);
                    return null;
                }

            }
        }

    }
}
