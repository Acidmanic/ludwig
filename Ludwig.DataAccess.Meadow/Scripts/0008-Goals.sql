-- ---------------------------------------------------------------------------------------------------------------------
-- SPLIT
-- ---------------------------------------------------------------------------------------------------------------------
create table Goals(
        Id bigint AUTO_INCREMENT PRIMARY KEY,
        Title nvarchar(64),
        Description nvarchar(256),
        ProjectId bigint
);
-- ---------------------------------------------------------------------------------------------------------------------
-- SPLIT
-- ---------------------------------------------------------------------------------------------------------------------
create view GoalsFullTree as 
    select
        Goals.Id 'Goals_Id',
        Goals.ProjectId 'Goals_ProjectId',
        Goals.Title 'Goals_Title',
        Goals.Description 'Goals_Description',
        Steps.Id 'Steps_Id',
        Steps.GoalId 'Steps_GoalId',
        Steps.ProjectId 'Steps_ProjectId',
        Steps.Title 'Steps_Title',
        Steps.Description 'Steps_Description',
        Tasks.Id 'Tasks_Id',
        Tasks.IterationId 'Tasks_IterationId',
        Tasks.GoalId 'Tasks_GoalId',
        Tasks.StepId 'Tasks_StepId',
        Tasks.ProjectId 'Tasks_ProjectId',
        Tasks.Title 'Tasks_Title',
        Tasks.Description 'Tasks_Description'
        from Goals 
        left join Steps on Steps.GoalId = Goals.Id
        left join Tasks on Tasks.StepId = Steps.Id;
-- ---------------------------------------------------------------------------------------------------------------------
-- SPLIT
-- ---------------------------------------------------------------------------------------------------------------------
create procedure spReadAllGoals()
begin
    select * from Goals; 
end;
-- ---------------------------------------------------------------------------------------------------------------------
-- SPLIT
-- ---------------------------------------------------------------------------------------------------------------------
create procedure spReadAllGoalsFullTree()
begin
    select * from GoalsFullTree;
end;
-- ---------------------------------------------------------------------------------------------------------------------
-- SPLIT
-- ---------------------------------------------------------------------------------------------------------------------
create procedure spReadGoalById(IN Id bigint)
begin
    select * from Goals where Goals.Id=Id;
end;
-- ---------------------------------------------------------------------------------------------------------------------
-- SPLIT
-- ---------------------------------------------------------------------------------------------------------------------
create procedure spReadGoalByIdFullTree(IN Id bigint)
begin
    select * from GoalsFullTree where GoalsFullTree.Goals_Id=Id;
end;
-- ---------------------------------------------------------------------------------------------------------------------
-- SPLIT
-- ---------------------------------------------------------------------------------------------------------------------
create procedure spInsertGoal(
    IN Title nvarchar(64),
    IN Description nvarchar(256),
    IN ProjectId bigint)
begin 
    insert into Goals (Title, Description,ProjectId) 
        values (Title, Description, ProjectId);
    select * from Goals where Goals.Id = last_insert_id();
end;
-- ---------------------------------------------------------------------------------------------------------------------
-- SPLIT
-- ---------------------------------------------------------------------------------------------------------------------
create procedure spUpdateGoal(
    IN Id bigint,
    IN Title nvarchar(64),
    IN Description nvarchar(256),
    IN ProjectId bigint)
begin
    update Goals set Title=Title,Description=Description,ProjectId=ProjectId
                 where Goals.Id=Id;                        
    select * from Goals where Goals.Id=Id;
end;
-- ---------------------------------------------------------------------------------------------------------------------
-- SPLIT
-- ---------------------------------------------------------------------------------------------------------------------
create procedure spSaveGoal(
    IN Id bigint,
    IN Title nvarchar(64),
    IN Description nvarchar(256),
    IN ProjectId bigint)
begin
    
    if exists(select 1 from Goals where Goals.Id=Id) then 
        call spUpdateGoal(Id,Title,Description,ProjectId);
    else
        call spInsertGoal(Title,Description,ProjectId);
    end if;
end;
-- ---------------------------------------------------------------------------------------------------------------------
-- SPLIT
-- ---------------------------------------------------------------------------------------------------------------------
create procedure spDeleteGoalById(Id bigint)
begin
    delete from Goals where Goals.Id=Id;
    select true Success;
end;
-- ---------------------------------------------------------------------------------------------------------------------
-- SPLIT
-- ---------------------------------------------------------------------------------------------------------------------
alter view ProjectsFullTree as
    select
        Projects.Name 'Projects_Name',
        Projects.Id 'Projects_Id',
        Projects.Description 'Projects_Description' ,
        Goals.Id 'Goals_Id',
        Goals.ProjectId 'Goals_ProjectId',
        Goals.Title 'Goals_Title',
        Goals.Description 'Goals_Description',
        Steps.Id 'Steps_Id',
        Steps.GoalId 'Steps_GoalId',
        Steps.ProjectId 'Steps_ProjectId',
        Steps.Title 'Steps_Title',
        Steps.Description 'Steps_Description',
        Tasks.Id 'Steps_Tasks_Id',
        Tasks.IterationId 'Steps_Tasks_IterationId',
        Tasks.GoalId 'Steps_Tasks_GoalId',
        Tasks.StepId 'Steps_Tasks_StepId',
        Tasks.ProjectId 'Steps_Tasks_ProjectId',
        Tasks.Title 'Steps_Tasks_Title',
        Tasks.Description 'Steps_Tasks_Description'
    from Projects
             left join Goals on Goals.ProjectId =  Projects.Id
             left join Steps on Steps.GoalId = Goals.Id
             join Tasks on Tasks.StepId = Steps.Id;

-- ---------------------------------------------------------------------------------------------------------------------
-- SPLIT
-- ---------------------------------------------------------------------------------------------------------------------
create view IterationsFullTree as 
    select Iterations.Id 'Iterations_Id',
           Iterations.ProjectId 'Iterations_ProjectId',
           Iterations.Name 'Name',
           Iterations.Description 'Iterations_Description',
           Tasks.Description 'Tasks_Description',
           Tasks.ProjectId 'Tasks_ProjectId',
           Tasks.Id 'Tasks_Id',
           Tasks.IterationId 'IterationId',
           Tasks.StepId 'StepId',
           Tasks.GoalId 'GoalId',
           Tasks.Title 'Title'
        from Iterations join Tasks on Tasks.IterationId = Iterations.Id;
-- ---------------------------------------------------------------------------------------------------------------------
-- SPLIT
-- ---------------------------------------------------------------------------------------------------------------------
create procedure spReadIterationByIdFullTree(IN Id bigint)
begin
    select * from IterationsFullTree where IterationsFullTree.Iterations_Id=Id;
end;
-- ---------------------------------------------------------------------------------------------------------------------
-- SPLIT
-- ---------------------------------------------------------------------------------------------------------------------
create procedure spReadIterationByProjectFullTree(IN Id bigint)
begin
    select * from IterationsFullTree where IterationsFullTree.Iterations_ProjectId=Id;
end;
-- ---------------------------------------------------------------------------------------------------------------------
-- SPLIT
-- ---------------------------------------------------------------------------------------------------------------------

