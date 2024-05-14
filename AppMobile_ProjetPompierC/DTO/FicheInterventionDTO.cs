/// <summary>
/// Namespace pour les classe de type DTOs.
/// </summary>
namespace AppMobile_ProjetPompierC.DTO

{

	/// <summary>

	/// Classe représentant le DTO d'une fiche d'intervention.

	/// </summary>

	public class FicheInterventionDTO
	{

        #region Proprietes

        /// <summary>
        /// Propriété représentant la date et l'heure du début l'intervention.
        /// </summary>
        public string DateDebut { get; set; }

        /// <summary>
        /// Propriété représentant la date et l'heure de la fin de l'intervention.
        /// </summary>
        public string DateFin { get; set; }

        /// <summary>
        /// Propriété représentant l'adresse de l'intervention'.
        /// </summary>
        public string Adresse { get; set; }

        /// <summary>
        /// Propriété représentant le code du type d'intervention.
        /// </summary>
        public int CodeTypeIntervention { get; set; }

        /// <summary>
        /// Propriété représentant le resumé de l'intervention.
        /// </summary>
        public string Resume { get; set; }

        /// <summary>
        /// Propriété représentant le matricule du capitaine.
        /// </summary>
        public int MatriculeCapitaine { get; set; }


        #endregion Proprietes

	}

}
