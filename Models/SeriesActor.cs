using System.ComponentModel.DataAnnotations;

namespace TVSeriesApp.Models
{
	public class SeriesActor
	{
		public int SeriesId { get; set; }
		public Series Series { get; set; }

		public int ActorId { get; set; }
		public Actor Actor { get; set; }

		[StringLength(100)]
		public string CharacterName { get; set; }
	}
}