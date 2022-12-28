
create table StoryUsers(
     Id bigint AUTO_INCREMENT PRIMARY KEY,
     Name nvarchar(64)
);

--SPLIT

create procedure spReadAllStoryUsers(IN Id bigint)
begin
    select * from StoryUsers where StoryUsers.Id = Id;
end;

--SPLIT 

create procedure spReadStoryUserById(IN Id bigint)
begin
    select * from StoryUsers where StoryUsers.Id = Id;
end;

--SPLIT 

create procedure spInsertStoryUser(IN Name nvarchar(64))
begin
    insert into StoryUsers (Name) values (@Name);
    select * from StoryUsers where StoryUsers.Id=last_insert_id();
end;

--SPLIT 

create procedure spDeleteStoryUserById(IN Id bigint)
BEGIN
    delete from StoryUsers where StoryUsers.Id=Id;
    select TRUE success;
END;

--SPLIT 

    

--SPLIT 
