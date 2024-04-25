namespace LiveStreamTools.Models
{
    public class BibleVerseModel
    {
        public Book? book {get; set;}
        public int chapterId {get; set;}
        public int verseId {get; set;}
        public string? verse {get; set;}
    }

    public class Book
    {
        public int id { get; set; }

        public string? name { get; set; }

        public Genre? genre { get; set; }
    }
    
    public class Genre
    {
        public string? name { get; set; }
    }
}