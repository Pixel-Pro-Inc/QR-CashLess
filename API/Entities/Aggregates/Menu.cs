using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities.Aggregates
{
    /// <summary>
    /// This is an aggregate of <see cref="MenuItem"/>s. This provides everything that should be in Menu along with List functionality 
    /// </summary>
    public class Menu:BaseAggregates<MenuItem>
    {
        public override string ToString()
        {
            string orderItems = null;
            foreach (MenuItem menuItem in this)
            {
                orderItems = menuItem.Name + " IdentityFied with " + menuItem.Id.ToString() + "\n" + " Costing:" + Price.ToString()+"\n\n";
            }
            return orderItems;
        }

        // TODO: Have this use the baseAggregate property AggreProp for more conciseness
        private float? _price
        {
            get { return _price == null ? throw new NullReferenceException("There are no OrderItems in this Order") : _price; }
            set { _price = value; }
        }
        /// <summary>
        /// This is the Price gotten from the adding all the prices of the <see cref="OrderItem"/>s it has. If it doesn't have an element it throws <see cref="NullReferenceException()"/>
        /// <para> I also made it Immutable/ReadOnly</para>
        /// </summary>
        public float? Price
        {
            get { return _price; }
            init
            {
                if (this.Any())
                {
                    foreach (MenuItem item in this)
                    {
                        _price = +float.Parse(item.Price);
                    }
                }
            }
        }


    }
}
