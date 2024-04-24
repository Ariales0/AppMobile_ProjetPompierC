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
		/// Propriété représentant le type d'intervention.
		/// </summary>
		public string TypeIntervention { get; set; }

		/// <summary>
		/// Propriété représentant le resumé de l'intervention.
		/// </summary>
		public string Resume { get; set; }

		/// <summary>
		/// Propriété représentant le matricule du capitaine.
		/// </summary>
		public int MatriculeCapitaine { get; set; }


		#endregion Proprietes

		#region Constructeurs
		/// <summary>
		/// Constructeur vide.
		/// </summary>
		public FicheInterventionDTO() { }

		/// <summary>
		/// Constructeur avec paramètres.
		/// </summary>
		/// <param name="dateDebut">Date et heure de l'intervention</param>
		/// <param name="dateFin">Date et heure de l'intervention</param>
		/// <param name="adresse">Adresse de l'intervention</param>
		/// <param name="typeIntervention">Type d'intervention</param>
		/// <param name="resume">Resumé de l'intervention</param>
		/// <param name="matriculeCapitaine">Matricule du pompier capitaine</param>
		public FicheInterventionDTO(string dateDebut = "1999-01-01 00:00:00", string dateFin = "1999-01-01 00:00:00", string adresse = "", string typeIntervention = "", string resume = "", int matriculeCapitaine = 000000)
		{
			DateDebut = dateDebut;
			DateFin = dateFin;
			Adresse = adresse;
			TypeIntervention = typeIntervention;
			Resume = resume;
			MatriculeCapitaine = matriculeCapitaine;
		}

		#endregion Constructeurs
	}
}