using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseProject
{
    public class SingleBloodBank
    {
		public string bloodbank_id { get; set; }
		public string bloodbank_name { get; set; }
		public string blood_group { get; set; }
		public decimal? blood_amount { get; set; }
		public string blood_status { get; set; }
		public DateTime? checking_date { get; set; }
	}
}
