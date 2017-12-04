using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Newtonsoft.Json;
using Products.Data;
using Products.Models;

namespace Products.App
{
    class StartUp
    {
        static void Main(string[] args)
        {
            //JSON Processing
            // 00.Create Database
            DbReset();

            //01.Import Data from JSON
            ImportData();

            //02.Query and Export Data from JSON
            ExportData();

            //Reset Database
            DbReset();

            //XML Processing
            // 01.Import Data from XML
            ImportDataXml();

            //02.Query and Export Data from XML
            ExportDataXml();

        }

        private static void ExportDataXml()
        {
            using (var db = new ProductsContext())
            {
                ProductsInRangeXml(db);

                GetSoldProductsXml(db);

                CountProductsXml(db);

                GetUsersAndProductsXml(db);
            }
        }

        private static void GetUsersAndProductsXml(ProductsContext db)
        {
            var users = db.Users
                .Where(u => u.SoldProducts.Count >= 1)
                .Select(u => new
                {
                    u.FirstName,
                    u.LastName,
                    u.Age,
                    SoldProducts = u.SoldProducts.Select(p => new
                    {
                        p.Name,
                        p.Price
                    }).ToArray()

                })
                .OrderByDescending(u => u.SoldProducts.Length)
                .ThenBy(u => u.LastName)
                .ToArray();

            var xDoc = new XDocument();
            var xElementsUsers = new List<XElement>();

            foreach (var user in users)
            {
                var xElementsProducts = new List<XElement>();

                foreach (var product in user.SoldProducts)
                {
                    xElementsProducts.Add(new XElement("product",
                                            new XAttribute("name", product.Name),
                                            new XAttribute("price", product.Price)
                                            ));
                }

                xElementsUsers.Add(new XElement("user",
                                    new XAttribute("first-name", user.FirstName ?? ""),
                                    new XAttribute("last-name", user.LastName),
                                    new XAttribute("age", user.Age ?? 0),
                                    new XElement(new XElement("sold-products",
                                                    new XAttribute("count", user.SoldProducts.Length), xElementsProducts))
                                    ));
            }

            xDoc.Add(new XElement("users", new XAttribute("count", users.Length),
                        xElementsUsers));


            xDoc.Save(@"Exported\UsersAndProducts.xml");
        }

        private static void CountProductsXml(ProductsContext db)
        {
            var categories = db.Categories
                .Select(c => new
                {
                    c.Name,
                    ProductsCount = c.CategoryProducts.Count,
                    AveragePrice = c.CategoryProducts.Count == 0 ? 0 : c.CategoryProducts.Select(p => p.Product.Price).Average(),
                    TotalRevenue = c.CategoryProducts.Select(p => p.Product.Price).Sum()
                })
                .OrderBy(c => c.ProductsCount)
                .ToArray();

            var xDoc = new XDocument();
            var xElementsCategories = new List<XElement>();

            foreach (var category in categories)
            {
                xElementsCategories.Add(new XElement("category",
                                            new XAttribute("name", category.Name),
                                            new XElement("products-count", category.ProductsCount),
                                            new XElement("average-price", category.AveragePrice),
                                            new XElement("total-revenue", category.TotalRevenue))
                                            );
            }

            xDoc.Add(new XElement("categories", xElementsCategories));

            xDoc.Save(@"Exported\CategoriesByProductsCount.xml");
        }

        private static void GetSoldProductsXml(ProductsContext db)
        {
            var users = db.Users
                .Select(u => new
                {
                    u.FirstName,
                    u.LastName,
                    SoldProducts = u.SoldProducts.Select(p => new
                    {
                        p.Name,
                        p.Price,
                    })
                        .ToArray()
                })
                .Where(u => u.SoldProducts.Length >= 1)
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
                .ToArray();

            var xdoc = new XDocument();
            var xElementsUsers = new List<XElement>();

            foreach (var user in users)
            {
                var xElementsProducts = new List<XElement>();

                foreach (var product in user.SoldProducts)
                {
                    xElementsProducts.Add(new XElement("product",
                                            new XElement("name", product.Name),
                                            new XElement("price", product.Price)
                                            ));
                }


                xElementsUsers.Add(new XElement("user",
                                    new XAttribute("first-name", user.FirstName ?? ""),
                                    new XAttribute("last-name", user.LastName),
                                    new XElement("sold-products", xElementsProducts))
                                    );

            }

            xdoc.Add(new XElement("users", xElementsUsers));

            xdoc.Save(@"Exported\SoldProducts.xml");
        }

