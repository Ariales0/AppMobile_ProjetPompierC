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
    /// M�thode de service appel�e lors de la cr�ation de l'activit�.
    /// </summary>
    /// <param name="savedInstanceState">�tat de l'activit�.</param>
    protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        SetContentView(Resource.Layout.InterfaceGradeActivity);

        edtDesciptionGrade = FindViewById<EditText>(Resource.Id.edtDescriptionGradeAjout);

        listViewGrade = FindViewById<ListView>(Resource.Id.lvGrade);
        listViewGrade.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) =>
        {
            Intent activiteGradeDetailsModifier = new Intent(this, typeof(GradeDetailsModifierActivity));
            //On initialise les param�tres avant de lancer la nouvelle activit�.
            activiteGradeDetailsModifier.PutExtra("paramDesciptionGrade", listeGrade[e.Position].Description);
            //On d�marre la nouvelle activit�.
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
                    DialoguesUtils.AfficherToasts(this, "Le grade " + desciptionGrade + " est ajout�");

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
    /// M�thode de service appel�e lors du retour en avant plan de l'activit�.
    /// </summary>
    protected override async void OnResume()
    {
        base.OnResume();
        await RafraichirInterfaceDonnees();
    }

    /// <summary>
    /// M�thode permettant de rafraichir la liste des grades...
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

    /// <summary>M�thode de service permettant d'initialiser le menu de l'activit�.</summary>
    /// <param name="menu">Le menu � construire.</param>
    /// <returns>Retourne True si l'optionMenu est bien cr��.</returns>
    public override bool OnCreateOptionsMenu(IMenu menu)
    {
        MenuInflater.Inflate(Resource.Menu.GradeActivityOptionsMenu, menu);
        return base.OnCreateOptionsMenu(menu);
    }

    /// <summary>M�thode de service permettant de capter l'�venement ex�cut� lors d'un choix dans le menu.</summary>
    /// <param name="item">L'item s�lectionn�.</param>
    /// <returns>Retourne si une option � �t� s�lectionn� avec succ�s.</returns>
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