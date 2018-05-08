
import {ApiRequest, AxiosPromise} from './../modules/api-communication/';
import Base from './base'
export default  {

    constructor() { },
    add(param: any) : Promise<any>
    {
        return Base.post('dealer', '', param);
    },
    gets(page: number, perPage: number, sortField: string, sortOrder: string) : Promise<any>
    {
        return Base.get('dealer', '?page=' + page + '&&perPage=' + perPage + ((sortField) ? ('&&sort=' + sortField + ' ' + sortOrder) : ''));
    },
    get(id: number) : Promise<any>
    {
        return Base.get('dealer', '' + id);
    },
    edit(id: number, param: any) : Promise<any>
    {
        return Base.put('dealer', '' + id , param);
    },
    suggest(text: string) : Promise<any>
    {
        return Base.get('dealer', 'suggest/' + text);
    }
}