using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TVSeriesApp.Models
{
	public class Series
	{
		public int Id { get; set; }

		[Required(ErrorMessage = "Заглавието е задължително")]
		[StringLength(100, ErrorMessage = "Заглавието не може да бъде повече от 100 символа")]
		public string Title { get; set; }

		[StringLength(500, ErrorMessage = "Описанието не може да бъде повече от 500 символа")]
		public string Description { get; set; }

		[DataType(DataType.Date)]
		[Display(Name = "Дата на излизане")]
		public DateTime ReleaseDate { get; set; }

		[Range(1, 50, ErrorMessage = "Сезоните трябва да са между 1 и 50")]
		[Display(Name = "Брой сезони")]
		public int Seasons { get; set; }

		[Range(0.0, 10.0, ErrorMessage = "Рейтингът трябва да е между 0.0 и 10.0")]
		[Column(TypeName = "decimal(3,1)")]
		public decimal Rating { get; set; }

		[Required(ErrorMessage = "Жанрът е задължителен")]
		[StringLength(50, ErrorMessage = "Жанрът не може да бъде повече от 50 символа")]
		public string Genre { get; set; }

		[Display(Name = "Завършен сериал")]
		public bool IsCompleted { get; set; }

        [Display(Name = "Постер URL")]
        [StringLength(500, ErrorMessage = "URL адресът не може да бъде повече от 500 символа")]
        public string PosterUrl { get; set; } = "/images/series/default-series.jpg";

    public virtual ICollection<Episode> Episodes { get; set; } = new List<Episode>();
		

    }
}