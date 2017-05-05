namespace UniStore.Models.ViewModels.Product
{
    using System.Collections.Generic;
    using BindingModels.Product;

    public class ProductsListVM
    {
        public SearchProductsBM SearchProductsBM { get; set; }

        public IEnumerable<DetailsProductVM> Products { get; set; }

        public int UserProductsInShoppingCart { get; set; }
    }
}