        private static void ProductsInRangeXml(ProductsContext db)
        {
            var productsInRange = db.Products
                                .Where(p => p.Price >= 1000 && p.Price <= 2000 && p.Buyer != null)
                                .Select(x => new
                                {
                                    x.Name,
                                    x.Price,
                                    Buyer = x.Buyer.FullName
                                })
                                .OrderBy(p => p.Price)
                                .ToArray();

            var xDoc = new XDocument();
            var xElements = new List<XElement>();

            foreach (var product in productsInRange)
            {

                xElements.Add(new XElement("product",
                    new XAttribute("name", product.Name),
                    new XAttribute("price", product.Price),
                    new XAttribute("buyer", product.Buyer)
                ));
            }

            xDoc.Add(new XElement("products", xElements));

            xDoc.Save(@"Exported\ProductsInRnage.xml");
        }

        private static void ImportDataXml()
        {
            using (var db = new ProductsContext())
            {
                var users = ImportUsers(@"Imported\users.xml");
                db.Users.AddRange(users);
                db.SaveChanges();

                var categories = ImportCategories(@"Imported\categories.xml");
                db.Categories.AddRange(categories);
                db.SaveChanges();

                var products = ImportProducts(@"Imported\products.xml");
                AddSellersAndBuyers(products.ToArray(), db);

                var categoryProducts = AddCategoryProducts(db);
                db.CategoryProducts.AddRange(categoryProducts);
                db.SaveChanges();
            }
        }

        private static List<Product> ImportProducts(string path)
        {
            string xmlString = File.ReadAllText(path);
            var xmlDoc = XDocument.Parse(xmlString);

            var products = new List<Product>();

            var root = xmlDoc.Root.Elements();

            foreach (var element in root)
            {
                string name = element.Element("name")?.Value;
                decimal price = decimal.Parse(element.Element("price").Value);

                var product = new Product() { Name = name, Price = price };

                products.Add(product);
            }

            return products;
        }

        private static List<Category> ImportCategories(string path)
        {
            string xmlString = File.ReadAllText(path);

            var xmlDoc = XDocument.Parse(xmlString);

            var root = xmlDoc.Root.Elements();

            var categories = new List<Category>();

            foreach (var element in root)
            {
                var el = element.Element("name").Value;

                var category = new Category() { Name = el };
                categories.Add(category);
            }

            return categories;
        }

        private static List<User> ImportUsers(string path)
        {
            var xmlString = File.ReadAllText(path);

            var xml = XDocument.Parse(xmlString);

            var root = xml.Root.Elements();

            var users = new List<User>();

            foreach (var element in root)
            {
                var user = new User()
                {
                    FirstName = element.Attribute("firstName")?.Value,
                    LastName = element.Attribute("lastName")?.Value,
                    Age = element.Attribute("age") == null ? (int?)null : int.Parse(element.Attribute("age").Value)

                };

                users.Add(user);
            }

            return users;
        }

        private static void ExportData()
        {
            using (var db = new ProductsContext())
            {
                var productsInRange = GetPriceRange(db);
                string jsonProductsInRange = Export(productsInRange);

                File.WriteAllText(@"Exported\ProductsInRange.json", jsonProductsInRange);

                var succsessfulluSoldProducts = GetSoldProducts(db);
                string jsonSoldProducts = Export(succsessfulluSoldProducts);

                File.WriteAllText(@"Exported\SuccsessfullySoldProducts.json", jsonSoldProducts);

                var categoriesCountProducts = CountProducts(db);
                string jsonCount = Export(categoriesCountProducts);

                File.WriteAllText(@"Exported\CategoriesByPrroductsCount.json", jsonCount);

                var usersAndProducts = GetUsersAndProducts(db);
                var jsonUsersProducts = Export(usersAndProducts);

                File.WriteAllText(@"Exported\UsersAndProducts.json", jsonUsersProducts);
            }
        }

