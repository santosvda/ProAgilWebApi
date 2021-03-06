import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Evento } from '../_models/Evento';

@Injectable({
  /*Especifica que pode utilizar(injetar) este service na aplicação toda*/
  providedIn: 'root'
})
export class EventoService {
  baseURL = 'http://localhost:5000/api/evento';
  //tokenHeader: HttpHeaders;

constructor(private http:HttpClient) { 
   //this.tokenHeader = new HttpHeaders({'Authorization': `Bearer ${localStorage.getItem('token')}` });
   //{ headers: this.tokenHeader }
}

  getAllEvento(): Observable<Evento[]>{
    //retorna um observable https://tableless.com.br/entendendo-rxjs-observable-com-angular/
    return this.http.get<Evento[]>(this.baseURL, );
  }

  getEventoByTema(tema: string): Observable<Evento[]>{
    return this.http.get<Evento[]>(`${this.baseURL}/getByTema/${tema}`);
  }

  getEventoById(id: number): Observable<Evento>{
    return this.http.get<Evento>(`${this.baseURL}/${id}`);
  }

  postEvento(evento: Evento){
    return this.http.post(`${this.baseURL}`, evento,);
  }

  postUpload(file: File, name: string){
    const fileToUpload = <File>file[0];//no dotnet o arquivo é um array, logo aqui tbm sera
    const formData = new FormData();
    formData.append('file', fileToUpload, name);

    return this.http.post(`${this.baseURL}/upload`, formData)
  }

  putEvento(evento: Evento){
    return this.http.put(`${this.baseURL}/${evento.id}`, evento);
  }

  deleteEvento(id: number){
    return this.http.delete(`${this.baseURL}/${id}`);
  }
  

}
