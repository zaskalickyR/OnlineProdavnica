
export class CreateEditProduct {
  constructor(Name, Price, Stock, Address, Description, Image, Categories) {
    this.Name = Name;
    this.Price = Price;
    this.Stock = Stock;
    this.Address = Address;
    this.Description = Description;
    this.Image = Image;
    this.Categories = Categories;
    }
}



export default class ProductDataIn {
  constructor(category, categoryId, customerId, description, id, images, name, lastUpdateTime, isDeleted, price, stock) {
    this.category = category;
    this.categoryId = categoryId;
    this.customerId = customerId;
    this.description = description;
    this.id = id;
    this.images = images;
    this.name = name;
    this.lastUpdateTime = lastUpdateTime;
    this.isDeleted = isDeleted;
    this.price = price;
    this.stock = stock;
  }
}

//
export class Product{
  name = '';
  id = 0;
  price = 0;
  description = '';
  stock = 0;
  categoryName = '';
  categoryId = 0;
  images = '';
  count = 0;
  salerName = '';
  salerId = '';
}