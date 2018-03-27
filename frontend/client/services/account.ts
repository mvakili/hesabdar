
import { ApiRequest, AxiosPromise} from './../modules/api-communication/';
import Base from './base'

export class Account {

    constructor() { }

    public getPermissionLevel() : Promise<any>
    {
        return Base.post('account', 'GetPermissionLevel');
    }

    public getProfile(username: string) : Promise<any>
    {
        return Base.post('account', 'GetProfile', {Username: username});
    }

    public login(username: string, password) : Promise<any>
    {
        return Base.post('account', 'Login', {Username: username, Password: password});
    }

    public logout() : Promise<any>
    {
        return Base.post('account', 'Logout');
    }
}