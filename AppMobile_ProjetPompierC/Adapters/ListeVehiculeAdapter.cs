using Android.Views;
using AppMobile_ProjetPompierC.DTO;

/// <summary>
/// Namespace pour les adapteurs.
/// </summary>
namespace AppMobile_ProjetPompierC.Adapters
{
    /// <summary>
    /// Classe représentant un adapteur pour une liste de véhicule(s).
    /// </summary>
    internal class ListeVehiculeAdapter : BaseAdapter<VehiculeDTO>
    {
        /// <summary>
        /// Attribut représetant le contexte.
        /// </summary>
        private Activity context;

        /// <summary>
        /// Attribut représentant la liste des véhicules.
        /// </summary>
		private VehiculeDTO[] listeVehicule;

        /// <summary>
        /// Méthode réécrite de la classe BaseAdapter permettant d'accéder à un élément de la liste de véhicules selon un index.
        /// </summary>
        /// <param name="index">Index de véhicule.</param>
        /// <returns>Retourne l'objet PompierVehiculecorrespondant à sa position dans la liste selon l'index passé en paramètre.</returns>
        public override VehiculeDTO this[int index]
        {
            get { return listeVehicule[index]; }
        }

        /// <summary>
        /// Méthode réécrite de la classe BaseAdapter permettant d'obtenir le nombre de véhicule(s) dans la liste.
        /// </summary>
        /// <returns>Retourne le nombre de véhicule(s) dans la liste.</returns>
        public override int Count
        {
            get { return listeVehicule.Length; }
        }

        /// <summary>
        /// Constructeur de la classe.
        /// </summary>
        /// <param name="unContext">Contexte</param>
        /// <param name="uneListeVehicule">Liste des véhicules</param>
        public ListeVehiculeAdapter(Activity unContext, VehiculeDTO[] uneListeVehicule)
        {
            context = unContext;
            listeVehicule = uneListeVehicule;
        }

        /// <summary>
        /// Méthode réécrite de la classe BaseAdapter permettant d'obtenir le Id d'un véhicule selon une position.
        /// </summary>
        /// <param name="position">Position du véhicule dans la liste.</param>
        /// <returns>Retourne le ID du véhicule à la position demandée.</returns>
        public override long GetItemId(int position)
        {
            return position;
        }

        /// <summary>
        /// Méthode réécrite de la classe BaseAdapter permettant d'obtenir le visuel d'un véhicule.
        /// </summary>
        /// <param name="position">Position de pompier.</param>
        /// <param name="convertView">Vue.</param>
        /// <param name="parent">Parent de la vue.</param>
        /// <returns>Retourne une vue construite avec les données d'un véhicule.</returns>
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = (convertView ?? context.LayoutInflater.Inflate(Resource.Layout.ListeAdapteurVehiculeItem, parent, false)) as LinearLayout;

            view.FindViewById<TextView>(Resource.Id.tvVinVehicule).Text = listeVehicule[position].VinVehicule +" " + listeVehicule[position].Modele +" "+ listeVehicule[position].Annee;

            return view;
        }
    }
}