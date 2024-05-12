using Android.Content;
using Android.Views;
using Newtonsoft.Json;
using AlertDialog = Android.App.AlertDialog;
using AppMobile_ProjetPompierC.Adapters;
using AppMobile_ProjetPompierC.DTO;
using AppMobile_ProjetPompierC.Utils;

namespace AppMobile_ProjetPompierC.Vues;

[Activity(Label = "TypeInterventionActivity")]
public class TypeInterventionActivity : Activity
{
    #region Proprietes
    /// <summary>
    /// La liste des types d'intervention.
    /// </summary>
    private List<TypeInterventionDTO> listeTypeIntervention;

    /// <summary>
    /// L'adapteur pour la liste des types d'intervention.
    /// </summary>
    private ListeTypeInterventionAdapter adapteurListeTypeIntervention;

    /// <summary>
    /// Le listView pour afficher les types d'intervention.
    /// </summary>
    private ListView listViewTypeIntervention;

    /// <summary>
    /// Le code du type d'intervention.
    /// </summary>
    private EditText edtCodeTypeIntervention;

    /// <summary>
    /// La description du type d'intervention.
    /// </summary>
    private EditText edtDescriptionTypeIntervention;

    /// <summary>
    /// Le bouton pour ajouter un type d'intervention.
    /// </summary>
    private Button btnAjouterTypeIntervention;
    #endregion Proprietes

    #region Methodes

    /// <summary>
    /// M�thode de service appel�e lors de la cr�ation de l'activit�.
    /// </summary>
    /// <param name="savedInstanceState">�tat de l'activit�.</param>
    protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        SetContentView(Resource.Layout.InterfaceTypeInterventionActivity);

        edtCodeTypeIntervention = FindViewById<EditText>(Resource.Id.edtCodeTypeInterventionAjout);
        edtDescriptionTypeIntervention = FindViewById<EditText>(Resource.Id.edtDescriptionTypeInterventionAjout);

        listViewTypeIntervention = FindViewById<ListView>(Resource.Id.listViewTypeIntervention);

        btnAjouterTypeIntervention = FindViewById<Button>(Resource.Id.btnAjouterTypeIntervention);
        btnAjouterTypeIntervention.Click += async (sender, e) =>
        {
            if((edtCodeTypeIntervention.Text.Length > 0) && (edtDescriptionTypeIntervention.Text.Length > 0))
            {
                try
                {
                    string description = edtDescriptionTypeIntervention.Text;
                    TypeInterventionDTO typeIntervention = new TypeInterventionDTO
                    {
                        Code = int.Parse(edtCodeTypeIntervention.Text),
                        Description = description
                    };
                    await WebAPI.Instance.PostAsync("http://" + GetString(Resource.String.host) + ":" + GetString(Resource.String.port) + "/TypesIntervention/AjouterTypeIntervention", typeIntervention);
                    await RafraichirInterfaceDonnees();
                    DialoguesUtils.AfficherToasts(this, description + " ajout� !!!");
                }
                catch (Exception ex)
                {
                    AlertDialog.Builder alert = new AlertDialog.Builder(this);
                    alert.SetTitle("Erreur");
                    alert.SetMessage("Erreur lors de l'ajout du type d'intervention: ");
                    alert.SetPositiveButton("Ok", (senderAlert, args) => { });
                    Dialog dialog = alert.Create();
                    dialog.Show();
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
    /// M�thode permettant de rafraichir la liste des types...
    /// </summary>
    private async Task RafraichirInterfaceDonnees()
    {
        try
        { 
            string jsonResponse = await WebAPI.Instance.ExecuteGetAsync("http://" + GetString(Resource.String.host) + ":" + GetString(Resource.String.port) + "/TypesIntervention/ObtenirListeTypesIntervention");
            listeTypeIntervention = JsonConvert.DeserializeObject<List<TypeInterventionDTO>>(jsonResponse);
            adapteurListeTypeIntervention = new ListeTypeInterventionAdapter(this, listeTypeIntervention.ToArray());
            listViewTypeIntervention.Adapter = adapteurListeTypeIntervention;
            edtCodeTypeIntervention.Text = edtDescriptionTypeIntervention.Text = "";
            edtCodeTypeIntervention.RequestFocus();
        }
        catch (Exception e)
        {
            AlertDialog.Builder alert = new AlertDialog.Builder(this);
            alert.SetTitle("Erreur");
            alert.SetMessage("Erreur lors de la r�cup�ration des types d'intervention: ");
            alert.SetPositiveButton("Ok", (senderAlert, args) => { });
            Dialog dialog = alert.Create();
            dialog.Show();
        }
    }

    /// <summary>M�thode de service permettant d'initialiser le menu de l'activit�.</summary>
    /// <param name="menu">Le menu � construire.</param>
    /// <returns>Retourne True si l'optionMenu est bien cr��.</returns>
    public override bool OnCreateOptionsMenu(IMenu menu)
    {
        MenuInflater.Inflate(Resource.Menu.TypeInterventionActivityOptionsMenu, menu);
        return base.OnCreateOptionsMenu(menu);
    }

    /// <summary>M�thode de service permettant de capter l'�venement ex�cut� lors d'un choix dans le menu.</summary>
    /// <param name="item">L'item s�lectionn�.</param>
    /// <returns>Retourne si une option � �t� s�lectionn� avec succ�s.</returns>
    public override bool OnOptionsItemSelected(IMenuItem item)
    {
        switch (item.ItemId)
        {
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