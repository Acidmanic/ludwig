import {StepModel} from "./step-model";


export class GoalModel{
  public projectId:number=0;
  public steps:StepModel[]=[];
  public id: number=0;
  public title:string='';
  public description:string='';
}
