
import {ApiRequest, AxiosPromise} from './../modules/api-communication/';


export default {

    constructor() { },
    
    thrown() : ApiRequest {
        return null;
    },
    post(controller: string, action: string, params = {}) : Promise<any> {
        const vue = require('vue');
        console.log(vue);
        return new Promise<any> ( function(resolve, reject) {
            let req = new ApiRequest();
            req.post<any>(controller, action, params).then( response => {
                resolve(response.data);
            }).catch(error => {
                reject(error);
            })
        });
    }
}