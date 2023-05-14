------------------PROCEDURES OPERATION---------------------
----1. Add new donor -----
CREATE OR REPLACE PROCEDURE ADD_NEW_DONOR(
  dname Donor.donor_name%TYPE,
  dblood Donor.donor_blood_group%TYPE,
  daddress Donor.donor_address%TYPE,
  dcontact Donor.donor_phone_number%TYPE,
  dldd Donor.last_donation_date%TYPE
  ) IS
BEGIN
  insert into Donor(donor_name,donor_blood_group,donor_address,donor_phone_number,last_donation_date,donor_status) 
  values (dname,dblood,daddress,dcontact,dldd,null);
  commit;
  
END ADD_NEW_DONOR;
/
SHOW ERRORS
BEGIN
  ADD_NEW_DONOR('&donor_name','&donor_blood_group','&donor_address','&donor_phone_number','&last_donation_date');
END;
/
----2. Add new taker -----
CREATE OR REPLACE PROCEDURE ADD_NEW_TAKER(
  tname Taker.taker_name%TYPE,
  tblood Taker.taker_blood_group%TYPE,
  taddress Taker.taker_address%TYPE,
  tcontact Taker.taker_phone_number%TYPE
  ) IS
BEGIN
  insert into Taker(taker_name,taker_blood_group,taker_address,taker_phone_number) 
  values (tname,tblood,taddress,tcontact);
  
  commit;
END ADD_NEW_TAKER;
/
SHOW ERRORS
BEGIN
   ADD_NEW_TAKER('&taker_name','&taker_blood_group','&taker_address','&taker_phone_number');
END;
/
----3. Add new blood bank-----
CREATE OR REPLACE PROCEDURE ADD_NEW_BLOOD_BANK(
  bbid Blood_Bank.bloodbank_id%TYPE,
  bbname Blood_Bank.bloodbank_name%TYPE,
  bbbgroup Blood_Bank.blood_group%TYPE,
  bbbamount Blood_Bank.blood_amount%TYPE,
  bbchaeckingd Blood_Bank.checking_date%TYPE
  ) IS
BEGIN
  insert into Blood_Bank(bloodbank_id,bloodbank_name,blood_group,blood_amount,blood_status,checking_date) 
  values (bbid,bbname,bbbgroup,bbbamount,null,bbchaeckingd);
  commit;
END ADD_NEW_BLOOD_BANK;
/
SHOW ERRORS
BEGIN
   ADD_NEW_BLOOD_BANK('&bloodbank_id','&bloodbank_name','&blood_group','&blood_amount','&checking_date');
END;
/
----4. Add new Transfer-----
CREATE OR REPLACE PROCEDURE ADD_NEW_TRANSFER_DETAILS(
  bbid transfer_details.bloodbank_id%TYPE,
  donorid transfer_details.donor_id%TYPE,
  takerid transfer_details.taker_id%TYPE,
  bloodgroup transfer_details.blood_group%TYPE,
  bloodamount transfer_details.blood_amount%TYPE,
  transferdate transfer_details.transfer_date%TYPE
  ) IS
BEGIN
  insert into transfer_details(bloodbank_id,donor_id,taker_id,blood_group,blood_amount,transfer_date) 
  values (bbid,donorid,takerid,bloodgroup,bloodamount,transferdate); 
  commit;
END ADD_NEW_TRANSFER_DETAILS;
/
SHOW ERRORS
BEGIN
   ADD_NEW_TRANSFER_DETAILS('&bloodbank_id','&donor_id','&taker_id','&blood_group','&blood_amount','&transfer_date');
END;
/
----5. Add new Account
CREATE OR REPLACE PROCEDURE ADD_NEW_ACCOUNT(
    p_login account.login%TYPE,
    p_password account.password%TYPE
)
IS
BEGIN
    INSERT INTO Account(login, password) 
    VALUES(p_login, p_password);
    COMMIT;
END;
/
SHOW ERRORS
BEGIN
   ADD_NEW_ACCOUNT('&loggin','&password');
END;
/
---------------UPDATE--------------------
----1. Update donor -----
CREATE OR REPLACE PROCEDURE UPDATE_DONOR_INFO(
  d_id Donor.donor_id%TYPE,
  dname Donor.donor_name%TYPE,
  daddress Donor.donor_address%TYPE,
  dcontact Donor.donor_phone_number%TYPE,
  dldd Donor.last_donation_date%TYPE
) IS
BEGIN
  UPDATE Donor
  SET donor_name = dname,
  donor_address = daddress,
  donor_phone_number = dcontact,
  last_donation_date = dldd
  WHERE donor_id = d_id;
  COMMIT;
END UPDATE_DONOR_INFO;
/
SHOW ERRORS
BEGIN
  UPDATE_DONOR_INFO('&donor_id','&donor_name','&donor_address','&donor_phone_number','&last_donation_date');
END;
/
----2. Update taker -----
CREATE OR REPLACE PROCEDURE UPDATE_TAKER_INFO(
  t_id Taker.taker_id%TYPE,
  tname Taker.taker_name%TYPE,
  taddress Taker.taker_address%TYPE,
  tcontact Taker.taker_phone_number%TYPE
) IS
BEGIN
  UPDATE Taker
    SET taker_name = tname,
    taker_address = taddress,
    taker_phone_number = tcontact
    WHERE taker_id = t_id;
  COMMIT;
