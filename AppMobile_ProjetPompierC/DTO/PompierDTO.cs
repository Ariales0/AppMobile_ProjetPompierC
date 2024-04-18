
/// <summary>
/// Namespace pour les classe de type DTOs.
/// </summary>
namespace ProjetPompier_Mobile.DTO
{
    /// <summary>
    /// Classe représentant le DTO d'un pompier.
    /// </summary>
    public class PompierDTO
    {
        #region Proprietes
        /// <summary>
        /// Propriété représentant le matricule du pompier.
        /// </summary>
        public int Matricule { get; set; }

        /// <summary>
        /// Propriété représentant le grade du pompier.
        /// </summary>
        public string Grade { get; set; }

        /// <summary>
        /// Propriété représentant le nom du pompier.
        /// </summary>
        public string Nom { get; set; }

        /// <summary>
        /// Propriété représentant le prenom du pompier.
        /// </summary>
        public string Prenom { get; set; }

        #endregion Proprietes

        #region Constructeurs
        /// <summary>
        /// Constructeur vide.
        /// </summary>
        public PompierDTO() { }

        /// <summary>
        /// Constructeur avec paramètres.
        /// </summary>
        /// <param name="matricule">Matricule du pompier</param>
        /// <param name="grade">Grade du pompier</param>
        /// <param name="nom">Nom du pompier</param>
        /// <param name="prenom">Prenom du pompier</param>
        public PompierDTO(int matricule = 000000, string grade = "", string nom = "", string prenom = "")
        {
            Matricule = matricule;
            Grade = grade;
            Nom = nom;
            Prenom = prenom;
        }

        #endregion Constructeurs
    }
}
