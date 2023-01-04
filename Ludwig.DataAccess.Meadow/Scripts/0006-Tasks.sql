-- ---------------------------------------------------------------------------------------------------------------------
-- SPLIT
-- ---------------------------------------------------------------------------------------------------------------------
create table Tasks(
        Id bigint AUTO_INCREMENT PRIMARY KEY,
        Title nvarchar(64),
        Description nvarchar(256),
        StepId bigint,
        ProjectId bigint,
        GoalId bigint,
        IterationId bigint
);
-- ---------------------------------------------------------------------------------------------------------------------
-- SPLIT
-- ---------------------------------------------------------------------------------------------------------------------
create procedure spReadAllTasks()
begin
    select * from Tasks; 
end;
-- ---------------------------------------------------------------------------------------------------------------------
-- SPLIT
-- ---------------------------------------------------------------------------------------------------------------------
create procedure spReadTaskById(IN Id bigint)
begin
    select * from Tasks where Tasks.Id=Id;
end;
-- ---------------------------------------------------------------------------------------------------------------------
-- SPLIT
-- ---------------------------------------------------------------------------------------------------------------------
create procedure spReadTaskByStepId(IN Id bigint)
begin
    select * from Tasks where Tasks.StepId=Id;
end;
-- ---------------------------------------------------------------------------------------------------------------------
-- SPLIT
-- ---------------------------------------------------------------------------------------------------------------------
create procedure spReadTaskByGoalId(IN Id bigint)
begin
    select * from Tasks where Tasks.GoalId=Id;
end;
-- ---------------------------------------------------------------------------------------------------------------------
-- SPLIT
-- ---------------------------------------------------------------------------------------------------------------------
create procedure spReadTaskByIterationId(IN Id bigint)
begin
    select * from Tasks where Tasks.IterationId=Id;
end;
-- ---------------------------------------------------------------------------------------------------------------------
-- SPLIT
-- ---------------------------------------------------------------------------------------------------------------------
create procedure spInsertTask(
    IN Title nvarchar(64),
    IN Description nvarchar(256),
    IN StepId bigint,
    IN ProjectId bigint,
    IN GoalId bigint,
    IN IterationId bigint)
begin 
    insert into Tasks (Title, Description, StepId, ProjectId, GoalId, IterationId) 
        values (Title, Description, StepId, ProjectId, GoalId, IterationId);
    select * from Tasks where Tasks.Id = last_insert_id();
end;
-- ---------------------------------------------------------------------------------------------------------------------
-- SPLIT
-- ---------------------------------------------------------------------------------------------------------------------
create procedure spUpdateTask(
    IN Id bigint,
    IN Title nvarchar(64),
    IN Description nvarchar(256),
    IN StepId bigint,
    IN ProjectId bigint,
    IN GoalId bigint,
    IN IterationId bigint)
begin
    update Tasks set Title=Title,Description=Description,StepId=StepId,
                     ProjectId=ProjectId,GoalId=GoalId,IterationId=IterationId
                 where Tasks.Id=Id;                        
    select * from Tasks where Tasks.Id=Id;
end;
-- ---------------------------------------------------------------------------------------------------------------------
-- SPLIT
-- ---------------------------------------------------------------------------------------------------------------------
create procedure spSaveTask(
    IN Id bigint,
    IN Title nvarchar(64),
    IN Description nvarchar(256),
    IN StepId bigint,
    IN ProjectId bigint,
    IN GoalId bigint,
    IN IterationId bigint)
begin
    
    if exists(select 1 from Tasks where Tasks.Id=Id) then 
        call spUpdateTask(Id,Title,Description,
            StepId,ProjectId,GoalId,IterationId);
    else
        call spInsertTask(Title,Description,
                          StepId,ProjectId,GoalId,IterationId);
    end if;
end;
-- ---------------------------------------------------------------------------------------------------------------------
-- SPLIT
-- ---------------------------------------------------------------------------------------------------------------------
create procedure spDeleteTaskById(Id bigint)
begin
    delete from Tasks where Tasks.Id=Id;
    select true Success;
end;
-- ---------------------------------------------------------------------------------------------------------------------
-- SPLIT
-- ---------------------------------------------------------------------------------------------------------------------