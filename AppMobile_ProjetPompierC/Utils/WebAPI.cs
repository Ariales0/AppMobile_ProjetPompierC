using System.Text;
using Newtonsoft.Json;


/// <summary>
/// Namespace contenant les classes d'utilisation autres.
/// </summary>
namespace ProjetPompier_WEB.Utils
{
    /// <summary>
    /// Classe représentant l'outil de communication vers le service Web distant.
    /// </summary>
    public class WebAPI
    {
        /// <summary>
        /// Instance unique de la classe.
        /// </summary>
        private static WebAPI instance;

        /// <summary>
        /// Le constructeur privé de la classe.
        /// </summary>
        private WebAPI() { }

        /// <summary>
        /// Méthode permettant d'obtenir l'instance unique de la classe.
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
        /// Méthode permettant d'exécuter une commande GET.
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
                        // Gérer les erreurs ici si nécessaire
                        return "Erreur : " + response.StatusCode;
                    }
                }
                catch (Exception ex)
                {
                    // Gérer les exceptions ici si nécessaire
                    return "Exception : " + ex.Message;
                }
            }
        }

        /// <summary>
        /// Méthode permettant d'envoyer une commande POST.
        /// </summary>
        /// <param name="url">L'url pour la commande POST.</param>
        /// <param name="dto">Le DTO désiré.</param>
        public async Task PostAsync(string url, object dto)
        {
            HttpClient client = new HttpClient
            {
                MaxResponseContentBufferSize = 256000
            };

            StringContent content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync(new Uri(url), content);

            if (response.IsSuccessStatusCode)
                Console.Error.WriteLine("L'appel POST a réussi.");
            else
                Console.Error.WriteLine("Erreur : " + response.ReasonPhrase);
        }
    }
}