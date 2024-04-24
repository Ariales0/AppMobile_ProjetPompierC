using Android.Content;
using Android.Views;
using Newtonsoft.Json;
using AlertDialog = Android.App.AlertDialog;

using AppMobile_ProjetPompierC;
using AppMobile_ProjetPompierC.Adapters;
using AppMobile_ProjetPompierC.DTO;
using AppMobile_ProjetPompierC.Utils;

namespace AppMobile_ProjetPompierC.Vues;

[Activity(Label = "VehiculeActivity")]
public class VehiculeActivity : Activity
{
    #region Proprietes

    /// <summary>
    /// Attribut repr�sentant le nom de la caserne.
    /// </summary>
    private string paramNomCaserne;

    /// <summary>
    /// Liste des v�hicules de la caserne.
    /// </summary>
    private List<VehiculeDTO> listeVehicule;

    /// <summary>
    /// Liste des types de v�hicule.
    /// </summary>
    private List<TypeVehiculeDTO> listeTypeVehicule;

    /// <summary>
    /// Adapter pour la liste des v�hicule.
    /// </summary>
    private ListeVehiculeAdapter adapteurListeVehicule;

    /// <summary>
    /// Adapter pour la liste des types.
    /// </summary>
    private ListeTypeVehiculeAdapter adapteurListeTypeVehicule;

    /// <summary>
    /// Attribut repr�sentant le champ d'�dition de la marque du v�hicule.
    /// </summary>
    private EditText edtMarqueVehicule;

    /// <summary>
    /// Liste deroulante qui contient les types .
    /// </summary>
    private Spinner spinnerTypeVehicule;

    /// <summary>
    /// Types�l�ctionn� dans le spinner .
    /// </summary>
    string typeSelectionne;

    /// <summary>
    /// Attribut repr�sentant le champ d'�dition du mod�le du v�hicule.
    /// </summary>
    private EditText edtModeleVehicule;

    /// <summary>
    /// Attribut repr�sentant le champ d'�dition de l'ann�e du v�hicule.
    /// </summary>
    private EditText edtAnneeVehicule;

	/// <summary>
	/// Attribut repr�sentant le champ d'�dition de l'ann�e du v�hicule.
	/// </summary>
	private EditText edtVinVehicule;

	/// <summary>
	/// Bouton pour ajouter un pompier dans la caserne.
	/// </summary>
	private Button btnAjouterVehicule;

    /// <summary>
    /// ListView pour afficher les pompiers de la caserne.
    /// </summary>
    private ListView listViewVehicule;

    #endregion Proprietes

    #region MethodesVehiculeActivity

    /// <summary>
    /// M�thode de service appel�e lors de la cr�ation de l'activit�.
    /// </summary>
    /// <param name="savedInstanceState">�tat de l'activit�.</param>
    protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        SetContentView(Resource.Layout.InterfacePompierActivity);

        edtVinVehicule = FindViewById<EditText>(Resource.Id.edtVinVehicule);
        
        edtMarqueVehicule = FindViewById<EditText>(Resource.Id.edtMarqueVehicule);
        edtModeleVehicule= FindViewById<EditText>(Resource.Id.edtModeleVehicule);
		edtAnneeVehicule = FindViewById<EditText>(Resource.Id.edtAnneeVehicule);

		paramNomCaserne = Intent.GetStringExtra("paramNomCaserne");

        spinnerTypeVehicule = FindViewById<Spinner>(Resource.Id.spTypeVehicule);
        spinnerTypeVehicule.ItemSelected += (sender, e) =>
        {
            VehiculeDTO vehiculeDTOSelecionne = new VehiculeDTO();
            vehiculeDTOSelecionne = listeVehicule[e.Position];
            typeSelectionne = vehiculeDTOSelecionne.Type;
        };

