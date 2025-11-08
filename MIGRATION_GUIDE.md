# Database Migration Guide - SQL Server to MySQL/MariaDB

## ✅ Changes Completed

### 1. **App.config** - Updated connection string
   - Changed from: `System.Data.SqlClient` (SQL Server)
   - Changed to: `MySql.Data.MySqlClient` (MySQL/MariaDB)
   - Connection: `Server=localhost;Database=CMS_25_A;Uid=root;Pwd=;`

### 2. **Settings.settings** - Updated provider
   - Changed provider from `System.Data.SqlClient` to `MySql.Data.MySqlClient`

### 3. **CustomerForm.cs** - Fixed Critical Issues
   - ✅ Removed hardcoded SQL Server connection string
   - ✅ Fixed SQL injection vulnerability with parameterized queries
   - ✅ Changed to use MySQL connection from configuration
   - ✅ Added proper `using` statement for connection disposal

### 4. **CMS_25_ADataSet.xsd** - Updated data provider
   - Changed provider from `System.Data.SqlClient` to `MySql.Data.MySqlClient`

### 5. **Created MySQL Database Script**
   - File: `create_mysql_database.sql`
   - Ready to execute in HeidiSQL

---

## 🔧 Installation Steps

### Step 1: Install MySQL Connector for .NET

You need to install the MySQL.Data NuGet package in your project:

**Option A: Using NuGet Package Manager Console (in Visual Studio)**
```
Install-Package MySql.Data -Version 8.0.33
```

**Option B: Using .NET CLI**
```bash
dotnet add package MySql.Data --version 8.0.33
```

**Option C: Manually add to .csproj**
Add this to your `CMS_25_A.csproj` file inside `<ItemGroup>`:
```xml
<PackageReference Include="MySql.Data" Version="8.0.33" />
```

### Step 2: Create Database in HeidiSQL

1. Open **HeidiSQL**
2. Connect to your local MySQL/MariaDB server
3. Open the file: `create_mysql_database.sql`
4. Click **Execute** (F9) to run the script
5. Verify tables are created successfully

### Step 3: Update Connection String (if needed)

The default connection in `App.config` is:
```
Server=localhost;Database=CMS_25_A;Uid=root;Pwd=;
```

**If your MySQL setup is different, update:**
- `Uid=root` → Your MySQL username
- `Pwd=` → Your MySQL password (currently empty)
- `Server=localhost` → Your MySQL server address

---

## 📋 Database Schema Created

### Tables:
1. **tblCustomer** - Primary Key: CarNo
2. **tblWorker** - Primary Key: WorkerID  
3. **tblJobDetails** - Primary Key: JobID (AUTO_INCREMENT), with foreign keys to Customer and Worker

---

## ⚠️ Important Notes

1. **You must install MySQL.Data NuGet package** - The application won't compile without it
2. **Update the password** in `App.config` if your MySQL root has a password
3. **Test the connection** after setup before running the full application
4. The SQL injection vulnerability in `CustomerForm.cs` has been fixed with parameterized queries

---

## 🧪 Testing the Migration

After completing the installation:
1. Build the solution in Visual Studio
2. Run the application
3. Try adding a customer record
4. Check if data appears in HeidiSQL

---

## 🔍 Troubleshooting

**Error: "Could not load file or assembly MySql.Data"**
- Solution: Install the MySql.Data NuGet package

**Error: "Unable to connect to any of the specified MySQL hosts"**
- Solution: Check if MySQL/MariaDB service is running
- Verify connection string in App.config

**Error: "Access denied for user 'root'@'localhost'"**
- Solution: Update the password in App.config connection string
