import { Component, OnInit } from '@angular/core';
import {ActivatedRoute} from "@angular/router";
import {read} from "@popperjs/core";
import {ProjectModel} from "../../models/project-model";
import {ProjectsService} from "../../services/projects/projects.service";
import {WaiterService} from "../../services/waiter.service";
import {GoalModel} from "../../models/goal-model";
import {StepModel} from "../../models/step-model";
import {TaskModel} from "../../models/task-model";
import {CardModel} from "../../models/card-model";

@Component({
  selector: 'project-story-map',
  templateUrl: './project-story-map.component.html',
  styleUrls: ['./project-story-map.component.css']
})
export class ProjectStoryMapComponent implements OnInit {

  public projectId:number=0;
  public validProject:boolean=false;
  public project:ProjectModel=new ProjectModel();


  selectedCard:CardModel=new CardModel();
  anyCardSelected:boolean=false;
  selectedCardParents:CardModel[]=[];
  selectedCardChildName:string|null=null;
  selectedAddChild:(c:CardModel)=>void=(c:CardModel)=>{};
  selectedDelete:(c:CardModel)=>void=(c:CardModel)=>{};
  selectedParentChange:(c:CardModel)=>void=(c:CardModel)=>{};


  private generatedTaskId:number=-1;
  private generatedStepId:number=-1;
  private generatedGoalId:number=-1;

  constructor(private svcActivate:ActivatedRoute,
              private svcProjects:ProjectsService,
              private svcWait:WaiterService) { }



  ngOnInit(): void {

    var readId = this.svcActivate.snapshot.paramMap.get('id');

    if(readId){
      this.projectId=+readId;

      this.svcWait.start();

      this.svcProjects.getById(this.projectId).subscribe({
        next: value => {
          this.project = value;
          this.validProject=true;
        },
        error: err => {
          this.svcWait.stop();
        },
        complete: () =>{
          this.svcWait.stop();
        }
      });

    }


  }




  addGoalClicked(){
    let goal = new GoalModel();
    goal.id=this.generatedGoalId;
    this.generatedGoalId--;
    goal.projectId=this.project.id;
    goal.description="A Description About the goal";
    goal.title="Nice Feature";

    this.project.goals.push(goal);
  }


  selectGoal(goal:GoalModel){
    this.selectedCardParents=[];
    this.selectedCardChildName='Step';
    this.selectedAddChild=c=>{
      let step = new StepModel();
      step.id=this.generatedStepId;
      this.generatedStepId--;
      step.goalId=goal.id;
      step.projectId=goal.projectId;
      step.title="New Step";
      step.tasks=[];
      step.description="Description about Step.";
      goal.steps.push(step);
    };
    this.selectedParentChange=c=>{};
    this.selectCard(goal);
  }

  selectStep(step:StepModel){
    this.selectedCardParents=this.project.goals;
    this.selectedCardChildName='Task';
    this.selectedAddChild=c=>{
      let task = new TaskModel();
      task.id=this.generatedTaskId;
      this.generatedTaskId++;
      task.stepId=step.id;
      task.goalId=step.goalId;
      task.projectId=step.projectId;
      task.title="New Task";
      task.description="Description about Task.";
      step.tasks.push(task);
    };
    this.selectedParentChange=(c:CardModel)=>{
      let goal = c as GoalModel;
      if(goal){
        this.moveStepToGoal(step,goal);
      }
    };
    this.selectCard(step);
  }

  selectTask(task:TaskModel){

    this.selectedCardParents = this.getAllSteps();
    this.selectedCardChildName=null;
    this.selectedAddChild=c=>{};
    this.selectedParentChange=(c:CardModel)=>{
      let step = c as StepModel;
      if(step){
        this.moveTaskToStep(task,step);
      }
    };
    this.selectCard(task);

  }

  selectCard(card:CardModel){
    this.selectedCard=card;
    this.anyCardSelected=true;
  }

  popoverUnselect(){
    // this.selectedCardChildName=null;
    // this.selectedCardParents=[];
    // this.selectedDelete=c=>{};
    // this.anyCardSelected=false;
    // this.selectedAddChild=c=>{};
  }


  private findGoalById(id:number):GoalModel | null{
    for(let g of this.project.goals){
      if(g.id==id){
        return g;
      }
    }
    return null;
  }


  private getAllSteps():StepModel[]{

    let steps:StepModel[]=[];

    for (let goal of this.project.goals){
      for(let s of goal.steps){
        steps.push(s);
      }
    }

    return steps;
  }

  private getParentStepsForTask(task:TaskModel):StepModel[]{

    let goal = this.findGoalById(task.goalId);

    if(task.goalId==0 || !goal){

      let steps:StepModel[]=this.getAllSteps();

      return steps;
    }

    return goal.steps;
  }

  private indexOf(cards:CardModel[],card:CardModel):number{


    for(let i=0;i<cards.length;i++){
      if(cards[i].id==card.id){
        return i;
      }
    }

    return -1;
  }

  private remove(cards:CardModel[],card:CardModel):boolean{

    let index = this.indexOf(cards,card);

    if(index>-1){
      cards.splice(index,1);

      return true;
    }
    return false;
  }

  private moveStepToGoal(step:StepModel,goal:GoalModel){
    for(let g of this.project.goals){
      if(g.id==step.goalId){
        this.remove(g.steps,step);
        break;
      }
    }
    step.goalId= goal.id;
    goal.steps.push(step);
  }

  private moveTaskToStep(task:TaskModel,step:StepModel){
    var steps = this.getAllSteps();

    for(let step of steps){
      if(step.id==task.stepId){
        this.remove(step.tasks,task);
        break;
      }
    }
    task.stepId=step.id;
    task.goalId=step.goalId;
    task.projectId=step.projectId;
    step.tasks.push(task);
  }
}
