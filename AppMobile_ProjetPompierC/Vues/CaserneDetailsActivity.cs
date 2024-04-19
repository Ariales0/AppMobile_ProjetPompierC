using Android.Content;
using Android.Views;
using Newtonsoft.Json;

using AppMobile_ProjetPompierC;
using AppMobile_ProjetPompierC.DTO;
using AppMobile_ProjetPompierC.Utils;
using AppMobile_ProjetPompierC.Vues;

/// <summary>
/// Namespace pour les classes de type Vue.
/// </summary>
namespace ProjetPompier_Mobile.Vues
{
    /// <summary>
    /// Classe de type Activité pour la gestion d'une Caserne.
    /// </summary>
    [Activity(Label = "@string/app_name")]
    public class CaserneDetailsActivity : Activity
    {
        /// <summary>
        /// Attribut représentant le nom de la caserne.
        /// </summary>
        private string paramNomCaserne;

        /// <summary>
        /// Attribut représentant la caserne.
        /// </summary>
        private CaserneDTO laCaserne;

        /// <summary>
        /// Attribut représentant le champ d'édition du nom de la caserne.
        /// </summary>
        private TextView lblNomCaserneAfficher;

        /// <summary>
        /// Attribut représentant le champ d'édition de l'adresse de la caserne.
        /// </summary>
        private TextView lblAdresseCaserneAfficher;

        /// <summary>
        /// Attribut représentant le champ d'édition de la ville de la caserne.
        /// </summary>
        private TextView lblVilleCaserneAfficher;

        /// <summary>
        /// Attribut représentant le champ d'édition de province de la caserne.
        /// </summary>
        private TextView lblProvinceCaserneAfficher;

        /// <summary>
        /// Attribut représentant le champ d'édition du telephone de la caserne.
        /// </summary>
        private TextView lblTelephoneCaserneAfficher;

        /// <summary>
        /// Méthode de service appelée lors de la création de l'activité.
        /// </summary>
        /// <param name="savedInstanceState">État de l'activité.</param>
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.InterfaceCaserneDetails_Activity);

            lblNomCaserneAfficher = FindViewById<TextView>(Resource.Id.lblNomAfficher);
            lblAdresseCaserneAfficher = FindViewById<TextView>(Resource.Id.lblAdresseAfficher);
            lblVilleCaserneAfficher = FindViewById<TextView>(Resource.Id.lblVilleAfficher);
            lblProvinceCaserneAfficher = FindViewById<TextView>(Resource.Id.lblProvinceAfficher);
            lblTelephoneCaserneAfficher = FindViewById<TextView>(Resource.Id.lblTelephoneAfficher);

            paramNomCaserne = Intent.GetStringExtra("paramNomCaserne");
        }

        /// <summary>
        /// Méthode de service appelée lors du retour en avant plan de l'activité.
        /// </summary>
        protected override async void OnResume()
        {
            base.OnResume();
            await RafraichirInterfaceDonnees();
        }


        /// <summary>
        /// Méthode permettant de rafraichir les informations de la Caserne...
        /// </summary>
        private async Task RafraichirInterfaceDonnees()
        {            
            try
            {
                string jsonResponse = await WebAPI.Instance.ExecuteGetAsync("http://" + GetString(Resource.String.host) + ":" + GetString(Resource.String.port) + "/Caserne/ObtenirCaserne?nomCaserne=" + paramNomCaserne);
                laCaserne = JsonConvert.DeserializeObject<CaserneDTO>(jsonResponse);
                lblNomCaserneAfficher.Text = laCaserne.Nom;
                lblAdresseCaserneAfficher.Text = laCaserne.Adresse;
                lblVilleCaserneAfficher.Text = laCaserne.Ville;
                lblProvinceCaserneAfficher.Text = laCaserne.Province;
                lblTelephoneCaserneAfficher.Text = laCaserne.Telephone;
            }
            catch (Exception)
            {

                Finish();
            }
        }


        /// <summary>Méthode de service permettant d'initialiser le menu de l'activité principale.</summary>
        /// <param name="menu">Le menu à construire.</param>
        /// <returns>Retourne True si l'optionMenu est bien créé.</returns>
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.InterfaceCaserneDetails_ActivityMenu, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        /// <summary>Méthode de service permettant de capter l'évenement exécuté lors d'un choix dans le menu.</summary>
        /// <param name="item">L'item sélectionné.</param>
        /// <returns>Retourne si un option à été sélectionné avec succès.</returns>
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.Modifier:
                    Intent activiteModifier = new Intent(this, typeof(CaserneModifierActivity));
                    activiteModifier.PutExtra("paramNomCaserne", laCaserne.Nom);
                    StartActivity(activiteModifier);
                    break;


                case Resource.Id.Supprimer:
                    try
                    {
                        AlertDialog.Builder builder = new AlertDialog.Builder(this);
                        builder.SetPositiveButton("Non", (send, args) => { });
                        builder.SetNegativeButton("Oui", async (send, args) =>
                        {
                            await WebAPI.Instance.PostAsync("http://" + GetString(Resource.String.host) + ":" + GetString(Resource.String.port) + "/Caserne/SupprimerCaserne?nomCaserne=" + paramNomCaserne, null);
                            Finish();
                        });
                        AlertDialog dialog = builder.Create();
                        dialog.SetTitle("Suppression");
                        dialog.SetMessage("Voulez-vous vraiment supprimer la Caserne ?");
                        dialog.Window.SetGravity(GravityFlags.Bottom);
                        dialog.Show();
                    }
                    catch (Exception ex)
                    {
                        DialoguesUtils.AfficherMessageOK(this, "Erreur", ex.Message);
                    }
                    break;

                case Resource.Id.Intervention:
                    Intent activiteFicheIntervention = new Intent(this, typeof(FicheInterventionActivity));
                    activiteFicheIntervention.PutExtra("paramNomCaserne", laCaserne.Nom);
                    StartActivity(activiteFicheIntervention);
                    break;

                case Resource.Id.Pompier:
                    Intent activitePompier = new Intent(this, typeof(PompierActivity));
                    activitePompier.PutExtra("paramNomCaserne", laCaserne.Nom);
                    StartActivity(activitePompier);
                    break;

                case Resource.Id.Retour:
                    Finish();
                    break;
            }
            return base.OnOptionsItemSelected(item);
        }
    }
}
