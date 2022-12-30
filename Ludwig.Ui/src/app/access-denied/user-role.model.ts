

export class UserRoleModel{

  public static readonly nullRole= UserRoleModel.CreateUserRoleModel('','',0);
  public static readonly administrator=
    UserRoleModel.CreateUserRoleModel('administrator','Administrator',1);
  public static readonly issueManager=
    UserRoleModel.CreateUserRoleModel('issuemanager','Issue Manager',2);


  roleName:string='administrator';
  roleDisplayName:string='Administrator';
  id:number=1;


  public static CreateUserRoleModel(name:string,display:string,id:number){
    let role = new UserRoleModel();
    role.roleName=name;
    role.roleDisplayName=display;
    role.id=id;
    return role;
  }

  public equals(role:UserRoleModel):boolean{
    if(this.id==role.id){
      return true;
    }
    if(this.roleName.toLowerCase()==role.roleName.toLowerCase()){
      return true;
    }
    return false;
  }

  public sameRole(roleName:string):boolean{

    if(this.roleName.toLowerCase()==roleName.toLowerCase()){
      return true;
    }
    return false;
  }
}
