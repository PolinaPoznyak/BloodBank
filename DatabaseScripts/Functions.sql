------------------FUNCTIONS OPERATION---------------------

----1. Using Function to show the amount of blood transfer for a particular blood group -----

CREATE OR REPLACE FUNCTION TOTAL_BLOOD_TRANSFER RETURN NUMBER is
	total_t NUMBER(4);
BEGIN
    SELECT sum(blood_amount) into total_t from Blood_Bank where blood_group='&blood_group';
    RETURN total_t;
END TOTAL_BLOOD_TRANSFER;
/
SHOW ERRORS

SET SERVEROUTPUT ON
BEGIN
   dbms_output.put_line('Total Amount Blood Transfer is : ' || TOTAL_BLOOD_TRANSFER || ' L');
END;
/

----2. Using Function to show the number of blood transfer for a particular date -----

CREATE OR REPLACE FUNCTION NOTAL_NO RETURN NUMBER is
	cnt NUMBER(4);
BEGIN
    SELECT count(bloodbank_id) into cnt from Transfer_Details where transfer_date='&transfer_date';
    RETURN cnt;
END NOTAL_NO;
/
SHOW ERRORS

SET SERVEROUTPUT ON
BEGIN
   dbms_output.put_line('Total No of Blood Transfer is : ' || NOTAL_NO);
END;
/

--------3. A function that determines amount of blood transfer on a specific date------------------
CREATE OR REPLACE FUNCTION BLOOD_TRANSFERS_VOLUME_ON_DATE RETURN FLOAT IS
  total_volume FLOAT;
BEGIN
  SELECT SUM(blood_amount) INTO total_volume FROM Transfer_Details WHERE transfer_date = '&transfer_date';
  RETURN total_volume;
END BLOOD_TRANSFERS_VOLUME_ON_DATE;
/
SET SERVEROUTPUT ON

BEGIN
dbms_output.put_line('Total Blood Volume Transferred is : ' || BLOOD_TRANSFERS_VOLUME_ON_DATE);
END;
/
