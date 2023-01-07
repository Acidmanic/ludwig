import {TaskModel} from "./task-model";
import {CardModel} from "./card-model";


export class StepModel extends CardModel{

  public projectId:number=0;
  public goalId:number=0;
  public tasks:TaskModel[]=[];
}
