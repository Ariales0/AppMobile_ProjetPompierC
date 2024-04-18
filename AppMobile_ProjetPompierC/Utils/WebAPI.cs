using System.Text;
using Newtonsoft.Json;


/// <summary>
/// Namespace contenant les classes d'utilisation autres.
/// </summary>
namespace ProjetPompier_WEB.Utils
{
    /// <summary>
    /// Classe repr�sentant l'outil de communication vers le service Web distant.
    /// </summary>
    public class WebAPI
    {
        /// <summary>
        /// Instance unique de la classe.
        /// </summary>
        private static WebAPI instance;

        /// <summary>
        /// Le constructeur priv� de la classe.
        /// </summary>
        private WebAPI() { }

        /// <summary>
        /// M�thode permettant d'obtenir l'instance unique de la classe.
        /// </summary>
        public static WebAPI Instance
        {
            get
            {
                if (instance == null)
                    instance = new WebAPI();
                return instance;
            }
        }

        /// <summary>
        /// M�thode permettant d'ex�cuter une commande GET.
        /// </summary>
        /// <param name="url">L'url pour la commande GET.</param>
        /// <returns>Le retour de la commande en format JSON.</returns>
        public async Task<string> ExecuteGetAsync(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        return responseBody;
                    }
                    else
                    {
                        // G�rer les erreurs ici si n�cessaire
                        return "Erreur : " + response.StatusCode;
                    }
                }
                catch (Exception ex)
                {
                    // G�rer les exceptions ici si n�cessaire
                    return "Exception : " + ex.Message;
                }
            }
        }

        /// <summary>
        /// M�thode permettant d'envoyer une commande POST.
        /// </summary>
        /// <param name="url">L'url pour la commande POST.</param>
        /// <param name="dto">Le DTO d�sir�.</param>
        public async Task PostAsync(string url, object dto)
        {
            HttpClient client = new HttpClient
            {
                MaxResponseContentBufferSize = 256000
            };

            StringContent content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync(new Uri(url), content);

            if (response.IsSuccessStatusCode)
                Console.Error.WriteLine("L'appel POST a r�ussi.");
            else
                Console.Error.WriteLine("Erreur : " + response.ReasonPhrase);
        }
    }
}