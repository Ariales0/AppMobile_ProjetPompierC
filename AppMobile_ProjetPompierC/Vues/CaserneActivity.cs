using Android.Content;
using Android.Views;
using Newtonsoft.Json;
using AlertDialog = Android.App.AlertDialog;

using AppMobile_ProjetPompierC;
using AppMobile_ProjetPompierC.Adapters;
using AppMobile_ProjetPompierC.DTO;
using AppMobile_ProjetPompierC.Utils;


namespace AppMobile_ProjetPompierC.Vues
{
    [Activity(Label = "@string/app_name", MainLauncher = true)]
    public class CaserneActivity : Activity
    {

        /// <summary>
        /// Liste des casernes.
        /// </summary>
        private List<CaserneDTO> listeCaserne;

        /// <summary>
        /// Adapter pour la liste des casernes.
        /// </summary>
        private ListeCaserneAdapter adapteurListeCaserne;
        
        /// <summary>
        /// ListView pour afficher les casernes.
        /// </summary>
        private ListView listViewCaserne;

        /// <summary>
        /// Attribut représentant le champ d'édition du nom du Cégep pour l'ajout d'une caserne.
        /// </summary>
        private EditText edtNomCaserne;

        /// <summary>
        /// Attribut représentant le champ d'édition de l'adresse de la caserne pour l'ajout d'une caserne.
        /// </summary>
        private EditText edtAdresseCaserne;

        /// <summary>
        /// Attribut représentant le champ d'édition de la ville de la caserne pour l'ajout d'une caserne.
        /// </summary>
        private EditText edtVilleCaserne;

        /// <summary>
        /// Attribut représentant le champ d'édition de province de la caserne pour l'ajout d'une caserne.
        /// </summary>
        private EditText edtProvinceCaserne;

        /// <summary>
        /// Attribut représentant le champ d'édition du telephone de la caserne pour l'ajout d'une caserne.
        /// </summary>
        private EditText edtTelephoneCaserne;

        /// <summary>
        /// Bouton pour ajouter une caserne.
        /// </summary>
        private Button btnAjouterCaserne;

        /// <summary>
        /// Méthode de service appelée lors de la création de l'activité.
        /// </summary>
        /// <param name="savedInstanceState">État de l'activité.</param>
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.InterfaceCaserneActivity);

            listViewCaserne = FindViewById<ListView>(Resource.Id.listViewCaserne);
            listViewCaserne.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) =>
            {
                Intent activiteCaserneDetails = new Intent(this, typeof(CaserneDetailsActivity));
                //On initialise les paramètres avant de lancer la nouvelle activité.
                activiteCaserneDetails.PutExtra("paramNomCaserne", listeCaserne[e.Position].Nom);
                //On démarre la nouvelle activité.
                StartActivity(activiteCaserneDetails);
            };

            edtNomCaserne = FindViewById<EditText>(Resource.Id.edtNomInfo);
            edtAdresseCaserne = FindViewById<EditText>(Resource.Id.edtAdresseInfo);
            edtVilleCaserne = FindViewById<EditText>(Resource.Id.edtVilleInfo);
            edtProvinceCaserne = FindViewById<EditText>(Resource.Id.edtProvinceInfo);
            edtTelephoneCaserne = FindViewById<EditText>(Resource.Id.edtTelephoneInfo);

            btnAjouterCaserne = FindViewById<Button>(Resource.Id.btnAjouter);
            btnAjouterCaserne.Click += async (sender, e) =>
            {
                if ((edtNomCaserne.Text.Length > 0) && (edtAdresseCaserne.Text.Length > 0) && (edtVilleCaserne.Text.Length > 0) && (edtProvinceCaserne.Text.Length > 0) && (edtTelephoneCaserne.Text.Length > 0))
                {
                    try
                    {
                        string nom = edtNomCaserne.Text;
                        CaserneDTO caserneDTO = new CaserneDTO
                        {
                            Nom = nom,
                            Adresse = edtAdresseCaserne.Text,
                            Ville = edtVilleCaserne.Text,
                            Province = edtProvinceCaserne.Text,
                            Telephone = edtTelephoneCaserne.Text
                        };
                        await WebAPI.Instance.PostAsync("http://" + GetString(Resource.String.host) + ":" + GetString(Resource.String.port) + "/Caserne/AjouterCaserne", caserneDTO);
                        await RafraichirInterfaceDonnees();
                        DialoguesUtils.AfficherToasts(this, nom + " ajouté !!!");
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
        protected override async void OnResume()
        {
            base.OnResume();
            await RafraichirInterfaceDonnees();
        }

        /// <summary>
        /// Méthode permettant de rafraichir la liste des Casernes...
        /// </summary>
        private async Task RafraichirInterfaceDonnees()
        {
            try
            {
                string jsonResponse = await WebAPI.Instance.ExecuteGetAsync("http://" + GetString(Resource.String.host) + ":" + GetString(Resource.String.port) + "/Caserne/ObtenirListeCaserne");
                listeCaserne = JsonConvert.DeserializeObject<List<CaserneDTO>>(jsonResponse);
                adapteurListeCaserne = new ListeCaserneAdapter(this, listeCaserne.ToArray());
                listViewCaserne.Adapter = adapteurListeCaserne;
                edtNomCaserne.Text = edtAdresseCaserne.Text = edtVilleCaserne.Text = edtProvinceCaserne.Text = edtTelephoneCaserne.Text = "";   
                edtNomCaserne.RequestFocus();
            }
            catch (Exception ex)
            {
                DialoguesUtils.AfficherMessageOK(this, "Erreur", ex.Message);
            }
        }

        /// <summary>Méthode de service permettant d'initialiser le menu de l'activité.</summary>
        /// <param name="menu">Le menu à construire.</param>
        /// <returns>Retourne True si l'optionMenu est bien créé.</returns>
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.CaserneActivityOptionsMenu, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        /// <summary>Méthode de service permettant de capter l'évenement exécuté lors d'un choix dans le menu.</summary>
        /// <param name="item">L'item sélectionné.</param>
        /// <returns>Retourne si une option à été sélectionné avec succès.</returns>
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)    
            {
                case Resource.Id.Vider:
                    try
                    {
                        AlertDialog.Builder builder = new AlertDialog.Builder(this);
                        builder.SetPositiveButton("Non", (send, args) => { });
                        builder.SetNegativeButton("Oui", async (send, args) =>
                        {
                            await WebAPI.Instance.PostAsync("http://" + GetString(Resource.String.host) + ":" + GetString(Resource.String.port) + "/Caserne/ViderListeCaserne", null);
                            DialoguesUtils.AfficherToasts(this, "Tous les grades sont supprimés");
                            await RafraichirInterfaceDonnees();
                        });
                        AlertDialog dialog = builder.Create();
                        dialog.SetTitle("Suppression");
                        dialog.SetMessage("Voulez-vous vraiment vider la liste des casernes ?");
                        dialog.Window.SetGravity(GravityFlags.Bottom);
                        dialog.Show();
                    }
                    catch (Exception ex)
                    {

                        DialoguesUtils.AfficherMessageOK(this, "Erreur", ex.Message);
                    }
                    break;

                case Resource.Id.Grade:
                    Intent activiteGrade = new Intent(this, typeof(GradeActivity));
                    //On démarre la nouvelle activité.
                    StartActivity(activiteGrade);
                    break;

                case Resource.Id.Quitter:
                    Finish();
                    break;
            }
            return base.OnOptionsItemSelected(item);
        }
    }
}