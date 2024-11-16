using Core.Entities;
using Core.Entities.Enum;
using Core.Entities.OrderAggregate;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class SeedData
    {
        public static async Task SeedAsync(StoreContext context, ILoggerFactory loggerFactory,
            UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            try
            {
                //Seed Brands
                if (!context.ProductBrands.Any())
                {
                    var brandsData = await
                        File.ReadAllTextAsync("../Infrastructure/Data/SeedData/brands.json");

                    //var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);
                    var brands = JsonConvert.DeserializeObject<List<ProductBrand>>(brandsData);

                    foreach (var brand in brands)
                    {
                        context.ProductBrands.Add(brand);
                    }
                    await context.SaveChangesAsync();
                }

                //Seed Types
                if (!context.ProductTypes.Any())
                {
                    var typesData = await
                        File.ReadAllTextAsync("../Infrastructure/Data/SeedData/types.json");

                    //var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);
                    var types = JsonConvert.DeserializeObject<List<ProductType>>(typesData);

                    foreach (var type in types)
                    {
                        context.ProductTypes.Add(type);
                    }
                    await context.SaveChangesAsync();
                }

                //Seed Products
                if (!context.Products.Any())
                {
                    var productsData = await
                        File.ReadAllTextAsync("../Infrastructure/Data/SeedData/products.json");


                    var products = JsonConvert.DeserializeObject<List<Product>>(productsData);

                    foreach (var product in products)
                    {
                        context.Products.Add(product);
                    }
                    await context.SaveChangesAsync();
                }

                //Seed Delivery Methods
                if (!context.DeliveryMethods.Any())
                {
                    var DeliveryMethodsData = await
                        File.ReadAllTextAsync("../Infrastructure/Data/SeedData/delivery.json");

                    //var products = JsonSerializer.Deserialize<List<Product>>(productsData);

                    var deliveyMethods = JsonConvert.DeserializeObject<List<DeliveryMethod>>(DeliveryMethodsData);

                    foreach (var method in deliveyMethods)
                    {
                        context.DeliveryMethods.Add(method);
                    }
                    await context.SaveChangesAsync();
                }


                // Seed Roles
                if (!await roleManager.RoleExistsAsync("Admin"))
                {
                    await roleManager.CreateAsync(new IdentityRole("Admin"));
                }

                if (!await roleManager.RoleExistsAsync("Customer"))
                {
                    await roleManager.CreateAsync(new IdentityRole("Customer"));
                }
                var adminEmail = "admin@furnitica.com";
                var adminUser = await userManager.FindByEmailAsync(adminEmail);
                if (adminUser == null)
                {
                    var newAdmin = new AppUser()
                    {
                        Email = adminEmail,
                        UserName = "Thanos",
                        ProfilePicture = "https://i.ytimg.com/vi/N2YTmooNR8E/maxresdefault.jpg",
                        Gender = Gender.Male
                    };
                    var result = await userManager.CreateAsync(newAdmin, "Admin@1234");
                    await userManager.AddToRoleAsync(newAdmin, "Admin");
                }

                var customerEmail = "customer@furnitica.com";
                var customerUser = await userManager.FindByEmailAsync(customerEmail);
                if (customerUser == null)
                {
                    var newCustomer = new AppUser()
                    {
                        Email = customerEmail,
                        UserName = "Wanda",
                        ProfilePicture = "https://sm.ign.com/ign_me/news/w/wandavisio/wandavision-director-says-theres-a-lot-more-of-scarlet-witch_wyke.jpg",
                        Gender = Gender.Female
                    };
                    await userManager.CreateAsync(newCustomer, "Customer@1234");
                    await userManager.AddToRoleAsync(newCustomer, "Customer");
                }
                if (!context.Medias.Any())
                {
                    var mediaData = await
                        File.ReadAllTextAsync("../Infrastructure/Data/SeedData/media.json");

                    var medias = JsonConvert.DeserializeObject<List<Media>>(mediaData);

                    foreach (var media in medias)
                    {
                        context.Medias.Add(media);
                    }
                    await context.SaveChangesAsync();
                }


                //Seed Orders
                if (!context.Orders.Any())
                {
                    var ordersData = new List<Order>()
                    {
                        new Order()
                        {
                            BuyerEmail = "customer@furnitica.com",
                            OrderDate = DateTime.Now,
                            ShipToAddress = new Address()
                            {
                                FirstName = "Hiso",
                                LastName = "Hoso",
                                Street = "street1",
                                City = "cairo",
                                State = "NY",
                                Zipcode = "1234"
                            },
                            DeliveryMethod = deliveryMethod_1,
                            OrderItems = new List<OrderItem>()
                            {
                                new OrderItem()
                                {
                                    ItemOrdered = new ProductItemOrdered()
                                    {
                                        ProductItemId = 3,
                                        ProductName = "Chair"
                                    },
                                    Price = 100,
                                    Quantity = 4,
                                },

                                new OrderItem()
                                {
                                    ItemOrdered = new ProductItemOrdered()
                                    {
                                        ProductItemId = 1,
                                        ProductName = "Chair with Pillows"
                                    },
                                    Price = 40,
                                    Quantity = 3,
                                },

                                new OrderItem()
                                {
                                    ItemOrdered = new ProductItemOrdered()
                                    {
                                        ProductItemId = 7,
                                        ProductName = "Table"
                                    },
                                    Price = 200,
                                    Quantity = 1,
                                },

                                new OrderItem()
                                {
                                    ItemOrdered = new ProductItemOrdered()
                                    {
                                        ProductItemId = 5,
                                        ProductName = "cupboard"
                                    },
                                    Price = 900,
                                    Quantity = 8,
                                },
                            },
                            Subtotal = 11111,
                            Status = OrderStatus.Pending,
                            PaymentIntentId = "53454nskjdfb"
                        },

                        new Order()
                        {
                            BuyerEmail = "customer@furnitica.com",
                            OrderDate = DateTime.Now,
                            ShipToAddress = new Address()
                            {
                                FirstName = "Thanous",
                                LastName = "Wanda",
                                Street = "street1",
                                City = "cairo",
                                State = "NY",
                                Zipcode = "1234"
                            },
                            DeliveryMethod = deliveryMethod_1,
                            OrderItems = new List<OrderItem>()
                            {
                                new OrderItem()
                                {
                                    ItemOrdered = new ProductItemOrdered()
                                    {
                                        ProductItemId = 3,
                                        ProductName = "Chair"
                                    },
                                    Price = 34,
                                    Quantity =5,
                                },

                                new OrderItem()
                                {
                                    ItemOrdered = new ProductItemOrdered()
                                    {
                                        ProductItemId = 1,
                                        ProductName = "Cupboard"
                                    },
                                    Price = 67,
                                    Quantity = 7,
                                },

                                new OrderItem()
                                {
                                    ItemOrdered = new ProductItemOrdered()
                                    {
                                        ProductItemId = 7,
                                        ProductName = "Table"
                                    },
                                    Price = 76,
                                    Quantity = 3,
                                },

                                new OrderItem()
                                {
                                    ItemOrdered = new ProductItemOrdered()
                                    {
                                        ProductItemId = 5,
                                        ProductName = "Stand"
                                    },
                                    Price = 900,
                                    Quantity = 8,
                                },
                            },
                            Subtotal = 12,
                            Status = OrderStatus.Pending,
                            PaymentIntentId = "53454nskjdfb"
                        },

                        new Order()
                        {
                            BuyerEmail = "customer@furnitica.com",
                            OrderDate = DateTime.Now,
                            ShipToAddress = new Address()
                            {
                                FirstName = "Sayed",
                                LastName = "Ahmed",
                                Street = "street1",
                                City = "cairo",
                                State = "NY",
                                Zipcode = "1234"
                            },
                            DeliveryMethod = deliveryMethod_1,
                            OrderItems = new List<OrderItem>()
                            {
                                new OrderItem()
                                {
                                    ItemOrdered = new ProductItemOrdered()
                                    {
                                        ProductItemId = 3,
                                        ProductName = "Chair"
                                    },
                                    Price = 100,
                                    Quantity = 4,
                                },

                                new OrderItem()
                                {
                                    ItemOrdered = new ProductItemOrdered()
                                    {
                                        ProductItemId = 1,
                                        ProductName = "Wooden Floor"
                                    },
                                    Price = 40,
                                    Quantity = 3,
                                },

                                new OrderItem()
                                {
                                    ItemOrdered = new ProductItemOrdered()
                                    {
                                        ProductItemId = 7,
                                        ProductName = "Sofe"
                                    },
                                    Price = 200,
                                    Quantity = 1,
                                },

                                new OrderItem()
                                {
                                    ItemOrdered = new ProductItemOrdered()
                                    {
                                        ProductItemId = 5,
                                        ProductName = "Door"
                                    },
                                    Price = 900,
                                    Quantity = 8,
                                },
                            },
                            Subtotal = 5443,
                            Status = OrderStatus.Pending,
                            PaymentIntentId = "53454nskjdfb"
                        },

                        new Order()
                        {
                            BuyerEmail = "admin@furnitica.com",
                            OrderDate = DateTime.Now,
                            ShipToAddress = new Address()
                            {
                                FirstName = "Ramadan",
                                LastName = "Hossam",
                                Street = "street1",
                                City = "cairo",
                                State = "NY",
                                Zipcode = "1234"
                            },
                            DeliveryMethod = deliveryMethod_1,
                            OrderItems = new List<OrderItem>()
                            {
                                new OrderItem()
                                {
                                    ItemOrdered = new ProductItemOrdered()
                                    {
                                        ProductItemId = 3,
                                        ProductName = "Chair"
                                    },
                                    Price = 34,
                                    Quantity = 6,
                                },

                                new OrderItem()
                                {
                                    ItemOrdered = new ProductItemOrdered()
                                    {
                                        ProductItemId = 1,
                                        ProductName = "Mirror Stand"
                                    },
                                    Price = 677,
                                    Quantity = 6,
                                },

                                new OrderItem()
                                {
                                    ItemOrdered = new ProductItemOrdered()
                                    {
                                        ProductItemId = 7,
                                        ProductName = "Samll Table"
                                    },
                                    Price = 200,
                                    Quantity = 1,
                                },

                                new OrderItem()
                                {
                                    ItemOrdered = new ProductItemOrdered()
                                    {
                                        ProductItemId = 5,
                                        ProductName = "Bed"
                                    },
                                    Price = 122,
                                    Quantity = 3,
                                },
                            },
                            Subtotal = 4553,
                            Status = OrderStatus.Pending,
                            PaymentIntentId = "53454nskjdfb"
                        },

                        new Order()
                        {
                            BuyerEmail = "admin@furnitica.com",
                            OrderDate = DateTime.Now,
                            ShipToAddress = new Address()
                            {
                                FirstName = "Mahroos",
                                LastName = "Hoso",
                                Street = "street1",
                                City = "cairo",
                                State = "NY",
                                Zipcode = "1234"
                            },
                            DeliveryMethod = deliveryMethod_2,
                            OrderItems = new List<OrderItem>()
                            {
                                new OrderItem()
                                {
                                    ItemOrdered = new ProductItemOrdered()
                                    {
                                        ProductItemId = 3,
                                        ProductName = "Bed"
                                    },
                                    Price = 100,
                                    Quantity = 4,
                                },

                                new OrderItem()
                                {
                                    ItemOrdered = new ProductItemOrdered()
                                    {
                                        ProductItemId = 1,
                                        ProductName = "Sofa"
                                    },
                                    Price = 40,
                                    Quantity = 3,
                                },

                                new OrderItem()
                                {
                                    ItemOrdered = new ProductItemOrdered()
                                    {
                                        ProductItemId = 7,
                                        ProductName = "Meeting Table"
                                    },
                                    Price = 200,
                                    Quantity = 1,
                                },

                                new OrderItem()
                                {
                                    ItemOrdered = new ProductItemOrdered()
                                    {
                                        ProductItemId = 5,
                                        ProductName = "Cupboard"
                                    },
                                    Price = 66,
                                    Quantity = 2,
                                },
                            },
                            Subtotal = 4637,
                            Status = OrderStatus.Pending,
                            PaymentIntentId = "53454nskjdfb"
                        },

                        new Order()
                        {
                            BuyerEmail = "admin@furnitica.com",
                            OrderDate = DateTime.Now,
                            ShipToAddress = new Address()
                            {
                                FirstName = "Fathy",
                                LastName = "Hoso",
                                Street = "street1",
                                City = "cairo",
                                State = "NY",
                                Zipcode = "1234"
                            },
                            DeliveryMethod = deliveryMethod_2,
                            OrderItems = new List<OrderItem>()
                            {
                                new OrderItem()
                                {
                                    ItemOrdered = new ProductItemOrdered()
                                    {
                                        ProductItemId = 3,
                                        ProductName = "Chair"
                                    },
                                    Price = 100,
                                    Quantity = 12,
                                },

                                new OrderItem()
                                {
                                    ItemOrdered = new ProductItemOrdered()
                                    {
                                        ProductItemId = 1,
                                        ProductName = "Wooden Fence"
                                    },
                                    Price = 40,
                                    Quantity = 7,
                                },

                                new OrderItem()
                                {
                                    ItemOrdered = new ProductItemOrdered()
                                    {
                                        ProductItemId = 7,
                                        ProductName = "Table"
                                    },
                                    Price = 200,
                                    Quantity = 1,
                                },

                                new OrderItem()
                                {
                                    ItemOrdered = new ProductItemOrdered()
                                    {
                                        ProductItemId = 5,
                                        ProductName = "Wooden Counter"
                                    },
                                    Price = 900,
                                    Quantity = 5,
                                },
                            },
                            Subtotal = 37637,
                            Status = OrderStatus.Pending,
                            PaymentIntentId = "53454nskjdfb"
                        },

                        new Order()
                        {
                            BuyerEmail = "customer@furnitica.com",
                            OrderDate = DateTime.Now,
                            ShipToAddress = new Address()
                            {
                                FirstName = "Sayed",
                                LastName = "Hoso",
                                Street = "street1",
                                City = "cairo",
                                State = "NY",
                                Zipcode = "1234"
                            },
                            DeliveryMethod = deliveryMethod_2,
                            OrderItems = new List<OrderItem>()
                            {
                                new OrderItem()
                                {
                                    ItemOrdered = new ProductItemOrdered()
                                    {
                                        ProductItemId = 3,
                                        ProductName = "Big Chair"
                                    },
                                    Price = 100,
                                    Quantity = 7,
                                },

                                new OrderItem()
                                {
                                    ItemOrdered = new ProductItemOrdered()
                                    {
                                        ProductItemId = 1,
                                        ProductName = "Seat with Pillows"
                                    },
                                    Price = 40,
                                    Quantity = 3,
                                },

                                new OrderItem()
                                {
                                    ItemOrdered = new ProductItemOrdered()
                                    {
                                        ProductItemId = 7,
                                        ProductName = "Table"
                                    },
                                    Price = 200,
                                    Quantity = 1,
                                },

                                new OrderItem()
                                {
                                    ItemOrdered = new ProductItemOrdered()
                                    {
                                        ProductItemId = 5,
                                        ProductName = "Molla"
                                    },
                                    Price = 900,
                                    Quantity = 10,
                                },
                            },
                            Subtotal = 464,
                            Status = OrderStatus.Pending,
                            PaymentIntentId = "53454nskjdfb"
                        },

                        new Order()
                        {
                            BuyerEmail = "customer@furnitica.com",
                            OrderDate = DateTime.Now,
                            ShipToAddress = new Address()
                            {
                                FirstName = "Ramadan",
                                LastName = "Hoso",
                                Street = "street1",
                                City = "cairo",
                                State = "NY",
                                Zipcode = "1234"
                            },
                            DeliveryMethod = deliveryMethod_2,
                            OrderItems = new List<OrderItem>()
                            {
                                new OrderItem()
                                {
                                    ItemOrdered = new ProductItemOrdered()
                                    {
                                        ProductItemId = 3,
                                        ProductName = "Chair"
                                    },
                                    Price = 100,
                                    Quantity = 14,
                                },

                                new OrderItem()
                                {
                                    ItemOrdered = new ProductItemOrdered()
                                    {
                                        ProductItemId = 1,
                                        ProductName = "Sofa"
                                    },
                                    Price = 40,
                                    Quantity = 3,
                                },

                                new OrderItem()
                                {
                                    ItemOrdered = new ProductItemOrdered()
                                    {
                                        ProductItemId = 7,
                                        ProductName = "Sofra"
                                    },
                                    Price = 200,
                                    Quantity = 1,
                                },

                                new OrderItem()
                                {
                                    ItemOrdered = new ProductItemOrdered()
                                    {
                                        ProductItemId = 5,
                                        ProductName = "Molla"
                                    },
                                    Price = 900,
                                    Quantity = 16,
                                },
                            },
                            Subtotal = 424,
                            Status = OrderStatus.Confirmed,
                            PaymentIntentId = "53454nskjdfb"
                        },

                        new Order()
                        {
                            BuyerEmail = "customer@furnitica.com",
                            OrderDate = DateTime.Now,
                            ShipToAddress = new Address()
                            {
                                FirstName = "Hiso",
                                LastName = "Ramadan",
                                Street = "street1",
                                City = "cairo",
                                State = "NY",
                                Zipcode = "1234"
                            },
                            DeliveryMethod = deliveryMethod_2,
                            OrderItems = new List<OrderItem>()
                            {
                                new OrderItem()
                                {
                                    ItemOrdered = new ProductItemOrdered()
                                    {
                                        ProductItemId = 3,
                                        ProductName = "Chair"
                                    },
                                    Price = 100,
                                    Quantity = 4,
                                },

                                new OrderItem()
                                {
                                    ItemOrdered = new ProductItemOrdered()
                                    {
                                        ProductItemId = 1,
                                        ProductName = "Sofa"
                                    },
                                    Price = 40,
                                    Quantity = 3,
                                },

                                new OrderItem()
                                {
                                    ItemOrdered = new ProductItemOrdered()
                                    {
                                        ProductItemId = 7,
                                        ProductName = "Table"
                                    },
                                    Price = 200,
                                    Quantity = 1,
                                },

                                new OrderItem()
                                {
                                    ItemOrdered = new ProductItemOrdered()
                                    {
                                        ProductItemId = 5,
                                        ProductName = "Bed"
                                    },
                                    Price = 900,
                                    Quantity = 8,
                                },
                            },
                            Subtotal = 32,
                            Status = OrderStatus.Pending,
                            PaymentIntentId = "customer@furnitica.com"
                        },

                        new Order()
                        {
                            BuyerEmail = "Cusomter@1234",
                            OrderDate = DateTime.Now,
                            ShipToAddress = new Address()
                            {
                                FirstName = "Hiso",
                                LastName = "Mahroooos",
                                Street = "street1",
                                City = "cairo",
                                State = "NY",
                                Zipcode = "1234"
                            },
                            DeliveryMethod = deliveryMethod_2,
                            OrderItems = new List<OrderItem>()
                            {
                                new OrderItem()
                                {
                                    ItemOrdered = new ProductItemOrdered()
                                    {
                                        ProductItemId = 3,
                                        ProductName = "Door"
                                    },
                                    Price = 100,
                                    Quantity = 3,
                                },

                                new OrderItem()
                                {
                                    ItemOrdered = new ProductItemOrdered()
                                    {
                                        ProductItemId = 1,
                                        ProductName = "Sofa"
                                    },
                                    Price = 40,
                                    Quantity = 3,
                                },

                                new OrderItem()
                                {
                                    ItemOrdered = new ProductItemOrdered()
                                    {
                                        ProductItemId = 7,
                                        ProductName = "Table"
                                    },
                                    Price = 200,
                                    Quantity = 1,
                                },

                                new OrderItem()
                                {
                                    ItemOrdered = new ProductItemOrdered()
                                    {
                                        ProductItemId = 5,
                                        ProductName = "Wooden Table"
                                    },
                                    Price = 900,
                                    Quantity = 2,
                                },
                            },
                            Subtotal = 6465,
                            Status = OrderStatus.Confirmed,
                            PaymentIntentId = "53454nskjdfb"
                        },

                        new Order()
                        {
                            BuyerEmail = "customer@furnitica.com",
                            OrderDate = DateTime.Now,
                            ShipToAddress = new Address()
                            {
                                FirstName = "Eslam",
                                LastName = "Mahroos",
                                Street = "street1",
                                City = "cairo",
                                State = "NY",
                                Zipcode = "1234"
                            },
                            DeliveryMethod = deliveryMethod_3,
                            OrderItems = new List<OrderItem>()
                            {
                                new OrderItem()
                                {
                                    ItemOrdered = new ProductItemOrdered()
                                    {
                                        ProductItemId = 3,
                                        ProductName = "Tray"
                                    },
                                    Price = 100,
                                    Quantity = 1,
                                },

                                new OrderItem()
                                {
                                    ItemOrdered = new ProductItemOrdered()
                                    {
                                        ProductItemId = 1,
                                        ProductName = "Dolap"
                                    },
                                    Price = 40,
                                    Quantity = 1,
                                },

                                new OrderItem()
                                {
                                    ItemOrdered = new ProductItemOrdered()
                                    {
                                        ProductItemId = 7,
                                        ProductName = "Table"
                                    },
                                    Price = 200,
                                    Quantity = 1,
                                },

                                new OrderItem()
                                {
                                    ItemOrdered = new ProductItemOrdered()
                                    {
                                        ProductItemId = 5,
                                        ProductName = "Vase"
                                    },
                                    Price = 900,
                                    Quantity = 1,
                                },
                            },
                            Subtotal = 8797,
                            Status = OrderStatus.Confirmed,
                            PaymentIntentId = "53454nskjdfb"
                        },

                        new Order()
                        {
                            BuyerEmail = "customer@furnitica.com",
                            OrderDate = DateTime.Now,
                            ShipToAddress = new Address()
                            {
                                FirstName = "Ahmed",
                                LastName = "Sayed",
                                Street = "street1",
                                City = "cairo",
                                State = "NY",
                                Zipcode = "1234"
                            },
                            DeliveryMethod = deliveryMethod_3,
                            OrderItems = new List<OrderItem>()
                            {
                                new OrderItem()
                                {
                                    ItemOrdered = new ProductItemOrdered()
                                    {
                                        ProductItemId = 3,
                                        ProductName = "Chair"
                                    },
                                    Price = 100,
                                    Quantity = 4,
                                },

                                new OrderItem()
                                {
                                    ItemOrdered = new ProductItemOrdered()
                                    {
                                        ProductItemId = 1,
                                        ProductName = "Door"
                                    },
                                    Price = 40,
                                    Quantity = 3,
                                },

                                new OrderItem()
                                {
                                    ItemOrdered = new ProductItemOrdered()
                                    {
                                        ProductItemId = 7,
                                        ProductName = "Table"
                                    },
                                    Price = 200,
                                    Quantity = 1,
                                },

                                new OrderItem()
                                {
                                    ItemOrdered = new ProductItemOrdered()
                                    {
                                        ProductItemId = 5,
                                        ProductName = "long table"
                                    },
                                    Price = 900,
                                    Quantity = 8,
                                },
                            },
                            Subtotal = 12456,
                            Status = OrderStatus.Pending,
                            PaymentIntentId = "53454nskjdfb"
                        },

                        new Order()
                        {
                            BuyerEmail = "customer@furnitica.com",
                            OrderDate = DateTime.Now,
                            ShipToAddress = new Address()
                            {
                                FirstName = "Ahmed",
                                LastName = "Ramadan",
                                Street = "street1",
                                City = "cairo",
                                State = "NY",
                                Zipcode = "1234"
                            },
                            DeliveryMethod = deliveryMethod_3,
                            OrderItems = new List<OrderItem>()
                            {
                                new OrderItem()
                                {
                                    ItemOrdered = new ProductItemOrdered()
                                    {
                                        ProductItemId = 3,
                                        ProductName = "Chair"
                                    },
                                    Price = 100,
                                    Quantity = 4,
                                },

                                new OrderItem()
                                {
                                    ItemOrdered = new ProductItemOrdered()
                                    {
                                        ProductItemId = 1,
                                        ProductName = "Sofa"
                                    },
                                    Price = 40,
                                    Quantity = 3,
                                },

                                new OrderItem()
                                {
                                    ItemOrdered = new ProductItemOrdered()
                                    {
                                        ProductItemId = 7,
                                        ProductName = "Counter"
                                    },
                                    Price = 200,
                                    Quantity = 1,
                                },

                                new OrderItem()
                                {
                                    ItemOrdered = new ProductItemOrdered()
                                    {
                                        ProductItemId = 5,
                                        ProductName = "stool"
                                    },
                                    Price = 900,
                                    Quantity = 8,
                                },
                            },
                            Subtotal = 543,
                            Status = OrderStatus.Pending,
                            PaymentIntentId = "53454nskjdfb"
                        },

                        new Order()
                        {
                            BuyerEmail = "customer@furnitica.com",
                            OrderDate = DateTime.Now,
                            ShipToAddress = new Address()
                            {
                                FirstName = "Mohamed",
                                LastName = "Fathy",
                                Street = "street1",
                                City = "cairo",
                                State = "NY",
                                Zipcode = "1234"
                            },
                            DeliveryMethod = deliveryMethod_3,
                            OrderItems = new List<OrderItem>()
                            {
                                new OrderItem()
                                {
                                    ItemOrdered = new ProductItemOrdered()
                                    {
                                        ProductItemId = 3,
                                        ProductName = "Wing"
                                    },
                                    Price = 100,
                                    Quantity = 4,
                                },

                                new OrderItem()
                                {
                                    ItemOrdered = new ProductItemOrdered()
                                    {
                                        ProductItemId = 1,
                                        ProductName = "Sofa"
                                    },
                                    Price = 40,
                                    Quantity = 3,
                                },

                                new OrderItem()
                                {
                                    ItemOrdered = new ProductItemOrdered()
                                    {
                                        ProductItemId = 7,
                                        ProductName = "Desk"
                                    },
                                    Price = 200,
                                    Quantity = 1,
                                },

                                new OrderItem()
                                {
                                    ItemOrdered = new ProductItemOrdered()
                                    {
                                        ProductItemId = 5,
                                        ProductName = "Molla"
                                    },
                                    Price = 900,
                                    Quantity = 8,
                                },
                            },
                            Subtotal = 122,
                            Status = OrderStatus.Confirmed,
                            PaymentIntentId = "53454nskjdfb"
                        }
                    };

                    context.Orders.AddRange(ordersData);

                    await context.SaveChangesAsync();
                }


                // Seed Reviews

                if (!context.Reviews.Any())
                {
                    var reviewsData = await
                        File.ReadAllTextAsync("../Infrastructure/Data/SeedData/reviews.json");

                    var reviews = JsonConvert.DeserializeObject<List<Review>>(reviewsData);

                    foreach (var review in reviews)
                    {
                        context.Reviews.Add(review);
                    }
                    await context.SaveChangesAsync();
                }

                

            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<SeedData>();

                logger.LogError(ex.Message);
            }
        }

    }
}
