import {GoalModel} from "./goal-model";
import {IterationModel} from "./iteration-model";


export class ProjectModel{

  public name:string='';
  public description:string='';
  public id:number=0;
  public goals:GoalModel[]=[];
  public iterations:IterationModel[]=[];
}
