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
    /// Classe de type Activité pour les détails d'un pompier.
    /// </summary>
    [Activity(Label = "@string/app_name")]
    public class PompierDetailsActivity : Activity
    {
        #region Proprietes

        /// <summary>
        /// Attribut représentant le nom de la caserne.
        /// </summary>
        private string paramNomCaserne;

        /// <summary>
        /// Attribut représentant le matricule du pompier.
        /// </summary>
        private int paramMatriculePompier;

        /// <summary>
        /// Attribut représentant le pompier en objet PompierDTO.
        /// </summary>
        private PompierDTO lePompier;

        /// <summary>
        /// Attribut représentant le champ d'affichage du matricule du pompier.
        /// </summary>
        private TextView lblMatriculePompierAfficher;

        /// <summary>
        /// Attribut représentant le champ d'affichage du grade du pompier.
        /// </summary>
        private TextView lblGradePompierAfficher;

        /// <summary>
        /// Attribut représentant le champ d'affichage du nom du pompier.
        /// </summary>
        private TextView lblNomPompierAfficher;

        /// <summary>
        /// Attribut représentant le champ d'affichage du prenom du pompier.
        /// </summary>
        private TextView lblPrenomPompierAfficher;

        #endregion Proprietes
        #region MethodesPompierActivity

        /// <summary>
        /// Méthode de service appelée lors de la création de l'activité.
        /// </summary>
        /// <param name="savedInstanceState">État de l'activité.</param>
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.InterfacePompierDetails);

            lblMatriculePompierAfficher = FindViewById<TextView>(Resource.Id.tvMatriculePompierDetails);
            lblGradePompierAfficher = FindViewById<TextView>(Resource.Id.tvGradePompierDetails);
            lblNomPompierAfficher = FindViewById<TextView>(Resource.Id.tvNomPompierDetails);
            lblPrenomPompierAfficher = FindViewById<TextView>(Resource.Id.tvPrenomPompierDetails);

            paramNomCaserne = Intent.GetStringExtra("paramNomCaserne");
            paramMatriculePompier = Intent.GetIntExtra("paramMatriculePompier",0);
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
                string jsonResponse = await WebAPI.Instance.ExecuteGetAsync("http://" + GetString(Resource.String.host) + ":" + GetString(Resource.String.port) + "/Pompier/ObtenirPompier?nomCaserne=" + paramNomCaserne +" &matriculePompier=" + paramMatriculePompier);
                lePompier = JsonConvert.DeserializeObject<PompierDTO>(jsonResponse);
                lblMatriculePompierAfficher.Text = lePompier.Matricule.ToString();
                lblGradePompierAfficher.Text = lePompier.Grade;
                lblNomPompierAfficher.Text = lePompier.Nom;
                lblPrenomPompierAfficher.Text = lePompier.Prenom;
            }
            catch (Exception)
            {
                Finish();
            }
        }

        #region Menu
        /// <summary>Méthode de service permettant d'initialiser le menu de l'activité principale.</summary>
        /// <param name="menu">Le menu à construire.</param>
        /// <returns>Retourne True si l'optionMenu est bien créé.</returns>
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.PompierDetailsOptionsMenu, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        /// <summary>Méthode de service permettant de capter l'évenement exécuté lors d'un choix dans le menu.</summary>
        /// <param name="item">L'item sélectionné.</param>
        /// <returns>Retourne si un option à été sélectionné avec succès.</returns>
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.ModifierPompierDetails:
                    Intent activiteModifier = new Intent(this, typeof(PompierModifierActivity));
                    activiteModifier.PutExtra("paramNomCaserne", paramNomCaserne);
                    activiteModifier.PutExtra("paramMatriculePompier", paramMatriculePompier);
                    StartActivity(activiteModifier);
                    break;


                case Resource.Id.SupprimerPompierDetails:
                    try
                    {
                        bool reponse = DialoguesUtils.AfficherDialogueQuestionOuiNon(this, "Suppression", "Voulez-vous vraiment supprimer le pompier " + lePompier.Nom + " " + lePompier.Prenom + "?");
                        if (reponse)
                        {
                            async Task SupprimerPompierAsync()
                            {
                                await WebAPI.Instance.PostAsync("http://" + GetString(Resource.String.host) + ":" + GetString(Resource.String.port) + "/Pompier/SupprimerPompier?matriculePompier=" + paramMatriculePompier + "&nomCaserne=" + paramNomCaserne, null);
                                Finish();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        DialoguesUtils.AfficherMessageOK(this, "Erreur", ex.Message);
                    }
                    break;

                case Resource.Id.RetourPompierDetails:
                    Finish();
                    break;

                case Resource.Id.QuitterPompierDetails:
                    FinishAffinity();
                    break;
            }
            return base.OnOptionsItemSelected(item);
        }
        
    }
    #endregion Menu
    #endregion MethodesPompierActivity
}
