using Android.Content;
using Android.Views;
using Newtonsoft.Json;

using AppMobile_ProjetPompierC;
using AppMobile_ProjetPompierC.DTO;
using AppMobile_ProjetPompierC.Utils;

/// <summary>
/// Namespace pour les classes de type Vue.
/// </summary>
namespace ProjetPompier_Mobile.Vues
{
    /// <summary>
    /// Classe de type Activité pour la modification d'un Cégep.
    /// </summary>
    [Activity(Label = "@string/app_name")]
    public class PompierModifierActivity : Activity
    {
        #region Proprietes

        /// <summary>
        /// Attribut représentant le nom de la caserne.
        /// </summary>
        private string paramNomCaserne;

        /// <summary>
        /// Attribut représentant le matricule du pompier.
        /// </summary>
        private int paramMatriculePompier;

        /// <summary>
        /// Attribut représentant le pompier en objet PompierDTO.
        /// </summary>
        private PompierDTO lePompier;

        /// <summary>
        /// Attribut représentant le champ d'affichage du matricule du pompier.
        /// </summary>
        private TextView edtMatriculePompierModifier;

        /// <summary>
        /// Attribut représentant le champ d'affichage du grade du pompier.
        /// </summary>
        private TextView edtGradePompierModifier;

        /// <summary>
        /// Attribut représentant le champ d'affichage du nom du pompier.
        /// </summary>
        private TextView edtNomPompierModifier;

        /// <summary>
        /// Attribut représentant le champ d'affichage du prenom du pompier.
        /// </summary>
        private TextView edtPrenomPompierModifier;

        /// <summary>
        /// Bouton pour ajouter un pompier dans la caserne.
        /// </summary>
        private Button btnModifierPompier;

        #endregion Proprietes

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.InterfacePompierModifier);

            paramNomCaserne = Intent.GetStringExtra("paramNomCaserne");
            paramMatriculePompier = Intent.GetIntExtra("paramMatriculePompier", 0);

            edtMatriculePompierModifier = FindViewById<TextView>(Resource.Id.tvMatriculePompierModifier);
            edtGradePompierModifier = FindViewById<TextView>(Resource.Id.edtGradePompierModifier);
            edtNomPompierModifier = FindViewById<TextView>(Resource.Id.edtNomPompierModifier);
            edtPrenomPompierModifier = FindViewById<TextView>(Resource.Id.edtPrenomPompierModifier);

            btnModifierPompier = FindViewById<Button>(Resource.Id.btnModifierPompier);
            btnModifierPompier.Click += async (sender, e) =>
            {
                if ((edtGradePompierModifier.Text.Length > 0) && (edtNomPompierModifier.Text.Length > 0) && (edtPrenomPompierModifier.Text.Length > 0))
                {
                    try
                    {
                        string lePompierModifier = edtGradePompierModifier.Text + " " + edtNomPompierModifier.Text + " " + edtPrenomPompierModifier.Text;

                        PompierDTO pompierDTO = new PompierDTO
                        {
                            Matricule = paramMatriculePompier,
                            Grade = edtGradePompierModifier.Text,
                            Nom = edtNomPompierModifier.Text,
                            Prenom = edtPrenomPompierModifier.Text
                        };
                        await WebAPI.Instance.PostAsync("http://" + GetString(Resource.String.host) + ":" + GetString(Resource.String.port) + "/Pompier/ModifierPompier?nomCaserne="+paramNomCaserne, pompierDTO);
                        DialoguesUtils.AfficherToasts(this, lePompierModifier + " est modifié !!!");
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
        /// Méthode de service appelée lors du retour en avant plan de l'activité.
        /// </summary>
        protected async override void OnResume()
        {
            base.OnResume();
            await RafraichirInterfaceDonnees();
        }

        /// <summary>
        /// Méthode permettant de rafraichir les informations du Cégep...
        /// </summary>
        private async Task RafraichirInterfaceDonnees()
        {
            try
            {
                string jsonResponse = await WebAPI.Instance.ExecuteGetAsync("http://" + GetString(Resource.String.host) + ":" + GetString(Resource.String.port) + "/Pompier/ObtenirPompier?nomCaserne=" + paramNomCaserne + " &matriculePompier=" + paramMatriculePompier);
                lePompier = JsonConvert.DeserializeObject<PompierDTO>(jsonResponse);
                edtMatriculePompierModifier.Text = lePompier.Matricule.ToString();
                edtGradePompierModifier.Text = lePompier.Grade;
                edtNomPompierModifier.Text = lePompier.Nom;
                edtPrenomPompierModifier.Text = lePompier.Prenom;

            }
            catch (Exception)
            {
                Finish();
            }
        }

        /// <summary>Méthode de service permettant d'initialiser le menu de l'activité principale.</summary>
        /// <param name="menu">Le menu à construire.</param>
        /// <returns>Retourne True si l'optionMenu est bien créé.</returns>
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.PompierModifierOptionsMenu, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        /// <summary>Méthode de service permettant de capter l'évenement exécuté lors d'un choix dans le menu.</summary>
        /// <param name="item">L'item sélectionné.</param>
        /// <returns>Retourne si un option à été sélectionné avec succès.</returns>
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.RetourPompierModifier:
                    Finish();
                    break;

                case Resource.Id.QuitterPompierModifier:
                    FinishAffinity();
                    break;
            }
            return base.OnOptionsItemSelected(item);
        }
    }
}