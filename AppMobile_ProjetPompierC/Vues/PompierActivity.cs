using Android.Content;
using Android.Views;
using Newtonsoft.Json;
using AlertDialog = Android.App.AlertDialog;

using AppMobile_ProjetPompierC;
using AppMobile_ProjetPompierC.Adapters;
using AppMobile_ProjetPompierC.DTO;
using AppMobile_ProjetPompierC.Utils;

namespace AppMobile_ProjetPompierC.Vues;

[Activity(Label = "PompierActivity")]
public class PompierActivity : Activity
{
    #region Proprietes

    /// <summary>
    /// Attribut représentant le nom de la caserne.
    /// </summary>
    private string paramNomCaserne;

    /// <summary>
    /// Liste des pompiers de la caserne.
    /// </summary>
    private List<PompierDTO> listePompier;

    /// <summary>
    /// Adapter pour la liste des pompier.
    /// </summary>
    private ListePompierAdapter adapteurListePompier;

    /// <summary>
    /// Attribut représentant le champ d'édition du matricule du pompier .
    /// </summary>
    private EditText edtMatriculePompier;

    /// <summary>
    /// Attribut représentant le champ d'édition du grade du pompier .
    /// </summary>
    private EditText edtGradePompier;

    /// <summary>
    /// Attribut représentant le champ d'édition du nom du pompier .
    /// </summary>
    private EditText edtNomPompier;

    /// <summary>
    /// Attribut représentant le champ d'édition du prenom du pompier .
    /// </summary>
    private EditText edtPrenomPompier;

    /// <summary>
    /// Bouton pour ajouter un pompier dans la caserne.
    /// </summary>
    private Button btnAjouterpompier;

    /// <summary>
    /// ListView pour afficher les pompiers de la caserne.
    /// </summary>
    private ListView listViewPompier;

    #endregion Proprietes

    #region MethodesPompierActivity

    /// <summary>
    /// Méthode de service appelée lors de la création de l'activité.
    /// </summary>
    /// <param name="savedInstanceState">État de l'activité.</param>
    protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        SetContentView(Resource.Layout.InterfacePompierActivity);

        edtMatriculePompier = FindViewById<EditText>(Resource.Id.edtMatriculePompier);
        edtGradePompier = FindViewById<EditText>(Resource.Id.edtGradePompier);
        edtNomPompier = FindViewById<EditText>(Resource.Id.edtNomPompier);
        edtPrenomPompier = FindViewById<EditText>(Resource.Id.edtPrenomPompier);

        listViewPompier = FindViewById<ListView>(Resource.Id.lvPompier);
        listViewPompier.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) =>
        {
            Intent activitePompierDetails = new Intent(this, typeof(PompierDetailsActivity));
            //On initialise les paramètres avant de lancer la nouvelle activité.
            activitePompierDetails.PutExtra("paramNomCaserne", paramNomCaserne);
            activitePompierDetails.PutExtra("paramMatriculePompier", listePompier[e.Position].Matricule);
            //On démarre la nouvelle activité.
            StartActivity(activitePompierDetails);
        };

        btnAjouterpompier = FindViewById<Button>(Resource.Id.btnAjouterPompier);
        btnAjouterpompier.Click += async (sender, e) =>
        {
            if ((int.TryParse(edtMatriculePompier.Text, out int matriculePompier)) && (edtGradePompier.Text.Length > 0) && (edtNomPompier.Text.Length > 0) && (edtPrenomPompier.Text.Length > 0))
            {
                try
                {
                    string lePompierAjoute = edtGradePompier.Text + " " + edtNomPompier.Text + " " + edtPrenomPompier.Text;

                    PompierDTO pompierDTO = new PompierDTO
                    {
                        Matricule = matriculePompier,
                        Grade = edtGradePompier.Text,
                        Nom = edtNomPompier.Text,
                        Prenom = edtPrenomPompier.Text
                    };

                    await WebAPI.Instance.PostAsync("http://" + GetString(Resource.String.host) + ":" + GetString(Resource.String.port) + "/Pompier/AjouterPompier?nomCaserne=" + paramNomCaserne, pompierDTO);
                    await RafraichirInterfaceDonnees();
                    DialoguesUtils.AfficherToasts(this, lePompierAjoute + " est ajouté à la caserne.");

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
    /// Méthode permettant de rafraichir la liste des pompiers...
    /// </summary>
    private async Task RafraichirInterfaceDonnees()
    {
        try
        {
            string jsonResponse = await WebAPI.Instance.ExecuteGetAsync("http://" + GetString(Resource.String.host) + ":" + GetString(Resource.String.port) + "/Caserne/ObtenirListePompier?nomCaserne="+ paramNomCaserne);
            listePompier = JsonConvert.DeserializeObject<List<PompierDTO>>(jsonResponse);
            adapteurListePompier = new ListePompierAdapter(this, listePompier.ToArray()); 
            listViewPompier.Adapter = adapteurListePompier;
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
        MenuInflater.Inflate(Resource.Menu.PompierActivityOptionsMenu, menu);
        return base.OnCreateOptionsMenu(menu);
    }

    /// <summary>Méthode de service permettant de capter l'évenement exécuté lors d'un choix dans le menu.</summary>
    /// <param name="item">L'item sélectionné.</param>
    /// <returns>Retourne si une option à été sélectionné avec succès.</returns>
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
                        await WebAPI.Instance.PostAsync("http://" + GetString(Resource.String.host) + ":" + GetString(Resource.String.port) + "/Pompier/ViderListePompier?nomCaserne"+paramNomCaserne, null);
                        await RafraichirInterfaceDonnees();
                    });
                    AlertDialog dialog = builder.Create();
                    dialog.SetTitle("Suppression");
                    dialog.SetMessage("Voulez-vous vraiment vider la liste des pompiers de la caserne"+paramNomCaserne+" ?");
                    dialog.Window.SetGravity(GravityFlags.Bottom);
                    dialog.Show();
                }
                catch (Exception ex)
                {

                    //DialoguesUtils.AfficherMessageOK(this, "Erreur", ex.Message);
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