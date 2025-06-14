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
    /// Attribut repr�sentant le nom de la caserne.
    /// </summary>
    private string paramNomCaserne;

    /// <summary>
    /// Liste des pompiers de la caserne.
    /// </summary>
    private List<PompierDTO> listePompier;

    /// <summary>
    /// Liste des grades.
    /// </summary>
    private List<GradeDTO> listeGrade;

    /// <summary>
    /// Adapter pour la liste des pompier.
    /// </summary>
    private ListePompierAdapter adapteurListePompier;

    /// <summary>
    /// Adapter pour la liste des grade.
    /// </summary>
    private ListeGradeAdapter adapteurListeGrade;

    /// <summary>
    /// Attribut repr�sentant le champ d'�dition du matricule du pompier .
    /// </summary>
    private EditText edtMatriculePompier;

    /// <summary>
    /// Liste deroulante qui contient les grades .
    /// </summary>
    private Spinner spinnerGradePompier;

    /// <summary>
    /// Grade s�l�ctionn� dans le spinner .
    /// </summary>
    string gradeSelectionne;

    /// <summary>
    /// Attribut repr�sentant le champ d'�dition du nom du pompier .
    /// </summary>
    private EditText edtNomPompier;

    /// <summary>
    /// Attribut repr�sentant le champ d'�dition du prenom du pompier .
    /// </summary>
    private EditText edtPrenomPompier;

    /// <summary>
    /// Bouton pour ajouter un pompier dans la caserne.
    /// </summary>
    private Button btnAjouterPompier;

    /// <summary>
    /// ListView pour afficher les pompiers de la caserne.
    /// </summary>
    private ListView listViewPompier;

    #endregion Proprietes

    #region MethodesPompierActivity

    /// <summary>
    /// M�thode de service appel�e lors de la cr�ation de l'activit�.
    /// </summary>
    /// <param name="savedInstanceState">�tat de l'activit�.</param>
    protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        SetContentView(Resource.Layout.InterfacePompierActivity);

        edtMatriculePompier = FindViewById<EditText>(Resource.Id.edtMatriculePompier);
        
        edtNomPompier = FindViewById<EditText>(Resource.Id.edtNomPompier);
        edtPrenomPompier = FindViewById<EditText>(Resource.Id.edtPrenomPompier);

        paramNomCaserne = Intent.GetStringExtra("paramNomCaserne");

        spinnerGradePompier = FindViewById<Spinner>(Resource.Id.spGradePompier);
        spinnerGradePompier.ItemSelected += (sender, e) =>
        {
            GradeDTO gradeDTOSelecionne = new GradeDTO();
            gradeDTOSelecionne = listeGrade[e.Position];
            gradeSelectionne = gradeDTOSelecionne.Description;
        };

        listViewPompier = FindViewById<ListView>(Resource.Id.lvPompier);
        listViewPompier.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) =>
        {
            Intent activitePompierDetails = new Intent(this, typeof(PompierDetailsActivity));
            //On initialise les param�tres avant de lancer la nouvelle activit�.
            activitePompierDetails.PutExtra("paramNomCaserne", paramNomCaserne);
            activitePompierDetails.PutExtra("paramMatriculePompier", listePompier[e.Position].Matricule);
            //On d�marre la nouvelle activit�.
            StartActivity(activitePompierDetails);
        };

        btnAjouterPompier = FindViewById<Button>(Resource.Id.btnAjouterPompier);
        btnAjouterPompier.Click += async (sender, e) =>
        {
            if ((int.TryParse(edtMatriculePompier.Text, out int matriculePompier)) && (!string.IsNullOrEmpty(gradeSelectionne)) && (edtNomPompier.Text.Length > 0) && (edtPrenomPompier.Text.Length > 0))
            {
                try
                {
                    string lePompierAjoute = gradeSelectionne + " " + edtNomPompier.Text + " " + edtPrenomPompier.Text;

                    PompierDTO pompierDTO = new PompierDTO
                    {
                        Matricule = matriculePompier,
                        Grade = gradeSelectionne,
                        Nom = edtNomPompier.Text,
                        Prenom = edtPrenomPompier.Text
                    };

                    await WebAPI.Instance.PostAsync("http://" + GetString(Resource.String.host) + ":" + GetString(Resource.String.port) + "/Pompier/AjouterPompier?nomCaserne=" + paramNomCaserne, pompierDTO);
                    await RafraichirInterfaceDonnees();
                    DialoguesUtils.AfficherToasts(this, lePompierAjoute + " est ajout� � la caserne.");

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
            string jsonResponsePompier = await WebAPI.Instance.ExecuteGetAsync("http://" + GetString(Resource.String.host) + ":" + GetString(Resource.String.port) + "/Pompier/ObtenirListePompier?nomCaserne="+ paramNomCaserne+ "&seulementCapitaine=false");
            listePompier = JsonConvert.DeserializeObject<List<PompierDTO>>(jsonResponsePompier);
            adapteurListePompier = new ListePompierAdapter(this, listePompier.ToArray()); 
            listViewPompier.Adapter = adapteurListePompier;

            string jsonResponseGrade = await WebAPI.Instance.ExecuteGetAsync("http://" + GetString(Resource.String.host) + ":" + GetString(Resource.String.port) + "/Grade/ObtenirListeGrade");
            listeGrade = JsonConvert.DeserializeObject<List<GradeDTO>>(jsonResponseGrade);
            adapteurListeGrade = new ListeGradeAdapter(this, listeGrade.ToArray());
            spinnerGradePompier.Adapter = adapteurListeGrade;

            edtMatriculePompier.Text = edtNomPompier.Text = edtPrenomPompier.Text = string.Empty;
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
                        await WebAPI.Instance.PostAsync("http://" + GetString(Resource.String.host) + ":" + GetString(Resource.String.port) + "/Pompier/ViderListePompier?nomCaserne=" + paramNomCaserne, null);
                        await RafraichirInterfaceDonnees();
                    });
                    AlertDialog dialog = builder.Create();
                    dialog.SetTitle("Suppression");
                    dialog.SetMessage("Voulez-vous vraiment vider la liste des pompiers de la caserne " + paramNomCaserne + " ?");
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