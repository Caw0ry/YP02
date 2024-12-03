using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GoidaLibrary
{
    public class Bookmark : AbstractItem 
    {
        public Bookmark(string isbn, string Name, string Edition, int Quantity, double Price)
        : base(isbn, Name, Edition, Quantity, Price) 
        {
         
        }
        public Bookmark() : base() 
        { 

        }
        public override void IsFormValid() 
        {
            if (string.IsNullOrWhiteSpace(Isbn) ||
            String.IsNullOrWhiteSpace(Name) ||
            String.IsNullOrWhiteSpace(Edition) ||
            String.IsNullOrWhiteSpace(Price.ToString()) ||
            String.IsNullOrWhiteSpace(Quantity.ToString()))
            {
                throw new ArgumentNullException("One or more fields are not set!");
            }
        }
        public override string ToString() 
        {
            return $"Bookmark Art: {Isbn} | Name: {Name} | Date of Print: {DateOfPrint} Edition: {Edition} | " +
            $" Quantity in Shop: {Quantity} | Price: {Price:C}";
        }
    }
}
