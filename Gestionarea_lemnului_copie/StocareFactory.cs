using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NivelStocareDate;
using System.Configuration;
using System.IO;

namespace Gestionarea_lemnului_copie
{
	public static class StocareFactory
	{
		private const string FORMAT_SALVARE = "FormatSalvare";
		private const string NUME_FISIER = "NumeFisier";

		public static IStocareClienti GetStocareClienti()
		{
			string formatSalvare = ConfigurationManager.AppSettings[FORMAT_SALVARE] ?? "";
			string numeFisier = ConfigurationManager.AppSettings[NUME_FISIER] ?? "";
			string locatieFisierSolutie = Directory.GetParent(Directory.GetCurrentDirectory())?.Parent?.Parent?.FullName ?? "";
			string caleCompletaFisier = locatieFisierSolutie + "\\" + numeFisier;

			switch (formatSalvare)
			{
				default:
				case "memorie":
					return new AdministrareClientiMemorie();
				case "txt":
					return new AdministrareClientiFisierText(caleCompletaFisier + "." + formatSalvare);
			}
		}

		public static IStocareLemnBrut GetStocareLemnBrut()
        {
            string formatSalvare = ConfigurationManager.AppSettings[FORMAT_SALVARE] ?? "";
            string locatieFisierSolutie = Directory.GetParent(Directory.GetCurrentDirectory())?.Parent?.Parent?.FullName ?? "";
            string numeFisier = ConfigurationManager.AppSettings[NUME_FISIER] ?? "date";
            string caleCompletaFisier = locatieFisierSolutie + "\\" + numeFisier + "_lemn.txt";

            switch (formatSalvare)
            {

                case "txt":
                    return new AdministrareLemnBrutFisierText(caleCompletaFisier);
                default:
                    return new AdministrareLemnBrutMemorie();

            }
        }
	}
}