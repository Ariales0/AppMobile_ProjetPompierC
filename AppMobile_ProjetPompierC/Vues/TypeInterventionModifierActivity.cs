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
    public class TypeInterventionModifierActivity : Activity
    {
        #region Proprietes
        
        /// <summary>
        /// Attribut représentant le code du type d'intervention.
        /// </summary>
        private int paramCodeTypeIntervention;

        /// <summary>
        /// Attrubut représentant le type d'intervention DTO.
        /// </summary>
        private TypeInterventionDTO leTypeIntervention;

        /// <summary>
        /// Attrubut représentant le champ d'édition du code du type d'intervention.
        /// </summary>
        private EditText edtCodeTypeIntervention;

        /// <summary>
        /// Attrubut représentant le champ d'édition de la description du type d'intervention.
        /// </summary>
        private EditText edtDescriptionTypeIntervention;

        /// <summary>
        /// Bouton pour modifier un type d'intervention.
        /// </summary>
        private Button btnModifierTypeIntervention;

        #endregion Proprietes

        #region Methodes
        /// <summary>
        /// Méthode de service appelée lors de la création de l'activité.
        /// </summary>
        /// <param name="savedInstanceState">État de l'activité.</param>
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.InterfaceTypeInterventionModifier);

            // Récupération des paramètres de l'activité
            paramCodeTypeIntervention = Intent.GetIntExtra("CodeTypeIntervention", 0);

            // Récupération des éléments de l'interface
            edtCodeTypeIntervention = FindViewById<EditText>(Resource.Id.edtCodeModifier);

            edtDescriptionTypeIntervention = FindViewById<EditText>(Resource.Id.edtDescriptionModifier);

            btnModifierTypeIntervention = FindViewById<Button>(Resource.Id.btnModifierTypeIntervention);
            btnModifierTypeIntervention.Click += async (sender, e) =>
            {
                // Modification du type d'intervention
                if((edtDescriptionTypeIntervention.Text.Length > 0) && (edtCodeTypeIntervention.Text.Length > 0))
                {
                    TypeInterventionDTO typeIntervention = new TypeInterventionDTO
                    {
                        Code = int.Parse(edtCodeTypeIntervention.Text),
                        Description = edtDescriptionTypeIntervention.Text
                    };
                    try
                    {
                        await WebAPI.Instance.PostAsync("http://" + GetString(Resource.String.host) + ":" + GetString(Resource.String.port) + "/TypesIntervention/ModifierTypeIntervention", typeIntervention);
                        DialoguesUtils.AfficherToasts(this, "Type d'intervention modifié avec succès.");
                        Finish();
                    }
                    catch (Exception ex)
                    {
                        DialoguesUtils.AfficherToasts(this, "Erreur lors de la modification du type d'intervention: " + ex.Message);
                    }
                }
                else
                {
                    DialoguesUtils.AfficherToasts(this, "Veuillez modifier une valeur.");
                }

            };


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
                edtCodeTypeIntervention.Text = leTypeIntervention.Code.ToString();
                edtDescriptionTypeIntervention.Text = leTypeIntervention.Description;
                edtDescriptionTypeIntervention.RequestFocus();
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
            MenuInflater.Inflate(Resource.Menu.TypeInterventionModifierActivityOptionsMenu, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        /// <summary>Méthode de service permettant de capter l'évenement exécuté lors d'un choix dans le menu.</summary>
        /// <param name="item">L'item sélectionné.</param>
        /// <returns>Retourne si un option à été sélectionné avec succès.</returns>
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                // Retour à l'activité précédente
                case Resource.Id.RetourTypeInterventionActivity:
                    Finish();
                    break;
                // Quitter l'application
                case Resource.Id.QuitterTypeInterventionActivity:
                    FinishAffinity();
                    break;
            }
            return base.OnOptionsItemSelected(item);
        }
        #endregion Methodes
    }
}