        private static IEnumerable GetUsersAndProducts(ProductsContext db)
        {
            var users = db.Users
                .Where(u => u.SoldProducts.Count >= 1)
                .Select(u => new
                {
                    u.FirstName,
                    u.LastName,
                    u.Age,
                    SoldProducts = new
                    {
                        Count = u.SoldProducts.Count,
                        Products = u.SoldProducts.Select(p => new
                        {
                            p.Name,
                            p.Price
                        }).ToArray()
                    }
                })
                .OrderByDescending(u => u.SoldProducts.Count)
                .ToArray();

            var obj = new
            {
                UsersCount = users.Length,
                Users = users
            };

            return new[] { obj };
        }

        private static IEnumerable CountProducts(ProductsContext db)
        {
            var categories = db.Categories
                .Select(c => new
                {
                    c.Name,
                    ProductsCount = c.CategoryProducts.Count,
                    AveragePrice = c.CategoryProducts.Count == 0 ? null : (decimal?)c.CategoryProducts.Select(p => p.Product.Price).Average(),
                    TotalRevenue = (decimal?)c.CategoryProducts.Select(p => p.Product.Price).Sum()
                })
                .OrderBy(c => c.Name)
                .ToArray();

            return categories;
        }

        private static IEnumerable GetSoldProducts(ProductsContext db)
        {
            var users = db.Users
                .Select(u => new
                {
                    u.FirstName,
                    u.LastName,
                    SoldProducts = u.SoldProducts.Select(p => new
                    {
                        p.Name,
                        p.Price,
                        BuyerFirstName = p.Buyer.FirstName,
                        BuyerLastName = p.Buyer.LastName,
                    })
                    .Where(s => s.BuyerLastName != null)
                    .ToArray()
                })
                .Where(u => u.SoldProducts.Length >= 1)
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
                .ToArray();

            return users;
        }

        private static string Export(IEnumerable items)
        {
            var json = JsonConvert.SerializeObject(items, Formatting.Indented);

            return json;
        }
        private static IEnumerable GetPriceRange(ProductsContext db)
        {
            var products = db.Products
                .Where(p => p.Price >= 500 && p.Price <= 1000)
                .Select(x => new
                {
                    x.Name,
                    x.Price,
                    Seller = x.Seller.FullName
                })
                .OrderBy(p => p.Price)
                .ToArray();


            return products;
        }

        private static void ImportData()
        {
            using (var db = new ProductsContext())
            {
                var users = Import<User>(@"Imported\users.json");
                db.Users.AddRange(users);

                var categories = Import<Category>(@"Imported\categories.json");
                db.Categories.AddRange(categories);
                db.SaveChanges();

                var products = Import<Product>(@"Imported\products.json");
                AddSellersAndBuyers(products, db);

                var categoryProducts = AddCategoryProducts(db);
                db.CategoryProducts.AddRange(categoryProducts);
                db.SaveChanges();
            }
        }

        private static HashSet<CategoryProduct> AddCategoryProducts(ProductsContext db)
        {
            var productsCount = db.Products.Count();
            var categoriesCount = db.Categories.Count();

            var categoryProducts = new HashSet<CategoryProduct>();

            var random = new Random();

            for (int i = 0; i < 20; i++)
            {
                var categoryProduct = new CategoryProduct()
                {
                    CategoryId = random.Next(1, categoriesCount),
                    ProductId = random.Next(1, productsCount)
                };

                categoryProducts.Add(categoryProduct);
            }

            return categoryProducts;
        }

        private static void AddSellersAndBuyers(Product[] products, ProductsContext db)
        {
            var sellers = db.Users.Count();
            var random = new Random();

            foreach (var product in products)
            {
                product.SellerId = random.Next(1, sellers);
            }

            db.Products.AddRange(products);
            db.SaveChanges();

            foreach (var product in db.Products)
            {
                int id = product.Id;
                int sellId = product.SellerId;
                int result = Math.Abs(id * sellId);
                bool isEven = result % 2 == 0;
                int? value = null;

                if (!isEven)
                {
                    value = random.Next(1, sellers);
                }

                product.BuyerId = value;
            }

            db.SaveChanges();
        }

        private static T[] Import<T>(string path)
        {
            var jsonUsers = File.ReadAllText(path);

            T[] objects = JsonConvert.DeserializeObject<T[]>(jsonUsers);

            return objects;
        }

        private static void DbReset()
        {
            using (var db = new ProductsContext())
            {
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();
            }
        }
    }
}
