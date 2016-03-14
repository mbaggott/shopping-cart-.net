namespace ShoppingCart.Models
{
    using System;
    using System.Collections.Generic;

    public partial class Product
    {

        public override bool Equals(object obj)
        {
            var k = obj as Product;
            if (k != null)
            {
                return this.ProductID == k.ProductID;
            }
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return this.ProductID.GetHashCode();
        }
    }
}