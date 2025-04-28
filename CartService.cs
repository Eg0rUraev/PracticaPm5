using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticaBd
{
    public static class CartService
    {
        public static List<CartItem> Items { get; } = new List<CartItem>();

        public static void AddItem(int id, string name, float price, string type)
        {
            Items.Add(new CartItem { Id = id, Name = name, Price = price, Type = type });
        }
        public static void Clear()
        {
            Items.Clear();
        }
        public static float GetTotalPrice()
        {
            return Items.Sum(item => item.Price);
        }
    }
    public class CartItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
        public string Type { get; set; } 
    }
}
