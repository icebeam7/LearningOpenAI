namespace Models
{
	public class FineTuneCreateResponse
	{
		public string id { get; set; }
		public string _object { get; set; }
		public string model { get; set; }
		public long created_at { get; set; }
		public Event[] events { get; set; }
		public string? fine_tuned_model { get; set; }
		public Hyperparams hyperparams { get; set; }
		public string organization_id { get; set; }
		public string status { get; set; }
		public long updated_at { get; set; }
	}

	public class Hyperparams
	{
		public int? batch_size { get; set; }
		public float? learning_rate_multiplier { get; set; }
		public int n_epochs { get; set; }
		public float prompt_loss_weight { get; set; }
	}

	public class Event
	{
		public string _object { get; set; }
		public long created_at { get; set; }
		public string level { get; set; }
		public string message { get; set; }
	}
}
