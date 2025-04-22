using Battle_Spells.Models.DTOs;

namespace Battle_Spells.Api.Services.Interfaces
{
    public interface ISyncService
    {
        /// <summary>
        /// Sincronizza le risorse di eroi e carte ricevute dal frontend.
        /// Se una risorsa esiste già (per es. in base al nome per gli eroi o per le carte), aggiorna i dati,
        /// altrimenti la crea.
        /// </summary>
        Task<bool> SyncResourcesAsync(SyncResourcesRequest request);
    }
}
