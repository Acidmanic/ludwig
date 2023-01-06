import {TaskModel} from "./task-model";


export class IterationModel{

  public id:number=0;
  public name:string='';
  public description:string='';
  public projectId:number=0;
  public tasks:TaskModel[]=[];
}
