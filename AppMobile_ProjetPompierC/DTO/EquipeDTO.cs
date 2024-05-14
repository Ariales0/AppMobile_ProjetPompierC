/// <summary>
/// Namespace pour les classe de type DTOs.
/// </summary>
namespace AppMobile_ProjetPompierC.DTO
{
    /// <summary>
    /// Classe représentant le DTO d'une équipe.
    /// </summary>
    public class EquipeDTO
    {
        #region Proprietes
        /// <summary>
        /// Propriété représentant le code de l'équipe
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// Propriété représentant la liste des pompiers solicités dans l'intervention
        /// </summary>
        public List<PompierDTO> ListePompierEquipe { get; set; }

        /// <summary>
        /// Propriété représentant le vin du véhicule solicité pour l'intervention.
        /// </summary>
        public string VinVehicule { get; set; }

        #endregion Proprietes
    }
}
