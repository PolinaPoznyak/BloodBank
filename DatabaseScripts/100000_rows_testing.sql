set timing on serveroutput on

--make a loop for 10000 iterations and insert in tracks table

BEGIN
FOR Lcntr IN 1..100000
LOOP
   ADD_NEW_DONOR('Testing','A+','Minsk','12345','01-MAY-2023');
END LOOP;
END;

begin
  DELETE FROM Donor WHERE donor_name = 'Testing';
end;

select * from donor;

declare
    start_time number := dbms_utility.get_time();
begin
    for r in (select * from Donor where donor_id > 80000 and donor_id < 90000) loop null; end loop; 
    dbms_output.put_line('Elapsed time: '||(dbms_utility.get_time() - start_time)/100); 
end;

create index index_donors_id on Donor(donor_id, donor_name, donor_blood_group);

declare
    start_time number := dbms_utility.get_time();
begin
    for r in (select * from Donor where donor_id > 80000 and donor_id < 90000) loop null; end loop; 
    dbms_output.put_line('Elapsed time: '||(dbms_utility.get_time() - start_time)/100); 
end;

drop index index_donors_id;

select * from donor;
