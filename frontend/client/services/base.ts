
import {ApiRequest, AxiosPromise} from './../modules/api-communication/';


export default {

    constructor() { },
    
    thrown() : ApiRequest {
        return null;
    },
    post(controller: string, action: string, params = {}) : Promise<any> {
        return new Promise<any> ( function(resolve, reject) {
            let req = new ApiRequest();
            req.post<any>(controller, action, params).then( response => {
                resolve(response.data);
            }).catch(error => {
                reject(error);
            })
        });
    },
    get(controller: string, action: string, params = {}) : Promise<any> {
        return new Promise<any> ( function(resolve, reject) {
            let req = new ApiRequest();
            req.get<any>(controller, action, params).then( response => {
                resolve(response.data);
            }).catch(error => {
                reject(error);
            })
        });
    },
    put(controller: string, action: string,  params = {}) : Promise<any> {
        return new Promise<any> ( function(resolve, reject) {
            let req = new ApiRequest();
            req.put<any>(controller, action, params).then( response => {
                resolve(response.data);
            }).catch(error => {
                reject(error);
            })
        });
    },
    delete(controller: string, action: string, id: any) : Promise<any> {
        return new Promise<any> ( function(resolve, reject) {
            let req = new ApiRequest();
            req.delete<any>(controller, action, id).then( response => {
                resolve(response.data);
            }).catch(error => {
                reject(error);
            })
        });
    }


}