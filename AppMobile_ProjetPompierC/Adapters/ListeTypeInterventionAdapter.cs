using Android.Views;
using AppMobile_ProjetPompierC.DTO;

/// <summary>
/// Namespace pour les adapteurs.
/// </summary>
namespace AppMobile_ProjetPompierC.Adapters
{
    /// <summary>
    /// Classe représentant un adapteur pour une liste de type(s) de véhicule(s).
    /// </summary>
    internal class ListeTypeInterventionAdapter : BaseAdapter<TypeInterventionDTO>
    {
        /// <summary>
        /// Attribut représetant le contexte.
        /// </summary>
        private Activity context;

        /// <summary>
        /// Attribut représentant la liste des types.
        /// </summary>
		private TypeInterventionDTO[] listeTypeIntervention;

        /// <summary>
        /// Méthode réécrite de la classe BaseAdapter permettant d'accéder à un élément de la liste des types selon un index.
        /// </summary>
        /// <param name="index">Index du grade.</param>
        /// <returns>Retourne l'objet TypeVehiculeDTO correspondant à sa position dans la liste selon l'index passé en paramètre.</returns>
        public override TypeInterventionDTO this[int index]
        {
            get { return listeTypeIntervention[index]; }
        }

        /// <summary>
        /// Méthode réécrite de la classe BaseAdapter permettant d'obtenir le nombre de type(s) dans la liste.
        /// </summary>
        /// <returns>Retourne le nombre de type(s) dans la liste.</returns>
        public override int Count
        {
            get { return listeTypeIntervention.Length; }
        }

        /// <summary>
        /// Constructeur de la classe.
        /// </summary>
        /// <param name="unContext">Contexte</param>
        /// <param name="uneListeTypeIntervention">Liste des types</param>
        public ListeTypeInterventionAdapter(Activity unContext, TypeInterventionDTO[] uneListeTypeIntervention)
        {
            context = unContext;
            listeTypeIntervention = uneListeTypeIntervention;
        }

        /// <summary>
        /// Méthode réécrite de la classe BaseAdapter permettant d'obtenir le Id d'un type selon une position.
        /// </summary>
        /// <param name="position">Position du grade dans laa liste.</param>
        /// <returns>Retourne le ID du type à la position demandée.</returns>
        public override long GetItemId(int position)
        {
            return position;
        }

        /// <summary>
        /// Méthode réécrite de la classe BaseAdapter permettant d'obtenir le visuel d'un type.
        /// </summary>
        /// <param name="position">Position du grade.</param>
        /// <param name="convertView">Vue.</param>
        /// <param name="parent">Parent de la vue.</param>
        /// <returns>Retourne une vue construite avec les données d'un grade.</returns>
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = (convertView ?? context.LayoutInflater.Inflate(Resource.Layout.ListeAdapteurTypeInterventionItem, parent, false)) as LinearLayout;

            view.FindViewById<TextView>(Resource.Id.tvDescriptionTypeIntervention).Text = listeTypeIntervention[position].Description;

            return view;
        }
    }
}