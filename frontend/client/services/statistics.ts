
import {ApiRequest, AxiosPromise} from './../modules/api-communication/';
import Base from './base'
export default  {

    constructor() { },
    getGeneral() : Promise<any> {
        return Base.get('statistics', 'general');
    },
    getTwoWeeksSalesPrice() : Promise<any> {
        return Base.get('statistics', 'sales/price/twoweeks');
    },
    getTwoWeeksPurchasesPrice() : Promise<any> {
        return Base.get('statistics', 'purchases/price/twoweeks');
    },
    getWeeklyPurchaseAndSalePrice() : Promise<any> {
        return Base.get('statistics', 'purchaseAndSale/price/weekly');
    },
    getMonthlyPurchaseAndSalePrice() : Promise<any> {
        return Base.get('statistics', 'purchaseAndSale/price/monthly');
    }
}