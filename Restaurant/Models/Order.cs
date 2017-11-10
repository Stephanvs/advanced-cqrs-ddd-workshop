//using System;
//using System.Collections.Generic;
//using Newtonsoft.Json;
//using Newtonsoft.Json.Linq;

//namespace Restaurant
//{
//    public class OrderDocument
//    {
//        private readonly JObject _json;

//        public OrderDocument()
//        {
//            _json = new JObject();
//        }

//        public OrderDocument(string json)
//        {
//            _json = JObject.Parse(json);
//        }

//        public string Waiter
//        {
//            get => _json[nameof(Waiter)].Value<string>();
//            set => _json[nameof(Waiter)] = value;
//        }

//        public Guid OrderNumber
//        {
//            get => _json[nameof(OrderNumber)].Value<Guid>();
//            set => _json[nameof(OrderNumber)] = value;
//        }

//        public bool Paid
//        {
//            get => _json[nameof(Paid)].Value<bool>();
//            set => _json[nameof(Paid)] = value;
//        }

//        public double CookingMinutes
//        {
//            get => _json[nameof(CookingMinutes)].Value<double>();
//            set => _json[nameof(CookingMinutes)] = value;
//        }

//        public double TotalPrice
//        {
//            get => _json[nameof(TotalPrice)].Value<double>();
//            set => _json[nameof(TotalPrice)] = value;
//        }

//        public string CookName
//        {
//            get => GetProperty(nameof(CookName)).Value<string>();
//            set => _json[nameof(CookName)] = value;
//        }

//        public void AddLineItem(LineItem lineItem)
//        {
//            var arr = _json[nameof(LineItems)] as JArray;

//            var itemJson = JToken.FromObject(lineItem);

//            arr?.Add(itemJson);
//        }

//        public IEnumerable<LineItem> LineItems
//            => _json[nameof(LineItems)].Value<IList<LineItem>>();

//        public IEnumerable<string> Ingredients
//            => _json[nameof(Ingredients)].ToObject<IList<string>>();

//        public void AddIngredient(string ingredient)
//        {
//            var arr = _json[nameof(Ingredients)] as JArray;
//            arr?.Add(ingredient);
//        }

//        public string Serialize()
//        {
//            return ToString();
//        }

//        public override string ToString()
//        {
//            return _json.ToString(Formatting.None);
//        }

//        private JToken GetProperty(string name)
//            => _json[name];
//    }
//}