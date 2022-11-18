


export class Trigger{

  constructor() {
  }

  private subscriber:()=>void=() => {};

  public subscribe(fire:()=>void){
    this.subscriber=fire;
  }

  public fire(){
    this.subscriber();
  }
}


export class TriggerOf<T>{

  constructor() {
  }

  private subscriber: (a:T) => void=(a:T) => {};


  public subscribe(fire:(a:T) => void){
    this.subscriber=fire;
  }

  public fire(arg:T){
    this.subscriber(arg);
  }
}
