import {TaskModel} from "./task-model";


export class StepModel {

  public projectId:number=0;
  public goalId:number=0;
  public tasks:TaskModel[]=[];
  public id:number=0;
  public title:string='';
  public description:string='';
}
