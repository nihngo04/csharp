using QLSach.DTO;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLSach.DAL
{
    class DAL
    {
        private static DAL _Instance;
        public static DAL Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new DAL();
                return _Instance;
            }
            private set { }
        }
        private DAL()
        {

        }
       
        public List<Book> GetBooks(int AuthorID, string property_Name, string property_Value)
        {
            List<Book> searchedBook = GetBooksBy(property_Name, property_Value);
            List<Book> books = new List<Book>();
            if (AuthorID == 0)
            {
                books = searchedBook;
            }
            else
            {
                foreach(var book in searchedBook)
                {
                    if (book.Author_ID == AuthorID)
                        books.Add(book);
                }
            }
            return books;
        }
        private List<Book> GetBooksBy(string property_Name, string value)
        {
            List<Book> books = new List<Book>();
            using (var db = new CSDL())
            {
                if (value == "")
                {
                    books = db.Books.Include("Author").ToList();
                }
                else
                {
                    switch (property_Name)
                    {
                        case "ID":
                            books = db.Books.Where(p => p.ID.ToUpper().Contains(value.ToUpper())).Include("Author").ToList();
                            break;
                        case "Name":
                            books = db.Books.Where(p => p.Name.ToUpper().Contains(value.ToUpper())).Include("Author").ToList();
                            break;
                        case "ReleaseDate":
                            books = db.Books.Where(p => p.ReleaseDate.ToString().Contains(value)).Include("Author").ToList();
                            break;
                        case "IsEbook":
                            books = db.Books.Where(p => p.IsEbook.ToString().Contains(value)).Include("Author").ToList();
                            break;
                    }
                }
                return books;
            }    
        }
        
        public Book Get1Book(string ID)
        {
            using (var db = new CSDL())
            {
                var book = db.Books.Find(ID);
                if (book == null)
                    return new Book();
                else
                    return book;
            }    
        }
       
        public List<Author> GetAllAuthor()
        {
            using (var db = new CSDL())
            {
                return db.Authors.ToList();
            }               
        }
       
        public List<String> GetBookProperty()
        {
            BookViewModel b = new BookViewModel();
            List<string> listProperty = new List<string>();
            foreach(var p in b.GetType().GetProperties())
            {
                listProperty.Add(p.Name);
            }
            return listProperty;
        }
      
        public bool DeleteBooks(List<string> IDs)
        {
            using (var db = new CSDL())
            {
                foreach (string ID in IDs)
                {
                    var book = db.Books.Find(ID);
                    if (book == null)
                        return false;
                    db.Books.Remove(book);
                }
                db.SaveChanges();
                return true;
            }    
        }
       
        public bool AddBook(Book book)
        {
            using (var db = new CSDL())
            {
                db.Books.Add(book);
                db.SaveChanges();
                return true;
            }    
        }
      
        public bool EditBook(Book book)
        {
            using (var db = new CSDL())
            {
                var b = db.Books.Find(book.ID);
                if (b == null)
                    return false;
                else
                {
                    b.Name = book.Name;
                    b.ReleaseDate = book.ReleaseDate;
                    b.IsEbook = book.IsEbook;
                    b.Author_ID = book.Author_ID;
                }

                db.SaveChanges();
                return true;
            }    
        }
        
        public bool IsExist(string ID)
        {
            using (var db = new CSDL())
            {
                var b = db.Books.Find(ID);
                if (b != null)
                    return true;
                return false;
            }    
        }
    }
}
