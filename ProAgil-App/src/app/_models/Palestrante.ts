import { RedeSocial } from "./RedeSocial";
import { Evento } from "./Evento";

export interface Palestrante {
        id: number;
        nome: string;
        minicurriculo: string;
        imagemurl: string;
        telefone: string;
        email: string;
        redessociais: RedeSocial[];
        palestranteseventos: Evento[];
}
