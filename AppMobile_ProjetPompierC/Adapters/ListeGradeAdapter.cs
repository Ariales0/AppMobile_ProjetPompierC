using AppMobile_ProjetPompierC;
using Android.Views;
using ProjetPompier_Mobile.DTO;

/// <summary>
/// Namespace pour les adapteurs.
/// </summary>
namespace ProjetPompier_Mobile.Adapters
{
    /// <summary>
    /// Classe représentant un adapteur pour une liste de grade(s).
    /// </summary>
    internal class ListeGradeAdapter : BaseAdapter<GradeDTO>
    {
        /// <summary>
        /// Attribut représetant le contexte.
        /// </summary>
        private Activity context;

        /// <summary>
        /// Attribut représentant la liste des pompiers.
        /// </summary>
		private GradeDTO[] listeGrade;

        /// <summary>
        /// Méthode réécrite de la classe BaseAdapter permettant d'accéder à un élément de la liste des grades selon un index.
        /// </summary>
        /// <param name="index">Index du grade.</param>
        /// <returns>Retourne l'objet GradeDTO correspondant à sa position dans la liste selon l'index passé en paramètre.</returns>
        public override GradeDTO this[int index]
        {
            get { return listeGrade[index]; }
        }

        /// <summary>
        /// Méthode réécrite de la classe BaseAdapter permettant d'obtenir le nombre de grade(s) dans la liste.
        /// </summary>
        /// <returns>Retourne le nombre de pompier(s) dans la liste.</returns>
        public override int Count
        {
            get { return listeGrade.Length; }
        }

        /// <summary>
        /// Constructeur de la classe.
        /// </summary>
        /// <param name="unContext">Contexte</param>
        /// <param name="uneListePompier">Liste des pompiers</param>
        public ListeGradeAdapter(Activity unContext, GradeDTO[] uneListeGrade)
        {
            context = unContext;
            listeGrade = uneListeGrade;
        }

        /// <summary>
        /// Méthode réécrite de la classe BaseAdapter permettant d'obtenir le Id d'un grade selon une position.
        /// </summary>
        /// <param name="position">Position du grade dans laa liste.</param>
        /// <returns>Retourne le ID du grade à la position demandée.</returns>
        public override long GetItemId(int position)
        {
            return position;
        }

        /// <summary>
        /// Méthode réécrite de la classe BaseAdapter permettant d'obtenir le visuel d'un grade.
        /// </summary>
        /// <param name="position">Position du grade.</param>
        /// <param name="convertView">Vue.</param>
        /// <param name="parent">Parent de la vue.</param>
        /// <returns>Retourne une vue construite avec les données d'un grade.</returns>
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = (convertView ?? context.LayoutInflater.Inflate(Resource.Layout.ListeAdapteurGradeItem, parent, false)) as LinearLayout;

            view.FindViewById<TextView>(Resource.Id.tvDescriptionGrade).Text = listeGrade[position].Description;

            return view;
        }
    }
}