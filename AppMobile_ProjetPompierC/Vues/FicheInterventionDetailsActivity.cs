using Android.Content;
using Android.Views;
using Newtonsoft.Json;

using AppMobile_ProjetPompierC;
using AppMobile_ProjetPompierC.DTO;
using AppMobile_ProjetPompierC.Utils;
using ProjetPompier_Mobile.Vues;
using Java.Sql;

/// <summary>
/// Namespace pour les classes de type Vue.
/// </summary>
namespace AppMobile_ProjetPompierC.Vues
{
    /// <summary>
    /// Classe de type Activité pour les détails d'un pompier.
    /// </summary>
    [Activity(Label = "@string/app_name")]
    public class FicheInterventionDetailsActivity : Activity
    {
        #region Proprietes

        /// <summary>
        /// Attribut représentant le nom de la caserne.
        /// </summary>
        private string paramNomCaserne;

        /// <summary>
        /// Attribut représentant le matricule du Capitaine.
        /// </summary>
        private int paramMatriculeCapitaine;
        /// <summary>
        /// Attribut représentant la date de début de l'intervention.
        /// </summary>
        private string paramDateDebut;

       /// <summary>
       /// Attribut représentant la fiche d'intervention.
       /// </summary>
        private FicheInterventionDTO laFiche;

        /// <summary>
        /// Attribut représentant le champ d'affichage du type de la fiche..
        /// </summary>
        private TextView lblTypeAfficher;

        /// <summary>
        /// Attribut représentant le champ d'affichage de l'adresse de la fiche.
        /// </summary>
        private TextView lblAdresseAfficher;

        /// <summary>
        /// Attribut représentant le champ d'affichage du résumé de la fiche.
        /// </summary>
        private TextView lblResumeAfficher;

        /// <summary>
        /// Attribut représentant le champ d'affichage de la date de début de la fiche.
        /// </summary>
        private TextView lblDateDebutAfficher;

        #endregion Proprietes
        #region MethodesFicheInterventionDetailsActivity

        /// <summary>
        /// Méthode de service appelée lors de la création de l'activité.
        /// </summary>
        /// <param name="savedInstanceState">État de l'activité.</param>
        protected override void OnCreate(Bundle savedInstanceState)
            {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.InterfaceFicheInterventionDetails);

            lblTypeAfficher = FindViewById<TextView>(Resource.Id.tvTypeAfficher);
            lblResumeAfficher = FindViewById<TextView>(Resource.Id.tvResumeAfficher);
            lblAdresseAfficher = FindViewById<TextView>(Resource.Id.tvAdresseAfficher);
            lblDateDebutAfficher = FindViewById<TextView>(Resource.Id.tvDateDebutAfficher);

            paramNomCaserne = Intent.GetStringExtra("paramNomCaserne");
            paramMatriculeCapitaine = Intent.GetIntExtra("paramMatriculeCapitaine",0);
            paramDateDebut = Intent.GetStringExtra("paramDateDebut");

			
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
        /// Méthode permettant de rafraichir les informations de la fiche...
        /// </summary>
        private async Task RafraichirInterfaceDonnees()
        {
            try
            {
                string jsonResponse = await WebAPI.Instance.ExecuteGetAsync("http://" + GetString(Resource.String.host) + ":" + GetString(Resource.String.port) + "/Intervention/ObtenirFicheIntervention?nomCaserne=" + paramNomCaserne + "&matriculeCapitaine=" + paramMatriculeCapitaine + "&dateIntervention=" + paramDateDebut);
                
				laFiche = JsonConvert.DeserializeObject<FicheInterventionDTO>(jsonResponse);
                lblTypeAfficher.Text = laFiche.TypeIntervention;
                lblAdresseAfficher.Text = laFiche.Adresse;
                lblResumeAfficher.Text = laFiche.Resume;
                lblDateDebutAfficher.Text = laFiche.DateDebut;

            }
            catch (Exception e)
            {
                //Finish();
                
            }
        }

        public async Task AfficherOptionsMenu(IMenu menu)
        {
            await RafraichirInterfaceDonnees();
			if (laFiche.DateFin != "")
            {
				menu.FindItem(Resource.Id.Fermer).SetVisible(false);
				menu.FindItem(Resource.Id.Modifier).SetVisible(false);
			}
		}
      

        #region Menu
        /// <summary>Méthode de service permettant d'initialiser le menu de l'activité principale.</summary>
        /// <param name="menu">Le menu à construire.</param>
        /// <returns>Retourne True si l'optionMenu est bien créé.</returns>
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.FicheInterventionDetailsOptionsMenu, menu);
            AfficherOptionsMenu(menu);
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
                    Intent activiteModifier = new Intent(this, typeof(FicheInterventionModifierActivity));
                    activiteModifier.PutExtra("paramNomCaserne", paramNomCaserne);
                    activiteModifier.PutExtra("paramMatriculeCapitaine", paramMatriculeCapitaine);
                    activiteModifier.PutExtra("paramDateDebut", paramDateDebut);
                    StartActivity(activiteModifier);
                    break;


                case Resource.Id.Fermer:
                    try
                    {
                        AlertDialog.Builder builder = new AlertDialog.Builder(this);
                        builder.SetPositiveButton("Non", (send, args) => { });
                        builder.SetNegativeButton("Oui", async (send, args) =>
                        {
                            laFiche.DateFin = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            await WebAPI.Instance.PostAsync("http://" + GetString(Resource.String.host) + ":" + GetString(Resource.String.port) + "/Intervention/FermerFicheIntervention?nomCaserne=" + paramNomCaserne, laFiche);
                            Finish();
                        });
                        AlertDialog dialog = builder.Create();
                        dialog.SetTitle("Suppression");
                        dialog.SetMessage("Voulez-vous vraiment fermer la fiche " + laFiche.TypeIntervention+ " " + laFiche.Resume+ "?");
                        dialog.Window.SetGravity(GravityFlags.Bottom);
                        dialog.Show();
                    }
                    catch (Exception ex)
                    {
                        DialoguesUtils.AfficherMessageOK(this, "Erreur", ex.Message);
                    }
                    break;

                case Resource.Id.Retour:
                    Finish();
                    break;

                case Resource.Id.Quitter:
                    FinishAffinity();
                    break;
            }
            return base.OnOptionsItemSelected(item);
        }
        
    }
    #endregion Menu
    #endregion MethodesPompierActivity
}
