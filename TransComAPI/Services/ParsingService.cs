using TransComAPI.Models;
using System;
using static TRAN;

namespace TransComAPI.Services
{
    public class ParsingService
    {
        // Cette méthode utilise le buffer pour créer et renvoyer un objet Test_Helper_t
        public Test_Helper_t Parse(byte[] buffer)
        {
            // Créer une nouvelle instance de la classe TRAN (ou une autre classe appropriée)
            // et l'utiliser pour le processus de parsing.
            // Vous devriez remplacer 'TRAN' par le nom réel de votre classe de parsing,
            // et 'ParseRecordData' par le nom réel de la méthode de parsing.

            // Supposons que TRAN est la classe qui contient la méthode ParseRecordData,
            // et que vous avez déjà cette méthode définie dans TRAN.cs.
            TRAN parser = new TRAN();
            return parser.ParseRecordData(buffer);
        }

        // Si votre méthode de parsing est statique, vous n'avez pas besoin d'instancier la classe.
        // Il suffirait alors d'appeler directement la méthode statique, par exemple :
        // return TRAN.ParseRecordData(buffer);

        // Méthode utilitaire pour convertir les données binaires à partir d'une chaîne encodée
        public byte[] ConvertFromBase64String(string base64EncodedData)
        {
            // Convertit une chaîne encodée en Base64 en tableau d'octets
            return Convert.FromBase64String(base64EncodedData);
        }
    }
}
