namespace Models
{
	public class CompletionRequest
	{
		public string model { get; set; }
		public string prompt { get; set; }
		public int max_tokens { get; set; } = 1;
		public int temperature { get; set; } = 0;
	}
}
