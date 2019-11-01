using System;
using System.Collections.Generic;
using System.Text;

namespace HostCommunication.HostModels
{
    public class DependentQuery
    {
        public DbDescription DatabaseDescription { get; set; }
        public string Query { get; set; }

        public DependentQuery UpdateQueryParams()
        {
            this.Query = this.Query.Replace("dbName", DatabaseDescription.Name);
            return this;
        }
    }
}
