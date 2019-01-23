import { HttpClient, HttpParams, HttpEvent } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { HttpParam } from '../_models/http-param.model';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable()
export class ApiService {

    private baseUrl: string;

    constructor(private http: HttpClient) {
        this.baseUrl = environment.serverUrl;
    }

    public getAll<T>(actionURL: string): Observable<T> {
        return this.http.get<T>(this.baseUrl + actionURL);
    }

    public getByStringId<T>(actionUrl: string, id: string): Observable<T> {
        return this.http.get<T>(this.baseUrl + actionUrl + id);
    }
    public getById<T>(actionUrl: string, id: string): Observable<T> {
        return this.http.get<T>(this.baseUrl + actionUrl + id);
    }

    public post<T>(actionUrl: string, model: T): Observable<T> {
        return this.http.post<T>(this.baseUrl + actionUrl, model);
    }

    public put<T>(actionUrl: string, itemToUpdate: any): Observable<T> {
        return this.http.put<T>(this.baseUrl + actionUrl, itemToUpdate);
    }

    public delete<T>(actionUrl: string, id: string): Observable<T> {
        return this.http.delete<T>(this.baseUrl + actionUrl + id);
    }

    public getPage<T>(actionURL: string, params?: HttpParam[]): Observable<T> {
        let httpParams = new HttpParams();
        if (params) {
            params.forEach(param => {
                httpParams = httpParams.append(param.key, param.value);
            });
        }
        return this.http.get<T>(this.baseUrl + actionURL, { params: httpParams });
    }

}
