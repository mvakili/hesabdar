
import { ResultStatus } from './result-status';

export interface ApiResult {
    ResultStatus : ResultStatus;
    Messages: string[];
}

export interface GenericApiResult<T> extends ApiResult {
    Data : T;
}