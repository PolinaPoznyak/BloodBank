----------------------- DROP ALL TABLES ------------------------------

drop table Transfer_Details cascade constraints;
drop table Donor cascade constraints;
drop table Taker cascade constraints;
drop table Blood_Bank cascade constraints;
drop table Account cascade constraints;

------------------------ CREATE ALL TABLES ----------------------------

create table Donor
(
	donor_id number generated always as identity,
	donor_name varchar(50) not null,
	donor_blood_group varchar(20) not null,
	donor_phone_number varchar(20),
	donor_address varchar(50) not null,
	last_donation_date date not null,
	donor_status varchar(20),
	primary key(donor_id)
);

create table Taker
(
	taker_id number generated always as identity,
	taker_name varchar(50) not null,
	taker_blood_group varchar(20) not null,
	taker_address varchar(50) not null,
	taker_phone_number varchar(20),
	primary key (taker_id)
);

create table Blood_Bank
(
	bloodbank_id varchar(10),
	bloodbank_name varchar(50) not null,
	blood_group varchar(20)not null,
	blood_amount float,
	blood_status varchar(20),
	checking_date date,
	primary key(bloodbank_id)
);

create table Transfer_Details
(
  transer_id number generated always as identity,
	bloodbank_id varchar(10),
	donor_id number,
	taker_id number,
	blood_group varchar(20),
	blood_amount float,
	transfer_date date,
	foreign key (bloodbank_id) references Blood_Bank on delete cascade,
	foreign key (donor_id) references Donor on delete cascade,
	foreign key (taker_id) references Taker on delete cascade	
);

create table Account
(
	userid number generated always as identity primary key,
  login varchar2(30) not null,
	password NVARCHAR2(100)
);

------------------------ TABLE INFORMATION -------------------------
		
describe Donor;
describe Taker;
describe Transfer_Details;
describe Blood_Bank;
describe Account;


------------------------- TRIGGER for Donor Table ------------------------

DROP TRIGGER TR_Donor; 
CREATE or REPLACE TRIGGER TR_Donor 
BEFORE UPDATE OR INSERT ON Donor
FOR EACH ROW 
BEGIN
      IF EXTRACT(DAY FROM(systimestamp - :new.last_donation_date)) < 90  THEN
        :new.donor_status := 'Not Available';
      else
		:new.donor_status := 'Available';
    END IF;
END TR_Donor;
/
SHOW ERRORS;

------------------- INSERT DATA INTO Donor & Taker Table -----------------


insert into Donor(donor_name,donor_blood_group,donor_address,donor_phone_number,last_donation_date,donor_status) values ('Mr Prosun','O+','Khulna','01768186003','22-DEC-2016',null);
insert into Donor(donor_name,donor_blood_group,donor_address,donor_phone_number,last_donation_date,donor_status) values ('Mr Tusher','A+','Rangpur','01745001539','12-JAN-2017',null);
insert into Donor(donor_name,donor_blood_group,donor_address,donor_phone_number,last_donation_date,donor_status) values ('Mr Billah','B+','Dhaka','01674304054','02-AUG-2016',null);
insert into Donor(donor_name,donor_blood_group,donor_address,donor_phone_number,last_donation_date,donor_status) values ('Mr Shorol','AB+','Dhaka','01720356489','21-FEB-2017',null);
insert into Donor(donor_name,donor_blood_group,donor_address,donor_phone_number,last_donation_date,donor_status) values ('Mr Arnab','O+','Jessore','01912564578','31-MAY-2016',null);
insert into Donor(donor_name,donor_blood_group,donor_address,donor_phone_number,last_donation_date,donor_status) values ('Mr Mehedi','AB+','Rangpur','01895626489','22-SEP-2016',null);
insert into Donor(donor_name,donor_blood_group,donor_address,donor_phone_number,last_donation_date,donor_status) values ('Mr Bahadur','B+','Chittagang','01556456449','22-JAN-2017',null);
insert into Donor(donor_name,donor_blood_group,donor_address,donor_phone_number,last_donation_date,donor_status) values ('Mr Dibbo','AB+','Dhaka','01854787895','02-MAY-2017',null);
insert into Donor(donor_name,donor_blood_group,donor_address,donor_phone_number,last_donation_date,donor_status) values ('Mr Amit','O-','Dhaka','01996797754','12-JUN-2017',null);
insert into Donor(donor_name,donor_blood_group,donor_address,donor_phone_number,last_donation_date,donor_status) values ('Mr Fahim','B+','Dhaka','01956489312','01-MAY-2017',null);
insert into Donor(donor_name,donor_blood_group,donor_address,donor_phone_number,last_donation_date,donor_status) values ('Mr Naeem','AB+','Dhaka','01956489312','01-MAY-2017',null);
select * from Donor;


