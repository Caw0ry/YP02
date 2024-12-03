using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GoidaLibrary
{
    public class Journal : AbstractItem 
    {
        public Journal(string Isbn, string Name, string Edition, int Quantity, double Price)
        : base(Isbn, Name, Edition, Quantity, Price) 
        {

        }
        public Journal() : base() 
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
            return $"Journal | ISBN: {Isbn} | Name: {Name} | Date of Print: {DateOfPrint} Edition: {Edition} | " +
            $" Quantity in Shop: {Quantity} | Price: {Price:C}";
        }
    }
}
