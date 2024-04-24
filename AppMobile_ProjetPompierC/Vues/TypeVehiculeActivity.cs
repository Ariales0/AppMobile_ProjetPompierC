using Android.Content;
using Android.Views;
using Newtonsoft.Json;
using AlertDialog = Android.App.AlertDialog;

using AppMobile_ProjetPompierC.Adapters;
using AppMobile_ProjetPompierC.DTO;
using AppMobile_ProjetPompierC.Utils;

namespace AppMobile_ProjetPompierC.Vues;

[Activity(Label = "TypeVehiculeActivity")]
public class TypeVehiculeActivity : Activity
{
    #region Proprietes

    /// <summary>
    /// Liste des types.
    /// </summary>
    private List<TypeVehiculeDTO> listeTypeVehicule;

    /// <summary>
    /// Adapter pour la liste des types.
    /// </summary>
    private ListeTypeVehiculeAdapter adapteurListeTypeVehicule;

    /// <summary>
    /// Type du type.
    /// </summary>
    private EditText edtTypeTypeVehicule;

	/// <summary>
	/// Code du type.
	/// </summary>
	private EditText edtCodeTypeVehicule;

	/// <summary>
	/// Nombre de personne du type.
	/// </summary>
	private EditText edtNbPersonneTypeVehicule;


	/// <summary>
	/// Bouton d'ajout d'un type.
	/// </summary>
	private Button btnAjouterTypeVehicule;

    /// <summary>
    /// ListView pour afficher les types.
    /// </summary>
    private ListView listViewTypeVehicule;

    #endregion Proprietes

    #region Methodes

    /// <summary>
    /// Méthode de service appelée lors de la création de l'activité.
    /// </summary>
    /// <param name="savedInstanceState">État de l'activité.</param>
    protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        SetContentView(Resource.Layout.InterfaceTypeVehiculeActivity);

        edtTypeTypeVehicule = FindViewById<EditText>(Resource.Id.edtTypeTypeVehiculeAjout);
		edtCodeTypeVehicule = FindViewById<EditText>(Resource.Id.edtCodeTypeVehiculeAjout);
		edtNbPersonneTypeVehicule = FindViewById<EditText>(Resource.Id.edtNbPersonneTypeVehiculeAjout);

		listViewTypeVehicule = FindViewById<ListView>(Resource.Id.lvTypeVehicule);
        listViewTypeVehicule.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) =>
        {
            Intent activiteTypeVehiculeDetailsModifier = new Intent(this, typeof(TypeVehiculeDetailsModifierActivity));
            //On initialise les paramètres avant de lancer la nouvelle activité.
            activiteTypeVehiculeDetailsModifier.PutExtra("paramTypeTypeVehicule", listeTypeVehicule[e.Position].Type);
			activiteTypeVehiculeDetailsModifier.PutExtra("paramCodeTypeVehicule", listeTypeVehicule[e.Position].Code);
			activiteTypeVehiculeDetailsModifier.PutExtra("paramNbPersonneTypeVehicule", listeTypeVehicule[e.Position].NombrePersonne);
			//On démarre la nouvelle activité.
			StartActivity(activiteTypeVehiculeDetailsModifier);
        };

        btnAjouterTypeVehicule = FindViewById<Button>(Resource.Id.btnAjouterTypeVehicule);
        btnAjouterTypeVehicule.Click += async (sender, e) =>
        {
            if (edtTypeTypeVehicule.Text.Length > 0 && edtCodeTypeVehicule.Text.Length > 0 && int.Parse(edtNbPersonneTypeVehicule.Text) > 0)
            {
                try
                {
                    string typeTypeVehicule = edtTypeTypeVehicule.Text;
					string codeTypeVehicule = edtCodeTypeVehicule.Text;
					int nbPersonneTypeVehicule = int.Parse(edtNbPersonneTypeVehicule.Text);

					TypeVehiculeDTO typeDTO = new TypeVehiculeDTO(typeTypeVehicule, codeTypeVehicule, nbPersonneTypeVehicule);

                    /* Enlever le commentaire quand l'API sera prêt :)
                    await WebAPI.Instance.PostAsync("http://" + GetString(Resource.String.host) + ":" + GetString(Resource.String.port) + "/TypeVehicule/AjouterTypeVehicule", typeDTO);
                    DialoguesUtils.AfficherToasts(this, "Le type " + typeTypeVehicule+ " est ajouté");
                    */
                    await RafraichirInterfaceDonnees();
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
    /// Méthode permettant de rafraichir la liste des types...
    /// </summary>
    private async Task RafraichirInterfaceDonnees()
    {

        /* Enlever le commentaire quand l'API sera prêt :)
        try
        {
            string jsonResponse = await WebAPI.Instance.ExecuteGetAsync("http://" + GetString(Resource.String.host) + ":" + GetString(Resource.String.port) + "/TypeVehicule/ObtenirListeTypeVehicule");
            listeTypeVehicule = JsonConvert.DeserializeObject<List<TypeVehiculeDTO>>(jsonResponse);

            
			adapteurListeTypeVehicule = new ListeTypeVehiculeAdapter(this, listeTypeVehicule.ToArray());
            listViewTypeVehicule.Adapter = adapteurListeTypeVehicule;
            edtTypeTypeVehicule.Text = "";
			edtCodeTypeVehicule.Text = "";
			edtNbPersonneTypeVehicule.Text = "";
		}
        catch (Exception ex)
        {
            DialoguesUtils.AfficherMessageOK(this, "Erreur", ex.Message);
        }
        */
    }

    /// <summary>Méthode de service permettant d'initialiser le menu de l'activité.</summary>
    /// <param name="menu">Le menu à construire.</param>
    /// <returns>Retourne True si l'optionMenu est bien créé.</returns>
    public override bool OnCreateOptionsMenu(IMenu menu)
    {
        MenuInflater.Inflate(Resource.Menu.TypeVehiculeActivityOptionsMenu, menu);
        return base.OnCreateOptionsMenu(menu);
    }

    /// <summary>Méthode de service permettant de capter l'évenement exécuté lors d'un choix dans le menu.</summary>
    /// <param name="item">L'item sélectionné.</param>
    /// <returns>Retourne si une option à été sélectionné avec succès.</returns>
    public override bool OnOptionsItemSelected(IMenuItem item)
    {
        switch (item.ItemId)
        {
            case Resource.Id.ViderTypeVehiculeActivity:

                try
                {
                    AlertDialog.Builder builder = new AlertDialog.Builder(this);
                    builder.SetPositiveButton("Non", (send, args) => { });
                    builder.SetNegativeButton("Oui", async (send, args) =>
                    {
                        await WebAPI.Instance.PostAsync("http://" + GetString(Resource.String.host) + ":" + GetString(Resource.String.port) + "/TypeVehicule/ViderListetypeVehicule", null);
                        await RafraichirInterfaceDonnees();
                    });
                    AlertDialog dialog = builder.Create();
                    dialog.SetTitle("Suppression");
                    dialog.SetMessage("Voulez-vous vraiment vider la liste des types de véhicule ?");
                    dialog.Window.SetGravity(GravityFlags.Bottom);
                    dialog.Show();
                }
                catch (Exception ex)
                {

                    DialoguesUtils.AfficherMessageOK(this, "Erreur", ex.Message);
                }
                break;

            case Resource.Id.RetourTypeVehiculeActivity:
                Finish();
                break;

            case Resource.Id.QuitterTypeVehiculeActivity:
                FinishAffinity();
                break;
        }
        return base.OnOptionsItemSelected(item);
    }

    #endregion Methodes
}