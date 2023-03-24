namespace Models
{
	public class FineTuneCreateRequest
	{
        public string training_file { get; set; }
        public string model { get; set; } = "ada";
    }
}
