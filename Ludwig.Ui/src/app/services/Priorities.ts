import {PriorityModel} from "../models/priority-model";


export class Priorities{

  public static highest:PriorityModel={name:'Highest',value:0};
  public static high:PriorityModel={name:'High',value:1};
  public static medium:PriorityModel={name:'Medium',value:2};
  public static low:PriorityModel={name:'Low',value:3};
  public static lowest:PriorityModel={name:'Lowest',value:4};

  public static all:PriorityModel[]=[Priorities.highest,Priorities.high,Priorities.medium,Priorities.low,Priorities.lowest];
}
