-- ---------------------------------------------------------------------------------------------------------------------
-- SPLIT
-- ---------------------------------------------------------------------------------------------------------------------
create table Projects(
        Id bigint AUTO_INCREMENT PRIMARY KEY,
        Name nvarchar(64),
        Description nvarchar(256)
);
-- ---------------------------------------------------------------------------------------------------------------------
-- SPLIT
-- ---------------------------------------------------------------------------------------------------------------------
create view ProjectsFullTree as
    select
        Projects.Name 'Name',
        Projects.Id 'Projects_Id',
        Projects.Description 'Projects_Description'
    from Projects;
-- ---------------------------------------------------------------------------------------------------------------------
-- SPLIT
-- ---------------------------------------------------------------------------------------------------------------------
create procedure spReadAllProjects()
begin
    select * from Projects; 
end;
-- ---------------------------------------------------------------------------------------------------------------------
-- SPLIT
-- ---------------------------------------------------------------------------------------------------------------------
create procedure spReadAllProjectsFullTree()
begin
    select * from ProjectsFullTree;
end;
-- ---------------------------------------------------------------------------------------------------------------------
-- SPLIT
-- ---------------------------------------------------------------------------------------------------------------------
create procedure spReadProjectById(IN Id bigint)
begin
    select * from Projects where Projects.Id=Id;
end;
-- ---------------------------------------------------------------------------------------------------------------------
-- SPLIT
-- ---------------------------------------------------------------------------------------------------------------------
create procedure spReadProjectByIdFullTree(IN Id bigint)
begin
    select * from ProjectsFullTree where ProjectsFullTree.Projects_Id=Id;
end;
-- ---------------------------------------------------------------------------------------------------------------------
-- SPLIT
-- ---------------------------------------------------------------------------------------------------------------------
create procedure spInsertProject(
            IN Name nvarchar(64),
            IN Description nvarchar(256))
begin 
    insert into Projects (Name,Description) values (Name, Description);
    select * from Projects where Projects.Id = last_insert_id();
end;
-- ---------------------------------------------------------------------------------------------------------------------
-- SPLIT
-- ---------------------------------------------------------------------------------------------------------------------
create procedure spUpdateProject(
    IN Id bigint,
    IN Name nvarchar(64),
    IN Description nvarchar(256))
begin
    update Projects set Name=Name,Description=Description where Projects.Id=Id;                        
    select * from Projects where Projects.Id=Id;
end;
-- ---------------------------------------------------------------------------------------------------------------------
-- SPLIT
-- ---------------------------------------------------------------------------------------------------------------------
create procedure spSaveProject(
    IN Id bigint,
    IN Name nvarchar(64),
    IN Description nvarchar(256))
begin
    
    if exists(select 1 from Projects where Projects.Id=Id) then 
        call spUpdateProject(Id,Name,Description);
    else
        call spInsertProject(Name,Description);
    end if;
end;
-- ---------------------------------------------------------------------------------------------------------------------
-- SPLIT
-- ---------------------------------------------------------------------------------------------------------------------
create procedure spDeleteProjectById(Id bigint)
begin
    delete from Projects where Projects.Id=Id;
    select true Success;
end;
-- ---------------------------------------------------------------------------------------------------------------------
-- SPLIT
-- ---------------------------------------------------------------------------------------------------------------------
