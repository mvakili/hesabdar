
import {ApiResult, GenericApiResult, ApiRequest, AxiosPromise} from './../modules/api-communication/';

export class Material {

    constructor() { }

    public static addMaterial(name: string) : AxiosPromise<GenericApiResult<any>>
    {
        let req = new ApiRequest();
        let result = req.post<GenericApiResult<any>>('material', 'AddMaterial');
        return result;
    }

    public static getMaterials() : AxiosPromise<GenericApiResult<any>>
    {
        let req = new ApiRequest();
        let result = req.post<GenericApiResult<any>>('material', 'GetMaterials');
        return result;
    }
}