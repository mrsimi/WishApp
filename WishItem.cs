using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WishAppAzFunction
{
    public class WishItem
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public string Sender {get; set;}
    }
}