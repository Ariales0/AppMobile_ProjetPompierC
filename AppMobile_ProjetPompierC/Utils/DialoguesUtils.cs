using Android.Views;
/// <summary>
/// 
/// </summary>
namespace AppMobile_ProjetPompierC.Utils
{
    /// <summary>
    /// 
    /// </summary>
    public static class DialoguesUtils
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="message"></param>
        public static void AfficherToasts(Activity sender, string message)
        {
            Toast.MakeText(sender, message, ToastLength.Long).Show();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="titre"></param>
        /// <param name="message"></param>
        public static void AfficherMessageOK(Activity sender, string titre, string message)
        {
            AlertDialog.Builder builder = new AlertDialog.Builder(sender);
            builder.SetPositiveButton("OK", (send, args) => {});
            AlertDialog dialog = builder.Create();
            dialog.SetTitle(titre);
            dialog.SetMessage(message);
            dialog.Show();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="titre"></param>
        /// <param name="question"></param>
        /// <returns></returns>
        public static bool AfficherDialogueQuestionOuiNon(Activity sender, string titre, string question)
        {
            bool reponse = false;
            AlertDialog.Builder builder = new AlertDialog.Builder(sender);
            builder.SetPositiveButton("Non", (send, args) =>
            {
                reponse = false;
            });
            builder.SetNegativeButton("Oui", (send, args) =>
            {
                reponse =  true;
            });
            AlertDialog dialog = builder.Create();
            dialog.SetTitle(titre);
            dialog.SetMessage(question);
            dialog.Window.SetGravity(GravityFlags.Bottom);
            dialog.Show();
            return reponse;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="titre"></param>
        /// <param name="question"></param>
        /// <returns></returns>
        public static string AfficherMessageDialogueQuestionTexte(Activity sender, string titre, string question)
        {
            string reponse = "";
            EditText input = new EditText(sender);

            AlertDialog.Builder builder = new AlertDialog.Builder(sender);
            builder.SetPositiveButton("OK", (send, args) =>
            {
                reponse = input.Text;
            });
            AlertDialog dialog = builder.Create();
            dialog.SetTitle(titre);
            dialog.SetMessage(question);
            dialog.SetView(input);
            dialog.Window.SetGravity(GravityFlags.Bottom);
            dialog.Show();
            return reponse;
        }
    }
}