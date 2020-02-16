import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-eventos',
  templateUrl: './eventos.component.html',
  styleUrls: ['./eventos.component.css']
})
export class EventosComponent implements OnInit {
  eventos: any;
/*
  eventos: any = [
    {
      EventoId: 1,
      Tema: "Angular",
      Local: "Ipatinga"
    },
    {
      EventoId: 2,
      Tema: "Rodeio .NetCore",
      Local: "Timoteo"
    }
  ];
  */
 //injetando dependencia
  constructor(private http: HttpClient) { }


  //executa antes da interface ser renderizada
  ngOnInit() {
    this.getEventos();
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
