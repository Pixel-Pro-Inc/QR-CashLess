using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities.Aggregates
{
    /// <summary>
    /// This is an aggregate of OrderItems, ( a list of OrderItems). It inherits from <see cref="BaseAggregates{T}"/>
    /// <para> We needed this for a long while but finally was pushed to make it when Abel had to convert object to json in
    /// the extention but the List of List of orderitem  wasn't really working nice with single items like a list of appusers</para>
    /// </summary>
    public class Order : BaseAggregates<OrderItem>
    {

        public override string ToString()
        {
            string orderItems = null;
            foreach (var orderItem in this)
            {
                orderItems = orderItem.Name + " IdentityFied with " + orderItem.Id.ToString() + "\n" + " Costing:" + Price.ToString();
            }
            return orderItems;
        }

        // TODO: Have the aggreProperty come back so that we don't have to have these private properties anymore

        private string _orderNumber
        {
            get { return _orderNumber == null ? throw new NullReferenceException("There are no OrderItems in this Order") : _orderNumber; }
            set { _orderNumber=value; }
        }
        /// <summary>
        /// This is the ordernumber gotten from the first element of <see cref="OrderItem"/>s it has. If it doesn't have an element it throws <see cref="NullReferenceException()"/>
        /// <para> I also made it Immutable/ReadOnly</para>
        /// </summary>
        public string OrderNumber
        {
            get { return _orderNumber; }
           init 
           {
                if (this.Any())
                {
                    _orderNumber = this.First().OrderNumber;
                }
            
            }
        }

        private string _user
        {
            get { return _user == null ? throw new NullReferenceException("There are no OrderItems in this Order") : _user; }
            set { _user= value; }
        }
        /// <summary>
        /// This is the User as a string (Their name) gotten from the first element of <see cref="OrderItem"/>s it has. If it doesn't have an element it throws <see cref="NullReferenceException()"/>
        /// <para> I also made it Immutable/ReadOnly</para>
        /// </summary>
        public string User
        {
            get { return _user; }
            init
            {
                if (this.Any())
                {
                    _user = this.First().User;
                }
            }
        }

        private string _reference
        {
            get { return _reference == null ? throw new NullReferenceException("There are no OrderItems in this Order") : _reference; }
            set { _reference = value; } 
        }
        /// <summary>
        /// This is the Reference (Where the <see cref="Order"/> came from) gotten from the first element of <see cref="OrderItem"/>s it has. If it doesn't have an element it throws <see cref="NullReferenceException()"/>
        /// <para> I also made it Immutable/ReadOnly</para>
        /// </summary>
        public string Reference
        {
            get { return _reference; }
            init
            {
                if (this.Any())
                {
                    _reference = this.First().Reference;
                }
            }
        }

        private DateTime? _orderDateTime
        {
            get { return _orderDateTime == null ? throw new NullReferenceException("There are no OrderItems in this Order") : _orderDateTime; }
            set { _orderDateTime = value; }
        }
        /// <summary>
        /// This is the <see cref="DateTime"/> gotten from the first element of <see cref="OrderItem"/>s it has. If it doesn't have an element it throws <see cref="NullReferenceException()"/>
        /// <para> I also made it Immutable/ReadOnly</para>
        /// </summary>
        public DateTime? OrderDateTime
        {
            get { return _orderDateTime; }
            init 
            {
                if (this.Any())
                {
                    _orderDateTime = this.First().OrderDateTime;
                }
            }
        }

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
                    foreach (OrderItem item in this)
                    {
                        _price = +float.Parse(item.Price);
                    }
                }
            }
        }

        private int? _id
        {
            get { return _id == null ? throw new NullReferenceException("There are no OrderItems in this Order") : _id; }
            set { _id = value; }
        }
        /// <summary>
        /// This is the id gotten from the first element of <see cref="OrderItem"/>s it has. If it doesn't have an element it throws <see cref="NullReferenceException()"/>
        /// <para> I also made it Immutable/ReadOnly</para>
        /// </summary>
        public int? Id
        {
            get { return _id; }
            init
            {
                if (this.Any())
                {
                    _id = this.First().Id;
                }
            }
        }

        private int? _prepTime
        {
            get { return _prepTime == null ? throw new NullReferenceException("There are no OrderItems in this Order") : _prepTime; }
            set
            {
                _prepTime = value;
            }
        }
        /// <summary>
        /// This is the <see cref="PrepTime"/> gotten from the first element of <see cref="OrderItem"/>s it has. If it doesn't have an element it throws <see cref="NullReferenceException()"/>
        /// <para> I also made it Immutable/ReadOnly</para>
        /// </summary>
        public int? PrepTime
        {
            get { return _prepTime; }
            init
            {
                if (this.Any())
                {
                    _prepTime = this.First().PrepTime;
                }
            }
        }

        private bool? _purchased
        {
            get { return _purchased == null ? throw new NullReferenceException("There are no OrderItems in this Order") : _purchased; }
            set{ _purchased= value; }   
        }
        /// <summary>
        /// This is whether or not the order has been purchased gotten from the first element of <see cref="OrderItem"/>s it has. If it doesn't have an element it throws <see cref="NullReferenceException()"/>
        /// <para> I also made it Immutable/ReadOnly</para>
        /// </summary>
        public bool? Purchased
        {
            get { return _purchased; }
            init
            {
                if (this.Any())
                {
                    _purchased = this.First().Purchased;
                }
            }
        }

        public string _paymentMethod
        {
            get { return _paymentMethod == null ? throw new NullReferenceException("There are no OrderItems in this Order") : _paymentMethod; }
            set{ _paymentMethod = value;}
        }
        /// <summary>
        /// This is the payment method gotten from the first element of <see cref="OrderItem"/>s it has. If it doesn't have an element it throws <see cref="NullReferenceException()"/>
        /// <para> I also made it Immutable/ReadOnly</para>
        /// </summary>
        public string PaymentMethod
        {
            get { return _paymentMethod; }
            init
            {
                if (this.Any())
                {
                    _paymentMethod = this.First().PaymentMethod;
                }
            }
        }

        private bool? _waitingForPayment
        {
            get { return _waitingForPayment == null ? throw new NullReferenceException("There are no OrderItems in this Order") : _waitingForPayment; }
            set{ _waitingForPayment = value; }  
        }
        /// <summary>
        /// This is whether or not the order is still waiting for payment, gotten from the first element of <see cref="OrderItem"/>s it has. If it doesn't have an element it throws <see cref="NullReferenceException()"/>
        /// <para> I also made it Immutable/ReadOnly</para>
        /// </summary>
        public bool? WaitingForPayment
        {
            get { return _waitingForPayment; }
            init
            {
                if (this.Any())
                {
                    _waitingForPayment = this.First().WaitingForPayment;
                }
            }
        }


        private bool? _collected
        {
            get { return _collected == null ? throw new NullReferenceException("There are no OrderItems in this Order") : _collected; }
            set{ _collected = value; }  
        }
        /// <summary>
        /// This is whether or not the order is collected, gotten from the first element of <see cref="OrderItem"/>s it has. If it doesn't have an element it throws <see cref="NullReferenceException()"/>
        /// <para> I also made it Immutable/ReadOnly</para>
        /// </summary>
        public bool? Collected
        {
            get { return _collected; }
            init
            {
                if (this.Any())
                {
                    _collected = this.First().Collected;
                }
            }
        }


    }
}
