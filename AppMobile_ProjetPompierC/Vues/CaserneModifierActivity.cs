using Android.Content;
using Android.Views;
using Newtonsoft.Json;

using AppMobile_ProjetPompierC;
using AppMobile_ProjetPompierC.DTO;
using AppMobile_ProjetPompierC.Utils;

/// <summary>
/// Namespace pour les classes de type Vue.
/// </summary>
namespace AppMobile_ProjetPompierC.Vues
{
    /// <summary>
    /// Classe de type Activité pour la modification d'un Cégep.
    /// </summary>
    [Activity(Label = "@string/app_name")]
    public class CaserneModifierActivity : Activity
    {
        private string paramNomCaserne;

        private CaserneDTO laCaserne;

        private EditText edtNomCaserne;

        private EditText edtAdresseCaserne;

        private EditText edtVilleCaserne;

        private EditText edtProvinceCaserne;

        private EditText edtTelephoneCaserne;

        private Button btnModifierCaserne;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.InterfaceCaserneModifier_Activity);

            paramNomCaserne = Intent.GetStringExtra("paramNomCaserne");

            edtNomCaserne = FindViewById<EditText>(Resource.Id.edtNomModifier);
            edtAdresseCaserne = FindViewById<EditText>(Resource.Id.edtAdresseModifier);
            edtVilleCaserne = FindViewById<EditText>(Resource.Id.edtVilleModifier);
            edtProvinceCaserne = FindViewById<EditText>(Resource.Id.edtProvinceModifier);
            edtTelephoneCaserne = FindViewById<EditText>(Resource.Id.edtTelephoneModifier);

            btnModifierCaserne = FindViewById<Button>(Resource.Id.btnModifier);
            btnModifierCaserne.Click += async (sender, e) =>
            {
                if ((edtNomCaserne.Text.Length > 0) && (edtAdresseCaserne.Text.Length > 0) && (edtVilleCaserne.Text.Length > 0) && (edtProvinceCaserne.Text.Length > 0) && (edtTelephoneCaserne.Text.Length > 0))
                {
                    try
                    {
                        CaserneDTO caserneDTO = new CaserneDTO
                        {
                            Nom = edtNomCaserne.Text,
                            Adresse = edtAdresseCaserne.Text,
                            Ville = edtVilleCaserne.Text,
                            Province = edtProvinceCaserne.Text,
                            Telephone = edtTelephoneCaserne.Text
                        };
                        await WebAPI.Instance.PostAsync("http://" + GetString(Resource.String.host) + ":" + GetString(Resource.String.port) + "/Caserne/ModifierCaserne", caserneDTO);
                        DialoguesUtils.AfficherToasts(this, paramNomCaserne + " modifiée !!!");
                    }
                    catch (Exception ex)
                    {

                        DialoguesUtils.AfficherMessageOK(this, "Erreur", ex.Message);
                    }

                }
                else
                {
                    DialoguesUtils.AfficherMessageOK(this, "Erreur", "Veuillez remplir tous les champs...");
                }
            };
        }

        /// <summary>
        /// Méthode de service appelée lors du retour en avant plan de l'activité.
        /// </summary>
        protected async override void OnResume()
        {
            base.OnResume();
            await RafraichirInterfaceDonnees();
        }

        /// <summary>
        /// Méthode permettant de rafraichir les informations du Cégep...
        /// </summary>
        private async Task RafraichirInterfaceDonnees()
        {
            try
            {
                string jsonResponse = await WebAPI.Instance.ExecuteGetAsync("http://" + GetString(Resource.String.host) + ":" + GetString(Resource.String.port) + "/Caserne/ObtenirCaserne?nomCaserne=" + paramNomCaserne);
                laCaserne = JsonConvert.DeserializeObject<CaserneDTO>(jsonResponse);
                edtNomCaserne.Text = laCaserne.Nom;
                edtAdresseCaserne.Text = laCaserne.Adresse;
                edtVilleCaserne.Text = laCaserne.Ville;
                edtProvinceCaserne.Text = laCaserne.Province;
                edtTelephoneCaserne.Text = laCaserne.Telephone;

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
            MenuInflater.Inflate(Resource.Menu.InterfaceCaserneModifier_ActivityMenu, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        /// <summary>Méthode de service permettant de capter l'évenement exécuté lors d'un choix dans le menu.</summary>
        /// <param name="item">L'item sélectionné.</param>
        /// <returns>Retourne si un option à été sélectionné avec succès.</returns>
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.Retour:
                    Finish();
                    break;
            }
            return base.OnOptionsItemSelected(item);
        }
    }
}