-- ---------------------------------------------------------------------------------------------------------------------
create table AuthorizationRecords(
           Id bigint AUTO_INCREMENT PRIMARY KEY,
           Token varchar(64),
           ExpirationEpoch bigint,
           LoginMethodName nvarchar(128),
           IsAdministrator bool,
           IsIssueManager bool,
           SubjectId nvarchar(256),
           EmailAddress nvarchar(64),
           SubjectWebPage nvarchar(128),
           Cookie nvarchar(256),
           RequestOrigin nvarchar(128));
-- ---------------------------------------------------------------------------------------------------------------------
-- ---------------------------------------------------------------------------------------------------------------------
create view AuthorizationRecordsFullTree as 
    
    select
        AuthorizationRecords.Id 'AuthorizationRecords_Id',
        Token,
        ExpirationEpoch,
        LoginMethodName,
        IsAdministrator,
        IsIssueManager ,
        SubjectId ,
        EmailAddress ,
        SubjectWebPage,
        Cookie,
        RequestOrigin,
        RequestUpdates.Id 'RequestUpdates_Id',
        `Key`,
        Value,
        Type,
        AuthorizationRecordId
    from AuthorizationRecords 
    left join RequestUpdates on AuthorizationRecords.Id = RequestUpdates.AuthorizationRecordId;
-- ---------------------------------------------------------------------------------------------------------------------
#SPLIT
-- ---------------------------------------------------------------------------------------------------------------------
create procedure spReadAuthorizationRecordDalById(IN Id bigint)
begin
    select * from AuthorizationRecords where AuthorizationRecords.Id = Id;
end;
-- ---------------------------------------------------------------------------------------------------------------------
#SPLIT
-- ---------------------------------------------------------------------------------------------------------------------
create procedure spReadAuthorizationRecordByIdFullTree(IN Id bigint)
begin
    select * from AuthorizationRecordsFullTree where AuthorizationRecordsFullTree.AuthorizationRecords_Id = Id;
end;
-- ---------------------------------------------------------------------------------------------------------------------
#SPLIT
-- ---------------------------------------------------------------------------------------------------------------------
create procedure spReadAllAuthorizationRecordsFullTree()
begin
    select * from AuthorizationRecordsFullTree;
end;
-- ---------------------------------------------------------------------------------------------------------------------
#SPLIT
-- ---------------------------------------------------------------------------------------------------------------------
create procedure spInsertAuthorizationRecordDal(IN Token varchar(64),
                                          IN ExpirationEpoch bigint,
                                          IN LoginMethodName nvarchar(128),
                                          IN IsAdministrator bool,
                                          IN IsIssueManager bool,
                                          IN SubjectId nvarchar(256),
                                          IN EmailAddress nvarchar(64),
                                          IN SubjectWebPage nvarchar(128),
                                          IN Cookie nvarchar(256),
                                          IN RequestOrigin nvarchar(128))
begin
    insert into AuthorizationRecords (Token, ExpirationEpoch, LoginMethodName, IsAdministrator, IsIssueManager, SubjectId, EmailAddress, SubjectWebPage, Cookie, RequestOrigin)
    values (Token, ExpirationEpoch, LoginMethodName, IsAdministrator, IsIssueManager, SubjectId, EmailAddress, SubjectWebPage, Cookie, RequestOrigin); 
    select * from AuthorizationRecords where AuthorizationRecords.Id=last_insert_id();
end;
-- ---------------------------------------------------------------------------------------------------------------------
#SPLIT
-- ---------------------------------------------------------------------------------------------------------------------
create procedure spDeleteAuthorizationRecordDalById(IN Id bigint)
BEGIN
    delete from AuthorizationRecords where AuthorizationRecords.Id=Id;
    select TRUE success;
END;
-- ---------------------------------------------------------------------------------------------------------------------
#SPLIT
-- ---------------------------------------------------------------------------------------------------------------------
create procedure spUpdateAuthorizationRecordDal(IN Id bigint,
                                                IN Token varchar(64),
                                                IN ExpirationEpoch bigint,
                                                IN LoginMethodName nvarchar(128),
                                                IN IsAdministrator bool,
                                                IN IsIssueManager bool,
                                                IN SubjectId nvarchar(256),
                                                IN EmailAddress nvarchar(64),
                                                IN SubjectWebPage nvarchar(128),
                                                IN Cookie nvarchar(256),
                                                IN RequestOrigin nvarchar(128))
BEGIN
    update AuthorizationRecords
    set Token=Token,ExpirationEpoch=ExpirationEpoch,LoginMethodName=LoginMethodName,
        IsAdministrator=IsAdministrator,IsIssueManager=IsIssueManager,SubjectId=SubjectId,
        EmailAddress=EmailAddress,SubjectWebPage=SubjectWebPage,Cookie=Cookie,RequestOrigin=RequestOrigin
    where AuthorizationRecords.Id=Id;
    select TRUE success;
END;
-- ---------------------------------------------------------------------------------------------------------------------
-- ---------------------------------------------------------------------------------------------------------------------
-- ---------------------------------------------------------------------------------------------------------------------