import {Http} from '../modules/http-common';
import { error } from 'util';
export default abstract  class {
    public static async getResponse()
    {
        return Http.get('Values')
    }
}