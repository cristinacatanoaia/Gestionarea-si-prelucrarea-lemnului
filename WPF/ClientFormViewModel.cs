using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace WPF
{
	public class ClientFormViewModel : INotifyPropertyChanged, IDataErrorInfo
	{
		private string nume = string.Empty;
		private string telefon = string.Empty;
		private string email = string.Empty;
		private bool numeTouched;
		private bool telefonTouched;
		private bool emailTouched;

		public event PropertyChangedEventHandler? PropertyChanged;

		public string Nume
		{
			get => nume;
			set
			{
				if (nume != value)
				{
					nume = value;
					numeTouched = true;
					OnPropertyChanged();
					OnPropertyChanged(nameof(IsValid));
				}
			}
		}

		public string Telefon
		{
			get => telefon;
			set
			{
				if (telefon != value)
				{
					telefon = value;
					telefonTouched = true;
					OnPropertyChanged();
					OnPropertyChanged(nameof(IsValid));
				}
			}
		}

		public string Email
		{
			get => email;
			set
			{
				if (email != value)
				{
					email = value;
					emailTouched = true;
					OnPropertyChanged();
					OnPropertyChanged(nameof(IsValid));
				}
			}
		}

		public void MarkAllTouched()
		{
			numeTouched = true;
			telefonTouched = true;
			emailTouched = true;
			OnPropertyChanged(nameof(Nume));
			OnPropertyChanged(nameof(Telefon));
			OnPropertyChanged(nameof(Email));
			OnPropertyChanged(nameof(IsValid));
		}

		public string[] GetAllErrors()
		{
			return new[]
			{
				this[nameof(Nume)],
				this[nameof(Telefon)],
				this[nameof(Email)]
			}.Where(error => !string.IsNullOrWhiteSpace(error)).ToArray();
		}

		public string GetFirstError()
		{
			return GetAllErrors().FirstOrDefault() ?? string.Empty;
		}

		public void Clear()
		{
			nume = string.Empty;
			telefon = string.Empty;
			email = string.Empty;
			numeTouched = false;
			telefonTouched = false;
			emailTouched = false;
			OnPropertyChanged(nameof(Nume));
			OnPropertyChanged(nameof(Telefon));
			OnPropertyChanged(nameof(Email));
			OnPropertyChanged(nameof(IsValid));
		}

		public bool IsValid =>
			string.IsNullOrWhiteSpace(this[nameof(Nume)]) &&
			string.IsNullOrWhiteSpace(this[nameof(Telefon)]) &&
			string.IsNullOrWhiteSpace(this[nameof(Email)]);

		public string Error => string.Empty;

		public string this[string columnName]
		{
			get
			{
				switch (columnName)
				{
					case nameof(Nume):
						if (!numeTouched)
						{
							return string.Empty;
						}
						return string.IsNullOrWhiteSpace(Nume) ? "Introduceti nume." : string.Empty;
					case nameof(Telefon):
						if (!telefonTouched)
						{
							return string.Empty;
						}
						if (string.IsNullOrWhiteSpace(Telefon))
						{
							return "Introduceti telefon.";
						}

						if (Telefon.Length != 10 || !Telefon.All(char.IsDigit))
						{
							return "Telefonul trebuie sa aiba 10 cifre.";
						}

						return string.Empty;
					case nameof(Email):
						if (!emailTouched)
						{
							return string.Empty;
						}
						if (string.IsNullOrWhiteSpace(Email))
						{
							return "Introduceti email.";
						}

						return Email.Contains("@") ? string.Empty : "Email invalid.";
					default:
						return string.Empty;
				}
			}
		}

		private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
