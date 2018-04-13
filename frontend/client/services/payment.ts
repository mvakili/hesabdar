
import {ApiRequest, AxiosPromise} from './../modules/api-communication/';
import Base from './base'
export default  {

    constructor() { },
    add(param: any) : Promise<any>
    {
        return Base.post('payment', '', param);
    },
    gets(page: number, perPage: number, sortField: string, sortOrder: string) : Promise<any>
    {
        return Base.get('payment', '?page=' + page + '&&perPage=' + perPage + ((sortField) ? ('&&sort=' + sortField + ' ' + sortOrder) : ''));
    },
    get(id: number) : Promise<any>
    {
        return Base.get('payment', '' + id);
    },
    getPaymentOfDeal(dealId: number) : Promise<any>
    {
        return Base.get('payment', 'Deal/' + dealId);
    },
    edit(id: number, param: any) : Promise<any>
    {
        return Base.put('payment', '' + id , param);
    }
}