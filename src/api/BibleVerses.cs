using System.Linq.Expressions;
using System.Text.Json;
using LiveStreamTools.Models;

namespace LiveStreamTools.Api
{
    public class BibleVerses
    {
        private static readonly string BaseApiUrl = "https://bible-go-api.rkeplin.com/v1/books";

        public static async Task<string> GetBookIdByGenre(string genre)
        {
            try
            {
                using var httpClient = new HttpClient();
                HttpResponseMessage booksRes = await httpClient.GetAsync(BaseApiUrl);

                if (booksRes.IsSuccessStatusCode)
                {
                    var books = await JsonSerializer.DeserializeAsync<Book[]>(await booksRes.Content.ReadAsStreamAsync());

                    var filteredBooks = books.Where(book => book.genre.name == genre).ToList();

                    if (filteredBooks.Any())
                    {
                        var randomBook = filteredBooks[new Random().Next(0, filteredBooks.Count)];
                        return randomBook.id.ToString();
                    }
                    else
                    {
                        Console.WriteLine($"No books found for genre: {genre}");
                    }
                }
                else
                {
                    Console.WriteLine("Failed to fetch books");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            return null;
        }

        public static async Task<string> GetRandomVerseUrl(string genre, string translation)
        {
            try
            {
                using var httpClient = new HttpClient();
                string bookNum = genre == "All" ? new Random().Next(1, 67).ToString() : await GetBookIdByGenre(genre);

                if (bookNum != null)
                {
                    string bookUrl = $"{BaseApiUrl}/{bookNum}/chapters";
                    HttpResponseMessage bookResponse = await httpClient.GetAsync(bookUrl);

                    if (bookResponse.IsSuccessStatusCode)
                    {
                        var bookData = await JsonSerializer.DeserializeAsync<JsonElement>(await bookResponse.Content.ReadAsStreamAsync());
                        int bookLen = bookData.GetArrayLength();

                        string chapNum = new Random().Next(1, bookLen + 1).ToString();
                        string chapUrl = $"{bookUrl}/{chapNum}";

                        HttpResponseMessage chapResponse = await httpClient.GetAsync(chapUrl);

                        if (chapResponse.IsSuccessStatusCode)
                        {
                            var chapData = await JsonSerializer.DeserializeAsync<JsonElement>(await chapResponse.Content.ReadAsStreamAsync());
                            int chapLen = chapData.GetArrayLength();
                            
                            List<int> verseIds = new List<int>();

                            foreach (var verse in chapData.EnumerateArray())
                            {
                                int verseId = verse.GetProperty("id").GetInt32();
                                verseIds.Add(verseId);
                            }

                            Random random = new Random();
                            int randomVerseIndex = random.Next(0, verseIds.Count);
                            int randomVerseId = verseIds[randomVerseIndex];

                            return $"{chapUrl}/{randomVerseId}?translation={translation}";
                        }
                        else
                        {
                            Console.WriteLine($"Error fetching chapter data: {chapResponse.StatusCode}");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Error fetching book data: {bookResponse.StatusCode}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex);
            }
            return null;
        }

        public static async Task<BibleVerseModel> GetBibleVerse(string genre, string translation)
        {
            try
            {
                string verseUrl = await GetRandomVerseUrl(genre, translation);

                if (verseUrl != null)
                {
                    using var httpClient = new HttpClient();
                    HttpResponseMessage verseResponse = await httpClient.GetAsync(verseUrl);

                    if (verseResponse.IsSuccessStatusCode)
                    {
                        string verseContent = await verseResponse.Content.ReadAsStringAsync();
                        return JsonSerializer.Deserialize<BibleVerseModel>(verseContent);
                    }
                    else
                    {
                        Console.WriteLine($"Error fetching verse: {verseResponse.StatusCode}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            return null;
        }
    }
}