import {AxiosPromise} from 'axios'
import axios from 'axios'

export class ApiRequest {
    constructor() {
    }

    public get<T>(controller: string, method: string, params: any = null) : AxiosPromise<T>{
        return axios.get<T>(`http://localhost:5000/api/`+ controller + `/` + method, params
        );
    }
    public post<T>(controller: string, method: string, params: any = null) : AxiosPromise<T>{
        return axios.post<T>(`http://localhost:5000/api/`+ controller + `/` + method, params
        );
    }

    public put<T>(controller: string, method: string, params: any = null) : AxiosPromise<T>{
        return axios.put<T>(`http://localhost:5000/api/`+ controller + `/` + method, params
        );
    }

    public delete<T>(controller: string, method: string, id: number) : AxiosPromise<T>{
        return axios.delete(`http://localhost:5000/api/`+ controller + `/` + method + `/` + id);
    }
}