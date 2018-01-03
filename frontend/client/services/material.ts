
import {ApiResult, GenericApiResult, ApiRequest, AxiosPromise} from './../modules/api-communication/';

export default {

    constructor() { },
    addMaterial(name: string) : AxiosPromise<GenericApiResult<any>>
    {
        let req = new ApiRequest();
        let result = req.post<GenericApiResult<any>>('material', 'AddMaterial', { Name: name });
        return result;
    },
    getMaterials() : AxiosPromise<GenericApiResult<any>>
    {
        let req = new ApiRequest();
        let result = req.post<GenericApiResult<any>>('material', 'GetMaterials');
        return result;
    }
}