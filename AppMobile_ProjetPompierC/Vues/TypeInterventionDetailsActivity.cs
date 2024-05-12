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
namespace AppMobile_ProjetPompierC.Vues
{
    /// <summary>
    /// Classe de type Activité pour la gestion d'un type d'intervention.
    /// </summary>
    [Activity(Label = "@string/app_name")]
    public class TypeInterventionDetailsActivity : Activity
    {
        #region Proprietes
        
        /// <summary>
        /// Attribut représentant le code du type d'intervention.
        /// </summary>
        private int paramCodeTypeIntervention;

        /// <summary>
        ///  Le type d'intervention DTO.
        /// </summary>
        private TypeInterventionDTO leTypeIntervention;

        /// <summary>
        ///  Attrubut représentant le champ d'édition du code du type d'intervention.
        /// </summary>
        private TextView lblCodeTypeInterventionAfficher;

        /// <summary>
        /// Attrubut représentant le champ d'édition de la description du type d'intervention.
        /// </summary>
        private TextView lblDescriptionTypeInterventionAfficher;


        #endregion Proprietes

        #region Methodes
        /// <summary>
        /// Méthode de service appelée lors de la création de l'activité.
        /// </summary>
        /// <param name="savedInstanceState">État de l'activité.</param>
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.InterfaceTypeInterventionDetails);

            // Récupération des paramètres de l'activité
            paramCodeTypeIntervention = Intent.GetIntExtra("CodeTypeIntervention", 0);

            // Récupération des éléments de l'interface
            lblCodeTypeInterventionAfficher = FindViewById<TextView>(Resource.Id.lblCodeTypeInterventionAfficher);
            lblDescriptionTypeInterventionAfficher = FindViewById<TextView>(Resource.Id.lblDescriptionTypeInterventionAfficher);
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
            // Récupération des informations du type d'intervention
            try
            {
                string jsonResponse = await WebAPI.Instance.ExecuteGetAsync("http://" + GetString(Resource.String.host) + ":" + GetString(Resource.String.port) + "/TypesIntervention/ObtenirTypeIntervention?code=" + paramCodeTypeIntervention);
                leTypeIntervention = JsonConvert.DeserializeObject<TypeInterventionDTO>(jsonResponse);
                lblCodeTypeInterventionAfficher.Text = leTypeIntervention.Code.ToString();
                lblDescriptionTypeInterventionAfficher.Text = leTypeIntervention.Description;
            }
            // Affichage d'un message d'erreur en cas d'exception
            catch (Exception ex)
            {
                DialoguesUtils.AfficherToasts(this, "Erreur lors de la récupération du type d'intervention: " + ex.Message);
            }
        }


        /// <summary>Méthode de service permettant d'initialiser le menu de l'activité principale.</summary>
        /// <param name="menu">Le menu à construire.</param>
        /// <returns>Retourne True si l'optionMenu est bien créé.</returns>
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.TypeInterventionDetailsActivityOptionsMenu, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        /// <summary>Méthode de service permettant de capter l'évenement exécuté lors d'un choix dans le menu.</summary>
        /// <param name="item">L'item sélectionné.</param>
        /// <returns>Retourne si un option à été sélectionné avec succès.</returns>
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.SupprimerTypeIntervention:
                    try
                    {
                        AlertDialog.Builder builder = new AlertDialog.Builder(this);
                        builder.SetPositiveButton("Non", (send, args) => { });
                        builder.SetNegativeButton("Oui", async (send, args) =>
                        {
                            await WebAPI.Instance.PostAsync("http://" + GetString(Resource.String.host) + ":" + GetString(Resource.String.port) + "/TypesIntervention/SupprimerTypeIntervention?code=" + paramCodeTypeIntervention, null);
                            DialoguesUtils.AfficherToasts(this, "Type d'intervention supprimé avec succès.");
                            Finish();
                        });
                        builder.SetMessage("Voulez-vous vraiment supprimer ce type d'intervention?");
                        builder.SetTitle("Suppression");
                        builder.Show();
                    }
                    catch (Exception ex)
                    {
                        DialoguesUtils.AfficherToasts(this, "Erreur lors de la suppression du type d'intervention: " + ex.Message);
                    }
                    break;
                case Resource.Id.RetourTypeInterventionActivity:
                    Finish();
                    break;

                case Resource.Id.QuitterTypeInterventionActivity:
                    FinishAffinity();
                    break;
            }
            return base.OnOptionsItemSelected(item);
        }
        #endregion Methodes
    }
}
