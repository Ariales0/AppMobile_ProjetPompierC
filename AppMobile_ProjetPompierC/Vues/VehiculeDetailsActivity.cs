using Android.Content;
using Android.Views;
using Newtonsoft.Json;

using AppMobile_ProjetPompierC;
using AppMobile_ProjetPompierC.DTO;
using AppMobile_ProjetPompierC.Utils;
using ProjetPompier_Mobile.Vues;

/// <summary>
/// Namespace pour les classes de type Vue.
/// </summary>
namespace AppMobile_ProjetPompierC.Vues
{
    /// <summary>
    /// Classe de type Activit� pour les d�tails d'un pompier.
    /// </summary>
    [Activity(Label = "@string/app_name")]
    public class VehiculeDetailsActivity : Activity
    {
        #region Proprietes

        /// <summary>
        /// Attribut repr�sentant le nom de la caserne.
        /// </summary>
        private string paramNomCaserne;

        /// <summary>
        /// Attribut repr�sentant le VIN du v�hicule.
        /// </summary>
        private string paramVinVehicule;

        /// <summary>
        /// Attribut repr�sentant le v�hicule en objet VehiculeDTO.
        /// </summary>
        private VehiculeDTO leVehicule;

        /// <summary>
        /// Attribut repr�sentant le champ d'affichage du VIN du v�hicule.
        /// </summary>
        private TextView lblVinVehiculeAfficher;

        /// <summary>
        /// Attribut repr�sentant le champ d'affichage du type du v�hicule.
        /// </summary>
        private TextView lblTypeVehiculeAfficher;

        /// <summary>
        /// Attribut repr�sentant le champ d'affichage de la marque du v�hicule.
        /// </summary>
        private TextView lblMarqueVehiculeAfficher;

		/// <summary>
		/// Attribut repr�sentant le champ d'affichage de l'ann�e du v�hicule.
		/// </summary>
		private TextView lblAnneeVehiculeAfficher;

		/// <summary>
		/// Attribut repr�sentant le champ d'affichage du mod�le du v�hicule.
		/// </summary>
		private TextView lblModeleVehiculeAfficher;

        #endregion Proprietes
        #region MethodesVehiculeActivity

        /// <summary>
        /// M�thode de service appel�e lors de la cr�ation de l'activit�.
        /// </summary>
        /// <param name="savedInstanceState">�tat de l'activit�.</param>
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.InterfaceVehiculeDetails);

            lblVinVehiculeAfficher = FindViewById<TextView>(Resource.Id.tvVinVehiculeDetails);
            lblTypeVehiculeAfficher = FindViewById<TextView>(Resource.Id.tvTypeVehiculeDetails);
            lblMarqueVehiculeAfficher = FindViewById<TextView>(Resource.Id.tvMarqueVehiculeDetails);
            lblModeleVehiculeAfficher = FindViewById<TextView>(Resource.Id.tvModeleVehiculeDetails);
            lblAnneeVehiculeAfficher = FindViewById<TextView>(Resource.Id.tvAnneeVehiculeDetails);

            paramNomCaserne = Intent.GetStringExtra("paramNomCaserne");
            paramVinVehicule = Intent.GetStringExtra("paramVinVehicule");
        }

        /// <summary>
        /// M�thode de service appel�e lors du retour en avant plan de l'activit�.
        /// </summary>
        protected override async void OnResume()
        {
            base.OnResume();
            await RafraichirInterfaceDonnees();
        }


        /// <summary>
        /// M�thode permettant de rafraichir les informations de la Caserne...
        /// </summary>
        private async Task RafraichirInterfaceDonnees()
        {
            try
            {
                string jsonResponse = await WebAPI.Instance.ExecuteGetAsync("http://" + GetString(Resource.String.host) + ":" + GetString(Resource.String.port) + "/Vehicule/ObtenirVehicule?nomCaserne=" + paramNomCaserne +" &VinVehicule=" + paramVinVehicule);
                leVehicule = JsonConvert.DeserializeObject<VehiculeDTO>(jsonResponse);
                lblVinVehiculeAfficher.Text = leVehicule.VinVehicule;
                lblMarqueVehiculeAfficher.Text = leVehicule.Marque;
                lblTypeVehiculeAfficher.Text = leVehicule.Type;
                lblModeleVehiculeAfficher.Text = leVehicule.Modele;
                lblAnneeVehiculeAfficher.Text = leVehicule.Annee.ToString();
            }
            catch (Exception)
            {
                Finish();
            }
        }

        #region Menu
        /// <summary>M�thode de service permettant d'initialiser le menu de l'activit� principale.</summary>
        /// <param name="menu">Le menu � construire.</param>
        /// <returns>Retourne True si l'optionMenu est bien cr��.</returns>
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.VehiculeDetailsOptionsMenu, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        /// <summary>M�thode de service permettant de capter l'�venement ex�cut� lors d'un choix dans le menu.</summary>
        /// <param name="item">L'item s�lectionn�.</param>
        /// <returns>Retourne si un option � �t� s�lectionn� avec succ�s.</returns>
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.ModifierVehiculeDetails:
                    Intent activiteModifier = new Intent(this, typeof(VehiculeModifierActivity));
                    activiteModifier.PutExtra("paramNomCaserne", paramNomCaserne);
                    activiteModifier.PutExtra("paramVinVehicule", paramVinVehicule);
                    StartActivity(activiteModifier);
                    break;


                case Resource.Id.SupprimerPompierDetails:
                    try
                    {
                        AlertDialog.Builder builder = new AlertDialog.Builder(this);
                        builder.SetPositiveButton("Non", (send, args) => { });
                        builder.SetNegativeButton("Oui", async (send, args) =>
                        {
                            await WebAPI.Instance.PostAsync("http://" + GetString(Resource.String.host) + ":" + GetString(Resource.String.port) + "/Vehicule/SupprimerVehicule?vinVehicule=" + paramVinVehicule+ "&nomCaserne=" + paramNomCaserne, null);
                            Finish();
                        });
                        AlertDialog dialog = builder.Create();
                        dialog.SetTitle("Suppression");
                        dialog.SetMessage("Voulez-vous vraiment supprimer le v�hicule " + leVehicule.VinVehicule +  " ?");
                        dialog.Window.SetGravity(GravityFlags.Bottom);
                        dialog.Show();
                    }
                    catch (Exception ex)
                    {
                        DialoguesUtils.AfficherMessageOK(this, "Erreur", ex.Message);
                    }
                    break;

                case Resource.Id.RetourVehiculeDetails:
                    Finish();
                    break;

                case Resource.Id.QuitterVehiculeDetails:
                    FinishAffinity();
                    break;
            }
            return base.OnOptionsItemSelected(item);
        }
        
    }
    #endregion Menu
    #endregion MethodesVehiculeActivity
}
