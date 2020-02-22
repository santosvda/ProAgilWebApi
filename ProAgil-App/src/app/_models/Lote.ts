import { Evento } from './Evento';

export interface Lote {
        nome: string;  
        preco: number;  
        dataInicio: Date;
        dataFim: Date;  
        quantidade: number;  
        eventoId: number;  
        evento: Evento; 
}
