using Android.Content;
using Android.Views;
using Newtonsoft.Json;

using AppMobile_ProjetPompierC;
using AppMobile_ProjetPompierC.DTO;
using AppMobile_ProjetPompierC.Utils;
using AppMobile_ProjetPompierC.Adapters;

/// <summary>
/// Namespace pour les classes de type Vue.
/// </summary>
namespace ProjetPompier_Mobile.Vues
{
    /// <summary>
    /// Classe de type Activité pour la modification d'un véhicule.
    /// </summary>
    [Activity(Label = "@string/app_name")]
    public class VehiculeModifierActivity : Activity
    {
        #region Proprietes

        /// <summary>
        /// Attribut représentant le nom de la caserne.
        /// </summary>
        private string paramNomCaserne;

        /// <summary>
        /// Attribut représentant le vin du véhicule.
        /// </summary>
        private string paramVinVehicule;

        /// <summary>
        /// Liste des type de véhicule.
        /// </summary>
        private List<TypeVehiculeDTO> listeTypeVehicule;

        /// <summary>
        /// Adapter pour la liste des types.
        /// </summary>
        private ListeTypeVehiculeAdapter adapteurListeTypeVehicule;

        /// <summary>
        /// Attribut représentant le véhicule en objet VehiculeDTO.
        /// </summary>
        private VehiculeDTO leVehicule;

        /// <summary>
        /// Attribut représentant le champ d'affichage du VIN du véhicule.
        /// </summary>
        private TextView edtVinVehiculeModifier;

        /// <summary>
        /// Liste deroulante qui contient les types.
        /// </summary>
        private Spinner spinnerTypeVehicule;

        /// <summary>
        /// Type séléctionné dans le spinner.
        /// </summary>
        string typeSelectionne;

        /// <summary>
        /// Attribut représentant le champ d'affichage de la marque du véhicule.
        /// </summary>
        private TextView edtMarqueVehiculeModifier;

        /// <summary>
        /// Attribut représentant le champ d'affichage du modèle du véhicule.
        /// </summary>
        private TextView edtModeleVehiculeModifier;

		// <summary>
		/// Attribut représentant le champ d'affichage de l'année du véhicule.
		/// </summary>
		private TextView edtAnneeVehiculeModifier;

		/// <summary>
		/// Bouton pour ajouter un véhicule dans la caserne.
		/// </summary>
		private Button btnModifierVehicule;

        #endregion Proprietes

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.InterfaceVehiculeModifier);

            paramNomCaserne = Intent.GetStringExtra("paramNomCaserne");
            paramVinVehicule = Intent.GetStringExtra("paramVinVehicule");

            edtVinVehiculeModifier = FindViewById<TextView>(Resource.Id.tvVinVehiculeModifier);
            edtMarqueVehiculeModifier = FindViewById<TextView>(Resource.Id.edtMarqueVehiculeModifier); 
            edtModeleVehiculeModifier = FindViewById<TextView>(Resource.Id.edtModeleVehiculeModifier);
			edtAnneeVehiculeModifier = FindViewById<TextView>(Resource.Id.edtAnneeVehiculeModifier);

			spinnerTypeVehicule = FindViewById<Spinner>(Resource.Id.spTypeVehiculeModifier);
            
            spinnerTypeVehicule.ItemSelected += (sender, e) =>
            {
                TypeVehiculeDTO typeVehiculeDTOSelecionne;
                typeVehiculeDTOSelecionne = listeTypeVehicule[e.Position];
                typeSelectionne = typeVehiculeDTOSelecionne.Type;
				
			};

            btnModifierVehicule = FindViewById<Button>(Resource.Id.btnModifierVehicule);
            btnModifierVehicule.Click += async (sender, e) =>
            {
                // DEMANDER À KEVEN SI ON PEUT MODIFIER LE TYPE D'UN VÉHICULE
                if ((edtMarqueVehiculeModifier.Text.Length > 0) && (edtModeleVehiculeModifier.Text.Length > 0) && (int.Parse(edtAnneeVehiculeModifier.Text) > 0))
                {
                    try
                    {
                        string leVehiculeModifier = leVehicule.VinVehicule + " " + edtModeleVehiculeModifier.Text + " " + edtAnneeVehiculeModifier.Text;

                        VehiculeDTO vehiculeDTO = new VehiculeDTO
                        {
                            VinVehicule = paramVinVehicule,
                            Type = typeSelectionne,
                            Marque = edtMarqueVehiculeModifier.Text,
                            Modele = edtModeleVehiculeModifier.Text,
                            Annee = int.Parse(edtAnneeVehiculeModifier.Text)
                        };
                        await WebAPI.Instance.PostAsync("http://" + GetString(Resource.String.host) + ":" + GetString(Resource.String.port) + "/Vehicule/ModifierVehicule?nomCaserne="+paramNomCaserne, vehiculeDTO);
                        DialoguesUtils.AfficherToasts(this, leVehiculeModifier + " est modifié !!!");
                        Finish();
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
        /// Méthode permettant de rafraichir les informations du véhicule...
        /// </summary>
        private async Task RafraichirInterfaceDonnees()
        {
            try
            {
                string jsonResponse = await WebAPI.Instance.ExecuteGetAsync("http://" + GetString(Resource.String.host) + ":" + GetString(Resource.String.port) + "/Vehicule/ObtenirVehicule?nomCaserne=" + paramNomCaserne + " &vinVehicule=" + paramVinVehicule);
                leVehicule = JsonConvert.DeserializeObject<VehiculeDTO>(jsonResponse);
                edtVinVehiculeModifier.Text = leVehicule.VinVehicule;
                edtMarqueVehiculeModifier.Text = leVehicule.Marque;
                edtModeleVehiculeModifier.Text = leVehicule.Modele;
                edtAnneeVehiculeModifier.Text = leVehicule.Annee.ToString();
                

                string jsonResponseGrade = await WebAPI.Instance.ExecuteGetAsync("http://" + GetString(Resource.String.host) + ":" + GetString(Resource.String.port) + "/TypeVehicule/ObtenirListeTypeVehicule");
                listeTypeVehicule = JsonConvert.DeserializeObject<List<TypeVehiculeDTO>>(jsonResponseGrade);
                adapteurListeTypeVehicule = new ListeTypeVehiculeAdapter(this, listeTypeVehicule.ToArray());
                spinnerTypeVehicule.Adapter = adapteurListeTypeVehicule;
                // Pas sur de ce que cette ligne là fait
                // TypeVehiculeDTO typeVehiculeDTODuVehicule= new TypeVehiculeDTO(leVehicule.Type, );
                // À réparer quand on va être prêt :D
                //int typeVehiculeIndex = listeTypeVehicule.FindIndex(item => item.Description == lePompier.Grade);
                //spinnerGradePompier.SetSelection(typeVehiculeIndex);

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
            MenuInflater.Inflate(Resource.Menu.VehiculeModifierOptionsMenu, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        /// <summary>Méthode de service permettant de capter l'évenement exécuté lors d'un choix dans le menu.</summary>
        /// <param name="item">L'item sélectionné.</param>
        /// <returns>Retourne si un option à été sélectionné avec succès.</returns>
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.RetourVehiculeModifier:
                    Finish();
                    break;

                case Resource.Id.QuitterVehiculeModifier:
                    FinishAffinity();
                    break;
            }
            return base.OnOptionsItemSelected(item);
        }
    }
}