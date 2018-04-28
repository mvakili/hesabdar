
import {ApiRequest, AxiosPromise} from './../modules/api-communication/';
import Base from './base'
export default  {

    constructor() { },
    add(param: any) : Promise<any>
    {
        return Base.post('material', '', param);
    },
    gets(page: number, perPage: number, sortField: string, sortOrder: string) : Promise<any>
    {
        return Base.get('material', '?page=' + page + '&&perPage=' + perPage + ((sortField) ? ('&&sort=' + sortField + ' ' + sortOrder) : ''));
    },
    get(id: number) : Promise<any>
    {
        return Base.get('material', '' + id);
    },
    edit(id: number, param: any) : Promise<any>
    {
        return Base.put('material', '' + id , param);
    },
    suggest(text: string) : Promise<any>
    {
        return Base.get('material', 'suggest/' + text);
    }
}