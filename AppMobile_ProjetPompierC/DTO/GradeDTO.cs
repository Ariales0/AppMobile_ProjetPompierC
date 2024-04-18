
/// <summary>
/// Namespace pour les classe de type DTOs.
/// </summary>
namespace AppMobile_ProjetPompierC.DTO
{
    /// <summary>
    /// Classe représentant le DTO d'un grade.
    /// </summary>
    public class GradeDTO
    {
        #region Proprietes

        /// <summary>
        /// Propriété représentant la description du grade.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Constructeur avec paramètres.
        /// </summary>
        /// <param name="description">Description du grade</param>
        public GradeDTO( string description = "")
        {
            Description = description;
        }

        #endregion Constructeurs
    }
}
