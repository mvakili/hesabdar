
import {ApiResult, GenericApiResult, ApiRequest, AxiosPromise} from './../modules/api-communication/';

export class Account {

    constructor() { }

    public emailValidate(email: string) : AxiosPromise<ApiResult>
    {
        let req = new ApiRequest();
        let result = req.post<ApiResult>('account', 'emailValidate', email);
        return result;
    }

    public getPermissionLevel() : AxiosPromise<GenericApiResult<any>>
    {
        let req = new ApiRequest();
        let result = req.post<GenericApiResult<any>>('account', 'GetPermissionLevel');
        return result;
    }
}