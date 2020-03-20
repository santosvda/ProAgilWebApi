import { Lote } from "./Lote";
import { Palestrante } from "./Palestrante";
import { RedeSocial } from "./RedeSocial";
import { DateTimeFormatPipe } from '../_helps/DateTimeFormat.pipe';
export interface Evento {
         id: number;  
         local: string;  
         dataEvento: Date;  
         tema: string;  
         qtdPessoas: number;  
         lote: string;  
         imagemURL: string;  
         telefone: string;  
         email: string;  
         lotes: Lote[];
         redesSociais: RedeSocial[];
         palestrantesEventos: Palestrante[];
}
