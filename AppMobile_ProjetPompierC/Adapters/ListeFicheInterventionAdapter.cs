using Android.Views;
using AppMobile_ProjetPompierC.DTO;

/// <summary>
/// Namespace pour les adapteurs.
/// </summary>
namespace AppMobile_ProjetPompierC.Adapters
{
    /// <summary>
    /// Classe représentant un adapteur pour une liste de Intervention(s).
    /// </summary>
    internal class ListeInterventionAdapter : BaseAdapter<FicheInterventionDTO>
    {
        /// <summary>
        /// Attribut représetant le contexte.
        /// </summary>
        private Activity context;

        /// <summary>
        /// Attribut représentant la liste des interventions.
        /// </summary>
		private FicheInterventionDTO[] listeIntervention;

        /// <summary>
        /// Méthode réécrite de la classe BaseAdapter permettant d'accéder à un élément de la liste de Interventions selon un index.
        /// </summary>
        /// <param name="index">Index de intervention.</param>
        /// <returns>Retourne une ficheIntevetnionDTO contenant les informations de la caserne selon l'index passé en paramètre.</returns>
        public override FicheInterventionDTO this[int index]
        {
            get { return listeIntervention[index]; }
        }

        /// <summary>
        /// Méthode réécrite de la classe BaseAdapter permettant d'obtenir le nombre d'intervention(s) dans la liste.
        /// </summary>
        /// <returns>Retourne le nombre d'intervention(s) dans la liste.</returns>
        public override int Count
        {
            get { return listeIntervention.Length; }
        }

        /// <summary>
        /// Constructeur de la classe.
        /// </summary>
        /// <param name="unContext">Contexte</param>
        /// <param name="uneListeIntervention">Liste des interventions</param>
        public ListeInterventionAdapter(Activity unContext, FicheInterventionDTO[] uneListeIntervention)
        {
            context = unContext;
            listeIntervention = uneListeIntervention;
        }

        /// <summary>
        /// Méthode réécrite de la classe BaseAdapter permettant d'obtenir le Id d'une intervention selon une position.
        /// </summary>
        /// <param name="position">Position de l'intervention.</param>
        /// <returns>Retourne le ID de intervention à la position demandée.</returns>
        public override long GetItemId(int position)
        {
            return position;
        }

        /// <summary>
        /// Méthode réécrite de la classe BaseAdapter permettant d'obtenir le visuel d'une intervention.
        /// </summary>
        /// <param name="position">Position de intervention.</param>
        /// <param name="convertView">Vue.</param>
        /// <param name="parent">Parent de la vue.</param>
        /// <returns>Retourne une vue construite avec les données d'une intervention.</returns>
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = (convertView ?? context.LayoutInflater.Inflate(Resource.Layout.ListeAdapteurFicheInterventionItem, parent, false)) as LinearLayout;

            view.FindViewById<TextView>(Resource.Id.tvTypeIntervention).Text = listeIntervention[position].TypeIntervention;

            return view;
        }
    }
}