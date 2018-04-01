
import {ApiRequest, AxiosPromise} from './../modules/api-communication/';
import Base from './base'
export default  {

    constructor() { },
    addDealer(name: string) : Promise<any>
    {
        return Base.post('dealer', '', { Name: name });

    },
    getDealers(page: number, perPage: number, sortField: string, sortOrder: string) : Promise<any>
    {
        return Base.get('dealer', '?page=' + page + '&&perPage=' + perPage + ((sortField) ? ('&&sort=' + sortField + ' ' + sortOrder) : ''));
    }
}