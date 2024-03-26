using System.Text;
using TextExtractor.Entities;

namespace TextExtractor
{
    public class TextExtractorService
    {
        public List<Section> Sections { get; protected set; }

        public TextExtractorService(string filePath)
        {
            Sections = ParseText(filePath);
        }
        public Section? GetChapter(string chapterName)
        {
            return Sections.Where(c => c.Title == chapterName).FirstOrDefault();
        }
        private List<Section> ParseText(string filePath)
        {
            var sections = new List<Section>();
            string currentTitle = null;
            StringBuilder contentBuilder = new StringBuilder();

            foreach (var line in File.ReadLines(filePath))
            {
                if (line.StartsWith("<Title:"))
                {
                    // Save the previous section if it exists
                    if (currentTitle != null)
                    {
                        var section = new Section(currentTitle, contentBuilder.ToString().Trim());
                        sections.Add(section);
                    }

                    // Start a new section
                    currentTitle = line.Substring(7, line.Length - 8); // Remove <Title: and >
                    contentBuilder.Clear();
                }
                else if (line.StartsWith("<Content:"))
                {
                    // Just clear the content builder if it's a new content tag
                    if (currentTitle != null)  // Only clear if we have started a new section
                    {
                        contentBuilder.Clear();
                    }
                }
                else
                {
                    // Accumulate the content
                    contentBuilder.AppendLine(line);
                }
            }

            // Add the last section if it exists
            if (currentTitle != null)
            {
                var section = new Section(currentTitle, contentBuilder.ToString().Trim());
                sections.Add(section);
            }

            return sections;
        }
    }
}