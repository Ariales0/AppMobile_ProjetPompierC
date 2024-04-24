
/// <summary>
/// Namespace pour les classe de type DTOs.
/// </summary>
namespace AppMobile_ProjetPompierC.DTO
{
    /// <summary>
    /// Classe représentant le DTO d'un véhicule.
    /// </summary>
    public class VehiculeDTO
    {
        #region Proprietes
        /// <summary>
        /// Propriété représentant le VIN du véhicule.
        /// </summary>
        public string VinVehicule { get; set; }
        /// <summary>
        /// Propriété représentant le code du véhicule.
        /// </summary>
        public int Code { get; set; }
        /// <summary>
        /// Propriété représentant le type du véhicule.
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// Propriété représentant la marque du véhicule.
        /// </summary>
        public string Marque { get; set; }
        /// <summary>
        /// Propriété représentant le modèle du véhicule.
        /// </summary>
        public string Modele { get; set; }
        /// <summary>
        /// Propriété représentant l'année du véhicule.
        /// </summary>
        public int Annee { get; set; }
        /// <summary>
        /// Propriété représentant le nombre de personne du véhicule.
        /// </summary>
        public int NombrePersonne { get; set; }


        #endregion Proprietes

        #region Constructeurs
        /// <summary>
        /// Constructeur vide.
        /// </summary>
        public VehiculeDTO() { }

        /// <summary>
        /// Constructeur avec paramètres.
        /// </summary>
        /// <param name="vinVehicule"></param>
        /// <param name="code"></param>
        /// <param name="type"></param>
        /// <param name="marque"></param>
        /// <param name="modele"></param>
        /// <param name="annee"></param>
        /// <param name="nombrePersonne"></param>
        public VehiculeDTO(string vinVehicule, int code, string type, string marque, string modele, int annee, int nombrePersonne)
        {
			VinVehicule = vinVehicule;
            Code = code;
            Type = type;
            Marque = marque;
            Modele = modele;
            Annee = annee;
            NombrePersonne = nombrePersonne;
            
        }

        #endregion Constructeurs
    }
}
