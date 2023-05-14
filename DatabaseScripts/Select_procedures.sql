-------------------------------------------------------------------------------
------------------------SELECT without parameters
--1. Get all donors
CREATE OR REPLACE PROCEDURE GETDONORS(
    clients OUT SYS_REFCURSOR
) IS
BEGIN
    OPEN clients FOR 
    SELECT donor_id, donor_name, donor_blood_group, donor_phone_number, donor_address, last_donation_date, donor_status FROM Donor;
END;
/
SHOW ERRORS

VARIABLE donor_curs REFCURSOR;
EXECUTE GETDONORS(:donor_curs);
PRINT donor_curs;

VARIABLE donor_curs REFCURSOR;
EXECUTE BB_ADMIN.GETDONORS(:donor_curs);
PRINT donor_curs;

--2. Get all takers
CREATE OR REPLACE PROCEDURE GETTAKERS(
    takers OUT SYS_REFCURSOR
) IS
BEGIN
    OPEN takers FOR 
    SELECT taker_id, taker_name, taker_blood_group, taker_address, taker_phone_number FROM Taker;
END;
/
SHOW ERRORS

VARIABLE taker_curs REFCURSOR;
EXECUTE GETTAKERS(:taker_curs);
PRINT taker_curs;

--3. Get all blood_banks
CREATE OR REPLACE PROCEDURE GETBLOOD_BANKS(
    blood_banks OUT SYS_REFCURSOR
) IS
BEGIN
    OPEN blood_banks FOR 
    SELECT bloodbank_id, bloodbank_name, blood_group, blood_amount, blood_status, checking_date from blood_bank;
END;
/
SHOW ERRORS

VARIABLE blood_bank_curs REFCURSOR;
EXECUTE GETBLOOD_BANKS(:blood_bank_curs);
PRINT blood_bank_curs;

--4. Get all transfer details for admin
CREATE OR REPLACE PROCEDURE GETTRANSFER_DETAILS(
    transfer_details OUT SYS_REFCURSOR
) IS
BEGIN
    OPEN transfer_details FOR 
    SELECT transer_id, bloodbank_id, donor_id, taker_id, blood_group, blood_amount, transfer_date FROM transfer_details;
END;
/
SHOW ERRORS

VARIABLE trans_det_curs REFCURSOR;
EXECUTE GETTRANSFER_DETAILS(:trans_det_curs);
PRINT trans_det_curs;

--5. Get all transfer details for client
CREATE OR REPLACE PROCEDURE GETTRANSFER_DETAILS_FOR_CLIEN(
    transfer_details OUT SYS_REFCURSOR
) IS
BEGIN
    OPEN transfer_details FOR 
    SELECT tr.transer_id, bb.bloodbank_name, d.donor_name, t.taker_name, tr.blood_group, tr.blood_amount, tr.transfer_date 
    FROM Transfer_details tr
    JOIN Donor d on d.donor_id = tr.donor_id
    JOIN Taker t ON t.taker_id = tr.taker_id
    JOIN Blood_bank bb on bb.bloodbank_id = tr.bloodbank_id;
END;
/
SHOW ERRORS

VARIABLE trans_det_curs REFCURSOR;
EXECUTE GETTRANSFER_DETAILS_FOR_CLIEN(:trans_det_curs);
PRINT trans_det_curs;

--6. Get all accounts details for admin
CREATE OR REPLACE PROCEDURE GETACCOUNTS(
    accunts OUT SYS_REFCURSOR
) IS
BEGIN
    OPEN accunts FOR 
    SELECT userid, login, password FROM account;
END;
/
SHOW ERRORS

VARIABLE acc_curs REFCURSOR;
EXECUTE GETACCOUNTS(:acc_curs);
PRINT acc_curs;
--------------------------------------------------------------------------------
---------------------SELECT with parameters
-------1. Get donors by bloodgroup
CREATE OR REPLACE PROCEDURE FIND_DONORS_BY_BLOODGROUP(check_blood_group IN VARCHAR, donor_cursor OUT SYS_REFCURSOR) IS
BEGIN
    OPEN donor_cursor FOR
    SELECT donor_id, donor_name, donor_phone_number, donor_address, last_donation_date, donor_status
    FROM Donor
    WHERE donor_blood_group = check_blood_group;
END;
/
SHOW ERRORS

VARIABLE cur REFCURSOR;
EXECUTE FIND_DONORS_BY_BLOODGROUP('A+', :cur);
PRINT cur;

select * from donor where donor_blood_group = 'A+';

-------2. Get available donors by donor_blood_group and donor_adress
CREATE OR REPLACE PROCEDURE FIND_AVAIL_DONORS_BY_GR_ADDR(
  check_blood_group IN VARCHAR, 
  check_donoe_address IN VARCHAR,
  donor_cursor OUT SYS_REFCURSOR
) IS
BEGIN
    OPEN donor_cursor FOR
    SELECT donor_id, donor_name, donor_phone_number, donor_address, donor_status
    FROM Donor
    WHERE donor_blood_group = check_blood_group
          AND donor_address = check_donoe_address
          AND donor_status = 'Available';
END;
/
SHOW ERRORS

VARIABLE av_donors REFCURSOR;
EXECUTE FIND_AVAIL_DONORS_BY_GR_ADDR('A+', 'Rangpur', :av_donors);
PRINT av_donors;

-------3. Get unavailable donors by donor_blood_group and donor_adress
CREATE OR REPLACE PROCEDURE FIND_UNAVAIL_DONORS_BY_GR_ADDR(
  check_blood_group IN VARCHAR, 
  check_donoe_address IN VARCHAR,
  donor_cursor OUT SYS_REFCURSOR
) IS
BEGIN
    OPEN donor_cursor FOR
    SELECT donor_id, donor_name, donor_phone_number, donor_address, donor_status
    FROM Donor
    WHERE donor_blood_group = check_blood_group
          AND donor_address = check_donoe_address
          AND donor_status = 'Not Available';
