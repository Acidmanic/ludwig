
create table RequestUpdates(
        Id bigint AUTO_INCREMENT PRIMARY KEY,
        `Key` varchar(128),
        Value nvarchar(256),
        Type integer,
        AuthorizationRecordId bigint
);

--SPLIT

create procedure spReadRequestUpdateDalById(IN Id bigint)
begin
    select * from RequestUpdates where RequestUpdates.Id = Id;
end;

--SPLIT

create procedure spReadAllRequestUpdateDals()
begin
    select * from RequestUpdates;
end;

--SPLIT

create procedure spInsertRequestUpdateDal(IN `Key` varchar(128),
                                          IN Value nvarchar(256),
                                          IN Type integer,
                                          IN AuthorizationRecordId bigint)
begin
    insert into RequestUpdates (`Key`, Value, Type, AuthorizationRecordId) 
        values (`Key`,Value,Type,AuthorizationRecordId);
    select * from RequestUpdates where RequestUpdates.Id=last_insert_id();
end;

--SPLIT

create procedure spDeleteRequestUpdateDalById(IN Id bigint)
BEGIN
    delete from RequestUpdates where RequestUpdates.Id=Id;
    select TRUE success;
END;

--SPLIT

create procedure spUpdateRequestUpdateDal(IN Id bigint,
                                          IN `Key` varchar(128),
                                          IN Value nvarchar(256),
                                          IN Type integer,
                                          IN AuthorizationRecordId bigint)
BEGIN
    update RequestUpdates
    set `Key`=`Key`,Value=Value,Type=Type,AuthorizationRecordId=AuthorizationRecordId
    where RequestUpdates.Id=Id;
    select TRUE success;
END;