namespace AppMobile_ProjetPompierC.DTO
{
    public class FicheInterventionDTO
    {
        #region Proprities

        /// <summary>
        /// Propriété pour le numéro de la fiche d'intervention.
        /// </summary>
        public string DateTemps { get; set; }

        /// <summary>
        /// Propriété pour l'adresse de la fiche d'intervention.
        /// </summary>
        public string Adresse { get; set; }

        /// <summary>
        /// Propriété pour le type d'intervention de la fiche d'intervention.
        /// </summary>
        public string TypeIntervention { get; set;}

        /// <summary>
        /// Propriété pour le résumé de la fiche d'intervention.
        /// </summary>
        public string Resume { get; set; }

        /// <summary>
        /// Propriété pour le matricule du capitaine de la fiche d'intervention.
        /// </summary>
        public int MatriculeCapitaine { get; set; }

        #endregion Proprities
    }
}