import {StepModel} from "./step-model";
import {CardModel} from "./card-model";


export class GoalModel extends CardModel{
  public projectId:number=0;
  public steps:StepModel[]=[];

}
