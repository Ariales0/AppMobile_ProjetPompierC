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
    /// Classe de type Activit� pour la modification d'un v�hicule.
    /// </summary>
    [Activity(Label = "@string/app_name")]
    public class VehiculeModifierActivity : Activity
    {
        #region Proprietes

        /// <summary>
        /// Attribut repr�sentant le nom de la caserne.
        /// </summary>
        private string paramNomCaserne;

        /// <summary>
        /// Attribut repr�sentant le vin du v�hicule.
        /// </summary>
        private string paramVinVehicule;

        /// <summary>
        /// Liste des type de v�hicule.
        /// </summary>
        private List<TypeVehiculeDTO> listeTypeVehicule;

        /// <summary>
        /// Adapter pour la liste des types.
        /// </summary>
        private ListeTypeVehiculeAdapter adapteurListeTypeVehicule;

        /// <summary>
        /// Attribut repr�sentant le v�hicule en objet VehiculeDTO.
        /// </summary>
        private VehiculeDTO leVehicule;

        /// <summary>
        /// Attribut repr�sentant le champ d'affichage du VIN du v�hicule.
        /// </summary>
        private TextView edtVinVehiculeModifier;

        /// <summary>
        /// Liste deroulante qui contient les types.
        /// </summary>
        private Spinner spinnerTypeVehicule;

        /// <summary>
        /// Type s�l�ctionn� dans le spinner.
        /// </summary>
        string typeSelectionne;

        /// <summary>
        /// Attribut repr�sentant le champ d'affichage de la marque du v�hicule.
        /// </summary>
        private TextView edtMarqueVehiculeModifier;

        /// <summary>
        /// Attribut repr�sentant le champ d'affichage du mod�le du v�hicule.
        /// </summary>
        private TextView edtModeleVehiculeModifier;

		// <summary>
		/// Attribut repr�sentant le champ d'affichage de l'ann�e du v�hicule.
		/// </summary>
		private TextView edtAnneeVehiculeModifier;

		/// <summary>
		/// Bouton pour ajouter un v�hicule dans la caserne.
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
                // DEMANDER � KEVEN SI ON PEUT MODIFIER LE TYPE D'UN V�HICULE
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
                        DialoguesUtils.AfficherToasts(this, leVehiculeModifier + " est modifi� !!!");
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
        /// M�thode de service appel�e lors du retour en avant plan de l'activit�.
        /// </summary>
        protected async override void OnResume()
        {
            base.OnResume();
            await RafraichirInterfaceDonnees();
        }

        /// <summary>
        /// M�thode permettant de rafraichir les informations du v�hicule...
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
                // Pas sur de ce que cette ligne l� fait
                // TypeVehiculeDTO typeVehiculeDTODuVehicule= new TypeVehiculeDTO(leVehicule.Type, );
                // � r�parer quand on va �tre pr�t :D
                //int typeVehiculeIndex = listeTypeVehicule.FindIndex(item => item.Description == lePompier.Grade);
                //spinnerGradePompier.SetSelection(typeVehiculeIndex);

            }
            catch (Exception)
            {
                Finish();
            }
        }

        /// <summary>M�thode de service permettant d'initialiser le menu de l'activit� principale.</summary>
        /// <param name="menu">Le menu � construire.</param>
        /// <returns>Retourne True si l'optionMenu est bien cr��.</returns>
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.VehiculeModifierOptionsMenu, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        /// <summary>M�thode de service permettant de capter l'�venement ex�cut� lors d'un choix dans le menu.</summary>
        /// <param name="item">L'item s�lectionn�.</param>
        /// <returns>Retourne si un option � �t� s�lectionn� avec succ�s.</returns>
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