END UPDATE_TAKER_INFO;
/
SHOW ERRORS
BEGIN
UPDATE_TAKER_INFO('&taker_id','&taker_name','&taker_address','&taker_phone_number');
END;
/
----3. Update blood bank -----
CREATE OR REPLACE PROCEDURE UPDATE_BLOOD_BANK(
	id Blood_Bank.bloodbank_id%TYPE,
	bgroup Blood_Bank.blood_group%TYPE,
	amount Blood_Bank.blood_amount%TYPE
) IS
BEGIN
	UPDATE Blood_Bank set blood_amount=amount,checking_date=current_date where bloodbank_id=id and blood_group=bgroup;
EXCEPTION
  WHEN no_data_found THEN 
    RAISE_APPLICATION_ERROR(-20203, 'No Data found.');   
    COMMIT;
END UPDATE_BLOOD_BANK;
/
SHOW ERRORS
BEGIN
	UPDATE_BLOOD_BANK('&bloodbank_id','&blood_group','&blood_amount');
END;
/
SHOW ERRORS;
------4. Update transfer_details
CREATE OR REPLACE PROCEDURE UPDATE_TRANSFER_DETAILS(
  tid Transfer_Details.transer_id%TYPE,
  bbid Transfer_Details.bloodbank_id%TYPE,
  donorid Transfer_Details.donor_id%TYPE,
  takerid Transfer_Details.taker_id%TYPE,
  bloodgroup Transfer_Details.blood_group%TYPE,
  bloodamount Transfer_Details.blood_amount%TYPE,
  transferdate Transfer_Details.transfer_date%TYPE
) IS
BEGIN
  UPDATE Transfer_Details
  SET bloodbank_id = bbid, 
      donor_id = donorid, 
      taker_id = takerid, 
      blood_group = bloodgroup, 
      blood_amount = bloodamount, 
      transfer_date = transferdate
  WHERE transer_id = tid;
  
  COMMIT;
END UPDATE_TRANSFER_DETAILS;
/
SHOW ERRORS
BEGIN
   UPDATE_TRANSFER_DETAILS('&transer_id','&bloodbank_id','&donor_id','&taker_id','&blood_group','&blood_amount','&transfer_date');
END;
/
-----5. Update Account
CREATE OR REPLACE PROCEDURE UPDATE_ACCOUNT(
  accid account.userid%TYPE,
  acclogin account.login%TYPE,
  accpassword account.password%TYPE
) IS
BEGIN
  UPDATE Account set login=acclogin,password=accpassword where userid=accid;
EXCEPTION
  WHEN no_data_found THEN 
    RAISE_APPLICATION_ERROR(-20203, 'No Data found.');
  
  COMMIT;
END UPDATE_ACCOUNT;
/
SHOW ERRORS
BEGIN
  UPDATE_ACCOUNT('&userid', '&login', '&passsword');
END;
/
-------------------------DELETE
----1. Delete Donor
CREATE OR REPLACE PROCEDURE DELETE_DONOR(
  d_id donor.donor_id%TYPE
) IS
BEGIN
  DELETE FROM Donor WHERE donor_id = d_id;

  COMMIT;
END DELETE_DONOR;
/
SHOW ERRORS

BEGIN
  DELETE_DONOR('&donor_id');
END;
/
----2. Delete Taker
CREATE OR REPLACE PROCEDURE DELETE_TAKER(
  t_id Taker.taker_id%TYPE
) IS
BEGIN
  DELETE FROM Taker WHERE taker_id = t_id;

  COMMIT;
END DELETE_TAKER;
/
SHOW ERRORS

BEGIN
  DELETE_TAKER('&taker_id');
END;
/
----3. Delete Blood_Bank
CREATE OR REPLACE PROCEDURE DELETE_BLOOD_BANK(
  bb_id blood_bank.bloodbank_id%TYPE
) IS
BEGIN
  DELETE FROM blood_bank WHERE bloodbank_id = bb_id;
  COMMIT;
END DELETE_BLOOD_BANK;
/
SHOW ERRORS

BEGIN
  DELETE_BLOOD_BANK('&bloodbank_id');
END;
/
---4. Delete Transfer Details
CREATE OR REPLACE PROCEDURE DELETE_TRANSFER_DETAILS(
  transferid transfer_details.transer_id%TYPE
) IS
BEGIN
  DELETE FROM Transfer_Details WHERE transer_id = transferid;
  COMMIT;
END DELETE_TRANSFER_DETAILS;
/
SHOW ERRORS
BEGIN
  DELETE_TRANSFER_DETAILS('&transer_id');
END;
/
--&&--
--drop procedure 
CREATE OR REPLACE PROCEDURE DELETE_TRANSF_DET_BY_PARAMS(
  bbid Transfer_Details.bloodbank_id%TYPE,
  donorid Transfer_Details.donor_id%TYPE,
  takerid Transfer_Details.taker_id%TYPE
) IS
BEGIN
  DELETE FROM Transfer_Details 
    WHERE bloodbank_id = bbid AND donor_id = donorid AND taker_id = takerid; 
  COMMIT;
END DELETE_TRANSF_DET_BY_PARAMS;
/
SHOW ERRORS
-----5. Delete Account
CREATE OR REPLACE PROCEDURE DELETE_ACCOUNT(
  accountid account.userid%TYPE
) IS
BEGIN
  DELETE FROM Account WHERE userid = accountid;
  COMMIT;
END DELETE_ACCOUNT;
/
SHOW ERRORS
BEGIN
  DELETE_ACCOUNT('&userid');
END;
/