using Android.Content;
using Android.Views;
using Newtonsoft.Json;

using AppMobile_ProjetPompierC;
using AppMobile_ProjetPompierC.Adapters;
using AppMobile_ProjetPompierC.DTO;
using AppMobile_ProjetPompierC.Utils;

namespace AppMobile_ProjetPompierC.Vues
{
    [Activity(Label = "@string/app_name")]
    public class FicheInterventionActivity : Activity
    {
        private string paramNomCaserne;
        int matriculeCapitaine;
        private List<FicheInterventionDTO> listeFicheIntervention;
        private ListeInterventionAdapter adapteurListeFicheIntervention;
        private List<PompierDTO> listePompier;
        private ListePompierAdapter adapteurListePompier;
        EditText edtAdresse;
        EditText edtTypeIntervention;
        EditText edtResume;
        Spinner spnCapitaine;
        Button btnOuvrirFicheIntervention;
        ListView listViewFicheIntervention;


        /// <summary>
        /// Méthode de service appelée lors de la création de l'activité.
        /// </summary>
        /// <param name="savedInstanceState">État de l'activité.</param>
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.InterfaceFicheIntervention_Activity);

            paramNomCaserne = Intent.GetStringExtra("paramNomCaserne");

            edtAdresse = FindViewById<EditText>(Resource.Id.edtAdresseInterventionInfo);
            edtTypeIntervention = FindViewById<EditText>(Resource.Id.edtTypeInterventionInfo);
            edtResume = FindViewById<EditText>(Resource.Id.edtResumeInfo);



            spnCapitaine = FindViewById<Spinner>(Resource.Id.spnCapitaineInfo);
            // Événement de sélection d'un capitaine dans la liste
            spnCapitaine.ItemSelected += async (sender, e) =>
            {
                // Obtenir la liste des fiches d'intervention du capitaine sélectionné
                matriculeCapitaine = listePompier[e.Position].Matricule;
                string jsonResponseIntevention = await WebAPI.Instance.ExecuteGetAsync("http://" + GetString(Resource.String.host) + ":" + GetString(Resource.String.port) + "/Intervention/ObtenirListeFicheIntervention?nomCaserne=" + paramNomCaserne + "&matriculeCapitaine=" + matriculeCapitaine);
                listeFicheIntervention = JsonConvert.DeserializeObject<List<FicheInterventionDTO>>(jsonResponseIntevention);
                adapteurListeFicheIntervention = new ListeInterventionAdapter(this, listeFicheIntervention.ToArray());
                listViewFicheIntervention.Adapter = adapteurListeFicheIntervention;
            };

            btnOuvrirFicheIntervention = FindViewById<Button>(Resource.Id.btnOuvrir);
            // Événement de clic sur le bouton pour ouvrir une fiche d'intervention
            btnOuvrirFicheIntervention.Click += async (sender, e) =>
            {
                // Vérifier que les propriétés de la fiche ne sont pas null
                if (edtAdresse.Text.Length > 0 && edtTypeIntervention.Text.Length > 0 && edtAdresse.Text.Length > 0)  
                {
                    try
                    {
                        // Créer une nouvelle fiche d'intervention uniquement si les champs ne sont pas vides
                        FicheInterventionDTO fiche = new FicheInterventionDTO()
                        {
                            DateTemps = DateTime.Now.ToString(),
                            Adresse = edtAdresse.Text,
                            TypeIntervention = edtTypeIntervention.Text,
                            Resume = edtResume.Text,
                            MatriculeCapitaine = matriculeCapitaine
                        };


                        // Envoi de la fiche à l'API
                        await WebAPI.Instance.PostAsync("http://" + GetString(Resource.String.host) + ":" + GetString(Resource.String.port) + "/Intervention/OuvrirFicheIntervention?nomCaserne=" + paramNomCaserne, fiche);
                        DialoguesUtils.AfficherToasts(this, "Fiche d'intervention ouverte !!!");

                    }
                    // Gestion des erreurs lors de l'ouverture de la fiche
                    catch (Exception ex)
                    {
                        DialoguesUtils.AfficherMessageOK(this, "Erreur", ex.Message);
                    }
                }
                else
                {
                    // Afficher un message d'erreur si un des champs est vide
                    DialoguesUtils.AfficherMessageOK(this, "Erreur", "Veuillez remplir tous les champs...");
                }
            };

            listViewFicheIntervention = FindViewById<ListView>(Resource.Id.listViewIntervention);
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
        /// Méthode permettant de rafraichir la liste des Pompiers...
        /// </summary>
        private async Task RafraichirInterfaceDonnees()
        {
            try
            {
                // Obtenir la liste des pompiers
                string jsonResponsePompier = await WebAPI.Instance.ExecuteGetAsync("http://" + GetString(Resource.String.host) + ":" + GetString(Resource.String.port) + "/Pompier/ObtenirListePompier?nomCaserne=" + paramNomCaserne + "&seulementCapitaine=" + true);
                listePompier = JsonConvert.DeserializeObject<List<PompierDTO>>(jsonResponsePompier);
                adapteurListePompier = new ListePompierAdapter(this, listePompier.ToArray());
                spnCapitaine.Adapter = adapteurListePompier;

            }
            catch (Exception ex)
            {

                DialoguesUtils.AfficherMessageOK(this, "Erreur", ex.Message);
            }
        }

        /// <summary>Méthode de service permettant d'initialiser le menu de l'activité principale.</summary>
        /// <param name="menu">Le menu à construire.</param>
        /// <returns>Retourne True si l'optionMenu est bien créé.</returns>
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.InterfaceFicheIntervention_ActivityMenu, menu);
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