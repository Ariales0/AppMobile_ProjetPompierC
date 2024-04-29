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
    /// Type du type envoye par l'activite precedent.
    /// </summary>
    private string paramTypeTypeVehicule;

	/// <summary>
	/// Code du type envoye par l'activite precedent.
	/// </summary>
	private int paramCodeTypeVehicule;

	/// <summary>
	/// Nombre de personne du type envoye par l'activite precedent.
	/// </summary>
	private int paramNbPersonneTypeVehicule;

	/// <summary>
	/// Attribut représentant le pompier en objet PompierDTO.
	/// </summary>
	private PompierDTO lePompier;

    /// <summary>
    /// Type du type afficher.
    /// </summary>
    private TextView lblTypeTypeVehicule;

    /// <summary>
    /// Type du type pour modification.
    /// </summary>
    private EditText edtTypeTypeVehicule;

	/// <summary>
	/// Code du type afficher.
	/// </summary>
	private TextView lblCodeTypeVehicule;

	/// <summary>
	/// Code du type pour modification.
	/// </summary>
	private EditText edtCodeTypeVehicule;

	/// <summary>
	/// Nombre de personne du type afficher.
	/// </summary>
	private TextView lblNbPersonneTypeVehicule;

    /// <summary>
    /// Nombre de personne du type pour modification.
    /// </summary>
    private EditText edtNbPersonneTypeVehicule;

	/// <summary>
	/// CheckBox qui rend accessible ou non la modification du grade.
	/// </summary>
	private CheckBox checkBoxModification;

    /// <summary>
    /// Bouton d'ajout d'un grade.
    /// </summary>
    private Button btnModifierTypeVehicule;

    #endregion Proprietes
    #region Methodes

    /// <summary>
    /// Méthode de service appelée lors de la création de l'activité.
    /// </summary>
    /// <param name="savedInstanceState">État de l'activité.</param>
    protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        SetContentView(Resource.Layout.InterfaceTypeVehiculeDetailsModifier);

        lblTypeTypeVehicule = FindViewById<TextView>(Resource.Id.tvTypeTypeVehiculeDetails);
        edtTypeTypeVehicule = FindViewById<EditText>(Resource.Id.tvTypeTypeVehiculeModifier);

		lblCodeTypeVehicule = FindViewById<TextView>(Resource.Id.tvCodeTypeVehiculeDetails);
		edtCodeTypeVehicule = FindViewById<EditText>(Resource.Id.tvCodeTypeVehiculeModifier);

		lblNbPersonneTypeVehicule = FindViewById<TextView>(Resource.Id.tvNbPersonneTypeVehiculeDetails);
		edtNbPersonneTypeVehicule = FindViewById<EditText>(Resource.Id.tvNbPersonneTypeVehiculeModifier);

		paramTypeTypeVehicule = Intent.GetStringExtra("paramTypeTypeVehicule");
		paramCodeTypeVehicule = Intent.GetIntExtra("paramCodeTypeVehicule", 0);
		paramNbPersonneTypeVehicule = Intent.GetIntExtra("paramNbPersonneTypeVehicule", 0);

		btnModifierTypeVehicule = FindViewById<Button>(Resource.Id.btnModifierTypeVehicule);
        btnModifierTypeVehicule.Click += async (sender, e) =>
        {
            if (edtTypeTypeVehicule.Text.Length > 0 || int.Parse(edtNbPersonneTypeVehicule.Text) > 0 )
			{
                try
                {
                    string typeTypeVehicule = edtTypeTypeVehicule.Text;
					int codeTypeVehicule = int.Parse(edtCodeTypeVehicule.Text);
					int nbPersonneTypeVehicule = int.Parse(edtNbPersonneTypeVehicule.Text);

					TypesVehiculeDTO typeDTO = new TypesVehiculeDTO(typeTypeVehicule, codeTypeVehicule, nbPersonneTypeVehicule);
                    
                    await WebAPI.Instance.PostAsync("http://" + GetString(Resource.String.host) + ":" + GetString(Resource.String.port) + "/TypesVehicule/ModifierTypesVehicule", typeDTO);
                    DialoguesUtils.AfficherToasts(this,"Le type "+ typeTypeVehicule + " est modifié !!!");
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

        checkBoxModification = FindViewById<CheckBox>(Resource.Id.checkBoxTypeVehiculeModifier);
        checkBoxModification.CheckedChange += (sender, e) =>
        {
            bool isChecked = checkBoxModification.Checked;
            btnModifierTypeVehicule.Enabled = isChecked;
            edtTypeTypeVehicule.Enabled = isChecked;
			edtCodeTypeVehicule.Enabled = isChecked;
			edtNbPersonneTypeVehicule.Enabled = isChecked;
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
            //Ajouter l'obtention du type par l'API
            lblTypeTypeVehicule.Text = paramTypeTypeVehicule;
            edtTypeTypeVehicule.Text = paramTypeTypeVehicule;
			lblNbPersonneTypeVehicule.Text = paramNbPersonneTypeVehicule.ToString();
            edtNbPersonneTypeVehicule.Text = paramNbPersonneTypeVehicule.ToString();
            lblCodeTypeVehicule.Text = paramCodeTypeVehicule.ToString();
			edtCodeTypeVehicule.Text = paramCodeTypeVehicule.ToString();


			btnModifierTypeVehicule.Enabled = false;
            edtTypeTypeVehicule.Enabled = false;
			edtNbPersonneTypeVehicule.Enabled = false;

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
        MenuInflater.Inflate(Resource.Menu.TypeVehiculeDetailsModifierOptionsMenu, menu);
        return base.OnCreateOptionsMenu(menu);
    }

    /// <summary>Méthode de service permettant de capter l'évenement exécuté lors d'un choix dans le menu.</summary>
    /// <param name="item">L'item sélectionné.</param>
    /// <returns>Retourne si un option à été sélectionné avec succès.</returns>
    public override bool OnOptionsItemSelected(IMenuItem item)
    {
        switch (item.ItemId)
        {
            case Resource.Id.SupprimerTypeVehiculeDM:
                try
                {
                    AlertDialog.Builder builder = new AlertDialog.Builder(this);
                    builder.SetPositiveButton("Non", (send, args) => { });
                    builder.SetNegativeButton("Oui", async (send, args) =>
                    {
                        await WebAPI.Instance.PostAsync("http://" + GetString(Resource.String.host) + ":" + GetString(Resource.String.port) + "/TypesVehicule/SupprimerTypesVehicule?code=" + paramCodeTypeVehicule, null);
                        Finish();
                    });
                    AlertDialog dialog = builder.Create();
                    dialog.SetTitle("Suppression");
                    dialog.SetMessage("Voulez-vous vraiment supprimer le type " + paramCodeTypeVehicule + "?");
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
