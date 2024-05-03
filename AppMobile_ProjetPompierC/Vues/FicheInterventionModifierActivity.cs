using Android.Content;
using Android.Views;
using Newtonsoft.Json;

using AppMobile_ProjetPompierC;
using AppMobile_ProjetPompierC.DTO;
using AppMobile_ProjetPompierC.Utils;
using AppMobile_ProjetPompierC.Adapters;

/// <summary>
/// Namespace pour les classes de type Vue.
/// </summary>
namespace ProjetPompier_Mobile.Vues
{
    /// <summary>
    /// Classe de type Activité pour la modification d'une fiche d'intervention.
    /// </summary>
    [Activity(Label = "@string/app_name")]
    public class FicheInterventionModifierActivity : Activity
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
        /// Attribut représentant la date de début de l'intervention.
        /// </summary>
        private string paramDateDebut;

       
        /// <summary>
        /// Attribut représentant la fiche d'intervention.
        /// </summary>
        private FicheInterventionDTO laFiche;
        /// <summary>
        /// Attribut représentant le champ d'affichage du nom du pompier.
        /// </summary>
        private TextView edtCapitaineModifier;

        /// <summary>
        /// Liste deroulante qui contient les grades .
        /// </summary>
        private TextView edtTypeModifier;

        /// <summary>
        /// Grade séléctionné dans le spinner .
        /// </summary>
        string typeSelectionne;

        /// <summary>
        /// Attribut représentant le champ d'affichage du nom du pompier.
        /// </summary>
        private TextView edtAdresseModifier;

        /// <summary>
        /// Attribut représentant le champ d'affichage du prenom du pompier.
        /// </summary>
        private TextView edtResumeModifier;

        /// <summary>
        /// Bouton pour ajouter un pompier dans la caserne.
        /// </summary>
        private Button btnModifierIntervention;

        #endregion Proprietes

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.InterfaceFicheInterventionModifier);

            paramNomCaserne = Intent.GetStringExtra("paramNomCaserne");
            paramMatriculePompier = Intent.GetIntExtra("paramMatriculeCapitaine", 0);
            paramDateDebut = Intent.GetStringExtra("paramDateDebut");

            edtAdresseModifier = FindViewById<TextView>(Resource.Id.edtAdresseModifier);
            edtCapitaineModifier = FindViewById<TextView>(Resource.Id.edtCapitaineModifier);
            edtResumeModifier = FindViewById<TextView>(Resource.Id.edtResumeModifier);
            edtTypeModifier = FindViewById<TextView>(Resource.Id.edtTypeModifier);

           

            btnModifierIntervention = FindViewById<Button>(Resource.Id.btnModifier);
            btnModifierIntervention.Click += async (sender, e) =>
            {
                if ((edtResumeModifier.Text.Length > 0) && (edtAdresseModifier.Text.Length > 0))
                {
                    try
                    {
                        //string interventionModifie = gradeSelectionne + " " + edtNomPompierModifier.Text + " " + edtPrenomPompierModifier.Text;

                        FicheInterventionDTO ficheInterventionDTO = new FicheInterventionDTO
                        {
                            DateDebut = paramDateDebut,
                            Adresse = edtAdresseModifier.Text,
                            TypeIntervention = edtTypeModifier.Text,
                            Resume = edtResumeModifier.Text,
                            MatriculeCapitaine = paramMatriculePompier
                        };
                        await WebAPI.Instance.PostAsync("http://" + GetString(Resource.String.host) + ":" + GetString(Resource.String.port) + "/Intervention/ModifierFicheIntervention?nomCaserne=" + paramNomCaserne, ficheInterventionDTO);
                        DialoguesUtils.AfficherToasts(this, "L'intervention du " + paramDateDebut + " est modifié !!!");
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
        /// Méthode permettant de rafraichir les informations de la fiche...
        /// </summary>
        private async Task RafraichirInterfaceDonnees()
        {
            try
            {
                string jsonResponse = await WebAPI.Instance.ExecuteGetAsync("http://" + GetString(Resource.String.host) + ":" + GetString(Resource.String.port) + "/Intervention/ObtenirFicheIntervention?nomCaserne=" + paramNomCaserne + " &matriculeCapitaine=" + paramMatriculePompier + "&dateIntervention=" + paramDateDebut);
                laFiche = JsonConvert.DeserializeObject<FicheInterventionDTO>(jsonResponse);
                edtResumeModifier.Text = laFiche.Resume;
                edtTypeModifier.Text = laFiche.TypeIntervention;
                edtAdresseModifier.Text = laFiche.Adresse;
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
            MenuInflater.Inflate(Resource.Menu.FicheInterventionModifierOptionsMenu, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        /// <summary>Méthode de service permettant de capter l'évenement exécuté lors d'un choix dans le menu.</summary>
        /// <param name="item">L'item sélectionné.</param>
        /// <returns>Retourne si un option à été sélectionné avec succès.</returns>
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.Retour:
                    Finish();
                    break;

                case Resource.Id.Quitter:
                    FinishAffinity();
                    break;
            }
            return base.OnOptionsItemSelected(item);
        }
    }
}