
import {ApiRequest, AxiosPromise} from './../modules/api-communication/';
import Base from './base'
export default  {

    constructor() { },
    add(param: any) : Promise<any>
    {
        return Base.post('deal', '', param);
    },
    addNewSale(param: any) : Promise<any>
    {
        return Base.post('deal', 'sale', param)
    },
    addNewPurchase(param: any) : Promise<any>
    {
        return Base.post('deal', 'purchase', param)
    },
    gets(page: number, perPage: number, sortField: string, sortOrder: string) : Promise<any>
    {
        return Base.get('deal', '?page=' + page + '&&perPage=' + perPage + ((sortField) ? ('&&sort=' + sortField + ' ' + sortOrder) : ''));
    },
    getPurchases(page: number, perPage: number, sortField: string, sortOrder: string) : Promise<any>
    {
        return Base.get('deal', 'dealer/purchases?page=' + page + '&&perPage=' + perPage + ((sortField) ? ('&&sort=' + sortField + ' ' + sortOrder) : ''));
    },
    getSales(page: number, perPage: number, sortField: string, sortOrder: string) : Promise<any>
    {
        return Base.get('deal', 'dealer/sales?page=' + page + '&&perPage=' + perPage + ((sortField) ? ('&&sort=' + sortField + ' ' + sortOrder) : ''));
    },
    get(id: number) : Promise<any>
    {
        return Base.get('deal', '' + id);
    },
    edit(id: number, param: any) : Promise<any>
    {
        return Base.put('deal', '' + id , param);
    },
    getDealsOfDealer(id: number, page: number, perPage: number, sortField: string, sortOrder: string) : Promise<any> 
    {
        return Base.get('deal', 'dealer/' + id + '?page=' + page + '&&perPage=' + perPage + ((sortField) ? ('&&sort=' + sortField + ' ' + sortOrder) : ''));
    }
}