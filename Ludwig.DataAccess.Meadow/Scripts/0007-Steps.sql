-- ---------------------------------------------------------------------------------------------------------------------
-- SPLIT
-- ---------------------------------------------------------------------------------------------------------------------
create table Steps(
        Id bigint AUTO_INCREMENT PRIMARY KEY,
        Title nvarchar(64),
        Description nvarchar(256),
        ProjectId bigint,
        GoalId bigint
);
-- ---------------------------------------------------------------------------------------------------------------------
-- SPLIT
-- ---------------------------------------------------------------------------------------------------------------------
create view StepsFullTree as
    select
        Steps.Id 'Steps_Id',
        Steps.GoalId 'Steps_GoalId',
        Steps.ProjectId 'Steps_ProjectId',
        Steps.Title 'Steps_Title',
        Steps.Description 'Steps_Description',
        Tasks.Id 'Tasks_Id',
        Tasks.IterationId 'IterationId',
        Tasks.GoalId 'Tasks_GoalId',
        Tasks.StepId 'StepId',
        Tasks.ProjectId 'Tasks_ProjectId',
        Tasks.Title 'Tasks_Title',
        Tasks.Description 'Tasks_Description'
        from Steps 
    left join Tasks on Tasks.StepId =  Steps.Id;  
-- ---------------------------------------------------------------------------------------------------------------------
-- SPLIT
-- ---------------------------------------------------------------------------------------------------------------------
create procedure spReadAllSteps()
begin
    select * from Steps; 
end;
-- ---------------------------------------------------------------------------------------------------------------------
-- SPLIT
-- ---------------------------------------------------------------------------------------------------------------------
create procedure spReadAllStepsFullTree()
begin
    select * from StepsFullTree;
end;
-- ---------------------------------------------------------------------------------------------------------------------
-- SPLIT
-- ---------------------------------------------------------------------------------------------------------------------
create procedure spReadStepById(IN Id bigint)
begin
    select * from Steps where Steps.Id=Id;
end;
-- ---------------------------------------------------------------------------------------------------------------------
-- SPLIT
-- ---------------------------------------------------------------------------------------------------------------------
create procedure spReadStepByIdFullTree(IN Id bigint)
begin
    select * from StepsFullTree where StepsFullTree.Steps_Id=Id;
end;
-- ---------------------------------------------------------------------------------------------------------------------
-- SPLIT
-- ---------------------------------------------------------------------------------------------------------------------
create procedure spInsertStep(
    IN Title nvarchar(64),
    IN Description nvarchar(256),
    IN ProjectId bigint,
    IN GoalId bigint)
begin 
    insert into Steps (Title, Description,  ProjectId, GoalId) 
        values (Title, Description, ProjectId, GoalId);
    select * from Steps where Steps.Id = last_insert_id();
end;
-- ---------------------------------------------------------------------------------------------------------------------
-- SPLIT
-- ---------------------------------------------------------------------------------------------------------------------
create procedure spUpdateStep(
    IN Id bigint,
    IN Title nvarchar(64),
    IN Description nvarchar(256),
    IN ProjectId bigint,
    IN GoalId bigint)
begin
    update Steps set Title=Title,Description=Description, 
                     ProjectId=ProjectId,GoalId=GoalId
                 where Steps.Id=Id;                        
    select * from Steps where Steps.Id=Id;
end;
-- ---------------------------------------------------------------------------------------------------------------------
-- SPLIT
-- ---------------------------------------------------------------------------------------------------------------------
create procedure spSaveStep(
    IN Id bigint,
    IN Title nvarchar(64),
    IN Description nvarchar(256),
    IN ProjectId bigint,
    IN GoalId bigint)
begin
    
    if exists(select 1 from Steps where Steps.Id=Id) then 
        call spUpdateStep(Id,Title,Description,ProjectId,GoalId);
    else
        call spInsertStep(Title,Description,ProjectId,GoalId);
    end if;
end;
-- ---------------------------------------------------------------------------------------------------------------------
-- SPLIT
-- ---------------------------------------------------------------------------------------------------------------------
create procedure spDeleteStepById(Id bigint)
begin
    delete from Steps where Steps.Id=Id;
    select true Success;
end;
-- ---------------------------------------------------------------------------------------------------------------------
-- SPLIT
-- ---------------------------------------------------------------------------------------------------------------------