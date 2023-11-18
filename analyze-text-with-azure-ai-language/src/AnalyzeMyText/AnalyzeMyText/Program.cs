using Azure;
using Azure.AI.TextAnalytics;
using Microsoft.Extensions.Configuration;

IConfigurationRoot configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

string azureAiEndpoint = configuration["AzureAIEndpoint"];
string azureAiKey = configuration["AzureAIKey"];

// Create the Azure AI Text Client;
AzureKeyCredential keyCredential = new AzureKeyCredential(azureAiKey);
Uri endpoint = new Uri(azureAiEndpoint);
TextAnalyticsClient textAnalyticsClient = new TextAnalyticsClient(endpoint, keyCredential);

// Provide some input for the AI Text client to analyze
Console.WriteLine("----------");
Console.WriteLine("Welcome to AnalyzeMyText");
Console.WriteLine("----------");

Console.Write("Please enter some text for the AI text client to analyze: ");
string userInput = Console.ReadLine();

Console.WriteLine($"You wrote: {userInput}");

// Get the language of the input
DetectedLanguage detectedLanguage = textAnalyticsClient.DetectLanguage(userInput);
Console.WriteLine($"The detected language of the input is: {detectedLanguage.Name}");
Console.WriteLine($"Confidence score of the detection: {detectedLanguage.ConfidenceScore}");

// Evaluate the sentiment of the input
DocumentSentiment documentSentiment = textAnalyticsClient.AnalyzeSentiment(userInput);
Console.WriteLine($"The overall sentiment of the input is: {documentSentiment.Sentiment}");
Console.WriteLine($"Positive sentiment score: {documentSentiment.ConfidenceScores.Positive}");
Console.WriteLine($"Neutral sentiment score: {documentSentiment.ConfidenceScores.Neutral}");
Console.WriteLine($"Negative sentiment score: {documentSentiment.ConfidenceScores.Negative}");

// Identify key phrases in the text
KeyPhraseCollection phrases = textAnalyticsClient.ExtractKeyPhrases(userInput);
if (phrases.Count > 0)
{
    Console.WriteLine("I found some key phrases in that sentence! Let's print them out: ");
    foreach (var phrase in phrases)
    {
        Console.WriteLine($"\t{phrase}");
    }
}
else
{
    Console.WriteLine("I didn't find any phrases in that sentence");
}

// Get the entities from our input
CategorizedEntityCollection entities = textAnalyticsClient.RecognizeEntities(userInput);
if (entities.Count > 0)
{
    Console.WriteLine("I found some entities in that sentence! Let's print them out: ");
    foreach (var entity in entities)
    {
        Console.WriteLine($"\t{entity.Text} ({entity.Category} | {entity.SubCategory})");
    }
}
else
{
    Console.WriteLine("I didn't find any entities in that sentence");
}

// Extract the linked entities from the input
LinkedEntityCollection linkedEntities = textAnalyticsClient.RecognizeLinkedEntities(userInput);
if (linkedEntities.Count > 0)
{
    Console.WriteLine("I found some interesting articles from Wikipedia that might interest you!");
    foreach (var entity in linkedEntities)
    {
        Console.WriteLine($"\t {entity.Name} ({entity.Url})");
    }
}
else
{
    Console.WriteLine("I didn't find any interesting articles related to this text :(");
}