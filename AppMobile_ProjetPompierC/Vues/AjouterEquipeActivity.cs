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
    [Activity(Label = "@string/app_name")]
    public class AjouterEquipeActivity : Activity
    {
        #region Proprietes
        List<PompierDTO> listePompier;

        ListePompierAdapter adapteurListePompier;

        List<VehiculeDTO> listeVehicule;

        ListeVehiculeAdapter adapteurListeVehicule;

        Spinner spnPompier;

        Spinner spnVehicule;

        ListView listViewPompier;

        Button btnAjouterPompier;

        Button btnAjouterEquipe;

        string paramNomCaserne;

        string paramDateDebut;

        string vinVehichule;

        int paramMatriculeCapitaine;

        int codeVehicule;

        List<PompierDTO> listePompierAjout;

        #endregion Proprietes

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.InterfaceAjouterEquipeActivity);
            // Récupération des paramètres de l'activité précédente
            paramNomCaserne = Intent.GetStringExtra("paramNomCaserne");
            paramDateDebut = Intent.GetStringExtra("paramDateDebut");
            paramMatriculeCapitaine = Intent.GetIntExtra("paramMatriculeCapitaine", 0);

    
            ObtenirListeVehicule();
            spnVehicule = FindViewById<Spinner>(Resource.Id.spnVehiculeInfo);
            spnVehicule.ItemSelected += async (sender, e) =>
            {
                vinVehichule = listeVehicule[e.Position].Vin;
                codeVehicule = listeVehicule[e.Position].Code;
            };

            listePompierAjout = new List<PompierDTO>();

            ObtenirListePompier();
            spnPompier = FindViewById<Spinner>(Resource.Id.spnPompierInfo);

            // Liste view pour afficher les pompiers ajoutés
            listViewPompier = FindViewById<ListView>(Resource.Id.listViewPompierÉquipe);

            // Partie pour ajouter un pompier dans l'équipe
            btnAjouterPompier = FindViewById<Button>(Resource.Id.btnAjouterPompier);
            btnAjouterPompier.Click += async (sender, e) =>
            {
                foreach (PompierDTO pompier in listePompierAjout)
                {
                    if (pompier.Matricule == listePompier[spnPompier.SelectedItemPosition].Matricule)
                    {
                        AlertDialog.Builder alerte = new AlertDialog.Builder(this);
                        alerte.SetTitle("Erreur");
                        alerte.SetMessage("Le pompier a déjà été ajouté à l'équipe.");
                        alerte.SetPositiveButton("OK", (senderAlert, args) =>
                        {
                            alerte.Dispose();
                        });
                        Dialog dialog = alerte.Create();
                        dialog.Show();
                        return;
                    }
                }
                string jsonResponse = await WebAPI.Instance.ExecuteGetAsync("http://" + GetString(Resource.String.host) + ":" + GetString(Resource.String.port) + "/TypesVehicule/ObtenirTypesVehicule?code=" + codeVehicule);
                TypesVehiculeDTO typeVehicule = JsonConvert.DeserializeObject<TypesVehiculeDTO>(jsonResponse);
                if (typeVehicule.Personnes < listePompierAjout.Count + 1)
                {
                    AlertDialog.Builder alerte = new AlertDialog.Builder(this);
                    alerte.SetTitle("Erreur");
                    alerte.SetMessage("Le nombre de pompiers ajoutés est supérieur au nombre de places disponibles dans le véhicule.");
                    alerte.SetPositiveButton("OK", (senderAlert, args) =>
                    {
                        alerte.Dispose();
                    });
                    Dialog dialog = alerte.Create();
                    dialog.Show();
                }
                else
                {
                    listePompierAjout.Add(listePompier[spnPompier.SelectedItemPosition]);
                    RafraichirInterfaceDonnees();
                }
            };

            // Partie pour ajouter l'équipe
            btnAjouterEquipe = FindViewById<Button>(Resource.Id.btnAjouterEquipe);
            btnAjouterEquipe.Click += async (sender, e) =>
            {
                try
                {
                    EquipeDTO equipe = new EquipeDTO
                    {
                        Code = 1,
                        ListePompierEquipe = listePompierAjout,
                        VinVehicule = vinVehichule
                    };
                    await WebAPI.Instance.PostAsync("http://" + GetString(Resource.String.host) + ":" + GetString(Resource.String.port) + "/Equipe/AjouterEquipe?nomCaserne=" + paramNomCaserne + "&matriculeCapitaine=" + paramMatriculeCapitaine + "&dateDebutIntervention=" + paramDateDebut, equipe);
                    AlertDialog.Builder alerte = new AlertDialog.Builder(this);
                    alerte.SetTitle("Succès");
                    alerte.SetMessage("L'équipe a été ajoutée avec succès.");
                    alerte.SetPositiveButton("OK", (senderAlert, args) =>
                    {
                        alerte.Dispose();
                        Finish();
                    });
                    Dialog dialog = alerte.Create();
                    dialog.Show();
                }
                catch (Exception ex)
                {
                    AlertDialog.Builder alerte = new AlertDialog.Builder(this);
                    alerte.SetTitle("Erreur");
                    alerte.SetMessage("Une erreur est survenue lors de l'ajout de l'équipe.");
                    alerte.SetPositiveButton("OK", (senderAlert, args) =>
                    {
                        alerte.Dispose();
                    });
                    Dialog dialog = alerte.Create();
                    dialog.Show();
                }
            };


        }

        /// <summary>
        /// Obtenir la liste des pompiers disponibles.
        /// </summary>
        /// <returns></returns>
        private async Task ObtenirListePompier()
        {
            string jsonResponse = await WebAPI.Instance.ExecuteGetAsync("http://" + GetString(Resource.String.host) + ":" + GetString(Resource.String.port) + "/Pompier/ObtenirListePompierDisponible?nomCaserne=" + paramNomCaserne + "&disponibleSeulement=" + true);
            listePompier = JsonConvert.DeserializeObject<List<PompierDTO>>(jsonResponse);
            adapteurListePompier = new ListePompierAdapter(this, listePompier.ToArray());
            spnPompier.Adapter = adapteurListePompier;
        }

        /// <summary>
        /// Obtenir la liste des véhicules disponibles.
        /// </summary>
        /// <returns></returns>
        private async Task ObtenirListeVehicule()
        {
            string jsonResponse = await WebAPI.Instance.ExecuteGetAsync("http://" + GetString(Resource.String.host) + ":" + GetString(Resource.String.port) + "/Vehicule/ObtenirListeVehicule?nomCaserne=" + paramNomCaserne);
            listeVehicule = JsonConvert.DeserializeObject<List<VehiculeDTO>>(jsonResponse);
            adapteurListeVehicule = new ListeVehiculeAdapter(this, listeVehicule.ToArray());
            spnVehicule.Adapter = adapteurListeVehicule;
        }


        protected void OnResume()
        {
            base.OnResume();
            RafraichirInterfaceDonnees();
        }

        private void RafraichirInterfaceDonnees()
        {
            adapteurListePompier = new ListePompierAdapter(this, listePompierAjout.ToArray());
            listViewPompier.Adapter = adapteurListePompier;
        }

    }
}