END;
/
SHOW ERRORS

VARIABLE unav_donors REFCURSOR;
EXECUTE FIND_UNAVAIL_DONORS_BY_GR_ADDR('A+', 'Minsk', :unav_donors);
PRINT unav_donors;

---------------------------------------------------------------------------
----Determination of the leader among donors within a period of time specified by the user
--drop procedure GET_BLOOD_LEADER;
CREATE OR REPLACE PROCEDURE GET_BLOOD_LEADER(
    start_date IN DATE,
    end_date IN DATE,
    blood_leader_cursor OUT SYS_REFCURSOR
) IS
BEGIN
    OPEN blood_leader_cursor FOR
      SELECT d.donor_id, d.donor_name, d.donor_phone_number, d.donor_address, SUM(td.blood_amount) AS total_blood_amount
      FROM Donor d
      JOIN Transfer_Details td ON d.donor_id = td.donor_id
        WHERE td.transfer_date BETWEEN start_date AND end_date
      GROUP BY d.donor_id, d.donor_name, d.donor_phone_number, d.donor_address, d.donor_blood_group
      ORDER BY total_blood_amount DESC
    FETCH FIRST ROW ONLY;
END;
/
SHOW ERRORS

VARIABLE blood_leaders REFCURSOR;
EXECUTE GET_BLOOD_LEADER('01-JAN-2016', '31-DEC-2016', :blood_leaders);
PRINT blood_leaders;

----------------
CREATE OR REPLACE PROCEDURE NOTAL_NO (transfer_date IN DATE) AS
  cnt NUMBER(4);
BEGIN
  SELECT COUNT(bloodbank_id) INTO cnt FROM Transfer_Details WHERE transfer_date = transfer_date;
dbms_output.put_line('Total No of Blood Transfer is : ' || cnt);
END NOTAL_NO;
/
-------------------------FUNCTIONS
----1. Using Function to show the amount of blood transfer for a particular blood group -----
CREATE OR REPLACE FUNCTION TOTAL_BLOOD_TRANSFER (p_blood_group IN VARCHAR) RETURN NUMBER 
IS
	total_t NUMBER(4);
BEGIN
    SELECT sum(blood_amount) into total_t from Blood_Bank where blood_group=p_blood_group;
    RETURN total_t;
END TOTAL_BLOOD_TRANSFER;
/
SHOW ERRORS

SET SERVEROUTPUT ON
BEGIN
   dbms_output.put_line('Total Amount Blood Transfer is : ' || TOTAL_BLOOD_TRANSFER('A+') || ' L');
END;
/
----2. Using Function to show the number of blood transfer for a particular date -----
CREATE OR REPLACE FUNCTION NOTAL_NO(transfer_date_in IN DATE) RETURN NUMBER is
  cnt NUMBER(4);
BEGIN
    SELECT count(bloodbank_id) into cnt from Transfer_Details where transfer_date = transfer_date_in;
    RETURN cnt;
END NOTAL_NO;
/
SHOW ERRORS

SET SERVEROUTPUT ON
BEGIN
   dbms_output.put_line('Total No of Blood Transfer is : ' || NOTAL_NO('08-MAY-2023'));
END;
/
--------3. A function that determines amount of blood transfer on a specific date------------------
CREATE OR REPLACE FUNCTION BLOOD_TRANSFERS_VOLUME_ON_DATE(transfer_date IN DATE) RETURN FLOAT IS
  total_volume FLOAT;
BEGIN
  SELECT SUM(blood_amount) INTO total_volume FROM Transfer_Details WHERE transfer_date = transfer_date;
  RETURN total_volume;
END BLOOD_TRANSFERS_VOLUME_ON_DATE;
/

SET SERVEROUTPUT ON

BEGIN
dbms_output.put_line('Total Blood Volume Transferred is : ' || BLOOD_TRANSFERS_VOLUME_ON_DATE(TO_DATE('2023-05-09', 'YYYY-MM-DD')));
END;
/
--=======================================================================
CREATE OR REPLACE PROCEDURE NUM_OF_TRANSF(
  transfer_date_in IN DATE, 
  num_of_transf_result OUT SYS_REFCURSOR
) IS
BEGIN
  OPEN num_of_transf_result FOR
    SELECT count(bloodbank_id) as number_of_transf from Transfer_Details where transfer_date = transfer_date_in;
END NUM_OF_TRANSF;
/
VARIABLE num_of_transf_result REFCURSOR;
EXECUTE NUM_OF_TRANSF('10-FEB-2017', :num_of_transf_result);
PRINT num_of_transf_result;
------------------------------------------------------------------------------
--drop procedure VOLUME_OF_BLOOD_TRANSF_ON_DATE;
CREATE OR REPLACE PROCEDURE VOLUME_OF_BLOOD_TRANSF_ON_DATE(
  selected_date IN DATE,
  volume_result OUT SYS_REFCURSOR
) IS
BEGIN
  OPEN volume_result FOR
    SELECT SUM(blood_amount) AS total_volume_on_date 
    FROM Transfer_Details 
    WHERE transfer_date = selected_date
    GROUP BY transfer_date
    FETCH FIRST ROW ONLY;
END;
/
VARIABLE volume_result REFCURSOR;
EXECUTE VOLUME_OF_BLOOD_TRANSF_ON_DATE('19-MAR-2017', :volume_result);
PRINT volume_result;