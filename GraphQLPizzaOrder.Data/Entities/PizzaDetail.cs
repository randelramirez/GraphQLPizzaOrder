
using GraphQLPizzaOrder.Data.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GraphQLPizzaOrder.Data.Entities
{
    public class PizzaDetail
    {
        #region Fields

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(40)]
        public string Name { get; set; }

        [Required]
        [StringLength(200)]
        public Toppings Toppings { get; set; }

        [Required]
        public double Price { get; set; }

        [Required]
        public int Size { get; set; }

        [Required]
        public int OrderDetailId { get; set; }

        [ForeignKey("OrderDetailId")]
        public OrderDetail OrderDetail { get; set; }

        #endregion

        #region Ctor

        public PizzaDetail()
        {
        }

        public PizzaDetail(string name, Toppings toppings, double price, int size, int orderDetailsId)
        {
            Name = name;
            Toppings = toppings;
            Price = price;
            Size = size;
            OrderDetailId = orderDetailsId;
        }

        #endregion
    }
}
