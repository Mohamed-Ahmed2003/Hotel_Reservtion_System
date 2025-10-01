# 🏨 Hotel Management System (Backend)

This is a **Hotel Management System Backend** built with Node.js and Express.  
It provides APIs for managing **branches, rooms, employees, bookings, invoices, and payments**.  

The system supports **role-based access** for:
- **Admin** → full system control
- **Manager** → branch-level management
- **Receptionist** → booking & check-in/out handling

---

## 🚀 Features
- 🔑 User authentication & role-based access (Admin, Manager, Receptionist)
- 🏢 Branch management (multi-branch support)
- 🛏 Room management (Single, Double, Suite, availability, pricing)
- 📅 Booking system (check-in, check-out, status tracking)
- 💳 Invoices & Payments (tax, discount, final amount)
- 📢 Notifications (booking confirmation, reminders)

---

## 📦 Installation

### 1️⃣ Clone the repository
```bash
git clone https://github.com/your-username/hotel-management-system.git
cd hotel-management-system
```

###2️⃣ Install dependencies
npm install

### 3️⃣ Configure environment

Open appsettings.json (or .env if you prefer environment variables).

Update your database connection string:

{
  "Database": {
    "provider": "mssql", 
    "connectionString": "Server=localhost;Database=HotelDB;User Id=sa;Password=your_password;"
  },
  "JWT": {
    "SecretKey": "your_secret_key"
  }
}


4️⃣ Run database migrations (if using Sequelize/TypeORM/Knex)

npm run migrate


