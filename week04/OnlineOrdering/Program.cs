using System;
using System.Collections.Generic;

// -------------------- Address Class --------------------
public class Address
{
    private string _street;
    private string _city;
    private string _stateOrProvince;
    private string _country;

    public Address(string street, string city, string stateOrProvince, string country)
    {
        _street = street;
        _city = city;
        _stateOrProvince = stateOrProvince;
        _country = country;
    }

    public bool IsInUSA()
    {
        return _country.ToUpper() == "USA";
    }

    public string GetFullAddress()
    {
        return $"{_street}\n{_city}, {_stateOrProvince}\n{_country}";
    }
}

// -------------------- Customer Class --------------------
public class Customer
{
    private string _name;
    private Address _address;

    public Customer(string name, Address address)
    {
        _name = name;
        _address = address;
    }

    public bool LivesInUSA()
    {
        return _address.IsInUSA();
    }

    public string GetName()
    {
        return _name;
    }

    public Address GetAddress()
    {
        return _address;
    }
}

// -------------------- Product Class --------------------
public class Product
{
    private string _name;
    private string _id;
    private double _price;
    private int _quantity;

    public Product(string name, string id, double price, int quantity)
    {
        _name = name;
        _id = id;
        _price = price;
        _quantity = quantity;
    }

    public string GetName() => _name;
    public string GetId() => _id;

    public double GetTotalCost()
    {
        return _price * _quantity;
    }
}

// -------------------- Order Class --------------------
public class Order
{
    private List<Product> _products = new List<Product>();
    private Customer _customer;

    public Order(Customer customer)
    {
        _customer = customer;
    }

    public void AddProduct(Product p)
    {
        _products.Add(p);
    }

    public double GetTotalCost()
    {
        double sum = 0;

        foreach (var p in _products)
        {
            sum += p.GetTotalCost();
        }

        double shipping = _customer.LivesInUSA() ? 5 : 35;
        return sum + shipping;
    }

    public string GetPackingLabel()
    {
        string label = "Packing Label:\n";

        foreach (var p in _products)
        {
            label += $"- {p.GetName()} (ID: {p.GetId()})\n";
        }

        return label;
    }

    public string GetShippingLabel()
    {
        return $"Shipping Label:\n{_customer.GetName()}\n{_customer.GetAddress().GetFullAddress()}";
    }
}

// -------------------- Program Main --------------------
class Program
{
    static void Main(string[] args)
    {
        // -------- Order 1 (USA customer) --------
        Address addr1 = new Address("123 Apple St", "New York", "NY", "USA");
        Customer cust1 = new Customer("John Smith", addr1);
        Order order1 = new Order(cust1);

        order1.AddProduct(new Product("Keyboard", "K001", 29.99, 2));
        order1.AddProduct(new Product("Mouse", "M010", 19.99, 1));
        order1.AddProduct(new Product("USB Cable", "U100", 5.99, 3));

        Console.WriteLine("===== Order 1 =====");
        Console.WriteLine(order1.GetPackingLabel());
        Console.WriteLine(order1.GetShippingLabel());
        Console.WriteLine($"Total Price: ${order1.GetTotalCost():0.00}");
        Console.WriteLine();

        // -------- Order 2 (International customer) --------
        Address addr2 = new Address("88 Sakura Road", "Tokyo", "Tokyo", "Japan");
        Customer cust2 = new Customer("Yuki Tanaka", addr2);
        Order order2 = new Order(cust2);

        order2.AddProduct(new Product("Camera", "C500", 199.99, 1));
        order2.AddProduct(new Product("Tripod", "T200", 49.99, 1));

        Console.WriteLine("===== Order 2 =====");
        Console.WriteLine(order2.GetPackingLabel());
        Console.WriteLine(order2.GetShippingLabel());
        Console.WriteLine($"Total Price: ${order2.GetTotalCost():0.00}");
        Console.WriteLine();
    }
}
