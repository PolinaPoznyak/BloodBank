using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CourseProject
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SingleDonor _donor;
        SingleTaker _taker;
        SingleBloodBank _bloodbank;
        SingleTransferDetails _transfDet;

        private BindingList<SingleDonor> donors = new BindingList<SingleDonor>();
        private BindingList<SingleDonor> searchDonors = new BindingList<SingleDonor>();
        private BindingList<SingleDonor> leaderDonors = new BindingList<SingleDonor>();
        private BindingList<SingleTaker> takers = new BindingList<SingleTaker>();
        private BindingList<SingleBloodBank> bloodbanks = new BindingList<SingleBloodBank>();
        private BindingList<SingleTransferDetails> transferdetails = new BindingList<SingleTransferDetails>();
        private BindingList<SingleTransferDetails> resultNumOfTransf = new BindingList<SingleTransferDetails>();
        private BindingList<SingleTransferDetails> resultVolume = new BindingList<SingleTransferDetails>();

        public MainWindow()
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

        #region <--------DONOR CRUD-------->

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

        private void update_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_donor == null)
                {
                    MessageBox.Show("Select an object to update!");
                }
                else
                {
                    using (Entities ent = new Entities())
                    {
                        ent.UPDATE_DONOR_INFO(_donor.donor_id, donor_name.Text, donor_address.Text, donor_phone_number.Text, Convert.ToDateTime(last_donation_date.Text));
                        MessageBox.Show("Donor update successfully! Refresh database.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating donor: " + ex.Message);
            }
        }

        private void delete_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_donor == null)
                {
                    MessageBox.Show("Select an object to delete!");
                }
                else
                {
                    using (Entities ent = new Entities())
                    {
                        ent.DELETE_DONOR(_donor.donor_id);
                        MessageBox.Show("Donor deleted successfully! Refresh database.");
                        
                        //DGDonors.Items.Refresh();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting donor: " + ex.Message);
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

        #region <--------TAKER CRUD-------->

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

        private void Update_taker_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_taker == null)
                {
                    MessageBox.Show("Select an object to update!");
                }
                else
                {
                    using (Entities ent = new Entities())
                    {
                        ent.UPDATE_TAKER_INFO(_taker.taker_id, taker_name.Text, taker_address.Text, taker_phone_number.Text);
                        MessageBox.Show("Taker update successfully! Refresh database.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating taker: " + ex.Message);
            }
        }

        private void Delete_taker_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_taker == null)
                {
                    MessageBox.Show("Select an object to delete!");
                }
                else
                {
                    using (Entities ent = new Entities())
                    {
                        ent.DELETE_TAKER(_taker.taker_id);
                        MessageBox.Show("Taker deleted successfully! Refresh database.");

                        //DGDonors.Items.Refresh();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting taker: " + ex.Message);
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

        #region <--------BLOOD BANK CRUD-------->

        private void Add_BloodBank_btn_Click(object sender, RoutedEventArgs e)
        {
            using (Entities ent = new Entities())
            {
                string newBBId = BloodBank_id.Text;
                string newBBName = BloodBank_name.Text;
                string newBBBloodGroup = ((TextBlock)BloodBank_blood_group.SelectedItem)?.Text;
                string newBBAmount = BloodBank_amount.Text;
                string newBBCheckDate = BloodBank_checking_date.Text;

                if (string.IsNullOrEmpty(newBBId))
                {
                    MessageBox.Show("Please enter blood bank ID");
                    return;
                }

                if (string.IsNullOrEmpty(newBBName))
                {
                    MessageBox.Show("Please enter blood bank name");
                    return;
                }

                if (string.IsNullOrEmpty(newBBBloodGroup))
                {
                    MessageBox.Show("Please select blood group for Blood Bank");
                    return;
                }

                if (string.IsNullOrEmpty(newBBAmount))
                {
                    MessageBox.Show("Please enter amount of blood");
                    return;
                }

                if (string.IsNullOrEmpty(newBBCheckDate))
                {
                    MessageBox.Show("Please enter checking date");
                    return;
                }

                try
                {
                    ent.ADD_NEW_BLOOD_BANK(newBBId, newBBName, newBBBloodGroup, Convert.ToDecimal(newBBAmount), Convert.ToDateTime(newBBCheckDate));
                    MessageBox.Show("New blood bank successfully added! Refresh database.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error while adding new blood bank: " + ex.Message);
                }
            }
        }

        private void Update_BloodBank_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_bloodbank == null)
                {
                    MessageBox.Show("Select an object to update!");
                }
                else
                {
                    using (Entities ent = new Entities())
                    {
                        ent.UPDATE_BLOOD_BANK(_bloodbank.bloodbank_id, BloodBank_blood_group.Text, Convert.ToDecimal(BloodBank_amount.Text));
                        MessageBox.Show("Blood Bank update successfully! Refresh database.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating Blood Bank: " + ex.Message);
            }
        }

        private void Delete_BloodBank_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_bloodbank == null)
                {
                    MessageBox.Show("Select an object to delete!");
                }
                else
                {
                    using (Entities ent = new Entities())
                    {
                        ent.DELETE_BLOOD_BANK(_bloodbank.bloodbank_id);
                        MessageBox.Show("Blood Bank deleted successfully! Refresh database.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting Blood Bank: " + ex.Message);
            }
        }

        private void Reset_BloodBank_btn_Click(object sender, RoutedEventArgs e)
        {
            BloodBank_id.Text = string.Empty;
            BloodBank_name.Text = string.Empty;
            BloodBank_blood_group.Text = string.Empty;
            BloodBank_amount.Text = string.Empty;
            BloodBank_checking_date.Text = string.Empty;
        }

        #endregion

        #region <--------TRANSFER DETAILS CRUD-------->


        private void Add_TransferDetails_btn_Click(object sender, RoutedEventArgs e)
        {
            using (Entities ent = new Entities())
            {
                string newTDIDBloodBank = TransferDetails_bb_id.Text;
                string newTDIDDonor = TransferDetails_donor_id.Text;
                string newTDIDTaker = TransferDetails_taker_id.Text;
                string newTDGroup = ((TextBlock)TransferDetails_blood_group.SelectedItem)?.Text;
                string newTDAmount = TransferDetails_amount.Text;
                string newTDTransfDate = TransferDetails_date.Text;

                if (string.IsNullOrEmpty(newTDIDBloodBank))
                {
                    MessageBox.Show("Please enter Blood Bank ID");
                    return;
                }

                if (string.IsNullOrEmpty(newTDIDDonor))
                {
                    MessageBox.Show("Please enter Donor ID");
                    return;
                }

                if (string.IsNullOrEmpty(newTDIDTaker))
                {
                    MessageBox.Show("Please enter Taker ID");
                    return;
                }

                if (string.IsNullOrEmpty(newTDGroup))
                {
                    MessageBox.Show("Please select blood group");
                    return;
                }


                if (string.IsNullOrEmpty(newTDAmount))
                {
                    MessageBox.Show("Please enter amount of blood");
                    return;
                }

                if (string.IsNullOrEmpty(newTDTransfDate))
                {
                    MessageBox.Show("Please select transfer date");
                    return;
                }

                try
                {
                    ent.ADD_NEW_TRANSFER_DETAILS(newTDIDBloodBank, Convert.ToDecimal(newTDIDDonor), Convert.ToDecimal(newTDIDTaker), newTDGroup, Convert.ToDecimal(newTDAmount), Convert.ToDateTime(newTDTransfDate));
                    MessageBox.Show("New transfer detail row successfully added! Refresh database.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error while adding new transfer detail row: " + ex.Message);
                }
            }
        }

        private void Update_TransferDetails_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_bloodbank == null)
                {
                    MessageBox.Show("Select an object to update!");
                }
                else
                {
                    using (Entities ent = new Entities())
                    {
                        ent.UPDATE_TRANSFER_DETAILS(_transfDet.transer_id, TransferDetails_bb_id.Text, Convert.ToDecimal(TransferDetails_donor_id.Text), Convert.ToDecimal(TransferDetails_taker_id.Text), BloodBank_blood_group.Text, Convert.ToDecimal(BloodBank_amount.Text), Convert.ToDateTime(TransferDetails_date.Text));
                        MessageBox.Show("Selected transfer detail row update successfully! Refresh database.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating Blood Bank: " + ex.Message);
            }
        }

        private void Delete_TransferDetails_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_transfDet == null)
                {
                    MessageBox.Show("Select an object to delete!");
                }
                else
                {
                    using (Entities ent = new Entities())
                    {
                        ent.DELETE_TRANSFER_DETAILS(_transfDet.transer_id);
                        MessageBox.Show("Selected transfer detail row deleted successfully! Refresh database.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting selected transfer detail row: " + ex.Message);
            }
        }

        private void Reset_TransferDetails_btn_Click(object sender, RoutedEventArgs e)
        {
            TransferDetails_bb_id.Text = string.Empty;
            TransferDetails_donor_id.Text = string.Empty;
            TransferDetails_taker_id.Text = string.Empty;
            TransferDetails_blood_group.Text = string.Empty;
            TransferDetails_amount.Text = string.Empty;
            TransferDetails_date.Text = string.Empty;
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
                        MessageBox.Show("Please select a blood group.");
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
                        MessageBox.Show("No date selectedа.");
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

        #region Selections Changed

        private void DonorDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (donors.Count == 0) return;
            _donor = DGDonors.SelectedItem as SingleDonor;

            donor_name.Text = _donor.donor_name;
            donor_blood_group.Text = _donor.donor_blood_group;
            donor_phone_number.Text = _donor.donor_phone_number;
            donor_address.Text = _donor.donor_address;
            last_donation_date.Text = (_donor.last_donation_date).ToLongDateString();
        }

        private void TakerDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (takers.Count == 0) return;
            _taker = DGTakers.SelectedItem as SingleTaker;

            taker_name.Text = _taker.taker_name;
            taker_blood_group.Text = _taker.taker_blood_group;
            taker_phone_number.Text = _taker.taker_phone_number;
            taker_address.Text = _taker.taker_address;
        }

        private void DGBloodBank_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (bloodbanks.Count == 0) return;
            _bloodbank = DGBloodBank.SelectedItem as SingleBloodBank;

            BloodBank_id.Text = _bloodbank.bloodbank_id;
            BloodBank_name.Text = _bloodbank.bloodbank_name;
            BloodBank_blood_group.Text = _bloodbank.blood_group;
            BloodBank_amount.Text = Convert.ToString(_bloodbank.blood_amount);
            BloodBank_checking_date.Text = _bloodbank.blood_status;
        }

        private void DGTransferDetails_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (transferdetails.Count == 0) return;
            _transfDet = DGTransferDetails.SelectedItem as SingleTransferDetails;

            TransferDetails_bb_id.Text = _transfDet.bloodbank_id;
            TransferDetails_donor_id.Text = Convert.ToString(_transfDet.donor_id);
            TransferDetails_taker_id.Text = Convert.ToString(_transfDet.taker_id);
            TransferDetails_blood_group.Text = _transfDet.blood_group;
            TransferDetails_amount.Text = Convert.ToString(_transfDet.blood_amount);
            TransferDetails_date.Text = Convert.ToString(_transfDet.transfer_date);
        }

        #endregion

        #region ExportImport

        private void ExportTakersBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (Entities ent = new Entities())
                {
                    ent.EXPORT_TAKERS_TO_XML();
                }
                MessageBox.Show("Data export from Taker table was successful!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        private void ImportTakersBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (Entities ent = new Entities())
                {
                    ent.IMPORT_TAKERS_FROM_XML();
                }
                MessageBox.Show("Data import from XML document was successful! Please, refresh data.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        #endregion
    }
}