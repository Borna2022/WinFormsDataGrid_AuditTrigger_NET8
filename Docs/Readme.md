# WinFormsApp2 – Windows Forms + EF Core 8

A simple Windows Forms application built with .NET 8 and Entity Framework Core 8.  
It connects to a SQL Server database and displays data from the `TMixedZoneSteelGroup` table using a DataGridView.

---

## 🔧 Setup Instructions

1. Create a SQL Server database named `Test`.
2. Update the connection string in `MyDbContext.cs` if needed.
3. Open **Package Manager Console** and run:

This will apply EF Core migrations and create the required tables.
4. Build and run the project. Data will appear in the form.

---

## 📁 Project Structure

- `Models/` – Entity classes (e.g., `MixedZoneSteelGroup`)
- `Data/` – EF Core DbContext (`MyDbContext.cs`)
- `Migrations/` – Auto-generated EF Core migration files
- `Form1.cs` – Main form with DataGridView
- `database/` – Optional raw SQL scripts
- `docs/README.md` – Project documentation

---

## 🛠 Technologies

- .NET 8
- Windows Forms
- Entity Framework Core 8
- SQL Server
- GitHub

---

## 👤 Author

Developed by Borna – combining 20 years of steelmaking expertise with modern software development.
