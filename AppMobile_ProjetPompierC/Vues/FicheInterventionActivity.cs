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
        /// <summary>
        /// Attribut représentant le nom de la caserne.
        /// </summary>
        private string paramNomCaserne;
        /// <summary>
        /// Attribut représentant le matricule du capitaine.
        /// </summary>
        int matriculeCapitaine;
        /// <summary>
        /// Attribut représentant la liste des fiches d'intervention.
        /// </summary>
        List<FicheInterventionDTO> listeFicheIntervention;
        /// <summary>
        /// Attribut représentant l'adapteur pour la liste des fiches d'intervention.
        /// </summary>
        private ListeInterventionAdapter adapteurListeFicheIntervention;
        /// <summary>
        /// Attribut représentant la liste des pompiers.
        /// </summary>
        private List<PompierDTO> listePompier;
        /// <summary>
        /// Attribut représentant l'adapteur pour la liste des pompiers.
        /// </summary>
        private ListePompierAdapter adapteurListePompier;
        /// <summary>
        /// Attribut représentant le champ d'édition de l'adresse de l'intervention.
        /// </summary>
        EditText edtAdresse;
        /// <summary>
        /// Attribut représentant le champ d'édition du type d'intervention.
        /// </summary>
        EditText edtTypeIntervention;
        /// <summary>
        /// Attribut représentant le champ d'édition du résumé de l'intervention.
        /// </summary>
        EditText edtResume;
        /// <summary>
        /// Attribut représentant le spinner pour la liste des capitaines.
        /// </summary>
        Spinner spnCapitaine;
        /// <summary>
        /// Attribut représentant le bouton pour ouvrir une fiche d'intervention.
        /// </summary>
        Button btnOuvrirFicheIntervention;
        /// <summary>
        /// Attribut représentant la liste des fiches d'intervention.
        /// </summary>
        ListView listViewFicheIntervention;


        /// <summary>
        /// Méthode de service appelée lors de la création de l'activité.
        /// </summary>
        /// <param name="savedInstanceState">État de l'activité.</param>
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.InterfaceFicheInterventionActivity);

            paramNomCaserne = Intent.GetStringExtra("paramNomCaserne");

            edtAdresse = FindViewById<EditText>(Resource.Id.edtAdresseInterventionInfo);
            edtTypeIntervention = FindViewById<EditText>(Resource.Id.edtTypeInterventionInfo);
            edtResume = FindViewById<EditText>(Resource.Id.edtResumeInfo);

            listeFicheIntervention = new List<FicheInterventionDTO>(); 
            listePompier = new List<PompierDTO>();

			
            ObtenirListePompiers();


			spnCapitaine = FindViewById<Spinner>(Resource.Id.spnCapitaineInfo);
            // Événement de sélection d'un capitaine dans la liste
            spnCapitaine.ItemSelected += async (sender, e) =>
            {
                // Obtenir la liste des fiches d'intervention du capitaine sélectionné
                matriculeCapitaine = listePompier[e.Position].Matricule;
				RafraichirInterfaceDonnees();

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
                            DateDebut = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss"),
                            DateFin = null,
                            Adresse = edtAdresse.Text,
                            TypeIntervention = edtTypeIntervention.Text,
                            Resume = edtResume.Text,
                            MatriculeCapitaine = matriculeCapitaine
                        };


                        // Envoi de la fiche à l'API
                        await WebAPI.Instance.PostAsync("http://" + GetString(Resource.String.host) + ":" + GetString(Resource.String.port) + "/Intervention/OuvrirFicheIntervention?nomCaserne=" + paramNomCaserne, fiche);
                        DialoguesUtils.AfficherToasts(this, "Fiche d'intervention ouverte !!!");
                        RafraichirInterfaceDonnees();
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
			listViewFicheIntervention.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) =>
			{
				Intent activiteFicheInterventionDetails = new Intent(this, typeof(FicheInterventionDetailsActivity));
				//On initialise les paramètres avant de lancer la nouvelle activité.
				activiteFicheInterventionDetails.PutExtra("paramNomCaserne", paramNomCaserne);
                activiteFicheInterventionDetails.PutExtra("paramMatriculeCapitaine", matriculeCapitaine);
                activiteFicheInterventionDetails.PutExtra("paramDateDebut", listeFicheIntervention[e.Position].DateDebut);
				//On démarre la nouvelle activité.
				StartActivity(activiteFicheInterventionDetails);
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
       /// Fonction permettant d'obtenir la liste des pompiers.
       /// </summary>
       /// <returns></returns>
        private async Task ObtenirListePompiers()
        {
			// Obtenir la liste des pompiers.
			string jsonResponsePompier = await WebAPI.Instance.ExecuteGetAsync("http://" + GetString(Resource.String.host) + ":" + GetString(Resource.String.port) + "/Pompier/ObtenirListePompier?nomCaserne=" + paramNomCaserne + "&seulementCapitaine=" + true);
			listePompier = JsonConvert.DeserializeObject<List<PompierDTO>>(jsonResponsePompier);
			adapteurListePompier = new ListePompierAdapter(this, listePompier.ToArray());
            spnCapitaine.Adapter = adapteurListePompier;

		}

        /// <summary>
        /// Méthode permettant de rafraichir la liste des Pompiers...
        /// </summary>
        private async Task RafraichirInterfaceDonnees()
        {
            try
            {
				string jsonResponseIntevention = await WebAPI.Instance.ExecuteGetAsync("http://" + GetString(Resource.String.host) + ":" + GetString(Resource.String.port) + "/Intervention/ObtenirListeFicheIntervention?nomCaserne=" + paramNomCaserne + "&matriculeCapitaine=" + matriculeCapitaine);
				listeFicheIntervention = JsonConvert.DeserializeObject<List<FicheInterventionDTO>>(jsonResponseIntevention);
				adapteurListeFicheIntervention = new ListeInterventionAdapter(this, listeFicheIntervention.ToArray());
				listViewFicheIntervention.Adapter = adapteurListeFicheIntervention;


				if (listeFicheIntervention.Count > 0)
                {
					if (listeFicheIntervention[listeFicheIntervention.Count - 1].DateFin == "")
					{
						btnOuvrirFicheIntervention.Enabled = false;
					}
					else
					{
						btnOuvrirFicheIntervention.Enabled = true;
					}
				}
                
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
            MenuInflater.Inflate(Resource.Menu.FicheInterventionActivityOptionsMenu, menu);
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