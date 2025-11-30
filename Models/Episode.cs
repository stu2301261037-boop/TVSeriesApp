using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TVSeriesApp.Models
{
	public class Episode
	{
		public int Id { get; set; }

		[Required(ErrorMessage = "Заглавието е задължително")]
		[StringLength(100, ErrorMessage = "Заглавието не може да бъде повече от 100 символа")]
		public string Title { get; set; }

		[Range(1, 50, ErrorMessage = "Номерът на сезона трябва да е между 1 и 50")]
		[Display(Name = "Сезон")]
		public int SeasonNumber { get; set; }

		[Range(1, 100, ErrorMessage = "Номерът на епизода трябва да е между 1 и 100")]
		[Display(Name = "Епизод")]
		public int EpisodeNumber { get; set; }

		[DataType(DataType.Date)]
		[Display(Name = "Дата на излъчване")]
		public DateTime AirDate { get; set; }

		[Range(1, 180, ErrorMessage = "Продължителността трябва да е между 1 и 180 минути")]
		[Display(Name = "Продължителност (мин)")]
		public int Duration { get; set; }

		[Range(0.0, 10.0, ErrorMessage = "Рейтингът трябва да е между 0.0 и 10.0")]
		[Column(TypeName = "decimal(3,1)")]
		public decimal Rating { get; set; }

		// Връзка със сериала
		public int SeriesId { get; set; }
		public virtual Series Series { get; set; }
	}
}