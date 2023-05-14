--==========================INSERT=========================
-----------------------------DONOR
BEGIN
  ADD_NEW_DONOR('Maria','A+','Minsk','12345','01-MAY-2023');
END;
/
select * from Donor;
------------------------------TAKER
BEGIN
   ADD_NEW_TAKER('Kirill','A+','Minsk','+37545888');
END;
/
select * from Taker;
-----------------------------BLOUD_BANK
BEGIN
  ADD_NEW_BLOOD_BANK('H01','HEAVEN','A+',18.00,'15-MAR-2020');
END;
/
select * from blood_bank;
---------------------------TRANSFER_DITAIL
BEGIN
   ADD_NEW_TRANSFER_DETAILS('D01',13,13,'A+','5.00','08-MAY-2023');
END;
/
select * from transfer_details;
---------------------------------------Account
BEGIN
   ADD_NEW_ACCOUNT('trainee','qwerty');
END;
/
select * from account;
--==========================UPDATE=========================
------------------------DONOR
BEGIN
  UPDATE_DONOR_INFO(12,'Ms Maria','Minsk','375333568999','01-MAY-23');
END;
/
select * from donor;
------------------------TAKER
BEGIN
UPDATE_TAKER_INFO('12','Mr Kirill','Minsk','+37545888');
END;
/
select * from taker;
------------------------BLOOD_BANK
BEGIN
	update_blood_bank('H02','B+','30.00');
END;
/
select * from Blood_Bank;	
----------------------TRANSFER_DETAILS
BEGIN
   UPDATE_TRANSFER_DETAILS(10,'D01',13,13,'A+','3.00','08-MAY-2023');
END;
/
select * from transfer_details;
---------------------------------------Account
BEGIN
  UPDATE_ACCOUNT('1', 'trainee', '1111');
END;
/
select * from account;
--==========================DELETE=========================
------------------------DONOR
BEGIN
  DELETE_DONOR('12');
END;
/
select * from donor;
------------------------TAKER
BEGIN
  DELETE_TAKER('12');
END;
/
select * from taker;
------------------------BLOOD_BANK
BEGIN
  DELETE_BLOOD_BANK('H01');
END;
/
select * from blood_bank;
----------------------TRANSFER_DETAILS
BEGIN
  DELETE_TRANSFER_DETAILS(10);
END;
/ 
BEGIN
  DELETE_TRANSFER_DETAILS_BY_PARAMS('D01', 13, 13);
END;
/ 
select * from transfer_details;
----------------------ACCOUNT
BEGIN
  DELETE_ACCOUNT('1');
END;
/
select * from account;
-------------------------------------------------------------------------------


select * from user_procedures;
select * from user_tables;