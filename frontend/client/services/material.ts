
import {ApiRequest, AxiosPromise} from './../modules/api-communication/';
import Base from './base'
export default  {

    constructor() { },
    addMaterial(name: string) : Promise<any>
    {
        return Base.post('material', 'AddMaterial', { Name: name });

    },
    getMaterials(page: number, perPage: number, sortField: string, sortOrder: string) : Promise<any>
    {
        return Base.get('material', '?page=' + page + '&&perPage=' + perPage + ((sortField) ? ('&&sort=' + sortField + ' ' + sortOrder) : ''));
    }
}