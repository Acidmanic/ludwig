import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class StructuredLocalStorageService {

  constructor() { }

  public acquireData<T>(key: string): T {
    const json = localStorage.getItem(key);

    const data: T = json ? JSON.parse(json) : null;

    return data;
  }

  public storeData<T>(key: string, data: T) {
    localStorage.setItem(key, JSON.stringify(data));
  }

  public removeData(key: string) {
    localStorage.removeItem(key);
  }


}

