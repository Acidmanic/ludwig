import {ImageSrcMap} from "../models/image-src-map";
import {ResultOf} from "../models/result-of";

export class SrcSize{
  public size:number=0;
  public src:string='';
}


export class MapIndex{


  items:SrcSize[]=new Array<SrcSize>();

  public add(size:number,src:string){
    this.items.push({size:size,src:src});
  }

  public static fromImageSrcMap(map:ImageSrcMap):MapIndex{

    let mapIndex:MapIndex=new MapIndex();

    console.log('getting index ImageMap for',map );

    if(map["16x16"]){
      mapIndex.add(16,map["16x16"]);
    }
    if(map["24x24"]){
      mapIndex.add(24,map["24x24"]);
    }
    if(map["32x32"]){
      mapIndex.add(32,map["32x32"]);
    }
    if(map["48x48"]){
      mapIndex.add(48,map["48x48"]);
    }
    return mapIndex;
  }

  getCloset(size:number):ResultOf<string>{
    let minDif = 10000000;
    let bestItem: { size: number, src: string } = {size: 0, src: ''};
    let foundAny: boolean = false;

    for (let item of this.items){

      let dif = Math.abs(item.size-size);

      if(dif<minDif){
        minDif=dif;
        bestItem=item;
        foundAny=true;
      }
    }
    if(foundAny){
      return {success:true,value:bestItem.src};
    }
    return {success:false,value:''};
  }

  getBySize(size:number):ResultOf<string>{
    for (let item of this.items){

      if(item.size===size){
        return {success:true,value:item.src};
      }
    }
    return {success:false,value:''};
  }

  getSrcAnyway(size:number):string{

    var result = this.getBySize(size);

    if(result.success){
      return result.value!;
    }
    result = this.getCloset(size);
    if(result.success){
      return result.value!;
    }
    return '';
  }
}
