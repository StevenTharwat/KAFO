﻿using KAFO.Domain.Statics;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KAFO.Domain.Products
{
    public class Product : Base
    {
        public int Id { set; get; }
        public string Name { set; get; }
        public string? ImageUrl { set; get; }

        public decimal AveragePurchasePrice { set; get; }
        public decimal LastPurchasingPrice { set; get; }
        [Range(20, 100, ErrorMessage = "حااااااسب")]
        public decimal SellingPrice { set; get; }
        public decimal StockQuantity { get; set; }
        public bool IsActive { get; set; }
        [ForeignKey(nameof(Category))]
        public int CategoryId { get; set; }
        public virtual Category? Category { set; get; }


        //for ASP.NET Binding 
        public Product()
        {

        }
        public Product(string name, decimal purchasingPrice, decimal sellingPrice, int initQuantity = 0, string? imageUrl = null, Category? category = null)
        {
            Name = name;
            ImageUrl = imageUrl;
            AveragePurchasePrice = purchasingPrice;
            LastPurchasingPrice = purchasingPrice;
            StockQuantity = initQuantity;
            SellingPrice = sellingPrice;
            Category = category;
        }

        public void IncreaseStockQuantity(decimal quantity)
        {
            if (quantity > 0)
                StockQuantity += quantity;
            else
                throw new Exception(Messages.NegativeQuantityException);
        }
        public void DecreaseStockQuantity(decimal quantity)
        {
            if (quantity > 0 && ( StockQuantity - quantity ) >= 0)
                StockQuantity -= quantity;
            else
                throw new Exception(Messages.NegativeQuantityException);
        }
        public decimal ChangePurchasingPriceAndQuantity(decimal newPurchasingPrice, decimal newSellingPrice, decimal newQuantity)
        {
            decimal totalPaid = ( StockQuantity * AveragePurchasePrice ) + ( newQuantity * newPurchasingPrice );
            decimal totalQuantity = StockQuantity + newQuantity;
            AveragePurchasePrice = totalPaid / totalQuantity;
            //must increase the quantity to be compatible with the new Average price - i can not update the average proce without add the new quantity
            IncreaseStockQuantity(newQuantity);
            SellingPrice = newSellingPrice;
            LastPurchasingPrice = newPurchasingPrice;
            // return the total Revenue if we sell the current stock
            return ( SellingPrice - AveragePurchasePrice ) * totalQuantity;
        }

        public override string ToString()
        {
            return $"product:: Name: {Name}, LastPurchasingPrice: {LastPurchasingPrice}, " +
                $"AveragePurchasePrice: {AveragePurchasePrice}, SellingPrice: {SellingPrice}, " +
                $"StockQuantity: {StockQuantity}";
        }
        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (GetType() != obj.GetType()) return false;
            Product other = (Product) obj;
            return other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode()
                + AveragePurchasePrice.GetHashCode()
                + SellingPrice.GetHashCode();
        }
    }
}