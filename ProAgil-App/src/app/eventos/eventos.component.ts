import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-eventos',
  templateUrl: './eventos.component.html',
  styleUrls: ['./eventos.component.css']
})
export class EventosComponent implements OnInit {

  _filtroLista: string;
  get filtroLista(): string{
    return this._filtroLista;
  }

  set filtroLista(value: string){
    this._filtroLista = value;
    this.eventosFiltrado = this.filtroLista ? this.filtrarEventos(this.filtroLista) : this.eventos;
  }

  eventosFiltrado: any = []
  eventos: any = [];//define a variavel como array []
  imagemLargura: number = 50;
  imagemMargem: number = 2;
  mostrarImagem: boolean = false;

  constructor(private http: HttpClient) { }


  //executa antes da interface ser renderizada
  ngOnInit() {
    this.getEventos();
  }

  filtrarEventos(filtrarPor: string): any{
    filtrarPor = filtrarPor.toLocaleLowerCase();
    return this.eventos.filter(
      evento => evento.tema.toLocaleLowerCase().indexOf(filtrarPor) !== -1 
    );
  }

  alternarImagem(){
    this.mostrarImagem = !this.mostrarImagem; 
  }

  getEventos(){
    this.http.get('http://localhost:5000/api/values').subscribe(response=>{
      this.eventos = response;
      console.log(response)
    }, error =>{
      console.log(error);
    }
    );
  }

}
