-- ---------------------------------------------------------------------------------------------------------------------
-- SPLIT
-- ---------------------------------------------------------------------------------------------------------------------
create table Iterations
(
        Id bigint AUTO_INCREMENT PRIMARY KEY,
        Name nvarchar(64),
        Description nvarchar(256),
        ProjectId bigint
);
-- ---------------------------------------------------------------------------------------------------------------------
-- SPLIT
-- ---------------------------------------------------------------------------------------------------------------------
create procedure spReadAllIterations()
begin
    select * from Iterations; 
end;
-- ---------------------------------------------------------------------------------------------------------------------
-- SPLIT
-- ---------------------------------------------------------------------------------------------------------------------
create procedure spReadIterationById(IN Id bigint)
begin
    select * from Iterations where Iterations.Id=Id;
end;
-- ---------------------------------------------------------------------------------------------------------------------
-- SPLIT
-- ---------------------------------------------------------------------------------------------------------------------
create procedure spReadIterationsByProjectId(IN Id bigint)
begin
    select * from Iterations where Iterations.ProjectId=Id;
end;
-- ---------------------------------------------------------------------------------------------------------------------
-- SPLIT
-- ---------------------------------------------------------------------------------------------------------------------
create procedure spInsertIteration(
    IN Name nvarchar(64),
    IN Description nvarchar(256),
    IN ProjectId bigint)
begin 
    insert into Iterations (Name,Description,ProjectId) values (Name, Description,ProjectId);
    select * from Iterations where Iterations.Id = last_insert_id();
end;
-- ---------------------------------------------------------------------------------------------------------------------
-- SPLIT
-- ---------------------------------------------------------------------------------------------------------------------
create procedure spUpdateIteration(
    IN Id bigint, 
    IN Name nvarchar(64),
    IN Description nvarchar(256),
    IN ProjectId bigint)
begin
    update Iterations set Name=Name,Description=Description,ProjectId=ProjectId where Iterations.Id=Id;                        
    select * from Iterations where Iterations.Id=Id;
end;
-- ---------------------------------------------------------------------------------------------------------------------
-- SPLIT
-- ---------------------------------------------------------------------------------------------------------------------
create procedure spSaveIteration(
    IN Id bigint,
    IN Name nvarchar(64),
    IN Description nvarchar(256),
    IN ProjectId bigint)
begin
    
    if exists(select 1 from Iterations where Iterations.Id=Id) then 
        call spUpdateIteration(Id,Name,Description,ProjectId);
    else
        call spInsertIteration(Name,Description,ProjectId);
    end if;
end;
-- ---------------------------------------------------------------------------------------------------------------------
-- SPLIT
-- ---------------------------------------------------------------------------------------------------------------------
create procedure spDeleteIterationById(Id bigint)
begin
    delete from Iterations where Iterations.Id=Id;
    select true Success;
end;
-- ---------------------------------------------------------------------------------------------------------------------
-- SPLIT
-- ---------------------------------------------------------------------------------------------------------------------