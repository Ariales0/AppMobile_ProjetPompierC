
/// <summary>
/// Namespace pour les classe de type DTOs.
/// </summary>
namespace AppMobile_ProjetPompierC.DTO;

/// <summary>
/// Classe représentant le DTO d'un véhicule.
/// </summary>
public class VehiculeDTO
{
    #region Proprietes
    /// <summary>
    /// Propriété représentant le VIN du véhicule.
    /// </summary>
    public string Vin { get; set; }

    /// <summary>
    /// Propriété représentant le code du véhicule.
    /// </summary>
    public int Code { get; set; }
    /// <summary>
    /// Propriété représentant la marque du véhicule.
    /// </summary>
    public string Marque { get; set; }
    /// <summary>
    /// Propriété représentant le modèle du véhicule.
    /// </summary>
    public string Modele { get; set; }
    /// <summary>
    /// Propriété représentant l'année du véhicule.
    /// </summary>
    public int Annee { get; set; }



    #endregion Proprietes

    #region Constructeurs
    /// <summary>
    /// Constructeur vide.
    /// </summary>
    public VehiculeDTO() 
    { 
    }

    /// <summary>
    /// Constructeur avec paramètres.
    /// </summary>
    /// <param name="vin">Le vin</param>
    /// <param name="typeVehicule">Le type de vehicule</param>
    /// <param name="marque">La marque</param>
    /// <param name="modele">Le modele</param>
    /// <param name="annee">L'annee</param>
    public VehiculeDTO(string vin = "", int code = 0000, string marque = "", string modele = "", int annee = 0000)
    {
	    Vin = vin;
        Code = code;
        Marque = marque;
        Modele = modele;
        Annee = annee;
    }

    



    #endregion Constructeurs
}
