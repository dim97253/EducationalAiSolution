namespace TextExtractor.Entities
{
    public class Section
    {
        public string Title { get; protected set; }
        public string Content { get; protected set; }

        public Section(string title, string content)
        {
            Title = title;
            Content = content;
        }
    }
}
