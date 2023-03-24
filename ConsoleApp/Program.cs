using Models;
using Services;

var option = 0;
var fileId = "file-ihtoP2c8DQaMmC8V54jeK9yD"; //string.Empty;
var trainingJobId = "ft-diLawtp9wwQVBkoelcAlkOgQ"; //"ft-8RUZvSAlPdDtjzpJhANhx7Fa"; // string.Empty;
var modelId = "ada:ft-luis-beltran-2023-03-24-21-34-56"; // string.Empty;

var openAIService = new OpenAIService();

do
{
	Console.WriteLine("Here is the menu:");
	Console.WriteLine("1. Upload File");
	Console.WriteLine("2. Fine-tune model");
	Console.WriteLine("3. Check fine-tuning job status");
	Console.WriteLine("4. Test the model");
	Console.WriteLine("0. Exit");

	Console.WriteLine("What is your option?");
	option = int.Parse(Console.ReadLine());

	switch (option)
	{
		case 1:
			Console.WriteLine("Enter the jsonL filename (default: sport2_prepared_train.jsonl)");
			var filename = Console.ReadLine();
			if (string.IsNullOrWhiteSpace(filename))
				filename = "sport2_prepared_train.jsonl";

			Console.WriteLine("Enter the purpose (default: fine-tune)");
			var purpose = Console.ReadLine();
			if (string.IsNullOrWhiteSpace(purpose))
				purpose = "fine-tune";

			var fileUploadResponse = await openAIService.UploadFile(filename, purpose);
			fileId = fileUploadResponse?.id;
			Console.WriteLine($"File ID: {fileId}");
			break;

		case 2:
			Console.WriteLine($"Enter the training file ID (default: {fileId})");
			var trainingFileId = Console.ReadLine();
			if (string.IsNullOrWhiteSpace(trainingFileId))
				trainingFileId = fileId;

			var fineTuneCreateRequest = new FineTuneCreateRequest()
			{
				training_file = trainingFileId,
			};

			var fineTuneCreateResponse = await openAIService.CreateFineTune(fineTuneCreateRequest);
			trainingJobId = fineTuneCreateResponse?.id;
			Console.WriteLine($"Training Job ID: {trainingJobId}");
			break;

		case 3:
			Console.WriteLine($"Enter the training job ID (default: {trainingJobId})");
			var fineTuneId = Console.ReadLine();
			if (string.IsNullOrWhiteSpace(fineTuneId))
				fineTuneId = trainingJobId;

			var fineTuneRetrieveResponse = await openAIService.RetrieveFineTune(fineTuneId);
			Console.WriteLine($"Status: {fineTuneRetrieveResponse.status}");

			if (fineTuneRetrieveResponse.status == "succeeded")
				modelId = fineTuneRetrieveResponse.fine_tuned_model;
			break;

		case 4:
			Console.WriteLine($"Enter your query: ");
			var query = Console.ReadLine();
			query += Environment.NewLine + Environment.NewLine + "###" + Environment.NewLine + Environment.NewLine;

			var question = new CompletionRequest()
			{
				prompt = query,
				model = modelId
			};

			var completionResponse = await openAIService.AskQuestion(question);
			Console.WriteLine($"Answer: {completionResponse.choices.FirstOrDefault()?.text}");
			break;

		default:
			break;
	}

	await Task.Delay(4000);
	Console.Clear();
}while(option > 0);
