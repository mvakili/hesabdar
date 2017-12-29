
import {ApiResult, GenericApiResult, ApiRequest, AxiosPromise} from './../modules/api-communication/';

export class Account {

    constructor() { }

    public getPermissionLevel() : AxiosPromise<GenericApiResult<any>>
    {
        let req = new ApiRequest();
        let result = req.post<GenericApiResult<any>>('account', 'GetPermissionLevel');
        return result;
    }

    public getProfile(username: string) : AxiosPromise<GenericApiResult<any>>
    {
        let req = new ApiRequest();
        let result = req.post<GenericApiResult<any>>('account', 'GetProfile', {Username: username});
        return result;
    }

    public login(username: string, password) : AxiosPromise<GenericApiResult<any>>
    {
        let req = new ApiRequest();
        let result = req.post<GenericApiResult<any>>('account', 'Login', {Username: username, Password: password});
        return result;
    }

    public logout() : AxiosPromise<GenericApiResult<any>>
    {
        let req = new ApiRequest();
        let result = req.post<GenericApiResult<any>>('account', 'Login');
        return result;
    }
}