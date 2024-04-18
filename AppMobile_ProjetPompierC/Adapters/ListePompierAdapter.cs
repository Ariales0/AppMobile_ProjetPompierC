using Android.Views;
using ProjetPompier_Mobile.DTO;
using AppMobile_ProjetPompierC;

/// <summary>
/// Namespace pour les adapteurs.
/// </summary>
namespace ProjetPompier_Mobile.Adapters
{
    /// <summary>
    /// Classe représentant un adapteur pour une liste de Pompier(s).
    /// </summary>
    internal class ListePompierAdapter : BaseAdapter<PompierDTO>
    {
        /// <summary>
        /// Attribut représetant le contexte.
        /// </summary>
        private Activity context;

        /// <summary>
        /// Attribut représentant la liste des pompiers.
        /// </summary>
		private PompierDTO[] listePompier;

        /// <summary>
        /// Méthode réécrite de la classe BaseAdapter permettant d'accéder à un élément de la liste de pompiers selon un index.
        /// </summary>
        /// <param name="index">Index de pompier.</param>
        /// <returns>Retourne l'objet PompierDTO correspondant à sa position dans la liste selon l'index passé en paramètre.</returns>
        public override PompierDTO this[int index]
        {
            get { return listePompier[index]; }
        }

        /// <summary>
        /// Méthode réécrite de la classe BaseAdapter permettant d'obtenir le nombre de pompier(s) dans la liste.
        /// </summary>
        /// <returns>Retourne le nombre de pompier(s) dans la liste.</returns>
        public override int Count
        {
            get { return listePompier.Length; }
        }

        /// <summary>
        /// Constructeur de la classe.
        /// </summary>
        /// <param name="unContext">Contexte</param>
        /// <param name="uneListePompier">Liste des pompiers</param>
        public ListePompierAdapter(Activity unContext, PompierDTO[] uneListePompier)
        {
            context = unContext;
            listePompier = uneListePompier;
        }

        /// <summary>
        /// Méthode réécrite de la classe BaseAdapter permettant d'obtenir le Id d'un pompier selon une position.
        /// </summary>
        /// <param name="position">Position du pompier dans la liste.</param>
        /// <returns>Retourne le ID du pompier à la position demandée.</returns>
        public override long GetItemId(int position)
        {
            return position;
        }

        /// <summary>
        /// Méthode réécrite de la classe BaseAdapter permettant d'obtenir le visuel d'un pompier.
        /// </summary>
        /// <param name="position">Position de pompier.</param>
        /// <param name="convertView">Vue.</param>
        /// <param name="parent">Parent de la vue.</param>
        /// <returns>Retourne une vue construite avec les données d'un pompier.</returns>
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = (convertView ?? context.LayoutInflater.Inflate(Resource.Layout.ListeAdapteurPompierItem, parent, false)) as LinearLayout;

            view.FindViewById<TextView>(Resource.Id.tvNomPompier).Text = listePompier[position].Grade +" " + listePompier[position].Nom +" "+ listePompier[position].Prenom;

            return view;
        }
    }
}