//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ShoppingCart.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Order
    {
        public Order()
        {
            this.Order_Products = new HashSet<Order_Products>();
        }
    
        public int OrderId { get; set; }
        public int CustId { get; set; }
        public System.DateTime OrderDate { get; set; }
        public string OrderStatus { get; set; }
    
        public virtual Customer Customer { get; set; }
        public virtual ICollection<Order_Products> Order_Products { get; set; }
    }
}
