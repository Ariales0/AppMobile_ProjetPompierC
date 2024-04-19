using Android.Content;
using Android.Views;
using Newtonsoft.Json;
using AlertDialog = Android.App.AlertDialog;

using AppMobile_ProjetPompierC.Adapters;
using AppMobile_ProjetPompierC.DTO;
using AppMobile_ProjetPompierC.Utils;

namespace AppMobile_ProjetPompierC.Vues;

[Activity(Label = "GradeActivity")]
public class GradeActivity : Activity
{
    #region Proprietes

    /// <summary>
    /// Liste des grades.
    /// </summary>
    private List<GradeDTO> listeGrade;

    /// <summary>
    /// Adapter pour la liste des grade.
    /// </summary>
    private ListeGradeAdapter adapteurListeGrade;

    /// <summary>
    /// Desciption du grade.
    /// </summary>
    private EditText edtDesciptionGrade;

    /// <summary>
    /// Bouton d'ajout d'un grade.
    /// </summary>
    private Button btnAjouterGrade;

    /// <summary>
    /// ListView pour afficher les grades.
    /// </summary>
    private ListView listViewGrade;

    #endregion Proprietes

    #region Methodes

    /// <summary>
    /// Méthode de service appelée lors de la création de l'activité.
    /// </summary>
    /// <param name="savedInstanceState">État de l'activité.</param>
    protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        SetContentView(Resource.Layout.InterfaceGradeActivity);

        edtDesciptionGrade = FindViewById<EditText>(Resource.Id.edtDescriptionGradeAjout);

        listViewGrade = FindViewById<ListView>(Resource.Id.lvGrade);
        listViewGrade.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) =>
        {
            Intent activiteGradeDetailsModifier = new Intent(this, typeof(GradeDetailsModifierActivity));
            //On initialise les paramètres avant de lancer la nouvelle activité.
            activiteGradeDetailsModifier.PutExtra("paramDesciptionGrade", listeGrade[e.Position].Description);
            //On démarre la nouvelle activité.
            StartActivity(activiteGradeDetailsModifier);
        };

        btnAjouterGrade = FindViewById<Button>(Resource.Id.btnAjouterGrade);
        btnAjouterGrade.Click += async (sender, e) =>
        {
            if (edtDesciptionGrade.Text.Length > 0)
            {
                try
                {
                    string desciptionGrade = edtDesciptionGrade.Text;
                    GradeDTO gradeDTO = new GradeDTO(desciptionGrade);

                    await WebAPI.Instance.PostAsync("http://" + GetString(Resource.String.host) + ":" + GetString(Resource.String.port) + "/Grade/AjouterGrade", gradeDTO);
                    DialoguesUtils.AfficherToasts(this, "Le grade " + desciptionGrade + " est ajouté");

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
    /// Méthode permettant de rafraichir la liste des grades...
    /// </summary>
    private async Task RafraichirInterfaceDonnees()
    {
        try
        {
            string jsonResponse = await WebAPI.Instance.ExecuteGetAsync("http://" + GetString(Resource.String.host) + ":" + GetString(Resource.String.port) + "/Grade/ObtenirListeGrade");
            listeGrade = JsonConvert.DeserializeObject<List<GradeDTO>>(jsonResponse);
            adapteurListeGrade = new ListeGradeAdapter(this, listeGrade.ToArray());
            listViewGrade.Adapter = adapteurListeGrade;
            edtDesciptionGrade.Text = "";
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
        MenuInflater.Inflate(Resource.Menu.GradeActivityOptionsMenu, menu);
        return base.OnCreateOptionsMenu(menu);
    }

    /// <summary>Méthode de service permettant de capter l'évenement exécuté lors d'un choix dans le menu.</summary>
    /// <param name="item">L'item sélectionné.</param>
    /// <returns>Retourne si une option à été sélectionné avec succès.</returns>
    public override bool OnOptionsItemSelected(IMenuItem item)
    {
        switch (item.ItemId)
        {
            case Resource.Id.ViderGradeActivity:

                try
                {
                    bool reponse = DialoguesUtils.AfficherDialogueQuestionOuiNon(this, "Vider la liste des grades !?", "Voulez-vous vraiment vider la liste des grades ?");
                    if (reponse)
                    {
                        async Task ViderListeGradeAsync()
                        {
                            await WebAPI.Instance.PostAsync("http://" + GetString(Resource.String.host) + ":" + GetString(Resource.String.port) + "/Grade/ViderListeGrade", null);
                            await RafraichirInterfaceDonnees();
                        }
                    }
                }
                catch (Exception ex)
                {

                    DialoguesUtils.AfficherMessageOK(this, "Erreur", ex.Message);
                }
                break;

            case Resource.Id.RetourGradeActivity:
                Finish();
                break;

            case Resource.Id.QuitterGradeActivity:
                FinishAffinity();
                break;
        }
        return base.OnOptionsItemSelected(item);
    }

    #endregion Methodes
}