insert into Taker(taker_name,taker_blood_group,taker_address,taker_phone_number) values ('Mr Shishir','A+','Dhaka','01785642980');
insert into Taker(taker_name,taker_blood_group,taker_address,taker_phone_number) values ('Mr Shovon','B+','Comilla','01676892449');
insert into Taker(taker_name,taker_blood_group,taker_address,taker_phone_number) values ('Mr Shopto','O+','Jenaidah','01941391259');
insert into Taker(taker_name,taker_blood_group,taker_address,taker_phone_number) values ('Mr Ashik','B+','Dhaka','01758514578');
insert into Taker(taker_name,taker_blood_group,taker_address,taker_phone_number) values ('Mr Prem','AB+','Rajshahi','01854785647');
insert into Taker(taker_name,taker_blood_group,taker_address,taker_phone_number) values ('Mr Bikash','O-','Khulna','01987451971');
insert into Taker(taker_name,taker_blood_group,taker_address,taker_phone_number) values ('Mr Mubin','A+','Dhaka','01777125472');
insert into Taker(taker_name,taker_blood_group,taker_address,taker_phone_number) values ('Mr Pranto','AB+','Dhaka','01833264851');
insert into Taker(taker_name,taker_blood_group,taker_address,taker_phone_number) values ('Mr Tawhid','O+','Khulna','01658784125');
insert into Taker(taker_name,taker_blood_group,taker_address,taker_phone_number) values ('Mr Wahid','O+','Khulna','01554786932');	
insert into Taker(taker_name,taker_blood_group,taker_address,taker_phone_number) values ('Mr Tanjim','O+','Khulna','01554786932');	
select * from taker;
----------------------- TRIGGERS for Blood_Bank Table ---------------------	

DROP TRIGGER TR_Bank; 
CREATE or REPLACE TRIGGER TR_Bank 
BEFORE UPDATE OR INSERT ON Blood_Bank
FOR EACH ROW 
BEGIN
	
	if :new.blood_amount < 1 then
		:new.blood_status := 'Not Available';
	elsif :new.blood_amount >1 and 	:new.blood_amount <20 then
		:new.blood_status := 'Only For Emergency';
	elsif :new.blood_amount >20 and 	:new.blood_amount <100 then
		:new.blood_status := 'Good Collection';
	elsif :new.blood_amount >100 then
		:new.blood_status := 'Adequate';
			
    END IF;
END TR_Bank;
/
SHOW ERRORS;	
	
DROP TRIGGER TR_Bank2; 
CREATE or REPLACE TRIGGER TR_Bank2 
BEFORE UPDATE OR INSERT ON Blood_Bank
FOR EACH ROW 
BEGIN
	
	if :new.blood_group = 'A+' or :new.blood_group = 'A-' or :new.blood_group = 'B+' or :new.blood_group = 'B-' or :new.blood_group = 'AB+' or :new.blood_group = 'AB-' or :new.blood_group = 'O+' or :new.blood_group = 'O-' then
		dbms_output.put_line('Blood group insertion is Okay');
	else
		RAISE_APPLICATION_ERROR(-20000,'Incorrect Blood Group Insertion');
    END IF;
