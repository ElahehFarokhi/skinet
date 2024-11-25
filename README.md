# E-Commerce Application

Welcome to the E-Commerce Application! This project is a fully functional e-commerce platform designed to demonstrate modern software development techniques and provide a seamless shopping experience for users.

---

## Features

### General Features
- User registration and login system.
- Role-based authorization for buyers and admins.
- Dynamic shopping cart functionality.
- Seamless checkout process with **Stripe** payment integration.
- Refund management through the admin panel.
- Order history accessible by both buyers and admins with role-specific views.

### Admin Panel Features
- Manage orders with refund capabilities using **Stripe**.
- View and process all orders with advanced accessibility controls.

### Technologies Used

#### Backend
- **Platform:** .NET 8
- **Database Access:** Entity Framework Core (EF Core)
- **Caching:** Redis
- **Design Patterns:**
  - Generic Repository
  - Unit of Work
  - Specification Pattern
- **Real-Time Communication:** SignalR
- **Payment Integration:** Stripe
- **Exception Handling:** Robust centralized error management
- **Caching:** Optimized for performance using Redis
- **Role-Based Authorization:** Secure user access levels
- **Database:** SQL Server

#### Frontend
- **Platform:** Angular 18
- **UI Framework:** Tailwind CSS
- **Material Design:** Angular Material
- **Error Handling:** Centralized and user-friendly error notifications
- **Real-Time Updates:** SignalR integration
- **Payment Integration:** Stripe
- **Performance Enhancements:**
  - HTTP Interceptors
  - Lazy Loading Modules

---

## Application Flow

### Buyer Flow
1. **Registration & Login:** Users can register and log in to access the app.
2. **Product Selection:** Browse and select products.
3. **Shopping Cart:** Add selected products to the cart.
4. **Checkout:** Complete the purchase via Stripe.
5. **Order History:** View past orders with detailed information.

### Admin Flow
1. **Login:** Admins access a secured admin panel.
2. **Order Management:** View all orders with full details.
3. **Refund Processing:** Manage refunds seamlessly through Stripe.

---

## Installation & Setup

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [Node.js](https://nodejs.org/) and npm
- [Redis](https://redis.io/download)
- Angular CLI
- [SQL Server](https://www.microsoft.com/sql-server)

### Backend Setup
1. Clone the repository.
2. Navigate to the backend folder: `cd backend`
3. Restore dependencies: `dotnet restore`
4. Update the appsettings.json with your Stripe, Redis, and SQL Server configurations.
5. Run the application:
   ```bash
   dotnet run
   ```

### Frontend Setup
1. Navigate to the frontend folder: `cd frontend`
2. Install dependencies: `npm install`
3. Update environment files with Stripe configurations.
4. Start the development server:
   ```bash
   ng serve
   ```

---

## Deployment
Follow your preferred deployment strategy for .NET and Angular applications. Ensure Redis and SQL Server are configured correctly and Stripe keys are securely stored in environment variables.

---

## Acknowledgments
This project is inspired by the Udemy course: [Learn to Build an E-Commerce App with .NET Core and Angular](https://www.udemy.com/course/learn-to-build-an-e-commerce-app-with-net-core-and-angular/?couponCode=BFCPSALE24) by Neil Cummings.

---

## Future Enhancements
- Add product reviews and ratings.
- Add Inventory system
- Add Email service
- Implement advanced analytics in the admin panel.
- Add multi-language support.

---

## License
This project is licensed under the MIT License. See the LICENSE file for more details.

---

## Contact
Feel free to contact me if you're interested in collaborating or discussing job opportunities!

