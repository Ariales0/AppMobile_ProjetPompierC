using Android.Content;
using Android.Views;
using Newtonsoft.Json;

using AppMobile_ProjetPompierC.DTO;
using AppMobile_ProjetPompierC.Utils;
using ProjetPompier_Mobile.Vues;

namespace AppMobile_ProjetPompierC.Vues;

[Activity(Label = "TypeVehiculeDetailsModifierActivity")]
public class TypeVehiculeDetailsModifierActivity : Activity
{
    #region Proprietes

    /// <summary>
    /// Description de la caserne envoye par l'activite precedent.
    /// </summary>
    private string paramDesciptionGrade;

    /// <summary>
    /// Attribut représentant le pompier en objet PompierDTO.
    /// </summary>
    private PompierDTO lePompier;

    /// <summary>
    /// Desciption du grade afficher.
    /// </summary>
    private TextView lblDesciptionGrade;

    /// <summary>
    /// Desciption du grade pour modification.
    /// </summary>
    private EditText edtDesciptionGrade;

    /// <summary>
    /// CheckBox qui rend accessible ou non la modification du grade.
    /// </summary>
    private CheckBox checkBoxModification;

    /// <summary>
    /// Bouton d'ajout d'un grade.
    /// </summary>
    private Button btnModifierGrade;

    #endregion Proprietes
    #region Methodes

    /// <summary>
    /// Méthode de service appelée lors de la création de l'activité.
    /// </summary>
    /// <param name="savedInstanceState">État de l'activité.</param>
    protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        SetContentView(Resource.Layout.InterfaceGradeDetailsModifier);

        lblDesciptionGrade = FindViewById<TextView>(Resource.Id.tvDescriptionGradeDetails);
        edtDesciptionGrade = FindViewById<EditText>(Resource.Id.tvGradeDescriptionModifier);

        paramDesciptionGrade = Intent.GetStringExtra("paramDesciptionGrade");

        btnModifierGrade = FindViewById<Button>(Resource.Id.btnModifierGrade);
        btnModifierGrade.Click += async (sender, e) =>
        {
            if (edtDesciptionGrade.Text.Length > 0)
            {
                try
                {
                    string desciptionGrade = edtDesciptionGrade.Text;
                    GradeDTO gradeDTO = new GradeDTO(desciptionGrade);

                    await WebAPI.Instance.PostAsync("http://" + GetString(Resource.String.host) + ":" + GetString(Resource.String.port) + "/Grade/ModifierGrade?descriptionAvantChangement=" + lblDesciptionGrade.Text + "&descriptionApresChangement=" + edtDesciptionGrade.Text, null);
                    DialoguesUtils.AfficherToasts(this,"Le grade "+ desciptionGrade + " est modifié !!!");
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

        checkBoxModification = FindViewById<CheckBox>(Resource.Id.checkBoxGradeModifier);
        checkBoxModification.CheckedChange += (sender, e) =>
        {
            bool isChecked = checkBoxModification.Checked;
            btnModifierGrade.Enabled = isChecked;
            edtDesciptionGrade.Enabled = isChecked;
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
    /// Méthode permettant de rafraichir les informations de la Caserne...
    /// </summary>
    private async Task RafraichirInterfaceDonnees()
    {
        try
        {
            //Obtenir grade de l'API n'est pas utile
            lblDesciptionGrade.Text = paramDesciptionGrade;
            edtDesciptionGrade.Text = paramDesciptionGrade;

            btnModifierGrade.Enabled = false;
            edtDesciptionGrade.Enabled = false;

        }
        catch (Exception)
        {
            Finish();
        }
    }

    #region Menu
    /// <summary>Méthode de service permettant d'initialiser le menu de l'activité principale.</summary>
    /// <param name="menu">Le menu à construire.</param>
    /// <returns>Retourne True si l'optionMenu est bien créé.</returns>
    public override bool OnCreateOptionsMenu(IMenu menu)
    {
        MenuInflater.Inflate(Resource.Menu.GradeDetailsModifierOptionsMenu, menu);
        return base.OnCreateOptionsMenu(menu);
    }

    /// <summary>Méthode de service permettant de capter l'évenement exécuté lors d'un choix dans le menu.</summary>
    /// <param name="item">L'item sélectionné.</param>
    /// <returns>Retourne si un option à été sélectionné avec succès.</returns>
    public override bool OnOptionsItemSelected(IMenuItem item)
    {
        switch (item.ItemId)
        {
            case Resource.Id.SupprimerGradeDM:
                try
                {
                    AlertDialog.Builder builder = new AlertDialog.Builder(this);
                    builder.SetPositiveButton("Non", (send, args) => { });
                    builder.SetNegativeButton("Oui", async (send, args) =>
                    {
                        await WebAPI.Instance.PostAsync("http://" + GetString(Resource.String.host) + ":" + GetString(Resource.String.port) + "/Grade/SupprimerGrade?description=" + paramDesciptionGrade, null);
                        Finish();
                    });
                    AlertDialog dialog = builder.Create();
                    dialog.SetTitle("Suppression");
                    dialog.SetMessage("Voulez-vous vraiment supprimer le grade " + paramDesciptionGrade + "?");
                    dialog.Window.SetGravity(GravityFlags.Bottom);
                    dialog.Show();
                }
                catch (Exception ex)
                {
                    DialoguesUtils.AfficherMessageOK(this, "Erreur", ex.Message);
                }
                break;

            case Resource.Id.RetourGradeDM:
                Finish();
                break;

            case Resource.Id.QuitterGradeDM:
                FinishAffinity();
                break;
        }
        return base.OnOptionsItemSelected(item);
    }
    #endregion Menu
    #endregion Methodes

}
