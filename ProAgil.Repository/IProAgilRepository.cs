using System.Threading.Tasks;
using ProAgil.Domain;

namespace ProAgil.Repository
{
    public interface IProAgilRepository
    {
        //Geral
         void Add<T>(T entity) where T : class;
         void Update<T>(T entity) where T : class;
         void Delete<T>(T entity) where T : class;
         void DeleteRange<T>(T[] entity) where T : class;
         Task<bool> SaveChangesAsync();

         //Eventos
         Task<Evento[]> GetAllEventoAsyncByTema(string tema, bool includePalestrantes);
         Task<Evento[]> GetAllEventoAsync(bool includePalestrantes);
         Task<Evento> GetAllEventoAsyncById(int EventoId, bool includePalestrantes);

         //Palestrante
         Task<Palestrante[]> GetAllPalestranteAsyncByName(string nome,bool includeEventos);
         Task<Palestrante> GetAllPalestranteAsyncById(int PalestranteId, bool includeEventos);
         Task<Palestrante[]> GetAllPalestranteAsync(bool includeEventos);




    }
}