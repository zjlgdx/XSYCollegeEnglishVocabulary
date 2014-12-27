﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Storage;

namespace XSYCollegeEnglishVocabulary.DataModel
{
    /// <summary>
    /// Generic group data model.
    /// </summary>
    public class Book
    {
        public Book(String uniqueId, String title)
        {
            this.UniqueId = uniqueId;
            this.Title = title;
        }

        public string UniqueId { get; set; }
        public string Title { get; private set; }

        public override string ToString()
        {
            return this.Title;
        }
    }

    public sealed class BookListDataSource
    {
        private static BookListDataSource _bookListDataSource = new BookListDataSource();

        private ObservableCollection<Book> _books = new ObservableCollection<Book>();
        public ObservableCollection<Book> Books
        {
            get { return this._books; }
        }

        public static async Task<IEnumerable<Book>> GetBooksAsync()
        {
            await _bookListDataSource.GetDataAsync();

            return _bookListDataSource.Books;
        }

        private async Task GetDataAsync()
        {
            if (this._books.Count != 0)
                return;

            Uri dataUri = new Uri("ms-appx:///DataModel/BookListData.json");

            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(dataUri);
            string jsonText = await FileIO.ReadTextAsync(file);
            JsonObject jsonObject = JsonObject.Parse(jsonText);
            JsonArray jsonArray = jsonObject["Books"].GetArray();

            foreach (JsonValue bookValue in jsonArray)
            {
                JsonObject bookObject = bookValue.GetObject();
                Book book = new Book(bookObject["UniqueId"].GetString(), bookObject["Title"].GetString());

                this.Books.Add(book);
            }
        }
    }
}
