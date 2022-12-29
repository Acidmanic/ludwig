-- ---------------------------------------------------------------------------------------------------------------------
-- SPLIT
-- ---------------------------------------------------------------------------------------------------------------------
create table UserStories(
        Id bigint AUTO_INCREMENT PRIMARY KEY,
        Title nvarchar(128),
        StoryUserId bigint,
        StoryFeature nvarchar(128),
        StoryBenefit nvarchar(128),
        CardColor varchar(16),
        IsDone bool,
        PriorityValue int,
        PriorityName nvarchar(16)
);
-- ---------------------------------------------------------------------------------------------------------------------
-- SPLIT
-- ---------------------------------------------------------------------------------------------------------------------
create view UserStoriesFullTree as 
    select
        UserStories.StoryUserId 'StoryUserId', 
        UserStories.Id 'UserStories_Id', 
        UserStories.CardColor 'CardColor', 
        UserStories.IsDone 'IsDone', 
        UserStories.PriorityName 'PriorityName', 
        UserStories.StoryBenefit 'StoryBenefit', 
        UserStories.StoryFeature 'StoryFeature', 
        UserStories.PriorityValue 'PriorityValue', 
        UserStories.Title 'Title', 
        StoryUsers.Name 'Name', 
        StoryUsers.Id 'StoryUsers_Id'  
        from UserStories 
        left join StoryUsers on UserStories.StoryUserId=StoryUsers.Id;
-- ---------------------------------------------------------------------------------------------------------------------
-- SPLIT
-- ---------------------------------------------------------------------------------------------------------------------
create procedure spReadAllUserStories()
begin
    select * from UserStories; 
end;
-- ---------------------------------------------------------------------------------------------------------------------
-- SPLIT
-- ---------------------------------------------------------------------------------------------------------------------
create procedure spReadAllUserStoriesFullTree()
begin
    select * from UserStoriesFullTree;
end;
-- ---------------------------------------------------------------------------------------------------------------------
-- SPLIT
-- ---------------------------------------------------------------------------------------------------------------------
create procedure spReadUserStoryDalById(IN Id bigint)
begin
    select * from UserStories where UserStories.Id=Id;
end;
-- ---------------------------------------------------------------------------------------------------------------------
-- SPLIT
-- ---------------------------------------------------------------------------------------------------------------------
create procedure spReadUserStoryByIdFullTree(IN Id bigint)
begin
    select * from UserStoriesFullTree where UserStoriesFullTree.UserStories_Id=Id;
end;
-- ---------------------------------------------------------------------------------------------------------------------
-- SPLIT
-- ---------------------------------------------------------------------------------------------------------------------
create procedure spInsertUserStoryDal(
            IN Title nvarchar(128),
            IN StoryUserId bigint,
            IN StoryFeature nvarchar(128),
            IN StoryBenefit nvarchar(128),
            IN CardColor varchar(16),
            IN IsDone bool,
            IN PriorityValue int,
            IN PriorityName nvarchar(16))
begin 
    insert into UserStories (Title, StoryUserId, StoryFeature, StoryBenefit, CardColor, IsDone, PriorityValue, PriorityName) 
        values (Title, StoryUserId, StoryFeature, StoryBenefit, CardColor, IsDone, PriorityValue, PriorityName);
    
    select * from UserStories where UserStories.Id = last_insert_id();
end;
-- ---------------------------------------------------------------------------------------------------------------------
-- SPLIT
-- ---------------------------------------------------------------------------------------------------------------------
create procedure spUpdateUserStoryDal(
    IN Id bigint,
    IN Title nvarchar(128),
    IN StoryUserId bigint,
    IN StoryFeature nvarchar(128),
    IN StoryBenefit nvarchar(128),
    IN CardColor varchar(16),
    IN IsDone bool,
    IN PriorityValue int,
    IN PriorityName nvarchar(16))
begin
    update UserStories set Title=Title,StoryUserId=StoryUserId,StoryFeature=StoryFeature,
        StoryBenefit=StoryBenefit,CardColor=CardColor,IsDone=false,PriorityValue=PriorityValue,PriorityName=PriorityName;                        
    select * from UserStories where UserStories.Id=Id;
end;
-- ---------------------------------------------------------------------------------------------------------------------
-- SPLIT
-- ---------------------------------------------------------------------------------------------------------------------
create procedure spSaveUserStoryDal(
    IN Id bigint,
    IN Title nvarchar(128),
    IN StoryUserId bigint,
    IN StoryFeature nvarchar(128),
    IN StoryBenefit nvarchar(128),
    IN CardColor varchar(16),
    IN IsDone bool,
    IN PriorityValue int,
    IN PriorityName nvarchar(16))
begin
    
    if exists(select 1 from UserStories where UserStories.Id=Id) then 
        call spUpdateUserStoryDal(Id,Title,StoryUserId,StoryFeature,
            StoryBenefit,CardColor,IsDone,PriorityValue,PriorityName);
    else
        call spInsertUserStoryDal(Title,StoryUserId,StoryFeature,
                                  StoryBenefit,CardColor,IsDone,PriorityValue,PriorityName);
    end if;
end;
-- ---------------------------------------------------------------------------------------------------------------------
-- SPLIT
-- ---------------------------------------------------------------------------------------------------------------------
create procedure spDeleteUserStoryDalById(Id bigint)
begin
    delete from UserStories where UserStories.Id=Id;
    select true Success;
end;
-- ---------------------------------------------------------------------------------------------------------------------
-- SPLIT
-- ---------------------------------------------------------------------------------------------------------------------