-- ---------------------------------------------------------------------------------------------------------------------
-- SPLIT
-- ---------------------------------------------------------------------------------------------------------------------
create table StoryUsers(
     Id bigint AUTO_INCREMENT PRIMARY KEY,
     Name nvarchar(64)
);
-- ---------------------------------------------------------------------------------------------------------------------
-- SPLIT
-- ---------------------------------------------------------------------------------------------------------------------
create procedure spReadAllStoryUsers()
begin
    select * from StoryUsers;
end;
-- ---------------------------------------------------------------------------------------------------------------------
-- SPLIT
-- ---------------------------------------------------------------------------------------------------------------------
create procedure spReadStoryUserById(IN Id bigint)
begin
    select * from StoryUsers where StoryUsers.Id = Id;
end;
-- ---------------------------------------------------------------------------------------------------------------------
-- SPLIT
-- ---------------------------------------------------------------------------------------------------------------------
create procedure spReadStoryUserByName(IN Name nvarchar(64))
begin
    select * from StoryUsers where StoryUsers.Name like Name;
end;
-- ---------------------------------------------------------------------------------------------------------------------
-- SPLIT
-- ---------------------------------------------------------------------------------------------------------------------
create procedure spSaveStoryUser(IN Id bigint,IN Name nvarchar(64))
begin
    if exists(select 1 from StoryUsers where StoryUsers.Name like Name) then
        
        select * from StoryUsers where StoryUsers.Name like Name limit 1;
        
    elseif exists(select 1 from StoryUsers where StoryUsers.Id = Id) then
        
        update StoryUsers set StoryUsers.Name = Name where StoryUsers.Id = Id;
        
        select * from StoryUsers where StoryUsers.Id = Id;
        
    else
        insert into StoryUsers (Name) values (Name);
        select * from StoryUsers where StoryUsers.Id = last_insert_id();
    end if;
end;
-- ---------------------------------------------------------------------------------------------------------------------
-- SPLIT
-- ---------------------------------------------------------------------------------------------------------------------
create procedure spInsertStoryUser(IN Name nvarchar(64))
begin
    insert into StoryUsers (Name) values (Name);
    select * from StoryUsers where StoryUsers.Id=last_insert_id();
end;
-- ---------------------------------------------------------------------------------------------------------------------
-- SPLIT
-- ---------------------------------------------------------------------------------------------------------------------
create procedure spDeleteStoryUserById(IN Id bigint)
BEGIN
    delete from StoryUsers where StoryUsers.Id=Id;
    select TRUE Success;
END;
-- ---------------------------------------------------------------------------------------------------------------------
-- SPLIT
-- ---------------------------------------------------------------------------------------------------------------------