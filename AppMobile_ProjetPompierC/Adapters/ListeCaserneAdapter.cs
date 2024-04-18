using AppMobile_ProjetPompierC;
using Android.Views;
using ProjetPompier_Mobile.DTO;

/// <summary>
/// Namespace pour les adapteurs.
/// </summary>
namespace ProjetPompier_Mobile.Adapters
{
    /// <summary>
    /// Classe représentant un adapteur pour une liste de Caserne(s).
    /// </summary>
    internal class ListeCaserneAdapter : BaseAdapter<CaserneDTO>
    {
        /// <summary>
        /// Attribut représetant le contexte.
        /// </summary>
        private Activity context;

        /// <summary>
        /// Attribut représentant la liste des casernes.
        /// </summary>
		private CaserneDTO[] listeCaserne;

        /// <summary>
        /// Méthode réécrite de la classe BaseAdapter permettant d'accéder à un élément de la liste de Caserne selon un index.
        /// </summary>
        /// <param name="index">Index de Caserne.</param>
        /// <returns>Retourne une caserneDTO contenant les informations de la caserne selon l'index passé en paramètre.</returns>
        public override CaserneDTO this[int index]
        {
            get { return listeCaserne[index]; }
        }

        /// <summary>
        /// Méthode réécrite de la classe BaseAdapter permettant d'obtenir le nombre de Caserne(s) dans la liste.
        /// </summary>
        /// <returns>Retourne le nombre de Caserne(s) dans la liste.</returns>
        public override int Count
        {
            get { return listeCaserne.Length; }
        }

        /// <summary>
        /// Constructeur de la classe.
        /// </summary>
        /// <param name="unContext">Contexte</param>
        /// <param name="uneListeCaserne">Liste des Casernes</param>
        public ListeCaserneAdapter(Activity unContext, CaserneDTO[] uneListeCaserne)
        {
            context = unContext;
            listeCaserne = uneListeCaserne;
        }

        /// <summary>
        /// Méthode réécrite de la classe BaseAdapter permettant d'obtenir le Id d'une Caserne selon une position.
        /// </summary>
        /// <param name="position">Position de Caserne.</param>
        /// <returns>Retourne le ID de la Caserne à la position demandée.</returns>
        public override long GetItemId(int position)
        {
            return position;
        }

        /// <summary>
        /// Méthode réécrite de la classe BaseAdapter permettant d'obtenir le visuel d'une Caserne.
        /// </summary>
        /// <param name="position">Position de Caserne.</param>
        /// <param name="convertView">Vue.</param>
        /// <param name="parent">Parent de la vue.</param>
        /// <returns>Retourne une vue construite avec les données d'une Caserne.</returns>
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = (convertView ?? context.LayoutInflater.Inflate(Resource.Layout.ListeAdapteurCaserneItem, parent, false)) as LinearLayout;

            view.FindViewById<TextView>(Resource.Id.tvNomCaserne).Text = listeCaserne[position].Nom;

            return view;
        }
    }
}