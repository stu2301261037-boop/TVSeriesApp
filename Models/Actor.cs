using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TVSeriesApp.Models
{
	public class Actor
	{
		public int Id { get; set; }

		[Required(ErrorMessage = "Името е задължително")]
		[StringLength(50, ErrorMessage = "Името не може да бъде повече от 50 символа")]
		[Display(Name = "Име")]
		public string FirstName { get; set; }

		[Required(ErrorMessage = "Фамилията е задължителна")]
		[StringLength(50, ErrorMessage = "Фамилията не може да бъде повече от 50 символа")]
		[Display(Name = "Фамилия")]
		public string LastName { get; set; }

		[DataType(DataType.Date)]
		[Display(Name = "Дата на раждане")]
		public DateTime BirthDate { get; set; }

		[StringLength(50, ErrorMessage = "Националността не може да бъде повече от 50 символа")]
		public string Nationality { get; set; }

		[Range(0, 10000000, ErrorMessage = "Заплатата трябва да е положително число")]
		[Column(TypeName = "decimal(18,2)")]
		[Display(Name = "Заплата")]
		public decimal Salary { get; set; }

		[Display(Name = "Носител на награди")]
		public bool IsAwardWinner { get; set; }


		// Навигационно свойство
		public virtual ICollection<SeriesActor> SeriesActors { get; set; } = new List<SeriesActor>();

		[NotMapped]
		public string FullName => $"{FirstName} {LastName}";

	}
}