using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CourseProject
{
    /// <summary>
    /// Логика взаимодействия для MainClientWindow.xaml
    /// </summary>
    public partial class MainClientWindow : Window
    {
        private BindingList<SingleDonor> donors = new BindingList<SingleDonor>();
        private BindingList<SingleDonor> searchDonors = new BindingList<SingleDonor>();
        private BindingList<SingleDonor> leaderDonors = new BindingList<SingleDonor>();
        private BindingList<SingleTaker> takers = new BindingList<SingleTaker>();
        private BindingList<SingleBloodBank> bloodbanks = new BindingList<SingleBloodBank>();
        private BindingList<SingleTransferDetails> transferdetails = new BindingList<SingleTransferDetails>();
        private BindingList<SingleTransferDetails> resultNumOfTransf = new BindingList<SingleTransferDetails>();
        private BindingList<SingleTransferDetails> resultVolume = new BindingList<SingleTransferDetails>();

        public MainClientWindow()
        {
            InitializeComponent();
            LoadDonors();
            LoadTakers();
            LoadBloodBank();
            LoadTransferDetails();
            LoadDataToGrid();
        }

        #region Load info methods

        private void LoadDonors()
        {
            using (Entities ent = new Entities())
            {
                donors.Clear();
                foreach (var item in ent.GETDONORS())
                {
                    donors.Add(new SingleDonor()
                    {
                        donor_id = item.DONOR_ID,
                        donor_name = item.DONOR_NAME,
                        donor_blood_group = item.DONOR_BLOOD_GROUP,
                        donor_phone_number = item.DONOR_PHONE_NUMBER,
                        donor_address = item.DONOR_ADDRESS,
                        last_donation_date = item.LAST_DONATION_DATE,
                        donor_status = item.DONOR_STATUS
                    });
                }
            }
        }

        private void LoadTakers()
        {
            using (Entities ent = new Entities())
            {
                takers.Clear();
                foreach (var item in ent.GETTAKERS())
                {
                    takers.Add(new SingleTaker()
                    {
                        taker_id = item.TAKER_ID,
                        taker_name = item.TAKER_NAME,
                        taker_blood_group = item.TAKER_BLOOD_GROUP,
                        taker_address = item.TAKER_ADDRESS,
                        taker_phone_number = item.TAKER_PHONE_NUMBER
                    });
                }
            }
        }

        private void LoadBloodBank()
        {
            using (Entities ent = new Entities())
            {
                bloodbanks.Clear();
                foreach (var item in ent.GETBLOOD_BANKS())
                {
                    bloodbanks.Add(new SingleBloodBank()
                    {
                        bloodbank_id = item.BLOODBANK_ID,
                        bloodbank_name = item.BLOODBANK_NAME,
                        blood_group = item.BLOOD_GROUP,
                        blood_amount = item.BLOOD_AMOUNT,
                        blood_status = item.BLOOD_STATUS,
                        checking_date = item.CHECKING_DATE
                    });
                }
            }
        }

        private void LoadTransferDetails()
        {
            using (Entities ent = new Entities())
            {
                transferdetails.Clear();
                foreach (var item in ent.GETTRANSFER_DETAILS())
                {
                    transferdetails.Add(new SingleTransferDetails()
                    {
                        transer_id = item.TRANSER_ID,
                        bloodbank_id = item.BLOODBANK_ID,
                        donor_id = item.DONOR_ID,
                        taker_id = item.TAKER_ID,
                        blood_group = item.BLOOD_GROUP,
                        blood_amount = item.BLOOD_AMOUNT,
                        transfer_date = item.TRANSFER_DATE
                    });
                }
            }
        }

        private void LoadDataToGrid()
        {
            DGDonors.ItemsSource = donors;
            DGTakers.ItemsSource = takers;
            DGBloodBank.ItemsSource = bloodbanks;
            DGTransferDetails.ItemsSource = transferdetails;
        }
        #endregion

        #region Window monipulations methods

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Minimise_Button_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }


        private void Close_Button_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Entities ent = new Entities();
            ent.GETDONORS();
        }

        private void Exit_btn_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow signup = new LoginWindow();
            signup.Show();
            Close();
        }

        private void RefreshDBInfoClick(object sender, RoutedEventArgs e)
        {
            LoadDonors();
            LoadTakers();
            LoadBloodBank();
            LoadTransferDetails();
        }
        #endregion

        #region <--------ADD DONOR-------->

        private void Add_donor_btn_Click(object sender, RoutedEventArgs e)
        {
            using (Entities ent = new Entities())
            {
                string newDonorName = donor_name.Text;
                string newDonorBloodGroup = ((TextBlock)donor_blood_group.SelectedItem)?.Text;
                string newDonorPhoneNumber = donor_phone_number.Text;
                string newDonorAddress = donor_address.Text;

                if (string.IsNullOrEmpty(newDonorName))
                {
                    MessageBox.Show("Please enter donor name");
                    return;
                }

                if (string.IsNullOrEmpty(newDonorBloodGroup))
                {
                    MessageBox.Show("Please select donor blood group");
                    return;
                }

                if (string.IsNullOrEmpty(newDonorPhoneNumber))
                {
                    MessageBox.Show("Please enter donor phone number");
                    return;
                }

                if (string.IsNullOrEmpty(newDonorAddress))
                {
                    MessageBox.Show("Please enter donor address");
                    return;
                }

                if (last_donation_date.SelectedDate == null)
                {
                    MessageBox.Show("Please select last donation date");
                    return;
                }

                try
                {
                    DateTime newLastDonation = last_donation_date.SelectedDate.Value;
                    ent.ADD_NEW_DONOR(newDonorName, newDonorBloodGroup, newDonorAddress, newDonorPhoneNumber, newLastDonation);
                    DGDonors.Items.Refresh();
                    MessageBox.Show("New donor successfully added! Refresh database.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error while adding new donor: " + ex.Message);
                }
            }
        }

        private void Reset_donor_info_btn_Click(object sender, RoutedEventArgs e)
        {
            donor_name.Text = string.Empty;
            donor_blood_group.Text = string.Empty;
            donor_phone_number.Text = string.Empty;
            donor_address.Text = string.Empty;
            last_donation_date.Text = string.Empty;
        }

        #endregion

        #region <--------ADD TAKER-------->

        private void Add_taker_btn_Click(object sender, RoutedEventArgs e)
        {
            using (Entities ent = new Entities())
            {
                string newTakerName = taker_name.Text;
                string newTakerBloodGroup = ((TextBlock)taker_blood_group.SelectedItem)?.Text;
                string newTakerPhoneNumber = taker_phone_number.Text;
                string newTakerAddress = taker_address.Text;

                if (string.IsNullOrEmpty(newTakerName))
                {
                    MessageBox.Show("Please enter taker name");
                    return;
                }

                if (string.IsNullOrEmpty(newTakerBloodGroup))
                {
                    MessageBox.Show("Please select taker blood group");
                    return;
                }

                if (string.IsNullOrEmpty(newTakerPhoneNumber))
                {
                    MessageBox.Show("Please enter taker phone number");
                    return;
                }

                if (string.IsNullOrEmpty(newTakerAddress))
                {
                    MessageBox.Show("Please enter taker address");
                    return;
                }

                try
                {
                    ent.ADD_NEW_TAKER(newTakerName, newTakerBloodGroup, newTakerAddress, newTakerPhoneNumber);
                    MessageBox.Show("New taker successfully added! Refresh database.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error while adding new taker: " + ex.Message);
                }
            }
        }

        private void Reset_taker_btn_Click(object sender, RoutedEventArgs e)
        {
            taker_name.Text = string.Empty;
            taker_blood_group.Text = string.Empty;
            taker_phone_number.Text = string.Empty;
            taker_address.Text = string.Empty;
        }

        #endregion

        #region Events for searching donors by parameters 

        private void search_donor_by_group_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (Entities ent = new Entities())
                {
                    var blood_groop = ((TextBlock)cb_donor_by_group.SelectedItem)?.Text;
                    if (string.IsNullOrEmpty(blood_groop))
                    {
                        MessageBox.Show("Please select a blood type.");
                        return;
                    }

                    searchDonors.Clear();
                    foreach (var item in ent.FIND_DONORS_BY_BLOODGROUP(blood_groop))
                    {
                        searchDonors.Add(new SingleDonor()
                        {
                            donor_id = item.DONOR_ID,
                            donor_name = item.DONOR_NAME,
                            donor_phone_number = item.DONOR_PHONE_NUMBER,
                            donor_address = item.DONOR_ADDRESS,
                            last_donation_date = item.LAST_DONATION_DATE,
                            donor_status = item.DONOR_STATUS
                        });
                    }
                    DGGetDonors.ItemsSource = searchDonors;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void search_avdonor_by_group_and_addr_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (Entities ent = new Entities())
                {
                    var blood_groop = ((TextBlock)cb_avdonor_by_group_and_addr.SelectedItem)?.Text;
                    var address = tb_avdonor_by_group_and_addr.Text;

                    if (string.IsNullOrEmpty(blood_groop) || string.IsNullOrEmpty(address))
                    {
                        MessageBox.Show("Please select a blood type and enter an address.");
                        return;
                    }

                    searchDonors.Clear();
                    foreach (var item in ent.FIND_AVAIL_DONORS_BY_GR_ADDR(blood_groop, address))
                    {
                        searchDonors.Add(new SingleDonor()
                        {
                            donor_id = item.DONOR_ID,
                            donor_name = item.DONOR_NAME,
                            donor_phone_number = item.DONOR_PHONE_NUMBER,
                            donor_address = item.DONOR_ADDRESS,
                            donor_status = item.DONOR_STATUS
                        });
                    }
                    DGGetDonors.ItemsSource = searchDonors;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void search_unavdonor_by_group_and_addr_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (Entities ent = new Entities())
                {
                    var blood_groop = ((TextBlock)cb_unavdonor_by_group_and_addr.SelectedItem)?.Text;
                    var address = tb_unavdonor_by_group_and_addr.Text;

                    if (string.IsNullOrEmpty(blood_groop) || string.IsNullOrEmpty(address))
                    {
                        MessageBox.Show("Please select a blood type and enter an address.");
                        return;
                    }

                    searchDonors.Clear();
                    foreach (var item in ent.FIND_UNAVAIL_DONORS_BY_GR_ADDR(blood_groop, address))
                    {
                        searchDonors.Add(new SingleDonor()
                        {
                            donor_id = item.DONOR_ID,
                            donor_name = item.DONOR_NAME,
                            donor_phone_number = item.DONOR_PHONE_NUMBER,
                            donor_address = item.DONOR_ADDRESS,
                            donor_status = item.DONOR_STATUS
                        });
                    }
                    DGGetDonors.ItemsSource = searchDonors;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }

        }

        #endregion

        #region Find Leader

        private void Get_leader_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (Entities ent = new Entities())
                {
                    var start_date = DP_Find_Leader_Start.SelectedDate;
                    var end_date = DP_Find_Leader_End.SelectedDate;

                    if (start_date == null || end_date == null)
                    {
                        MessageBox.Show("Please enter two dates to search for the leader in blood amount.");
                        return;
                    }

                    leaderDonors.Clear();
                    foreach (var item in ent.GET_BLOOD_LEADER(start_date, end_date))
                    {
                        leaderDonors.Add(new SingleDonor()
                        {
                            donor_id = item.DONOR_ID,
                            donor_name = item.DONOR_NAME,
                            donor_phone_number = item.DONOR_PHONE_NUMBER,
                            donor_address = item.DONOR_ADDRESS,
                            total_blood_amount = item.TOTAL_BLOOD_AMOUNT
                        });
                    }

                    if (leaderDonors.Count == 0)
                    {
                        MessageBox.Show("No blood transaction was found in the given transaction range.");
                        return;
                    }

                    var leader = leaderDonors.FirstOrDefault();
                    if (leader != null)
                    {
                        Leader_Id.Text = leader.donor_id.ToString();
                        Leader_Name.Text = leader.donor_name;
                        Leader_phone.Text = leader.donor_phone_number;
                        Leader_address.Text = leader.donor_address;
                        Leader_amount.Text = string.Format("{0:0.##}", leader.total_blood_amount);
                    }

                    Leader_result_row_1.Visibility = Visibility.Visible;
                    Leader_result_row_2.Visibility = Visibility.Visible;
                    Leader_result_row_3.Visibility = Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }


        #endregion

        #region Statistics

        private void Get_Num_Of_Transf_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (Entities ent = new Entities())
                {
                    var selectedDate = DP_Get_Num_Of_Transf.SelectedDate;

                    if (selectedDate == null)
                    {
                        throw new Exception("No date selected.");
                    }

                    resultNumOfTransf.Clear();
                    foreach (var item in ent.NUM_OF_TRANSF(selectedDate))
                    {
                        resultNumOfTransf.Add(new SingleTransferDetails()
                        {
                            resultVolumeOnDate = item.NUMBER_OF_TRANSF
                        });
                    }

                    var transact = resultNumOfTransf.FirstOrDefault();
                    if (transact != null)
                    {
                        Get_Num_Of_Transf_result.Text = string.Format("{0:0.##}", transact.resultVolumeOnDate);
                    }

                    Get_Num_Of_Transf_result_lable.Visibility = Visibility.Visible;
                    Get_Num_Of_Transf_result.Visibility = Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Get_Amount_Of_BloodTransf_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (Entities ent = new Entities())
                {
                    var selectedDate = DP_Get_Amount_Of_BloodTransf.SelectedDate;

                    if (selectedDate == null)
                    {
                        MessageBox.Show("No date selected.");
                    }

                    resultVolume.Clear();
                    foreach (var item in ent.VOLUME_OF_BLOOD_TRANSF_ON_DATE(selectedDate))
                    {
                        resultVolume.Add(new SingleTransferDetails()
                        {
                            resultVolumeOnDate = item.TOTAL_VOLUME_ON_DATE
                        });
                    }

                    if (resultVolume.Count == 0)
                    {
                        MessageBox.Show("No blood transactions were found on the given date.");
                        return;
                    }

                    var transact = resultVolume.FirstOrDefault();
                    if (transact != null)
                    {
                        Get_Amount_Of_BloodTransf_result.Text = string.Format("{0:0.##}", transact.resultVolumeOnDate);
                    }

                    Get_Amount_Of_BloodTransf_result_lable.Visibility = Visibility.Visible;
                    Get_Amount_Of_BloodTransf_result.Visibility = Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion

    }
}
