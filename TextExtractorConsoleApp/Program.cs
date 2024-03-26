using OpenAiConnector;
using TextExtractor;

namespace TextExtractorConsoleApp
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var _openAiService = new OpenAiService();
            var _extractor = new TextExtractorService("Statistics-Through-an-Equity-Lens-1698777422.txt");

            var titles = _extractor.Sections.Select(s => s.Title).ToList<string>(); 

            var chapter1 = _extractor.GetChapter(titles[0]);
            var chapter2 = _extractor.GetChapter(titles[1]);
            var chapter3 = _extractor.GetChapter(titles[2]);
            var chapter4 = _extractor.GetChapter(titles[3]);
            var chapter5 = _extractor.GetChapter(titles[4]);

            var chapterPrompt = string.Format(Messages.ChapterTemplate, chapter4.Title, chapter4.Content);
            var messageSet = new List<string>() { Messages.SystemPrompt, chapterPrompt };
            var messages = _openAiService.BuildMessages(messageSet);

            var learningMaterial = await _openAiService.ChatCompletion(messages);

            messageSet.Add(learningMaterial);
            messageSet.Add(Messages.GenerateTestsPrompt);
            messages = _openAiService.BuildMessages(messageSet);

            var test = await _openAiService.ChatCompletion(messages);
        }
    }
}