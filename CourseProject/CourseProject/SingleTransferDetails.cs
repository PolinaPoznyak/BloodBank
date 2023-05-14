using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseProject
{
    public class SingleTransferDetails
    {
		public decimal transer_id { get; set; }
		public string bloodbank_id { get; set; }
		public decimal? donor_id { get; set; }
		public decimal? taker_id { get; set; }
		public string blood_group { get; set; }
		public decimal? blood_amount { get; set; }
		public DateTime? transfer_date { get; set; }
		public decimal? resultVolumeOnDate { get; set; }
		public decimal? numberOfTransfOnDate { get; set; }
	}
}