END TR_Bank2;
/
SHOW ERRORS;	
	
-------------- INSERT DATA INTO Blood_Bank & Transfer_Details Table ----------

insert into Blood_Bank (bloodbank_id,bloodbank_name,blood_group,blood_amount,blood_status,checking_date) values ('D01','DREAM','A+',80.00,null,'15-MAR-2020');
insert into Blood_Bank (bloodbank_id,bloodbank_name,blood_group,blood_amount,blood_status,checking_date) values ('D02','DREAM','A-',25.00,null,'15-MAR-2020');
insert into Blood_Bank (bloodbank_id,bloodbank_name,blood_group,blood_amount,blood_status,checking_date) values ('D03','DREAM','B+',90.00,null,'15-MAR-2020');
insert into Blood_Bank (bloodbank_id,bloodbank_name,blood_group,blood_amount,blood_status,checking_date) values ('D04','DREAM','B-',15.00,null,'15-MAR-2020');
insert into Blood_Bank (bloodbank_id,bloodbank_name,blood_group,blood_amount,blood_status,checking_date) values ('D05','DREAM','AB+',60.00,null,'15-MAR-2020');
insert into Blood_Bank (bloodbank_id,bloodbank_name,blood_group,blood_amount,blood_status,checking_date) values ('D06','DREAM','AB-',34.00,null,'15-MAR-2020');
insert into Blood_Bank (bloodbank_id,bloodbank_name,blood_group,blood_amount,blood_status,checking_date) values ('D07','DREAM','O+',200.00,null,'15-MAR-2020');
insert into Blood_Bank (bloodbank_id,bloodbank_name,blood_group,blood_amount,blood_status,checking_date) values ('D08','DREAM','O-',55.00,null,'15-MAR-2020');
select * from blood_bank;

insert into Transfer_Details (bloodbank_id,donor_id,taker_id,blood_group,blood_amount,transfer_date) values ('D07',1,3,'O+',3.00,'18-JAN-2017');
insert into Transfer_Details (bloodbank_id,donor_id,taker_id,blood_group,blood_amount,transfer_date) values ('D01',2,1,'A+',4.00,'17-FEB-2017');
insert into Transfer_Details (bloodbank_id,donor_id,taker_id,blood_group,blood_amount,transfer_date) values ('D03',3,4,'B+',6.00,'19-MAR-2017');
insert into Transfer_Details (bloodbank_id,donor_id,taker_id,blood_group,blood_amount,transfer_date) values ('D05',4,5,'AB+',2.00,'21-DEC-2016');
insert into Transfer_Details (bloodbank_id,donor_id,taker_id,blood_group,blood_amount,transfer_date) values ('D07',5,10,'O+',4.00,'18-NOV-2016');
insert into Transfer_Details (bloodbank_id,donor_id,taker_id,blood_group,blood_amount,transfer_date) values ('D05',6,8,'AB+',2.00,'18-JUL-2017');
insert into Transfer_Details (bloodbank_id,donor_id,taker_id,blood_group,blood_amount,transfer_date) values ('D08',9,6,'O-',6.00,'13-MAR-2017');
insert into Transfer_Details (bloodbank_id,donor_id,taker_id,blood_group,blood_amount,transfer_date) values ('D07',10,9,'O+',3.00,'17-AUG-2016');	
select * from transfer_details;
DELETE FROM transfer_details;
DELETE FROM taker;

commit;


 
create index donor_blood_group_idx on Donor (donor_blood_group);
create index taker_blood_group_idx on Taker (taker_blood_group);
create index blood_group_idx on Blood_Bank (blood_group);
create index donor_id_idx on Transfer_Details (donor_id);
create index taker_id_idx on Transfer_Details (taker_id);
create index blood_group_transfer_idx on Transfer_Details (blood_group);