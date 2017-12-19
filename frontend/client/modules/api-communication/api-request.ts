import {AxiosPromise} from 'axios'
import axios from 'axios'

export class ApiRequest {
    constructor() {
    }

    public post<T>(controller: string, method: string, params: any = null) : AxiosPromise<T>{
        return axios.post<T>(`http://localhost:5000/api/`+ controller + `/` + method,
        {
            body: params,
            headers: {
                'Content-type': 'application/x-www-form-urlencoded',
            }
        }
        );
    }
}