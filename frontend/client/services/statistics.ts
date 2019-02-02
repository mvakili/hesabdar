
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
    getPurchaseAndSalePrice( start: Date, end: Date) : Promise<any> {
        return Base.get('statistics', 'purchaseAndSale/price/overDate'+ "?start=" +start.toString() + "&end=" + end.toString());
    },
    getMonthlyPurchaseAndSalePrice() : Promise<any> {
        return Base.get('statistics', 'purchaseAndSale/price/monthly');
    },
    getMaterialAmountOverTime(materialId: number, dealerId: number, start: Date, end: Date) : Promise<any> {
        return Base.get('statistics', 'material/overtime/'+ materialId + '/' + dealerId + "?start=" +start.toString() + "&end=" + end.toString());
    },
    getMaterialAmountOverTimeforMainDealer(materialId: number, start: Date, end: Date) : Promise<any> {
        return Base.get('statistics', 'material/overtime/'+ materialId + "?start=" +start.toString() + "&end=" + end.toString());
    }
}