        listViewVehicule = FindViewById<ListView>(Resource.Id.lvVehicule);
        listViewVehicule.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) =>
        {
            Intent activiteVehiculeDetails = new Intent(this, typeof(VehiculeDetailsActivity));
            //On initialise les param�tres avant de lancer la nouvelle activit�.
            activiteVehiculeDetails.PutExtra("paramNomCaserne", paramNomCaserne);
            activiteVehiculeDetails.PutExtra("paramVinVehicule", listeVehicule[e.Position].VinVehicule);
            //On d�marre la nouvelle activit�.
            StartActivity(activiteVehiculeDetails);
        };

        btnAjouterVehicule = FindViewById<Button>(Resource.Id.btnAjouterVehicule);
        btnAjouterVehicule.Click += async (sender, e) =>
        {
            if ((int.TryParse(edtAnneeVehicule.Text, out int anneeVehicule)) && (!string.IsNullOrEmpty(typeSelectionne)) && (edtMarqueVehicule.Text.Length > 0) && (edtModeleVehicule.Text.Length > 0) && (edtVinVehicule.Text.Length > 0))
            {
                try
                {
                    string leVehicule = edtVinVehicule.Text;

                    VehiculeDTO vehiculeDTO = new VehiculeDTO
                    {
                        VinVehicule = edtVinVehicule.Text,
                        Marque = edtMarqueVehicule.Text,
                        Modele = edtModeleVehicule.Text,
                        Annee = anneeVehicule,
                        Type = typeSelectionne
                    };

                    await WebAPI.Instance.PostAsync("http://" + GetString(Resource.String.host) + ":" + GetString(Resource.String.port) + "/Vehicule/AjouterVehicule?nomCaserne=" + paramNomCaserne, vehiculeDTO);
                    await RafraichirInterfaceDonnees();
                    DialoguesUtils.AfficherToasts(this, "Le v�hicule " + leVehicule + " est ajout� � la caserne.");

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
    protected override async void OnResume()
    {
        base.OnResume();
        await RafraichirInterfaceDonnees();
    }

    /// <summary>
    /// M�thode permettant de rafraichir la liste des pompiers...
    /// </summary>
    private async Task RafraichirInterfaceDonnees()
    {
        try
        {
            string jsonResponseVehicule = await WebAPI.Instance.ExecuteGetAsync("http://" + GetString(Resource.String.host) + ":" + GetString(Resource.String.port) + "/Vehicule/ObtenirListeVehicule?nomCaserne="+ paramNomCaserne);
            listeVehicule = JsonConvert.DeserializeObject<List<VehiculeDTO>>(jsonResponseVehicule);
            adapteurListeVehicule = new ListeVehiculeAdapter(this, listeVehicule.ToArray()); 
            listViewVehicule.Adapter = adapteurListeVehicule;

            string jsonResponseType = await WebAPI.Instance.ExecuteGetAsync("http://" + GetString(Resource.String.host) + ":" + GetString(Resource.String.port) + "/TypeVehicule/ObtenirListeTypeVehicule");
            listeTypeVehicule = JsonConvert.DeserializeObject<List<TypeVehiculeDTO>>(jsonResponseType);
            adapteurListeTypeVehicule = new ListeTypeVehiculeAdapter(this, listeTypeVehicule.ToArray());
            spinnerTypeVehicule.Adapter = adapteurListeTypeVehicule;

            edtMarqueVehicule.Text = edtVinVehicule.Text = edtModeleVehicule.Text =  edtAnneeVehicule.Text = string.Empty;
        }
        catch (Exception ex)
        {
            DialoguesUtils.AfficherMessageOK(this, "Erreur", ex.Message);
        }
    }

    /// <summary>M�thode de service permettant d'initialiser le menu de l'activit�.</summary>
    /// <param name="menu">Le menu � construire.</param>
    /// <returns>Retourne True si l'optionMenu est bien cr��.</returns>
    public override bool OnCreateOptionsMenu(IMenu menu)
    {
        MenuInflater.Inflate(Resource.Menu.PompierActivityOptionsMenu, menu);
        return base.OnCreateOptionsMenu(menu);
    }

    /// <summary>M�thode de service permettant de capter l'�venement ex�cut� lors d'un choix dans le menu.</summary>
    /// <param name="item">L'item s�lectionn�.</param>
    /// <returns>Retourne si une option � �t� s�lectionn� avec succ�s.</returns>
    public override bool OnOptionsItemSelected(IMenuItem item)
    {
        switch (item.ItemId)
        {
            case Resource.Id.ViderPompierActivity:
                
                try
                {
                    AlertDialog.Builder builder = new AlertDialog.Builder(this);
                    builder.SetPositiveButton("Non", (send, args) => { });
                    builder.SetNegativeButton("Oui", async (send, args) =>
                    {
                        await WebAPI.Instance.PostAsync("http://" + GetString(Resource.String.host) + ":" + GetString(Resource.String.port) + "/Vehicule/ViderListeVehicule?nomCaserne=" + paramNomCaserne, null);
                        await RafraichirInterfaceDonnees();
                    });
                    AlertDialog dialog = builder.Create();
                    dialog.SetTitle("Suppression");
                    dialog.SetMessage("Voulez-vous vraiment vider la liste des v�hicules de la caserne " + paramNomCaserne + " ?");
                    dialog.Window.SetGravity(GravityFlags.Bottom);
                    dialog.Show();
                }
                catch (Exception ex)
                {

                    DialoguesUtils.AfficherMessageOK(this, "Erreur", ex.Message);
                }
                break;

            case Resource.Id.RetourPompierActivity:
                Finish();
                break;

            case Resource.Id.QuitterPompierActivity:
                FinishAffinity();
                break;
        }
        return base.OnOptionsItemSelected(item);
    }

    #endregion MethodesPompierActivity
}