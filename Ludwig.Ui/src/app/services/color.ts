



export class Color{


  public red:number=0;
  public green:number=0;
  public blue:number=0;
  public alpha:number=0;
  public colorCode:string='rgba(0,0,0,1)';

  private dr:number=5;
  private dg:number=5;
  private db:number=5;
  private da:number=0.05;

  public incrementColor(){
    this.red+=this.dr;
    if(this.red>=256){
      this.red=255;
      this.dr=-this.dr;
    }else if(this.red<0){
      this.red=0;
      this.dr=-this.dr;
    }

    this.green+=this.dg;
    if(this.green>=256){
      this.green=255;
      this.dg=-this.dg;
    }else if(this.green<0){
      this.green=0;
      this.dg=-this.dg;
    }

    this.blue+=this.db;
    if(this.blue>=256){
      this.blue=255;
      this.db=-this.db;
    }else if(this.blue<0){
      this.blue=0;
      this.db=-this.db;
    }
    this.updateCode();
  }

  public incrementAlpha(){
    this.alpha+=this.da;
    if(this.alpha>=1){
      this.alpha=1;
      this.da=-this.da;
    }else if(this.alpha<0){
      this.alpha=0;
      this.da=-this.da;
    }
    this.updateCode();
  }

  private updateCode(){
    this.colorCode='rgba(' + this.red + ',' + this.green + ',' + this.blue + ',' + this.alpha + ')';
  }

  public static create(r:number,g:number,b:number,a:number):Color{

    let c = new Color();

    c.red=r;
    c.green=g;
    c.blue=b;
    c.alpha=a;
    c.updateCode();

    return c;
  }

}
