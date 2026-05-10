namespace LibrarieModele
{
	public enum TipLemnEnum
	{
		Molid,
		Brad,
		Fag,
		Stejar,
		Pin
	}

	public class LemnBrut
	{
		private const char SEPARATOR_PRINCIPAL_FISIER = ';';
		private const int ID = 0;
		private const int TIP_LEMN = 1;
		private const int CANTITATE = 2;

		public int Id { get; set; }
		public TipLemnEnum TipLemn { get; set; }
		public double CantitateMc { get; set; }

		public LemnBrut() { }

		public LemnBrut(string linieFisier)
		{
			string[] dateFisier = linieFisier.Split(SEPARATOR_PRINCIPAL_FISIER);
			this.Id = Convert.ToInt32(dateFisier[ID]);
			this.TipLemn = (TipLemnEnum)Enum.Parse(typeof(TipLemnEnum), dateFisier[TIP_LEMN]);
			this.CantitateMc = Convert.ToDouble(dateFisier[CANTITATE]);
		}

		public string ConversieLaSirPentruFisier()
		{
			return string.Format("{1}{0}{2}{0}{3}",
				SEPARATOR_PRINCIPAL_FISIER,
				Id.ToString(),
				TipLemn.ToString(),
				CantitateMc.ToString());
		}
	}
}