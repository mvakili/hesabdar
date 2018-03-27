
import {ApiRequest, AxiosPromise} from './../modules/api-communication/';
import Base from './base'
export default  {

    constructor() { },
    addMaterial(name: string) : Promise<any>
    {
        return Base.post('material', 'AddMaterial', { Name: name });

    },
    getMaterials() : Promise<any>
    {
        return Base.post('material', 'GetMaterials');
    }
}