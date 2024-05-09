using AppMobile_ProjetPompierC.DTO;
/// <summary>
/// Namespace pour les classe de type DTOs.
/// </summary>

namespace AppMobile_ProjetPompierC.DTO
{
    /// <summary>
    /// Classe représentant le DTO d'un type d'intervention.
    /// </summary>
    public class TypeInterventionDTO
    {
        #region Proprietes

        /// <summary>
        /// Propriété représentant la description du type d'intervention.
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Propriété représentant le code du type d'intervention.
        /// </summary>
        public int Code { get; set; }

        #endregion Proprietes
    }
}
