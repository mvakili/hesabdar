
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
        if (id) {
            return Base.get('payment', '' + id);
        } else {
            return new Promise<any> ( function(resolve, reject) { 
                reject();
            })
        }
    },
    getPaymentOfDeal(dealId: number) : Promise<any>
    {
        return Base.get('payment', 'Deal/' + dealId);
    },
    getPaymentsOfDealer(dealerId: number, page: number, perPage: number, sortField: string, sortOrder: string): Promise<any> {
        return Base.get('payment', '?id=' + dealerId + '&&page=' + page + '&&perPage=' + perPage + ((sortField) ? ('&&sort=' + sortField + ' ' + sortOrder) : ''))
    },
    getPaymentsOfMainDealer( page: number, perPage: number, sortField: string, sortOrder: string): Promise<any> {
        return Base.get('payment', '?page=' + page + '&&perPage=' + perPage + ((sortField) ? ('&&sort=' + sortField + ' ' + sortOrder) : ''))

    },
    getPriceOfDeal(dealId: number) : Promise<any>
    {
        return Base.get('payment', 'Deal/Price/' + dealId);
    },
    edit(id: number, param: any) : Promise<any>
    {
        return Base.put('payment', '' + id , param);
    }
}