import { Inject } from '@angular/core/core';
import { environment } from '../environments/environment';
import { Observable, Subject } from 'rxjs';
import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr';

export enum ConnectionState {
    Connecting = 1,
    Connected = 2,
    Reconnecting = 3,
    Disconnected = 4
}

export class BaseHub {
    protected BASEURL: string;

    protected starting: Observable<any>;
    protected connectionState: Observable<ConnectionState>;
    protected error: Observable<string>;
    protected closing: Observable<string>;

    protected connectionStateSubject = new Subject<ConnectionState>();
    protected startingSubject = new Subject<any>();
    protected errorSubject = new Subject<any>();
    protected closeSubject = new Subject<any>();
    protected hubConnection: HubConnection;

    // protected hubProxy: HubProxy;

    constructor(protected readonly hubName: string) {
        this.BASEURL = environment.serverUrl;
        this.connectionState = this.connectionStateSubject.asObservable();
        this.error = this.errorSubject.asObservable();
        this.starting = this.startingSubject.asObservable();
        this.closing = this.closeSubject.asObservable();
        const hubUrl: string = this.BASEURL + hubName;
        this.hubConnection = new HubConnectionBuilder().withUrl(hubUrl).build();

        // this.hubConnection.onclose(() => {
        //     this.closeSubject.next();
        // });
    }

    start(): void {
        this.hubConnection.start()
            .then(() => {
                this.startingSubject.next();
            })
            .catch((error: any) => {
                this.startingSubject.error(error);
            });
    }
    public close(): void {
        this.hubConnection.stop();
    }

}
