
/// <summary>
/// Namespace pour les classe de type DTOs.
/// </summary>
namespace AppMobile_ProjetPompierC.DTO
{
    /// <summary>
    /// Classe représentant le DTO d'un type de véhicule.
    /// </summary>
    public class TypeVehiculeDTO
    {
        #region Proprietes

        /// <summary>
        /// Propriété représentant le type du véhicule.
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// Propriété représentant le code du véhicule.
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Propriété représentant le nombre de personne du véhicule.
        /// </summary>
        public int NombrePersonne { get; set; }

       /// <summary>
       /// Propriété représentant le nombre de personne du véhicule.
       /// </summary>
       /// <param name="type"></param>
       /// <param name="code"></param>
       /// <param name="nombrePersonne"></param>
        public TypeVehiculeDTO( string type = "", string code, int nombrePersonne)
        {
            Type = type;
            Code = code;
            NombrePersonne = nombrePersonne;
        }

        #endregion Constructeurs
    }
}
