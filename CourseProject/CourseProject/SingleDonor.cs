using System;

namespace CourseProject
{
    public class SingleDonor
    {
		public decimal donor_id { get; set; }
		public string donor_name { get; set; }
		public string donor_blood_group { get; set; }
		public string donor_phone_number { get; set; }
		public string donor_address { get; set; }
		public DateTime last_donation_date { get; set; }
		public string donor_status { get; set; }
		public decimal? total_blood_amount { get; set; }
        public SingleDonor SelectedItem { get; internal set; }
    }
}
