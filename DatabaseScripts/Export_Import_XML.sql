-----------------------IMPORT-------------------------------
SELECT file_name FROM dba_data_files; 
CREATE OR REPLACE DIRECTORY EXPORT_DATA AS 'C:/TakerExportImport/xml/export';
CREATE OR REPLACE DIRECTORY IMPORT_DATA AS 'C:/TakerExportImport/xml/import';

CREATE OR REPLACE PROCEDURE EXPORT_TAKERS_TO_XML
IS
    doc DBMS_XMLDOM.DOMDocument;
    xdata XMLTYPE;
    CURSOR xmlcur IS
        SELECT XMLELEMENT(
          "Takers",
           XMLAttributes('http://www.w3.org/2001/XMLSchema' AS "xmlns:xsi",
            'http://www.oracle.com/Users.xsd' AS "xsi:nonamespaceSchemaLocation"),
              XMLAGG(XMLELEMENT("Taker",
              xmlelement("id", Taker.taker_id),
              xmlelement("name", Taker.taker_name),
              xmlelement("blood_group", Taker.taker_blood_group),
              xmlelement("address", Taker.taker_address),
              xmlelement("phone_number", Taker.taker_phone_number)
            ))) from Taker;
BEGIN
open xmlcur;
    LOOP
        FETCH xmlcur INTO xdata;
        EXIT WHEN xmlcur%notfound;
    END LOOP;
    CLOSE xmlcur;
    doc := DBMS_XMLDOM.NewDOMDocument(xdata);
    DBMS_XMLDOM.WRITETOFILE(doc, 'EXPORT_DATA/Takers.xml');
END;
/SHOW ERRORS

begin
  EXPORT_TAKERS_TO_XML();
end;

-----------------------IMPORT-------------------------------
CREATE OR REPLACE PROCEDURE IMPORT_TAKERS_FROM_XML
IS
    L_CLOB CLOB;
    L_BFILE BFILE := BFILENAME('IMPORT_DATA', 'takers.xml');
    L_DEST_OFFSET INTEGER := 1;
    L_SRC_OFFSET INTEGER := 1;
    L_BFILE_CSID NUMBER := 0;
    L_LANG_CONTEXT INTEGER := 0;
    L_WARNING INTEGER := 0;
    P DBMS_XMLPARSER.PARSER;
    V_DOC DBMS_XMLDOM.DOMDOCUMENT;
    V_ROOT_ELEMENT DBMS_XMLDOM.DOMELEMENT;
    V_CHILD_NODES DBMS_XMLDOM.DOMNODELIST;
    V_CURRENT_NODE DBMS_XMLDOM.DOMNODE;
    tk Taker%ROWTYPE;
BEGIN
    DBMS_LOB.CREATETEMPORARY (L_CLOB, TRUE);
    DBMS_LOB.FILEOPEN(L_BFILE, DBMS_LOB.FILE_READONLY);
    DBMS_LOB.LOADCLOBFROMFILE(DEST_LOB => L_CLOB, SRC_BFILE => L_BFILE, AMOUNT => DBMS_LOB.LOBMAXSIZE,
        DEST_OFFSET => L_DEST_OFFSET, SRC_OFFSET => L_SRC_OFFSET, BFILE_CSID => L_BFILE_CSID,
        LANG_CONTEXT => L_LANG_CONTEXT, WARNING => L_WARNING);
    DBMS_LOB.FILECLOSE(L_BFILE);
    COMMIT;
    
    P := DBMS_XMLPARSER.NEWPARSER;
    DBMS_XMLPARSER.PARSECLOB(P, L_CLOB);
    V_DOC := DBMS_XMLPARSER.GETDOCUMENT(P);
    V_ROOT_ELEMENT := DBMS_XMLDOM.Getdocumentelement(V_DOC);
    V_CHILD_NODES := DBMS_XMLDOM.GETCHILDRENBYTAGNAME(V_ROOT_ELEMENT, '*');
    
    FOR i IN 0 .. DBMS_XMLDOM.GETLENGTH(V_CHILD_NODES) - 1 LOOP
        V_CURRENT_NODE := DBMS_XMLDOM.ITEM(V_CHILD_NODES, i);
        
        DBMS_XSLPROCESSOR.VALUEOF(V_CURRENT_NODE,
            'name/text()', tk.taker_name);
        DBMS_XSLPROCESSOR.VALUEOF(V_CURRENT_NODE,
            'blood_group/text()', tk.taker_blood_group);
        DBMS_XSLPROCESSOR.VALUEOF(V_CURRENT_NODE,
            'address/text()', tk.taker_address);
        DBMS_XSLPROCESSOR.VALUEOF(V_CURRENT_NODE,
            'phone_number/text()', tk.taker_phone_number);
        
        INSERT INTO Taker (taker_name, taker_blood_group, taker_address, taker_phone_number)
        VALUES (tk.taker_name, tk.taker_blood_group, tk.taker_address, tk.taker_phone_number);
END LOOP;
    
    DBMS_LOB.FREETEMPORARY(L_CLOB);
    DBMS_XMLPARSER.FREEPARSER(P);
    DBMS_XMLDOM.FREEDOCUMENT(V_DOC);
    COMMIT;
END;
/
SHOW ERRORS

begin
    IMPORT_TAKERS_FROM_XML();
end;

select * from taker;