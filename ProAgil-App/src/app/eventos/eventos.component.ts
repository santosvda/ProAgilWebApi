import { Component, OnInit, TemplateRef } from '@angular/core';
import { EventoService } from '../_services/evento.service';
import { Evento } from '../_models/Evento';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';

@Component({
  selector: 'app-eventos',
  templateUrl: './eventos.component.html',
  styleUrls: ['./eventos.component.css'],
  providers: [EventoService] //injeção de um serviço(se não tiver o "providedIn: 'root'" no service)
})
export class EventosComponent implements OnInit {


  eventosFiltrados: Evento[];
  eventos: Evento[];//define a variavel como array []
  imagemLargura: number = 50;
  imagemMargem: number = 2;
  mostrarImagem: boolean = false;
  modalRef: BsModalRef;

  constructor(
    private eventoService: EventoService,
    private modalService: BsModalService
    ) { }

  _filtroLista: string;
  get filtroLista(): string{
    return this._filtroLista;
  }

  set filtroLista(value: string){
    this._filtroLista = value;
    this.eventosFiltrados = this.filtroLista ? this.filtrarEventos(this.filtroLista) : this.eventos;
  }

  openModal(template: TemplateRef<any>){
    this.modalRef = this.modalService.show(template);
  }

  //executa antes da interface ser renderizada
  ngOnInit() {
    this.getEventos();
  }

  filtrarEventos(filtrarPor: string): Evento[]{
    filtrarPor = filtrarPor.toLocaleLowerCase();
    return this.eventos.filter(
      evento => evento.tema.toLocaleLowerCase().indexOf(filtrarPor) !== -1 
    );
  }

  alternarImagem(){
    this.mostrarImagem = !this.mostrarImagem; 
  }

  getEventos(){
    this.eventoService.getAllEvento().subscribe(
      (_eventos: Evento[])=>{
      this.eventos = _eventos;
      this.eventosFiltrados = this.eventos;
      console.log(_eventos)
    }, error =>{
      console.log(error);
    }
    );
  }

}
