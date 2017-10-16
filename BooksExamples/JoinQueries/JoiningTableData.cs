// JoiningTableData.cs
// Using LINQ to perform a join and aggregate data across tables.
using System;
using System.Linq;
using System.Windows.Forms;

namespace JoinQueries
{
    public partial class JoiningTableData : Form
    {
        public JoiningTableData()
        {
            InitializeComponent();
        } // end constructor

        private void JoiningTableData_Load(object sender, EventArgs e)
        {
            // Entity Framework DBContext
            BooksEntities dbcontext =
               new BooksEntities();

            // get authors and ISBNs of each book they co-authored
            var authorsAndISBNs =
               from author in dbcontext.Authors
               from book in author.Titles
               orderby book.Title1
               select new { book.Title1 ,author.FirstName, author.LastName};

            outputTextBox.AppendText("Titles and Authors");

            // display authors and ISBNs in tabular format
            foreach (var element in authorsAndISBNs)
            {
                outputTextBox.AppendText(
                   String.Format("\r\n\t{0,-50} {1,-10} {2}",
                       element.Title1,element.FirstName, element.LastName));
            } // end foreach

            // get authors and titles of each book they co-authored
            var authorsAndTitles =
                from book in dbcontext.Titles
                from author in book.Authors
              orderby book.Title1,author.LastName, author.FirstName
               select new
               {
                   book.Title1,
                   author.FirstName,
                   author.LastName
                  
               };

            outputTextBox.AppendText("\r\n\r\nAuthors and titles with authors sorted for each title:");

            // display authors and titles in tabular format
            foreach (var element in authorsAndTitles)
            {
                outputTextBox.AppendText(
                   String.Format("\r\n\t{0,-10} {1,-10} {2}",
                      element.FirstName, element.LastName, element.Title1));
            } // end foreach

            // get authors and titles of each book 
            // they co-authored; group by author
            var titlesByAuthor =
               from books in dbcontext.Titles
               orderby books.Title1
               select new
               {
                   Title = books.Title1,
                   Authors = from authors in books.Authors
                             orderby authors.LastName, authors.FirstName
                             select new
                             {
                                 authors.LastName,
                                 authors.FirstName
                             }
               };

            outputTextBox.AppendText("\r\n\r\nTitles grouped by author:");

            // display titles written by each author, grouped by author
            foreach (var title in titlesByAuthor)
            {
                // display author's name
                outputTextBox.AppendText("\r\n\t" + title.Title+ ":");

                // display titles written by that author
                foreach (var names in title.Authors)
                {
                    outputTextBox.AppendText("\r\n\t\t" + names.FirstName + " " + names.LastName);
                } // end inner foreach
            } // end outer foreach
             
                        
               
        } // end method JoiningTableData_Load

        private void outputTextBox_TextChanged(object sender, EventArgs e)
        {

        }
    } // end class JoiningTableData
} // end namespace JoinQueries

