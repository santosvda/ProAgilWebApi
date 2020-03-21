import { Component, OnInit, TemplateRef } from '@angular/core';
import { EventoService } from '../_services/evento.service';
import { Evento } from '../_models/Evento';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { defineLocale, BsLocaleService, ptBrLocale } from 'ngx-bootstrap';
import { templateJitUrl } from '@angular/compiler';
import { ToastrService } from 'ngx-toastr';

defineLocale('pt-br', ptBrLocale);

@Component({
  selector: 'app-eventos',
  templateUrl: './eventos.component.html',
  styleUrls: ['./eventos.component.css'],
  providers: [EventoService] //injeção de um serviço(se não tiver o "providedIn: 'root'" no service)
})
export class EventosComponent implements OnInit {
  
  titulo = "Eventos"
  eventosFiltrados: Evento[];

  eventos: Evento[];//define a variavel como array []
  modoSalvar = "post";
  bodyDeletarEvento = "";
  dataEvento: '';

  file: File;

  evento: Evento;
  imagemLargura: number = 50;
  imagemMargem: number = 2;
  mostrarImagem: boolean = false;
  registerForm: FormGroup;

  fileNameToUpdate: string;

  dataAtual: string;

  constructor(
    private eventoService: EventoService,
    private modalService: BsModalService,
    private fb: FormBuilder,
    private localeService: BsLocaleService,
    private toastr: ToastrService
    ) {
      this.localeService.use('pt-br');
     }

  _filtroLista: string;
  get filtroLista(): string{
    return this._filtroLista;
  }

  set filtroLista(value: string){
    this._filtroLista = value;
    this.eventosFiltrados = this.filtroLista ? this.filtrarEventos(this.filtroLista) : this.eventos;
  }

  excluirEvento(evento: Evento, template: any) {
    this.openModal(template);
    this.evento = evento;
    this.bodyDeletarEvento = `Tem certeza que deseja excluir o Evento: ${evento.tema}, Código: ${evento.id}`;
  }
  
  confirmeDelete(template: any) {
    console.log(this.evento.id)
    this.eventoService.deleteEvento(this.evento.id).subscribe(
      () => {
          template.hide();
          this.getEventos();
          this.toastr.success('Deletado com sucesso');
        }, error => {
          this.toastr.error(`Erro ao excluir ${error}`);
          console.log(error);
        }
    );
  }

  editarEvento(_evento: Evento, template: any){
    this.modoSalvar = "put";
    this.openModal(template);
    this.evento = Object.assign({}, _evento);//realiza um copia ao inves de um bind
    this.fileNameToUpdate = _evento.imagemURL.toString();
    this.evento.imagemURL = '';
    this.registerForm.patchValue(this.evento);
  }

  novoEvento(template: any){
    this.modoSalvar = "post";
    this.openModal(template);
  }

  openModal(template: any){
    this.registerForm.reset();
    template.show();
  }

  //executa antes da interface ser renderizada
  ngOnInit() {
    this.getEventos();
    this.validation();
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

  validation(){
    this.registerForm = this.fb.group({
      tema: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(50)]],
      local: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(50)]],
      dataEvento: ['', Validators.required],
      qtdPessoas: ['', [Validators.required, Validators.max(120000)]],
      imagemURL: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(50)]],
      telefone: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(12)]],
      email: ['', [Validators.required, Validators.email]],
    })
  }

  onFileChange(event){
    const reader = new FileReader();
    //verifica se existe imagem
    if(event.target.files && event.target.files.length){
      this.file = event.target.files;//atribui o file no evento a variaveil file do tipo File
      console.log(this.file);
    }
  }

  uploadImagem(){
    if (this.modoSalvar == 'post'){
        /*imgaemURL = c:\fakeFolder\3333.jpg
        nomeArquivo = [c:. fakeFolder, 3333jpg]
        */
        const nomeArquivo = this.evento.imagemURL.split('\\', 3);
        this.evento.imagemURL = nomeArquivo[2];

        this.eventoService.postUpload(this.file, nomeArquivo[2])
        .subscribe(
          () => {
            this.dataAtual = new Date().getMilliseconds().toString();
            this.getEventos();
          }
        );
    }else{
      this.evento.imagemURL = this.fileNameToUpdate;
      this.eventoService.postUpload(this.file, this.fileNameToUpdate).subscribe(
        () => {
          this.dataAtual = new Date().getMilliseconds().toString();
          console.log(this.dataAtual)
          this.getEventos();
        }
      );
    }
  }

  salvarAlteracao(template: any){
    if(this.registerForm.valid){
      if(this.modoSalvar === "post"){
        this.evento = Object.assign({}, this.registerForm.value);

        this.uploadImagem();

        this.eventoService.postEvento(this.evento).subscribe(
          (novoEvento: Evento) => {
            console.log(novoEvento);
            template.hide();
            this.getEventos();
            this.toastr.success('Inserido com sucesso');
          },
          error =>{
            this.toastr.error(`Erro ao inserir ${error}`);
            console.log(error);
          }
        );
      } else{
        this.evento = Object.assign({id: this.evento.id}, this.registerForm.value);

        this.uploadImagem();

        this.eventoService.putEvento(this.evento).subscribe(
          (novoEvento: Evento) => {
            console.log(novoEvento);
            template.hide();
            this.getEventos();
            this.toastr.success('Editado com sucesso');
          },
          error =>{
            this.toastr.error(`Erro ao editar ${error}`);
            console.log(error);
          }
        );
      }
    }
  }

  getEventos(){
    this.eventoService.getAllEvento().subscribe(
      (_eventos: Evento[])=>{
      this.eventos = _eventos;
      this.eventosFiltrados = this.eventos;
      console.log(_eventos)
    }, error =>{
      this.toastr.error(`Erro ao tentar carregar eventos ${error}`);
      console.log(error);
    }
    );
  }

}