using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FMBusiness.ViewModels.GraphVModels
{
    [DataContract]
    public class DataPoint
    {
        [DataMember(Name = "x")]
        public double? x = null;

        [DataMember(Name = "y")]
        public double? y = null;


        public DataPoint(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

    }
}
